using System.IO;
using System.Xml.Serialization;

namespace AnalyzerConfiguration
{
    public class XmlConfigurationProvider : IConfigurationProvider
    {
        public T LoadConfiguration<T>(string filename)
        {
            T result;

            var fullPath = ConfigurationHelper.GetConfigurationPath($"{filename}.xml");

            XmlSerializer ser = new XmlSerializer(typeof(T), "");
            TextReader reader = new StreamReader(fullPath);

            result = (T)ser.Deserialize(reader);
            reader.Close();

            return result;
        }

        public void SaveConfiguration<T>(T configuration, string filename)
        {
            ConfigurationHelper.SaveBackup($"{filename}.xml");

            var fullPath = ConfigurationHelper.GetConfigurationPath($"{filename}.xml");

            XmlSerializer serializer = new XmlSerializer(configuration.GetType(), "");
            TextWriter writer = new StreamWriter(fullPath);

            serializer.Serialize(writer, configuration);
            writer.Close();
        }
    }
}
