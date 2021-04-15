using AnalyzerService;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class ChargeUnitView : UserControl
    {
        public ChargeUnitView()
        {
            InitializeComponent();

            if (Analyzer.Charger != null)
                propertyGrid.SelectedObject = Analyzer.Charger.Options;
        }

        private void buttonHookHome_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeHook();
                });
        }

        private void buttonRotatorHome_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeRotator();
                });
        }

        private void buttonTurnChargeToCell_Click(object sender, EventArgs e)
        {
            int cell = (int)editCellNumber.Value;

            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeRotator();
                    Analyzer.Charger.TurnToCell(cell);
                });
        }

        private void buttonChargeCartridge_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeHook();
                    Analyzer.Charger.ChargeCartridge();
                    Analyzer.Charger.HomeHook();
                });
        }

        private void buttonTurnChargeToDischarge_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeRotator();
                    Analyzer.Charger.TurnToDischarge();
                });
        }
    }
}
