using System;
using System.Windows.Forms;

using SteppersControlCore;
using SteppersControlCore.Controllers;
using SteppersControlCore.Elements;

namespace SteppersControlApp.Controllers
{
    public partial class ArmControllerView : UserControl
    {
        public ArmControllerView()
        {
            InitializeComponent();
            if(Core.Arm != null)
                propertyGrid.SelectedObject = Core.Arm.Props;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CNCExecutor.ExecuteTask(Core.Arm.Home());
                });
        }

        private void turnOnTubeButton_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CNCExecutor.ExecuteTask(Core.Arm.Home());
                    Core.CNCExecutor.ExecuteTask(Core.Arm.MoveOnTube());
                });
        }

        private void moveOnWashingButton_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CNCExecutor.ExecuteTask(Core.Arm.Home());
                    Core.CNCExecutor.ExecuteTask(Core.Arm.MoveOnWashing());
                });
        }

        private void moveOnCartridgeButton_Click(object sender, EventArgs e)
        {
            CartridgeCell cell = CartridgeCell.WhiteCell;

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

            ArmController.FromPosition fromPosition = ArmController.FromPosition.Home;

            Core.Executor.StartTask(
                () =>
                {
                    Core.CNCExecutor.ExecuteTask(Core.Arm.Home());
                    Core.CNCExecutor.ExecuteTask(Core.Arm.MoveToCartridge(fromPosition, cell));
                });
        }

        private void buttonBrokeCartridge_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CNCExecutor.ExecuteTask(Core.Arm.BrokeCartridge());
                });
        }
    }
}
