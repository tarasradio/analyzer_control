using System;
using System.Collections.Generic;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class SteppersCncCommand : AbstractCommand
    {
        const int BytesPerStepper = 5;
        Dictionary<int, int> steppers;
        Protocol.CncCommands commandCode;

        public SteppersCncCommand(Dictionary<int, int> steppers, Protocol.CncCommands commandCode) : base()
        {
            this.steppers = steppers;
            this.commandCode = commandCode;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(steppers.Count * BytesPerStepper + 2);
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

            return packet.GetBytes();
        }
    }
}
