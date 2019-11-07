using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.Responses
{
    public class AbstaractResponse
    {
        protected byte[] _buffer;

        public AbstaractResponse(byte[] buffer)
        {
            _buffer = new byte[buffer.Length - 1];
            Array.Copy(buffer, 1, _buffer, 0, buffer.Length - 1);
        }
    }
}
