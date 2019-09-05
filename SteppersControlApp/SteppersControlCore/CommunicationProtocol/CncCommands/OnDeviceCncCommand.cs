using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class OnDeviceCncCommand : AbstractCommand, IDeviceCommand
    {
        List<int> _devices;

        public OnDeviceCncCommand(List<int> devices, uint packetId) : base(packetId, Protocol.CommandTypes.SIMPLE_COMMAND)
        {
            _devices = devices;
        }

        public byte[] GetBytes()
        {
            SendPacket2 packet = new SendPacket2(_devices.Count + 2);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.CncCommands.CNC_ON_DEVICE);
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
