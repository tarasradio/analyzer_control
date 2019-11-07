using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class GetFirmwareVersionCommand : AbstractCommand, IDeviceCommand
    {
        public GetFirmwareVersionCommand() : base()
        {

        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(1);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.GET_FIRMWARE_VERSION);

            return packet.GetBytes();
        }

        Protocol.CommandTypes IDeviceCommand.GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
