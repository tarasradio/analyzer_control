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
            Logger.ControllerInfo($"[Transporter] - Start prepare before scanning.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Properties.TransporterStepper, (uint)Properties.TransporterMoveSpeed));

            // Обнуление ленты конвеера
            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, Properties.TransporterHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            // Сдвиг на пол пробирки
            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, Properties.StepsPerTube / 2 } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Transporter] - Prepare before scanning finished.");
        }

        public enum ShiftType
        {
            HalfTube,
            OneTube
        };

        // Сдвиг
        public void Shift(bool reverse, ShiftType shiftType = ShiftType.OneTube)
        {
            Logger.ControllerInfo($"[Transporter] - Start shift.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Properties.TransporterStepper, (uint)Properties.TransporterMoveSpeed));

            int steps = Properties.StepsPerTube;

            if (shiftType == ShiftType.HalfTube) steps /= 2;
            if (reverse) steps *= -1;

            steppers = new Dictionary<int, int>() { { Properties.TransporterStepper, steps } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Transporter] - Shift finished.");
        }

        public void RotateAndScanTube()
        {
            Logger.ControllerInfo($"[Transporter] - Start rotating and scanning tube.");
            List<ICommand> commands = new List<ICommand>();
            
            // Сканирование пробирки
            commands.Add(new BarStartCommand());

            // Вращение пробирки
            commands.Add(new SetSpeedCommand(Properties.TurnTubeStepper, (uint)Properties.TurnTubeSpeed));

            steppers = new Dictionary<int, int>() { { Properties.TurnTubeStepper, Properties.StepsPerTubeRotate } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Transporter] - Rotating and scanning tube finished.");
        }
    }
}
