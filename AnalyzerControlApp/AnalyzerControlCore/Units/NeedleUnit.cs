using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.ControllersConfiguration;
using AnalyzerControlCore.MachineControl;
using Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace AnalyzerControlCore.Units
{
    public class NeedleUnit : AbstractUnit
    {
        public NeedleControllerConfiguration Config { get; set; }
        private IConfigurationProvider<NeedleControllerConfiguration> provider;

        public int LiftPosition { get; set; } = 0;
        public int RotatorPosition { get; set; } = 0;
        public bool LiftPositionUnderfined { get; set; } = true;

        public NeedleUnit(ICommandExecutor executor) : base(executor)
        {
            Config = new NeedleControllerConfiguration();
        }

        public void SetProvider(IConfigurationProvider<NeedleControllerConfiguration> provider)
        {
            this.provider = provider;
        }

        public void SaveConfiguration(string path)
        {
            provider.SaveConfiguration(Config, Path.Combine(path, nameof(NeedleControllerConfiguration)) );
        }

        public void LoadConfiguration(string path)
        {
            Config = provider.LoadConfiguration( Path.Combine(path, nameof(NeedleControllerConfiguration)) );
            if (Config == null)
                Config = new NeedleControllerConfiguration();
        }

        public void TurnToTubeAndWaitTouch()
        {
            Logger.ControllerInfo($"[Needle] - Start turn to tube and waiting touch.");
            List<ICommand> commands = new List<ICommand>();

            // Поворот иглы до пробирки
            commands.Add(new SetSpeedCommand(Config.RotatorStepper, (uint)Config.RotatorSpeed));

            steppers = new Dictionary<int, int>() { { Config.RotatorStepper, Config.RotatorStepsTurnToTube - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            RotatorPosition = Config.RotatorStepsTurnToTube;

            commands.Clear();

            // Опускание иглы до жидкости в пробирке
            commands.Add(new SetSpeedCommand(Config.LiftStepper, (uint)Config.LiftSpeed));

            steppers = new Dictionary<int, int>() { { Config.LiftStepper, Config.LiftSpeed } };
            commands.Add(new RunCncCommand(steppers, 0, 500, Protocol.ValueEdge.RisingEdge));

            // Дополнительное опускание иглы в жидкости
            steppers = new Dictionary<int, int>() { { Config.LiftStepper, Config.LiftStepsGoDownAfterTouch } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            
            LiftPositionUnderfined = true; // ибо неизвестно, где он будет после касания жидкости в пробирке

            Logger.ControllerInfo($"[Needle] - Turn to tube and waiting touch finished.");
        }

        public void HomeRotator()
        {
            Logger.ControllerInfo($"[Needle] - Start rotator going to home.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Config.RotatorStepper, 1000));

            steppers = new Dictionary<int, int>() { { Config.RotatorStepper, 100 } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            
            RotatorPosition = 0;

            Logger.ControllerInfo($"[Needle] - Rotator goint to home finished.");
        }

        public void HomeLift()
        {
            Logger.ControllerInfo($"[Needle] - Start lift going to home.");
            List<ICommand> commands = new List<ICommand>();

            
            commands.Add(new SetSpeedCommand(Config.LiftStepper, 1000));

            steppers = new Dictionary<int, int>() { { Config.LiftStepper, -200 } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);

            LiftPosition = 0;
            LiftPositionUnderfined = false;

            Logger.ControllerInfo($"[Needle] - Lift going to home finished.");
        }

        public void HomeAll()
        {
            // Поднятие иглы
            HomeLift();
            // Поворот иглы
            HomeRotator();
        }

        public void TurnAndGoDownToWashing()
        {
            Logger.ControllerInfo($"[Needle] - Start turn and going down to washing.");
            List<ICommand> commands = new List<ICommand>();

            // Поворот иглы до промывки
            commands.Add(new SetSpeedCommand(Config.RotatorStepper, 50));

            steppers = new Dictionary<int, int>() { { Config.RotatorStepper, Config.RotatorStepsTurnToWashing - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            commands.Clear();

            RotatorPosition = Config.RotatorStepsTurnToWashing;

            if (LiftPositionUnderfined)
                HomeLift();

            // Опускание иглы до промывки
            commands.Add(new SetSpeedCommand(Config.LiftStepper, 500));

            steppers = new Dictionary<int, int>() { { Config.LiftStepper, Config.LiftStepsGoDownToWashing - LiftPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            LiftPosition = Config.LiftStepsGoDownToWashing;

            Logger.ControllerInfo($"[Needle] - Turn and going down to washing finished.");
        }

        public void GoDownAndPierceCartridge(CartridgeCell cartridgeCell, bool needSuction = true)
        {
            Logger.ControllerInfo($"[Needle] - Start going down and piercing cartridge.");

            List<ICommand> commands = new List<ICommand>();

            int steps = Config.LiftStepsGoDownToCell;

            if (cartridgeCell == CartridgeCell.WhiteCell)
            {
                if (needSuction)
                    steps = Config.LiftStepsGoDownToMixCellAtSuction;
                else
                    steps = Config.LiftStepsGoDownToMixCell;
            }

            if (LiftPositionUnderfined)
                HomeLift();

            // Протыкание
            commands.Add(new SetSpeedCommand(Config.LiftStepper, (uint)Config.LiftSpeed));

            steppers = new Dictionary<int, int>() { { Config.LiftStepper, steps - LiftPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            LiftPosition = steps;

            Logger.ControllerInfo($"[Needle] - Going down and piercing cartridge finished.");
        }

        public void GoToSafeLevel()
        {
            Logger.ControllerInfo($"[Needle] - Start going to safe level.");

            List<ICommand> commands = new List<ICommand>();

            if (LiftPositionUnderfined)
                HomeLift();

            int steps = Config.LiftStepsGoDownToSafeLevel;

            commands.Add(new SetSpeedCommand(Config.LiftStepper, (uint)Config.LiftSpeed));

            steppers = new Dictionary<int, int>() { { Config.LiftStepper, steps - LiftPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            LiftPosition = steps;

            Logger.ControllerInfo($"[Needle] - Going to safe level finished.");
        }

        public void TurnToCartridge(CartridgeCell cell)
        {
            Logger.ControllerInfo($"[Needle] - Start turn to cartridge.");
            List<ICommand> commands = new List<ICommand>();
            
            int turnSteps = 0;

            if(cell == CartridgeCell.WhiteCell)
            {
                turnSteps = Config.RotatorStepsTurnToMixCell;
            }
            else if(cell == CartridgeCell.FirstCell)
            {
                turnSteps = Config.RotatorStepsTurnToFirstCell;
            }
            else if(cell == CartridgeCell.SecondCell)
            {
                turnSteps = Config.RotatorStepsTurnToSecondCell;
            }
            else if(cell == CartridgeCell.ThirdCell)
            {
                turnSteps = Config.RotatorStepsTurnToThirdCell;
            }

            commands.Add(new SetSpeedCommand(Config.RotatorStepper, 50));

            steppers = new Dictionary<int, int>() { { Config.RotatorStepper, turnSteps - RotatorPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            RotatorPosition = turnSteps;

            Logger.ControllerInfo($"[Needle] - Turn to cartridge finished.");
        }
    }
}
