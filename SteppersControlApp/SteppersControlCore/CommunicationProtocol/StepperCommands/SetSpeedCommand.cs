using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class SetSpeedCommand : AbstractCommand, IDeviceCommand
    {
        private byte _stepper;
        uint _speed;

        public SetSpeedCommand(int stepper, uint speed, uint packetId) : base(packetId, Protocol.CommandTypes.SIMPLE_COMMAND)
        {
            _stepper = (byte)stepper;
            _speed = speed;
        }

        public byte[] GetBytes()
        {
            byte[] speedBytes = BitConverter.GetBytes(_speed);

            SendPacket2 packet = new SendPacket2(6);

            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.SET_SPEED);
            packet.SetData(1, _stepper);
            packet.SetData(2, speedBytes);

            return packet.GetBytes();
        }
    }
}
