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
            _buffer = new byte[buffer.Length - 1];
            Array.Copy(buffer, 1, _buffer, 0, _buffer.Length - 1);
        }

        public String GetDebugMessage()
        {
            string message = Encoding.UTF8.GetString(_buffer, 0, _buffer.Length);
            return message;
        }
    }
}
