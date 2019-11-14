using SteppersControlCore.CommunicationProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.Interfaces
{
    public interface IRemoteCommand : ICommand
    {
        Protocol.CommandTypes GetType();
        byte[] GetBytes();
    }
}
