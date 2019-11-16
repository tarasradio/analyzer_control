using System.Collections.Generic;
using System.IO;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.Utils;

using SteppersControlCore.ControllersProperties;
using SteppersControlCore.Interfaces;

namespace SteppersControlCore.Controllers
{
    public class ChargeController : ControllerBase
    {
        const string filename = "ChargeControllerProps";

        public ChargeControllerProperties Properties { get; set; }

        public int RotatorPosition { get; set; } = 0;
        public int HookPosition { get; set; } = 0;

        public int CurrentCell { get; private set; } = 0;

        public ChargeController(ICommandExecutor executor) : base(executor)
        {
            Properties = new ChargeControllerProperties();
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<ChargeControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename) );
        }

        //Чтение насроек из файла
        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<ChargeControllerProperties>.ReadXML(
                Path.Combine(path, filename));
            if (Properties == null)
                Properties = new ChargeControllerProperties();
        }

        public void HomeRotator()
        {
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, Properties.RotatorHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, Properties.RotatorHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            RotatorPosition = 0;

            executor.WaitExecution(commands);
        }

        public void TurnToCell(int cell)
        {
            List<ICommand> commands = new List<ICommand>();

            CurrentCell = cell;

            steppers = new Dictionary<int, int>() {
                { Properties.RotatorStepper, 30 } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Properties.RotatorStepper, Properties.CellsSteps[cell] - RotatorPosition } };
            commands.Add( new MoveCncCommand(steppers) );

            RotatorPosition = Properties.CellsSteps[cell];

            executor.WaitExecution(commands);
        }

        public void HomeHook()
        {
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.HookStepper, Properties.HookHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Properties.HookStepper, Properties.HookHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            executor.WaitExecution(commands);
        }

        public void ChargeCartridge()
        {
            List<ICommand> commands = new List<ICommand>();

            //Отъезд загрузки, чтобы крюк мог пройти под картриджем
            steppers = new Dictionary<int, int>() {
                { Properties.RotatorStepper, Properties.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Properties.RotatorStepper, -Properties.RotatorStepsLeaveAtCharge } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            // Продвижение крюка до картриджа
            steppers = new Dictionary<int, int>() {
                { Properties.HookStepper, Properties.HookSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Properties.HookStepper, Properties.HookStepsToCartridge } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            // Возврат загрузки, чтобы крюк захватил картридж
            steppers = new Dictionary<int, int>() {
                { Properties.RotatorStepper, Properties.RotatorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Properties.RotatorStepper, Properties.RotatorStepsLeaveAtCharge } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }
    }
}
