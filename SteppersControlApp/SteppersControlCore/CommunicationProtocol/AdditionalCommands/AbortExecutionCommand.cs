using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class AbortExecutionCommand : AbstractCommand, ICommand
    {
        public AbortExecutionCommand(uint packetId) : base(packetId, Protocol.CommandType.SIMPLE_COMMAND)
        {

        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(1);
            packet.SetPacketId(PacketId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.ABORT_EXECUTION);

            return packet.GetBytes();
        }
    }
}
