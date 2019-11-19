using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.ControllersProperties;
using SteppersControlCore.Elements;
using SteppersControlCore.Utils;
using System.Collections.Generic;
using System.IO;

using SteppersControlCore.Interfaces;

namespace SteppersControlCore.Controllers
{
    public class NeedleController : ControllerBase
    {
        const string filename = "NeedleControllerProps";
        public NeedleControllerProperties Properties { get; set; }

        public int LiftPosition { get; set; } = 0;
        public int RotatorPosition { get; set; } = 0;
        public bool LiftPositionUnderfined { get; set; } = true;

        public NeedleController(ICommandExecutor executor) : base(executor)
        {
            Properties = new NeedleControllerProperties();
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<NeedleControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename));
        }

        //Чтение насроек из файла
        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<NeedleControllerProperties>.ReadXML(
                Path.Combine(path, filename));
            if (Properties == null)
                Properties = new NeedleControllerProperties();
        }

        public void TurnToTubeAndWaitTouch()
        {
            Logger.ControllerInfo($"[Needle] - Start turn to tube and waiting touch.");
            List<ICommand> commands = new List<ICommand>();

            // Поворот иглы до пробирки
            commands.Add(new SetSpeedCommand(Properties.RotatorStepper, (uint)Properties.RotatorSpeed));

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, Properties.RotatorStepsTurnToTube - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            RotatorPosition = Properties.RotatorStepsTurnToTube;

            commands.Clear();

            // Опускание иглы до жидкости в пробирке
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftSpeed));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.LiftSpeed } };
            commands.Add(new RunCncCommand(steppers, 0, 500, Protocol.ValueEdge.RisingEdge));

            // Дополнительное опускание иглы в жидкости
            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.LiftStepsGoDownAfterTouch } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            
            LiftPositionUnderfined = true; // ибо неизвестно, где он будет после касания жидкости в пробирке

            Logger.ControllerInfo($"[Needle] - Turn to tube and waiting touch finished.");
        }

        public void HomeRotator()
        {
            Logger.ControllerInfo($"[Needle] - Start rotator going to home.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Properties.RotatorStepper, 1000));

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, 100 } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            
            RotatorPosition = 0;

            Logger.ControllerInfo($"[Needle] - Rotator goint to home finished.");
        }

        public void HomeLift()
        {
            Logger.ControllerInfo($"[Needle] - Start lift going to home.");
            List<ICommand> commands = new List<ICommand>();

            
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, 1000));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, -200 } };
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
            commands.Add(new SetSpeedCommand(Properties.RotatorStepper, 50));

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, Properties.RotatorStepsTurnToWashing - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            commands.Clear();

            RotatorPosition = Properties.RotatorStepsTurnToWashing;

            if (LiftPositionUnderfined)
                HomeLift();

            // Опускание иглы до промывки
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, 500));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.LiftStepsGoDownToWashing - LiftPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);

            LiftPosition = Properties.LiftStepsGoDownToWashing;

            Logger.ControllerInfo($"[Needle] - Turn and going down to washing finished.");
        }

        public void GoDownAndPierceCartridge(CartridgeCell cartridgeCell, bool needSuction = true)
        {
            Logger.ControllerInfo($"[Needle] - Start going down and piercing cartridge.");

            List<ICommand> commands = new List<ICommand>();

            int steps = Properties.LiftStepsGoDownToCell;

            if (cartridgeCell == CartridgeCell.WhiteCell)
            {
                if (needSuction)
                    steps = Properties.LiftStepsGoDownToMixCellAtSuction;
                else
                    steps = Properties.LiftStepsGoDownToMixCell;
            }

            if (LiftPositionUnderfined)
                HomeLift();

            // Протыкание
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftSpeed));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, steps - LiftPosition } };
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

            int steps = Properties.LiftStepsGoDownToSafeLevel;

            commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftSpeed));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, steps - LiftPosition } };
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
                turnSteps = Properties.RotatorStepsTurnToMixCell;
            }
            else if(cell == CartridgeCell.FirstCell)
            {
                turnSteps = Properties.RotatorStepsTurnToFirstCell;
            }
            else if(cell == CartridgeCell.SecondCell)
            {
                turnSteps = Properties.RotatorStepsTurnToSecondCell;
            }
            else if(cell == CartridgeCell.ThirdCell)
            {
                turnSteps = Properties.RotatorStepsTurnToThirdCell;
            }

            commands.Add(new SetSpeedCommand(Properties.RotatorStepper, 50));

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, turnSteps - RotatorPosition } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            RotatorPosition = turnSteps;

            Logger.ControllerInfo($"[Needle] - Turn to cartridge finished.");
        }
    }
}
