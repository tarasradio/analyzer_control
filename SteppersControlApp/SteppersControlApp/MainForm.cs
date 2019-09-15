using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteppersControlCore;
using SteppersControlCore.MachineControl;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.AdditionalCommands;
using SteppersControlCore.SerialCommunication;
using System.Diagnostics;

namespace SteppersControlApp
{
    public partial class MainForm : Form
    {
        Core _core;
        SerialHelper _helper;

        PackageReceiver _packageReceiver;
        PackageHandler _packageHandler;
        CncExecutor _cncExecutor;

        ControlPanelForm controlPanel = new ControlPanelForm();

        string ConfigurationFilename = "settings.xml";

        private void UpdateState()
        {
            if (_helper.IsConnected())
            {
                buttonConnect.Text = "Отключиться";
                connectionState.Text = "CONNECTED";
                connectionState.ForeColor = Color.DarkGreen;
            }
            else
            {
                buttonConnect.Text = "Подключиться";
                connectionState.Text = "DISCONNECTED";
                connectionState.ForeColor = Color.Brown;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeAll()
        {
            Logger.OnNewMessageAdded += logView.AddMessage;

            _packageReceiver = new PackageReceiver(Protocol.PacketHeader, Protocol.PacketEnd);
            _packageHandler = new PackageHandler();

            _packageReceiver.PackageReceived += _packageHandler.ProcessPacket;

            _packageHandler.SteppersStatesReceived += SteppersStatesReceived;
            _packageHandler.MessageReceived += MessageReceived;
            _packageHandler.CommandStateResponseReceived += OkResponseReceived;
            _packageHandler.SensorsValuesReceived += SensorsValuesReceived;
            _packageHandler.BarCodeReceived += BarCodeReceived;

            _helper = new SerialHelper(_packageReceiver);

            _cncExecutor = new CncExecutor(_helper);
            
            cncView.SetHelper(_helper);
            cncView.SetExecutor(_cncExecutor);

            _cncExecutor.CommandExecuted += _cncExecutor_CommandExecuted;

            editBaudrate.SelectedIndex = editBaudrate.Items.Count - 1;

            buttonConnect.Enabled = false;

            stepperTurningView.SetSerialHelper(_helper);
        }

        private void UpdateBarCode(string barCode)
        {
            _core.UpdateBarCode(barCode);
            Logger.AddMessage($"Принят код :{barCode}");
        }

        private void BarCodeReceived(string message)
        {
            Action<string> action = UpdateBarCode;
            BeginInvoke(action, message);
        }

        private void _cncExecutor_CommandExecuted(int executedCommandNumber)
        {
            Action<int> action = cncView.UpdateExecutionProgress;
            Invoke(action, executedCommandNumber);
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
            controlPanel.setSerialHelper(_helper);
            controlPanel.Show();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (_helper.IsConnected())
            {
                _helper.Disconnect();

                steppersGridView.StopUpdate();
                sensorsView.StopUpdate();

                buttonConnect.Text = "Подключиться";
                connectionState.Text = "Ожидание соединения";

                rescanOpenPorts();
            }
            else
            {
                string portName = portsListBox.SelectedItem.ToString();
                int baudrate = int.Parse(editBaudrate.SelectedItem.ToString());

                bool isOK = _helper.OpenConnection(portName, baudrate);

                if (isOK)
                {
                    connectionState.Text = "Установленно соединение с " + portName;
                    Logger.AddMessage(
                        "Открытие подключения - Подключение к " + portName + " открыто");
                    buttonConnect.Text = "Отключение";

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
            Logger.AddMessage("OK + " + commandId);

            _cncExecutor.ChangeSuccesCommandId(commandId, state);
        }

        private void UpdateSteppersState(ushort[] states)
        {
            steppersGridView.UpdateSteppersStatus(states);
        }

        private void UpdateSensorsValues(ushort[] values)
        {
            sensorsView.UpdateSensorsValues(values);
            _core.UpdateSensorsValues(values);
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

            bool isOpen = _helper.GetOpenPorts(ref portsNames);

            portsListBox.Items.AddRange(portsNames.ToArray());

            if (isOpen == true)
            {
                Logger.AddMessage(
                    "Поиск портов - Найдены открытые порты");
                buttonConnect.Enabled = true;
                portsListBox.SelectedIndex = 0;
            }
            else
            {
                Logger.AddMessage(
                    "Поиск портов - Открытых портов не найдено");
                buttonConnect.Enabled = false;
                portsListBox.SelectedText = "";
            }
        }

        private void DeviceButton_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                ((CheckBox)sender).BackColor = Color.GreenYellow;
            }
            else
            {
                ((CheckBox)sender).BackColor = Color.White;
            }

            if (((CheckBox)sender).Name == buttonUnit1.Name)
            {
                SetDeviceState(0, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit2.Name)
            {
                SetDeviceState(1, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit3.Name)
            {
                SetDeviceState(2, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit4.Name)
            {
                SetDeviceState(3, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit5.Name)
            {
                SetDeviceState(4, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit6.Name)
            {
                SetDeviceState(5, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit7.Name)
            {
                SetDeviceState(6, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit8.Name)
            {
                SetDeviceState(7, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit9.Name)
            {
                SetDeviceState(8, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit10.Name)
            {
                SetDeviceState(9, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit11.Name)
            {
                SetDeviceState(10, sender);
            }
            if (((CheckBox)sender).Name == buttonUnit12.Name)
            {
                SetDeviceState(11, sender);
            }
        }

        private void SetDeviceState(int device, object sender)
        {
            //_helper.SendBytes(new SetUnitStateCommand(device, ((CheckBox)sender).Checked).getBytes());
            SetDeviceStateCommand.DeviseState state = SetDeviceStateCommand.DeviseState.DEVICE_OFF;
            if (((CheckBox)sender).Checked == true)
                state = SetDeviceStateCommand.DeviseState.DEVICE_ON;

            uint packetId = Protocol.GetPacketId();

            //_helper.SendBytes(new SetDeviceStateCommand(device, state, packetId).GetBytes());
            _helper.SendPacket(new SetDeviceStateCommand(device, state, packetId).GetBytes());
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            _core = new Core();

            if (!Core.GetConfig().LoadFromFile(ConfigurationFilename))
            {
                MessageBox.Show("Ошибка при отрытии файла конфурации!");
                Close(); Dispose(); return;
            }
            else
            {
                InitializeAll();
                
                steppersGridView.UpdateInformation();
                stepperTurningView.UpdateInformation();
                devicesControlView.UpdateInformation();
                sensorsView.UpdateInformation();

                steppersGridView.StepperChanged += stepperTurningView.ChangeStepper;
                
                _core.InitSensorsValues();
            }
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
                Core.GetConfig().SaveToFile(ConfigurationFilename);
            }
        }

        private void buttonSaveConfig_Click(object sender, EventArgs e)
        {
            Core.GetConfig().SaveToFile(ConfigurationFilename);
        }
    }
}
