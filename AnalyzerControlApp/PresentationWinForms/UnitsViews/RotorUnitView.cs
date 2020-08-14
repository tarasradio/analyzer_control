using AnalyzerConfiguration;
using AnalyzerControlCore;
using AnalyzerControlCore.Units;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class RotorUnitView : UserControl
    {
        public RotorUnitView()
        {
            InitializeComponent();
            if (Core.Rotor != null)
                propertyGrid.SelectedObject = Core.Rotor.Options;
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
                    Core.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorUnit.CellPosition.CellLeft);
                });
            }
            else if(selectNeedleRightPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.Home();
                    Core.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorUnit.CellPosition.CellRight);
                });
            }
            else if(selectWashBufferPlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.Home();
                    Core.Rotor.PlaceCellUnderWashBuffer(cellNumber);
                });
            }
            else if(selectDischargePlace.Checked)
            {
                Core.Executor.StartTask(
                () =>
                {
                    Core.Rotor.Home();
                    Core.Rotor.PlaceCellAtDischarge(cellNumber);
                });
            }
        }
    }
}
