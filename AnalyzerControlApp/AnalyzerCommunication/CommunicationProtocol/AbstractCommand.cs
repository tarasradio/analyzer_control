using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerCommunication.CommunicationProtocol
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
