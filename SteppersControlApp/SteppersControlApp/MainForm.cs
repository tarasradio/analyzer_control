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

namespace SteppersControlApp
{
    public partial class MainForm : Form
    {
        ControlPanelForm controlPanel = new ControlPanelForm();

        ResourceManager resourceManager;
        CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;

        private void UpdateState()
        {
            if (Core.Serial.IsConnected())
            {
                buttonConnect.Text = resourceManager.GetString("disconnect_text", Core.Settings.Culture);
                //buttonConnect.Text = "Отключиться";
                connectionState.Text = "Подключен";
                connectionState.ForeColor = Color.DarkGreen;
            }
            else
            {
                resourceManager.GetString("connect_text", Core.Settings.Culture);
                //buttonConnect.Text = "Подключиться";
                connectionState.Text = "Не подключен";
                connectionState.ForeColor = Color.Brown;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            resourceManager = new ResourceManager("SteppersControlApp.Strings", typeof(Resources).Assembly);
            culture = new CultureInfo("en-US");

            UpdateCurrentLanguage();
        }

        private void UpdateCurrentLanguage()
        {
            buttonConnect.Text = resourceManager.GetString("connect_text", Core.Settings.Culture);
            buttonUpdateList.Text = resourceManager.GetString("button_update_text", Core.Settings.Culture);
            buttonShowControlPanel.Text = resourceManager.GetString("button_show_control_panel_text", Core.Settings.Culture);
            buttonAbortExecution.Text = resourceManager.GetString("button_abort_execution_text", Core.Settings.Culture);
            buttonStartDemo.Text = resourceManager.GetString("button_start_demo_text", Core.Settings.Culture);
        }

        private void InitializeAll()
        {
            Logger.OnNewMessageAdded += logView.AddMessage;
            
            Core.PackHandler.SteppersStatesReceived += SteppersStatesReceived;
            Core.PackHandler.MessageReceived += MessageReceived;
            Core.PackHandler.CommandStateResponseReceived += OkResponseReceived;
            Core.PackHandler.SensorsValuesReceived += SensorsValuesReceived;
            Core.PackHandler.BarCodeReceived += BarCodeReceived;
            
            Core.CNCExecutor.CommandExecuted += _cncExecutor_CommandExecuted;

            editBaudrate.SelectedIndex = editBaudrate.Items.Count - 1;

            buttonConnect.Enabled = false;
        }

        private void UpdateBarCode(string barCode)
        {
            Core.UpdateBarCode(barCode);
            //Logger.AddMessage($"Принят код :{barCode}");
        }

        private void BarCodeReceived(string message)
        {
            Action<string> action = UpdateBarCode;
            BeginInvoke(action, message);
        }

        private void _cncExecutor_CommandExecuted(int executedCommandNumber)
        {
            //Action<int> action = cncView.UpdateExecutionProgress;
            //Invoke(action, executedCommandNumber);
        }

        private void SensorsValuesReceived(ushort[] values)
        {
            Action<ushort[]> action = UpdateSensorsValues;
            BeginInvoke(action, values);
        }

        private void OkResponseReceived(uint commandId, Protocol.CommandStates state)
        {
            Action<uint, Protocol.CommandStates> action = HandleOkResponse;
            BeginInvoke(action, commandId, state);
        }

        private void MessageReceived(string message)
        {
            Action<String> action = ShowReceivedMessage;
            BeginInvoke(action, message);
        }

        private void SteppersStatesReceived(ushort[] states)
        {
            Action<ushort[]> action = UpdateSteppersState;
            BeginInvoke(action, states);
        }

        private void buttonShowControlPanel_Click(object sender, EventArgs e)
        {
            controlPanel = new ControlPanelForm();
            controlPanel.StartPosition = FormStartPosition.CenterScreen;
            controlPanel.Show();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (Core.Serial.IsConnected())
            {
                Core.Serial.Disconnect();

                steppersGridView.StopUpdate();
                sensorsView.StopUpdate();

                buttonConnect.Text = resourceManager.GetString("connect_text", culture);
                //buttonConnect.Text = "Подключиться";
                connectionState.Text = "Ожидание соединения";

                rescanOpenPorts();
            }
            else
            {
                string portName = portsListBox.SelectedItem.ToString();
                int baudrate = int.Parse(editBaudrate.SelectedItem.ToString());

                bool isOK = Core.Serial.OpenConnection(portName, baudrate);

                if (isOK)
                {
                    connectionState.Text = "Установленно соединение с " + portName;
                    Logger.AddMessage(
                        "Открытие подключения - подключение к " + portName + " открыто");
                    buttonConnect.Text = resourceManager.GetString("disconnect_text", culture);
                    //buttonConnect.Text = "Отключение";

                    steppersGridView.StartUpdate();
                    sensorsView.StartUpdate();
                }
                else
                {
                    Logger.AddMessage(
                        "Открытие подключения - Ошибка при подключении!");
                    connectionState.Text = "Ожидание соединения";
                }
            }

            UpdateState();
        }

        private void HandleOkResponse(uint commandId, Protocol.CommandStates state)
        {
            //Logger.AddMessage("OK + " + commandId);

            Core.CNCExecutor.ChangeSuccesCommandId(commandId, state);
        }

        private void UpdateSteppersState(ushort[] states)
        {
            steppersGridView.UpdateSteppersStatus(states);
        }

        private void UpdateSensorsValues(ushort[] values)
        {
            sensorsView.UpdateSensorsValues(values);
            Core.UpdateSensorsValues(values);
        }

        private void ShowReceivedMessage(string message)
        {
            Logger.AddMessage(message);
        }

        private void buttonUpdateList_Click(object sender, EventArgs e)
        {
            rescanOpenPorts();
        }

        private void rescanOpenPorts()
        {
            portsListBox.Items.Clear();

            List<string> portsNames = new List<string>();

            bool isOpen = Core.Serial.GetOpenPorts(ref portsNames);

            portsListBox.Items.AddRange(portsNames.ToArray());

            if (isOpen == true)
            {
                Logger.AddMessage(
                    resourceManager.GetString("ports_found", culture));
                buttonConnect.Enabled = true;
                portsListBox.SelectedIndex = 0;
            }
            else
            {
                Logger.AddMessage(
                    resourceManager.GetString("no_ports_found", culture));
                buttonConnect.Enabled = false;
                portsListBox.SelectedText = "";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeAll();

            steppersGridView.UpdateInformation();
            devicesControlView.UpdateInformation();
            sensorsView.UpdateInformation();

            demoExecutorView.StartUpdate();

            Core.InitSensorsValues();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Сохранить настройки при выходе из программы?",
                "Сохранение настроек при выходе из программы",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

            if(dialogResult == DialogResult.Yes)
            {
                Core.GetConfig().SaveToFile("config.xml");
            }

            demoExecutorView.StopUpdate();

            if (Core.Serial.IsConnected())
            {
                Core.Serial.Disconnect();

                steppersGridView.StopUpdate();
                sensorsView.StopUpdate();
            }
        }

        private void buttonSaveConfig_Click(object sender, EventArgs e)
        {
            Core.GetConfig().SaveToFile("config.xml");
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
