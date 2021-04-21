using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzerConfiguration;
using AnalyzerControl;
using AnalyzerService;

namespace AnalyzerConsole
{
    class Program
    {
        static IConfigurationProvider provider = new XmlConfigurationProvider();
        static Analyzer analyzer = null;
        static AnalyzerDemoController demoController = null;

        const string controllerFileName = "DemoControllerConfiguration";

        static void Main(string[] args)
        {
            bool appFinished = false;

            tryRunAnalyzer();

            while(!appFinished)
            {
                Console.WriteLine("Для остановки введите q");
                string line = Console.ReadLine();

                if(line.Equals("q", comparisonType: StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Остановка анализатора...");
                    tryStopAnalyzer();
                    appFinished = true;
                } else {
                    Console.WriteLine("Вы ввели неверную команду, попробуйте снова.");
                }
            }
        }

        static void tryRunAnalyzer()
        {
            try {
                analyzer = new Analyzer(provider);
                demoController = new AnalyzerDemoController(provider); 
                demoController.LoadConfiguration(controllerFileName);
                Console.WriteLine("Анализатор запущен");
            }
            catch {
                Console.WriteLine($"Возникла ошибка при открытии файла конфигурации!");
                return;
            }
        }

        static void tryStopAnalyzer()
        {
            try {
                analyzer.SaveUnitsConfiguration();
                demoController.SaveConfiguration(controllerFileName);
            } catch {
                Console.WriteLine($"Возникла ошибка при сохранении файла конфигурации!");
                return;
            }
        }
    }
}
