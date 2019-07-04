using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class OffDeviceCncCommand : AbstractCommand, ICommand
    {
        private List<int> _devices;

        public OffDeviceCncCommand(List<int> devices, uint packetId) : base(packetId, Protocol.CommandType.SIMPLE_COMMAND)
        {
            _devices = devices;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(_devices.Count + 2);
            packet.SetPacketId(PacketId);

            packet.SetData(0, (byte)Protocol.CncCommands.CNC_OFF_DEVICE);
            packet.SetData(1, (byte)_devices.Count);

            int i = 0;

            foreach (var device in _devices)
            {
                packet.SetData(i + 2, (byte)device);
                i++;
            }

            return packet.GetBytes();
        }
    }
}
