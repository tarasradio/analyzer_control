using AnalyzerControlCore;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class ChargeUnitView : UserControl
    {
        public ChargeUnitView()
        {
            InitializeComponent();

            if (AnalyzerGateway.Charger != null)
                propertyGrid.SelectedObject = AnalyzerGateway.Charger.GetConfiguration();
        }

        private void buttonHookHome_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Charger.HomeHook();
                });
        }

        private void buttonRotatorHome_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Charger.HomeRotator();
                });
        }

        private void buttonTurnChargeToCell_Click(object sender, EventArgs e)
        {
            int cell = (int)editCellNumber.Value;

            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Charger.HomeRotator();
                    AnalyzerGateway.Charger.TurnToCell(cell);
                });
        }

        private void buttonChargeCartridge_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Charger.HomeHook();
                    AnalyzerGateway.Charger.ChargeCartridge();
                    AnalyzerGateway.Charger.HomeHook();
                });
        }

        private void buttonTurnChargeToDischarge_Click(object sender, EventArgs e)
        {

        }
    }
}
