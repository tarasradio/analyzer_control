using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using Infrastructure;
using System;
using System.Collections.Generic;

namespace AnalyzerCommunication.SerialCommunication
{
    public class PacketHandler
    {
        public delegate void DeviseStatesReceivedDelegate(UInt16[] states);
        public delegate void MessageReceivedDelegate(string message);
        public delegate void CommandStateReceivedDelegate(uint commandId, CommandStateResponse.CommandStates state);

        public event DeviseStatesReceivedDelegate SteppersStatesReceived;
        public event DeviseStatesReceivedDelegate SensorsValuesReceived;
        public event MessageReceivedDelegate DebugMessageReceived;
        public event MessageReceivedDelegate TubeBarCodeReceived;
        public event MessageReceivedDelegate CartridgeBarCodeReceived;
        public event MessageReceivedDelegate FirmwareVersionReceived;
        public event CommandStateReceivedDelegate CommandStateReceived;

        private Dictionary<Protocol.ResponsesTypes, Action<byte[]>> responsesHandlers;

        public PacketHandler()
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
                    TubeBarCodeReceived(barCode);
                    break;
                case BarCodeResponse.ScannerTypes.CARTRIDGE_SCANNER:
                    CartridgeBarCodeReceived(barCode);
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

            CommandStateReceived(commandId, commandState);
        }

        private void ProcessSteppersStatesResponse(byte[] packet)
        {
            UInt16[] states = new SteppersStatesResponse(packet).GetStates();

            if (states != null)
            {
                //TODO: (Проверять в Core число двигателей)
                SteppersStatesReceived(states);
            }
        }
        private void ProcessSensorsValuesResponse(byte[] packet)
        {
            UInt16[] values = new SensorsValuesResponse(packet).GetStates();

            if (null != values)
            {
                //TODO: (Ароверять в Core число двигателей)
                SensorsValuesReceived(values);
            }
        }

        private void ProcessDebugMessageResponse(byte[] packet)
        {
            string message = new DebugResponse(packet).GetDebugMessage();

            DebugMessageReceived(message);
        }

        private void ProcessFirmwareVersionResponse(byte[] packet)
        {
            string message = new FirmwareVersionResponse(packet).GetFirmwareVersion();

            FirmwareVersionReceived(message);
        }
    }
}
