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
    public partial class TransporterControllerView : UserControl
    {
        public TransporterControllerView()
        {
            InitializeComponent();
            if(Core.Transporter != null)
                propertyGrid.SelectedObject = Core.Transporter.Properties;
        }

        private void buttonPrepare_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CncExecutor.ExecuteTask(Core.Transporter.PrepareBeforeScanning());
                });
        }

        private void buttonScanAndTurn_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.CncExecutor.ExecuteTask(Core.Transporter.TurnAndScanTube());
                });
        }
    }
}
