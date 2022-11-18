using AnalyzerConfiguration;
using AnalyzerControl.Configuration;
using AnalyzerControl.Services;
using AnalyzerDomain;
using AnalyzerDomain.Models;
using AnalyzerService;
using AnalyzerService.Units;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;

namespace AnalyzerControl
{
    public class AnalyzerDemoController : Configurable<DemoControllerConfiguration>
    {
        ConveyorService conveyor;
        RotorService rotor;

        private Stopwatch stopwatch;
        Timer timer;

        const int millisecondsInMinute = 60 * 1000;
        private const int timerInterval = 1000;
        private static object locker = new object();

        public enum States
        {
            Interrupted,
            AnalysisProcessing
        }

        public States state { get; private set; }

        bool interruptRequest = false;

        AnalyzesRepository analyzesRepository;
        private ObservableCollection<AnalysisDescription> analyzes;

        public AnalyzerDemoController(IConfigurationProvider provider, ConveyorService conveyor, RotorService rotor, AnalyzesRepository analyzesRepository) : base(provider)
        {
            this.conveyor = conveyor;
            this.rotor = rotor;
            this.analyzesRepository = analyzesRepository;

            initTimer();

            analyzes = analyzesRepository.Analyzes;

            state = States.Interrupted;
        }

        public void test()
        {
            analyzes[0].CurrentStage++;
        }

        private void initTimer()
        {
            stopwatch = new Stopwatch();
            timer = new Timer();

            timer.Interval = timerInterval;
            timer.Elapsed += timerElapsed;
        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds >= millisecondsInMinute) {
                stopwatch.Restart();

                Logger.Debug("Прошла минута");

                foreach (AnalysisDescription analysis in analyzes) {
                    lock (locker) {
                        if (analysis.IncubationStarted)
                        {
                            analysis.RemainingIncubationTime--;
                            // нужно уменьшить время инкубации и проверить, не закончилась ли она
                            if (analysis.RemainingIncubationTime == 0) {
                                analysis.IncubationStarted = false;
                                analysis.CurrentStage++;
                            }
                        }
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
                Analyzer.Needle.GoHome();
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
                mainCycle();
            });
        }

        private void mainCycle()
        {
            AnalyzerOperations.MoveAllToHome();
            AnalyzerOperations.WashNeedle();

            Analyzer.Needle.GoHome();

            while(!interruptRequest)
                processAnalyzes2();

            if(interruptRequest) {
                state = States.Interrupted;
                interruptRequest = false;
                Logger.Debug($"Прерывание работы выполнено.");
            } else {
                Logger.Debug($"Все пробирки обработаны!"); // Точно? Ты уверен?
            }
        }

        private void processAnalyzes2()
        {
            foreach(AnalysisDescription analysis in analyzes)
            {
                if(analysis.CurrentStage == -1) {
                    // необходим забор материала
                    conveyor.PlaceCellInSamplePosition(analysis.ConveyorPosition);

                    sampleAnalysis(analysis);

                    analysis.CurrentStage = 1;
                    analysis.IncubationStarted = true;
                    analysis.RemainingIncubationTime = analysis.Inc1Duration;
                } else if(analysis.CurrentStage == 2) {
                    stage2Analysis(analysis);

                    analysis.CurrentStage = 3;
                    analysis.IncubationStarted = true;
                    analysis.RemainingIncubationTime = analysis.inc2Duration;
                } else if(analysis.CurrentStage == 4) {
                    finishAnalysis(analysis);

                    analysis.CurrentStage = 5;
                    analysis.IsCompleted = true;
                }
            }
        }

