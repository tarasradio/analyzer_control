using AnalyzerConfiguration;
using AnalyzerControlCore.Units;
using Infrastructure;
using System.Diagnostics;
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
            ConveyorCells = Enumerable.Repeat(new ConveyorCell(), ConveyorCellsNumber).ToArray();

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
            Logger.DemoInfo("Работа анализатора была прервана.");

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
                AnalyzerWorkTask(); // Итак, запустили мы эту херню в отдельном потоке? зачем? (Возможно, для того, чтобы ее можно было аварийно прервать)
            });
        }

        // Типа главная задача, которая запускается в отдельном потоке, выполнение которой (задачи) можно прервать
        // - Вопрос - что будет, если мы прервем задачу и затем запустим ее заного?? Состояние куда то сохраняется?
        private void AnalyzerWorkTask()
        {
            foreach (Tube tube in Options.Tubes)
            {
                lock(locker)
                {
                    tube.Clear(); // А надо ли это делать? А что, если нужно продолжить работу, если пробирки были поставленны ранее? (См. пункт про полный поворот ротора)
                }
            }

            InitializationTask(); // Вызываем задачу из задачи...
            NeedleWashing(); // Опять это делаем

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

                SearchTubeInConveyorCell(attemptsNumber: 2);

                //TODO: А че, нельзя как то нормально посчитать??? Че за ебаная магия тут???

                // Получение номера ячейки в точке забора материала из пробирки
                int cellUnderNeedle = CurrentCellAtScanner - CellsNumberBetweenScanAndSuction;
                if (cellUnderNeedle < 0) cellUnderNeedle += ConveyorCellsNumber;

                Tube tubeUnderNeedle = ConveyorCells[cellUnderNeedle].Tube;

                if(tubeUnderNeedle != null)
                {
                    Logger.DemoInfo($"Пробирка со штрихкодом [{tubeUnderNeedle.BarCode}] дошла до точки забора материала.");

                    if (NoSampleWasTaken(tubeUnderNeedle)) 
                    {
                        Logger.DemoInfo($"Забор материала ранее не производился.");

                        tubeUnderNeedle.CurrentStage = 0; //TODO: ПИЗДЕЦ!!!! Заменить на enum

                        ProcessingAnalisysInitialStage(tubeUnderNeedle);

                        // Жесть!
                        tubeUnderNeedle.TimeToStageComplete =
                            tubeUnderNeedle.Stages[tubeUnderNeedle.CurrentStage].TimeToPerform;
                    }
                }

                ProcessingScheduledAnalizes();

                CurrentCellAtScanner++;
            }

            Logger.DemoInfo($"Все пробирки обработаны!"); // Точно? Ты уверен?
        }

        private static bool NoSampleWasTaken(Tube tubeUnderNeedle)
        {
            return tubeUnderNeedle.CurrentStage == -1; //TODO: ПИЗДЕЦ!!!! Заменить на enum
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
        /// Обработка запланированных (оставшихся) анализов
        /// </summary>
        private void ProcessingScheduledAnalizes()
        {
            foreach (Tube tube in Options.Tubes)
            {
                if (ProcessingNotFinished(tube))
                {
                    tube.CurrentStage++;

                    if (IsNotFinishStage(tube))
                    {
                        tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;

                        Logger.DemoInfo($"Завершена инкубация для пробирки [{tube.BarCode}]!");

                        ProcessingAnalisysStage(tube);
                    }
                    else
                    {
                        Logger.DemoInfo($"Завершены все стадии анализа для пробирки [{tube.BarCode}]!");

                        ProcessingAnalisysFinishStage(tube);
                    }
                }
            }
        }

        // И эту тоже => IsLastStage(tube.CurrentStage)
        private static bool IsNotFinishStage(Tube tube)
        {
            return tube.CurrentStage < tube.Stages.Count;
        }

        // Как бы эту логику вынести в саму пробирку?
        private static bool ProcessingNotFinished(Tube tube)
        {
            return tube.CurrentStage >= 0 &&
                IsNotFinishStage(tube) &&
                tube.TimeToStageComplete == 0;
        }

        private bool ExistUnhandledTubes()
        {
            bool result = false;

            foreach(Tube tube in Options.Tubes)
            {
                if (tube.CurrentStage <= tube.Stages.Count) // Это еще почему ???
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        // TODO: А что, если пробирка найдена, но в cell уже забит штрих-код другой пробирки? 
        // (это все к вопросу о том, что мы не можем остлеживать точно полный круг конвейера)
        private void SearchTubeInConveyorCell(int attemptsNumber)
        {
            ConveyorCell cell = ConveyorCells[CurrentCellAtScanner];

            Logger.DemoInfo($"Запущен поиск пробирки (сканирование)");

            AnalyzerGateway.Conveyor.Shift(reverse:false);
            
            int attempt = 0;

            while (attempt < attemptsNumber)
            {
                Logger.DemoInfo($"Попытка {attempt}.");

                AnalyzerGateway.Conveyor.RotateAndScanTube();

                string barcode = AnalyzerGateway.Context.TubeBarcode;

                if ( !string.IsNullOrWhiteSpace(barcode) )
                {
                    Logger.DemoInfo($"Обнаружена пробирка со штрихкодом [{barcode}].");

                    cell.Tube = SearchBarcodeInDatabase(barcode);

                    if(cell.Tube != null)
                    {
                        Logger.DemoInfo($"Пробирка со штрихкодом [{barcode}] найдена в списке анализов!");
                        cell.Tube.IsFind = true;
                    }
                    else
                    {
                        Logger.DemoInfo($"Пробирка со штрихкодом [{barcode}] не найдена в списке анализов!");
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

        private Tube SearchBarcodeInDatabase(string barcode)
        {
            // Значение по умолчанию для ссылочных и допускающих значения NULL типов равно null .
            Tube result = Options.Tubes.Where(tube => barcode.Contains(tube.BarCode)).FirstOrDefault();

            return result;
        }

        private void ProcessingAnalisysInitialStage(Tube tube)
        {
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - запущено выполнение подготовительной стадии.");
            Logger.DemoInfo($"Подготовка к забору материала из пробирки.");
            
            // Смещаем пробирку, чтобы она оказалась под иглой
            AnalyzerGateway.Conveyor.Shift(false, ConveyorUnit.ShiftType.HalfTube);

            // Поднимаем иглу вверх до дома
            AnalyzerGateway.Needle.HomeLifter();

            Logger.DemoInfo($"Ожидание загрузки картриджа...");

            CartridgeCharging(cartirdgePosition: 0, cellNumber: 5);

            Logger.DemoInfo($"Загрузка картриджа завершена.");
            Logger.DemoInfo($"Ожидание касания жидкости в пробирке...");

            // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            AnalyzerGateway.Needle.TurnToTubeAndWaitTouch();

            Logger.DemoInfo($"Забор материала из пробирки.");

            // Набираем материал из пробирки
            AnalyzerGateway.Pomp.Pull(0);
            
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
            AnalyzerGateway.Pomp.Push(0);

            // Поднимаем иглу вверх до дома
            AnalyzerGateway.Needle.HomeLifter();
            
            // Промываем иглу
            NeedleWashing();

            Logger.DemoInfo($"Перенос реагента в белую кювету.");

            // Выполняем перенос реагента из нужной ячейки картриджа в белую кювету
            ProcessingAnalisysStage(tube);
            
            // Устанавливаем иглу в домашнюю позицию
            AnalyzerGateway.Needle.HomeLifterAndRotator();

            // Смещаем пробирку обратно
            AnalyzerGateway.Conveyor.Shift(reverse: true, ConveyorUnit.ShiftType.HalfTube);

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - завершено выполнение подготовительной стадии.");
        }

        private void CartridgeCharging(int cartirdgePosition, int cellNumber)
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

        private void ProcessingAnalisysStage(Tube tube)
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
            AnalyzerGateway.Pomp.Pull(0);

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
            AnalyzerGateway.Pomp.Push(0);

            // Поднимаем иглу до дома
            AnalyzerGateway.Needle.HomeLifter();

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - завершено выполнение {tube.CurrentStage}-й стадии.");
        }

        // TODO: Эта задача не реализована до конца!!!
        private void ProcessingAnalisysFinishStage(Tube tube)
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

            AnalyzerGateway.Pomp.Pull(0);

            AnalyzerGateway.Needle.HomeLifter();

            // Далее нужно перелить в прозрачную кювету и отправить на анализ.

            // TODO: Эта задача не реализована до конца!!!

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - выполнения завершающей стадии завершено.");
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
    }
}
