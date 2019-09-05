using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class MoveCncCommand : AbstractCommand, IDeviceCommand
    {
        private Dictionary<int, int> _steps;

        public MoveCncCommand(Dictionary<int, int> steps, uint packetId) : base(packetId, Protocol.CommandTypes.WAITING_COMMAND)
        {
            _steps = steps;
        }

        public byte[] GetBytes()
        {
            SendPacket2 packet = new SendPacket2(_steps.Count * 6 + 2);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.CncCommands.CNC_MOVE);
            packet.SetData(1, (byte)_steps.Count);

            int i = 0;

            foreach (var stepper in _steps.Keys)
            {
                packet.SetData(i * 6 + 2, (byte)stepper);

                int stepsCount = _steps[stepper];

                Protocol.Direction direction = Protocol.Direction.FWD;
                if (stepsCount < 0)
                    direction = Protocol.Direction.REV;

                packet.SetData(i * 6 + 3, (byte)direction);
                byte[] stepsBytes = BitConverter.GetBytes((uint)Math.Abs(stepsCount));
                packet.SetData(i * 6 + 4, stepsBytes);
                i++;
            }
            return packet.GetBytes();
        }
    }
}
