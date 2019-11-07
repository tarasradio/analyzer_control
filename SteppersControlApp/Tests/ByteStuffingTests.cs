using SteppersControlCore;
using SteppersControlCore.SerialCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ByteStuffingTests
    {
        public void Test1()
        {
            byte[] bytes = Encoding.ASCII.GetBytes("Hello");
            byte[] packet = new byte[bytes.Length + 1];
            Array.Copy(bytes, 0, packet, 1, bytes.Length);
            packet[0] = 0x13;
            byte[] wrap = ByteStuffing.WrapPacket(packet);
            Core.PackReceiver.FindPacket(wrap);

            packet = new byte[6];

            packet[0] = 0x12;
            packet[1] = 0x01;

            bytes = BitConverter.GetBytes(123);

            Array.Copy(bytes, 0, packet, 2, bytes.Length);

            wrap = ByteStuffing.WrapPacket(packet);
            Core.PackReceiver.FindPacket(wrap);
        }
    }
}
