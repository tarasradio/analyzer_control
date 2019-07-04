using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class EmmergencyStopCommand
    {
        public EmmergencyStopCommand()
        {

        }

        public byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public uint GetId()
        {
            return 0;
        }
    }
}
