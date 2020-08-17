using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerControlCore.Units;
using Infrastructure;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;

namespace AnalyzerControlCore
{
    public class ConveyorCell
    {
        public bool HaveTube { get; set; }
        public string BarCode { get; set; }
        public Tube Tube { get; set; }

        public ConveyorCell()
        {
            Tube = null;
            HaveTube = false;
            BarCode = string.Empty;
        }
    };

    public class DemoController : UnitBase<DemoControllerConfiguration>
    {
        // Ячейки конвейера
        private const int ConveyorCellsNumber = 54;
        private ConveyorCell[] ConveyorCells;
        private const int CellsNumberBetweenScanAndSuction = 7;
        private int CurrentCellAtScanner = 0;

        // Ячейки ротора
        private const int RotorCellsNumber = 40; // !!! Нужно уточнить этот параметр
        private Tube[] RotorCells;

        private Stopwatch stopwatch;
        Timer timer;

        private static object locker = new object();

        public DemoController(IConfigurationProvider provider) : base(null, provider)
        {
            ConveyorCells = new ConveyorCell[ConveyorCellsNumber];
            ConveyorCells.Initialize();

            RotorCells = new Tube[RotorCellsNumber];

            stopwatch = new Stopwatch();
            timer = new Timer();

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if( stopwatch.ElapsedMilliseconds >= (60 * 1e3) )
            {
                stopwatch.Restart();
                Logger.DemoInfo("Прошла минута");
                
                // уменьшаем оставшееся время инкубации
                foreach (Tube tube in Options.Tubes)
                {
                    lock (locker)
                    {
                        if(tube.IsFind && tube.TimeToStageComplete != 0)
                            tube.TimeToStageComplete -= 1;
                    }
                }
            }
        }

        public void AbortWork()
        {
            Logger.DemoInfo("Работа демо режима прервана.");

            if (timer.Enabled) timer.Stop();
            if (stopwatch.IsRunning) stopwatch.Stop();
        }

        public void StartWork()
        {
            timer.Start();
            stopwatch.Reset();
            stopwatch.Start();

            //TODO: ПИЗДЕЦ (Надо что то сделать, убрать зависимость...)
            AnalyzerGateway.Executor.StartTask(() =>
            {
                AnalyzerWorkTask();
            });
        }

        private void AnalyzerWorkTask()
        {
            foreach (Tube tube in Options.Tubes)
            {
                lock(locker)
                {
                    tube.Clear(); // А надо ли это делать? А что, если нужно продолжить работу, если паробирки были поставленны ранее?
                }
            }

            InitializationTask();
            NeedleWashing();

            Logger.DemoInfo($"Запущена подготовка перед сканированием пробирок");

            AnalyzerGateway.Needle.HomeLifterAndRotator();
            AnalyzerGateway.Conveyor.PrepareBeforeScanning();

            Logger.DemoInfo($"Подготовка завершена");

            CurrentCellAtScanner = 0;

            while ( ExistUnhandledTubes() ) // пока есть невыполненные задачи
            {
                if (CurrentCellAtScanner == ConveyorCellsNumber) // прошли полный круг (и чо???)
                {
                    CurrentCellAtScanner = 0;
                    Logger.DemoInfo($"Круг пройден. Запуск повторного сканирования.");
                }

                SearchTubeInConveyorCell(2);

                //TODO: А че, нельзя как то нормально посчитать??? Че за ебаная магия тут???
                // Получение номера ячейки в точке забора материала из пробирки
                int cellUnderNeedle = CurrentCellAtScanner - CellsNumberBetweenScanAndSuction;
                if (cellUnderNeedle < 0) cellUnderNeedle += ConveyorCellsNumber;
                // Номер получен

                Tube tubeUnderNeedle = ConveyorCells[cellUnderNeedle].Tube;

                if(tubeUnderNeedle != null)
                {
                    Logger.DemoInfo($"Пробирка со штрихкодом [{tubeUnderNeedle.BarCode}] дошла до точки забора материала.");
                    
                    if(tubeUnderNeedle.CurrentStage == -1) //TODO: ПИЗДЕЦ!!!! Заменить на enum
                    {
                        Logger.DemoInfo($"Забор материала ранее не производился.");

                        tubeUnderNeedle.CurrentStage = 0; //TODO: ПИЗДЕЦ!!!! Заменить на enum

                        TubeFirstStageHandle(tubeUnderNeedle);

                        // Жесть!
                        tubeUnderNeedle.TimeToStageComplete = 
                            tubeUnderNeedle.Stages[tubeUnderNeedle.CurrentStage].TimeToPerform;
                    }
                }

                PerformingOutstandingTasks();

                CurrentCellAtScanner++;
            }

            Logger.DemoInfo($"Все пробирки обработаны!"); // Точно? Ты уверен?
        }

        private void InitializationTask()
        {
            Logger.DemoInfo($"Запущена инициализация всех устройств");

            AnalyzerGateway.Needle.HomeLifterAndRotator();
            AnalyzerGateway.Charger.HomeHook();
            AnalyzerGateway.Charger.MoveHookAfterHome();
            AnalyzerGateway.Charger.HomeRotator();
            AnalyzerGateway.Rotor.Home();
            AnalyzerGateway.Pomp.Home();

            Logger.DemoInfo($"Инициализация завершена");
        }

