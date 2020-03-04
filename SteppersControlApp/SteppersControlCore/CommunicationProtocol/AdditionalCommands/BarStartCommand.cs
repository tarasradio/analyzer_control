using SteppersControlCore.Interfaces;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class BarStartCommand : AbstractCommand, IRemoteCommand
    {
        public BarStartCommand() : base()
        {

        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(1);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.BAR_START);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
