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
            Logger.NewInfoMessageAdded += Logger_NewInfoMessageAdded;
            Logger.NewDemoInfoMessageAdded += Logger_NewDemoInfoMessageAdded;
            Logger.NewControllerInfoMessageAdded += Logger_NewControllerInfoMessageAdded;
            Logger.OnNewMessageAdded += Logger_OnNewMessageAdded;
        }

        private void Logger_NewControllerInfoMessageAdded(string message)
        {
            BeginInvoke((Action)(() =>
            {
                AddMessage(message, Color.Green);
            }));
        }

        private void Logger_NewDemoInfoMessageAdded(string message)
        {
            BeginInvoke((Action)(() =>
            {
                AddMessage(message, Color.OrangeRed);
            }));
        }

        private void Logger_NewInfoMessageAdded(string message)
        {
            BeginInvoke((Action)(() =>
            {
                AddMessage(message, Color.Blue);
            }));
        }

        private void Logger_OnNewMessageAdded(string message)
        {
            BeginInvoke((Action)( () =>
            {
                AddMessage(message, Color.Black);
            }) );
        }

        public void AddMessage(string message, Color color)
        {
            this.InvokeThread(() => {
                logTextBox.SelectionColor = color;
                logTextBox.AppendText(message);
                logTextBox.Select(logTextBox.Text.Length, 0);
                logTextBox.ScrollToCaret();
            });
        }
    }
}
