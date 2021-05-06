using System;
using System.IO.Ports;
using Infrastructure;

namespace AnalyzerCommunication.SerialCommunication
{
    public class SerialAdapter : ISerialAdapter
    {
        private IPacketFinder _packetFinder = null;
        private SerialPort _serialPort = null;

        public event Action<bool> ConnectionChanged;

        public string PortName
        {
            get => _serialPort.PortName;
            set => _serialPort.PortName = value;
        }

        public string[] GetAvailablePorts() => SerialPort.GetPortNames();
        public bool IsOpen() => _serialPort.IsOpen;

        public SerialAdapter(IPacketFinder packetFinder)
        {
            _packetFinder = packetFinder;
            _serialPort = new SerialPort();
        }

        public bool Open(string portName, int baudrate)
        {
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudrate;
            _serialPort.DataBits = 8;
            _serialPort.DataReceived += onDataReceived;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.NewLine = "\r";

            try {
                _serialPort.Open();
                ConnectionChanged?.Invoke(true);
            } catch (Exception ex) {
                Logger.Info($"[{nameof(SerialAdapter)}] - Ошибка при открытии порта { portName }. {ex.Message}");
            }

            return _serialPort.IsOpen;
        }

        public void Close()
        {
            try {
                _serialPort.Close();
                ConnectionChanged?.Invoke(false);
            } catch (Exception ex) {
                Logger.Info($"[{nameof(SerialAdapter)}] - Ошибка при закрытии порта {_serialPort.PortName}. {ex.Message}");
            }
        }

        private void onDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[_serialPort.BytesToRead];
            _serialPort.Read(buffer, 0, buffer.Length);

            _packetFinder.FindPacket(buffer);
        }

        public void SendPacket(byte[] packet)
        {
            byte[] wrappedPacket = ByteStuffing.WrapPacket(packet);

            SendBytes(wrappedPacket);
        }

        private void SendBytes(byte[] bytes)
        {
            try {
                _serialPort.Write(bytes, 0, bytes.Length);
            } catch (Exception ex) {
                Logger.Info($"[{nameof(SerialAdapter)}] - Ошибка записи в порт. {ex.Message}");
            }
        }
    }
}
