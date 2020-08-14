using System;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class CommandStateResponse : AbstaractResponse
    {
        public enum CommandStates
        {
            COMMAND_RECEIVED,
            COMMAND_BAD_FORMAT,
            COMMAND_EXECUTED
        }

        public CommandStateResponse(byte[] buffer) : base(buffer) { }

        public uint GetCommandId()
        {
            uint id = BitConverter.ToUInt32(buffer, 1);
            return id;
        }

        public CommandStates GetCommandState()
        {
            return (CommandStates)buffer[0];
        }
    }
}
