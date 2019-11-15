using SteppersControlCore.Controllers;
using SteppersControlCore.Elements;
using SteppersControlCore.Interfaces;
using SteppersControlCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace SteppersControlCore
{
    [Serializable]
    public class DemoControllerProperties
    {
        public List<TubeInfo> Tubes { get; set; } = new List<TubeInfo>();

        public DemoControllerProperties()
        {

        }
    }

    public class DemoController
    {
        public DemoControllerProperties Properties;

        private const string filename = "Tubes";

        private const int numberTubeCells = 54;
        private TubeCell[] cells;
        private int currentCell = 0;

        private Stopwatch stopWatch = new Stopwatch();
        Timer timer = new Timer();

        private static object locker = new object();

        public DemoController()
        {
            Properties = new DemoControllerProperties();

            cells = new TubeCell[numberTubeCells];

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<DemoControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename));
        }

        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<DemoControllerProperties>.ReadXML(
                Path.Combine(path, filename));

            if (Properties == null)
                Properties = new DemoControllerProperties();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(stopWatch.ElapsedMilliseconds >= 60000)
            {
                stopWatch.Restart();
                Logger.ControllerInfo("Прошла минута");
                
                // уменьшаем оставшееся время инкубации
                foreach (TubeInfo tube in Properties.Tubes)
                {
                    lock (locker)
                    {
                        if(tube.IsFind && tube.TimeToStageComplete != 0)
                            tube.TimeToStageComplete -= 1;
                    }
                }
            }
        }
        
        public void StartDemo()
        {
            timer.Start();
            stopWatch.Reset();
            stopWatch.Start();

            Core.Executor.StartTask(() =>
            {
                DemoTask();
            });
        }

        public void AbortExecution()
        {
            Logger.DemoInfo("Работа демо режима прервана.");
            if(timer.Enabled)
                timer.Stop();
            if(stopWatch.IsRunning)
                stopWatch.Stop();
        }

        private void DemoTask()
        {
            foreach (TubeInfo tube in Properties.Tubes)
            {
                lock(locker)
                {
                    tube.IsFind = false;
                    tube.CurrentStage = -1;
                    tube.TimeToStageComplete = 0;
                }
            }
               
            initTask();
            washingNeedleTask();

            //Закрываем клапана
            Core.Pomp.CloseValves();

            Logger.ControllerInfo($"Запущена подготовка перед сканированием пробирок");

            Core.Arm.Home();
            Core.Transporter.PrepareBeforeScanning();

            Logger.ControllerInfo($"Подготовка завершена");

            currentCell = 0;

            while (haveTasks()) // пока есть задачи
            {
                if (currentCell == numberTubeCells) // прошли полный круг
                {
                    currentCell = 0;
                }

                if (findTubeTask(3) == false)
                {
                    Logger.ControllerInfo($"Пробирка со штрихкодом [{cells[currentCell].BarCode}] не найдена в списке задач!");
                }
                
                if(currentCell >= 7 && cells[currentCell - 7].HaveTube == true) // первая пробирка уже под иглой
                {
                    TubeInfo tube = findBarCode(cells[currentCell - 7].BarCode);
                    
                    if(null != tube)
                    {
                        lock(locker)
                        {
                            tube.IsFind = true;
                        }

                        Logger.ControllerInfo($"Пробирка со штрихкодом [{tube.BarCode}] запущена в обработку!");
                        // постановка на выполнение задач и забор из пробирки в белую кювету

                        tube.CurrentStage = 0;
                        PerformFirstStageTask(tube);

                        lock (locker)
                        {
                            tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                        }
                    }
                }

                performTasks();
                currentCell++;
            }

            Logger.ControllerInfo($"Все пробирки обработаны!");
        }

        /// <summary>
        /// Выполнение запланированных задач
        /// </summary>
        private void performTasks()
        {
            foreach (TubeInfo tube in Properties.Tubes)
            {
                if (tube.IsFind && tube.TimeToStageComplete == 0)
                {
                    tube.CurrentStage++;
                    

                    if (tube.CurrentStage < tube.Stages.Count)
                    {
                        tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                        Logger.ControllerInfo($"Завершена инкубация для пробирки [{tube.BarCode}]!");

                        performMiddleTask(tube);
                    }
                    else
                    {
                        Logger.ControllerInfo($"Завершены все стадии анализа для пробирки [{tube.BarCode}]!");

                        performFinishTask(tube);
                    }
                }
            }
        }

        /// <summary>
        /// Поиск пробирки с заданным штрихкодом
        /// </summary>
        /// <param name="barCode">Искомый штрихкод</param>
        /// <returns>Пробирка со штрихкодом или null</returns>
        private TubeInfo findBarCode(string barCode)
        {
            foreach (TubeInfo tube in Properties.Tubes)
            {
                if (barCode.Contains(tube.BarCode))
                    return tube;
            }
            return null;
        }

        /// <summary>
        /// Проверка наличия невыполненных задач
        /// </summary>
        /// <returns>Наличие невыполненных задач</returns>
        private bool haveTasks()
        {
            foreach(TubeInfo tube in Properties.Tubes)
            {
                if (tube.CurrentStage <= tube.Stages.Count)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void initTask()
        {
            Logger.ControllerInfo($"Запущена инициализация всех устройств");

            Core.Arm.Home();
            Core.Loader.HomeShuttle();
            Core.Loader.HomeLoad();
            Core.Rotor.Home();
            Core.Pomp.Home();

            Logger.ControllerInfo($"Инициализация завершена");
        }

        /// <summary>
        /// Промывка иглы
        /// </summary>
        private void washingNeedleTask()
        {
            Logger.ControllerInfo($"Запущена промывка иглы");

            Core.Arm.Home();
            Core.Arm.MoveOnWashing();
            Core.Pomp.Washing(2);
            Core.Pomp.Home();

            Logger.ControllerInfo($"Промывка иглы завершена.");
        }
        
        /// <summary>
        /// Поиск пробирки в конвейере (поиск штрихкода)
        /// </summary>
        /// <param name="countRepeats">Число повторений поиска</param>
        /// <returns>Успешность выполнения</returns>
        private bool findTubeTask(int countRepeats)
        {
            int numberRepeat = 0;
            bool result = false;

            Logger.ControllerInfo($"Запущен поиск пробирки (сканирование)");
            //Закрываем клапана
            Core.Pomp.CloseValves();
            Core.Transporter.Shift(false);

            while (numberRepeat < countRepeats)
            {
                Core.Transporter.TurnAndScanTube();
                cells[currentCell] = new TubeCell();

                String barCode = Core.GetLastABarCode();

                if (barCode != null)
                {
                    cells[currentCell].HaveTube = true;
                    cells[currentCell].BarCode = barCode;
                    Logger.ControllerInfo($"В ячейке под номером {currentCell} найдена пробирка!");

                    TubeInfo tube = findBarCode(cells[currentCell].BarCode);

                    if(tube != null)
                    {
                        lock(locker)
                        {
                            tube.IsFind = true;
                        }
                        Logger.ControllerInfo($"Штрихкод [{tube.BarCode}] найден  списке задач!");
                        result = true;
                        break;
                    } 
                }
                numberRepeat++;
            }

            Logger.ControllerInfo($"Поиск пробирки (сканирование) завершен.");

            return result;
        }

        /// <summary>
        /// Выполнение завершающей задачи 
        /// (перенос материала из белой кюветы в прозрачную кювету и снятие показаний с датчика)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void performFinishTask(TubeInfo tube)
        {

        }

        /// <summary>
        /// Выполнение промежуточной задачи (добавление реагентов из ячейки в белую кювету)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void performMiddleTask(TubeInfo tube)
        {
            Logger.ControllerInfo($"Пробирка [{tube.BarCode}] - запущено выполнение {tube.CurrentStage}-я стадии.");

            Core.Arm.Home();
            washingNeedleTask(); // Промывка иглы

            // Начало выполнения подготовительного этапа

            // Подводим нужную ячейку картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.MoveCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                tube.Stages[tube.CurrentStage].Cell,
                RotorController.CellPosition.CellLeft);

            // Устанавливаем иглу над нужной ячейкой картриджа
            Core.Arm.MoveToCartridge(
                ArmController.FromPosition.Washing,
                tube.Stages[tube.CurrentStage].Cell);

            // Прокалываем ячейку картриджа
            Core.Arm.BrokeCartridge();

            // Забираем реагент из ячейки картриджа
            Core.Pomp.Suction(0);

            // Устанавливаем иглу над белой ячейкой картриджа
            Core.Arm.MoveToCartridge(
                ArmController.FromPosition.FirstCell,
                CartridgeCell.WhiteCell);

            // Подводим белую кювету картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.MoveCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                CartridgeCell.WhiteCell,
                RotorController.CellPosition.CellRight);

            // Опускаем иглу в кювету
            Core.Arm.BrokeCartridge();

            // Сливаем реагент в белую кювету
            Core.Pomp.Unsuction(0);

            // Конец выполнения подготовительного этапа

            Logger.ControllerInfo($"Пробирка [{tube.BarCode}] - завершено выполнение {tube.CurrentStage}-я стадии.");
        }

        /// <summary>
        /// Выполнение первой стадии (включает забор материала из пробирки)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void PerformFirstStageTask(TubeInfo tube)
        {
            Logger.ControllerInfo($"Пробирка [{tube.BarCode}] - запущено выполнение подготовительной (0-й) стадии.");

            // Смещаем пробирку, чтобы она оказалась под иглой
            Core.Transporter.Shift(false, TransporterController.ShiftType.HalfTube);
            
            // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            Core.Arm.MoveOnTube();
            
            // Набираем материал из пробирки
            Core.Pomp.Suction(0);
            
            // Подводим белую кювету картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.MoveCellUnderNeedle( 
                tube.Stages[0].CartridgePosition,
                CartridgeCell.WhiteCell,
                RotorController.CellPosition.CenterCell);

            // Устанавливаем иглу над белой ячейкой картриджа
            Core.Arm.MoveToCartridge(
                ArmController.FromPosition.Tube,
                CartridgeCell.WhiteCell);

            // Опускаем иглу в кювету
            Core.Arm.BrokeCartridge();

            // Сливаем материал в белую кювету
            Core.Pomp.Unsuction(0); // слив из иглы в картридж

            Core.Arm.Home();
            
            // Промываем иглу
            washingNeedleTask();

            // Выполняем перенос реагента из нужной ячейки картриджа в белую кювету
            performMiddleTask(tube);
            
            // Устанавливаем иглу в домашнюю позицию
            Core.Arm.Home();

            // Смещаем пробирку обратно
            Core.Transporter.Shift(true, TransporterController.ShiftType.HalfTube);

            Logger.ControllerInfo($"Пробирка [{tube.BarCode}] - завершено выполнение подготовительной (0-й) стадии.");
        }
    }
}
