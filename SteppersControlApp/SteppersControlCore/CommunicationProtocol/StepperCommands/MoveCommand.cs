using SteppersControlCore.Interfaces;
using System;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class MoveCommand : AbstractCommand, IRemoteCommand
    {
        private byte stepper;
        private int countSteps;

        public MoveCommand(int stepper, int countSteps) : base()
        {
            this.stepper = (byte)stepper;
            this.countSteps = countSteps;
        }

        public byte[] GetBytes()
        {
            byte[] stepsBytes = BitConverter.GetBytes(countSteps);

            SendPacket packet = new SendPacket(6);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.MOVE);
            packet.SetData(1, stepper);
            packet.SetData(2, stepsBytes);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
