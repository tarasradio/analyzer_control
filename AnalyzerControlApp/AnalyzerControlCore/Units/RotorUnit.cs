using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerControlCore.MachineControl;
using AnalyzerDomain.Entyties;
using Infrastructure;
using System.Collections.Generic;

namespace AnalyzerControlCore.Units
{
    public class RotorUnit : UnitBase<RotorConfiguration>
    {
        public int Position { get; set; } = 0;

        public RotorUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        {

        }

        public void Home()
        {
            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Start homing.");
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, Options.RotorHomeSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, -Options.RotorHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = 0;

            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Homing finished.");
        }

        public void PlaceCellUnderWashBuffer(int cartridgePosition)
        {
            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Start placing cell under washing buffer.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Options.StepsToWashBuffer;
            turnSteps += Options.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, Options.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Placing cell under washing buffer finished.");
        }

        public void PlaceCellAtDischarge(int cartridgePosition)
        {
            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Start placing cell at discharger.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Options.StepsToUnload;
            turnSteps += Options.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, Options.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Placing cell at discharger finished.");
        }
        
        public void PlaceCellAtCharge(int cartridgePosition, int chargePosition)
        {
            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Start placing cell at charger.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Options.StepsToLoad[chargePosition];
            turnSteps += Options.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, Options.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Placing cell at charger finished.");
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
            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Start placing cell under needle.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Options.RotorStepper, (uint)Options.RotorSpeed));

            int turnSteps = 0;

            if (cartridgeCell == CartridgeCell.ResultCell)
            {
                turnSteps = Options.StepsToNeedleResultCenter;
            }
            else if (cartridgeCell == CartridgeCell.MixCell)
            {
                turnSteps = Options.StepsToNeedleWhiteCenter;
            }
            else if (cartridgeCell == CartridgeCell.FirstCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Options.StepsToNeedleLeft1;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Options.StepsToNeedleRight1;
                else
                {
                    turnSteps = Options.StepsToNeedleLeft1 +
                        (Options.StepsToNeedleRight1 - Options.StepsToNeedleLeft1) / 2;
                }
            }
            else if (cartridgeCell == CartridgeCell.SecondCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Options.StepsToNeedleLeft2;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Options.StepsToNeedleRight2;
                else
                {
                    turnSteps = Options.StepsToNeedleLeft2 +
                        (Options.StepsToNeedleRight2 - Options.StepsToNeedleLeft2) / 2;
                }
            }
            else if (cartridgeCell == CartridgeCell.ThirdCell)
            {
                if (cellPosition == CellPosition.CellLeft)
                    turnSteps = Options.StepsToNeedleLeft3;
                else if (cellPosition == CellPosition.CellRight)
                    turnSteps = Options.StepsToNeedleRight3;
                else
                {
                    turnSteps = Options.StepsToNeedleLeft3 +
                        (Options.StepsToNeedleRight3 - Options.StepsToNeedleLeft3) / 2;
                }
            }

            turnSteps += Options.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, turnSteps - Position } };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.ControllerInfo($"[{nameof(RotorUnit)}] - Placing cell under needle finished.");
        }
    }
}
