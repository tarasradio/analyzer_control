namespace AnalyzerCommunication.SerialCommunication
{
    public interface IPacketFinder
    {
        void FindPacket(byte[] buffer);
    }
}
