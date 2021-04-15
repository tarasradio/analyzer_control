using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using AnalyzerCommunication.SerialCommunication;
using System;

namespace AnalyzerService
{
    public class ResponseHandler
    {
        public event Action<UInt16[]> SteppersStatesReceived;
        public event Action<UInt16[]> SensorsValuesReceived;
        public event Action<string> DebugMessageReceived;
        public event Action<string> TubeBarCodeReceived;
        public event Action<string> CartridgeBarCodeReceived;
        public event Action<string> FirmwareVersionReceived;
        public event Action<uint, CommandStateResponse.CommandStates> CommandStateReceived;

        IAnalyzerState analyzerState;

        public ResponseHandler(IPacketHandler handler, IAnalyzerState analyzerState)
        {
            this.analyzerState = analyzerState;

            handler.AddResponseHandler((byte)Protocol.ResponsesTypes.BAR_CODE_RESPONSE, new Action<byte[]>(ProcessBarCodeResponse));
            handler.AddResponseHandler((byte)Protocol.ResponsesTypes.COMMAND_STATE_RESPONSE, new Action<byte[]>(ProcessCommandStateResponse));
            handler.AddResponseHandler((byte)Protocol.ResponsesTypes.DEBUG_MESSAGE_RESPONSE, new Action<byte[]>(ProcessDebugMessageResponse));
            handler.AddResponseHandler((byte)Protocol.ResponsesTypes.FIRMWARE_VERSION_RESPONSE, new Action<byte[]>(ProcessFirmwareVersionResponse));
            handler.AddResponseHandler((byte)Protocol.ResponsesTypes.SENSORS_VALUES_RESPONSE, new Action<byte[]>(ProcessSensorsValuesResponse));
            handler.AddResponseHandler((byte)Protocol.ResponsesTypes.STEPPERS_STATES_RESPONSE, new Action<byte[]>(ProcessSteppersStatesResponse));
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
                    analyzerState.TubeBarcode = barCode;
                    break;
                case BarCodeResponse.ScannerTypes.CARTRIDGE_SCANNER:
                    CartridgeBarCodeReceived?.Invoke(barCode);
                    analyzerState.CartridgeBarcode = barCode;
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
                analyzerState.SteppersStates = states;
            }
        }
        private void ProcessSensorsValuesResponse(byte[] packet)
        {
            UInt16[] values = new SensorsValuesResponse(packet).GetStates();

            if (null != values)
            {
                SensorsValuesReceived?.Invoke(values);
                analyzerState.SensorsValues = values;
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
            analyzerState.FirmwareVersion = message;
        }
    }
}
