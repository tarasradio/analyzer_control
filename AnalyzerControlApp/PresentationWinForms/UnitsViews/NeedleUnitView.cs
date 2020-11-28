using AnalyzerControlCore;
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
            if(AnalyzerGateway.Needle != null)
                propertyGrid.SelectedObject = AnalyzerGateway.Needle.Options;
        }

        private void buttonHomeRotator_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Needle.HomeRotator();
                });
        }

        private void turnOnTubeButton_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Needle.HomeLifterAndRotator();
                    AnalyzerGateway.Needle.TurnToTubeAndWaitTouch();
                });
        }

        private void moveOnWashingButton_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Needle.HomeLifterAndRotator();
                    AnalyzerGateway.Needle.TurnAndGoDownToWashing();
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

            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Needle.HomeLifterAndRotator();
                    AnalyzerGateway.Needle.TurnToCartridge(cell);
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

            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Needle.GoDownAndPerforateCartridge(cell);
                });
        }

        private void buttonHomeLift_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Needle.HomeLifter();
                });
        }
    }
}
