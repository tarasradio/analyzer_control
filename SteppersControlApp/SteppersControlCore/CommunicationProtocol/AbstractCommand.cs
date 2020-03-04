using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.Interfaces;

namespace SteppersControlCore.CommunicationProtocol
{
    public class AbstractCommand : ICommand
    {
        protected uint commandId = 0;

        public AbstractCommand()
        {
            commandId = Protocol.GetPacketId();
        }

        public uint GetId()
        {
            return commandId;
        }
    }
}
