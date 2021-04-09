using PresentationWinForms.Forms;
using AnalyzerControlCore;
using System;
using System.Windows.Forms;

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
                AnalyzerGateway core = new AnalyzerGateway();

                StartWindow startWindow = new StartWindow();
                MainWindow mainWindow = new MainWindow();

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

                core.SaveUnitsConfiguration();
            }
            catch(System.IO.FileLoadException)
            {
                MessageBox.Show("Ошибка при открытии файла конфигурации!");
                return;
            }
        }
    }
}
