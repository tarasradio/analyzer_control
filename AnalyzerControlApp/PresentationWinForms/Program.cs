using PresentationWinForms.Forms;
using System;
using System.Windows.Forms;
using AnalyzerControl;
using AnalyzerConfiguration;
using AnalyzerControl.Services;
using AnalyzerService;

namespace PresentationWinForms
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool useAuthentication = false;

            try
            {
                IConfigurationProvider provider = new XmlConfigurationProvider();

                Analyzer analyzer = new Analyzer(provider);
                ConveyorService conveyor = new ConveyorService(54);
                AnalyzerDemoController demoController = new AnalyzerDemoController(provider, conveyor);

                demoController.LoadConfiguration("DemoControllerConfiguration");

                StartWindow startWindow = new StartWindow();
                MainWindow mainWindow = new MainWindow();

                mainWindow.Init(analyzer, conveyor, demoController);

                startWindow.StartPosition = FormStartPosition.CenterScreen;
                mainWindow.StartPosition = FormStartPosition.CenterScreen;

                if(useAuthentication)
                {
                    Application.Run(startWindow);

                    if (startWindow.IsAuthenticated)
                    {
                        Application.Run(mainWindow);
                    }
                }
                else
                {
                    Application.Run(mainWindow);
                }

                analyzer.SaveUnitsConfiguration();
                demoController.SaveConfiguration("DemoControllerConfiguration");
            }
            catch(System.IO.FileLoadException)
            {
                MessageBox.Show("Ошибка при открытии файла конфигурации!");
                return;
            }
            catch(System.IO.IOException)
            {
                MessageBox.Show("Ошибка при открытии файла конфигурации!");
                return;
            }
        }
    }
}
