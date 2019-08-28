using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class RunCncCommand : AbstractCommand, IDeviceCommand
    {
        private Dictionary<int, int> _speeds;

        uint _sensor = 0;
        uint _value = 0;
        Protocol.ValueEdge _valueEdge = Protocol.ValueEdge.RisingEdge;

        public RunCncCommand(Dictionary<int, int> speeds, uint sensor, uint value, Protocol.ValueEdge edge, uint packetId) : base(packetId, Protocol.CommandTypes.WAITING_COMMAND)
        {
            _speeds = speeds;

            _sensor = sensor;
            _value = value;
            _valueEdge = edge;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(_speeds.Count * 6 + 4 + 2);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.CncCommands.CNC_RUN);
            packet.SetData(1, (byte)_speeds.Count);

            int i = 0;

            foreach (var stepper in _speeds.Keys)
            {
                packet.SetData(i * 6 + 2, (byte)stepper);

                int speed = _speeds[stepper];

                Protocol.Direction direction = Protocol.Direction.FWD;
                if (speed < 0)
                    direction = Protocol.Direction.REV;

                packet.SetData(i * 6 + 3, (byte)direction);
                byte[] speedBytes = BitConverter.GetBytes((uint)Math.Abs(speed));
                packet.SetData(i * 6 + 4, speedBytes);
                i++;
            }

            packet.SetData(_speeds.Count * 6 + 2, (byte)_sensor);
            byte[] valueBytes = BitConverter.GetBytes((ushort)_value);
            packet.SetData(_speeds.Count * 6 + 3, valueBytes);
            packet.SetData(_speeds.Count * 6 + 5, (byte)_valueEdge);

            return packet.GetBytes();
        }
    }
}
