using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.ControllersConfiguration;
using AnalyzerControlCore.MachineControl;
using Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace AnalyzerControlCore.Units
{
    public class RotorUnit : AbstractUnit
    {
        public RotorControllerConfiguration Config { get; set; }

        public int Position { get; set; } = 0;

        public RotorUnit(ICommandExecutor executor) : base(executor)
        {
            Config = new RotorControllerConfiguration();
        }

        public void SaveConfiguration(string path)
        {
            XmlSerializeHelper<RotorControllerConfiguration>.WriteXml(Config, Path.Combine(path, nameof(RotorControllerConfiguration)) );
        }

        public void LoadConfiguration(string path)
        {
            Config = XmlSerializeHelper<RotorControllerConfiguration>.ReadXml( Path.Combine(path, nameof(RotorControllerConfiguration)) );

            if (Config == null)
                Config = new RotorControllerConfiguration();
        }

        public void Home()
        {
            Logger.ControllerInfo($"[Rotor] - Start homing.");
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, Config.RotorHomeSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, -Config.RotorHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = 0;

            Logger.ControllerInfo($"[Rotor] - Homing finished.");
        }

        public void PlaceCellUnderWashBuffer(int cartridgePosition)
        {
            Logger.ControllerInfo($"[Rotor] - Start placing cell under washing buffer.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Config.StepsToWashBuffer;
            turnSteps += Config.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, Config.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[Rotor] - Placing cell under washing buffer finished.");
        }

        public void PlaceCellAtDischarge(int cartridgePosition)
        {
            Logger.ControllerInfo($"[Rotor] - Start placing cell at discharger.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Config.StepsToUnload;
            turnSteps += Config.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, Config.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[Rotor] - Placing cell at discharger finished.");
        }
        
        public void PlaceCellAtCharge(int cartridgePosition, int chargePosition)
        {
            Logger.ControllerInfo($"[Rotor] - Start placing cell at charger.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Config.StepsToLoad[chargePosition];
            turnSteps += Config.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, Config.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, turnSteps - Position} };
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
            
            commands.Add(new SetSpeedCommand(Config.RotorStepper, (uint)Config.RotorSpeed));

            int turnSteps = 0;

            if (cartridgeCell == CartridgeCell.WhiteCell)
            {
                turnSteps = Config.StepsToNeedleWhiteCenter;
            }
            else if (cartridgeCell == CartridgeCell.FirstCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Config.StepsToNeedleLeft1;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Config.StepsToNeedleRight1;
                else
                {
                    turnSteps = Config.StepsToNeedleLeft1 +
                        (Config.StepsToNeedleRight1 - Config.StepsToNeedleLeft1) / 2;
                }
            }
            else if (cartridgeCell == CartridgeCell.SecondCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Config.StepsToNeedleLeft2;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Config.StepsToNeedleRight2;
                else
                {
                    turnSteps = Config.StepsToNeedleLeft2 +
                        (Config.StepsToNeedleRight2 - Config.StepsToNeedleLeft2) / 2;
                }
            }
            else if (cartridgeCell == CartridgeCell.ThirdCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Config.StepsToNeedleLeft3;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Config.StepsToNeedleRight3;
                else
                {
                    turnSteps = Config.StepsToNeedleLeft3 +
                        (Config.StepsToNeedleRight3 - Config.StepsToNeedleLeft3) / 2;
                }
            }

            turnSteps += Config.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Config.RotorStepper, turnSteps - Position } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[Rotor] - Placing cell under needle finished.");
        }
    }
}
