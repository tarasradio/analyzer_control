using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteppersControlCore;
using SteppersControlCore.Controllers;
using SteppersControlCore.Elements;

namespace SteppersControlApp.ControllersViews
{
    public partial class RotorControllerView : UserControl
    {
        public RotorControllerView()
        {
            InitializeComponent();
            if (Core.Rotor != null)
                propertyGrid.SelectedObject = Core.Rotor.Properties;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CncExecutor.ExecuteTask(Core.Rotor.Home());
                });
        }

        private void buttonMoveCell_Click(object sender, EventArgs e)
        {
            int cellNumber = (int)editCellNumber.Value;
            int loadPosition = (int)editLoadPosition.Value;

            CartridgeCell cell = CartridgeCell.WhiteCell;

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

            if (selectLoadPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.CncExecutor.ExecuteTask(Core.Rotor.Home());
                    Core.CncExecutor.ExecuteTask(Core.Rotor.MoveToLoad(cellNumber, loadPosition));
                });
            }
            else if(selectNeedleLeftPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.CncExecutor.ExecuteTask(Core.Rotor.Home());
                    Core.CncExecutor.ExecuteTask(Core.Rotor.MoveCellUnderNeedle(cellNumber, cell,
                        RotorController.CellPosition.CellLeft));
                });
            }
            else if(selectNeedleRightPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.CncExecutor.ExecuteTask(Core.Rotor.Home());
                    Core.CncExecutor.ExecuteTask(Core.Rotor.MoveCellUnderNeedle(cellNumber, cell,
                        RotorController.CellPosition.CellRight));
                });
            }
            else if(selectWashingPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.CncExecutor.ExecuteTask(Core.Rotor.Home());
                    Core.CncExecutor.ExecuteTask(Core.Rotor.MoveToWashBuffer());
                });
            }
            else if(selectUnloadPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.CncExecutor.ExecuteTask(Core.Rotor.Home());
                    Core.CncExecutor.ExecuteTask(Core.Rotor.MoveToUnload());
                });
            }
        }
    }
}
