using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.SerialCommunication
{
    public class ByteStuffing
    {
        public const byte EscSymbol = 0x7D;
        public const byte FlagSymbol = 0xDD;

        public static byte[] WrapPacket(byte [] packet)
        {
            List<byte> _result = new List<byte>();

            foreach (byte symbol in packet)
            {
                if ( (symbol == EscSymbol) || (symbol == FlagSymbol) )
                {
                    _result.Add(EscSymbol);
                }
                _result.Add(symbol);
            }
            _result.Add(FlagSymbol);

            return _result.ToArray();
        }
    }
}
