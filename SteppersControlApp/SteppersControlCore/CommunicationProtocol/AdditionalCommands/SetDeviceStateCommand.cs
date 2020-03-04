using SteppersControlCore.Interfaces;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class SetDeviceStateCommand : AbstractCommand, IRemoteCommand
    {
        public enum DeviseState
        {
            DEVICE_OFF = 0x00,
            DEVICE_ON = 0x01
        }

        private byte device;
        private byte state;

        public SetDeviceStateCommand(int device, DeviseState state) : base()
        {
            this.device = (byte)device;
            this.state = (byte)state;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(3);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.SET_DEVICE_STATE);
            packet.SetData(1, device);
            packet.SetData(2, state);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
