using AnalyzerService;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class PompUnitView : UserControl
    {
        public PompUnitView()
        {
            InitializeComponent();
            if (Analyzer.Pomp != null)
                propertyGrid.SelectedObject = Analyzer.Pomp.Options;
        }

        private void buttonNeedleWashing_Click(object sender, EventArgs e)
        {
            int cycles = (int)editNumberCycles.Value;

            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Pomp.WashTheNeedle(cycles);
                });
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Pomp.Home();
                });
        }

        private void buttonSuction_Click(object sender, EventArgs e)
        {
            int value = (int)editSuctionValue.Value;

            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Pomp.Pull(value);
                });
        }

        private void buttonUnsuction_Click(object sender, EventArgs e)
        {
            int value = (int)editSuctionValue.Value;

            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Pomp.Push(value);
                });
        }
    }
}
