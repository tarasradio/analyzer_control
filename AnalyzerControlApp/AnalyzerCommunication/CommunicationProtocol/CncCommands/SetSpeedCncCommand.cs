using System.Collections.Generic;

namespace AnalyzerCommunication.CommunicationProtocol.CncCommands
{
    public class SetSpeedCncCommand : SteppersCncCommand, IRemoteCommand
    {
        public SetSpeedCncCommand(Dictionary<int, int> steppers) : base(steppers, Protocol.CncCommands.CNC_SET_SPEED) { }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
