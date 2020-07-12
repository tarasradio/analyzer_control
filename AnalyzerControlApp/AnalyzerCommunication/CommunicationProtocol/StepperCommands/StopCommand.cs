namespace AnalyzerCommunication.CommunicationProtocol.StepperCommands
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

        private byte stepper;
        private byte stopType;

        public StopCommand(int stepper, StopType stopType) : base()
        {
            this.stepper = (byte)stepper;
            this.stopType = (byte)stopType;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(3);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.STOP);
            packet.SetData(1, stepper);
            packet.SetData(2, stopType);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
