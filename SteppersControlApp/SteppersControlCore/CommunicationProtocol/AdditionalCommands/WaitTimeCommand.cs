
using SteppersControlCore.Interfaces;
using System.Threading;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class WaitTimeCommand : AbstractCommand, IHostCommand
    {
        private uint timeDelay = 0;

        public WaitTimeCommand(uint period) : base()
        {
            timeDelay = period;
        }

        public void Execute()
        {
            Thread.Sleep((int)timeDelay);
        }
    }
}
