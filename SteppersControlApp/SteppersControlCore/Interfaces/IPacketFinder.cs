using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.Interfaces
{
    public interface IPacketFinder
    {
        void FindPacket(byte[] buffer);
    }
}