        private void sampleAnalysis(AnalysisDescription analysis)
        {
            // забираем материал из пробирки
            Analyzer.Needle.HomeLifter(); // Поднимаем иглу вверх до дома
            Analyzer.Needle.GoHome();
            Analyzer.Needle.TurnToTubeAndWaitTouch(); // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке

            Analyzer.Pomp.Pull(analysis.SampleVolume);

            // забираем из TW2
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(analysis.RotorPosition, CartridgeWell.W2, RotorUnit.CellPosition.CellLeft);

            Analyzer.Needle.HomeLifter();
            Analyzer.Needle.TurnToCartridge(CartridgeWell.W2);
            Analyzer.Needle.PerforateCartridge(CartridgeWell.W2);

            Analyzer.Pomp.Pull(analysis.Tw2Volume);

            Analyzer.Needle.HomeLifter();

            // выливаем в TACW
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(analysis.RotorPosition, CartridgeWell.ACW);

            Analyzer.Needle.HomeLifter();
            Analyzer.Needle.TurnToCartridge(CartridgeWell.ACW);
            Analyzer.Needle.PerforateCartridge(CartridgeWell.ACW);

            Analyzer.Pomp.Push(analysis.SampleVolume + analysis.Tw2Volume);

            Analyzer.Needle.HomeLifter();

            AnalyzerOperations.WashNeedle2();
        }

        void stage2Analysis(AnalysisDescription analysis)
        {
            // Перед переливанием из TW3 в TACW нужно промыть TACW в wash buffer
            washTacw(analysis);

            // забираем из TW3
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(analysis.RotorPosition, CartridgeWell.W3, RotorUnit.CellPosition.CellLeft);

            Analyzer.Needle.HomeLifter();
            Analyzer.Needle.TurnToCartridge(CartridgeWell.W3);
            Analyzer.Needle.PerforateCartridge(CartridgeWell.W3);

            Analyzer.Pomp.Pull(analysis.Tw3Volume);

            Analyzer.Needle.HomeLifter();

            // выливаем в TACW
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(analysis.RotorPosition, CartridgeWell.ACW);

            Analyzer.Needle.HomeLifter();
            Analyzer.Needle.TurnToCartridge(CartridgeWell.ACW);
            Analyzer.Needle.PerforateCartridge(CartridgeWell.ACW);

            Analyzer.Pomp.Push(analysis.Tw3Volume);

            Analyzer.Needle.HomeLifter();

            AnalyzerOperations.WashNeedle2();
        }

        private void washTacw(AnalysisDescription analysis)
        {
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderWashBuffer(analysis.RotorPosition);

            // Здесь нужно включить wash-буффер
            Analyzer.AdditionalDevices.HomeWashBuffer();
            // 1) опустить wash-буффер до высоты TACW
            Analyzer.AdditionalDevices.PutDownWashBuffer();
            // 2) запустить процесс промывки TACW
            AnalyzerOperations.WashTacw();
            // 3) поднять wash-буффер
            Analyzer.AdditionalDevices.HomeWashBuffer();
        }

        void finishAnalysis(AnalysisDescription analysis)
        {
            // забираем из TACW
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(analysis.RotorPosition, CartridgeWell.ACW);

            Analyzer.Needle.HomeLifter();
            Analyzer.Needle.TurnToCartridge(CartridgeWell.ACW);
            Analyzer.Needle.PerforateCartridge(CartridgeWell.ACW);

            Analyzer.Pomp.Pull(analysis.TacwVolume);

            Analyzer.Needle.HomeLifter();

            // Замер TCUV без материала

            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellAtOM(analysis.RotorPosition);
            analysis.OM1Value = Analyzer.State.SensorsValues[15];

            // выливаем в TCUV
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellUnderNeedle(analysis.RotorPosition, CartridgeWell.CUV);

            Analyzer.Needle.HomeLifter();
            Analyzer.Needle.TurnToCartridge(CartridgeWell.CUV);
            Analyzer.Needle.PerforateCartridge(CartridgeWell.CUV);

            Analyzer.Pomp.Push(analysis.TacwVolume);

            Analyzer.Needle.HomeLifter();

            // Замер TCUV с материалом

            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellAtOM(analysis.RotorPosition);
            analysis.OM2Value = Analyzer.State.SensorsValues[15];

            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellAtDischarge(analysis.RotorPosition);

            Analyzer.Charger.HomeRotator();
            Analyzer.Charger.TurnToDischarge();
            Analyzer.Charger.DischargeCartridge();
            Analyzer.Charger.HomeHook(false);
            Analyzer.Charger.MoveHookAfterHome();

            AnalyzerOperations.WashNeedle2();
        }
    }
}
