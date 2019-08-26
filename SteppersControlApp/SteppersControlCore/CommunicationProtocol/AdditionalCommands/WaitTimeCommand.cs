using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class WaitTimeCommand : AbstractCommand, IHostCommand
    {
        uint _timePeriodMs = 0;

        public WaitTimeCommand(uint timePeriodMs, uint packetId) : base(packetId, Protocol.CommandTypes.HOST_COMMAND)
        {
            _timePeriodMs = timePeriodMs;
        }

        public void Execute()
        {
            Thread.Sleep((int)_timePeriodMs);
        }
    }
}
