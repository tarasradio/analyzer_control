using AnalyzerControlCore;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class PompUnitView : UserControl
    {
        public PompUnitView()
        {
            InitializeComponent();
            if (Core.Pomp != null)
                propertyGrid.SelectedObject = Core.Pomp.Options;
        }

        private void buttonNeedleWashing_Click(object sender, EventArgs e)
        {
            int cycles = (int)editNumberCycles.Value;

            Core.Executor.StartTask(
                () =>
                {
                    Core.Pomp.WashingNeedle(cycles);
                });
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Pomp.Home();
                });
        }

        private void buttonSuction_Click(object sender, EventArgs e)
        {
            int value = (int)editSuctionValue.Value;

            Core.Executor.StartTask(
                () =>
                {
                    Core.Pomp.Suction(value);
                });
        }

        private void buttonUnsuction_Click(object sender, EventArgs e)
        {
            int value = (int)editSuctionValue.Value;

            Core.Executor.StartTask(
                () =>
                {
                    Core.Pomp.Unsuction(value);
                });
        }
    }
}
