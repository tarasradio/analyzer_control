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
        [DisplayName("Скорость подъема / опускания")]
        public int LiftStepperSpeed { get; set; } = 500;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель поврота")]
        public int TurnStepper { get; set; } = 8;
        [Category("1. Двигатели")]
        [DisplayName("Скорость поворота")]
        public int TurnStepperSpeed { get; set; } = 50;

        [Category("2. Шаги")]
        [DisplayName("Шагов до пробирки")]
        public int StepsToTube { get; set; } = 15500;
        [Category("2. Шаги")]
        [DisplayName("Шагов после касания жидкости в пробирке")]
        public int StepsToTubeAfterTouch { get; set; } = 500;
        [Category("2. Шаги")]
        [DisplayName("Шагов до промывки")]
        public int StepsTurnToWashing { get; set; } = 55900;
        [Category("2. Шаги")]
        [DisplayName("Шагов опускания до промывки")]
        public int StepsDownToWashing { get; set; } = 5500;

        [Category("2. Шаги")]
        [DisplayName("Шагов до белой ячейки")]
        public int StepsToMixCell { get; set; } = 46600; // уточнить
        [Category("2. Шаги")]
        [DisplayName("Шагов до 1-й ячейки")]
        public int StepsToFirstCell { get; set; } = 47000; // уточнить
        [Category("2. Шаги")]
        [DisplayName("Шагов до 2-й ячейки")]
        public int StepsToSecondCell { get; set; } = 48000; // уточнить
        [Category("2. Шаги")]
        [DisplayName("Шагов до 3-й ячейки")]
        public int StepsToThirdCell { get; set; } = 48900; // уточнить

        [Category("2. Шаги")]
        [DisplayName("Шагов для опускания до 1-3-й ячеек")]
        public int StepsDownToCell { get; set; } = 297000;
        [Category("2. Шаги")]
        [DisplayName("Шагов для опускания до белой ячейки")]
        public int StepsDownToMixCell { get; set; } = 272000;
        [Category("2. Шаги")]
        [DisplayName("Шагов для опускания до белой ячейки (при заборе)")]
        public int StepsDownToMixCellOnSuction { get; set; } = 272000;
        [Category("2. Шаги")]
        [DisplayName("Шагов не доходя до картриджа")]
        public int StepsOnBroke { get; set; } = 70000;

        public ArmControllerPropetries()
        {

        }
    }

    public class ArmController : Controller
    {
        const string filename = "ArmControllerProps";
        public ArmControllerPropetries Props { get; set; }

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

            steppers = new Dictionary<int, int>() { { Props.TurnStepper, -Props.StepsToTube } };
            commands.Add(new MoveCncCommand(steppers));

            // Опускание иглы до жидкости в пробирке
            commands.Add(new SetSpeedCommand(Props.LiftStepper, (uint)Props.LiftStepperSpeed));

            steppers = new Dictionary<int, int>() { { Props.LiftStepper, Props.LiftStepperSpeed } };
            commands.Add(new RunCncCommand(steppers, 0, 500, Protocol.ValueEdge.RisingEdge));

            // Дополнительное опускание иглы в жидкости
            steppers = new Dictionary<int, int>() { { Props.LiftStepper, Props.StepsToTubeAfterTouch } };
            commands.Add(new MoveCncCommand(steppers));

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

            return commands;
        }

        public List<IAbstractCommand> MoveOnWashing()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Поворот иглы до промывки
            commands.Add(new SetSpeedCommand(Props.TurnStepper, 50));

            steppers = new Dictionary<int, int>() { { Props.TurnStepper, -Props.StepsTurnToWashing } };
            commands.Add(new MoveCncCommand(steppers));

            // Опускание иглы до промывки
            commands.Add(new SetSpeedCommand(Props.LiftStepper, 500));

            steppers = new Dictionary<int, int>() { { Props.LiftStepper, Props.StepsDownToWashing } };
            commands.Add(new MoveCncCommand(steppers));

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

        public List<IAbstractCommand> BrokeCartridge()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            // Протыкание
            commands.Add(new SetSpeedCommand(Props.LiftStepper, (uint)Props.LiftStepperSpeed));

            steppers = new Dictionary<int, int>() { { Props.LiftStepper, Props.StepsOnBroke } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }

        public List<IAbstractCommand> MoveToCartridge(FromPosition fromPosition, CartridgeCell cell)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            int turnSteps = 0;
            int upSteps = 0;

            bool fromCartridge = false;

            if(fromPosition == FromPosition.Home)
            {
                turnSteps = 0;
            }
            else if(fromPosition == FromPosition.Tube)
            {
                turnSteps = Props.StepsToTube;
            }
            else if(fromPosition == FromPosition.Washing)
            {
                turnSteps = Props.StepsTurnToWashing;
            }
            else if(fromPosition == FromPosition.WhiteCell)
            {
                fromCartridge = true;
                turnSteps = Props.StepsToMixCell;
                upSteps = Props.StepsDownToMixCell;
            }
            else if(fromPosition == FromPosition.FirstCell)
            {
                fromCartridge = true;
                turnSteps = Props.StepsToFirstCell;
                upSteps = Props.StepsDownToCell;
            }
            else if (fromPosition == FromPosition.SecondCell)
            {
                fromCartridge = true;
                turnSteps = Props.StepsToSecondCell;
                upSteps = Props.StepsDownToCell;
            }
            else if (fromPosition == FromPosition.THirdCell)
            {
                fromCartridge = true;
                turnSteps = Props.StepsToThirdCell;
                upSteps = Props.StepsDownToCell;
            }

            int downSteps = Props.StepsDownToCell;

            if(cell == CartridgeCell.WhiteCell)
            {
                turnSteps = Props.StepsToMixCell - turnSteps;
                downSteps = Props.StepsDownToMixCell;
            }
            else if(cell == CartridgeCell.FirstCell)
            {
                turnSteps = Props.StepsToFirstCell - turnSteps;
            }
            else if(cell == CartridgeCell.SecondCell)
            {
                turnSteps = Props.StepsToSecondCell - turnSteps;
            }
            else if(cell == CartridgeCell.ThirdCell)
            {
                turnSteps = Props.StepsToThirdCell - turnSteps;
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

            steppers = new Dictionary<int, int>() { { Props.TurnStepper, -turnSteps } };
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
