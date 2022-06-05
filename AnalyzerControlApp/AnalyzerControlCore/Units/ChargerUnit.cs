using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerService.ExecutionControl;
using Infrastructure;
using System.Collections.Generic;

namespace AnalyzerService.Units
{
    public class ChargerUnit : UnitBase<ChargerConfiguration>
    {
        public int RotatorPosition { get; set; } = 0;
        public int HookPosition { get; set; } = 0;
        public int CurrentCell { get; private set; } = 0;

        public int hookCenterSensor = 4;

        public ChargerUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        { 

        }

        public void HomeRotator()
        {
            Logger.Debug($"[{nameof(ChargerUnit)}] - Start Rotator homing.");

            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, -Options.RotatorHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, -Options.RotatorHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            RotatorPosition = 0;

            executor.WaitExecution(commands);
            Logger.Debug($"[{nameof(ChargerUnit)}] - Rotator homing finished.");
        }

        /// <summary>
        /// Поворот к ячейке с кассетой
        /// </summary>
        /// <param name="cell">Номер ячейки</param>
        public void TurnToCell(int cell)
        {
            if (cell < 0)
                return;
            Logger.Debug($"[{nameof(ChargerUnit)}] - Start turn to cell[{cell}].");

            List<ICommand> commands = new List<ICommand>();

            CurrentCell = cell;

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, Options.RotatorSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, Options.RotatorStepsToCells[cell] - RotatorPosition } };
            commands.Add( new MoveCncCommand(steppers) );

            RotatorPosition = Options.RotatorStepsToCells[cell];

            executor.WaitExecution(commands);

            Logger.Debug($"[{nameof(ChargerUnit)}] - Turn to cell[{cell}] finished.");
        }

        public void TurnToDischarge()
        {
            Logger.Debug($"[{nameof(ChargerUnit)}] - Start turn to discharge.");

            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, Options.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, Options.RotatorStepsToUnload - RotatorPosition } };
            commands.Add(new MoveCncCommand(steppers));

            RotatorPosition = Options.RotatorStepsToUnload;

            executor.WaitExecution(commands);

            Logger.Debug($"[{nameof(ChargerUnit)}] - Turn to discharge finished.");
        }

        public void HomeHook(bool useChargeSpeed)
        {
            Logger.Debug($"[{nameof(ChargerUnit)}] - Start hook homing.");

            List<ICommand> commands = new List<ICommand>();

            int speed = useChargeSpeed ? Options.HookSpeedOnCharge : Options.HookHomeSpeed;

            steppers = new Dictionary<int, int>() { { Options.HookStepper, speed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Options.HookStepper, speed } };
            commands.Add( new HomeCncCommand(steppers) );

            executor.WaitExecution(commands);

            Logger.Debug($"[{nameof(ChargerUnit)}] - Hook homing finished.");
        }

        public void HookCenter( )
        {
            Logger.Debug($"[{nameof(ChargerUnit)}] - Start hook go to center.");

            List<ICommand> commands = new List<ICommand>();

            int speed = Options.HookHomeSpeed;

            steppers = new Dictionary<int, int>() { { Options.HookStepper, speed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.HookStepper, speed } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);

            Logger.Debug($"[{nameof(ChargerUnit)}] - Hook homing finished.");
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
            Logger.Debug($"[{nameof(ChargerUnit)}] - Start cartridge charging.");

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

            Logger.Debug($"[{nameof(ChargerUnit)}] - Cartridge charging finished.");
        }

        public void TurnScanner(bool underCartridge = false)
        {
            Logger.Debug($"[{nameof(ChargerUnit)}] - Запуск перемещения сканера.");

            List<ICommand> commands = new List<ICommand>();

            commands.Clear();

            // Поворот загрузки, чтобы чканер попал под картридж
            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, Options.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            int steps = underCartridge ? Options.RotatorStepsToOffsetAtScan : -Options.RotatorStepsToOffsetAtScan;

            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, steps } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            Logger.Debug($"[{nameof(ChargerUnit)}] - Перемещение сканера завершено.");
        }

        public void ScanBarcode()
        {
            Logger.Debug($"[{nameof(ChargerUnit)}] - Запуск сканирования кассеты.");
            List<ICommand> commands = new List<ICommand>();

            // Сканирование кассеты
            commands.Add(new ScanBarcodeCommand(scanner: BarcodeScanner.CartridgeScanner));

            executor.WaitExecution(commands);

            Logger.Debug($"[{nameof(ChargerUnit)}] - Сканирование кассеты завершено.");
        }

        public void DischargeCartridge()
        {
            Logger.Info($"[{nameof(ChargerUnit)}] - Start cartridge discharging.");

            List<ICommand> commands = new List<ICommand>();

            // Продвижение крюка до картриджа
            steppers = new Dictionary<int, int>() {
                { Options.HookStepper, Options.HookSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.HookStepper, Options.HookStepsToCartridge} };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            commands.Clear();

            //Отъезд загрузки, чтобы крюк мог пройти под картриджем
            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, Options.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.RotatorStepper, -Options.RotatorStepsToOffsetAtCharging } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            commands.Clear();



            Logger.Info($"[{nameof(ChargerUnit)}] - Cartridge discharging finished.");
        }
    }
}
