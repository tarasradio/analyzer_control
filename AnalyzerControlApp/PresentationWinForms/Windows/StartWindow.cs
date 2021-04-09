using AutoUpdaterDotNET;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.Forms
{
    public partial class StartWindow : Form
    {
        public bool IsAuthenticated { get; private set; }

        public StartWindow()
        {
            InitializeComponent();
            AutoUpdater.ReportErrors = true;
            AutoUpdater.Mandatory = true;
            AutoUpdater.Start("https://raw.githubusercontent.com/tarasradio/stepper_control/master/updates/AnalyzerControlApp.xml");
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
