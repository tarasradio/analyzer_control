using Infrastructure;
using System;

namespace AnalyzerCommunication.SerialCommunication
{
    public class PacketFinder : IPacketFinder
    {
        private IPacketHandler packetHandler;

        private const uint maxPacketLength = 1024;

        private static byte[] packetBuffer = new byte[maxPacketLength];
        private static uint packetTail = 0;

        static bool escapeFlag = false;

        public PacketFinder(IPacketHandler handler)
        {
            this.packetHandler = handler;
        }

        public void FindPacket(byte[] buffer)
        {
            int currentBufferByte = 0;

            while (currentBufferByte < buffer.Length)
            {
                if (ByteStuffing.FlagSymbol == buffer[currentBufferByte])
                {
                    if (escapeFlag) {
                        TryPacketBuild(buffer[currentBufferByte]);
                    } else {
                        byte[] recvPacket = new byte[packetTail];
                        Array.Copy(packetBuffer, recvPacket, packetTail);

                        packetHandler.ProcessPacket(recvPacket);

                        packetTail = 0;
                    }
                }
                else if (ByteStuffing.EscSymbol == buffer[currentBufferByte])
                {
                    if (escapeFlag) {
                        TryPacketBuild(buffer[currentBufferByte]);
                    } else {
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
                Logger.Debug($"[{nameof(PacketFinder)}] - Превышен размер пакета.");
                packetTail = 0;
            }
            escapeFlag = false;
        }
    }
}
