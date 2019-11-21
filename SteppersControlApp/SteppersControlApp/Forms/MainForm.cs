using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;

using SteppersControlCore;
using SteppersControlCore.MachineControl;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.AdditionalCommands;
using SteppersControlCore.SerialCommunication;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using SteppersControlApp.Properties;

namespace SteppersControlApp.Forms
{
    public partial class MainForm : Form
    {
        ControlPanelForm controlPanel = null;

        ResourceManager resourceManager;
        CultureInfo culture = Thread.CurrentThread.CurrentCulture;

        private void UpdateControlsState()
        {
            if (Core.Serial.IsOpen())
            {
                buttonConnect.Text = resourceManager.GetString("disconnect_text", Core.Settings.Culture);
                connectionState.Text = $"Установленно соединение с { Core.Serial.PortName }.";
                connectionState.ForeColor = Color.DarkGreen;
            }
            else
            {
                buttonConnect.Text = resourceManager.GetString("connect_text", Core.Settings.Culture);
                connectionState.Text = "Соединение не установлено.";
                connectionState.ForeColor = Color.Brown;
            }

            buttonUpdateList.Visible = !Core.Serial.IsOpen();
            selectPort.Enabled = !Core.Serial.IsOpen();
            editBaudrate.Enabled = !Core.Serial.IsOpen();
            buttonStartDemo.Visible = Core.Serial.IsOpen();
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void UpdateCurrentLanguage()
        {
            buttonConnect.Text = resourceManager.GetString("connect_text", Core.Settings.Culture);
            buttonUpdateList.Text = resourceManager.GetString("button_update_text", Core.Settings.Culture);
            buttonShowControlPanel.Text = resourceManager.GetString("button_show_control_panel_text", Core.Settings.Culture);
            buttonAbortExecution.Text = resourceManager.GetString("button_abort_execution_text", Core.Settings.Culture);
            buttonStartDemo.Text = resourceManager.GetString("button_start_demo_text", Core.Settings.Culture);
        }

        private void buttonShowControlPanel_Click(object sender, EventArgs e)
        {
            controlPanel = new ControlPanelForm(Core.Settings.Steppers);
            controlPanel.StartPosition = FormStartPosition.CenterScreen;
            controlPanel.Show();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (Core.Serial.IsOpen())
            {
                Core.Serial.Close();

                steppersGridView.StopUpdate();
                sensorsView.StopUpdate();

                rescanOpenPorts();
            }
            else
            {
                string portName = selectPort.SelectedItem.ToString();
                int baudrate = int.Parse(editBaudrate.SelectedItem.ToString());
                
                if( Core.Serial.Open(portName, baudrate) )
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

            String[] portsNames = Core.Serial.GetAvailablePorts();

            if(portsNames.Length != 0)
            {
                selectPort.Items.AddRange(portsNames);
                Logger.Info( resourceManager.GetString("ports_found", culture));
                selectPort.SelectedIndex = 0;
            }
            else
            {
                Logger.Info(
                    resourceManager.GetString("no_ports_found", culture));
                selectPort.SelectedText = "";
            }

            buttonConnect.Visible = (portsNames.Length != 0);
            editBaudrate.SelectedIndex = editBaudrate.Items.Count - 1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            resourceManager = new ResourceManager("SteppersControlApp.Strings", typeof(Resources).Assembly);
            culture = new CultureInfo("ru-RU");

            UpdateCurrentLanguage();
            
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
                Core.Settings.SaveToFile("settings\\config.xml");
            }
            else if(dialogResult == DialogResult.Cancel ||
                dialogResult == DialogResult.Abort)
            {
                e.Cancel = true;
                return;
            }

            demoExecutorView.StopUpdate();

            if (Core.Serial.IsOpen())
            {
                Core.Serial.Close();

                steppersGridView.StopUpdate();
                sensorsView.StopUpdate();
            }
        }

        private void abortExecutionButton_Click(object sender, EventArgs e)
        {
            Core.AbortExecution();
        }

        private void buttonStartDemo_Click(object sender, EventArgs e)
        {
            Core.Demo.StartDemo();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F8)
            {
                Core.AbortExecution();
            }
            if(e.KeyCode == Keys.F5)
            {
                rescanOpenPorts();
            }
            if(e.KeyCode == Keys.F7)
            {
                Core.Demo.StartDemo();
            }
        }
    }
}