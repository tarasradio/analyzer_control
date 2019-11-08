using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.Interfaces
{
    public interface ISerial
    {
        bool Open(string portName, int baudrate);
        void Close();
        bool IsOpen();
        void SendPacket(byte[] packet);
        string[] GetAvailablePorts();
    }
}
