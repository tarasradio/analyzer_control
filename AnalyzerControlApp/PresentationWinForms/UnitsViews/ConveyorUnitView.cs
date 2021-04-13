using AnalyzerService;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class ConveyorUnitView : UserControl
    {
        public ConveyorUnitView()
        {
            InitializeComponent();
            if(Analyzer.Conveyor != null)
                propertyGrid.SelectedObject = Analyzer.Conveyor.Options;
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            Analyzer.Executor.StartTask(
                () =>
                {
                    Analyzer.Conveyor.PrepareBeforeScanning();
                });
        }

        private void buttonScanAndTurn_Click(object sender, EventArgs e)
        {
            Analyzer.Executor.StartTask(
                () =>
                {
                    Analyzer.Conveyor.RotateAndScanTube();
                });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Analyzer.Executor.StartTask(
               () =>
               {
                   Analyzer.Conveyor.Shift(reverse: false, 
                       shiftType: AnalyzerService.Units.ConveyorUnit.ShiftType.OneTube);
               });
        }
    }
}
