using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class StopCommand : AbstractCommand, IDeviceCommand
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
            SendPacket2 packet = new SendPacket2(3);
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
