using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.SerialCommunication
{
    public class ByteStuffing
    {
        static byte escSymbol = 0x7D;
        static byte flagSymbol = 0xDD;

        public static byte[] wrapPacket(byte [] packet)
        {
            List<byte> _result = new List<byte>();

            foreach (byte symbol in packet)
            {
                if (symbol == escSymbol || symbol == flagSymbol)
                {
                    _result.Add(escSymbol);
                }
                _result.Add(symbol);
            }

            return _result.ToArray();
        }

        public static byte[] unwrapPacket(byte[] packet)
        {
            List<byte> _result = new List<byte>();

            bool isEsc = false;

            foreach (byte symbol in packet)
            {
                if (symbol == escSymbol)
                {
                    if(isEsc) isEsc = false;
                    else
                    {
                        isEsc = true;
                        continue;
                    }
                }
                _result.Add(symbol);
            }

            return _result.ToArray();
        }
    }
}
