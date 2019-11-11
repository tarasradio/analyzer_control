using System.Collections.Generic;
using System.IO;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.Utils;
using SteppersControlCore.ControllersProperties;

namespace SteppersControlCore.Controllers
{
    public class PompController : ControllerBase
    {
        const string filename = "PompControllerProps";

        public PompControllerProperties Properties { get; set; }

        //TODO: а нужно ли здесь отслеживание позиции???

        public int BigPistonStepperPosition { get; set; } = 0;
        public int SmallPistonStepperPosition { get; set; } = 0;

        public PompController() : base()
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

        public List<IAbstractCommand> CloseValves()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            commands.Add(new OnDeviceCncCommand(new List<int>() { 0 ,1 }));

            return commands;
        }

        public List<IAbstractCommand> Home()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
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

            return commands;
        }

        public List<IAbstractCommand> Suction(int value)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

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

            return commands;
        }

        public List<IAbstractCommand> Unsuction(int value)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
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

            return commands;
        }

        public List<IAbstractCommand> Washing(int cycles)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

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

            return commands;
        }
    }
}
