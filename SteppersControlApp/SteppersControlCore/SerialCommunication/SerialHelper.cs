using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace SteppersControlCore.SerialCommunication
{
    public class SerialHelper
    {
        Logger _logger;
        PackageReceiver _packageReceiver;

        SerialPort _serialPort;

        public SerialHelper(Logger logger, PackageReceiver packageReceiver)
        {
            _logger = logger;
            _packageReceiver = packageReceiver;
            _serialPort = new SerialPort();
            _serialPort.DataReceived += Port_DataReceived;
        }

        public bool OpenConnection(string portName, int baudrate)
        {
            bool isOK = false;

            _serialPort = new SerialPort(portName);
            _serialPort.BaudRate = baudrate;
            _serialPort.DataBits = 8;
            _serialPort.DataReceived += Port_DataReceived;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.NewLine = "\r";

            try
            {
                _serialPort.Open();
            }
            catch (UnauthorizedAccessException)
            {
                Logger.AddMessage($"Ошибка при открытии порта {portName}");
            }


            if (_serialPort.IsOpen)
            {
                isOK = true;
            }

            return isOK;
        }

        public void Disconnect()
        {
            _serialPort.Close();
        }

        public void CloseConnection()
        {
            _serialPort.Close();
        }

        public bool IsConnected()
        {
            return _serialPort.IsOpen;
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[_serialPort.BytesToRead];
            _serialPort.Read(buffer, 0, buffer.Length);
            _packageReceiver.HandleData(buffer);
        }

        public void SendBytes(byte[] bytes)
        {
            try
            {
                _serialPort.Write(bytes, 0, bytes.Length);

                //while (_serialPort.BytesToWrite != 0) ;
            }
            catch (Exception)
            {
                Logger.AddMessage("Ошибка при попытке записи в порт");
            }
        }

        public static string[] getPortsList()
        {
            return SerialPort.GetPortNames();
        }

        public bool getOpenPorts(ref List<string> ports)
        {
            List<string> Ports = new List<string>();
            bool available = false;

            foreach (string str in SerialPort.GetPortNames())
            {
                try
                {
                    ports.Add(str);
                    available = true;
                }
                catch (Exception)
                {
                    Logger.AddMessage("Ошибка при сканировании!");
                }
            }

            return available;
        }
    }
}
