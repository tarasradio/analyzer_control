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

            Logger.DebugMessageAdded += onDebugMessageAdded;
        }

        private void onDebugMessageAdded(string message)
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
