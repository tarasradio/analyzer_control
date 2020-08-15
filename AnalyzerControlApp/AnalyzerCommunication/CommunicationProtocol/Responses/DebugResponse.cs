using System.Text;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class DebugResponse : ResponseBase
    {
        public DebugResponse(byte[] buffer) : base(buffer) { }

        public string GetDebugMessage()
        {
            string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return message;
        }
    }
}
