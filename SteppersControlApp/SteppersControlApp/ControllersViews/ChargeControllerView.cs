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

namespace SteppersControlApp.ControllersViews
{
    public partial class ChargeControllerView : UserControl
    {
        public ChargeControllerView()
        {
            InitializeComponent();
            if (Core.Charger != null)
                propertyGrid.SelectedObject = Core.Charger.Properties;
        }

        private void buttonHookHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Charger.HomeHook();
                });
        }

        private void buttonRotatorHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Charger.HomeRotator();
                });
        }

        private void buttonTurnChargeToCell_Click(object sender, EventArgs e)
        {
            int cell = (int)editCellNumber.Value;

            Core.Executor.StartTask(
                () =>
                {
                    Core.Charger.HomeRotator();
                    Core.Charger.TurnToCell(cell);
                });
        }

        private void buttonChargeCartridge_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Charger.HomeHook();
                    Core.Charger.ChargeCartridge();
                    Core.Charger.HomeHook();
                });
        }

        private void buttonTurnChargeToDischarge_Click(object sender, EventArgs e)
        {

        }
    }
}
