using System;
using System.IO;

namespace AnalyzerConfiguration
{
    public abstract class Configurable<T> where T : new()
    {
        protected IConfigurationProvider provider;
        public T Options { get; private set; }

        public Configurable(IConfigurationProvider provider)
        {
            this.provider = provider;
            Options = new T();
        }

        public void LoadConfiguration(string filename)
        {
            try
            {
                Options = provider.LoadConfiguration<T>(filename);
            }
            catch (Exception exeption)
            {
                //TODO: дописать обработку извне
                throw new IOException("Ошибка при загрузке файла конфигурации. Используется конфигурация по умолчанию.", innerException: exeption);
                //Logger.Info($"Ошибка при загрузке файла конфигурации. Используется конфигурация по умолчанию.");
            }
        }

        public void SaveConfiguration(string filename)
        {
            try
            {
                provider.SaveConfiguration(Options, filename);
            }
            catch (Exception exeption)
            {
                throw new IOException("Ошибка при сохранении файла конфигурации.", innerException: exeption);
            }
        }
    }
}
