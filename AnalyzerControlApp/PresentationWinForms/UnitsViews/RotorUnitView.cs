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

            CartridgeWell well = CartridgeWell.ACW;

            if (selectW1.Checked)
                well = CartridgeWell.W1;
            else if (selectW2.Checked)
                well = CartridgeWell.W2;
            else if (selectW3.Checked)
                well = CartridgeWell.W3;
            else if (selectCUV.Checked)
                well = CartridgeWell.CUV;

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
                    Analyzer.Rotor.PlaceCellUnderNeedle(cellNumber, well, RotorUnit.CellPosition.CellLeft);
                });
            }
            else if(selectNeedleRightPlace.Checked)
            {
                Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Rotor.Home();
                    Analyzer.Rotor.PlaceCellUnderNeedle(cellNumber, well, RotorUnit.CellPosition.CellRight);
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
