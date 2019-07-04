using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;

namespace SteppersControlCore
{
    public class Configuration
    {
        const string ROOTNAME = "SteppersTool";

        public Dictionary<int, Stepper> Steppers { get; set; }

        public Configuration()
        {
            Steppers = new Dictionary<int, Stepper>();
        }

        public bool LoadFromFile(string filename)
        {
            Steppers.Clear();
            XmlDocument document = new XmlDocument();

            try
            {
                document.Load(filename);

                XmlNodeList SteppersNode =
                    document.SelectNodes("/" + ROOTNAME + "/Steppers/Stepper");

                foreach (XmlNode StepperNode in SteppersNode)
                {
                    Stepper item = new Stepper();

                    item.Number = int.Parse(StepperNode.SelectSingleNode("Number").InnerText);

                    if (item.Number < 0)
                        throw new FormatException("Номер двигателя не может быть меньше 0.");

                    item.Name = StepperNode.SelectSingleNode("Name").InnerText;
                    item.Speed = int.Parse(StepperNode.SelectSingleNode("Speed").InnerText);

                    Steppers.Add(item.Number, item);
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

            XmlElement SteppersNode = document.CreateElement("Steppers");

            foreach (KeyValuePair<int, Stepper> stepper in Steppers)
            {
                XmlElement StepperNode = document.CreateElement("Stepper");

                XmlElement Number = document.CreateElement("Number");
                Number.InnerText = stepper.Value.Number.ToString();
                StepperNode.AppendChild(Number);

                XmlElement Name = document.CreateElement("Name");
                Name.InnerText = stepper.Value.Name;
                StepperNode.AppendChild(Name);

                XmlElement Speed = document.CreateElement("Speed");
                Speed.InnerText = stepper.Value.Speed.ToString();
                StepperNode.AppendChild(Speed);

                SteppersNode.AppendChild(StepperNode);
            }

            root.AppendChild(SteppersNode);

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
