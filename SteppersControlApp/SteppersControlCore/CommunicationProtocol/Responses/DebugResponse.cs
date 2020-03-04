using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.Responses
{
    public class DebugResponse : AbstaractResponse
    {
        public DebugResponse(byte[] buffer) : base (buffer)
        {

        }

        public string GetDebugMessage()
        {
            string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return message;
        }
    }
}
