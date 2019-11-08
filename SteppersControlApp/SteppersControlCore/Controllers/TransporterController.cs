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

using SteppersControlCore.ControllersProperties;

namespace SteppersControlCore.Controllers
{
    //TODO: а тут видимо вообще нахер не надо отслеживание
    public class TransporterController : ControllerBase
    {
        string filename = "TransporterControllerProps";
        public TransporterControllerProperties Properties { get; set; }

        public int TransporterStepperPosition { get; set; } = 0;

        public TransporterController() : base()
        {
            Properties = new TransporterControllerProperties();
        }

        public void WriteXml()
        {
            XMLSerializeHelper<TransporterControllerProperties>.WriteXml(Properties, filename);
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            Properties = XMLSerializeHelper<TransporterControllerProperties>.ReadXML(filename);
        }

        public List<IAbstractCommand> PrepareBeforeScanning()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            commands.Add(new SetSpeedCommand(Properties.TransporterStepper, 100));

            // Обнуление ленты конвеера
            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, 50 } };
            commands.Add(new HomeCncCommand(steppers));

            // Сдвиг на пол пробирки
            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, Properties.StepsOneTube / 2 } };
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
            
            commands.Add(new SetSpeedCommand(Properties.TransporterStepper, 50));

            int steps = Properties.StepsOneTube;

            if (shiftType == ShiftType.HalfTube) steps /= 2;
            if (reverse) steps *= -1;

            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, steps } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }

        public List<IAbstractCommand> TurnAndScanTube()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            // Сканирование пробирки
            commands.Add(new BarStartCommand());

            // Вращение пробирки
            commands.Add(new SetSpeedCommand(Properties.TurnTubeStepper, (uint)Properties.SpeedToTurnTube));

            steppers = new Dictionary<int, int>() { { Properties.TurnTubeStepper, Properties.StepsToTurnTube } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }
    }
}
