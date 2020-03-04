using SteppersControlCore.Interfaces;
using System;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class SetSpeedCommand : AbstractCommand, IRemoteCommand
    {
        private byte stepper;
        uint speed;

        public SetSpeedCommand(int stepper, uint speed) : base()
        {
            this.stepper = (byte)stepper;
            this.speed = speed;
        }

        public byte[] GetBytes()
        {
            byte[] speedBytes = BitConverter.GetBytes(speed);

            SendPacket packet = new SendPacket(6);

            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.SET_SPEED);
            packet.SetData(1, stepper);
            packet.SetData(2, speedBytes);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
