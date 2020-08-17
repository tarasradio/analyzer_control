using AnalyzerControlCore;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class ConveyorUnitView : UserControl
    {
        public ConveyorUnitView()
        {
            InitializeComponent();
            if(AnalyzerGateway.Conveyor != null)
                propertyGrid.SelectedObject = AnalyzerGateway.Conveyor.Options;
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Conveyor.PrepareBeforeScanning();
                });
        }

        private void buttonScanAndTurn_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Conveyor.RotateAndScanTube();
                });
        }
    }
}
