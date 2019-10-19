using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SteppersControlCore.Utils
{
    public class XMLSerializeHelper<T>
    {
        const string backupDir = "backup_settings";

        public static void WriteXml(T data, string filename)
        {
            XmlSerializer ser = new XmlSerializer(data.GetType());

            string dir = $"{backupDir}/{DateTime.Now.ToString("dd_MM_yyyy_#_HH_mm_ss")}/";

            Directory.CreateDirectory("backup_settings");
            Directory.CreateDirectory(dir);

            string fileNameForBackup =
                dir + $"{filename}.xml";

            File.Copy(filename + ".xml", fileNameForBackup);
            File.Delete(filename + ".xml");

            TextWriter writer = new StreamWriter(filename + ".xml");
            ser.Serialize(writer, data);
            writer.Close();
        }

        public static T ReadXML(string filename)
        {
            T data = default(T);
            if (File.Exists(filename + ".xml"))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(filename + ".xml");
                data = (T)ser.Deserialize(reader);
                reader.Close();
            }
            else
            {
                //можно написать вывод сообщения если файла не существует
            }

            return data;
        }
    }
}
