using System;
using System.IO;
using System.Xml.Serialization;

namespace AnalyzerConfiguration
{
    public class XmlSerializeHelper<T>
    {
        const string backupDir = "ConfigurationBackup";

        public static void WriteXml(T data, string filename)
        {
            XmlSerializer ser = new XmlSerializer(data.GetType());

            string dir = $"{backupDir}/{DateTime.Now.ToString("dd_MM_yyyy_#_HH_mm_ss")}/";

            Directory.CreateDirectory(backupDir);
            Directory.CreateDirectory(dir);
            
            if(File.Exists($"{filename}.xml"))
            {
                string fileNameForBackup = $"{dir}{Path.GetFileName(filename)}.xml";
                File.Copy($"{filename}.xml", fileNameForBackup);
                File.Delete($"{filename}.xml");
            }

            TextWriter writer = new StreamWriter($"{filename}.xml");
            ser.Serialize(writer, data);
            writer.Close();
        }

        public static T ReadXml(string filename)
        {
            T data;

            if (File.Exists($"{filename}.xml"))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader($"{filename}.xml");

                data = (T)ser.Deserialize(reader);
                reader.Close();
            }
            else
            {
                throw new FileNotFoundException($"Файл {filename}.xml не найден.", filename);
            }

            return data;
        }
    }
}
