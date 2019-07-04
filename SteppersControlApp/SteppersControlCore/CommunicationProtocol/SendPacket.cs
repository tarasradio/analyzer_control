using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol
{
    public class SendPacket
    {
        private byte[] _buffer;

        public SendPacket(int dataLength)
        {
            _buffer = new byte[
                Protocol.PacketHeader.Length + 
                Protocol.PacketEnd.Length + 
                dataLength + 4];
            
            Array.Copy(Protocol.PacketHeader, _buffer, Protocol.PacketHeader.Length);
            Array.Copy(Protocol.PacketEnd, 0, _buffer, Protocol.PacketHeader.Length + dataLength + 4, Protocol.PacketEnd.Length);
        }

        public void SetPacketId(uint packetId)
        {
            byte[] packetIdBytes = BitConverter.GetBytes(packetId);
            Array.Copy(packetIdBytes, 0, _buffer, _buffer.Length - Protocol.PacketEnd.Length - 4, 4);
        }

        public void SetData(int bytePosition, byte data)
        {
            _buffer[Protocol.PacketHeader.Length + bytePosition] = data;
        }

        public void SetData(int bytePosition, byte[] data)
        {
            Array.Copy(data, 0, _buffer, Protocol.PacketHeader.Length + bytePosition, data.Length);
        }

        public byte[] GetBytes()
        {
            return _buffer;
        }
    }
}
