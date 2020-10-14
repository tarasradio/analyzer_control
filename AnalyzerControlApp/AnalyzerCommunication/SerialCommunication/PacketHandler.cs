using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace AnalyzerCommunication.SerialCommunication
{
    public class PacketHandler : IPacketHandler
    {
        public event Action<UInt16[]> SteppersStatesReceived;
        public event Action<UInt16[]> SensorsValuesReceived;
        public event Action<string> DebugMessageReceived;
        public event Action<string> TubeBarCodeReceived;
        public event Action<string> CartridgeBarCodeReceived;
        public event Action<string> FirmwareVersionReceived;
        public event Action<uint, CommandStateResponse.CommandStates> CommandStateReceived;

        IAnalyzerContext analyzerContext;

        private Dictionary<Protocol.ResponsesTypes, Action<byte[]>> responsesHandlers;

        public PacketHandler(IAnalyzerContext analyzerContext)
        {
            this.analyzerContext = analyzerContext;
            buildHandlers();
        }

        private void buildHandlers()
        {
            responsesHandlers = new Dictionary<Protocol.ResponsesTypes, Action<byte[]>>()
            {
                { Protocol.ResponsesTypes.BAR_CODE_RESPONSE, new Action<byte[]>(ProcessBarCodeResponse) },
                { Protocol.ResponsesTypes.COMMAND_STATE_RESPONSE, new Action<byte[]>(ProcessCommandStateResponse) },
                { Protocol.ResponsesTypes.DEBUG_MESSAGE_RESPONSE, new Action<byte[]>(ProcessDebugMessageResponse) },
                { Protocol.ResponsesTypes.FIRMWARE_VERSION_RESPONSE, new Action<byte[]>(ProcessFirmwareVersionResponse) },
                { Protocol.ResponsesTypes.SENSORS_VALUES_RESPONSE, new Action<byte[]>(ProcessSensorsValuesResponse) },
                { Protocol.ResponsesTypes.STEPPERS_STATES_RESPONSE, new Action<byte[]>(ProcessSteppersStatesResponse) }
            };
        }

        public void ProcessPacket(byte[] packet)
        {
            if (packet.Length > 0)
            {
                byte responseType = packet[0];

                try
                {
                    responsesHandlers[(Protocol.ResponsesTypes)responseType].Invoke(packet);
                }
                catch(KeyNotFoundException)
                {
                    Logger.Info($"[{nameof(PacketHandler)}] - Uncknown response with code: {responseType} has been received.");
                }
            }
        }

        private void ProcessBarCodeResponse(byte[] packet)
        {
            BarCodeResponse response = new BarCodeResponse(packet);

            string barCode = response.GetBarCode();
            BarCodeResponse.ScannerTypes type = response.GetScannerType();

            switch (type)
            {
                case BarCodeResponse.ScannerTypes.TUBE_SCANNER:
                    TubeBarCodeReceived?.Invoke(barCode);
                    analyzerContext.TubeBarcode = barCode;
                    break;
                case BarCodeResponse.ScannerTypes.CARTRIDGE_SCANNER:
                    CartridgeBarCodeReceived?.Invoke(barCode);
                    analyzerContext.CartridgeBarcode = barCode;
                    break;
                default:
                    break;
            }
        }

        private void ProcessCommandStateResponse(byte[] packet)
        {
            CommandStateResponse response = new CommandStateResponse(packet);
            UInt32 commandId = response.GetCommandId();
            CommandStateResponse.CommandStates commandState = response.GetCommandState();

            CommandStateReceived?.Invoke(commandId, commandState);
        }

        private void ProcessSteppersStatesResponse(byte[] packet)
        {
            UInt16[] states = new SteppersStatesResponse(packet).GetStates();

            if (states != null)
            {
                SteppersStatesReceived?.Invoke(states);
                analyzerContext.SteppersStates = states;
            }
        }
        private void ProcessSensorsValuesResponse(byte[] packet)
        {
            UInt16[] values = new SensorsValuesResponse(packet).GetStates();

            if (null != values)
            {
                SensorsValuesReceived?.Invoke(values);
                analyzerContext.SensorsValues = values;
            }
        }

        private void ProcessDebugMessageResponse(byte[] packet)
        {
            string message = new DebugResponse(packet).GetDebugMessage();

            DebugMessageReceived?.Invoke(message);
        }

        private void ProcessFirmwareVersionResponse(byte[] packet)
        {
            string message = new FirmwareVersionResponse(packet).GetFirmwareVersion();

            FirmwareVersionReceived?.Invoke(message);
            analyzerContext.FirmwareVersion = message;
        }
    }
}
