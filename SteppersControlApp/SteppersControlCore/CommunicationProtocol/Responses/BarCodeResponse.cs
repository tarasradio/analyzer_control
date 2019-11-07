using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.Responses
{
    public class BarCodeResponse : AbstaractResponse
    {
        public BarCodeResponse(byte[] buffer) : base (buffer)
        {

        }

        public String GetDebugMessage()
        {
            string message = Encoding.UTF8.GetString(_buffer, 0, _buffer.Length);
            return message;
        }
    }
}
