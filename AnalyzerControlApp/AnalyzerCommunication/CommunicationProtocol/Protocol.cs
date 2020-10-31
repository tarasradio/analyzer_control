namespace AnalyzerCommunication.CommunicationProtocol
{
    public static class Protocol
    {
        private static uint lastPacketId = 0;

        public static uint GetPacketId()
        {
            return lastPacketId++;
        }

        public enum ResponsesTypes
        {
            FIRMWARE_VERSION_RESPONSE = 0x0F,
            STEPPERS_STATES_RESPONSE = 0x10,
            SENSORS_VALUES_RESPONSE,
            COMMAND_STATE_RESPONSE,
            DEBUG_MESSAGE_RESPONSE,
            BAR_CODE_RESPONSE
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

        public enum CommandTypes
        {
            HOST_COMMAND,
            WAITING_COMMAND,
            SIMPLE_COMMAND
        }
    }
}
