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

                StartForm startForm = new StartForm();
                MainForm mainForm = new MainForm();

                startForm.StartPosition = FormStartPosition.CenterScreen;
                mainForm.StartPosition = FormStartPosition.CenterScreen;

                if(useAuthentication)
                {
                    Application.Run(startForm);

                    if (startForm.IsAuthenticated)
                    {
                        Application.Run(mainForm);
                    }
                }
                else
                {
                    Application.Run(mainForm);
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
