using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol
{
    public class AbstractCommand
    {
        public uint PacketId;
        Protocol.CommandType _type;

        public AbstractCommand(uint packetId, Protocol.CommandType type)
        {
            PacketId = packetId;
            _type = type;
        }

        public uint GetId()
        {
            return PacketId;
        }

        public Protocol.CommandType GetType()
        {
            return _type;
        }
    }
}
