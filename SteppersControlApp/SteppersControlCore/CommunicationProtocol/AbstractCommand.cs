using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol
{
    public class AbstractCommand : IAbstractCommand
    {
        protected uint _commandId = 0;

        public AbstractCommand()
        {
            _commandId = Protocol.GetPacketId();
        }

        public uint GetId()
        {
            return _commandId;
        }
    }
}
