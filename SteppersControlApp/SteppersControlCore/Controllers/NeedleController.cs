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
            List<ICommand> commands = new List<ICommand>();

            // Поворот иглы до пробирки
            commands.Add(new SetSpeedCommand(Properties.RotatorStepper, (uint)Properties.RotatorSpeed));

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, Properties.RotatorStepsTurnToTube - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            // Опускание иглы до жидкости в пробирке
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftSpeed));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.LiftSpeed } };
            commands.Add(new RunCncCommand(steppers, 0, 500, Protocol.ValueEdge.RisingEdge));

            // Дополнительное опускание иглы в жидкости
            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.LiftStepsGoDownAfterTouch } };
            commands.Add(new MoveCncCommand(steppers));

            RotatorPosition = Properties.RotatorStepsTurnToTube;
            LiftPosition = -1; // ибо неизвестн, где он будет после касания жидкости в пробирке

            executor.WaitExecution(commands);
        }

        public void HomeRotator()
        {
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Properties.RotatorStepper, 1000));

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, 100 } };
            commands.Add(new HomeCncCommand(steppers));

            LiftPosition = 0;
            RotatorPosition = 0;

            executor.WaitExecution(commands);
        }

        public void HomeLift()
        {
            List<ICommand> commands = new List<ICommand>();

            
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, 1000));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, -200 } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
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
            List<ICommand> commands = new List<ICommand>();

            // Поворот иглы до промывки
            commands.Add(new SetSpeedCommand(Properties.RotatorStepper, 50));

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, Properties.RotatorStepsTurnToWashing - RotatorPosition} };
            commands.Add(new MoveCncCommand(steppers));

            // Опускание иглы до промывки
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, 500));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.LiftStepsGoDownToWashing - LiftPosition } };
            commands.Add(new MoveCncCommand(steppers));

            RotatorPosition = Properties.RotatorStepsTurnToWashing;
            LiftPosition = Properties.LiftStepsGoDownToWashing;

            executor.WaitExecution(commands);
        }

        public enum FromPosition
        {
            Home,
            Tube,
            Washing,
            WhiteCell,
            FirstCell,
            SecondCell,
            THirdCell
        }

        //TODO: выяснить, используем относительное количество шагов или нет!!!
        public void GoDownAndBrokeCartridge()
        {
            List<ICommand> commands = new List<ICommand>();

            // Протыкание
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftSpeed));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.LiftStepsGoDownAtBroke } };
            commands.Add(new MoveCncCommand(steppers));

            //LiftStepperPosition = Props.StepsOnBroke;

            executor.WaitExecution(commands);
        }

        //TODO: Доделать, ибо говно, убрать from position
        public void TurnToCartridge(FromPosition fromPosition, CartridgeCell cell)
        {
            List<ICommand> commands = new List<ICommand>();
            
            int turnSteps = 0;
            int upSteps = 0;

            bool fromCartridge = false;

            if(fromPosition == FromPosition.WhiteCell)
            {
                fromCartridge = true;
                upSteps = Properties.LiftStepsGoDownToMixCell;
            }
            else if(fromPosition == FromPosition.FirstCell)
            {
                fromCartridge = true;
                upSteps = Properties.LiftStepsGoDownToCell;
            }
            else if (fromPosition == FromPosition.SecondCell)
            {
                fromCartridge = true;
                upSteps = Properties.LiftStepsGoDownToCell;
            }
            else if (fromPosition == FromPosition.THirdCell)
            {
                fromCartridge = true;
                upSteps = Properties.LiftStepsGoDownToCell;
            }

            int downSteps = Properties.LiftStepsGoDownToCell;

            if(cell == CartridgeCell.WhiteCell)
            {
                turnSteps = Properties.RotatorStepsTurnToMixCell - RotatorPosition;
                RotatorPosition = Properties.RotatorStepsTurnToMixCell;

                downSteps = Properties.LiftStepsGoDownToMixCell;
            }
            else if(cell == CartridgeCell.FirstCell)
            {
                turnSteps = Properties.RotatorStepsTurnToFirstCell - RotatorPosition;
                RotatorPosition = Properties.RotatorStepsTurnToFirstCell;
            }
            else if(cell == CartridgeCell.SecondCell)
            {
                turnSteps = Properties.RotatorStepsTurnToSecondCell - RotatorPosition;
                RotatorPosition = Properties.RotatorStepsTurnToSecondCell;
            }
            else if(cell == CartridgeCell.ThirdCell)
            {
                turnSteps = Properties.RotatorStepsTurnToThirdCell - RotatorPosition;
                RotatorPosition = Properties.RotatorStepsTurnToThirdCell;
            }

            if(fromCartridge)
            {
                // Подъем иглы
                commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftSpeed));

                steppers = new Dictionary<int, int>() { { Properties.LiftStepper, -Properties.LiftStepsGoDownAtBroke } };
                commands.Add(new MoveCncCommand(steppers));
            }
            else
            {
                downSteps = downSteps - Properties.LiftStepsGoDownAtBroke;

                // Подъем иглы
                commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftSpeed));

                steppers = new Dictionary<int, int>() { { Properties.LiftStepper, -100 } };
                commands.Add(new HomeCncCommand(steppers));
            }

            // Поворот иглы до картриджа
            commands.Add(new SetSpeedCommand(Properties.RotatorStepper, 50));

            steppers = new Dictionary<int, int>() { { Properties.RotatorStepper, turnSteps } };
            commands.Add(new MoveCncCommand(steppers));

            if(!fromCartridge)
            {
                // Доезд до картриджа (над ним)
                commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftSpeed));

                steppers = new Dictionary<int, int>() { { Properties.LiftStepper, downSteps } };
                commands.Add(new MoveCncCommand(steppers));
            }

            executor.WaitExecution(commands);
        }
    }
}
