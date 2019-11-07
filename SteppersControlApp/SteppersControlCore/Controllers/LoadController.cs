using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.Utils;

namespace SteppersControlCore.Controllers
{
    public class LoadControllerPropetries
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель поворота загрузки")]
        public int LoadStepper { get; set; } = 10;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель челнока")]
        public int ShuttleStepper { get; set; } = 15;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель толкателя")]
        public int PushStepper { get; set; } = 0;
        
        [Category("2.1 Скорость двигателя поворота загрузки")]
        [DisplayName("Скорость движения загрузки домой")]
        public int LoadStepperHomeSpeed { get; set; } = 50;
        [Category("2.1 Скорость двигателя поворота загрузки")]
        [DisplayName("Скорость движения загрузки")]
        public int LoadStepperSpeed { get; set; } = 50;

        [Category("2.2 Скорость двигателя челнока")]
        [DisplayName("Скорость движения челнока домой")]
        public int ShuttleStepperHomeSpeed { get; set; } = 500;
        [Category("2.2 Скорость двигателя челнока")]
        [DisplayName("Скорость движения челнока")]
        public int ShuttleStepperSpeed { get; set; } = 200;

        [Category("2.3 Скорость двигателя толкателя")]
        [DisplayName("Скорость движения толкателя домой")]
        public int PushStepperHomeSpeed { get; set; } = 500;
        [Category("2.3 Скорость двигателя толкателя")]
        [DisplayName("Скорость движения толкателя")]
        public int PushStepperSpeed { get; set; } = 200;

        [Category("3.1 Шаги двигателя поворота загрузки")]
        [DisplayName("Шаги до ячеек с кассетами")]
        public int[] CellsSteps { get; set; } =
        {
            800,
            4000,
            6800,
            10000,
            13200,
            16000,
            19300,
            22300,
            25300,
            28500
        };

        [Category("3.1 Шаги двигателя поворота загрузки")]
        [DisplayName("Шаги до разгрузки")]
        public int StepsToUnload { get; set; } = 0;

        [Category("3.2 Шаги двигателя челнока")]
        [DisplayName("Шаги отъезда челнока от дома")]
        public int StepsShuttleToStart { get; set; } = -20000;

        [Category("3.2 Шаги двигателя челнока")]
        [DisplayName("Шаги движения челнока к кассете")]
        public int StepsShuttleToCassette { get; set; } = -2840000;

        public LoadControllerPropetries()
        {

        }
    }

    public class LoadController : ControllerBase
    {
        const string filename = "LoadControllerProps";

        public LoadControllerPropetries Props { get; set; }

        public int LoadStepperPosition { get; set; } = 0;
        public int ShuttleStepperPosition { get; set; } = 0;
        public int PushStepperPosition { get; set; } = 0;

        public int CurrentCell { get; private set; } = 0;

        public LoadController() : base()
        {
            Props = new LoadControllerPropetries();
        }

        public void WriteXml()
        {
            XMLSerializeHelper<LoadControllerPropetries>.WriteXml(Props, filename);
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            Props = XMLSerializeHelper<LoadControllerPropetries>.ReadXML(filename);
        }

        public List<IAbstractCommand> HomeLoad()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.LoadStepper, Props.LoadStepperHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.LoadStepper, Props.LoadStepperHomeSpeed } };
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
                { Props.LoadStepper, 30 } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Props.LoadStepper, Props.CellsSteps[cell] - LoadStepperPosition } };
            commands.Add( new MoveCncCommand(steppers) );

            LoadStepperPosition = Props.CellsSteps[cell];

            return commands;
        }

        public List<IAbstractCommand> HomeShuttle()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, Props.ShuttleStepperHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, Props.ShuttleStepperHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, Props.StepsShuttleToStart } };
            commands.Add( new MoveCncCommand(steppers) );

            ShuttleStepperPosition = Props.StepsShuttleToStart;

            return commands;
        }

        public List<IAbstractCommand> MoveShuttleToCassette()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, Props.ShuttleStepperSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, Props.StepsShuttleToCassette } };
            commands.Add( new MoveCncCommand(steppers) );

            return commands;
        }
    }
}
