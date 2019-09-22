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

namespace SteppersControlApp.Controllers
{
    public partial class TransporterControllerView : UserControl
    {
        public TransporterControllerView()
        {
            InitializeComponent();
            if(Core.Transporter != null)
                propertyGrid.SelectedObject = Core.Transporter.Props;
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CNCExecutor.ExecuteTask(Core.Transporter.PrepareBeforeScanning());
                });
        }

        private void buttonScanAndTurn_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CNCExecutor.ExecuteTask(Core.Transporter.TurnAndScanTube());
                });
        }
    }
}
