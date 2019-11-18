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

        // шагов пробирки от точки сканирования до точки с забором 
        private const int cellsFromScanPlaceToSuctionPlace = 7;
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
                Logger.DemoInfo("Прошла минута");
                
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
               
            initializationTask();
            needleWashingTask();

            //Закрываем клапана
            Core.Pomp.CloseValves();

            Logger.DemoInfo($"Запущена подготовка перед сканированием пробирок");

            Core.Needle.HomeAll();
            Core.Transporter.PrepareBeforeScanning();

            Logger.DemoInfo($"Подготовка завершена");

            currentCell = 0;

            while (haveOutstandingTasks()) // пока есть задачи
            {
                if (currentCell == numberTubeCells) // прошли полный круг
                {
                    currentCell = 0;
                }

                if (tubeSearchTask(3) == false)
                {
                    Logger.DemoInfo($"Пробирка со штрихкодом [{cells[currentCell].BarCode}] не найдена в списке анализов!");
                }

                int cellUnderNeedle = currentCell - cellsFromScanPlaceToSuctionPlace;

                if (currentCell >= cellsFromScanPlaceToSuctionPlace && 
                    cells[cellUnderNeedle].HaveTube == true) // первая пробирка уже под иглой
                {
                    TubeInfo tube = searchBarcodeInDatabase(cells[cellUnderNeedle].BarCode);
                    
                    if(null != tube)
                    {
                        lock(locker)
                        {
                            tube.IsFind = true;
                        }

                        Logger.DemoInfo($"Пробирка со штрихкодом [{tube.BarCode}] дошла до точки забора!");
                        Logger.DemoInfo($"Пробирка со штрихкодом [{tube.BarCode}] запущена в обработку!");
                        // постановка на выполнение задач и забор из пробирки в белую кювету

                        tube.CurrentStage = 0;
                        performingPreparatoryTask(tube);

                        lock (locker)
                        {
                            tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                        }
                    }
                }

                performingOutstandingTasks();
                currentCell++;
            }

            Logger.DemoInfo($"Все пробирки обработаны!");
        }

        /// <summary>
        /// Выполнение запланированных задач
        /// </summary>
        private void performingOutstandingTasks()
        {
            foreach (TubeInfo tube in Properties.Tubes)
            {
                if (tube.IsFind && tube.TimeToStageComplete == 0)
                {
                    tube.CurrentStage++;
                    

                    if (tube.CurrentStage < tube.Stages.Count)
                    {
                        tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                        Logger.DemoInfo($"Завершена инкубация для пробирки [{tube.BarCode}]!");

                        performingIntermediateTask(tube);
                    }
                    else
                    {
                        Logger.DemoInfo($"Завершены все стадии анализа для пробирки [{tube.BarCode}]!");

                        performingFinishTask(tube);
                    }
                }
            }
        }

        /// <summary>
        /// Поиск пробирки с заданным штрихкодом
        /// </summary>
        /// <param name="barCode">Искомый штрихкод</param>
        /// <returns>Пробирка со штрихкодом или null</returns>
        private TubeInfo searchBarcodeInDatabase(string barCode)
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
        private bool haveOutstandingTasks()
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
        private void initializationTask()
        {
            Logger.DemoInfo($"Запущена инициализация всех устройств");

            Core.Needle.HomeAll();
            Core.Charger.HomeHook();
            Core.Charger.HomeRotator();
            Core.Rotor.Home();
            Core.Pomp.Home();

            Logger.DemoInfo($"Инициализация завершена");
        }

        /// <summary>
        /// Промывка иглы
        /// </summary>
        private void needleWashingTask()
        {
            Logger.DemoInfo($"Запущена промывка иглы");

            Core.Needle.HomeAll();
            Core.Needle.TurnAndGoDownToWashing();
            Core.Pomp.Washing(2);
            Core.Pomp.Home();

            Logger.DemoInfo($"Промывка иглы завершена.");
        }
        
        /// <summary>
        /// Поиск пробирки в конвейере (поиск штрихкода)
        /// </summary>
        /// <param name="numberAttempts">Количество попыток (при неудаче)</param>
        /// <returns>Успешность выполнения</returns>
        private bool tubeSearchTask(int numberAttempts)
        {
            int attempt = 0;
            bool result = false;

            Logger.DemoInfo($"Запущен поиск пробирки (сканирование)");
            //Закрываем клапана
            Core.Pomp.CloseValves();
            Core.Transporter.Shift(false);

            while (attempt < numberAttempts)
            {
                Core.Transporter.RotateAndScanTube();
                cells[currentCell] = new TubeCell();

                string barCode = Core.GetLastABarCode();

                if (barCode != null)
                {
                    cells[currentCell].HaveTube = true;
                    cells[currentCell].BarCode = barCode;
                    Logger.DemoInfo($"В ячейке под номером {currentCell} найдена пробирка!");

                    TubeInfo tube = searchBarcodeInDatabase(cells[currentCell].BarCode);

                    if(tube != null)
                    {
                        lock(locker)
                        {
                            tube.IsFind = true;
                        }
                        Logger.DemoInfo($"Штрихкод [{tube.BarCode}] найден  списке задач!");
                        result = true;
                        break;
                    } 
                }
                attempt++;
            }

            Logger.DemoInfo($"Поиск пробирки (сканирование) завершен.");

            return result;
        }

        /// <summary>
        /// Выполнение завершающей задачи 
        /// (перенос материала из белой кюветы в прозрачную кювету и снятие показаний с датчика)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void performingFinishTask(TubeInfo tube)
        {

        }

        /// <summary>
        /// Выполнение промежуточной задачи (добавление реагентов из ячейки в белую кювету)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void performingIntermediateTask(TubeInfo tube)
        {
            Logger.ControllerInfo($"Пробирка [{tube.BarCode}] - запуск выполнения {tube.CurrentStage}-й стадии.");

            Core.Needle.HomeAll();
            needleWashingTask(); // Промывка иглы

            // Начало выполнения подготовительного этапа

            // Подводим нужную ячейку картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                tube.Stages[tube.CurrentStage].Cell,
                RotorController.CellPosition.CellLeft);

            // Устанавливаем иглу над нужной ячейкой картриджа
            Core.Needle.TurnToCartridge(
                NeedleController.FromPosition.Washing,
                tube.Stages[tube.CurrentStage].Cell);

            // Прокалываем ячейку картриджа
            Core.Needle.GoDownAndBrokeCartridge();

            // Забираем реагент из ячейки картриджа
            Core.Pomp.Suction(0);

            // Устанавливаем иглу над белой ячейкой картриджа
            Core.Needle.TurnToCartridge(
                NeedleController.FromPosition.FirstCell,
                CartridgeCell.WhiteCell);

            // Подводим белую кювету картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                CartridgeCell.WhiteCell,
                RotorController.CellPosition.CellRight);

            // Опускаем иглу в кювету
            Core.Needle.GoDownAndBrokeCartridge();

            // Сливаем реагент в белую кювету
            Core.Pomp.Unsuction(0);

            // Конец выполнения подготовительного этапа

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - завершено выполнение {tube.CurrentStage}-й стадии.");
        }

        /// <summary>
        /// Выполнение подготовительной задачи (включает забор материала из пробирки)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void performingPreparatoryTask(TubeInfo tube)
        {
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - запущено выполнение подготовительной (0-й) стадии.");

            Logger.DemoInfo($"Подготовка к забору материала из пробирки.");
            // Смещаем пробирку, чтобы она оказалась под иглой
            Core.Transporter.Shift(false, TransporterController.ShiftType.HalfTube);
            
            // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            Core.Needle.TurnToTubeAndWaitTouch();

            Logger.DemoInfo($"Забор материала из пробирки.");
            // Набираем материал из пробирки
            Core.Pomp.Suction(0);
            
            // Подводим белую кювету картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.PlaceCellUnderNeedle( 
                tube.Stages[0].CartridgePosition,
                CartridgeCell.WhiteCell,
                RotorController.CellPosition.CenterCell);

            // Устанавливаем иглу над белой ячейкой картриджа
            Core.Needle.TurnToCartridge(
                NeedleController.FromPosition.Tube,
                CartridgeCell.WhiteCell);

            // Опускаем иглу в кювету
            Core.Needle.GoDownAndBrokeCartridge();

            Logger.DemoInfo($"Слив забранного материала в белую кювету.");
            // Сливаем материал в белую кювету
            Core.Pomp.Unsuction(0); // слив из иглы в картридж

            Core.Needle.HomeAll();
            
            // Промываем иглу
            needleWashingTask();

            Logger.DemoInfo($"Перенос реагента в белую кювету.");
            // Выполняем перенос реагента из нужной ячейки картриджа в белую кювету
            performingIntermediateTask(tube);
            
            // Устанавливаем иглу в домашнюю позицию
            Core.Needle.HomeAll();

            // Смещаем пробирку обратно
            Core.Transporter.Shift(true, TransporterController.ShiftType.HalfTube);

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - завершено выполнение подготовительной (0-й) стадии.");
        }
    }
}
