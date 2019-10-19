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
        private int _countSteps;

        public MoveCommand(int stepper, int countSteps) : base()
        {
            _stepper = (byte)stepper;
            _countSteps = countSteps;
        }

        public byte[] GetBytes()
        {
            byte[] stepsBytes = BitConverter.GetBytes(_countSteps);

            SendPacket2 packet = new SendPacket2(6);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.StepperCommands.MOVE);
            packet.SetData(1, _stepper);
            packet.SetData(2, stepsBytes);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
