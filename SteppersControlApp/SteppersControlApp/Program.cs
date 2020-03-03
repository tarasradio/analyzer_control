using SteppersControlApp.Forms;
using SteppersControlCore;
using System;
using System.Windows.Forms;

namespace SteppersControlApp
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
                Core core = new Core("settings/config.xml");

                StartForm startForm = new StartForm();
                MainForm mainForm = new MainForm();

                startForm.StartPosition = FormStartPosition.CenterScreen;
                mainForm.StartPosition = FormStartPosition.CenterScreen;

                Application.Run(startForm);

                if (startForm.IsAuthenticated)
                {
                    Application.Run(mainForm);
                }

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
