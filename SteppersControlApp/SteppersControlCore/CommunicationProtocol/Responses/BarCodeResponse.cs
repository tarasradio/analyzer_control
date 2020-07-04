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
            string message = Encoding.UTF8.GetString(buffer, 1, buffer.Length - 1);
            return message;
        }

        public byte GetBarScannerId()
        {
            return buffer[0];
        }
    }
}
