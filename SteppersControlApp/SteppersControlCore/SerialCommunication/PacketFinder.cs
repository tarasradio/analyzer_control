using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.Interfaces;

namespace SteppersControlCore.SerialCommunication
{
    public delegate void PackageReceivedDelegate(byte[] packet);

    public class PacketFinder : IPacketFinder
    {
        public event PackageReceivedDelegate PacketReceived;
        
        private const uint maxPacketLength = 128;

        private static byte[] _packetBuffer = new byte[maxPacketLength];
        private static uint packetTail = 0;
        
        static bool escapeFlag = false;
        
        public PacketFinder()
        {

        }

        private void tryPacketBuild(byte bufferByte)
        {
            _packetBuffer[packetTail++] = bufferByte;

            if (packetTail == maxPacketLength) {
                Logger.Info($"[Packet finder] - Превышен размер пакета.");
                packetTail = 0;
            }
            escapeFlag = false;
        }

        public void FindPacket(byte[] buffer)
        {
            int currentBufferByte = 0;
            
            while (currentBufferByte < buffer.Length)
            {
                if(ByteStuffing.FlagSymbol == buffer[currentBufferByte] )
                {
                    if(escapeFlag) {
                        tryPacketBuild(buffer[currentBufferByte]);
                    } else {
                        byte[] recvPacket = new byte[packetTail];
                        Array.Copy(_packetBuffer, recvPacket, packetTail);
                        PacketReceived(recvPacket);
                        packetTail = 0;
                    }
                }
                else if(ByteStuffing.EscSymbol == buffer[currentBufferByte])
                {
                    if (escapeFlag) {
                        tryPacketBuild(buffer[currentBufferByte]);
                    } else {
                        escapeFlag = true;
                    }
                }
                else {
                    tryPacketBuild(buffer[currentBufferByte]);
                }
                currentBufferByte++;
            }
        }
    }
}
