using SteppersControlCore.Interfaces;
using System;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class RunCommand : AbstractCommand, IRemoteCommand
    {
        private byte _stepper;
        private int _speed;

        public RunCommand(int stepper, int speed) : base()
        {
            _stepper = (byte)stepper;
            _speed = speed;
        }

        public byte[] GetBytes()
        {
            byte[] speedBytes = BitConverter.GetBytes(_speed);

            SendPacket packet = new SendPacket(6);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.RUN);
            packet.SetData(1, _stepper);
            packet.SetData(2, speedBytes);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
