using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerService.ExecutionControl;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerService.Units
{
    public class AdditionalDevicesUnit : UnitBase<AdditionalDevicesConfiguration>
    {
        public AdditionalDevicesUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        {

        }

        public void HomeScreen()
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new SetSpeedCommand(Options.ScreenUpDownStepper, 100));

            // Выставить экран прямо
            steppers = new Dictionary<int, int>() { { Options.ScreenUpDownStepper, 100 } };
            commands.Add(new HomeCncCommand(steppers));

            commands.Add(new SetSpeedCommand(Options.ScreenTurnStepper, 150));

            // Выставить экран прямо
            steppers = new Dictionary<int, int>() { { Options.ScreenTurnStepper, 200 } };
            commands.Add(new HomeCncCommand(steppers));
            
            executor.WaitExecution(commands);
        }

        public void OpenScreen()
        {
            HomeScreen();

            List<ICommand> commands = new List<ICommand>();

            // Выставить экран прямо
            commands.Add(new SetSpeedCommand(Options.ScreenTurnStepper, 150));
            steppers = new Dictionary<int, int>() { { Options.ScreenTurnStepper, -310000 } };
            commands.Add(new MoveCncCommand(steppers));

            // Выставить экран прямо
            commands.Add(new SetSpeedCommand(Options.ScreenUpDownStepper, 100));
            steppers = new Dictionary<int, int>() { { Options.ScreenUpDownStepper, -70000 } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }

        public void CloseScreen()
        {
            HomeScreen();
        }

        public void HomeWashBuffer()
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new SetSpeedCommand(Options.WashBufferUpDownStepper, (uint)Options.WashBufferHomeSpeed));

            // Выставить экран прямо
            steppers = new Dictionary<int, int>() { { Options.WashBufferUpDownStepper, Options.WashBufferHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
        }

        public void PutDownWashBuffer()
        {
            List<ICommand> commands = new List<ICommand>();

            // Выставить экран прямо
            commands.Add(new SetSpeedCommand(Options.WashBufferUpDownStepper, (uint)Options.WashBufferPutDownSpeed));
            steppers = new Dictionary<int, int>() { { Options.WashBufferUpDownStepper, Options.WashBufferPutDownSteps } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
        }
    }
}
