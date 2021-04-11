using AnalyzerControlCore;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationWinForms.Forms
{
    public partial class ConnectionSettingsWindow : Form
    {
        public ConnectionSettingsWindow()
        {
            InitializeComponent();

            updateControlsState();
            updateFromSavedPreferences();
            rescanOpenPorts();
        }

        private void buttonUpdatePorts_Click(object sender, EventArgs e)
        {
            rescanOpenPorts();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (AnalyzerGateway.Serial.IsOpen()) {
                AnalyzerGateway.Serial.Close();
                // Тут останавливалось обновление таблиц

                rescanOpenPorts();
            } else {
                string portName = selectPort.SelectedItem.ToString();
                int baudrate = int.Parse(selectBaudrate.SelectedItem.ToString());

                if (AnalyzerGateway.Serial.Open(portName, baudrate)) {
                    Logger.Info("Открытие подключения - подключение к " + portName + " открыто");

                    // Тут запускалось обновление таблиц
                } else {
                    Logger.Info("Открытие подключения - Ошибка при подключении!");
                }
            }

            updateControlsState();
        }

        private void rescanOpenPorts()
        {
            selectPort.Items.Clear();

            String[] portsNames = AnalyzerGateway.Serial.GetAvailablePorts();

            if (portsNames.Length != 0)
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
            selectBaudrate.SelectedIndex = selectBaudrate.Items.Count - 1;
        }

        private void updateControlsState()
        {
            UpdateStatusState();

            buttonUpdatePorts.Visible = !AnalyzerGateway.Serial.IsOpen();
            selectPort.Enabled = !AnalyzerGateway.Serial.IsOpen();
            selectBaudrate.Enabled = !AnalyzerGateway.Serial.IsOpen();
        }

        private void UpdateStatusState()
        {
            if (AnalyzerGateway.Serial.IsOpen())
            {
                buttonConnect.Text = "Отключение";
                connectionStatus.Text = $"Установленно соединение с { AnalyzerGateway.Serial.PortName }.";
                connectionStatus.ForeColor = Color.DarkGreen;
            }
            else
            {
                buttonConnect.Text = "Подключение";
                connectionStatus.Text = "Соединение не установлено.";
                connectionStatus.ForeColor = Color.Brown;
            }
        }

        private void buttonSavePreferences_Click(object sender, EventArgs e)
        {
            // Здесь нужно выполнить сохранение выбранных настроек подключения
            string portName = selectPort.SelectedItem.ToString();
            int baudrate = int.Parse(selectBaudrate.SelectedItem.ToString());

            AnalyzerGateway.AppConfig.PortName = portName;
            AnalyzerGateway.AppConfig.Baudrate = (uint)baudrate;

            AnalyzerGateway.SaveAppConfiguration();

            updateFromSavedPreferences();
        }

        private void updateFromSavedPreferences()
        {
            savedPort.Text = AnalyzerGateway.AppConfig.PortName;
            savedBaudrate.Text = AnalyzerGateway.AppConfig.Baudrate.ToString();
        }
    }
}
