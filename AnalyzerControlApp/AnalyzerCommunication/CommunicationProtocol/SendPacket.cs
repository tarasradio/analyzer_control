using System;

namespace AnalyzerCommunication.CommunicationProtocol
{
    public class SendPacket
    {
        private byte[] buffer;
        const uint idLength = 4;

        public SendPacket(int dataLength)
        {
            buffer = new byte[dataLength + idLength];
        }

        public void SetPacketId(uint packetId)
        {
            byte[] packetIdBytes = BitConverter.GetBytes(packetId);
            Array.Copy(packetIdBytes, 0, buffer, 0, idLength);
        }

        public void SetData(int bytePosition, byte data)
        {
            buffer[idLength + bytePosition] = data;
        }

        public void SetData(int bytePosition, byte[] data)
        {
            Array.Copy(data, 0, buffer, idLength + bytePosition, data.Length);
        }

        public byte[] GetBytes()
        {
            return buffer;
        }
    }
}
