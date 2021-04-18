using AnalyzerConfiguration;
using AnalyzerControl.Configuration;
using AnalyzerControl.Entyties;
using AnalyzerDomain.Entyties;
using AnalyzerService;
using AnalyzerService.Units;
using Infrastructure;
using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace AnalyzerControl
{
    public class AnalyzerDemoController : Configurable<DemoControllerConfiguration>
    {
        // Ячейки конвейера
        private const int ConveyorCellsNumber = 54;
        private ConveyorCell[] ConveyorCells;
        private const int CellsNumberBetweenScanAndSuction = 7;
        private int CurrentCellAtScanner = 0;

        // Ячейки ротора
        private const int RotorCellsNumber = 40; // !!! Нужно уточнить этот параметр
        private AnalysisInfo[] RotorCells;

        private Stopwatch stopwatch;
        Timer timer;

        private static object locker = new object();

        public AnalyzerDemoController(IConfigurationProvider provider) : base(provider)
        {
            ConveyorCells = Enumerable.Repeat(new ConveyorCell(), ConveyorCellsNumber).ToArray();

            RotorCells = new AnalysisInfo[RotorCellsNumber];

            stopwatch = new Stopwatch();
            timer = new Timer();

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds >= (60 * 1e3))
            {
                stopwatch.Restart();
                Logger.DemoInfo("Прошла минута");

                foreach (AnalysisInfo analysis in Options.Analyzes)
                {
                    lock (locker)
                    {
                        if (analysis.InProgress())
                            analysis.DecrementRemainingTime();
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
            Analyzer.TaskExecutor.StartTask(() =>
            {
                AnalyzerWorkTask(); // Итак, запустили мы эту херню в отдельном потоке? зачем? (Возможно, для того, чтобы ее можно было аварийно прервать)
            });
        }

        // Типа главная задача, которая запускается в отдельном потоке, выполнение которой (задачи) можно прервать
        // - Вопрос - что будет, если мы прервем задачу и затем запустим ее заного?? Состояние куда то сохраняется?
        private void AnalyzerWorkTask()
        {
            foreach (AnalysisInfo tube in Options.Analyzes)
            {
                lock (locker)
                {
                    tube.Clear(); // А надо ли это делать? А что, если нужно продолжить работу, если пробирки были поставленны ранее? (См. пункт про полный поворот ротора)
                }
            }

            InitializationTask(); // Вызываем задачу из задачи...
            AnalyzerOperations.NeedleWash();// Опять это делаем

            Logger.DemoInfo($"Запущена подготовка перед сканированием пробирок");

            Analyzer.Needle.HomeLifterAndRotator();
            Analyzer.Conveyor.PrepareBeforeScanning();

            Logger.DemoInfo($"Подготовка завершена");

            CurrentCellAtScanner = 0;

            while (ExistUnhandledAnalyzes()) // пока есть невыполненные задачи
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

                AnalysisInfo tubeUnderNeedle = ConveyorCells[cellUnderNeedle].AnalysisInfo;

                if (tubeUnderNeedle != null)
                {
                    Logger.DemoInfo($"Пробирка со штрихкодом [{tubeUnderNeedle.BarCode}] дошла до точки забора материала.");

                    if (NoSampleWasTaken(tubeUnderNeedle))
                    {
                        Logger.DemoInfo($"Забор материала ранее не производился.");

                        tubeUnderNeedle.CurrentStage = 0; //TODO: ПИЗДЕЦ!!!! Заменить на enum

                        ProcessAnalisysInitialStage(tubeUnderNeedle);

                        tubeUnderNeedle.SetNewIncubationTime();
                    }
                }

                ProcessScheduledAnalizes();

                CurrentCellAtScanner++;
            }

            Logger.DemoInfo($"Все пробирки обработаны!"); // Точно? Ты уверен?
        }

        private static bool NoSampleWasTaken(AnalysisInfo tubeUnderNeedle)
        {
            return tubeUnderNeedle.CurrentStage == -1; //TODO: ПИЗДЕЦ!!!! Заменить на enum
        }

        private void InitializationTask()
        {
            Logger.DemoInfo($"Запущена инициализация всех устройств");

            Analyzer.Needle.HomeLifterAndRotator();
            Analyzer.Charger.HomeHook();
            Analyzer.Charger.MoveHookAfterHome();
            Analyzer.Charger.HomeRotator();
            Analyzer.Rotor.Home();
            Analyzer.Pomp.Home();

            Logger.DemoInfo($"Инициализация завершена");
        }

        /// <summary>
        /// Обработка запланированных (оставшихся) анализов
        /// </summary>
        private void ProcessScheduledAnalizes()
        {
            foreach (AnalysisInfo analysis in Options.Analyzes)
            {
                if (analysis.ProcessingNotFinished())
                {
                    analysis.NextStage();

                    if (analysis.IsNotFinishStage())
                    {
                        analysis.SetNewIncubationTime();

                        Logger.DemoInfo($"Завершена инкубация для анализа [{analysis.BarCode}]!");

                        ProcessAnalisysStage(analysis);
                    }
                    else
                    {
                        Logger.DemoInfo($"Завершены все стадии анализа для анализа [{analysis.BarCode}]!");

                        ProcessAnalisysFinishStage(analysis);
                    }
                }
            }
        }

        private bool ExistUnhandledAnalyzes()
        {
            bool result = false;

            foreach (AnalysisInfo analysis in Options.Analyzes)
            {
                if (analysis.Finished())
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

            Analyzer.Conveyor.Shift(reverse: false);

            int attempt = 0;

            while (attempt < attemptsNumber)
            {
                Logger.DemoInfo($"Попытка {attempt}.");

                Analyzer.Conveyor.RotateAndScanTube();

                string barcode = Analyzer.State.TubeBarcode;

                if (!string.IsNullOrWhiteSpace(barcode))
                {
                    Logger.DemoInfo($"Обнаружена пробирка со штрихкодом [{barcode}].");

                    cell.AnalysisInfo = SearchBarcodeInDatabase(barcode);

                    if (cell.AnalysisInfo != null)
                    {
                        Logger.DemoInfo($"Пробирка со штрихкодом [{barcode}] найдена в списке анализов!");
                        cell.AnalysisInfo.IsFind = true;
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

        private AnalysisInfo SearchBarcodeInDatabase(string barcode)
        {
            // Значение по умолчанию для ссылочных и допускающих значения NULL типов равно null .
            return Options.Analyzes.Where(analysis => barcode.Contains(analysis.BarCode)).FirstOrDefault();
        }

        private void ProcessAnalisysInitialStage(AnalysisInfo analysis)
        {
            Logger.DemoInfo($"Анализ [{analysis.BarCode}] - запущено выполнение подготовительной стадии.");
            Logger.DemoInfo($"Подготовка к забору материала из пробирки.");

            // Смещаем пробирку, чтобы она оказалась под иглой
            Analyzer.Conveyor.Shift(false, ConveyorUnit.ShiftType.HalfTube);

            // Поднимаем иглу вверх до дома
            Analyzer.Needle.HomeLifter();

            Logger.DemoInfo($"Ожидание загрузки картриджа...");

            AnalyzerOperations.ChargeCartridge(cartirdgePosition: 0, chargePosition: 5);

            Logger.DemoInfo($"Загрузка картриджа завершена.");
            Logger.DemoInfo($"Ожидание касания жидкости в пробирке...");

            // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            Analyzer.Needle.TurnToTubeAndWaitTouch();

            Logger.DemoInfo($"Забор материала из пробирки.");

            // Набираем материал из пробирки
            Analyzer.Pomp.Pull(0);

            // Подводим белую кювету картриджа под иглу
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(
                analysis.Stages[0].CartridgePosition,
                CartridgeCell.MixCell);

            // Поднимаем иглу вверх до дома
            Analyzer.Needle.HomeLifter();

            // Устанавливаем иглу над белой ячейкой картриджа
            Analyzer.Needle.TurnToCartridge(CartridgeCell.MixCell);

            // Опускаем иглу в кювету
            Analyzer.Needle.GoDownAndPerforateCartridge(CartridgeCell.MixCell);

            Logger.DemoInfo($"Слив забранного материала в белую кювету.");

            // Сливаем материал в белую кювету
            Analyzer.Pomp.Push(0);

            // Поднимаем иглу вверх до дома
            Analyzer.Needle.HomeLifter();

            // Промываем иглу
            AnalyzerOperations.NeedleWash();

            Logger.DemoInfo($"Перенос реагента в белую кювету.");

            // Выполняем перенос реагента из нужной ячейки картриджа в белую кювету
            ProcessAnalisysStage(analysis);

            // Устанавливаем иглу в домашнюю позицию
            Analyzer.Needle.HomeLifterAndRotator();

            // Смещаем пробирку обратно
            Analyzer.Conveyor.Shift(reverse: true, ConveyorUnit.ShiftType.HalfTube);

            Logger.DemoInfo($"Анализ [{analysis.BarCode}] - завершено выполнение подготовительной стадии.");
        }

        private void ProcessAnalisysStage(AnalysisInfo analysis)
        {
            Logger.DemoInfo($"Анализ [{analysis.BarCode}] - запуск выполнения {analysis.CurrentStage}-й стадии.");

            Analyzer.Needle.HomeLifterAndRotator();

            AnalyzerOperations.NeedleWash();

            // Подводим нужную ячейку картриджа под иглу
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(
                analysis.Stages[analysis.CurrentStage].CartridgePosition,
                analysis.Stages[analysis.CurrentStage].Cell,
                RotorUnit.CellPosition.CellLeft);

            // Устанавливаем иглу над нужной ячейкой картриджа
            Analyzer.Needle.TurnToCartridge(analysis.Stages[analysis.CurrentStage].Cell);

            // Прокалываем ячейку картриджа
            Analyzer.Needle.GoDownAndPerforateCartridge(analysis.Stages[analysis.CurrentStage].Cell);

            // Поднимаемся на безопасную высоту над картриджем
            Analyzer.Needle.GoToSafeLevel();

            // Подводим центр ячейки картриджа под иглу
            Analyzer.Rotor.PlaceCellUnderNeedle(
                analysis.Stages[analysis.CurrentStage].CartridgePosition,
                analysis.Stages[analysis.CurrentStage].Cell,
                RotorUnit.CellPosition.CellCenter);

            // Прокалываем ячейку картриджа
            Analyzer.Needle.GoDownAndPerforateCartridge(analysis.Stages[analysis.CurrentStage].Cell);

            // Забираем реагент из ячейки картриджа
            Analyzer.Pomp.Pull(0);

            // Поднимаемся на безопасную высоту над картриджем
            Analyzer.Needle.GoToSafeLevel();

            // Устанавливаем иглу над белой ячейкой картриджа
            Analyzer.Needle.TurnToCartridge(CartridgeCell.MixCell);

            // Подводим белую кювету картриджа под иглу
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(
                analysis.Stages[analysis.CurrentStage].CartridgePosition,
                CartridgeCell.MixCell);

            // Опускаем иглу в белую кювету
            Analyzer.Needle.GoDownAndPerforateCartridge(CartridgeCell.MixCell, false);

            // Сливаем реагент в белую кювету
            Analyzer.Pomp.Push(0);

            // Поднимаем иглу до дома
            Analyzer.Needle.HomeLifter();

            Logger.DemoInfo($"Анализ [{analysis.BarCode}] - завершено выполнение {analysis.CurrentStage}-й стадии.");
        }

        // TODO: Эта задача не реализована до конца!!!
        private void ProcessAnalisysFinishStage(AnalysisInfo analysis)
        {
            Logger.DemoInfo($"Анализ [{analysis.BarCode}] - запуск выполнения завершающей стадии.");

            AnalyzerOperations.NeedleWash();

            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(
                analysis.Stages[analysis.Stages.Count - 1].CartridgePosition,
                CartridgeCell.MixCell);

            Analyzer.Needle.HomeLifter();
            Analyzer.Needle.TurnToCartridge(CartridgeCell.MixCell);

            Analyzer.Needle.GoDownAndPerforateCartridge(CartridgeCell.MixCell);

            Analyzer.Pomp.Pull(0);

            Analyzer.Needle.GoToSafeLevel();

            Analyzer.Rotor.PlaceCellUnderNeedle(analysis.Stages[analysis.Stages.Count - 1].CartridgePosition,
                CartridgeCell.ResultCell);

            // Устанавливаем иглу над белой ячейкой картриджа
            Analyzer.Needle.TurnToCartridge(CartridgeCell.ResultCell);

            Analyzer.Needle.GoDownAndPerforateCartridge(CartridgeCell.ResultCell); // TODO: Добавить реализацию в NeedleUnit

            Analyzer.Pomp.Push(0);

            Analyzer.Needle.HomeLifter();

            Analyzer.Rotor.PlaceCellUnderWashBuffer(analysis.Stages[analysis.Stages.Count - 1].CartridgePosition);

            //AnalyzerGateway.Rotor.PlaceCellAtDischarge(analysis.Stages[analysis.Stages.Count - 1].CartridgePosition);

            AnalyzerOperations.DischargeCartridge(analysis.Stages[analysis.Stages.Count - 1].CartridgePosition);

            // Далее нужно перелить в прозрачную кювету и отправить на анализ.

            // TODO: Эта задача не реализована до конца!!!

            Logger.DemoInfo($"Анализ [{analysis.BarCode}] - выполнения завершающей стадии завершено.");
        }
    }
}
