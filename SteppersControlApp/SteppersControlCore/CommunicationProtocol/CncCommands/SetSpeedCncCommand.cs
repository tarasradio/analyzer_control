using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class SetSpeedCncCommand : AbstractCommand, IDeviceCommand
    {
        private Dictionary<int, int> _speeds;

        public SetSpeedCncCommand(Dictionary<int, int> speeds) : base()
        {
            _speeds = speeds;
        }

        public byte[] GetBytes()
        {
            SendPacket2 packet = new SendPacket2(_speeds.Count * 5 + 2);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.CncCommands.CNC_SET_SPEED);
            packet.SetData(1, (byte)_speeds.Count);
            
            int i = 0;

            foreach (var stepper in _speeds.Keys)
            {
                packet.SetData(i * 5 + 2, (byte)stepper);
                byte[] speedBytes = BitConverter.GetBytes(_speeds[stepper]);
                packet.SetData(i * 5 + 3, speedBytes);
                i++;
            }

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
