using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SteppersControlCore.CommunicationProtocol
{
    public interface ICommand
    {
        Protocol.CommandType GetType();
        byte[] GetBytes();
        uint GetId();
    }
}
