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
        [DisplayName("Двигатель загрузки")]
        public int LoadStepper { get; set; } = 10;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель челнока")]
        public int ShuttleStepper { get; set; } = 15;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель толкателя")]
        public int PushStepper { get; set; } = 0;

        [Category("2. Скорость")]
        [DisplayName("Скорость движения загрузки домой")]
        public int LoadStepperHomeSpeed { get; set; } = 50;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения загрузки")]
        public int LoadStepperSpeed { get; set; } = 50;

        [Category("2. Скорость")]
        [DisplayName("Скорость движения челнока домой")]
        public int ShuttleStepperHomeSpeed { get; set; } = 500;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения челнока")]
        public int ShuttleStepperSpeed { get; set; } = 200;

        [Category("2. Скорость")]
        [DisplayName("Скорость движения толкателя домой")]
        public int PushStepperHomeSpeed { get; set; } = 500;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения толкателя")]
        public int PushStepperSpeed { get; set; } = 200;

        [Category("3. Шаги")]
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

        [Category("3. Шаги")]
        [DisplayName("Шаги отъезда челнока от дома")]
        public int StepsShuttleToStart { get; set; } = 20000;

        [Category("3. Шаги")]
        [DisplayName("Шаги движения челнока к кассете")]
        public int StepsShuttleToCassette { get; set; } = 2840000;

        public LoadControllerPropetries()
        {

        }
    }

    public class LoadController : Controller
    {
        const string filename = "LoadControllerProps";

        public LoadControllerPropetries Props { get; set; }

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

            steppers = new Dictionary<int, int>() { { Props.LoadStepper, -Props.LoadStepperHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            return commands;
        }

        public List<IAbstractCommand> TurnLoadToCell(int cell)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            CurrentCell = cell;

            steppers = new Dictionary<int, int>() { { Props.LoadStepper, 30 } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.LoadStepper, Props.CellsSteps[cell] } };
            commands.Add( new MoveCncCommand(steppers) );

            return commands;
        }

        public List<IAbstractCommand> HomeShuttle()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, Props.ShuttleStepperHomeSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, Props.ShuttleStepperHomeSpeed } };
            commands.Add( new HomeCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, -Props.StepsShuttleToStart } };
            commands.Add( new MoveCncCommand(steppers) );

            return commands;
        }

        public List<IAbstractCommand> MoveShuttleToCassette()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, Props.ShuttleStepperSpeed } };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, -1000000 } };
            commands.Add( new MoveCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, -1000000 } };
            commands.Add( new MoveCncCommand(steppers) );

            steppers = new Dictionary<int, int>() { { Props.ShuttleStepper, -900000 } };
            commands.Add( new MoveCncCommand(steppers) );

            return commands;
        }
    }
}
