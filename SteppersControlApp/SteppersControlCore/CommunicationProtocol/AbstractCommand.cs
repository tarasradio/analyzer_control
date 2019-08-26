using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol
{
    public class AbstractCommand : IAbstractCommand
    {
        public uint _commandId;
        Protocol.CommandTypes _type;

        public AbstractCommand(uint commandId, Protocol.CommandTypes type)
        {
            _commandId = commandId;
            _type = type;
        }

        public uint GetId()
        {
            return _commandId;
        }

        public Protocol.CommandTypes GetType()
        {
            return _type;
        }
    }
}
