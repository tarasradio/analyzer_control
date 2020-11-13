using AnalyzerControlCore;
using Infrastructure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationWinForms.Forms
{
    public partial class MainForm : Form
    {
        ControlPanelForm controlPanel = null;

        private void UpdateControlsState()
        {
            if (AnalyzerGateway.Serial.IsOpen())
            {
                buttonConnect.Text = "Отключение";
                connectionState.Text = $"Установленно соединение с { AnalyzerGateway.Serial.PortName }.";
                connectionState.ForeColor = Color.DarkGreen;
            }
            else
            {
                buttonConnect.Text = "Подключение";
                connectionState.Text = "Соединение не установлено.";
                connectionState.ForeColor = Color.Brown;
            }

            buttonUpdateList.Visible = !AnalyzerGateway.Serial.IsOpen();
            selectPort.Enabled = !AnalyzerGateway.Serial.IsOpen();
            editBaudrate.Enabled = !AnalyzerGateway.Serial.IsOpen();
            buttonStartDemo.Visible = AnalyzerGateway.Serial.IsOpen();
        }

        public MainForm()
        {
            InitializeComponent();

            PresentationWPF.EditAnalisysWindow editAnalisysWindow = new PresentationWPF.EditAnalisysWindow();
            editAnalisysWindow.InitializeComponent();
            editAnalisysWindow.Show();
        }

        private void buttonShowControlPanel_Click(object sender, EventArgs e)
        {
            controlPanel = new ControlPanelForm(AnalyzerGateway.AppConfig.Steppers);
            controlPanel.StartPosition = FormStartPosition.CenterScreen;
            controlPanel.Show();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (AnalyzerGateway.Serial.IsOpen())
            {
                AnalyzerGateway.Serial.Close();

                steppersGridView.StopUpdate();
                sensorsView.StopUpdate();

                rescanOpenPorts();
            }
            else
            {
                string portName = selectPort.SelectedItem.ToString();
                int baudrate = int.Parse(editBaudrate.SelectedItem.ToString());
                
                if( AnalyzerGateway.Serial.Open(portName, baudrate) )
                {
                    //Core.CheckFirmwareVersion();
                    
                    Logger.Info(
                        "Открытие подключения - подключение к " + portName + " открыто");

                    steppersGridView.StartUpdate();
                    sensorsView.StartUpdate();
                }
                else
                {
                    Logger.Info(
                        "Открытие подключения - Ошибка при подключении!");
                }
            }

            UpdateControlsState();
        }

        private void buttonUpdateList_Click(object sender, EventArgs e)
        {
            rescanOpenPorts();
        }

        private void rescanOpenPorts()
        {
            selectPort.Items.Clear();

            String[] portsNames = AnalyzerGateway.Serial.GetAvailablePorts();

            if(portsNames.Length != 0)
            {
                selectPort.Items.AddRange(portsNames);
                Logger.Info("Поиск портов - найдены открытые порты");
                selectPort.SelectedIndex = 0;
            }
            else
            {
                Logger.Info("Поиск портов - открытых портов не найдено");
                selectPort.SelectedText = "";
            }

            buttonConnect.Visible = (portsNames.Length != 0);
            editBaudrate.SelectedIndex = editBaudrate.Items.Count - 1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateControlsState();
            rescanOpenPorts();

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
            if(e.KeyCode == Keys.F5)
            {
                rescanOpenPorts();
            }
            if(e.KeyCode == Keys.F7)
            {
                AnalyzerGateway.Demo.StartWork();
            }
        }
    }
}