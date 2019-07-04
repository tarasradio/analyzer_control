using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;

namespace SteppersControlCore.MachineControl
{
    public class CncFrame
    {
        public List<ICommand> Commands { get; set; }

        public CncFrame()
        {
            Commands = new List<ICommand>();
        }
    }
}
