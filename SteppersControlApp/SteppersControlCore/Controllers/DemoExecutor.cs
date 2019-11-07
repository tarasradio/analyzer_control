using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.Elements;
using SteppersControlCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SteppersControlCore.Controllers
{
    public class DemoExecutor
    {
        public List<TubeInfo> Tubes { get; set; }

        private const string filename = "Tubes";

        private const int numberTubeCells = 54;
        private TubeCell[] cells;
        private int currentCell = 0;

        private Stopwatch stopWatch = new Stopwatch();
        Timer timer = new Timer();

        private static object _syncRoot = new object();

        public DemoExecutor()
        {
            Tubes = new List<TubeInfo>();
            cells = new TubeCell[numberTubeCells];

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(stopWatch.ElapsedMilliseconds >= 60000)
            {
                stopWatch.Restart();
                Logger.AddMessage("Прошла минута");
                
                // уменьшаем оставшееся время инкубации
                foreach (TubeInfo tube in Tubes)
                {
                    lock (_syncRoot)
                    {
                        if(tube.IsFind && tube.TimeToStageComplete != 0)
                            tube.TimeToStageComplete -= 1;
                    }
                }
            }
        }

        public void WriteXml()
        {
            XMLSerializeHelper<List<TubeInfo>>.WriteXml(Tubes, filename);
        }
        
        public void ReadXml()
        {
            Tubes = XMLSerializeHelper<List<TubeInfo>>.ReadXML(filename);
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

        private void DemoTask()
        {
            foreach (TubeInfo tube in Tubes)
            {
                lock(_syncRoot)
                {
                    tube.IsFind = false;
                    tube.CurrentStage = -1;
                    tube.TimeToStageComplete = 0;
                }
            }
               
            initTask();
            washingNeedleTask();

            //Закрываем клапана
            WaitExecution(Core.Pomp.CloseValves());

            Logger.AddMessage($"Prepare before scanning task [Start]");
            WaitExecution(Core.Arm.Home());
            WaitExecution(Core.Transporter.PrepareBeforeScanning());

            Logger.AddMessage($"Prepare before scanning task [Finish]");

            currentCell = 0;

            while (haveTasks()) // пока есть задачи
            {
                if (currentCell == numberTubeCells) // прошли полный круг
                {
                    currentCell = 0;
                }

                if (findTubeTask(3) == false)
                {
                    Logger.AddMessage($"Пробирка со штрихкодом [{cells[currentCell].BarCode}] не найдена в списке задач!");
                }
                
                if(currentCell >= 7 && cells[currentCell - 7].HaveTube == true) // первая пробирка уже под иглой
                {
                    TubeInfo tube = findBarCode(cells[currentCell - 7].BarCode);
                    
                    if(null != tube)
                    {
                        lock(_syncRoot)
                        {
                            tube.IsFind = true;
                        }
                        
                        Logger.AddMessage($"Пробирка со штрихкодом {tube.BarCode} запущена в обработку!");
                        // постановка на выполнение задач и забор из пробирки в белую кювету

                        tube.CurrentStage = 0;
                        PerformFirstStageTask(tube);

                        lock (_syncRoot)
                        {
                            tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                        }
                    }
                }

                performTasks();
                currentCell++;
            }
            
            Logger.AddMessage($"Все пробирки обработаны!");
        }

        /// <summary>
        /// Выполнение запланированных задач
        /// </summary>
        private void performTasks()
        {
            foreach (TubeInfo tube in Tubes)
            {
                if (tube.IsFind && tube.TimeToStageComplete == 0)
                {
                    tube.CurrentStage++;
                    

                    if (tube.CurrentStage < tube.Stages.Count)
                    {
                        tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                        Logger.AddMessage($"Завершена инкубация для пробирки [{tube.BarCode}]!");

                        performMiddleTask(tube);
                    }
                    else
                    {
                        Logger.AddMessage($"Завершены все стадии анализа для пробирки [{tube.BarCode}]!");

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
            foreach (TubeInfo tube in Tubes)
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
            foreach(TubeInfo tube in Tubes)
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
            Logger.AddMessage($"Init task [Start]");
            WaitExecution( Core.Arm.Home() );
            WaitExecution( Core.Loader.HomeShuttle() );
            WaitExecution( Core.Loader.HomeLoad() );
            WaitExecution( Core.Rotor.Home() );
            WaitExecution( Core.Pomp.Home() );
            Logger.AddMessage($"Init task [Finish]");
        }

        /// <summary>
        /// Промывка иглы
        /// </summary>
        private void washingNeedleTask()
        {
            Logger.AddMessage($"Washing needle task [Start]");
            WaitExecution( Core.Arm.Home() );
            WaitExecution( Core.Arm.MoveOnWashing() );
            WaitExecution( Core.Pomp.Washing(2) );
            WaitExecution( Core.Pomp.Home() );
            Logger.AddMessage($"Washing needle task [Finish]");
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

            Logger.AddMessage($"Scan tube task [Start]");
            //Закрываем клапана
            WaitExecution(Core.Pomp.CloseValves());
            WaitExecution(Core.Transporter.Shift(false));

            while (numberRepeat < countRepeats)
            {
                WaitExecution(Core.Transporter.TurnAndScanTube());
                cells[currentCell] = new TubeCell();

                String barCode = Core.GetLastBarCode();

                if (barCode != null)
                {
                    cells[currentCell].HaveTube = true;
                    cells[currentCell].BarCode = barCode;
                    Logger.AddMessage($"В ячейке под номером {currentCell} найдена пробирка!");

                    TubeInfo tube = findBarCode(cells[currentCell].BarCode);

                    if(tube != null)
                    {
                        lock(_syncRoot)
                        {
                            tube.IsFind = true;
                        }
                        Logger.AddMessage($"Штрихкод {tube.BarCode} найден  списке задач!");
                        result = true;
                        break;
                    } 
                }
                numberRepeat++;
            }
            
            Logger.AddMessage($"Scan tube task [Finish]");

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
            Logger.AddMessage($"Пробирка [{tube.BarCode}] - {tube.CurrentStage}-я стадия [Start]");

            WaitExecution(Core.Arm.Home());
            washingNeedleTask(); // Промывка иглы

            // Начало выполнения подготовительного этапа

            // Подводим нужную ячейку картриджа под иглу
            WaitExecution(Core.Rotor.Home());
            WaitExecution(Core.Rotor.MoveCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                tube.Stages[tube.CurrentStage].Cell,
                RotorController.CellPosition.CellLeft));

            // Устанавливаем иглу над нужной ячейкой картриджа
            WaitExecution(Core.Arm.MoveToCartridge(
                ArmController.FromPosition.Washing,
                tube.Stages[tube.CurrentStage].Cell));

            // Прокалываем ячейку картриджа
            WaitExecution(Core.Arm.BrokeCartridge());

            // Забираем реагент из ячейки картриджа
            WaitExecution(Core.Pomp.Suction(0));

            // Устанавливаем иглу над белой ячейкой картриджа
            WaitExecution(Core.Arm.MoveToCartridge(
                ArmController.FromPosition.FirstCell,
                CartridgeCell.WhiteCell));

            // Подводим белую кювету картриджа под иглу
            WaitExecution(Core.Rotor.Home());
            WaitExecution(Core.Rotor.MoveCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                CartridgeCell.WhiteCell,
                RotorController.CellPosition.CellRight));

            // Опускаем иглу в кювету
            WaitExecution(Core.Arm.BrokeCartridge());

            // Сливаем реагент в белую кювету
            WaitExecution(Core.Pomp.Unsuction(0));

            // Конец выполнения подготовительного этапа

            Logger.AddMessage($"Пробирка [{tube.BarCode}] - {tube.CurrentStage}-я стадия [Stop]");
        }

        /// <summary>
        /// Выполнение первой стадии (включает забор материала из пробирки)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void PerformFirstStageTask(TubeInfo tube)
        {
            Logger.AddMessage($"Пробирка [{tube.BarCode}] - подготовительная (0-я) стадия [Start]");

            // Смещаем пробирку, чтобы она оказалась под иглой
            WaitExecution(Core.Transporter.Shift(false, TransporterController.ShiftType.HalfTube));
            
            // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            WaitExecution(Core.Arm.MoveOnTube());
            
            // Набираем материал из пробирки
            WaitExecution(Core.Pomp.Suction(0));
            
            // Подводим белую кювету картриджа под иглу
            WaitExecution(Core.Rotor.Home());
            WaitExecution(Core.Rotor.MoveCellUnderNeedle( 
                tube.Stages[0].CartridgePosition,
                CartridgeCell.WhiteCell,
                RotorController.CellPosition.CenterCell));

            // Устанавливаем иглу над белой ячейкой картриджа
            WaitExecution(Core.Arm.MoveToCartridge(
                ArmController.FromPosition.Tube,
                CartridgeCell.WhiteCell));

            // Опускаем иглу в кювету
            WaitExecution(Core.Arm.BrokeCartridge() );

            // Сливаем материал в белую кювету
            WaitExecution(Core.Pomp.Unsuction(0)); // слив из иглы в картридж

            WaitExecution(Core.Arm.Home());
            
            // Промываем иглу
            washingNeedleTask();

            // Выполняем перенос реагента из нужной ячейки картриджа в белую кювету
            performMiddleTask(tube);
            
            // Устанавливаем иглу в домашнюю позицию
            WaitExecution(Core.Arm.Home());

            // Смещаем пробирку обратно
            WaitExecution(Core.Transporter.Shift(true, TransporterController.ShiftType.HalfTube));

            Logger.AddMessage($"Пробирка [{tube.BarCode}] - подготовительная (0-я) стадия [Stop]");
        }

        private void WaitExecution(List<IAbstractCommand> task)
        {
            Core.CNCExecutor.ExecuteTask(task);
        }
    }
}
