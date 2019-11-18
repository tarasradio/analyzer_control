using System.Collections.Generic;
using System.IO;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.Utils;
using SteppersControlCore.ControllersProperties;
using SteppersControlCore.Interfaces;

namespace SteppersControlCore.Controllers
{
    public class PompController : ControllerBase
    {
        const string filename = "PompControllerProps";

        public PompControllerProperties Properties { get; set; }

        //TODO: а нужно ли здесь отслеживание позиции???

        public int BigPistonStepperPosition { get; set; } = 0;
        public int SmallPistonStepperPosition { get; set; } = 0;

        public PompController(ICommandExecutor executor) : base(executor)
        {
            Properties = new PompControllerProperties();
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<PompControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename));
        }

        //Чтение насроек из файла
        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<PompControllerProperties>.ReadXML(
                Path.Combine(path, filename));

            if (Properties == null)
                Properties = new PompControllerProperties();
        }

        public void CloseValves()
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add(new OffDeviceCncCommand(new List<int>() { 0 , 1 }));

            executor.WaitExecution(commands);
        }

        public void Home()
        {
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 }));
            
            commands.Add(new OffDeviceCncCommand(new List<int>() { 1 }));

            steppers = new Dictionary<int, int>() {
                { Properties.BigPistonStepper, Properties.BigPistonHomeSpeed },
                { Properties.SmallPistonStepper, Properties.SmallPistonHomeSpeed }
            };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Properties.BigPistonStepper, Properties.BigPistonHomeSpeed },
                { Properties.SmallPistonStepper, Properties.SmallPistonHomeSpeed }
            };
            commands.Add(new HomeCncCommand(steppers));

            BigPistonStepperPosition = 0;
            SmallPistonStepperPosition = 0;

            executor.WaitExecution(commands);
        }

        public void Suction(int value)
        {
            List<ICommand> commands = new List<ICommand>();

            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );
            
            steppers = new Dictionary<int, int>() {
                { Properties.SmallPistonStepper, Properties.SmallPistonSuctionSpeed }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Properties.SmallPistonStepper, Properties.SmallPistonSuctionSteps - SmallPistonStepperPosition}
            };
            commands.Add( new MoveCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            SmallPistonStepperPosition = Properties.SmallPistonSuctionSteps;

            executor.WaitExecution(commands);
        }

        public void Unsuction(int value)
        {
            List<ICommand> commands = new List<ICommand>();
            
            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );

            steppers = new Dictionary<int, int>() {
                { Properties.SmallPistonStepper, Properties.SmallPistonSuctionSpeed }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Properties.SmallPistonStepper, Properties.SmallPistonSuctionSpeed }
            };
            commands.Add( new HomeCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            SmallPistonStepperPosition = 0;

            executor.WaitExecution(commands);
        }

        public void Washing(int cycles)
        {
            List<ICommand> commands = new List<ICommand>();

            for (int i = 0; i < cycles; i++)
            {
                commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
                commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );

                steppers = new Dictionary<int, int>() {
                    { Properties.BigPistonStepper, Properties.BigPistonHomeSpeed },
                    { Properties.SmallPistonStepper, Properties.SmallPistonHomeSpeed }
                };
                commands.Add( new SetSpeedCncCommand(steppers) );

                steppers = new Dictionary<int, int>() {
                    { Properties.BigPistonStepper, Properties.BigPistonHomeSpeed },
                    { Properties.SmallPistonStepper, Properties.SmallPistonHomeSpeed }
                };
                commands.Add( new HomeCncCommand(steppers) );
                
                commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );
                commands.Add( new OnDeviceCncCommand(new List<int>() { 1 }) );

                steppers = new Dictionary<int, int>() {
                    { Properties.BigPistonStepper, Properties.BigPistonWashingSpeed },
                    { Properties.SmallPistonStepper, Properties.SmallPistonWashingSpeed }
                };
                commands.Add( new SetSpeedCncCommand(steppers) );

                steppers = new Dictionary<int, int>() {
                    { Properties.BigPistonStepper, Properties.BigPistonWashingSteps },
                    { Properties.SmallPistonStepper, Properties.SmallPistonWashingSteps }
                };
                commands.Add( new MoveCncCommand(steppers) );
            }

            SmallPistonStepperPosition = Properties.BigPistonWashingSteps;
            BigPistonStepperPosition = Properties.SmallPistonWashingSteps;

            executor.WaitExecution(commands);
        }
    }
}
