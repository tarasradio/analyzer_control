using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.Responses
{
    class FirmwareVersionResponse : AbstaractResponse
    {
        public FirmwareVersionResponse(byte[] buffer) : base(buffer)
        {

        }

        public string GetFirmwareVersion()
        {
            string message = Encoding.UTF8.GetString(_buffer, 0, _buffer.Length);
            return message;
        }
    }
}
