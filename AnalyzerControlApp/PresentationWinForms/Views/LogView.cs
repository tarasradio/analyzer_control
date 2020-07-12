using Infrastructure;
using PresentationWinForms.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationWinForms.Views
{
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();

            Logger.NewInfoMessageAdded += Logger_NewInfoMessageAdded;
            Logger.NewDemoInfoMessageAdded += Logger_NewDemoInfoMessageAdded;
            Logger.NewControllerInfoMessageAdded += Logger_NewControllerInfoMessageAdded;
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
