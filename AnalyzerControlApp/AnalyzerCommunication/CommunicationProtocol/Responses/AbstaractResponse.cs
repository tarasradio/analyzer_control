using System;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class AbstaractResponse
    {
        protected byte[] buffer;

        public AbstaractResponse(byte[] buffer)
        {
            this.buffer = new byte[buffer.Length - 1];
            Array.Copy(buffer, 1, this.buffer, 0, buffer.Length - 1);
        }
    }
}
