using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

using System.Globalization;
using System.Xml.Serialization;
using System.IO;

namespace SteppersControlCore
{
    public class Device
    {
        [XmlAttribute]
        public int Number { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
    }

    public class StepperParams
    {
        [XmlAttribute]
        public bool Reverse { get; set; } = false;
        [XmlAttribute]
        public uint NumberSteps { get; set; } = 100000;
        [XmlAttribute]
        public uint FullSpeed { get; set; } = 1000;
    }

    public class Configuration
    {
        const string ROOTNAME = "SteppersTool";

        public Dictionary<int, Device> Steppers { get; set; }
        public Dictionary<int, StepperParams> SteppersParams { get; set; }

        public Dictionary<int, Device> Devices { get; set; }
        public Dictionary<int, Device> Sensors { get; set; }

        public CultureInfo Culture { get; set; }

        public Configuration()
        {
            Steppers = new Dictionary<int, Device>();
            SteppersParams = new Dictionary<int, StepperParams>();

            Devices = new Dictionary<int, Device>();
            Sensors = new Dictionary<int, Device>();

            Culture = new CultureInfo("en-US");
        }

        public bool LoadFromFile(string filename)
        {
            Steppers.Clear();
            SteppersParams.Clear();

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
                    Device item = new Device();
                    item.Number = int.Parse(StepperNode.SelectSingleNode("Number").InnerText);

                    if (item.Number < 0)
                        throw new FormatException("Номер двигателя не может быть меньше 0.");

                    item.Name = StepperNode.SelectSingleNode("Name").InnerText;
                    Steppers.Add(item.Number, item);

                    StepperParams stepperParams = new StepperParams();

                    stepperParams.Reverse = bool.Parse(StepperNode.SelectSingleNode("Reverse").InnerText);
                    stepperParams.FullSpeed = uint.Parse(StepperNode.SelectSingleNode("FullSpeed").InnerText);
                    stepperParams.NumberSteps = uint.Parse(StepperNode.SelectSingleNode("NumberSteps").InnerText);

                    SteppersParams.Add(item.Number, stepperParams);
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

                    Devices.Add(item.Number, item);
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

                    Sensors.Add(item.Number, item);
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

            foreach (KeyValuePair<int, Device> stepper in Steppers)
            {
                XmlElement stepperNode = document.CreateElement("Stepper");

                XmlElement number = document.CreateElement("Number");
                number.InnerText = stepper.Value.Number.ToString();
                stepperNode.AppendChild(number);

                XmlElement name = document.CreateElement("Name");
                name.InnerText = stepper.Value.Name;
                stepperNode.AppendChild(name);

                XmlElement reverse = document.CreateElement("Reverse");
                reverse.InnerText = SteppersParams[stepper.Value.Number].Reverse.ToString();
                stepperNode.AppendChild(reverse);

                XmlElement fullSpeed = document.CreateElement("FullSpeed");
                fullSpeed.InnerText = SteppersParams[stepper.Value.Number].FullSpeed.ToString();
                stepperNode.AppendChild(fullSpeed);

                XmlElement numberSteps = document.CreateElement("NumberSteps");
                numberSteps.InnerText = SteppersParams[stepper.Value.Number].NumberSteps.ToString();
                stepperNode.AppendChild(numberSteps);

                steppersNode.AppendChild(stepperNode);
            }

            root.AppendChild(steppersNode);

            XmlElement devicesNode = document.CreateElement("Devices");

            foreach (KeyValuePair<int, Device> device in Devices)
            {
                XmlElement deviceNode = document.CreateElement("Device");

                XmlElement number = document.CreateElement("Number");
                number.InnerText = device.Value.Number.ToString();
                deviceNode.AppendChild(number);

                XmlElement name = document.CreateElement("Name");
                name.InnerText = device.Value.Name;
                deviceNode.AppendChild(name);

                devicesNode.AppendChild(deviceNode);
            }

            root.AppendChild(devicesNode);

            XmlElement sensorsNode = document.CreateElement("Sensors");

            foreach (KeyValuePair<int, Device> sensor in Sensors)
            {
                XmlElement sensorNode = document.CreateElement("Sensor");

                XmlElement number = document.CreateElement("Number");
                number.InnerText = sensor.Value.Number.ToString();
                sensorNode.AppendChild(number);

                XmlElement name = document.CreateElement("Name");
                name.InnerText = sensor.Value.Name;
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
