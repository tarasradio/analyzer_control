using SteppersControlCore.Interfaces;
using System.Collections.Generic;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class MoveCncCommand : SteppersCncCommand, IRemoteCommand
    {
        private Dictionary<int, int> _steps;

        public MoveCncCommand(Dictionary<int, int> steppers) : base(steppers, Protocol.CncCommands.CNC_MOVE)
        {
            _steps = steppers;
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
