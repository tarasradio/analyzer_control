using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;

namespace SteppersControlCore.MachineControl
{
    public class CncProgram
    {
        public List<IAbstractCommand> Commands { get; set; }

        public CncProgram()
        {
            Commands = new List<IAbstractCommand>();
        }
    }
}
