using AnalyzerCommunication.CommunicationProtocol;

namespace AnalyzerCommunication
{
    public interface IRemoteCommand : ICommand
    {
        Protocol.CommandTypes GetType();
        byte[] GetBytes();
    }
}
