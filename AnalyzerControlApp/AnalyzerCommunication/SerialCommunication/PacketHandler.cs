using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using System;

namespace AnalyzerCommunication.SerialCommunication
{
    public class PacketHandler
    {
        public delegate void DeviseStatesReceivedDelegate(UInt16[] states);
        public delegate void MessageReceivedDelegate(string message);
        public delegate void CommandStateReceivedDelegate(uint commandId, Protocol.CommandStates state);

        public event DeviseStatesReceivedDelegate SteppersStatesReceived;
        public event DeviseStatesReceivedDelegate SensorsValuesReceived;
        public event MessageReceivedDelegate DebugMessageReceived;
        public event MessageReceivedDelegate TubeBarCodeReceived;
        public event MessageReceivedDelegate CartridgeBarCodeReceived;
        public event MessageReceivedDelegate FirmwareVersionReceived;
        public event CommandStateReceivedDelegate CommandStateReceived;

        public PacketHandler() { }

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
            Protocol.CommandStates commandState = response.GetCommandState();

            CommandStateReceived(commandId, commandState);
        }

        private void ProcessSteppersStatesResponse(byte[] packet)
        {
            UInt16[] states = new SteppersStatesResponse(packet).GetStates();

            if (states != null)
            {
                //TODO: Убрать зависимость от Core!
                //if (states?.Length != Core.Settings.Steppers.Count)
                //{
                //    Logger.Info("[Packet handler] - Число двигателей в пакете не верное!");
                //}
                //else
                SteppersStatesReceived(states);
            }
        }
        private void ProcessSensorsValuesResponse(byte[] packet)
        {
            UInt16[] values = new SensorsValuesResponse(packet).GetStates();

            if (null != values)
            {
                //TODO: Убрать зависимость от Core!
                //if (values?.Length != Core.Settings.Sensors.Count)
                //{
                //    Logger.Info("[Packet handler] - Число датчиков в пакете не верное!");
                //}
                //else
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

        public void ProcessPacket(byte[] packet)
        {
            if (packet.Length == 0)
            {
                return;
            }

            byte packetType = packet[0];

            switch ((Protocol.ResponsesTypes)packetType)
            {
                case Protocol.ResponsesTypes.BAR_CODE_RESPONSE:
                    ProcessBarCodeResponse(packet);
                    break;
                case Protocol.ResponsesTypes.COMMAND_STATE_RESPONSE:
                    ProcessCommandStateResponse(packet);
                    break;
                case Protocol.ResponsesTypes.STEPPERS_STATES_RESPONSE:
                    ProcessSteppersStatesResponse(packet);
                    break;
                case Protocol.ResponsesTypes.FIRMWARE_VERSION_RESPONSE:
                    ProcessFirmwareVersionResponse(packet);
                    break;
                case Protocol.ResponsesTypes.SENSORS_VALUES_RESPONSE:
                    ProcessSensorsValuesResponse(packet);
                    break;
                case Protocol.ResponsesTypes.DEBUG_MESSAGE_RESPONSE:
                    ProcessDebugMessageResponse(packet);
                    break;
                default:
                    break;
            }
        }
    }
}
