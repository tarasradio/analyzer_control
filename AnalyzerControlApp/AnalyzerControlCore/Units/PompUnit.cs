using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.ControllersConfiguration;
using AnalyzerControlCore.MachineControl;
using Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace AnalyzerControlCore.Units
{
    public class PompUnit : AbstractUnit
    {
        const string filename = "PompControllerProps";

        public PompControllerConfiguration Config { get; set; }

        public PompUnit(ICommandExecutor executor) : base(executor)
        {
            Config = new PompControllerConfiguration();
        }

        public void SaveConfiguration(string path)
        {
            XmlSerializeHelper<PompControllerConfiguration>.WriteXml(Config, Path.Combine(path, nameof(PompControllerConfiguration)) );
        }

        public void LoadConfiguration(string path)
        {
            Config = XmlSerializeHelper<PompControllerConfiguration>.ReadXml( Path.Combine(path, nameof(PompControllerConfiguration)) );

            if (Config == null)
                Config = new PompControllerConfiguration();
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
                { Config.BigPistonStepper, Config.BigPistonHomeSpeed },
                { Config.SmallPistonStepper, Config.SmallPistonHomeSpeed }
            };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Config.BigPistonStepper, Config.BigPistonHomeSpeed },
                { Config.SmallPistonStepper, Config.SmallPistonHomeSpeed }
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
                { Config.SmallPistonStepper, Config.SmallPistonSuctionSpeed }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Config.SmallPistonStepper, Config.SmallPistonSuctionSteps }
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
                { Config.SmallPistonStepper, Config.SmallPistonSuctionSpeed }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Config.SmallPistonStepper, Config.SmallPistonSuctionSpeed }
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
                    { Config.BigPistonStepper, Config.BigPistonHomeSpeed },
                    { Config.SmallPistonStepper, Config.SmallPistonHomeSpeed }
                };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                    { Config.BigPistonStepper, Config.BigPistonHomeSpeed },
                    { Config.SmallPistonStepper, Config.SmallPistonHomeSpeed }
                };
            commands.Add(new HomeCncCommand(steppers));

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 }));
            commands.Add(new OnDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                    { Config.BigPistonStepper, Config.BigPistonWashingSpeed },
                    { Config.SmallPistonStepper, Config.SmallPistonWashingSpeed }
                };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                    { Config.BigPistonStepper, Config.BigPistonWashingSteps },
                    { Config.SmallPistonStepper, Config.SmallPistonWashingSteps }
                };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }
    }
}
