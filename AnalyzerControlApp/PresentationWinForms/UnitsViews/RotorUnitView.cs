using AnalyzerService;
using AnalyzerService.Units;
using AnalyzerDomain.Entyties;
using System;
using System.Windows.Forms;
using AnalyzerDomain.Models;

namespace PresentationWinForms.UnitsViews
{
    public partial class RotorUnitView : UserControl
    {
        public RotorUnitView()
        {
            InitializeComponent();
            if (Analyzer.Rotor != null)
                propertyGrid.SelectedObject = Analyzer.Rotor.Options;
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Rotor.Home();
                });
        }

        private void buttonMoveCell_Click(object sender, EventArgs e)
        {
            int cellNumber = (int)editCellNumber.Value;
            int chargePosition = (int)editChargePosition.Value;

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


            if (selectChargePlace.Checked)
            {
                Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Rotor.Home();
                    Analyzer.Rotor.PlaceCellAtCharge(cellNumber, chargePosition);
                });
            }
            else if(selectNeedleLeftPlace.Checked)
            {
                Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Rotor.Home();
                    Analyzer.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorUnit.CellPosition.CellLeft);
                });
            }
            else if(selectNeedleRightPlace.Checked)
            {
                Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Rotor.Home();
                    Analyzer.Rotor.PlaceCellUnderNeedle(cellNumber, cell, RotorUnit.CellPosition.CellRight);
                });
            }
            else if(selectWashBufferPlace.Checked)
            {
                Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Rotor.Home();
                    Analyzer.Rotor.PlaceCellUnderWashBuffer(cellNumber);
                });
            }
            else if(selectDischargePlace.Checked)
            {
                Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Rotor.Home();
                    Analyzer.Rotor.PlaceCellAtDischarge(cellNumber);
                });
            }
        }
    }
}
