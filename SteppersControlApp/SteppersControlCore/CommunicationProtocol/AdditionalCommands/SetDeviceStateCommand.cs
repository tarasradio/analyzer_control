using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class SetDeviceStateCommand : AbstractCommand, IDeviceCommand
    {
        public enum DeviseState
        {
            DEVICE_OFF = 0x00,
            DEVICE_ON = 0x01
        }

        private byte _device;
        private byte _state;

        public SetDeviceStateCommand(int device, DeviseState state) : base()
        {
            _device = (byte)device;
            _state = (byte)state;
        }

        public byte[] GetBytes()
        {
            SendPacket2 packet = new SendPacket2(3);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.SET_DEVICE_STATE);
            packet.SetData(1, _device);
            packet.SetData(2, _state);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
