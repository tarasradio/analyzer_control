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
            if(Core.Conveyor != null)
                propertyGrid.SelectedObject = Core.Conveyor.Options;
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Conveyor.PrepareBeforeScanning();
                });
        }

        private void buttonScanAndTurn_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Conveyor.RotateAndScanTube();
                });
        }
    }
}
