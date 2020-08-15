using System.Text;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class FirmwareVersionResponse : ResponseBase
    {
        public FirmwareVersionResponse(byte[] buffer) : base(buffer) { }

        public string GetFirmwareVersion()
        {
            string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return message;
        }
    }
}
