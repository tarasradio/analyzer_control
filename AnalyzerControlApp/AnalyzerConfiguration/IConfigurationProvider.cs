namespace AnalyzerConfiguration
{
    public interface IConfigurationProvider
    {
        void SaveConfiguration<T>(T configuration, string filename);
        T LoadConfiguration<T>(string filename);
    }
}
