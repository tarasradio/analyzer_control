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

            if (Core.Charger != null)
                propertyGrid.SelectedObject = Core.Charger.GetConfiguration();
        }

        private void buttonHookHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Charger.HomeHook();
                });
        }

        private void buttonRotatorHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Charger.HomeRotator();
                });
        }

        private void buttonTurnChargeToCell_Click(object sender, EventArgs e)
        {
            int cell = (int)editCellNumber.Value;

            Core.Executor.StartTask(
                () =>
                {
                    Core.Charger.HomeRotator();
                    Core.Charger.TurnToCell(cell);
                });
        }

        private void buttonChargeCartridge_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Charger.HomeHook();
                    Core.Charger.ChargeCartridge();
                    Core.Charger.HomeHook();
                });
        }

        private void buttonTurnChargeToDischarge_Click(object sender, EventArgs e)
        {

        }
    }
}
