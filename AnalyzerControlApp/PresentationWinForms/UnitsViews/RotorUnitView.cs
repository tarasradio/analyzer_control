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
            if (AnalyzerGateway.Rotor != null)
                propertyGrid.SelectedObject = AnalyzerGateway.Rotor.Options;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Rotor.Home();
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
                AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Rotor.Home();
                    AnalyzerGateway.Rotor.PlaceCellAtCharge(cellNumber, chargePosition);
                });
            }
            else if(selectNeedleLeftPlace.Checked)
            {
                AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Rotor.Home();
                    AnalyzerGateway.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorUnit.CellPosition.CellLeft);
                });
            }
            else if(selectNeedleRightPlace.Checked)
            {
                AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Rotor.Home();
                    AnalyzerGateway.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorUnit.CellPosition.CellRight);
                });
            }
            else if(selectWashBufferPlace.Checked)
            {
                AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Rotor.Home();
                    AnalyzerGateway.Rotor.PlaceCellUnderWashBuffer(cellNumber);
                });
            }
            else if(selectDischargePlace.Checked)
            {
                AnalyzerGateway.Executor.StartTask(
                () =>
                {
                    AnalyzerGateway.Rotor.Home();
                    AnalyzerGateway.Rotor.PlaceCellAtDischarge(cellNumber);
                });
            }
        }
    }
}
