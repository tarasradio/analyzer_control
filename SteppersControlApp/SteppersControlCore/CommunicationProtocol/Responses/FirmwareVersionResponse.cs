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
            string message = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return message;
        }
    }
}
