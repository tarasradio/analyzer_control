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

            try
            {
                Core core = new Core("Configuration");

                StartForm startForm = new StartForm();
                MainForm mainForm = new MainForm();

                startForm.StartPosition = FormStartPosition.CenterScreen;
                mainForm.StartPosition = FormStartPosition.CenterScreen;

                //Application.Run(startForm);

                //if (startForm.IsAuthenticated)
                //{
                    Application.Run(mainForm);
                //}

                core.SaveConfiguration();
            }
            catch(System.IO.FileLoadException)
            {
                MessageBox.Show("Ошибка при открытии файла конфигурации!");
                return;
            }
        }
    }
}
