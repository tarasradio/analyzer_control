using SteppersControlCore.Interfaces;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class StopCommand : AbstractCommand, IRemoteCommand
    {
        public enum StopType
        {
            SOFT_STOP = 0x00,
            HARD_STOP = 0x01,
            HiZ_SOFT,
            HiZ_HARD
        }

        private byte _stepper;
        private byte _stopType;

        public StopCommand(int stepper, StopType stopType) : base()
        {
            _stepper = (byte)stepper;
            _stopType = (byte)stopType;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(3);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.STOP);
            packet.SetData(1, _stepper);
            packet.SetData(2, _stopType);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
