using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerService;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class ChargeUnitView : UserControl
    {
        private string[] colors =
        {
            "Без цвета",
            "Зеленый",
            "Красный",
            "Синий"
        };

        public ChargeUnitView()
        {
            InitializeComponent();

            selectColor.Items.AddRange(colors);
            selectColor.SelectedIndex = 0;

            if (Analyzer.Charger != null)
                propertyGrid.SelectedObject = Analyzer.Charger.Options;
        }

        private void buttonHookHome_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeHook(false);
                    Analyzer.Charger.MoveHookAfterHome();
                });
        }

        private void buttonRotatorHome_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeRotator();
                });
        }

        private void buttonTurnChargeToCell_Click(object sender, EventArgs e)
        {
            int cell = (int)editCellNumber.Value;

            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeRotator();
                    Analyzer.Charger.TurnToCell(cell);
                });
        }

        private void buttonChargeCartridge_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeHook(false);
                    Analyzer.Charger.ChargeCartridge();
                    Analyzer.Charger.HomeHook(true);
                    Analyzer.Charger.MoveHookAfterHome();
                });
        }

        private void buttonTurnChargeToDischarge_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.HomeRotator();
                    Analyzer.Charger.TurnToDischarge();
                });
        }

        private void buttonDischargeCartridge_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.DischargeCartridge();
                    Analyzer.Charger.HomeHook(false);
                    Analyzer.Charger.MoveHookAfterHome();
                });
        }

        private void buttonScanCartridge_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.TurnScanner(true);
                    Analyzer.Charger.ScanBarcode();
                    System.Threading.Thread.Sleep(1000); // Типа ожидаем, когда бар-код будет прочитан
                    Analyzer.Charger.TurnScanner(false);
                });
        }

        private void buttonSetCellColor_Click(object sender, EventArgs e)
        {
            int color_index = selectColor.SelectedIndex;

            LEDColor color = new LEDColor();

            if(color_index == 0) {
                color = LEDColor.NoColor();
            } else if(color_index == 1) {
                color = LEDColor.Green();
            } else if (color_index == 2) {
                color = LEDColor.Red();
            } else if (color_index == 3) {
                color = LEDColor.Blue();
            }

            Analyzer.Serial.SendPacket(new SetLedColorCommand((int)editCellNumber.Value, color).GetBytes());
        }

        private void buttonHookCenter_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
                () =>
                {
                    Analyzer.Charger.TurnScanner(true);
                    Analyzer.Charger.ScanBarcode();
                    System.Threading.Thread.Sleep(2000); // Типа ожидаем, когда бар-код будет прочитан
                    Analyzer.Charger.TurnScanner(false);
                });
        }
    }
}
