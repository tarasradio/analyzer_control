using AnalyzerControl.Services;
using AnalyzerDomain.Models;
using AnalyzerService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace AnalyzerControl
{
    public class Assay
    {
        public int Id { get; set; } = 0;

        public AssayDescription Description = null;

        public int CurrentStep = -1;
        public string patientID = string.Empty;
        public int TestCartridgePositionInRotor = 0;

        public int measured = 0; // Число произведенных измерений
        public double[] OM = new double[2];
        public double result = 0;

        public bool incubationStarted = false;
        public int RemainingIncubationTime = 0;

        public AssayStep GetCurrentStep()
        {
            return Description.Steps[CurrentStep];
        }
    }

    public class AssaysProcessingController
    {
        ConveyorService conveyor;
        RotorService rotor;
        CartridgesDeckService cartridgesDeck;

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

        public List<Assay> Assays = new List<Assay>();

        public AssaysProcessingController(ConveyorService conveyor, RotorService rotor, CartridgesDeckService cartridgesDeck)
        {
            this.conveyor = conveyor;
            this.rotor = rotor;
            this.cartridgesDeck = cartridgesDeck;

            initTimer();

            state = States.Interrupted;
        }

        public void StartWork()
        {
            state = States.AnalysisProcessing;

            timer.Start();
            stopwatch.Reset();
            stopwatch.Start();

            startWorkCycle();
        }

        public void TerminateWork()
        {
            if (timer.Enabled) timer.Stop();
            if (stopwatch.IsRunning) stopwatch.Stop();
        }

        public void InterruptWork()
        {
            if (state == States.AnalysisProcessing)
            {
                interruptRequest = true;

                while (state != States.Interrupted) ;

                // Выполнение действий после прерывания
                Analyzer.Needle.GoHome();
            }
        }

        public void ResumeWork()
        {
            state = States.AnalysisProcessing;

            startWorkCycle();
        }

        private void initTimer()
        {
            stopwatch = new Stopwatch();
            timer = new Timer
            {
                Interval = timerInterval
            };
            timer.Elapsed += timerElapsed;
        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds >= millisecondsInMinute)
            {
                stopwatch.Restart();

                foreach (Assay assay in Assays)
                {
                    lock (locker)
                    {
                        if(assay.GetCurrentStep().StepType == AssayStepsTypes.INC)
                        {
                            assay.RemainingIncubationTime -= 1;
                        }
                    }
                }
            }
        }

        private void startWorkCycle()
        {
            Analyzer.TaskExecutor.StartTask(() =>
            {
                ProcessAssaysCycle();
            });
        }

        private void ProcessAssaysCycle()
        {
            AnalyzerOperations.MoveAllToHome();

            while(true)
            {
                foreach (Assay assay in Assays)
                {
                    ProcessAssay(assay);
                }
            }
        }

        public void ProcessAssay(Assay assay)
        {
            AssayStep currentStep = assay.GetCurrentStep();

            if(currentStep.StepType == AssayStepsTypes.INC) {
                Process_INC(assay, currentStep);
            } else if(currentStep.StepType == AssayStepsTypes.OM) {
                Process_OM(assay, currentStep);
            } else if(currentStep.StepType == AssayStepsTypes.CALC) {
                Process_CALC(assay, currentStep);
            } else if(currentStep.StepType == AssayStepsTypes.ASP) {
                Process_ASP(assay, currentStep);
            } else if(currentStep.StepType == AssayStepsTypes.DISP) {
                Process_DISP(assay, currentStep);
            }
        }

        public void Process_F_NCS_PERGEWASH(Assay assay, AssayStep step)
        {
            assay.CurrentStep++;
        }

        public void Process_ASP(Assay assay, AssayStep data)
        {
            // Сначала нужно поместить иглу в нужное место
            if (data.SourceWell == 7)
            {
                // забираем материал из иглы
                Analyzer.Needle.HomeLifter(); // Поднимаем иглу вверх до дома
                Analyzer.Needle.TurnToTubeAndWaitTouch(); // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            }
            else if (data.SourceWell == 2)
            {
                // забираем из TW2
                Analyzer.Rotor.Home();
                Analyzer.Rotor.PlaceCellUnderNeedle(assay.TestCartridgePositionInRotor, CartridgeWell.W2);

                Analyzer.Needle.HomeLifter();
                Analyzer.Needle.TurnToCartridge(CartridgeWell.W2);
                Analyzer.Needle.PerforateCartridge(CartridgeWell.W2);
            }
            else if (data.SourceWell == 3)
            {
                // забираем из TW3
                Analyzer.Rotor.Home();
                Analyzer.Rotor.PlaceCellUnderNeedle(assay.TestCartridgePositionInRotor, CartridgeWell.W3);

                Analyzer.Needle.HomeLifter();
                Analyzer.Needle.TurnToCartridge(CartridgeWell.W3);
                Analyzer.Needle.PerforateCartridge(CartridgeWell.W3);

            } else if(data.SourceWell == 4) {
                // забираем из TACW
                Analyzer.Rotor.Home();
                Analyzer.Rotor.PlaceCellUnderNeedle(assay.TestCartridgePositionInRotor, CartridgeWell.ACW);

                Analyzer.Needle.HomeLifter();
                Analyzer.Needle.TurnToCartridge(CartridgeWell.ACW);
                Analyzer.Needle.PerforateCartridge(CartridgeWell.ACW);
            }

            // Затем нужно штрицем взять нужный объем
            Analyzer.Pomp.Pull(data.Quantity); // TODO: Заменить на требуемое и разобраться с объемом и шагами!!!

            assay.CurrentStep++;
        }

        public void Process_AWER(Assay assay, AssayStep data)
        {
            assay.CurrentStep++;
        }

        public void Process_DISPMIX(Assay assay, AssayStep data)
        {
            assay.CurrentStep++;
        }

        public void Process_NCS_PURGEWASH(Assay assay, AssayStep data)
        {
            assay.CurrentStep++;
        }

        public void Process_INC(Assay assay, AssayStep data)
        {
            if (assay.incubationStarted) {
                // нужно уменьшить время инкубации и проверить, не закончилась ли она
                if (assay.RemainingIncubationTime == 0)
                {
                    assay.incubationStarted = false;
                    assay.CurrentStep++;
                }
            } else {
                assay.incubationStarted = true;
                assay.RemainingIncubationTime = data.Duration;
            }
        }

        public void Process_F_PURGEWASH(Assay assay, AssayStep data)
        {
            assay.CurrentStep++;
        }

        public void Process_WA(Assay assay, AssayStep data)
        {
            assay.CurrentStep++;
        }

        public void Process_DISP(Assay assay, AssayStep data)
        {
            if (data.SourceWell == 2) {
                // выливаем в TW2
                Analyzer.Rotor.Home();
                Analyzer.Rotor.PlaceCellUnderNeedle(assay.TestCartridgePositionInRotor, CartridgeWell.W2);

                Analyzer.Needle.HomeLifter();
                Analyzer.Needle.TurnToCartridge(CartridgeWell.W2);
                Analyzer.Needle.PerforateCartridge(CartridgeWell.W2);
            } else if (data.SourceWell == 3) {
                // выливаем в TW3
                Analyzer.Rotor.Home();
                Analyzer.Rotor.PlaceCellUnderNeedle(assay.TestCartridgePositionInRotor, CartridgeWell.W3);

                Analyzer.Needle.HomeLifter();
                Analyzer.Needle.TurnToCartridge(CartridgeWell.W3);
                Analyzer.Needle.PerforateCartridge(CartridgeWell.W3);

            } else if (data.SourceWell == 4) {
                // выливаем в TACW
                Analyzer.Rotor.Home();
                Analyzer.Rotor.PlaceCellUnderNeedle(assay.TestCartridgePositionInRotor, CartridgeWell.ACW);

                Analyzer.Needle.HomeLifter();
                Analyzer.Needle.TurnToCartridge(CartridgeWell.ACW);
                Analyzer.Needle.PerforateCartridge(CartridgeWell.ACW);
            } else if(data.TargetWell == 5) {
                // выливаем в CUV
                Analyzer.Rotor.Home();
                Analyzer.Rotor.PlaceCellUnderNeedle(assay.TestCartridgePositionInRotor, CartridgeWell.CUV);

                Analyzer.Needle.HomeLifter();
                Analyzer.Needle.TurnToCartridge(CartridgeWell.CUV);
                Analyzer.Needle.PerforateCartridge(CartridgeWell.CUV);
            }

            // Затем нужно штрицем взять нужный объем
            Analyzer.Pomp.Push(data.Quantity); // TODO: Заменить на требуемое и разобраться с объемом и шагами!!!

            assay.CurrentStep++;
        }

        public void Process_PURGEWASH(Assay assay, AssayStep data)
        {
            assay.CurrentStep++;
        }

        public void Process_OM(Assay assay, AssayStep data)
        {
            // TODO: Добавить реализацию
            assay.OM[assay.measured++] = 10; // TODO: Здесь нужно выполнить измерение!!!

            assay.CurrentStep++;
        }

        public void Process_CALC(Assay assay, AssayStep data)
        {
            assay.result = assay.OM[1] - assay.OM[0]; // TODO: Исправить на правильное измерение!!!

            assay.CurrentStep++;
        }
    }
}
