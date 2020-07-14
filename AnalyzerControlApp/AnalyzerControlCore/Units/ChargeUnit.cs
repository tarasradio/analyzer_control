using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.ControllersConfiguration;
using AnalyzerControlCore.MachineControl;
using Infrastructure;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace AnalyzerControlCore.Units
{
    public class ChargeUnit : AbstractUnit
    {
        public ChargeControllerConfiguration Config { get; set; }
        private IConfigurationProvider<ChargeControllerConfiguration> provider;

        public int RotatorPosition { get; set; } = 0;
        public int HookPosition { get; set; } = 0;

        public int CurrentCell { get; private set; } = 0;

        public void SetProvider(IConfigurationProvider<ChargeControllerConfiguration> provider)
        {
            this.provider = provider;
        }

        public ChargeUnit(ICommandExecutor executor) : base(executor)
        {
            Config = new ChargeControllerConfiguration();
        }

        public void SaveConfiguration(string path)
        {
            provider.SaveConfiguration(Config, Path.Combine(path, nameof(ChargeControllerConfiguration)));

            //XmlConfigurationProvider<ChargeControllerConfiguration>.SaveConfiguration( Config, Path.Combine(path, nameof(ChargeControllerConfiguration)) );
        }

        public void LoadConfiguration(string path)
        {
            Config = provider.LoadConfiguration( Path.Combine(path, nameof(ChargeControllerConfiguration)) ); 
            
            //XmlConfigurationProvider<ChargeControllerConfiguration>.LoadConfiguration( Path.Combine(path, nameof(ChargeControllerConfiguration)) );
            
            if (Config == null)
                Config = new ChargeControllerConfiguration();
        }

        public void HomeRotator()
        {
            Logger.ControllerInfo($"[{nameof(ChargeUnit)}] - Start Rotator homing.");

            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Config.RotatorStepper, Config.RotatorHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Config.RotatorStepper, Config.RotatorHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            RotatorPosition = 0;

            executor.WaitExecution(commands);
            Logger.ControllerInfo("[Charger] - Rotator homing finished.");
        }

        public void TurnToCell(int cell)
        {
            Logger.ControllerInfo($"[Charger] - Start turn to cell[{cell}].");

            List<ICommand> commands = new List<ICommand>();

            CurrentCell = cell;

            steppers = new Dictionary<int, int>() {
                { Config.RotatorStepper, 30 } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Config.RotatorStepper, Config.CellsSteps[cell] - RotatorPosition } };
            commands.Add( new MoveCncCommand(steppers) );

            RotatorPosition = Config.CellsSteps[cell];

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Charger] - Turn to cell[{cell}] finished.");
        }

        public void HomeHook()
        {
            Logger.ControllerInfo($"[Charger] - Start hook homing.");
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Config.HookStepper, Config.HookHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Config.HookStepper, Config.HookHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Charger] - Hook homing finished.");
        }

        public void HookAfterHome()
        {
            List<ICommand> commands = new List<ICommand>();

            // Продвижение крюка до картриджа
            steppers = new Dictionary<int, int>() {
                { Config.HookStepper, Config.HookSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Config.HookStepper, Config.HookStepsAfterHome } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            commands.Clear();
        }

        public void ChargeCartridge()
        {
            Logger.ControllerInfo($"[Charger] - Start cartridge charging.");
            List<ICommand> commands = new List<ICommand>();

            //Отъезд загрузки, чтобы крюк мог пройти под картриджем
            steppers = new Dictionary<int, int>() {
                { Config.RotatorStepper, Config.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Config.RotatorStepper, -Config.RotatorStepsLeaveAtCharge } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            commands.Clear();

            // Продвижение крюка до картриджа
            steppers = new Dictionary<int, int>() {
                { Config.HookStepper, Config.HookSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Config.HookStepper, Config.HookStepsToCartridge } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            commands.Clear();

            // Возврат загрузки, чтобы крюк захватил картридж
            steppers = new Dictionary<int, int>() {
                { Config.RotatorStepper, Config.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Config.RotatorStepper, Config.RotatorStepsLeaveAtCharge } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Charger] - Cartridge charging finished.");
        }
    }
}
