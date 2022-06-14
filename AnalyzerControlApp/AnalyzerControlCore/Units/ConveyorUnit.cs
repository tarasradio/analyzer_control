using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerService.ExecutionControl;
using Infrastructure;
using System.Collections.Generic;

namespace AnalyzerService.Units
{
    //TODO: а тут видимо вообще нахер не надо отслеживание
    public class ConveyorUnit : UnitBase<ConveyorConfiguration>
    {
        public int ConveyorStepperPosition { get; set; } = 0;

        public ConveyorUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        {

        }

        public void PrepareBeforeScanning()
        {
            Logger.Debug($"[{nameof(ConveyorUnit)}] - Start prepare before scanning.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Options.ConveyorStepper, (uint)Options.ConveyorSpeed));

            // Обнуление ленты конвеера
            steppers = new Dictionary<int, int>() { { Options.ConveyorStepper, Options.ConveyorSpeedAtHoming } };
            commands.Add(new HomeCncCommand(steppers));

            // Сдвиг на пол пробирки
            steppers = new Dictionary<int, int>() { { Options.ConveyorStepper, Options.ConveyorStepsPerSingleTube / 2 } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.Debug($"[{nameof(ConveyorUnit)}] - Prepare before scanning finished.");
        }

        public enum ShiftType
        {
            HalfTube,
            OneTube
        };

        public void Shift(bool reverse, ShiftType shiftType = ShiftType.OneTube)
        {
            Logger.Debug($"[{nameof(ConveyorUnit)}] - Start shift.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Options.ConveyorStepper, (uint)Options.ConveyorSpeed));

            int steps = Options.ConveyorStepsPerSingleTube;

            if (shiftType == ShiftType.HalfTube) steps /= 2;
            if (reverse) steps *= -1;

            steppers = new Dictionary<int, int>() { { Options.ConveyorStepper, steps } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.Debug($"[{nameof(ConveyorUnit)}] - Shift finished.");
        }

        public void RotateAndScanTube()
        {
            Logger.Debug($"[{nameof(ConveyorUnit)}] - Start rotating and scanning tube.");
            List<ICommand> commands = new List<ICommand>();
            
            // Сканирование пробирки
            commands.Add(new ScanBarcodeCommand(scanner: BarcodeScanner.TubeScanner));

            // Вращение пробирки
            commands.Add(new SetSpeedCommand(Options.TubeRotatorStepper, (uint)Options.TubeRotatorSpeed));

            steppers = new Dictionary<int, int>() { { Options.TubeRotatorStepper, Options.RotatorStepsPerTubeRotate } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.Debug($"[{nameof(ConveyorUnit)}] - Rotating and scanning tube finished.");
        }

        public void Shift2(int cellsCount)
        {
            for (int i = 0; i < cellsCount; i++)
            {
                Shift2();
            }
        }

        public void Shift2()
        {
            List<ICommand> commands = new List<ICommand>();

            if(Analyzer.State.SensorsValues[3] >= 500)
            {
                steppers = new Dictionary<int, int>() { { Options.ConveyorStepper, Options.ConveyorSpeed } };
                commands.Add(new RunCncCommand(steppers, 3, 500, ValueEdge.FallingEdge));
            }

            steppers = new Dictionary<int, int>() { { Options.ConveyorStepper, Options.ConveyorSpeed } };
            commands.Add(new RunCncCommand(steppers, 3, 500, ValueEdge.RisingEdge));

            executor.WaitExecution(commands);
        }
    }
}
