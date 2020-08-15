using System;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class CommandStateResponse : ResponseBase
    {
        public enum CommandStates
        {
            COMMAND_EXECUTE_STARTED,
            COMMAND_BAD_FORMAT,
            COMMAND_EXECUTE_FINISHED
        }

        public CommandStateResponse(byte[] buffer) : base(buffer) 
        { 

        }

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
