using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class SteppersCncCommand : AbstractCommand
    {
        const int BytesPerStepper = 5;
        Dictionary<int, int> _steppers;
        Protocol.CncCommands _commandCode;

        public SteppersCncCommand(Dictionary<int, int> steppers, Protocol.CncCommands commandCode) : base()
        {
            _steppers = steppers;
            _commandCode = commandCode;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(_steppers.Count * BytesPerStepper + 2);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)_commandCode);
            packet.SetData(1, (byte)_steppers.Count);

            int i = 0;

            foreach (var stepper in _steppers.Keys)
            {
                packet.SetData(i * BytesPerStepper + 2, (byte)stepper);
                byte[] speedBytes = BitConverter.GetBytes(_steppers[stepper]);
                packet.SetData(i * BytesPerStepper + 3, speedBytes);
                i++;
            }

            return packet.GetBytes();
        }
    }
}
