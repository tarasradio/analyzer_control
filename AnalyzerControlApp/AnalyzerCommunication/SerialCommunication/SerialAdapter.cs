using System;
using System.IO.Ports;
using Infrastructure;

namespace AnalyzerCommunication.SerialCommunication
{
    public class SerialAdapter : ISerialAdapter
    {
        private IPacketFinder packetFinder = null;
        private SerialPort serialPort = null;

        public event Action<bool> ConnectionChanged;

        public string PortName
        {
            get => serialPort.PortName;
            set => serialPort.PortName = value;
        }

        public string[] GetAvailablePorts() => SerialPort.GetPortNames();
        public bool IsOpen() => serialPort.IsOpen;

        public SerialAdapter(IPacketFinder packetFinder)
        {
            this.packetFinder = packetFinder;
            serialPort = new SerialPort();
            serialPort.DataReceived += Port_DataReceived;
        }

        public bool Open(string portName, int baudrate)
        {
            serialPort = new SerialPort(portName);
            serialPort.BaudRate = baudrate;
            serialPort.DataBits = 8;
            serialPort.DataReceived += Port_DataReceived;

            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;

            serialPort.NewLine = "\r";

            try
            {
                serialPort.Open();
            }
            catch (UnauthorizedAccessException)
            {
                Logger.Info($"[{nameof(SerialAdapter)}] - Ошибка при открытии порта { portName }.");
            }

            ConnectionChanged(true);

            return serialPort.IsOpen;
        }

        public void Close()
        {
            try
            {
                serialPort.Close();
            }
            catch (System.IO.IOException)
            {
                Logger.Info($"[{nameof(SerialAdapter)}] - Ошибка при закрытии порта {serialPort.PortName}.");
            }

            ConnectionChanged(false);
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[serialPort.BytesToRead];
            serialPort.Read(buffer, 0, buffer.Length);

            packetFinder.FindPacket(buffer);
        }

        public void SendPacket(byte[] packet)
        {
            byte[] wrappedPacket = ByteStuffing.WrapPacket(packet);

            SendBytes(wrappedPacket);
        }

        private void SendBytes(byte[] bytes)
        {
            try
            {
                serialPort.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                Logger.Info($"[{nameof(SerialAdapter)}] - Ошибка записи в порт.");
            }
        }
    }
}
