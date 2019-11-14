using System.Collections.Generic;
using System.IO;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.Utils;

using SteppersControlCore.ControllersProperties;
using SteppersControlCore.Interfaces;

namespace SteppersControlCore.Controllers
{
    public class LoadController : ControllerBase
    {
        const string filename = "LoadControllerProps";

        public LoadControllerProperties Properties { get; set; }

        public int LoadStepperPosition { get; set; } = 0;
        public int ShuttleStepperPosition { get; set; } = 0;

        public int CurrentCell { get; private set; } = 0;

        public LoadController(ICommandExecutor executor) : base(executor)
        {
            Properties = new LoadControllerProperties();
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<LoadControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename) );
        }

        //Чтение насроек из файла
        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<LoadControllerProperties>.ReadXML(
                Path.Combine(path, filename));
            if (Properties == null)
                Properties = new LoadControllerProperties();
        }

        public void HomeLoad()
        {
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.LoadStepper, Properties.LoadStepperHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Properties.LoadStepper, Properties.LoadStepperHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            LoadStepperPosition = 0;

            executor.WaitExecution(commands);
        }

        public void TurnLoadToCell(int cell)
        {
            List<ICommand> commands = new List<ICommand>();

            CurrentCell = cell;

            steppers = new Dictionary<int, int>() {
                { Properties.LoadStepper, 30 } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Properties.LoadStepper, Properties.CellsSteps[cell] - LoadStepperPosition } };
            commands.Add( new MoveCncCommand(steppers) );

            LoadStepperPosition = Properties.CellsSteps[cell];

            executor.WaitExecution(commands);
        }

        public void HomeShuttle()
        {
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.ShuttleStepper, Properties.ShuttleStepperHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Properties.ShuttleStepper, Properties.ShuttleStepperHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            executor.WaitExecution(commands);
        }

        public void LoadCartridge()
        {
            List<ICommand> commands = new List<ICommand>();

            //Отъезд загрузки, чтобы крюк мог пройти под картриджем
            steppers = new Dictionary<int, int>() {
                { Properties.LoadStepper, Properties.LoadStepperSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Properties.LoadStepper, -Properties.StepsTurnToLoad } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            // Продвижение крюка до картриджа
            steppers = new Dictionary<int, int>() {
                { Properties.ShuttleStepper, Properties.ShuttleStepperSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Properties.ShuttleStepper, Properties.StepsShuttleToCartridge } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            // Возврат загрузки, чтобы крюк захватил картридж
            steppers = new Dictionary<int, int>() {
                { Properties.LoadStepper, Properties.LoadStepperSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Properties.LoadStepper, Properties.StepsTurnToLoad } };

            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }
    }
}
