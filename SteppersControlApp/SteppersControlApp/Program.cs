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

            AuthForm authForm = new AuthForm();
            MainForm mainForm = new MainForm();
            authForm.StartPosition = FormStartPosition.CenterScreen;
            mainForm.StartPosition = FormStartPosition.CenterScreen;

            Application.Run(authForm);

            if(authForm.IsAuthenticated)
            {
                Application.Run(mainForm);
            }
        }
    }
}
