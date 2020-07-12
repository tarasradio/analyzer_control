using AnalyzerControlCore;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class TransporterUnitView : UserControl
    {
        public TransporterUnitView()
        {
            InitializeComponent();
            if(Core.Transporter != null)
                propertyGrid.SelectedObject = Core.Transporter.Config;
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Transporter.PrepareBeforeScanning();
                });
        }

        private void buttonScanAndTurn_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Transporter.RotateAndScanTube();
                });
        }
    }
}
