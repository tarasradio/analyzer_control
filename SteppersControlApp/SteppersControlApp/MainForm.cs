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
using System.Threading;

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
                connectionState.Text = "Подключен";
                connectionState.ForeColor = Color.DarkGreen;
            }
            else
            {
                buttonConnect.Text = "Подключиться";
                connectionState.Text = "Не подключен";
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

            devicesControlView.SetSerialHelper(_helper);
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
                        "Открытие подключения - подключение к " + portName + " открыто");
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
                    "Поиск портов - найдены открытые порты");
                buttonConnect.Enabled = true;
                portsListBox.SelectedIndex = 0;
            }
            else
            {
                Logger.AddMessage(
                    "Поиск портов - открытых портов не найдено");
                buttonConnect.Enabled = false;
                portsListBox.SelectedText = "";
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _core = new Core();

            if (!Core.GetConfig().LoadFromFile(ConfigurationFilename))
            {
                MessageBox.Show("Ошибка при открытии файла конфигурации!");
                Close(); Dispose(); return;
            }
            else
            {
                InitializeAll();
                
                steppersGridView.UpdateInformation();
                devicesControlView.UpdateInformation();
                sensorsView.UpdateInformation();
                
                _core.InitSensorsValues();
            }

            Thread thread = new Thread(LogStuffThread);
            //thread.IsBackground = true;
            //thread.Start();
        }

        private void LogStuffThread()
        {
            while (true)
            {
                Logger.AddMessage("Hello");
                Thread.Sleep(2);
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
