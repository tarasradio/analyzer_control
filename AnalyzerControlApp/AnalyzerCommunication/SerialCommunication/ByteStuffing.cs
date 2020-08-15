using System.Collections.Generic;

namespace AnalyzerCommunication.SerialCommunication
{
    public static class ByteStuffing
    {
        public const byte EscSymbol = 0x7D;
        public const byte FlagSymbol = 0xDD;

        public static byte[] WrapPacket(byte[] packet)
        {
            List<byte> result = new List<byte>();

            foreach (byte symbol in packet)
            {
                if ((symbol == EscSymbol) || (symbol == FlagSymbol))
                {
                    result.Add(EscSymbol);
                }
                result.Add(symbol);
            }
            result.Add(FlagSymbol);

            return result.ToArray();
        }
    }
}
