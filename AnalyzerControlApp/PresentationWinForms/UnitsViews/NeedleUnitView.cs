using AnalyzerService;
using AnalyzerDomain.Entyties;
using System;
using System.Windows.Forms;
using AnalyzerDomain.Models;
using AnalyzerControl;

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
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Needle.HomeRotator();
                });
        }

        private void turnOnTubeButton_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Needle.GoHome();
                    Analyzer.Needle.TurnToTubeAndWaitTouch();
                });
        }

        private void moveOnWashingButton_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Needle.GoHome();
                    Analyzer.Needle.TurnAndGoDownToWashing(false);
                });
        }

        private void buttonTurnToCartridge_Click(object sender, EventArgs e)
        {
            CartridgeWell well = CartridgeWell.ACW;
            well = selectWell(well);

            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    //Analyzer.Needle.GoHome();
                    Analyzer.Needle.TurnToCartridge(well);
                });
        }

        private void buttonGoDownAndBrokeCartridge_Click(object sender, EventArgs e)
        {
            CartridgeWell well = CartridgeWell.ACW;

            if (selectW1.Checked)
                well = CartridgeWell.W1;
            else if (selectW2.Checked)
                well = CartridgeWell.W2;
            else if (selectW3.Checked)
                well = CartridgeWell.W3;
            else if (selectCUV.Checked)
                well = CartridgeWell.CUV;

            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Needle.PerforateCartridge(well);
                });
        }

        private CartridgeWell selectWell(CartridgeWell well)
        {
            if (selectW1.Checked)
                well = CartridgeWell.W1;
            else if (selectW2.Checked)
                well = CartridgeWell.W2;
            else if (selectW3.Checked)
                well = CartridgeWell.W3;
            else if (selectCUV.Checked)
                well = CartridgeWell.CUV;
            return well;
        }

        private void buttonHomeLift_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Needle.HomeLifter();
                });
        }

        private void buttonTurnAndGoDownToWashingAlkali_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Needle.GoHome();
                    Analyzer.Needle.TurnAndGoDownToWashing(true);
                });
        }

        private void buttonWashing2_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Needle.GoHome();
                    AnalyzerOperations.WashNeedle2();
                });
        }

        private void buttonGoToSafeLevel_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Needle.GoToSafeLevel();
                });
        }
    }
}
