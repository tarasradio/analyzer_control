using AnalyzerConfiguration;
using AnalyzerService.MachineControl;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;

namespace AnalyzerService.Units
{
    // Calculation of necessary steps using the target and current position (in steps)
    // The target position is determined by the absolute number of steps from the starting position (0)
    // General rule: needed steps = taget_position - current_position
    // Example 1: position = 3000; target position = 2000; necessary steps = 2000 - 3000 = -1000
    // Example 2: position = 2000; target position = 3000; necessary steps = 3000 - 2000 = 1000
    // Example 3: position = -3000; target position = -2000; necessary steps = -2000 - (-3000) = 1000
    // Example 4: position = -2000; target position = -3000; necessary steps = -3000 - (-2000) = -1000

    public abstract class UnitBase<T> where T : new()
    {
        protected Dictionary<int, int> steppers;
        protected ICommandExecutor executor;

        protected IConfigurationProvider provider;
        public T Options;

        public UnitBase(ICommandExecutor executor, IConfigurationProvider provider)
        {
            this.executor = executor;
            this.provider = provider;

            Options = new T();

            steppers = new Dictionary<int, int>();
        }

        public void LoadConfiguration(string filename)
        {
            try
            {
                Options = provider.LoadConfiguration<T>(filename);
            }
            catch
            {
                Logger.Info($"Ошибка при загрузке файла конфигурации. Используется конфигурация по умолчанию.");
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

        public T GetConfiguration()
        {
            return Options;
        }
    }
}
