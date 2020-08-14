using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;

namespace AnalyzerCommunication.SerialCommunication
{
    public class PacketFinder : IPacketFinder
    {
        public delegate void PackageReceivedDelegate(byte[] packet);
        public event PackageReceivedDelegate PacketReceived;

        private const uint maxPacketLength = 128;

        private static byte[] packetBuffer = new byte[maxPacketLength];
        private static uint packetTail = 0;

        static bool escapeFlag = false;

        public PacketFinder()
        {

        }

        public void FindPacket(byte[] buffer)
        {
            int currentBufferByte = 0;

            while (currentBufferByte < buffer.Length)
            {
                if (ByteStuffing.FlagSymbol == buffer[currentBufferByte])
                {
                    if (escapeFlag)
                    {
                        TryPacketBuild(buffer[currentBufferByte]);
                    }
                    else
                    {
                        byte[] recvPacket = new byte[packetTail];
                        Array.Copy(packetBuffer, recvPacket, packetTail);
                        PacketReceived(recvPacket);
                        packetTail = 0;
                    }
                }
                else if (ByteStuffing.EscSymbol == buffer[currentBufferByte])
                {
                    if (escapeFlag)
                    {
                        TryPacketBuild(buffer[currentBufferByte]);
                    }
                    else
                    {
                        escapeFlag = true;
                    }
                }
                else
                {
                    TryPacketBuild(buffer[currentBufferByte]);
                }
                currentBufferByte++;
            }
        }

        private void TryPacketBuild(byte bufferByte)
        {
            packetBuffer[packetTail++] = bufferByte;

            if (packetTail == maxPacketLength)
            {
                Logger.Info($"[Packet finder] - Превышен размер пакета.");
                packetTail = 0;
            }
            escapeFlag = false;
        }
    }
}