        /// <summary>
        /// Выполнение запланированных задач
        /// </summary>
        private void PerformingOutstandingTasks()
        {
            foreach (Tube tube in Options.Tubes)
            {
                // Как бы эту логику вынести в саму пробирку?
                if( tube.CurrentStage >= 0 &&
                    tube.CurrentStage < tube.Stages.Count && 
                    tube.TimeToStageComplete == 0)
                {
                    tube.CurrentStage++;
                    
                    // И эту тоже => IsLastStage(tube.CurrentStage)
                    if (tube.CurrentStage < tube.Stages.Count)
                    {
                        tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;

                        Logger.DemoInfo($"Завершена инкубация для пробирки [{tube.BarCode}]!");

                        TubeIntermediateStageHandle(tube);
                    }
                    else
                    {
                        Logger.DemoInfo($"Завершены все стадии анализа для пробирки [{tube.BarCode}]!");

                        TubeFinishStageHandle(tube);
                    }
                }
            }
        }

        private Tube SearchBarcodeInDatabase(string barcode)
        {
            Tube result = Options.Tubes.Where( tube => barcode.Contains(tube.BarCode)).FirstOrDefault();

            //foreach (Tube tube in Options.Tubes)
            //{
            //    if (barcode.Contains(tube.BarCode))
            //    {
            //        result = tube;
            //        break;
            //    }
            //}

            return result;
        }

        private bool ExistUnhandledTubes()
        {
            bool result = false;

            foreach(Tube tube in Options.Tubes)
            {
                if (tube.CurrentStage <= tube.Stages.Count)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private void NeedleWashing()
        {
            Logger.DemoInfo($"Запущена промывка иглы");

            AnalyzerGateway.Needle.HomeLifter();
            AnalyzerGateway.Needle.TurnAndGoDownToWashing();
            AnalyzerGateway.Pomp.WashingNeedle(2);
            AnalyzerGateway.Pomp.Home();
            AnalyzerGateway.Pomp.CloseValves();

            Logger.DemoInfo($"Промывка иглы завершена.");
        }
        
        private void SearchTubeInConveyorCell(int numberAttempts)
        {
            int attempt = 0;
            
            ConveyorCell cell = ConveyorCells[CurrentCellAtScanner];

            Logger.DemoInfo($"Запущен поиск пробирки (сканирование)");

            AnalyzerGateway.Conveyor.Shift(reverse:false);

            while (attempt < numberAttempts)
            {
                Logger.DemoInfo($"Попытка {attempt}.");

                AnalyzerGateway.Conveyor.RotateAndScanTube();

                string foundBarcode = AnalyzerGateway.Context.TubeBarcode;

                if ( !string.IsNullOrWhiteSpace(foundBarcode) )
                {
                    Logger.DemoInfo($"Обнаружена пробирка со штрихкодом [{foundBarcode}].");

                    cell.Tube = SearchBarcodeInDatabase(foundBarcode);

                    if(cell.Tube != null)
                    {
                        Logger.DemoInfo($"Пробирка со штрихкодом [{foundBarcode}] найдена в списке анализов!");
                        cell.Tube.IsFind = true;
                    }
                    else
                    {
                        Logger.DemoInfo($"Пробирка со штрихкодом [{foundBarcode}] не найдена в списке анализов!");
                    }
                    break;
                }
                else
                {
                    Logger.DemoInfo($"Пробирка не обнаружена.");
                }
                attempt++;
            }

            Logger.DemoInfo($"Поиск пробирки (сканирование) завершен.");
        }

        private void TubeFinishStageHandle(Tube tube)
        {
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - запуск выполнения завершающей стадии.");

            NeedleWashing();

            AnalyzerGateway.Rotor.Home();
            AnalyzerGateway.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.Stages.Count - 1].CartridgePosition, 
                CartridgeCell.WhiteCell);

            AnalyzerGateway.Needle.HomeLifter();
            AnalyzerGateway.Needle.TurnToCartridge(CartridgeCell.WhiteCell);

            AnalyzerGateway.Needle.GoDownAndPerforateCartridge(CartridgeCell.WhiteCell);

            AnalyzerGateway.Pomp.Suction(0);

            AnalyzerGateway.Needle.HomeLifter();

            // Далее нужно перелить в прозрачную кювету и отправить на анализ.

            // TODO: Эта задача не реализована до конца!!!
            
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - выполнения завершающей стадии завершено.");
        }

        private void TubeIntermediateStageHandle(Tube tube)
        {
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - запуск выполнения {tube.CurrentStage}-й стадии.");

            AnalyzerGateway.Needle.HomeLifterAndRotator();

            NeedleWashing();

            // Подводим нужную ячейку картриджа под иглу
            AnalyzerGateway.Rotor.Home();
            AnalyzerGateway.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                tube.Stages[tube.CurrentStage].Cell,
                RotorUnit.CellPosition.CellLeft);

