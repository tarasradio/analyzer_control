using AnalyzerConfiguration;
using AnalyzerControl.Configuration;
using AnalyzerControl.Services;
using AnalyzerDomain.Entyties;
using AnalyzerService;
using AnalyzerService.Units;
using Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace AnalyzerControl
{
    public class AnalyzerDemoController : Configurable<DemoControllerConfiguration>
    {
        ConveyorService conveyor;

        private Stopwatch stopwatch;
        Timer timer;

        const int millisecondsInMinute = 60 * 1000;

        private static object locker = new object();

        public enum States
        {
            Interrupted,
            AnalysisProcessing
        }

        public States state { get; private set; }

        bool interruptRequest = false;

        public AnalyzerDemoController(IConfigurationProvider provider, ConveyorService conveyor) : base(provider)
        {
            this.conveyor = conveyor;

            stopwatch = new Stopwatch();
            timer = new Timer();

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;

            state = States.Interrupted;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds >= millisecondsInMinute) {
                stopwatch.Restart();
                Logger.Debug("Прошла минута");

                foreach (AnalysisInfo analysis in Options.Analyzes) {
                    lock (locker) {
                        if (analysis.InProgress())
                            analysis.DecrementRemainingTime();
                    }
                }
            }
        }

        public void AbortWork()
        {
            Logger.Debug("Работа анализатора была прервана.");

            if (timer.Enabled) timer.Stop();
            if (stopwatch.IsRunning) stopwatch.Stop();
        }

        public void StartWork()
        {
            state = States.AnalysisProcessing;

            timer.Start();
            stopwatch.Reset();
            stopwatch.Start();

            startWorkCycle();
        }

        public void InterruptWork()
        {
            if(state == States.AnalysisProcessing)
            {
                interruptRequest = true;

                while (state != States.Interrupted);

                // Выполнение действий после прерывания
                Analyzer.Needle.HomeLifterAndRotator();
            }
        }

        public void ResumeWork()
        {
            state = States.AnalysisProcessing;

            startWorkCycle();
        }

        private void startWorkCycle()
        {
            Analyzer.TaskExecutor.StartTask(() =>
            {
                analyzerWorkCycle(); // Итак, запустили мы эту херню в отдельном потоке? зачем? (Возможно, для того, чтобы ее можно было аварийно прервать)
            });
        }

        // Типа главная задача, которая запускается в отдельном потоке, выполнение которой (задачи) можно прервать
        // - Вопрос - что будет, если мы прервем задачу и затем запустим ее заного?? Состояние куда то сохраняется?
        private void analyzerWorkCycle()
        {
            resetAnalyzes();

            AnalyzerOperations.MoveAllToHome();
            AnalyzerOperations.NeedleWash();

            Logger.Debug($"Запуск подготовки перед сканированием пробирок.");

            Analyzer.Needle.HomeLifterAndRotator();
            Analyzer.Conveyor.PrepareBeforeScanning();

            Logger.Debug($"Подготовка перед сканированием пробирок завершена.");

            processAnalyzesCycle();

            if(interruptRequest) {
                state = States.Interrupted;
                interruptRequest = false;
                Logger.Debug($"Прерывание работы выполнено.");
            } else {
                Logger.Debug($"Все пробирки обработаны!"); // Точно? Ты уверен?
            }
        }

        private void processAnalyzesCycle()
        {
            conveyor.CellInScanPosition = 0;

            while (existUnhandledAnalyzes()) {

                if (interruptRequest) {
                    break;
                }

                conveyor.CheckFullCycle();

                searchTubeInConveyorCell(attemptsNumber: 2);

                int cellInSampling = conveyor.CellInSamplingPosition;

                AnalysisInfo analysisInSampling =
                    searchBarcodeInDatabase(conveyor.Cells[cellInSampling].AnalysisBarcode);

                if (analysisInSampling != null) {
                    Logger.Debug($"Пробирка со штрихкодом [{analysisInSampling.BarCode}] дошла до точки забора материала.");

                    if (analysisInSampling.NoSampleWasTaken()) {
                        Logger.Debug($"Забор материала ранее не производился.");

                        analysisInSampling.SetSamplingStage();

                        processAnalisysInitialStage(analysisInSampling);

                        analysisInSampling.SetNewIncubationTime();
                    }
                }

                processScheduledAnalyzes();

                conveyor.CellInScanPosition++;
            }
        }

        private void resetAnalyzes()
        {   // А надо ли это делать? А что, если нужно продолжить работу, если пробирки были поставленны ранее? (См. пункт про полный поворот ротора)
            foreach (AnalysisInfo analysis in Options.Analyzes) {
                lock (locker) {
                    analysis.Clear(); 
                }
            }
        }

        /// <summary>
        /// Обработка запланированных (оставшихся) анализов
        /// </summary>
        private void processScheduledAnalyzes()
        {
            foreach (AnalysisInfo analysis in Options.Analyzes) {
                if (interruptRequest) {
                    break;
                }

                if (analysis.ProcessingNotFinished()) {
                    analysis.NextStage();

                    if (analysis.IsNotFinishStage()) {
                        analysis.SetNewIncubationTime();

                        Logger.Debug($"Завершена инкубация для анализа [{analysis.BarCode}]!");

                        processAnalisysStage(analysis);
                    } else {
                        Logger.Debug($"Завершены все стадии анализа для анализа [{analysis.BarCode}]!");

                        processAnalisysFinishStage(analysis);
                    }
                }
            }
        }

        private bool existUnhandledAnalyzes()
        {
            bool result = false;

            foreach (AnalysisInfo analysis in Options.Analyzes) {
                if (analysis.Finished()) {
                    result = true;
                    break;
                }
            }

            return result;
        }

        // TODO: А что, если пробирка найдена, но в cell уже забит штрих-код другой пробирки? 
        // (это все к вопросу о том, что мы не можем остлеживать точно полный круг конвейера)
        private void searchTubeInConveyorCell(int attemptsNumber)
        {
            ConveyorCell cell = conveyor.Cells[conveyor.CellInScanPosition];

            Logger.Debug($"Запущен поиск пробирки (сканирование)");

            Analyzer.Conveyor.Shift(reverse: false);

            int attempt = 0;

            while (attempt < attemptsNumber) {
                Logger.Debug($"Попытка {attempt}.");

                Analyzer.Conveyor.RotateAndScanTube();

                string barcode = Analyzer.State.TubeBarcode;

                if (!string.IsNullOrWhiteSpace(barcode)) {
                    Logger.Debug($"Обнаружена пробирка со штрихкодом [{barcode}].");

                    cell.AnalysisBarcode = searchBarcodeInDatabase(barcode).BarCode; // TODO: - может вернуть null

                    if (!cell.IsEmpty) {
                        Logger.Debug($"Пробирка со штрихкодом [{barcode}] найдена в списке анализов!");
                        searchBarcodeInDatabase(cell.AnalysisBarcode).IsFind = true;
                    } else {
                        Logger.Debug($"Пробирка со штрихкодом [{barcode}] не найдена в списке анализов!");
                    }
                    break;
                } else {
                    Logger.Debug($"Пробирка не обнаружена.");
                }
                attempt++;
            }

            Logger.Debug($"Поиск пробирки (сканирование) завершен.");
        }

        private AnalysisInfo searchBarcodeInDatabase(string barcode)
        {
            // Значение по умолчанию для ссылочных и допускающих значения NULL типов равно null .
            return Options.Analyzes.Where(analysis => barcode.Contains(analysis.BarCode)).FirstOrDefault();
        }

        private void processAnalisysInitialStage(AnalysisInfo analysis)
        {
            Logger.Debug($"Анализ [{analysis.BarCode}] - запущено выполнение подготовительной стадии.");
            Logger.Debug($"Подготовка к забору материала из пробирки.");

            // Смещаем пробирку, чтобы она оказалась под иглой
            Analyzer.Conveyor.Shift(false, ConveyorUnit.ShiftType.HalfTube);

            // Поднимаем иглу вверх до дома
            Analyzer.Needle.HomeLifter();

            Logger.Debug($"Ожидание загрузки картриджа...");

            AnalyzerOperations.ChargeCartridge(cartirdgePosition: 0, chargePosition: 5);

            Logger.Debug($"Загрузка картриджа завершена.");
            Logger.Debug($"Ожидание касания жидкости в пробирке...");

            // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            Analyzer.Needle.TurnToTubeAndWaitTouch();

            Logger.Debug($"Забор материала из пробирки.");

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

            Logger.Debug($"Слив забранного материала в белую кювету.");

            // Сливаем материал в белую кювету
            Analyzer.Pomp.Push(0);

            // Промываем иглу
            AnalyzerOperations.NeedleWash();

            Logger.Debug($"Перенос реагента в белую кювету.");

            // Выполняем перенос реагента из нужной ячейки картриджа в белую кювету
            processAnalisysStage(analysis);

            // Устанавливаем иглу в домашнюю позицию
            Analyzer.Needle.HomeLifterAndRotator();

            // Смещаем пробирку обратно
            Analyzer.Conveyor.Shift(reverse: true, ConveyorUnit.ShiftType.HalfTube);

            Logger.Debug($"Анализ [{analysis.BarCode}] - завершено выполнение подготовительной стадии.");
        }

        private void processAnalisysStage(AnalysisInfo analysis)
        {
            Logger.Debug($"Анализ [{analysis.BarCode}] - запуск выполнения {analysis.CurrentStage}-й стадии.");

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

            Logger.Debug($"Анализ [{analysis.BarCode}] - завершено выполнение {analysis.CurrentStage}-й стадии.");
        }

        // TODO: Эта задача не реализована до конца!!!
        private void processAnalisysFinishStage(AnalysisInfo analysis)
        {
            Logger.Debug($"Анализ [{analysis.BarCode}] - запуск выполнения завершающей стадии.");

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

            Logger.Debug($"Анализ [{analysis.BarCode}] - выполнения завершающей стадии завершено.");
        }
    }
}
