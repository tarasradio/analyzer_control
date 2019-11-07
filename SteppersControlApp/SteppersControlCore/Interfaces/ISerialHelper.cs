using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.Interfaces
{
    public interface ISerialHelper
    {
        string[] GetAvailablePorts();
        bool OpenConnection(string portName, int baudrate);
    }
}
