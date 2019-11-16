using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.ControllersProperties;
using SteppersControlCore.Elements;
using SteppersControlCore.Interfaces;
using SteppersControlCore.Utils;
using System.Collections.Generic;
using System.IO;

namespace SteppersControlCore.Controllers
{
    public class RotorController : ControllerBase
    {
        public RotorControllerProperties Properties { get; set; }

        public int RotorStepperPosition { get; set; } = 0;

        const string filename = "RotorControllerProps";

        public RotorController(ICommandExecutor executor) : base(executor)
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

        public void Home()
        {
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorHomeSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, -Properties.RotorHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            RotorStepperPosition = 0;

            executor.WaitExecution(commands);
        }

        public void PlaceCellUnderWashBuffer()
        {
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.StepsToWashBuffer - RotorStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            RotorStepperPosition = Properties.StepsToWashBuffer;

            executor.WaitExecution(commands);
        }

        public void PlaceCellAtDischarge()
        {
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.StepsToUnload - RotorStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            RotorStepperPosition = Properties.StepsToUnload;

            executor.WaitExecution(commands);
        }
        
        public void PlaceCellAtCharge(int cellNumber, int chargePosition)
        {
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.StepsToLoad[chargePosition] - RotorStepperPosition} };
            commands.Add(new MoveCncCommand(steppers));

            RotorStepperPosition = Properties.StepsToLoad[chargePosition];

            executor.WaitExecution(commands);
        }
        
        public enum CellPosition
        {
            CenterCell,
            CellLeft,
            CellRight
        };

        public void PlaceCellUnderNeedle(int cellNumber, CartridgeCell cell, CellPosition position)
        {
            List<ICommand> commands = new List<ICommand>();
            
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

            executor.WaitExecution(commands);
        }
    }
}
