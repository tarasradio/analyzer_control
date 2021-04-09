using AnalyzerControlCore;
using Infrastructure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationWinForms.Forms
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();

            AnalyzerGateway.Serial.ConnectionChanged += Serial_ConnectionChanged;
            //runWpfWindow();
        }

        private void Serial_ConnectionChanged(bool connected)
        {
            updateControlsState();
        }

        private void runWpfWindow()
        {
            PresentationWPF.EditAnalisysWindow editAnalisysWindow = new PresentationWPF.EditAnalisysWindow();
            editAnalisysWindow.InitializeComponent();
            editAnalisysWindow.Show();
        }

        private void buttonShowControlPanel_Click(object sender, EventArgs e)
        {
            ControlPanelWindow controlPanel = new ControlPanelWindow(AnalyzerGateway.AppConfig.Steppers);
            controlPanel.StartPosition = FormStartPosition.CenterScreen;
            controlPanel.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            updateControlsState();

            Text = $"Управление анализами - версия {Application.ProductVersion}";

            steppersGridView.UpdateInformation();
            devicesControlView.UpdateInformation();
            sensorsView.UpdateInformation();

            demoExecutorView.StartUpdate();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Сохранить настройки при выходе из программы?",
                "Сохранение настроек при выходе из программы",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

            if(dialogResult == DialogResult.Yes)
            {
                AnalyzerGateway.SaveAppConfiguration();
            }
            else if(dialogResult == DialogResult.Cancel ||
                dialogResult == DialogResult.Abort)
            {
                e.Cancel = true;
                return;
            }

            demoExecutorView.StopUpdate();

            if (AnalyzerGateway.Serial.IsOpen())
            {
                AnalyzerGateway.Serial.Close();

                steppersGridView.StopUpdate();
                sensorsView.StopUpdate();
            }
        }

        private void abortExecutionButton_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.AbortExecution();
        }

        private void buttonStartDemo_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Demo.StartWork();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F8)
            {
                AnalyzerGateway.AbortExecution();
            }
            if(e.KeyCode == Keys.F7)
            {
                AnalyzerGateway.Demo.StartWork();
            }
        }

        private void buttonConnection_Click(object sender, EventArgs e)
        {
            ConnectionSettingsWindow settingsWindow = new ConnectionSettingsWindow();
            settingsWindow.StartPosition = FormStartPosition.CenterScreen;
            settingsWindow.Show();
        }

        private void updateControlsState()
        {
            if (AnalyzerGateway.Serial.IsOpen())
            {
                connectionState.Text = $"Установленно соединение с { AnalyzerGateway.Serial.PortName }.";
                connectionState.ForeColor = Color.DarkGreen;
            }
            else
            {
                connectionState.Text = "Соединение не установлено.";
                connectionState.ForeColor = Color.Brown;
            }
            buttonStartDemo.Visible = AnalyzerGateway.Serial.IsOpen();
        }
    }
}