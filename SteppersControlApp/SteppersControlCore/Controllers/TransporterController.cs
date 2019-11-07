using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.AdditionalCommands;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;

using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using SteppersControlCore.Utils;

namespace SteppersControlCore.Controllers
{
    public class TransporterControllerPropetries
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель вращения пробирки")]
        public int TurnTubeStepper { get; set; } = 5;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель движения пробирок")]
        public int TransporterStepper { get; set; } = 6;

        [Category("2. Скорость")]
        [DisplayName("Скорость вращения пробирки")]
        public int SpeedToTurnTube { get; set; } = 30;

        [Category("3. Шаги")]
        [DisplayName("Шагов для сдвига на одну пробирку")]
        public int StepsOneTube { get; set; } = 6400;
        [Category("3. Шаги")]
        [DisplayName("Шагов для вращения пробирки")]
        public int StepsToTurnTube { get; set; } = 10000;

        public TransporterControllerPropetries()
        {

        }
    }

    //TODO: а тут видимо вообще нахер не надо отслеживание
    public class TransporterController : ControllerBase
    {
        string filename = "TransporterControllerProps";
        public TransporterControllerPropetries Props { get; set; }

        public int TransporterStepperPosition { get; set; } = 0;

        public TransporterController() : base()
        {
            Props = new TransporterControllerPropetries();
        }

        public void WriteXml()
        {
            XMLSerializeHelper<TransporterControllerPropetries>.WriteXml(Props, filename);
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            Props = XMLSerializeHelper<TransporterControllerPropetries>.ReadXML(filename);
        }

        public List<IAbstractCommand> PrepareBeforeScanning()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            commands.Add(new SetSpeedCommand(Props.TransporterStepper, 100));

            // Обнуление ленты конвеера
            steppers = new Dictionary<int, int>() { { Props.TransporterStepper, 50 } };
            commands.Add(new HomeCncCommand(steppers));

            // Сдвиг на пол пробирки
            steppers = new Dictionary<int, int>() { { Props.TransporterStepper, Props.StepsOneTube / 2 } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }

        public enum ShiftType
        {
            HalfTube,
            OneTube
        };

        // Сдвиг
        public List<IAbstractCommand> Shift(bool reverse, ShiftType shiftType = ShiftType.OneTube)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            commands.Add(new SetSpeedCommand(Props.TransporterStepper, 50));

            int steps = Props.StepsOneTube;

            if (shiftType == ShiftType.HalfTube) steps /= 2;
            if (reverse) steps *= -1;

            steppers = new Dictionary<int, int>() { { Props.TransporterStepper, steps } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }

        public List<IAbstractCommand> TurnAndScanTube()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            // Сканирование пробирки
            commands.Add(new BarStartCommand());

            // Вращение пробирки
            commands.Add(new SetSpeedCommand(Props.TurnTubeStepper, (uint)Props.SpeedToTurnTube));

            steppers = new Dictionary<int, int>() { { Props.TurnTubeStepper, Props.StepsToTurnTube } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }
    }
}
