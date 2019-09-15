using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;

namespace SteppersControlCore.CommunicationProtocol.StepperCommands
{
    public class MoveCommand : AbstractCommand, IDeviceCommand
    {
        private byte _stepper;
        private Protocol.Direction _direction;
        private uint _countSteps;

        public MoveCommand(int stepper, Protocol.Direction direction, uint countSteps) : base()
        {
            _stepper = (byte)stepper;
            _direction = direction;
            _countSteps = countSteps;
        }

        public byte[] GetBytes()
        {
            byte[] stepsBytes = BitConverter.GetBytes(_countSteps);

            SendPacket2 packet = new SendPacket2(7);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.MOVE);
            packet.SetData(1, _stepper);
            packet.SetData(2, (byte)_direction);
            packet.SetData(3, stepsBytes);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
