using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AutoUpdaterDotNET;

namespace SteppersControlApp.Forms
{
    public partial class StartForm : Form
    {
        public bool IsAuthenticated { get; private set; }

        public StartForm()
        {
            InitializeComponent();
            AutoUpdater.ReportErrors = true;
            AutoUpdater.Start("https://github.com/tarasradio/stepper_control/updates/StepperControlApp.xml");
        }

        private void ButtonServiceMode_Click(object sender, EventArgs e)
        {
            IsAuthenticated = true;
            DialogResult = DialogResult.OK;
            Dispose();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
