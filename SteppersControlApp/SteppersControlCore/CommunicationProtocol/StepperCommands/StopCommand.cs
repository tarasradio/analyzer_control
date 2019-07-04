using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class StopCommand : AbstractCommand, ICommand
    {
        public enum StopType
        {
            SOFT_STOP = 0x00,
            HARD_STOP = 0x01
        }

        private byte _stepper;
        private byte _stopType;

        public StopCommand(int stepper, StopType stopType, uint packetId) : base(packetId, Protocol.CommandType.SIMPLE_COMMAND)
        {
            _stepper = (byte)stepper;
            _stopType = (byte)stopType;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(3);
            packet.SetPacketId(PacketId);

            packet.SetData(0, (byte)Protocol.StepperCommands.STOP);
            packet.SetData(1, _stepper);
            packet.SetData(2, _stopType);

            return packet.GetBytes();
        }
    }
}
