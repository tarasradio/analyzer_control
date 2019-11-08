using System.Collections.Generic;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.Utils;

using SteppersControlCore.ControllersProperties;

namespace SteppersControlCore.Controllers
{
    public class LoadController : ControllerBase
    {
        const string filename = "LoadControllerProps";

        public LoadControllerProperties Properties { get; set; }

        public int LoadStepperPosition { get; set; } = 0;
        public int ShuttleStepperPosition { get; set; } = 0;
        public int PushStepperPosition { get; set; } = 0;

        public int CurrentCell { get; private set; } = 0;

        public LoadController() : base()
        {
            Properties = new LoadControllerProperties();
        }

        public void WriteXml()
        {
            XMLSerializeHelper<LoadControllerProperties>.WriteXml(Properties, filename);
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            Properties = XMLSerializeHelper<LoadControllerProperties>.ReadXML(filename);
        }

        public List<IAbstractCommand> HomeLoad()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Properties.LoadStepper, Properties.LoadStepperHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Properties.LoadStepper, Properties.LoadStepperHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            LoadStepperPosition = 0;

            return commands;
        }


        // Calculation of necessary steps using the target and current position (in steps)
        // The target position is determined by the absolute number of steps from the starting position (0)
        // General rule: needed steps = taget_position - current_position
        // Example 1: position = 3000; target position = 2000; necessary steps = 2000 - 3000 = -1000
        // Example 2: position = 2000; target position = 3000; necessary steps = 3000 - 2000 = 1000
        // Example 3: position = -3000; target position = -2000; necessary steps = -2000 - (-3000) = 1000
        // Example 4: position = -2000; target position = -3000; necessary steps = -3000 - (-2000) = -1000

        public List<IAbstractCommand> TurnLoadToCell(int cell)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            CurrentCell = cell;

            steppers = new Dictionary<int, int>() {
                { Properties.LoadStepper, 30 } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Properties.LoadStepper, Properties.CellsSteps[cell] - LoadStepperPosition } };
            commands.Add( new MoveCncCommand(steppers) );

            LoadStepperPosition = Properties.CellsSteps[cell];

            return commands;
        }

        public List<IAbstractCommand> HomeShuttle()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Properties.ShuttleStepper, Properties.ShuttleStepperHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Properties.ShuttleStepper, Properties.ShuttleStepperHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Properties.ShuttleStepper, Properties.StepsShuttleToStart } };
            commands.Add( new MoveCncCommand(steppers) );

            ShuttleStepperPosition = Properties.StepsShuttleToStart;

            return commands;
        }

        public List<IAbstractCommand> MoveShuttleToCassette()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Properties.ShuttleStepper, Properties.ShuttleStepperSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Properties.ShuttleStepper, Properties.StepsShuttleToCassette } };
            commands.Add( new MoveCncCommand(steppers) );

            return commands;
        }
    }
}
