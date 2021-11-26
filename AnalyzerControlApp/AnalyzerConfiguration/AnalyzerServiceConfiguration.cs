using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace AnalyzerConfiguration
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
        [XmlElement]
        public bool Reverse { get; set; } = false;
        [XmlElement]
        public uint NumberSteps { get; set; } = 100000;
        [XmlElement]
        public uint FullSpeed { get; set; } = 1000;
        [XmlIgnore]
        public ushort Status { get; set; } = 0;
        [XmlIgnore]
        public int Position { get; set; } = 0;
    }

    public class AnalyzerServiceConfiguration
    {
        public string PortName { get; set; }
        public int Baudrate { get; set; }

        public List<Stepper> Steppers { get; set; }
        public List<Device> Devices { get; set; }
        public List<Device> Sensors { get; set; }

        public AnalyzerServiceConfiguration()
        {
            PortName = "COM1";
            Baudrate = 115200;

            Steppers = new List<Stepper>();
            Devices = new List<Device>();
            Sensors = new List<Device>();
        }
    }
}
