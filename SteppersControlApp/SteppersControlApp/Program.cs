using SteppersControlCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Core core = new Core("config.xml");

                StartForm authForm = new StartForm();
                MainForm mainForm = new MainForm();

                authForm.StartPosition = FormStartPosition.CenterScreen;
                mainForm.StartPosition = FormStartPosition.CenterScreen;

                Application.Run(authForm);

                if (authForm.IsAuthenticated)
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
