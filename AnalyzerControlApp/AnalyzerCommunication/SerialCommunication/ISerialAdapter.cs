using System;

namespace AnalyzerCommunication.SerialCommunication
{
    public interface ISerialAdapter
    {
        string PortName { get; set; }

        event Action<bool> ConnectionChanged;

        bool Open(string portName, int baudrate);
        void Close();
        bool IsOpen();
        void SendPacket(byte[] packet);
        string[] GetAvailablePorts();
    }
}
