﻿using System;
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
                    Core.Rotor.Home();
                });
        }

        private void buttonMoveCell_Click(object sender, EventArgs e)
        {
            int cellNumber = (int)editCellNumber.Value;
            int chargePosition = (int)editChargePosition.Value;

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

            if (selectChargePlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.Home();
                    Core.Rotor.PlaceCellAtCharge(cellNumber, chargePosition);
                });
            }
            else if(selectNeedleLeftPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.Home();
                    Core.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorController.CellPosition.CellLeft);
                });
            }
            else if(selectNeedleRightPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.Home();
                    Core.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorController.CellPosition.CellRight);
                });
            }
            else if(selectWashBufferPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.Home();
                    Core.Rotor.PlaceCellUnderWashBuffer();
                });
            }
            else if(selectDischargePlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.Home();
                    Core.Rotor.PlaceCellAtDischarge();
                });
            }
        }
    }
}
