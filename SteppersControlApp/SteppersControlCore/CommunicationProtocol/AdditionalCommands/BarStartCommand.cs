using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class BarStartCommand : AbstractCommand, IDeviceCommand
    {
        public BarStartCommand(uint packetId) : base(packetId, Protocol.CommandTypes.SIMPLE_COMMAND)
        {

        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(1);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.BAR_START);

            return packet.GetBytes();
        }
    }
}
