using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteppersControlApp.Utils;

using SteppersControlCore;

namespace SteppersControlApp.Views
{
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
            Logger.OnNewMessageAdded += Logger_OnNewMessageAdded;
        }

        private void Logger_OnNewMessageAdded(string message)
        {
            BeginInvoke((Action)( () =>
            {
                AddMessage(message);
            }) );
        }

        public void AddMessage(string message)
        {
            this.InvokeThread(() => {
                logTextBox.AppendText(message);
                logTextBox.Select(logTextBox.Text.Length, 0);
                logTextBox.ScrollToCaret();
            });
        }
    }
}
