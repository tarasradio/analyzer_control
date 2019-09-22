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

        [Category("2. Скорость")]
        [DisplayName("Скорость движения большого плунжера домой")]
        public int BigPistonHomeSpeed { get; set; } = 950;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения большого плунжера")]
        public int BigPistonSpeed { get; set; } = 50;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения большого плунжера при промывке")]
        public int BigPistonWashingSpeed { get; set; } = 280;

        [Category("2. Скорость")]
        [DisplayName("Скорость движения малого плунжера домой")]
        public int SmallPistonHomeSpeed { get; set; } = 950;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения малого плунжера")]
        public int SmallPistonSpeed { get; set; } = 200;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения малого плунжера при промывке")]
        public int SmallPistonWashingSpeed { get; set; } = 280;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения малого плунжера при заборе")]
        public int SmallPistonSuctionSpeed { get; set; } = 200;

        [Category("3. Шаги")]
        [DisplayName("Шаги малого плунжера при заборе")]
        public int SmallPistonSuctionSteps { get; set; } = 100000;

        [Category("3. Шаги")]
        [DisplayName("Шаги большого плунжера при промывке")]
        public int BigPistonWashingSteps { get; set; } = 700000;
        [Category("3. Шаги")]
        [DisplayName("Шаги малого плунжера при промывке")]
        public int SmallPistonWashingSteps { get; set; } = 700000;
        

        public PompControllerPropetries()
        {

        }
    }

    public class PompController : Controller
    {
        const string filename = "PompControllerProps.xml";

        public PompControllerPropetries Props { get; set; }

        public PompController() : base()
        {
            Props = new PompControllerPropetries();
        }

        public void WriteXml()
        {
            XmlSerializer ser = new XmlSerializer(typeof(PompControllerPropetries));

            TextWriter writer = new StreamWriter(filename);
            ser.Serialize(writer, Props);
            writer.Close();
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            if (File.Exists(filename))
            {
                XmlSerializer ser = new XmlSerializer(typeof(PompControllerPropetries));
                TextReader reader = new StreamReader(filename);
                Props = ser.Deserialize(reader) as PompControllerPropetries;
                reader.Close();
            }
            else
            {
                //можно написать вывод сообщения если файла не существует
            }
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
                { Props.SmallPistonStepper, -Props.SmallPistonSuctionSteps }
            };
            commands.Add( new MoveCncCommand(steppers) );
            
            commands.Add( new OffDeviceCncCommand(new List<int>() { 0 }) );

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
                    { Props.BigPistonStepper, -Props.BigPistonWashingSteps },
                    { Props.SmallPistonStepper, -Props.SmallPistonWashingSteps }
                };
                commands.Add( new MoveCncCommand(steppers) );
            }

            return commands;
        }
    }
}
