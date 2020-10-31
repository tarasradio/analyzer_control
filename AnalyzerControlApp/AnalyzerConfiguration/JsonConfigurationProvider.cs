using Newtonsoft.Json;
using System.IO;

namespace AnalyzerConfiguration
{
    public class JsonConfigurationProvider : IConfigurationProvider
    {
        public T LoadConfiguration<T>(string filename)
        {
            var fullPath = ConfigurationHelper.GetConfigurationPath($"{filename}.json");
            string jsonValue = File.ReadAllText(fullPath);

            return JsonConvert.DeserializeObject<T>(jsonValue);
        }

        public void SaveConfiguration<T>(T configuration, string filename)
        {
            ConfigurationHelper.SaveBackup($"{filename}.json");

            var result = JsonConvert.SerializeObject(configuration, Formatting.Indented);
            var fullPath = ConfigurationHelper.GetConfigurationPath($"{filename}.json");

            File.WriteAllText(fullPath, result);
        }
    }
}
