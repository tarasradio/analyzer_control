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

        public int Position { get; set; } = 0;

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
            Logger.ControllerInfo($"[Rotor] - Start homing.");
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorHomeSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, -Properties.RotorHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = 0;

            Logger.ControllerInfo($"[Rotor] - Homing finished.");
        }

        public void PlaceCellUnderWashBuffer(int cartridgePosition)
        {
            Logger.ControllerInfo($"[Rotor] - Start placing cell under washing buffer.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Properties.StepsToWashBuffer;
            turnSteps += Properties.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[Rotor] - Placing cell under washing buffer finished.");
        }

        public void PlaceCellAtDischarge(int cartridgePosition)
        {
            Logger.ControllerInfo($"[Rotor] - Start placing cell at discharger.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Properties.StepsToUnload;
            turnSteps += Properties.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[Rotor] - Placing cell at discharger finished.");
        }
        
        public void PlaceCellAtCharge(int cartridgePosition, int chargePosition)
        {
            Logger.ControllerInfo($"[Rotor] - Start placing cell at charger.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Properties.StepsToLoad[chargePosition];
            turnSteps += Properties.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, Properties.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[Rotor] - Placing cell at charger finished.");
        }
        
        public enum CellPosition
        {
            CellCenter,
            CellLeft,
            CellRight
        };

        /// <summary>
        /// Поместить ячеку картриджа в роторе под иглу
        /// </summary>
        /// <param name="cartridgePosition">Номер позиции картриджа в роторе</param>
        /// <param name="cartridgeCell">Ячейка катриджа</param>
        /// <param name="cellPosition">Позиция ячейки картриджа</param>
        public void PlaceCellUnderNeedle(int cartridgePosition, CartridgeCell cartridgeCell, CellPosition cellPosition = CellPosition.CellCenter)
        {
            Logger.ControllerInfo($"[Rotor] - Start placing cell under needle.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Properties.RotorStepper, (uint)Properties.RotorSpeed));

            int turnSteps = 0;

            if (cartridgeCell == CartridgeCell.WhiteCell)
            {
                turnSteps = Properties.StepsToNeedleWhiteCenter;
            }
            else if (cartridgeCell == CartridgeCell.FirstCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Properties.StepsToNeedleLeft1;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Properties.StepsToNeedleRight1;
                else
                {
                    turnSteps = Properties.StepsToNeedleLeft1 +
                        (Properties.StepsToNeedleRight1 - Properties.StepsToNeedleLeft1) / 2;
                }
            }
            else if (cartridgeCell == CartridgeCell.SecondCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Properties.StepsToNeedleLeft2;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Properties.StepsToNeedleRight2;
                else
                {
                    turnSteps = Properties.StepsToNeedleLeft2 +
                        (Properties.StepsToNeedleRight2 - Properties.StepsToNeedleLeft2) / 2;
                }
            }
            else if (cartridgeCell == CartridgeCell.ThirdCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Properties.StepsToNeedleLeft3;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Properties.StepsToNeedleRight3;
                else
                {
                    turnSteps = Properties.StepsToNeedleLeft3 +
                        (Properties.StepsToNeedleRight3 - Properties.StepsToNeedleLeft3) / 2;
                }
            }

            turnSteps += Properties.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Properties.RotorStepper, turnSteps - Position } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[Rotor] - Placing cell under needle finished.");
        }
    }
}
