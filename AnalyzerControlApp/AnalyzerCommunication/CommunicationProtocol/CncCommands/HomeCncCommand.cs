using System;
using System.Collections.Generic;

namespace AnalyzerCommunication.CommunicationProtocol.CncCommands
{
    public class HomeCncCommand : AbstractCommand, IRemoteCommand
    {
        private const int BytesPerStepper = 5;
        private Dictionary<int, int> steppers;
        private Protocol.CncCommands commandCode;

        byte is_falling_edge = 1;

        public HomeCncCommand(Dictionary<int, int> steppers, bool falling_edge = true)
        {
            this.steppers = steppers;
            this.commandCode = Protocol.CncCommands.CNC_HOME;
            is_falling_edge = (byte)(falling_edge ? 1 : 0);
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(steppers.Count * BytesPerStepper + 3);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)commandCode);
            packet.SetData(1, (byte)steppers.Count);

            int i = 0;

            foreach (var stepper in steppers.Keys)
            {
                packet.SetData(i * BytesPerStepper + 2, (byte)stepper);
                byte[] speedBytes = BitConverter.GetBytes(steppers[stepper]);
                packet.SetData(i * BytesPerStepper + 3, speedBytes);
                i++;
            }

            packet.SetData(steppers.Count * BytesPerStepper + 2, is_falling_edge);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
