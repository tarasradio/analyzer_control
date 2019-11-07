using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class HomeCncCommand : SteppersCncCommand, IDeviceCommand
    {
        public HomeCncCommand(Dictionary<int, int> steppers) : base(steppers, Protocol.CncCommands.CNC_HOME)
        {

        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
