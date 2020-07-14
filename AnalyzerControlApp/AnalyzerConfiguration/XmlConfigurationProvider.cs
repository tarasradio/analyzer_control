using System;
using System.IO;
using System.Xml.Serialization;

namespace AnalyzerConfiguration
{
    public class XmlConfigurationProvider<T> : IConfigurationProvider<T>
    {
        const string backupDir = "ConfigurationBackup";

        public void SaveConfiguration(T configuration, string filename)
        {
            XmlSerializer ser = new XmlSerializer(configuration.GetType());

            string dir = $"{backupDir}/{DateTime.Now:dd_MM_yyyy_#_HH_mm_ss}/";

            Directory.CreateDirectory(backupDir);
            Directory.CreateDirectory(dir);
            
            if(File.Exists($"{filename}.xml"))
            {
                string fileNameForBackup = $"{dir}{Path.GetFileName(filename)}.xml";
                File.Copy($"{filename}.xml", fileNameForBackup);
                File.Delete($"{filename}.xml");
            }

            TextWriter writer = new StreamWriter($"{filename}.xml");
            ser.Serialize(writer, configuration);
            writer.Close();
        }

        public T LoadConfiguration(string filename)
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
