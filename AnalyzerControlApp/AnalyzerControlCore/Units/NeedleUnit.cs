using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerService.ExecutionControl;
using AnalyzerDomain.Entyties;
using Infrastructure;
using System.Collections.Generic;

namespace AnalyzerService.Units
{
    public class NeedleUnit : UnitBase<NeedleConfiguration>
    {
        public int LifterPosition { get; set; } = 0;
        public int RotatorPosition { get; set; } = 0;
        public bool LifterPositionUnderfined { get; set; } = true;

        public NeedleUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        {

        }

        public void TurnToTubeAndWaitTouch()
        {
            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Start turn to tube and waiting touch.");
            List<ICommand> commands = new List<ICommand>();

            // Поворот иглы до пробирки
            commands.Add(new SetSpeedCommand(Options.RotatorStepper, (uint)Options.RotatorSpeed));

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, Options.RotatorStepsToTube - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            RotatorPosition = Options.RotatorStepsToTube;

            commands.Clear();

            // Опускание иглы до жидкости в пробирке
            commands.Add(new SetSpeedCommand(Options.LifterStepper, (uint)Options.LifterSpeed));

            steppers = new Dictionary<int, int>() { { Options.LifterStepper, Options.LifterSpeed } };
            commands.Add(new RunCncCommand(steppers, 0, 500, ValueEdge.RisingEdge));

            // Дополнительное опускание иглы в жидкости
            steppers = new Dictionary<int, int>() { { Options.LifterStepper, Options.LifterStepsAfterTouch } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            
            LifterPositionUnderfined = true; // ибо неизвестно, где он будет после касания жидкости в пробирке

            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Turn to tube and waiting touch finished.");
        }

        public void HomeRotator()
        {
            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Start rotator going to home.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Options.RotatorStepper, 1000));

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, 100 } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            
            RotatorPosition = 0;

            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Rotator goint to home finished.");
        }

        public void HomeLifter()
        {
            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Start lift going to home.");
            List<ICommand> commands = new List<ICommand>();

            
            commands.Add(new SetSpeedCommand(Options.LifterStepper, 1000));

            steppers = new Dictionary<int, int>() { { Options.LifterStepper, -200 } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);

            LifterPosition = 0;
            LifterPositionUnderfined = false;

            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Lift going to home finished.");
        }

        public void HomeLifterAndRotator()
        {
            HomeLifter();
            HomeRotator();
        }

        public void TurnAndGoDownToWashing()
        {
            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Start turn and going down to washing.");
            List<ICommand> commands = new List<ICommand>();

            // Поворот иглы до промывки
            commands.Add(new SetSpeedCommand(Options.RotatorStepper, 50));

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, Options.RotatorStepsToWashing - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            commands.Clear();

            RotatorPosition = Options.RotatorStepsToWashing;

            if (LifterPositionUnderfined)
                HomeLifter();

            // Опускание иглы до промывки
            commands.Add(new SetSpeedCommand(Options.LifterStepper, 500));

            steppers = new Dictionary<int, int>() { { Options.LifterStepper, Options.LifterStepsToWashing - LifterPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            LifterPosition = Options.LifterStepsToWashing;

            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Turning and going down to washing finished.");
        }

        public void GoDownAndPerforateCartridge(CartridgeCell cartridgeCell, bool needSuction = true)
        {
            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Start going down and perforating cartridge.");

            List<ICommand> commands = new List<ICommand>();

            int steps = Options.LifterStepsToCell;

            if (cartridgeCell == CartridgeCell.MixCell)
            {
                if (needSuction)
                    steps = Options.LifterStepsToMixCellAtSuction;
                else
                    steps = Options.LifterStepsToMixCell;
            }

            if (LifterPositionUnderfined)
                HomeLifter();

            // Протыкание
            commands.Add(new SetSpeedCommand(Options.LifterStepper, (uint)Options.LifterSpeed));

            steppers = new Dictionary<int, int>() { { Options.LifterStepper, steps - LifterPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            LifterPosition = steps;

            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Going down and piercing cartridge finished.");
        }

        public void GoToSafeLevel()
        {
            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Start going to safe level.");

            List<ICommand> commands = new List<ICommand>();

            if (LifterPositionUnderfined)
                HomeLifter();

            int steps = Options.LifterStepsToSafeLevel;

            commands.Add(new SetSpeedCommand(Options.LifterStepper, (uint)Options.LifterSpeed));

            steppers = new Dictionary<int, int>() { { Options.LifterStepper, steps - LifterPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            LifterPosition = steps;

            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Going to safe level finished.");
        }

        public void TurnToCartridge(CartridgeCell cell)
        {
            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Start turn to cartridge.");
            List<ICommand> commands = new List<ICommand>();
            
            int turnSteps = 0;

            //TODO: Что то тут не так (if-else)
            if(cell == CartridgeCell.ResultCell)
            {
                turnSteps = Options.RotatorStepsToResultCell;
            }
            if(cell == CartridgeCell.MixCell)
            {
                turnSteps = Options.RotatorStepsToMixCell;
            }
            else if(cell == CartridgeCell.FirstCell)
            {
                turnSteps = Options.RotatorStepsToFirstCell;
            }
            else if(cell == CartridgeCell.SecondCell)
            {
                turnSteps = Options.RotatorStepsToSecondCell;
            }
            else if(cell == CartridgeCell.ThirdCell)
            {
                turnSteps = Options.RotatorStepsToThirdCell;
            }

            commands.Add(new SetSpeedCommand(Options.RotatorStepper, 50));

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, turnSteps - RotatorPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            RotatorPosition = turnSteps;

            Logger.ControllerInfo($"[{nameof(NeedleUnit)}] - Turn to cartridge finished.");
        }
    }
}
