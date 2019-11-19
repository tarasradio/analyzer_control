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
using SteppersControlCore.Controllers;
using SteppersControlCore.Elements;

namespace SteppersControlApp.ControllersViews
{
    public partial class AdditionalMovesView : UserControl
    {
        public AdditionalMovesView()
        {
            InitializeComponent();
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Core.Executor.StartTask(
                () =>
                {
                    Core.Needle.HomeAll();
                    Core.Rotor.Home();
                });
        }

        private void moveOnCartridgeButton_Click(object sender, EventArgs e)
        {

        }
    }
}
