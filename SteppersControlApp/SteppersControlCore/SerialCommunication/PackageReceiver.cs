using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.SerialCommunication
{
    public delegate void PackageReceivedDelegate(byte[] packet);

    public class PackageReceiver
    {
        public event PackageReceivedDelegate PackageReceived;
        
        enum ReceiveState
        {
            RECEIVING_HEADER,
            RECEIVING_BODY
        };

        private ReceiveState _state;
        private int _currentHeaderByte;
        private byte[] _packetHeader;

        private int _currentEndByte;
        private byte[] _packetEnd;

        private List<byte> _receivedQueue;

        public PackageReceiver(byte[] header, byte[] end)
        {
            _packetHeader = new byte[header.Length];
            Array.Copy(header, _packetHeader, header.Length);

            _packetEnd = new byte[end.Length];
            Array.Copy(end, _packetEnd, end.Length);

            _receivedQueue = new List<byte>();
            
            _currentHeaderByte = 0;
            _state = ReceiveState.RECEIVING_HEADER;
        }

        private const uint maxPacketLength = 128;

        private static byte[] _packetBuffer = new byte[maxPacketLength];
        private static uint packetTail = 0;
        
        static bool escapeFlag = false;

        public void tryPacketBuild(byte bufferByte)
        {
            _packetBuffer[packetTail++] = bufferByte;

            if (packetTail == maxPacketLength)
            {
                Logger.AddMessage($"Packet size overflow");
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
                    if(escapeFlag)
                    {
                        tryPacketBuild(buffer[currentBufferByte]);
                        //_packetBuffer[packetTail++] = buffer[currentBufferByte];

                        //if(packetTail == maxPacketLength)
                        //{
                        //    Logger.AddMessage($"Packet size overflow");
                        //    packetTail = 0;
                        //}
                        //escapeFlag = false;
                    }
                    else
                    {
                        byte[] recvPacket = new byte[packetTail];
                        Array.Copy(_packetBuffer, recvPacket, packetTail);
                        PackageReceived(recvPacket);
                        packetTail = 0;
                    }
                }
                else if(ByteStuffing.EscSymbol == buffer[currentBufferByte])
                {
                    if (escapeFlag)
                    {
                        tryPacketBuild(buffer[currentBufferByte]);
                        //_packetBuffer[packetTail++] = buffer[currentBufferByte];

                        //if (packetTail == maxPacketLength)
                        //{
                        //    Logger.AddMessage($"Packet size overflow");
                        //    packetTail = 0;
                        //}
                        //escapeFlag = false;
                    }
                    else
                    {
                        escapeFlag = true;
                    }
                }
                else
                {
                    tryPacketBuild(buffer[currentBufferByte]);
                    //_packetBuffer[packetTail++] = buffer[currentBufferByte];

                    //if (packetTail == maxPacketLength)
                    //{
                    //    Logger.AddMessage($"Packet size overflow");
                    //    packetTail = 0;
                    //}
                }
                currentBufferByte++;
            }
        }

        public void HandleData(byte[] buffer)
        {
            //_logger.AddMessage($"Buffer: {buffer.Length} bytes");

            int i = 0;
            while(i < buffer.Length)
            {
                switch(_state)
                {
                    case ReceiveState.RECEIVING_HEADER:
                        {
                            if(buffer[i] == _packetHeader[_currentHeaderByte])
                            {
                                _currentHeaderByte++;
                            }
                            else
                            {
                                _currentHeaderByte = 0;
                            }

                            if (_currentHeaderByte == _packetHeader.Length)
                            {
                                _state = ReceiveState.RECEIVING_BODY;
                                _currentEndByte = 0;
                            }
                        }
                        break;
                    case ReceiveState.RECEIVING_BODY:
                        {
                            _receivedQueue.Add(buffer[i]);

                            if(buffer[i] == _packetEnd[_currentEndByte])
                            {
                                _currentEndByte++;
                            }
                            else
                            {
                                _currentEndByte = 0;
                            }

                            if (_currentEndByte == _packetEnd.Length)
                            {
                                _currentHeaderByte = 0;
                                _state = ReceiveState.RECEIVING_HEADER;
                                _receivedQueue.RemoveRange(_receivedQueue.Count - _packetEnd.Length, _packetEnd.Length);

                                //_logger.AddMessage($"find packet: {_receivedQueue.Count} bytes");
                                PackageReceived(_receivedQueue.ToArray());

                                _receivedQueue.Clear();
                            }
                        }
                        break;
                }
                i++;
            }
        }
    }
}
