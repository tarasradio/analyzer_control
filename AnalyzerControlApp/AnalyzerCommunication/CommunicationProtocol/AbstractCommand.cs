namespace AnalyzerCommunication.CommunicationProtocol
{
    public class AbstractCommand : ICommand
    {
        protected uint commandId = 0;

        public AbstractCommand()
        {
            commandId = Protocol.GetPacketId();
        }

        public uint GetId()
        {
            return commandId;
        }
    }
}
