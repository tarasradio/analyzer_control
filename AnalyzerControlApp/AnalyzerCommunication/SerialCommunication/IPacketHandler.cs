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
        event Action<UInt16[]> SteppersStatesReceived;
        event Action<UInt16[]> SensorsValuesReceived;
        event Action<string> DebugMessageReceived;
        event Action<string> TubeBarCodeReceived;
        event Action<string> CartridgeBarCodeReceived;
        event Action<string> FirmwareVersionReceived;
        event Action<uint , CommandStateResponse.CommandStates> CommandStateReceived;

        void ProcessPacket(byte[] packet);
    }
}
