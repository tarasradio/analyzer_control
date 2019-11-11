using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using System.Globalization;
using System.Xml.Serialization;
using System.IO;
using SteppersControlCore.Utils;

namespace SteppersControlCore
{
    public class Device
    {
        [XmlAttribute]
        public int Number { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
    }

    public class Stepper : Device
    {
        [XmlAttribute]
        public bool Reverse { get; set; } = false;
        [XmlAttribute]
        public uint NumberSteps { get; set; } = 100000;
        [XmlAttribute]
        public uint FullSpeed { get; set; } = 1000;
        [XmlIgnore]
        public ushort Status { get; set; } = 0;
    }

    public class Configuration
    {
        const string ROOTNAME = "SteppersTool";
        
        public IList<Stepper> Steppers { get; set; } = new List<Stepper>();

        public IList<Device> Devices { get; set; } = new List<Device>();
        public IList<Device> Sensors { get; set; } = new List<Device>();

        public CultureInfo Culture { get; set; } = new CultureInfo("en-US");

        public Configuration()
        {

        }
        
        public bool LoadFromFile(string filename)
        {
            Steppers.Clear();

            Devices.Clear();
            Sensors.Clear();

            Culture = new CultureInfo("en-US");

            XmlDocument document = new XmlDocument();

            try
            {
                document.Load(filename);

                XmlNode LanguageNode =
                    document.SelectSingleNode($"/{ROOTNAME}/Language");

                if(LanguageNode != null)
                {
                    Culture = new CultureInfo(LanguageNode.InnerText);
                }

                XmlNodeList SteppersNode =
                    document.SelectNodes($"/{ROOTNAME}/Steppers/Stepper");

                foreach (XmlNode StepperNode in SteppersNode)
                {
                    Stepper item = new Stepper();
                    item.Number = int.Parse(StepperNode.SelectSingleNode("Number").InnerText);

                    if (item.Number < 0)
                        throw new FormatException("Номер двигателя не может быть меньше 0.");

                    item.Name = StepperNode.SelectSingleNode("Name").InnerText;

                    item.Reverse = bool.Parse(StepperNode.SelectSingleNode("Reverse").InnerText);
                    item.FullSpeed = uint.Parse(StepperNode.SelectSingleNode("FullSpeed").InnerText);
                    item.NumberSteps = uint.Parse(StepperNode.SelectSingleNode("NumberSteps").InnerText);

                    Steppers.Add(item);
                }

                XmlNodeList DevicesNode =
                    document.SelectNodes($"/{ROOTNAME}/Devices/Device");

                foreach (XmlNode DeviceNode in DevicesNode)
                {
                    Device item = new Device();

                    item.Number = int.Parse(DeviceNode.SelectSingleNode("Number").InnerText);

                    if (item.Number < 0)
                        throw new FormatException("Номер устройства не может быть меньше 0.");

                    item.Name = DeviceNode.SelectSingleNode("Name").InnerText;

                    Devices.Add(item);
                }

                XmlNodeList sensorsNode =
                    document.SelectNodes($"/{ROOTNAME}/Sensors/Sensor");

                foreach (XmlNode sensorNode in sensorsNode)
                {
                    Device item = new Device();

                    item.Number = int.Parse(sensorNode.SelectSingleNode("Number").InnerText);

                    if (item.Number < 0)
                        throw new FormatException("Номер датчика не может быть меньше 0.");

                    item.Name = sensorNode.SelectSingleNode("Name").InnerText;

                    Sensors.Add(item);
                }
            }
            catch { return false; }

            return true;
        }

        public bool SaveToFile(string Filename)
        {
            XmlDocument document = new XmlDocument();

            XmlDeclaration xmlDeclaration =
                document.CreateXmlDeclaration("1.0", "UTF-8", "yes");

            XmlElement root = document.CreateElement(ROOTNAME);
            document.AppendChild(root);
            document.InsertBefore(xmlDeclaration, root);

            XmlElement languageNode = document.CreateElement("Language");
            languageNode.InnerText = Culture.Name;
            root.AppendChild(languageNode);

            XmlElement steppersNode = document.CreateElement("Steppers");

            foreach (Stepper stepper in Steppers)
            {
                XmlElement stepperNode = document.CreateElement("Stepper");

                XmlElement number = document.CreateElement("Number");
                number.InnerText = stepper.Number.ToString();
                stepperNode.AppendChild(number);

                XmlElement name = document.CreateElement("Name");
                name.InnerText = stepper.Name;
                stepperNode.AppendChild(name);

                XmlElement reverse = document.CreateElement("Reverse");
                reverse.InnerText = stepper.Reverse.ToString();
                stepperNode.AppendChild(reverse);

                XmlElement fullSpeed = document.CreateElement("FullSpeed");
                fullSpeed.InnerText = stepper.FullSpeed.ToString();
                stepperNode.AppendChild(fullSpeed);

                XmlElement numberSteps = document.CreateElement("NumberSteps");
                numberSteps.InnerText = stepper.NumberSteps.ToString();
                stepperNode.AppendChild(numberSteps);

                steppersNode.AppendChild(stepperNode);
            }

            root.AppendChild(steppersNode);

            XmlElement devicesNode = document.CreateElement("Devices");

            foreach (Device device in Devices)
            {
                XmlElement deviceNode = document.CreateElement("Device");

                XmlElement number = document.CreateElement("Number");
                number.InnerText = device.Number.ToString();
                deviceNode.AppendChild(number);

                XmlElement name = document.CreateElement("Name");
                name.InnerText = device.Name;
                deviceNode.AppendChild(name);

                devicesNode.AppendChild(deviceNode);
            }

            root.AppendChild(devicesNode);

            XmlElement sensorsNode = document.CreateElement("Sensors");

            foreach (Device sensor in Sensors)
            {
                XmlElement sensorNode = document.CreateElement("Sensor");

                XmlElement number = document.CreateElement("Number");
                number.InnerText = sensor.Number.ToString();
                sensorNode.AppendChild(number);

                XmlElement name = document.CreateElement("Name");
                name.InnerText = sensor.Name;
                sensorNode.AppendChild(name);

                sensorsNode.AppendChild(sensorNode);
            }

            root.AppendChild(sensorsNode);

            try
            {
                document.Save(Filename);
            }
            catch(XmlException)
            {
                return false;
            }

            return true;
        }
    }
}
