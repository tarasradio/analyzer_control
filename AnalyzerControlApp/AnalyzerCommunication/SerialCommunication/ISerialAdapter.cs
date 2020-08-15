namespace AnalyzerCommunication.SerialCommunication
{
    public interface ISerialAdapter
    {
        string PortName { get; set; }

        bool Open(string portName, int baudrate);
        void Close();
        bool IsOpen();
        void SendPacket(byte[] packet);
        string[] GetAvailablePorts();
    }
}
