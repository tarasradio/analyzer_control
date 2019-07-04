using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.Responses;

namespace SteppersControlCore.SerialCommunication
{
    public delegate void SteppersStatesReceivedDelegate(UInt16[] states);
    public delegate void MessageReceivedDelegate(string message);
    public delegate void CommandStateResponseReceivedDelegate(uint commandId, Protocol.CommandStates state);

    public class PackageHandler
    {
        public event SteppersStatesReceivedDelegate SteppersStatesReceived;
        public event MessageReceivedDelegate MessageReceived;
        public event CommandStateResponseReceivedDelegate CommandStateResponseReceived;

        Logger _logger;

        public PackageHandler(Logger logger)
        {
            _logger = logger;
        }

        public void ProcessPacket(byte[] packet)
        {
            if(packet.Length == 0)
            {
                return;
            }

            byte packetType = packet[0];
            
            switch(packetType)
            {
                case (byte)Protocol.ResponsesTypes.DRIVERS_STATES:
                    {
                        UInt16[] states = new SteppersStatesResponse(packet).GetStates();
                        if (states?.Length != Core._configuration.Steppers.Count)
                        {
                            _logger.AddMessage("Число двигателей в пакете не верное!");
                        }
                        if (states != null)
                        {
                            SteppersStatesReceived(states);
                        }
                    }
                    break;
                case (byte)Protocol.ResponsesTypes.COMMAND_STATE_RESPONSE:
                    {
                        CommandStateResponse response = new CommandStateResponse(packet);
                        UInt32 commandId = response.GetCommandId();
                        Protocol.CommandStates commandState = response.GetCommandState();
                        CommandStateResponseReceived(commandId, commandState);
                    }
                    break;
                case (byte)Protocol.ResponsesTypes.TEXT_MESSAGE:
                    {
                        string message = new DebugResponse(packet).GetDebugMessage();
                        MessageReceived(message);
                    }
                    break;
                default:
                    {

                    }
                    break;

            }
        }
    }
}
