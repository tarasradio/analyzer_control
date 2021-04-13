using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerService.MachineControl;
using Infrastructure;
using System.Collections.Generic;

namespace AnalyzerService.Units
{
    public class PompUnit : UnitBase<PompConfiguration>
    {
        public PompUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        {

        }

        public void CloseValves()
        {
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Close valves.");
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 , 1 }));

            executor.WaitExecution(commands);
        }

        public void Home()
        {
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Start homing.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));
            
            commands.Add(new OffDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                { Options.BigPistonStepper, Options.BigPistonSpeedAtHoming },
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtHoming }
            };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Options.BigPistonStepper, Options.BigPistonSpeedAtHoming },
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtHoming }
            };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Homing finished.");
        }

        public void Pull(int value)
        {
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Start suction.");
            List<ICommand> commands = new List<ICommand>();

            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );
            
            steppers = new Dictionary<int, int>() {
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtSuction }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Options.SmallPistonStepper, Options.SmallPistonStepsAtSuction }
            };
            commands.Add( new MoveCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Suction finished.");
        }

        public void Push(int value)
        {
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Start unsuction.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );

            steppers = new Dictionary<int, int>() {
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtSuction }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Options.SmallPistonStepper, Options.SmallPistonSpeedAtSuction }
            };
            commands.Add( new HomeCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            executor.WaitExecution(commands);
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Unsuction finished.");
        }

        public void WashTheNeedle(int cycles)
        {
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Start washing ({cycles} cycles).");
            
            for (int i = 0; i < cycles; i++)
            {
                WashingCycle();
            }
            Logger.ControllerInfo($"[{nameof(PompUnit)}] - Washing finished.");
        }

        private void WashingCycle()
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));
            commands.Add(new OffDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                    { Options.BigPistonStepper, Options.BigPistonSpeedAtHoming },
                    { Options.SmallPistonStepper, Options.SmallPistonSpeedAtHoming }
                };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                    { Options.BigPistonStepper, Options.BigPistonSpeedAtHoming },
                    { Options.SmallPistonStepper, Options.SmallPistonSpeedAtHoming }
                };
            commands.Add(new HomeCncCommand(steppers));

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 }));
            commands.Add(new OnDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                    { Options.BigPistonStepper, Options.BigPistonSpeedAtWashing },
                    { Options.SmallPistonStepper, Options.SmallPistonSpeedAtWashing }
                };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                    { Options.BigPistonStepper, Options.BigPistonStepsAtWashing },
                    { Options.SmallPistonStepper, Options.SmallPistonStepsAtWashing }
                };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }
    }
}
