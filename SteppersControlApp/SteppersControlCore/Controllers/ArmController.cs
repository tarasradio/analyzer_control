using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.Elements;

using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using SteppersControlCore.Utils;

namespace SteppersControlCore.Controllers
{
    public class ArmControllerPropetries
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель подъема")]
        public int LiftStepper { get; set; } = 17;
        
        [Category("1. Двигатели")]
        [DisplayName("Двигатель поворота")]
        public int TurnStepper { get; set; } = 8;
        

        [Category("2. Скорость")]
        [DisplayName("Скорость подъема / опускания")]
        public int LiftStepperSpeed { get; set; } = 500;
        [Category("2. Скорость")]
        [DisplayName("Скорость поворота")]
        public int TurnStepperSpeed { get; set; } = 50;

        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до пробирки")]
        public int StepsToTube { get; set; } = 15500;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до промывки")]
        public int StepsTurnToWashing { get; set; } = 55900;
        

        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до белой ячейки")]
        public int StepsToMixCell { get; set; } = 46600; // уточнить
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 1-й ячейки")]
        public int StepsToFirstCell { get; set; } = 47000; // уточнить
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 2-й ячейки")]
        public int StepsToSecondCell { get; set; } = 48000; // уточнить
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 3-й ячейки")]
        public int StepsToThirdCell { get; set; } = 48900; // уточнить

        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов после касания жидкости в пробирке")]
        public int StepsToTubeAfterTouch { get; set; } = 500;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов опускания до промывки")]
        public int StepsDownToWashing { get; set; } = 5500;

        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до 1-3-й ячеек")]
        public int StepsDownToCell { get; set; } = 297000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до белой ячейки")]
        public int StepsDownToMixCell { get; set; } = 272000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до белой ячейки (при заборе)")]
        public int StepsDownToMixCellOnSuction { get; set; } = 272000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов не доходя до картриджа")]
        public int StepsOnBroke { get; set; } = 70000;

