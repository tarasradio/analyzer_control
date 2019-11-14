using SteppersControlCore.Interfaces;
using System;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class MoveCommand : AbstractCommand, IRemoteCommand
    {
        private byte _stepper;
        private int _countSteps;

        public MoveCommand(int stepper, int countSteps) : base()
        {
            _stepper = (byte)stepper;
            _countSteps = countSteps;
        }

        public byte[] GetBytes()
        {
            byte[] stepsBytes = BitConverter.GetBytes(_countSteps);

            SendPacket packet = new SendPacket(6);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.MOVE);
            packet.SetData(1, _stepper);
            packet.SetData(2, stepsBytes);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
