using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class SetSpeedCncCommand : SteppersCncCommand
    {
        public SetSpeedCncCommand(Dictionary<int, int> steppers) : base(steppers, Protocol.CncCommands.CNC_SET_SPEED)
        {

        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
