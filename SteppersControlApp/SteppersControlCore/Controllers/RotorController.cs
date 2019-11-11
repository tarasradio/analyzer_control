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

using SteppersControlCore.ControllersProperties;

namespace SteppersControlCore.Controllers
{
    public class RotorController : ControllerBase
    {
        public RotorControllerProperties Properties { get; set; }

        public int RotorStepperPosition { get; set; } = 0;

        const string filename = "RotorControllerProps";

        public RotorController() : base()
        {
            Properties = new RotorControllerProperties();
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<RotorControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename));
        }

        //Чтение насроек из файла
        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<RotorControllerProperties>.ReadXML(
                Path.Combine(path, filename));

            if (Properties == null)
                Properties = new RotorControllerProperties();
        }

        public List<IAbstractCommand> Home()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorHomeSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, -Properties.RotorHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            RotorStepperPosition = 0;

            return commands;
        }

        public List<IAbstractCommand> MoveToWashBuffer()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.StepsToWashBuffer - RotorStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            RotorStepperPosition = Properties.StepsToWashBuffer;

            return commands;
        }

        public List<IAbstractCommand> MoveToUnload()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.StepsToUnload - RotorStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            RotorStepperPosition = Properties.StepsToUnload;

            return commands;
        }
        
        public List<IAbstractCommand> MoveToLoad(int cellNumber, int position)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.StepsToLoad[position] - RotorStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            RotorStepperPosition = Properties.StepsToLoad[position];

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
            
            commands.Add(new SetSpeedCommand(Properties.RotorStepper, (uint)Properties.RotorSpeed));

            int turnSteps = 0;

            if (cell == CartridgeCell.WhiteCell)
            {
                turnSteps = Properties.StepsToNeedleWhiteCenter;
            }
            else if (cell == CartridgeCell.FirstCell)
            {
                turnSteps = (position == CellPosition.CellLeft) ?
                    Properties.StepsToNeedleLeft1 : Properties.StepsToNeedleRight1;
            }
            else if (cell == CartridgeCell.SecondCell)
            {
                turnSteps = (position == CellPosition.CellLeft) ?
                    Properties.StepsToNeedleLeft2 : Properties.StepsToNeedleRight2;
            }
            else if (cell == CartridgeCell.ThirdCell)
            {
                turnSteps = (position == CellPosition.CellLeft) ?
                    Properties.StepsToNeedleLeft3 : Properties.StepsToNeedleRight3;
            }

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, turnSteps - RotorStepperPosition } };
            commands.Add(new MoveCncCommand(steppers));

            RotorStepperPosition = turnSteps;

            return commands;
        }
    }
}
