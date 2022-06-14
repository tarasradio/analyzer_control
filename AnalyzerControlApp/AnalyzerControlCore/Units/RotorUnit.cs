using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerCommunication.CommunicationProtocol.StepperCommands;
using AnalyzerConfiguration;
using AnalyzerConfiguration.UnitsConfiguration;
using AnalyzerService.ExecutionControl;
using AnalyzerDomain.Entyties;
using Infrastructure;
using System.Collections.Generic;
using AnalyzerDomain.Models;

namespace AnalyzerService.Units
{
    public class RotorUnit : UnitBase<RotorConfiguration>
    {
        public int Position { get; set; } = 0;

        public RotorUnit(ICommandExecutor executor, IConfigurationProvider provider) : base(executor, provider)
        {

        }

        public void Home()
        {
            Logger.Debug($"[{nameof(RotorUnit)}] - Start homing.");
            List<ICommand> commands = new List<ICommand>();

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, Options.RotorHomeSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, -Options.RotorHomeSpeed } };
            commands.Add(new HomeCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = 0;

            Logger.Debug($"[{nameof(RotorUnit)}] - Homing finished.");
        }

        public void PlaceCellUnderWashBuffer(int cartridgePosition)
        {
            Logger.Debug($"[{nameof(RotorUnit)}] - Start placing cell under washing buffer.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Options.StepsToWashBuffer;
            turnSteps += Options.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, Options.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.Debug($"[{nameof(RotorUnit)}] - Placing cell under washing buffer finished.");
        }

        public void PlaceCellAtDischarge(int cartridgePosition)
        {
            Logger.Debug($"[{nameof(RotorUnit)}] - Start placing cell at discharger.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Options.StepsToUnload;
            turnSteps += Options.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, Options.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.Debug($"[{nameof(RotorUnit)}] - Placing cell at discharger finished.");
        }
        
        /// <summary>
        /// Разместить ячеку ротора в позицию для загрузки картриджа из кассеты
        /// </summary>
        /// <param name="cartridgePosition">Позиция ячейки ротора</param>
        /// <param name="chargePosition">Позиция ячейки кассетницы (загрузки)</param>
        public void PlaceCellAtCharge(int cartridgePosition, int chargePosition)
        {
            Logger.Debug($"[{nameof(RotorUnit)}] - Start placing cell at charger.");
            List<ICommand> commands = new List<ICommand>();

            int turnSteps = Options.StepsToLoad[chargePosition];
            turnSteps += Options.StepsPerCell * cartridgePosition;

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, Options.RotorSpeed } };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() { { Options.RotorStepper, turnSteps - Position} };
            commands.Add(new MoveCncCommand(steppers));

            executor.WaitExecution(commands);
            Position = turnSteps;

            Logger.Debug($"[{nameof(RotorUnit)}] - Placing cell at charger finished.");
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
        public void PlaceCellUnderNeedle(int cartridgePosition, CartridgeWell cartridgeCell, CellPosition cellPosition = CellPosition.CellCenter)
        {
            Logger.Debug($"[{nameof(RotorUnit)}] - Start placing cell under needle.");
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new SetSpeedCommand(Options.RotorStepper, (uint)Options.RotorSpeed));

            int turnSteps = 0;

            if (cartridgeCell == CartridgeWell.CUV)
            {
                turnSteps = Options.StepsToNeedleResultCenter;
            }
            else if (cartridgeCell == CartridgeWell.ACW)
            {
                turnSteps = Options.StepsToNeedleWhiteCenter;
            }
            else if (cartridgeCell == CartridgeWell.W1)
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
            else if (cartridgeCell == CartridgeWell.W2)
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
            else if (cartridgeCell == CartridgeWell.W3)
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

            Logger.Debug($"[{nameof(RotorUnit)}] - Placing cell under needle finished.");
        }
    }
}
