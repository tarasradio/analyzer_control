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
using SteppersControlCore.Utils;

namespace SteppersControlCore.Controllers
{
    public class PompControllerPropetries
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель большого плунжера")]
        public int BigPistonStepper { get; set; } = 11;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель малого плунжера")]
        public int SmallPistonStepper { get; set; } = 12;

        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера домой")]
        public int BigPistonHomeSpeed { get; set; } = 950;
        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера")]
        public int BigPistonSpeed { get; set; } = 50;
        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера при промывке")]
        public int BigPistonWashingSpeed { get; set; } = 280;

        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера домой")]
        public int SmallPistonHomeSpeed { get; set; } = 950;
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера")]
        public int SmallPistonSpeed { get; set; } = 200;
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера при промывке")]
        public int SmallPistonWashingSpeed { get; set; } = 280;
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера при заборе")]
        public int SmallPistonSuctionSpeed { get; set; } = 200;

        [Category("3.1 Шаги малого плунжера")]
        [DisplayName("Шаги малого плунжера при заборе")]
        public int SmallPistonSuctionSteps { get; set; } = -200000;

        [Category("3.2 Шаги большого плунжера")]
        [DisplayName("Шаги большого плунжера при промывке")]
        public int BigPistonWashingSteps { get; set; } = -700000;
        [Category("3.1 Шаги малого плунжера")]
        [DisplayName("Шаги малого плунжера при промывке")]
        public int SmallPistonWashingSteps { get; set; } = -700000;
        

        public PompControllerPropetries()
        {

        }
    }

    public class PompController : ControllerBase
    {
        const string filename = "PompControllerProps";

        public PompControllerPropetries Props { get; set; }

        //TODO: а нужно ли здесь отслеживание позиции???

        public int BigPistonStepperPosition { get; set; } = 0;
        public int SmallPistonStepperPosition { get; set; } = 0;

        public PompController() : base()
        {
            Props = new PompControllerPropetries();
        }

        public void WriteXml()
        {
            XMLSerializeHelper<PompControllerPropetries>.WriteXml(Props, filename);
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            Props = XMLSerializeHelper<PompControllerPropetries>.ReadXML(filename);
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
                { Props.BigPistonStepper, Props.BigPistonHomeSpeed },
                { Props.SmallPistonStepper, Props.SmallPistonHomeSpeed }
            };
            commands.Add(new SetSpeedCncCommand(steppers));

            steppers = new Dictionary<int, int>() {
                { Props.BigPistonStepper, Props.BigPistonHomeSpeed },
                { Props.SmallPistonStepper, Props.SmallPistonHomeSpeed }
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
                { Props.SmallPistonStepper, Props.SmallPistonSuctionSpeed }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Props.SmallPistonStepper, Props.SmallPistonSuctionSteps - SmallPistonStepperPosition}
            };
            commands.Add( new MoveCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

            SmallPistonStepperPosition = Props.SmallPistonSuctionSteps;

            return commands;
        }

        public List<IAbstractCommand> Unsuction(int value)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();
            
            commands.Add( new OnDeviceCncCommand(new List<int>() { 0 }) );
            commands.Add( new OffDeviceCncCommand(new List<int>() { 1 }) );

            steppers = new Dictionary<int, int>() {
                { Props.SmallPistonStepper, Props.SmallPistonSuctionSpeed }
            };
            commands.Add( new SetSpeedCncCommand(steppers) );

            steppers = new Dictionary<int, int>() {
                { Props.SmallPistonStepper, Props.SmallPistonSuctionSpeed }
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
                    { Props.BigPistonStepper, Props.BigPistonHomeSpeed },
                    { Props.SmallPistonStepper, Props.SmallPistonHomeSpeed }
                };
                commands.Add( new SetSpeedCncCommand(steppers) );

                steppers = new Dictionary<int, int>() {
                    { Props.BigPistonStepper, Props.BigPistonHomeSpeed },
                    { Props.SmallPistonStepper, Props.SmallPistonHomeSpeed }
                };
                commands.Add( new HomeCncCommand(steppers) );
                
                commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );
                commands.Add( new OnDeviceCncCommand(new List<int>() { 1 }) );

                steppers = new Dictionary<int, int>() {
                    { Props.BigPistonStepper, Props.BigPistonWashingSpeed },
                    { Props.SmallPistonStepper, Props.SmallPistonWashingSpeed }
                };
                commands.Add( new SetSpeedCncCommand(steppers) );

                steppers = new Dictionary<int, int>() {
                    { Props.BigPistonStepper, Props.BigPistonWashingSteps },
                    { Props.SmallPistonStepper, Props.SmallPistonWashingSteps }
                };
                commands.Add( new MoveCncCommand(steppers) );
            }

            SmallPistonStepperPosition = Props.BigPistonWashingSteps;
            BigPistonStepperPosition = Props.SmallPistonWashingSteps;

            return commands;
        }
    }
}
