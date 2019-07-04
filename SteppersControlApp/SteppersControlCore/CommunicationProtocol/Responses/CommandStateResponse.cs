using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.Responses
{
    public class CommandStateResponse
    {
        byte[] _buffer;
        
        public CommandStateResponse(byte[] buffer)
        {
            _buffer = new byte[buffer.Length - 1];
            Array.Copy(buffer, 1, _buffer, 0, _buffer.Length - 1);
        }

        public uint GetCommandId()
        {
            uint id = BitConverter.ToUInt32(_buffer, 1);
            return id;
        }

        public Protocol.CommandStates GetCommandState()
        {
            return (Protocol.CommandStates)_buffer[0];
        }
    }
}
