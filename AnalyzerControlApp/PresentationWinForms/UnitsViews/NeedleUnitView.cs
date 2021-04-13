using AnalyzerService;
using AnalyzerDomain.Entyties;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class NeedleUnitView : UserControl
    {
        public NeedleUnitView()
        {
            InitializeComponent();
            if(Analyzer.Needle != null)
                propertyGrid.SelectedObject = Analyzer.Needle.Options;
        }

        private void buttonHomeRotator_Click(object sender, EventArgs e)
        {
            Analyzer.Executor.StartTask(
                () =>
                {
                    Analyzer.Needle.HomeRotator();
                });
        }

        private void turnOnTubeButton_Click(object sender, EventArgs e)
        {
            Analyzer.Executor.StartTask(
                () =>
                {
                    Analyzer.Needle.HomeLifterAndRotator();
                    Analyzer.Needle.TurnToTubeAndWaitTouch();
                });
        }

        private void moveOnWashingButton_Click(object sender, EventArgs e)
        {
            Analyzer.Executor.StartTask(
                () =>
                {
                    Analyzer.Needle.HomeLifterAndRotator();
                    Analyzer.Needle.TurnAndGoDownToWashing();
                });
        }

        private void buttonTurnToCartridge_Click(object sender, EventArgs e)
        {
            CartridgeCell cell = CartridgeCell.MixCell;

            if(selectFirstCell.Checked)
            {
                cell = CartridgeCell.FirstCell;
            }
            else if(selectSecondCell.Checked)
            {
                cell = CartridgeCell.SecondCell;
            }
            else if(selectThirdCell.Checked)
            {
                cell = CartridgeCell.ThirdCell;
            }
            else if(selectResultCell.Checked)
            {
                cell = CartridgeCell.ResultCell;
            }

            Analyzer.Executor.StartTask(
                () =>
                {
                    Analyzer.Needle.HomeLifterAndRotator();
                    Analyzer.Needle.TurnToCartridge(cell);
                });
        }

        private void buttonGoDownAndBrokeCartridge_Click(object sender, EventArgs e)
        {
            CartridgeCell cell = CartridgeCell.MixCell;

            if (selectFirstCell.Checked)
            {
                cell = CartridgeCell.FirstCell;
            }
            else if (selectSecondCell.Checked)
            {
                cell = CartridgeCell.SecondCell;
            }
            else if (selectThirdCell.Checked)
            {
                cell = CartridgeCell.ThirdCell;
            }
            else if (selectResultCell.Checked)
            {
                cell = CartridgeCell.ResultCell;
            }

            Analyzer.Executor.StartTask(
                () =>
                {
                    Analyzer.Needle.GoDownAndPerforateCartridge(cell);
                });
        }

        private void buttonHomeLift_Click(object sender, EventArgs e)
        {
            Analyzer.Executor.StartTask(
                () =>
                {
                    Analyzer.Needle.HomeLifter();
                });
        }
    }
}
