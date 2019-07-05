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

namespace SteppersControlApp
{
    public partial class MainForm : Form
    {
        Core _core;

        Logger _logger;
        SerialHelper _helper;

        PackageReceiver _packageReceiver;
        PackageHandler _packageHandler;
        CncExecutor _cncExecutor;
        
        ControlPanelForm controlPanel = new ControlPanelForm();

        string Configurationfilename = "WTFTOOL.txt";

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

            _core = new Core();
            
            if(!_core.getConfig().LoadFromFile(Configurationfilename))
            {
                MessageBox.Show("Файл настроек не найден!");
                Close();
            }

            InitializeAll();

            steppersGridView.SetConfiguration(_core.getConfig());
            steppersGridView.UpdateInformation();

            devicesControlView.SetConfiguration(_core.getConfig());
            devicesControlView.UpdateInformation();
        }

        private void InitializeAll()
        {
            _logger = _core.GetLogger();
            _logger.OnNewMessageAdded += logView.AddMessage;

            _packageReceiver = new PackageReceiver(Protocol.PacketHeader, Protocol.PacketEnd, _logger);
            _packageHandler = new PackageHandler(_logger);

            _packageReceiver.PackageReceived += _packageHandler.ProcessPacket;

            _packageHandler.SteppersStatesReceived += SteppersStatesReceived;
            _packageHandler.MessageReceived += MessageReceived;
            _packageHandler.CommandStateResponseReceived += OkResponseReceived;

            _helper = new SerialHelper(_logger, _packageReceiver);

            _cncExecutor = new CncExecutor(_logger, _helper);

            cncView.SetLogger(_logger);
            cncView.SetHelper(_helper);
            cncView.SetControlThread(_cncExecutor);

            editBaudrate.SelectedIndex = 0;
        }

        private void OkResponseReceived(uint commandId, Protocol.CommandStates state)
        {
            Action<uint, Protocol.CommandStates> action = HandleOkResponse;
            Invoke(action, commandId, state);
        }

        private void MessageReceived(string message)
        {
            Action<String> action = ShowReceivedMessage;
            Invoke(action, message);
        }

        private void SteppersStatesReceived(ushort[] states)
        {
            Action<ushort[]> action = UpdateSteppersState;
            Invoke(action, states);
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
                    _logger.AddMessage(
                        "Открытие подключения - Подключение к " + portName + " открыто");
                    buttonConnect.Text = "Отключение";

                    steppersGridView.StartUpdate();
                }
                else
                {
                    _logger.AddMessage(
                        "Открытие подключения - Ошибка при подключении!");
                    connectionState.Text = "Ожидание соединения";
                }
            }
            
            UpdateState();
        }

        private void HandleOkResponse(uint commandId, Protocol.CommandStates state)
        {
            _logger.AddMessage("OK + " + commandId);

            _cncExecutor.ChangeSuccesCommandId(commandId, state);
        }

        private void UpdateSteppersState(ushort[] states)
        {
            steppersGridView.UpdateSteppersStatus(states);
        }

        private void ShowReceivedMessage(string message)
        {
            _logger.AddMessage(message);
        }
        
        private void buttonUpdateList_Click(object sender, EventArgs e)
        {
            rescanOpenPorts();
        }

        private void rescanOpenPorts()
        {
            portsListBox.Items.Clear();

            List<string> portsNames = new List<string>();

            bool isOpen = _helper.getOpenPorts(ref portsNames);

            portsListBox.Items.AddRange(portsNames.ToArray());

            if (isOpen == true)
            {
                _logger.AddMessage(
                    "Поиск портов - Найдены открытые порты");
                buttonConnect.Enabled = true;
                portsListBox.SelectedIndex = 0;
            }
            else
            {
                _logger.AddMessage(
                    "Поиск портов - Открытых портов не найдено");
                buttonConnect.Enabled = false;
                portsListBox.SelectedText = "";
            }
        }

        private void DeviceButton_CheckedChanged(object sender, EventArgs e)
        {
            if( ((CheckBox)sender).Checked )
            {
                ((CheckBox)sender).BackColor = Color.GreenYellow;
            }
            else
            {
                ((CheckBox)sender).BackColor = Color.White;
            }

            if( ((CheckBox) sender).Name == buttonUnit1.Name)
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

            _helper.SendBytes(new SetDeviceStateCommand(device, state, packetId).GetBytes());
        }

        private void cncView_Load(object sender, EventArgs e)
        {

        }
    }
}
