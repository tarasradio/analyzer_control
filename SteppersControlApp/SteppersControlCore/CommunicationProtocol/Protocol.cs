using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol
{
    public class Protocol
    {
        private static uint lastPacketId = 0;
        public static byte[] PacketHeader = { 0x55, 0x55 };
        public static byte[] PacketEnd = { 0xAA, 0xAA };
        
        public static uint GetPacketId()
        {
            return lastPacketId++;
        }

        public enum CommandStates
        {
            COMMAND_OK,
            COMMAND_BAD_FORMAT,
            COMMAND_DONE
        }

        public enum ResponsesTypes
        {
            FIRMWARE_VERSION = 0x0F,
            DRIVERS_STATES = 0x10,
            SENSORS_VALUES,
            COMMAND_STATE_RESPONSE,
            TEXT_MESSAGE,
            BAR_CODE
        }

        public enum StepperCommands
        {
            GO_UNTIL = 0x0F,
            RUN = 0x10,
            MOVE = 0x11,
            STOP = 0x12,
            SET_SPEED = 0x13
        }

        public enum AdditionalCommands
        {
            SET_DEVICE_STATE = 0x14,
            ABORT_EXECUTION,
            WAIT_TIME,
            BAR_START,
            GET_FIRMWARE_VERSION
        }

        public enum CncCommands
        {
            CNC_MOVE = 0x19,
            CNC_SET_SPEED,
            CNC_STOP,
            CNC_HOME,
            CNC_ON_DEVICE,
            CNC_OFF_DEVICE,
            CNC_RUN
        }

        public enum Direction
        {
            REV = 0x00,
            FWD = 0x01
        }

        public enum ValueEdge
        {
            RisingEdge,
            FallingEdge
        }

        public enum CommandTypes
        {
            HOST_COMMAND,
            WAITING_COMMAND,
            SIMPLE_COMMAND
        }
    }
}
