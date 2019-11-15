using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.Responses;

namespace SteppersControlCore.SerialCommunication
{
    public delegate void DeviseStatesReceivedDelegate(UInt16[] states);
    public delegate void MessageReceivedDelegate(string message);
    public delegate void CommandStateResponseReceivedDelegate(uint commandId, Protocol.CommandStates state);

    public class PackageHandler
    {
        public event DeviseStatesReceivedDelegate SteppersStatesReceived;
        public event DeviseStatesReceivedDelegate SensorsValuesReceived;
        public event MessageReceivedDelegate MessageReceived;
        public event MessageReceivedDelegate BarCodeAReceived;
        public event MessageReceivedDelegate BarCodeBReceived;
        public event MessageReceivedDelegate FirmwareVersionReceived;
        public event CommandStateResponseReceivedDelegate CommandStateResponseReceived;
        
        public PackageHandler()
        {

        }

        public void ProcessPacket(byte[] packet)
        {
            if(packet.Length == 0)
            {
                return;
            }

            byte packetType = packet[0];
            
            if((byte)Protocol.ResponsesTypes.DRIVERS_STATES == packetType)
            {
                UInt16[] states = new SteppersStatesResponse(packet).GetStates();
                
                if (states != null)
                {
                    if (states?.Length != Core.Settings.Steppers.Count)
                    {
                        Logger.Info("[Packet handler] - Число двигателей в пакете не верное!");
                    }
                    else
                        SteppersStatesReceived(states);
                }
            }
            else if((byte)Protocol.ResponsesTypes.SENSORS_VALUES == packetType)
            {
                UInt16[] values = new SensorsValuesResponse(packet).GetStates();
                
                if (null != values)
                {
                    if (values?.Length != Core.Settings.Sensors.Count)
                    {
                        Logger.Info("[Packet handler] - Число датчиков в пакете не верное!");
                    }
                    else
                        SensorsValuesReceived(values);
                }
            }
            else if((byte)Protocol.ResponsesTypes.COMMAND_STATE_RESPONSE == packetType)
            {
                CommandStateResponse response = new CommandStateResponse(packet);
                UInt32 commandId = response.GetCommandId();
                Protocol.CommandStates commandState = response.GetCommandState();
                CommandStateResponseReceived(commandId, commandState);
            }
            else if((byte)Protocol.ResponsesTypes.TEXT_MESSAGE == packetType)
            {
                string message = new DebugResponse(packet).GetDebugMessage();
                MessageReceived(message);
            }
            else if ((byte)Protocol.ResponsesTypes.BAR_CODE == packetType)
            {
                string message = new BarCodeResponse(packet).GetDebugMessage();
                BarCodeAReceived(message);
            }
            else if ((byte)Protocol.ResponsesTypes.FIRMWARE_VERSION == packetType)
            {
                string message = new FirmwareVersionResponse(packet).GetFirmwareVersion();
                FirmwareVersionReceived(message);
            }
        }
    }
}
