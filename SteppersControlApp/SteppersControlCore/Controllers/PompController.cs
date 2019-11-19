using System.Collections.Generic;
using System.IO;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.Utils;
using SteppersControlCore.ControllersProperties;
using SteppersControlCore.Interfaces;

namespace SteppersControlCore.Controllers
{
    public class PompController : ControllerBase
    {
        const string filename = "PompControllerProps";

        public PompControllerProperties Properties { get; set; }

        public PompController(ICommandExecutor executor) : base(executor)
        {
            Properties = new PompControllerProperties();
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<PompControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename));
        }

        //Чтение насроек из файла
        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<PompControllerProperties>.ReadXML(
                Path.Combine(path, filename));

            if (Properties == null)
                Properties = new PompControllerProperties();
        }

        public void CloseValves()
        {
            Logger.ControllerInfo($"[Pomp] - Close valves.");
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 , 1 }));

            executor.WaitExecution(commands);
        }

        public void Home()
        {
            Logger.ControllerInfo($"[Pomp] - Start homing.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));
            
            commands.Add(new OffDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                { Properties.BigPistonStepper, Properties.BigPistonHomeSpeed },
                { Properties.SmallPistonStepper, Properties.SmallPistonHomeSpeed }
            };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Properties.BigPistonStepper, Properties.BigPistonHomeSpeed },
                { Properties.SmallPistonStepper, Properties.SmallPistonHomeSpeed }
            };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Pomp] - Homing finished.");
        }

        public void Suction(int value)
        {
            Logger.ControllerInfo($"[Pomp] - Start suction.");
            List<ICommand> commands = new List<ICommand>();

            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );
            
            steppers = new Dictionary<int, int>() {
                { Properties.SmallPistonStepper, Properties.SmallPistonSuctionSpeed }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Properties.SmallPistonStepper, Properties.SmallPistonSuctionSteps }
            };
            commands.Add( new MoveCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Pomp] - Suction finished.");
        }

        public void Unsuction(int value)
        {
            Logger.ControllerInfo($"[Pomp] - Start unsuction.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );

            steppers = new Dictionary<int, int>() {
                { Properties.SmallPistonStepper, Properties.SmallPistonSuctionSpeed }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Properties.SmallPistonStepper, Properties.SmallPistonSuctionSpeed }
            };
            commands.Add( new HomeCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[Pomp] - Unsuction finished.");
        }

        public void WashingNeedle(int cycles)
        {
            Logger.ControllerInfo($"[Pomp] - Start washing ({cycles} cycles).");
            
            for (int i = 0; i < cycles; i++)
            {
                WashingCycle();
            }
            Logger.ControllerInfo($"[Pomp] - Washing finished.");
        }

        private void WashingCycle()
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));
            commands.Add(new OffDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                    { Properties.BigPistonStepper, Properties.BigPistonHomeSpeed },
                    { Properties.SmallPistonStepper, Properties.SmallPistonHomeSpeed }
                };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                    { Properties.BigPistonStepper, Properties.BigPistonHomeSpeed },
                    { Properties.SmallPistonStepper, Properties.SmallPistonHomeSpeed }
                };
            commands.Add(new HomeCncCommand(steppers));

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 }));
            commands.Add(new OnDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                    { Properties.BigPistonStepper, Properties.BigPistonWashingSpeed },
                    { Properties.SmallPistonStepper, Properties.SmallPistonWashingSpeed }
                };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                    { Properties.BigPistonStepper, Properties.BigPistonWashingSteps },
                    { Properties.SmallPistonStepper, Properties.SmallPistonWashingSteps }
                };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }
    }
}
