using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.Elements;
using SteppersControlCore.Utils;

namespace SteppersControlCore.Controllers
{
    public class RotorControllerPropetries
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель ротора")]
        public int RotorStepper { get; set; } = 7;

        [Category("2. Скорость")]
        [DisplayName("Скорость ротора при движении домой")]
        public int RotorHomeSpeed { get; set; } = 100;

        [Category("2. Скорость")]
        [DisplayName("Скорость ротора при движении")]
        public int RotorSpeed { get; set; } = 50;

        [Category("3. Шаги")]
        [DisplayName("Шагов на одну ячейку")]
        public int StepsPerCell { get; set; } = 3400;

        [Category("3. Шаги")]
        [DisplayName("Шагов до загрузки (картриджа)")]
        public int[] StepsToLoad { get; set; } =
        {
            20000,
            20000,
            20000,
            20000,
            20000,
            20000,
            20000,
            20000,
            20000,
            20000
        };

        [Category("3. Шаги")]
        [DisplayName("Шагов до выгрузки")]
        public int StepsToUnload { get; set; } = 1000;

        [Category("3. Шаги")]
        [DisplayName("Шагов до wash-буфера")]
        public int StepsToWashBuffer { get; set; } = 1000;

        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы белой ячейки (центр)")]
        public int StepsToNeedleWhiteCenter { get; set; } = 10900;

        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 1-й ячейки (лево)")]
        public int StepsToNeedleLeft1 { get; set; } = 9750;
        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 1-й ячейки (право)")]
        public int StepsToNeedleRight1 { get; set; } = 10700;

        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 2-й ячейки (лево)")]
        public int StepsToNeedleLeft2 { get; set; } = 9200;
        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 2-й ячейки (право)")]
        public int StepsToNeedleRight2 { get; set; } = 9650;

        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 3-й ячейки (лево)")]
        public int StepsToNeedleLeft3 { get; set; } = 8630;
        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 3-й ячейки (право)")]
        public int StepsToNeedleRight3 { get; set; } = 9200;
        
        public RotorControllerPropetries()
        {

        }
    }

    public class RotorController : Controller
    {
        public RotorControllerPropetries Props { get; set; }

        const string filename = "RotorControllerProps";

        public RotorController() : base()
        {
            Props = new RotorControllerPropetries();
        }

        public void WriteXml()
        {
            XMLSerializeHelper<RotorControllerPropetries>.WriteXml(Props, filename);
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            Props = XMLSerializeHelper<RotorControllerPropetries>.ReadXML(filename);
        }

        public List<IAbstractCommand> Home()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, Props.RotorHomeSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, -Props.RotorHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            return commands;
        }

        public List<IAbstractCommand> MoveToWashBuffer()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, Props.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, Props.StepsToWashBuffer } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }

        public List<IAbstractCommand> MoveToUnload()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, Props.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, Props.StepsToUnload } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }
        
        public List<IAbstractCommand> MoveToLoad(int cellNumber, int position)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, Props.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, Props.StepsToLoad[position] } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }
        
        public enum CellPosition
        {
            CenterCell,
            CellLeft,
            CellRight
        };

        public List<IAbstractCommand> MoveCellUnderNeedle(int cellNumber, CartridgeCell cell, CellPosition position)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            commands.Add(new SetSpeedCommand(Props.RotorStepper, (uint)Props.RotorSpeed));

            int turnSteps = 0;

            if (cell == CartridgeCell.WhiteCell)
            {
                turnSteps = Props.StepsToNeedleWhiteCenter;
            }
            else if (cell == CartridgeCell.FirstCell)
            {
                turnSteps = (position == CellPosition.CellLeft) ?
                    Props.StepsToNeedleLeft1 : Props.StepsToNeedleRight1;
            }
            else if (cell == CartridgeCell.SecondCell)
            {
                turnSteps = (position == CellPosition.CellLeft) ?
                    Props.StepsToNeedleLeft2 : Props.StepsToNeedleRight2;
            }
            else if (cell == CartridgeCell.ThirdCell)
            {
                turnSteps = (position == CellPosition.CellLeft) ?
                    Props.StepsToNeedleLeft3 : Props.StepsToNeedleRight3;
            }

            steppers = new Dictionary<int, int>() { { Props.RotorStepper, turnSteps } };
            commands.Add(new MoveCncCommand(steppers));

            return commands;
        }
    }
}
