using System.Threading;

namespace AnalyzerCommunication.CommunicationProtocol.AdditionalCommands
{
    public class WaitTimeCommand : AbstractCommand, IHostCommand
    {
        private uint period = 0;

        public WaitTimeCommand(uint period) : base()
        {
            this.period = period;
        }

        public void Execute()
        {
            Thread.Sleep((int)period);
        }
    }
}
