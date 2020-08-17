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
            if (AnalyzerGateway.Pomp != null)
                propertyGrid.SelectedObject = AnalyzerGateway.Pomp.Options;
        }

        private void buttonNeedleWashing_Click(object sender, EventArgs e)
        {
            int cycles = (int)editNumberCycles.Value;

            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Pomp.WashingNeedle(cycles);
                });
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Pomp.Home();
                });
        }

        private void buttonSuction_Click(object sender, EventArgs e)
        {
            int value = (int)editSuctionValue.Value;

            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Pomp.Suction(value);
                });
        }

        private void buttonUnsuction_Click(object sender, EventArgs e)
        {
            int value = (int)editSuctionValue.Value;

            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Pomp.Unsuction(value);
                });
        }
    }
}