        public ArmControllerPropetries()
        {

        }
    }

    public class ArmController : ControllerBase
    {
        const string filename = "ArmControllerProps";
        public ArmControllerPropetries Props { get; set; }

        public int LiftStepperPosition { get; set; } = 0;
        public int TurnStepperPosition { get; set; } = 0;

        public ArmController() : base()
        {
            Props = new ArmControllerPropetries();
        }

        public void WriteXml()
        {
            XMLSerializeHelper<ArmControllerPropetries>.WriteXml(Props, filename);
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            Props = XMLSerializeHelper<ArmControllerPropetries>.ReadXML(filename);
        }

        public List<IAbstractCommand> MoveOnTube()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Поворот иглы до пробирки
            commands.Add(new SetSpeedCommand(Props.TurnStepper, (uint)Props.TurnStepperSpeed));

            steppers = new Dictionary<int, int>() { { Props.TurnStepper, Props.StepsToTube - TurnStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            // Опускание иглы до жидкости в пробирке
            commands.Add(new SetSpeedCommand(Props.LiftStepper, (uint)Props.LiftStepperSpeed));

            steppers = new Dictionary<int, int>() { { Props.LiftStepper, Props.LiftStepperSpeed } };
            commands.Add(new RunCncCommand(steppers, 0, 500, Protocol.ValueEdge.RisingEdge));

            // Дополнительное опускание иглы в жидкости
            steppers = new Dictionary<int, int>() { { Props.LiftStepper, Props.StepsToTubeAfterTouch } };
            commands.Add(new MoveCncCommand(steppers));

            TurnStepperPosition = Props.StepsToTube;
            LiftStepperPosition = -1; // ибо неизвестн, где он будет после касания жидкости в пробирке

            return commands;
        }

        public List<IAbstractCommand> Home()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Поднятие иглы
            commands.Add(new SetSpeedCommand(Props.LiftStepper, 1000));

            steppers = new Dictionary<int, int>() { { Props.LiftStepper, -200 } };
            commands.Add(new HomeCncCommand(steppers));
            
            // Поворот иглы
            commands.Add(new SetSpeedCommand(Props.TurnStepper, 1000));

            steppers = new Dictionary<int, int>() { { Props.TurnStepper, 100 } };
            commands.Add(new HomeCncCommand(steppers));

            LiftStepperPosition = 0;
            TurnStepperPosition = 0;

            return commands;
        }

        public List<IAbstractCommand> MoveOnWashing()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Поворот иглы до промывки
            commands.Add(new SetSpeedCommand(Props.TurnStepper, 50));

            steppers = new Dictionary<int, int>() { { Props.TurnStepper, Props.StepsTurnToWashing - TurnStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            // Опускание иглы до промывки
            commands.Add(new SetSpeedCommand(Props.LiftStepper, 500));

            steppers = new Dictionary<int, int>() { { Props.LiftStepper, Props.StepsDownToWashing - LiftStepperPosition } };
            commands.Add(new MoveCncCommand(steppers));

            TurnStepperPosition = Props.StepsTurnToWashing;
            LiftStepperPosition = Props.StepsDownToWashing;

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
            commands.Add(new SetSpeedCommand(Props.LiftStepper, (uint)Props.LiftStepperSpeed));

            steppers = new Dictionary<int, int>() { { Props.LiftStepper, Props.StepsOnBroke } };
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
                upSteps = Props.StepsDownToMixCell;
            }
            else if(fromPosition == FromPosition.FirstCell)
            {
                fromCartridge = true;
                upSteps = Props.StepsDownToCell;
            }
            else if (fromPosition == FromPosition.SecondCell)
            {
                fromCartridge = true;
                upSteps = Props.StepsDownToCell;
            }
            else if (fromPosition == FromPosition.THirdCell)
            {
                fromCartridge = true;
                upSteps = Props.StepsDownToCell;
            }

            int downSteps = Props.StepsDownToCell;

            if(cell == CartridgeCell.WhiteCell)
            {
                turnSteps = Props.StepsToMixCell - TurnStepperPosition;
                TurnStepperPosition = Props.StepsToMixCell;

                downSteps = Props.StepsDownToMixCell;
            }
            else if(cell == CartridgeCell.FirstCell)
            {
                turnSteps = Props.StepsToFirstCell - TurnStepperPosition;
                TurnStepperPosition = Props.StepsToFirstCell;
            }
            else if(cell == CartridgeCell.SecondCell)
            {
                turnSteps = Props.StepsToSecondCell - TurnStepperPosition;
                TurnStepperPosition = Props.StepsToSecondCell;
            }
            else if(cell == CartridgeCell.ThirdCell)
            {
                turnSteps = Props.StepsToThirdCell - TurnStepperPosition;
                TurnStepperPosition = Props.StepsToThirdCell;
            }

            if(fromCartridge)
            {
                // Подъем иглы
                commands.Add(new SetSpeedCommand(Props.LiftStepper, (uint)Props.LiftStepperSpeed));

                steppers = new Dictionary<int, int>() { { Props.LiftStepper, -Props.StepsOnBroke } };
                commands.Add(new MoveCncCommand(steppers));
            }
            else
            {
                downSteps = downSteps - Props.StepsOnBroke;

                // Подъем иглы
                commands.Add(new SetSpeedCommand(Props.LiftStepper, (uint)Props.LiftStepperSpeed));

                steppers = new Dictionary<int, int>() { { Props.LiftStepper, -100 } };
                commands.Add(new HomeCncCommand(steppers));
            }

            // Поворот иглы до картриджа
            commands.Add(new SetSpeedCommand(Props.TurnStepper, 50));

            steppers = new Dictionary<int, int>() { { Props.TurnStepper, turnSteps } };
            commands.Add(new MoveCncCommand(steppers));

            if(!fromCartridge)
            {
                // Доезд до картриджа (над ним)
                commands.Add(new SetSpeedCommand(Props.LiftStepper, (uint)Props.LiftStepperSpeed));

                steppers = new Dictionary<int, int>() { { Props.LiftStepper, downSteps } };
                commands.Add(new MoveCncCommand(steppers));
            }

            return commands;
        }
    }
}
