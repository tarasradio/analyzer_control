using AnalyzerCommunication.CommunicationProtocol.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerCommunication.SerialCommunication
{
    public interface IPacketHandler
    {
        void ProcessPacket(byte[] packet);
        void AddResponseHandler(byte responseCode, Action<byte[]> handler);
    }
}
