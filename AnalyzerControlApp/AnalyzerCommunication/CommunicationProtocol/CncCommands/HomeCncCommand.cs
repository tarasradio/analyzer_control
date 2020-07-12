using System.Collections.Generic;

namespace AnalyzerCommunication.CommunicationProtocol.CncCommands
{
    public class HomeCncCommand : SteppersCncCommand, IRemoteCommand
    {
        public HomeCncCommand(Dictionary<int, int> steppers) : base(steppers, Protocol.CncCommands.CNC_HOME) { }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
