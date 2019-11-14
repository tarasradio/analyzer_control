using SteppersControlCore.CommunicationProtocol.AdditionalCommands;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.ControllersProperties;
using SteppersControlCore.Interfaces;
using SteppersControlCore.Utils;
using System.Collections.Generic;
using System.IO;

namespace SteppersControlCore.Controllers
{
    //TODO: а тут видимо вообще нахер не надо отслеживание
    public class TransporterController : ControllerBase
    {
        string filename = "TransporterControllerProps";
        public TransporterControllerProperties Properties { get; set; }

        public int TransporterStepperPosition { get; set; } = 0;

        public TransporterController(ICommandExecutor executor) : base(executor)
        {
            Properties = new TransporterControllerProperties();
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<TransporterControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename));
        }

        //Чтение насроек из файла
        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<TransporterControllerProperties>.ReadXML(
                Path.Combine(path, filename));

            if (Properties == null)
                Properties = new TransporterControllerProperties();
        }

        public void PrepareBeforeScanning()
        {
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Properties.TransporterStepper, 100));

            // Обнуление ленты конвеера
            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, 50 } };
            commands.Add(new HomeCncCommand(steppers));

            // Сдвиг на пол пробирки
            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, Properties.StepsOneTube / 2 } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }

        public enum ShiftType
        {
            HalfTube,
            OneTube
        };

        // Сдвиг
        public void Shift(bool reverse, ShiftType shiftType = ShiftType.OneTube)
        {
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Properties.TransporterStepper, 50));

            int steps = Properties.StepsOneTube;

            if (shiftType == ShiftType.HalfTube) steps /= 2;
            if (reverse) steps *= -1;

            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, steps } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }

        public void TurnAndScanTube()
        {
            List<ICommand> commands = new List<ICommand>();
            
            // Сканирование пробирки
            commands.Add(new BarStartCommand());

            // Вращение пробирки
            commands.Add(new SetSpeedCommand(Properties.TurnTubeStepper, (uint)Properties.SpeedToTurnTube));

            steppers = new Dictionary<int, int>() { { Properties.TurnTubeStepper, Properties.StepsToTurnTube } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }
    }
}
