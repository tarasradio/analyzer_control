using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.Responses
{
    public class SensorsValuesResponse : AbstaractResponse
    {
        public SensorsValuesResponse(byte[] buffer) : base(buffer)
        {

        }

        public UInt16[] GetStates()
        {
            if ((_buffer.Length) % 2 != 0)
            {
                return null;
            }
            UInt16[] states = new UInt16[_buffer.Length / 2];

            for (int i = 0; i < _buffer.Length; i += 2)
            {
                UInt16 state = BitConverter.ToUInt16(_buffer, i);
                states[i / 2] = state;
            }

            return states;
        }
    }
}
