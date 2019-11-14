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
    public partial class LoadControllerView : UserControl
    {
        public LoadControllerView()
        {
            InitializeComponent();
            if (Core.Loader != null)
                propertyGrid.SelectedObject = Core.Loader.Properties;
        }

        private void buttonShuttleHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Loader.HomeShuttle();
                });
        }

        private void buttonLoadHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Loader.HomeLoad();
                });
        }

        private void buttonTurnLoad_Click(object sender, EventArgs e)
        {
            int cell = (int)editCellNumber.Value;

            Core.Executor.StartTask(
                () =>
                {
                    Core.Loader.HomeLoad();
                    Core.Loader.TurnLoadToCell(cell);
                });
        }

        private void buttonMoveShuttleToCassette_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Loader.MoveShuttleToCassette();
                });
        }
    }
}
