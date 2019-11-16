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
    public partial class AdditionalMovesView : UserControl
    {
        public AdditionalMovesView()
        {
            InitializeComponent();
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Needle.HomeAll();
                    Core.Rotor.Home();
                });
        }

        private void moveOnCartridgeButton_Click(object sender, EventArgs e)
        {
            int cellNumber = (int)editCellNumber.Value;

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
            
            Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorController.CellPosition.CenterCell);
                    Core.Needle.TurnToCartridge(NeedleController.FromPosition.Home, cell);
                });
        }
    }
}
