namespace AnalyzerCommunication.CommunicationProtocol.AdditionalCommands
{
    public class AbortExecutionCommand : AbstractCommand, IRemoteCommand
    {
        public AbortExecutionCommand() : base() { }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(1);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.ABORT_EXECUTION);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
