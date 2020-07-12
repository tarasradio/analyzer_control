using System;

namespace AnalyzerCommunication.CommunicationProtocol.StepperCommands
{
    public class HomeCommand : AbstractCommand, IRemoteCommand
    {
        private byte stepper;
        private int speed;

        public HomeCommand(int stepper, int speed) : base()
        {
            this.stepper = (byte)stepper;
            this.speed = speed;
        }

        public byte[] GetBytes()
        {
            byte[] speedBytes = BitConverter.GetBytes(speed);

            SendPacket packet = new SendPacket(6);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.GO_UNTIL);
            packet.SetData(1, stepper);
            packet.SetData(2, speedBytes);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
