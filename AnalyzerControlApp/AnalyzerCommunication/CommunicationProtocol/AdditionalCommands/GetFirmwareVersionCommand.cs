namespace AnalyzerCommunication.CommunicationProtocol.AdditionalCommands
{
    public class GetFirmwareVersionCommand : AbstractCommand, IRemoteCommand
    {
        public GetFirmwareVersionCommand() : base() { }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(1);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.GET_FIRMWARE_VERSION);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
