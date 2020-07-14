using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.ControllersConfiguration;
using AnalyzerControlCore.MachineControl;
using Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace AnalyzerControlCore.Units
{
    //TODO: а тут видимо вообще нахер не надо отслеживание
    public class TransporterUnit : AbstractUnit
    {
        public TransporterControllerConfiguration Config { get; set; }
        private IConfigurationProvider<TransporterControllerConfiguration> provider;

        public int TransporterStepperPosition { get; set; } = 0;

        public TransporterUnit(ICommandExecutor executor) : base(executor)
        {
            Config = new TransporterControllerConfiguration();
        }

        public void SetProvider(IConfigurationProvider<TransporterControllerConfiguration> provider)
        {
            this.provider = provider;
        }

        public void SaveConfiguration(string path)
        {
            provider.SaveConfiguration(Config, Path.Combine(path, nameof(TransporterControllerConfiguration)) );
        }

        //Чтение насроек из файла
        public void LoadConfiguration(string path)
        {
            Config = provider.LoadConfiguration( Path.Combine(path, nameof(TransporterControllerConfiguration)) );

            if (Config == null)
                Config = new TransporterControllerConfiguration();
        }

        public void PrepareBeforeScanning()
        {
            Logger.ControllerInfo($"[Transporter] - Start prepare before scanning.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Config.TransporterStepper, (uint)Config.TransporterMoveSpeed));

            // Обнуление ленты конвеера
            steppers = new Dictionary<int, int>() { { Config.TransporterStepper, Config.TransporterHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            // Сдвиг на пол пробирки
            steppers = new Dictionary<int, int>() { { Config.TransporterStepper, Config.StepsPerTube / 2 } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Transporter] - Prepare before scanning finished.");
        }

        public enum ShiftType
        {
            HalfTube,
            OneTube
        };

        // Сдвиг
        public void Shift(bool reverse, ShiftType shiftType = ShiftType.OneTube)
        {
            Logger.ControllerInfo($"[Transporter] - Start shift.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Config.TransporterStepper, (uint)Config.TransporterMoveSpeed));

            int steps = Config.StepsPerTube;

            if (shiftType == ShiftType.HalfTube) steps /= 2;
            if (reverse) steps *= -1;

            steppers = new Dictionary<int, int>() { { Config.TransporterStepper, steps } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Transporter] - Shift finished.");
        }

        public void RotateAndScanTube()
        {
            Logger.ControllerInfo($"[Transporter] - Start rotating and scanning tube.");
            List<ICommand> commands = new List<ICommand>();
            
            // Сканирование пробирки
            commands.Add(new BarStartCommand());

            // Вращение пробирки
            commands.Add(new SetSpeedCommand(Config.TurnTubeStepper, (uint)Config.TurnTubeSpeed));

            steppers = new Dictionary<int, int>() { { Config.TurnTubeStepper, Config.StepsPerTubeRotate } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Transporter] - Rotating and scanning tube finished.");
        }
    }
}
