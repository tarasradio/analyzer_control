using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerControlCore.MachineControl;
using Infrastructure;
using System.Collections.Generic;

namespace AnalyzerControlCore.Units
{
    public class ChargerUnit : UnitBase<ChargerConfiguration>
    {
        public int RotatorPosition { get; set; } = 0;
        public int HookPosition { get; set; } = 0;
        public int CurrentCell { get; private set; } = 0;

        public ChargerUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        { 

        }

        public void HomeRotator()
        {
            Logger.ControllerInfo($"[{nameof(ChargerUnit)}] - Start Rotator homing.");

            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, Options.RotatorHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, Options.RotatorHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            RotatorPosition = 0;

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[{nameof(ChargerUnit)}] - Rotator homing finished.");
        }

        public void TurnToCell(int cell)
        {
            Logger.ControllerInfo($"[{nameof(ChargerUnit)}] - Start turn to cell[{cell}].");

            List<ICommand> commands = new List<ICommand>();

            CurrentCell = cell;

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, 30 } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, Options.RotatorStepsToCells[cell] - RotatorPosition } };
            commands.Add( new MoveCncCommand(steppers) );

            RotatorPosition = Options.RotatorStepsToCells[cell];

            executor.WaitExecution(commands);

            Logger.ControllerInfo($"[{nameof(ChargerUnit)}] - Turn to cell[{cell}] finished.");
        }

        public void HomeHook()
        {
            Logger.ControllerInfo($"[{nameof(ChargerUnit)}] - Start hook homing.");

            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Options.HookStepper, Options.HookHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Options.HookStepper, Options.HookHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            executor.WaitExecution(commands);

            Logger.ControllerInfo($"[{nameof(ChargerUnit)}] - Hook homing finished.");
        }

        public void MoveHookAfterHome()
        {
            List<ICommand> commands = new List<ICommand>();

            // Продвижение крюка до картриджа
            steppers = new Dictionary<int, int>() {
                { Options.HookStepper, Options.HookSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.HookStepper, Options.HookStepsToOffsetAfterHoming } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            commands.Clear();
        }

        public void ChargeCartridge()
        {
            Logger.ControllerInfo($"[{nameof(ChargerUnit)}] - Start cartridge charging.");

            List<ICommand> commands = new List<ICommand>();

            //Отъезд загрузки, чтобы крюк мог пройти под картриджем
            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, Options.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, -Options.RotatorStepsToOffsetAtCharging } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            commands.Clear();

            // Продвижение крюка до картриджа
            steppers = new Dictionary<int, int>() {
                { Options.HookStepper, Options.HookSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.HookStepper, Options.HookStepsToCartridge } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            commands.Clear();

            // Возврат загрузки, чтобы крюк захватил картридж
            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, Options.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, Options.RotatorStepsToOffsetAtCharging } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            Logger.ControllerInfo($"[{nameof(ChargerUnit)}] - Cartridge charging finished.");
        }
    }
}
