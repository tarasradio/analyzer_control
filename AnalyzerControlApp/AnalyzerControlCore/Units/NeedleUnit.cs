using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerDomain.Models;
using AnalyzerService.ExecutionControl;
using Infrastructure;
using System.Collections.Generic;

namespace AnalyzerService.Units
{
    public class NeedleUnit : UnitBase<NeedleConfiguration>
    {
        public int LifterPosition { get; set; } = 0;
        public int RotatorPosition { get; set; } = 0;
        public bool LifterPositionUnderfined { get; set; } = true;
        
        private const int rotatorMovingSpeed = 50;
        private const int rotatorHomingSpeed = 100;
        private const int lifterHomingSpeed = 500;

        public NeedleUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        {

        }

        public void GoHome()
        {
            HomeLifter();
            HomeRotator();
        }

        public void HomeRotator()
        {
            Logger.Debug($"[{nameof(NeedleUnit)}] - Start rotator going to home.");

            List<ICommand> commands = new List<ICommand>();

            commands.Add(new SetSpeedCommand(Options.RotatorStepper, rotatorHomingSpeed));

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, rotatorHomingSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);

            RotatorPosition = 0;

            Logger.Debug($"[{nameof(NeedleUnit)}] - Rotator goint to home finished.");
        }

        public void HomeLifter()
        {
            Logger.Debug($"[{nameof(NeedleUnit)}] - Start lift going to home.");

            List<ICommand> commands = new List<ICommand>();

            commands.Add(new SetSpeedCommand(Options.LifterStepper, lifterHomingSpeed));

            steppers = new Dictionary<int, int>() { { Options.LifterStepper, -lifterHomingSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);

            LifterPosition = 0;
            LifterPositionUnderfined = false;

            Logger.Debug($"[{nameof(NeedleUnit)}] - Lift going to home finished.");
        }

        public void TurnToTubeAndWaitTouch()
        {
            Logger.Debug($"[{nameof(NeedleUnit)}] - Start turn to tube and waiting touch.");
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

            executor.WaitExecution(commands);

            commands.Clear();

            // Опускание иглы до жидкости в пробирке
            commands.Add(new SetSpeedCommand(Options.LifterStepper, (uint)Options.LifterSpeed));

            // Дополнительное опускание иглы в жидкости
            steppers = new Dictionary<int, int>() { { Options.LifterStepper, Options.LifterStepsAfterTouch } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            
            LifterPositionUnderfined = true; // ибо неизвестно, где он будет после касания жидкости в пробирке

            Logger.Debug($"[{nameof(NeedleUnit)}] - Turn to tube and waiting touch finished.");
        }

        public void TurnAndGoDownToWashing(bool alkali)
        {
            Logger.Debug($"[{nameof(NeedleUnit)}] - Start turn and going down to washing.");
            List<ICommand> commands = new List<ICommand>();

            int steps = alkali ? Options.RotatorStepsToAlkaliWashing : Options.RotatorStepsToWashing;

            // Поворот иглы до промывки
            commands.Add(new SetSpeedCommand(Options.RotatorStepper, rotatorMovingSpeed));

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, steps - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            commands.Clear();

            RotatorPosition = steps;

            if (LifterPositionUnderfined)
                HomeLifter();

            // Опускание иглы до промывки
            commands.Add(new SetSpeedCommand(Options.LifterStepper, (uint)Options.LifterSpeed));

            steppers = new Dictionary<int, int>() { { Options.LifterStepper, Options.LifterStepsToWashing - LifterPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            LifterPosition = Options.LifterStepsToWashing;

            Logger.Debug($"[{nameof(NeedleUnit)}] - Turning and going down to washing finished.");
        }

        public void PerforateCartridge(CartridgeWell cartridgeCell, bool needSuction = true)
        {
            Logger.Debug($"[{nameof(NeedleUnit)}] - Start going down and perforating cartridge.");

            List<ICommand> commands = new List<ICommand>();

            int steps = Options.LifterStepsToCell;

            if (cartridgeCell == CartridgeWell.ACW)
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

            Logger.Debug($"[{nameof(NeedleUnit)}] - Going down and piercing cartridge finished.");
        }

        public void GoToSafeLevel()
        {
            Logger.Debug($"[{nameof(NeedleUnit)}] - Start going to safe level.");

            List<ICommand> commands = new List<ICommand>();

            if (LifterPositionUnderfined)
                HomeLifter();

            int steps = Options.LifterStepsToSafeLevel;

            commands.Add(new SetSpeedCommand(Options.LifterStepper, (uint)Options.LifterSpeed));

            steppers = new Dictionary<int, int>() { { Options.LifterStepper, steps - LifterPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            LifterPosition = steps;

            Logger.Debug($"[{nameof(NeedleUnit)}] - Going to safe level finished.");
        }

        public void TurnToCartridge(CartridgeWell well)
        {
            Logger.Debug($"[{nameof(NeedleUnit)}] - Start turn to cartridge.");
            List<ICommand> commands = new List<ICommand>();
            
            int turnSteps = 0;

            //TODO: Что то тут не так (if-else)
            if(well == CartridgeWell.CUV) {
                turnSteps = Options.RotatorStepsToResultCell;
            }
            else if(well == CartridgeWell.ACW) {
                turnSteps = Options.RotatorStepsToMixCell;
            }
            else if(well == CartridgeWell.W1) {
                turnSteps = Options.RotatorStepsToFirstCell;
            }
            else if(well == CartridgeWell.W2) {
                turnSteps = Options.RotatorStepsToSecondCell;
            }
            else if(well == CartridgeWell.W3) {
                turnSteps = Options.RotatorStepsToThirdCell;
            }

            commands.Add(new SetSpeedCommand(Options.RotatorStepper, rotatorMovingSpeed));

            steppers = new Dictionary<int, int>() { { Options.RotatorStepper, turnSteps - RotatorPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            RotatorPosition = turnSteps;

            Logger.Debug($"[{nameof(NeedleUnit)}] - Turn to cartridge finished.");
        }
    }
}
