using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace AnalyzerCommunication.SerialCommunication
{
    public class PacketHandler : IPacketHandler
    {
        private Dictionary<byte, Action<byte[]>> responsesHandlers = null;

        public PacketHandler()
        {
            responsesHandlers = new Dictionary<byte, Action<byte[]>>();
        }

        public void AddResponseHandler(byte responseCode, Action<byte[]> handler)
        {
            responsesHandlers.Add(responseCode, handler);
        }

        public void ProcessPacket(byte[] packet)
        {
            if (packet.Length > 0)
            {
                byte responseType = packet[0];

                try
                {
                    responsesHandlers[responseType].Invoke(packet);
                }
                catch(KeyNotFoundException)
                {
                    Logger.Info($"[{nameof(PacketHandler)}] - Uncknown response with code: {responseType} has been received.");
                }
            }
        }
    }
}
