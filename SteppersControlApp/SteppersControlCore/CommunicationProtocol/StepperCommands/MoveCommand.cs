using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class MoveCommand : AbstractCommand, ICommand
    {
        private byte _stepper;
        private Protocol.Direction _direction;
        private uint _countSteps;

        public MoveCommand(int stepper, Protocol.Direction direction, uint countSteps, uint packetId) : base(packetId, Protocol.CommandType.WAITING_COMMAND)
        {
            _stepper = (byte)stepper;
            _direction = direction;
            _countSteps = countSteps;
        }

        public byte[] GetBytes()
        {
            byte[] stepsBytes = BitConverter.GetBytes(_countSteps);

            SendPacket packet = new SendPacket(7);
            packet.SetPacketId(PacketId);

            packet.SetData(0, (byte)Protocol.StepperCommands.MOVE);
            packet.SetData(1, _stepper);
            packet.SetData(2, (byte)_direction);
            packet.SetData(3, stepsBytes);

            return packet.GetBytes();
        }
    }
}
