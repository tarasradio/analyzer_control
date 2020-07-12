using System;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class SensorsValuesResponse : AbstaractResponse
    {
        public SensorsValuesResponse(byte[] buffer) : base(buffer) { }

        public UInt16[] GetStates()
        {
            if ((buffer.Length) % 2 != 0)
            {
                return null;
            }
            UInt16[] states = new UInt16[buffer.Length / 2];

            for (int i = 0; i < buffer.Length; i += 2)
            {
                UInt16 state = BitConverter.ToUInt16(buffer, i);
                states[i / 2] = state;
            }

            return states;
        }
    }
}
