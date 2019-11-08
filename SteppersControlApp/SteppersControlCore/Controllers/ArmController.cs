using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.Elements;

using SteppersControlCore.ControllersProperties;

using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using SteppersControlCore.Utils;

namespace SteppersControlCore.Controllers
{
    public class ArmController : ControllerBase
    {
        const string filename = "ArmControllerProps";
        public ArmControllerProperties Properties { get; set; }

        public int LiftStepperPosition { get; set; } = 0;
        public int TurnStepperPosition { get; set; } = 0;

        public ArmController() : base()
        {
            Properties = new ArmControllerProperties();
        }

        public void WriteXml()
        {
            XMLSerializeHelper<ArmControllerProperties>.WriteXml(Properties, filename);
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            Properties = XMLSerializeHelper<ArmControllerProperties>.ReadXML(filename);
        }

        public List<IAbstractCommand> MoveOnTube()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Поворот иглы до пробирки
            commands.Add(new SetSpeedCommand(Properties.TurnStepper, (uint)Properties.TurnStepperSpeed));

            steppers = new Dictionary<int, int>() { { Properties.TurnStepper, Properties.StepsToTube - TurnStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            // Опускание иглы до жидкости в пробирке
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftStepperSpeed));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.LiftStepperSpeed } };
            commands.Add(new RunCncCommand(steppers, 0, 500, Protocol.ValueEdge.RisingEdge));

            // Дополнительное опускание иглы в жидкости
            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.StepsToTubeAfterTouch } };
            commands.Add(new MoveCncCommand(steppers));

            TurnStepperPosition = Properties.StepsToTube;
            LiftStepperPosition = -1; // ибо неизвестн, где он будет после касания жидкости в пробирке

            return commands;
        }

        public List<IAbstractCommand> Home()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Поднятие иглы
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, 1000));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, -200 } };
            commands.Add(new HomeCncCommand(steppers));
            
            // Поворот иглы
            commands.Add(new SetSpeedCommand(Properties.TurnStepper, 1000));

            steppers = new Dictionary<int, int>() { { Properties.TurnStepper, 100 } };
            commands.Add(new HomeCncCommand(steppers));

            LiftStepperPosition = 0;
            TurnStepperPosition = 0;

            return commands;
        }

        public List<IAbstractCommand> MoveOnWashing()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Поворот иглы до промывки
            commands.Add(new SetSpeedCommand(Properties.TurnStepper, 50));

            steppers = new Dictionary<int, int>() { { Properties.TurnStepper, Properties.StepsTurnToWashing - TurnStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            // Опускание иглы до промывки
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, 500));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.StepsDownToWashing - LiftStepperPosition } };
            commands.Add(new MoveCncCommand(steppers));

            TurnStepperPosition = Properties.StepsTurnToWashing;
            LiftStepperPosition = Properties.StepsDownToWashing;

            return commands;
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
        public List<IAbstractCommand> BrokeCartridge()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Протыкание
            commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftStepperSpeed));

            steppers = new Dictionary<int, int>() { { Properties.LiftStepper, Properties.StepsOnBroke } };
            commands.Add(new MoveCncCommand(steppers));

            //LiftStepperPosition = Props.StepsOnBroke;

            return commands;
        }

        //TODO: Доделать, ибо говно, убрать from position
        public List<IAbstractCommand> MoveToCartridge(FromPosition fromPosition, CartridgeCell cell)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            int turnSteps = 0;
            int upSteps = 0;

            bool fromCartridge = false;

            if(fromPosition == FromPosition.WhiteCell)
            {
                fromCartridge = true;
                upSteps = Properties.StepsDownToMixCell;
            }
            else if(fromPosition == FromPosition.FirstCell)
            {
                fromCartridge = true;
                upSteps = Properties.StepsDownToCell;
            }
            else if (fromPosition == FromPosition.SecondCell)
            {
                fromCartridge = true;
                upSteps = Properties.StepsDownToCell;
            }
            else if (fromPosition == FromPosition.THirdCell)
            {
                fromCartridge = true;
                upSteps = Properties.StepsDownToCell;
            }

            int downSteps = Properties.StepsDownToCell;

            if(cell == CartridgeCell.WhiteCell)
            {
                turnSteps = Properties.StepsToMixCell - TurnStepperPosition;
                TurnStepperPosition = Properties.StepsToMixCell;

                downSteps = Properties.StepsDownToMixCell;
            }
            else if(cell == CartridgeCell.FirstCell)
            {
                turnSteps = Properties.StepsToFirstCell - TurnStepperPosition;
                TurnStepperPosition = Properties.StepsToFirstCell;
            }
            else if(cell == CartridgeCell.SecondCell)
            {
                turnSteps = Properties.StepsToSecondCell - TurnStepperPosition;
                TurnStepperPosition = Properties.StepsToSecondCell;
            }
            else if(cell == CartridgeCell.ThirdCell)
            {
                turnSteps = Properties.StepsToThirdCell - TurnStepperPosition;
                TurnStepperPosition = Properties.StepsToThirdCell;
            }

            if(fromCartridge)
            {
                // Подъем иглы
                commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftStepperSpeed));

                steppers = new Dictionary<int, int>() { { Properties.LiftStepper, -Properties.StepsOnBroke } };
                commands.Add(new MoveCncCommand(steppers));
            }
            else
            {
                downSteps = downSteps - Properties.StepsOnBroke;

                // Подъем иглы
                commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftStepperSpeed));

                steppers = new Dictionary<int, int>() { { Properties.LiftStepper, -100 } };
                commands.Add(new HomeCncCommand(steppers));
            }

            // Поворот иглы до картриджа
            commands.Add(new SetSpeedCommand(Properties.TurnStepper, 50));

            steppers = new Dictionary<int, int>() { { Properties.TurnStepper, turnSteps } };
            commands.Add(new MoveCncCommand(steppers));

            if(!fromCartridge)
            {
                // Доезд до картриджа (над ним)
                commands.Add(new SetSpeedCommand(Properties.LiftStepper, (uint)Properties.LiftStepperSpeed));

                steppers = new Dictionary<int, int>() { { Properties.LiftStepper, downSteps } };
                commands.Add(new MoveCncCommand(steppers));
            }

            return commands;
        }
    }
}
