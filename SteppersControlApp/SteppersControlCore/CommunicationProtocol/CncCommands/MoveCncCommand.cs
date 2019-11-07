using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class MoveCncCommand : SteppersCncCommand, IDeviceCommand
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
