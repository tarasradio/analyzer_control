using System;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class ResponseBase
    {
        protected byte[] buffer;

        public ResponseBase(byte[] buffer)
        {
            this.buffer = new byte[buffer.Length - 1];
            Array.Copy(buffer, 1, this.buffer, 0, buffer.Length - 1);
        }
    }
}