            // Устанавливаем иглу над нужной ячейкой картриджа
            AnalyzerGateway.Needle.TurnToCartridge(tube.Stages[tube.CurrentStage].Cell);

            // Прокалываем ячейку картриджа
            AnalyzerGateway.Needle.GoDownAndPerforateCartridge(tube.Stages[tube.CurrentStage].Cell);

            // Поднимаемся на безопасную высоту над картриджем
            AnalyzerGateway.Needle.GoToSafeLevel();

            // Подводим центр ячейки картриджа под иглу
            AnalyzerGateway.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                tube.Stages[tube.CurrentStage].Cell,
                RotorUnit.CellPosition.CellCenter);

            // Прокалываем ячейку картриджа
            AnalyzerGateway.Needle.GoDownAndPerforateCartridge(tube.Stages[tube.CurrentStage].Cell);

            // Забираем реагент из ячейки картриджа
            AnalyzerGateway.Pomp.Suction(0);

            // Поднимаемся на безопасную высоту над картриджем
            AnalyzerGateway.Needle.GoToSafeLevel();

            // Устанавливаем иглу над белой ячейкой картриджа
            AnalyzerGateway.Needle.TurnToCartridge(CartridgeCell.WhiteCell);

            // Подводим белую кювету картриджа под иглу
            AnalyzerGateway.Rotor.Home();
            AnalyzerGateway.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                CartridgeCell.WhiteCell);

            // Опускаем иглу в белую кювету
            AnalyzerGateway.Needle.GoDownAndPerforateCartridge(CartridgeCell.WhiteCell, false);

            // Сливаем реагент в белую кювету
            AnalyzerGateway.Pomp.Unsuction(0);

            // Поднимаем иглу до дома
            AnalyzerGateway.Needle.HomeLifter();

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - завершено выполнение {tube.CurrentStage}-й стадии.");
        }

        private void ChargeCartridge(int cartirdgePosition, int cellNumber)
        {
            AnalyzerGateway.Rotor.Home();

            AnalyzerGateway.Rotor.PlaceCellAtCharge(cartirdgePosition, cellNumber);

            AnalyzerGateway.Charger.HomeHook();
            AnalyzerGateway.Charger.MoveHookAfterHome();
            AnalyzerGateway.Charger.HomeRotator();

            AnalyzerGateway.Charger.TurnToCell(cellNumber);

            AnalyzerGateway.Charger.ChargeCartridge();
            AnalyzerGateway.Charger.HomeHook();
            AnalyzerGateway.Charger.MoveHookAfterHome();
        }

        private void TubeFirstStageHandle(Tube tube)
        {
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - запущено выполнение подготовительной стадии.");

            Logger.DemoInfo($"Подготовка к забору материала из пробирки.");
            
            // Смещаем пробирку, чтобы она оказалась под иглой
            AnalyzerGateway.Conveyor.Shift(false, ConveyorUnit.ShiftType.HalfTube);

            // Поднимаем иглу вверх до дома
            AnalyzerGateway.Needle.HomeLifter();

            Logger.DemoInfo($"Ожидание загрузки картриджа...");

            ChargeCartridge(cartirdgePosition: 0, cellNumber: 5);

            Logger.DemoInfo($"Загрузка картриджа завершена.");

            Logger.DemoInfo($"Ожидание касания жидкости в пробирке...");

            // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            AnalyzerGateway.Needle.TurnToTubeAndWaitTouch();

            Logger.DemoInfo($"Забор материала из пробирки.");

            // Набираем материал из пробирки
            AnalyzerGateway.Pomp.Suction(0);
            
            // Подводим белую кювету картриджа под иглу
            AnalyzerGateway.Rotor.Home();
            AnalyzerGateway.Rotor.PlaceCellUnderNeedle( 
                tube.Stages[0].CartridgePosition,
                CartridgeCell.WhiteCell);

            // Поднимаем иглу вверх до дома
            AnalyzerGateway.Needle.HomeLifter();

            // Устанавливаем иглу над белой ячейкой картриджа
            AnalyzerGateway.Needle.TurnToCartridge(CartridgeCell.WhiteCell);

            // Опускаем иглу в кювету
            AnalyzerGateway.Needle.GoDownAndPerforateCartridge(CartridgeCell.WhiteCell);


            Logger.DemoInfo($"Слив забранного материала в белую кювету.");

            // Сливаем материал в белую кювету
            AnalyzerGateway.Pomp.Unsuction(0);

            // Поднимаем иглу вверх до дома
            AnalyzerGateway.Needle.HomeLifter();
            
            // Промываем иглу
            NeedleWashing();

            Logger.DemoInfo($"Перенос реагента в белую кювету.");

            // Выполняем перенос реагента из нужной ячейки картриджа в белую кювету
            TubeIntermediateStageHandle(tube);
            
            // Устанавливаем иглу в домашнюю позицию
            AnalyzerGateway.Needle.HomeLifterAndRotator();

            // Смещаем пробирку обратно
            AnalyzerGateway.Conveyor.Shift(reverse: true, ConveyorUnit.ShiftType.HalfTube);

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - завершено выполнение подготовительной стадии.");
        }
    }
}
