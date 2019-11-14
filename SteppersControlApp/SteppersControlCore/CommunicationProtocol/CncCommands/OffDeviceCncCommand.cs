using SteppersControlCore.Interfaces;
using System.Collections.Generic;

namespace SteppersControlCore.CommunicationProtocol.CncCommands
{
    public class OffDeviceCncCommand : AbstractCommand, IRemoteCommand
    {
        private List<int> _devices;

        public OffDeviceCncCommand(List<int> devices) : base()
        {
            _devices = devices;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(_devices.Count + 2);
            packet.SetPacketId(_commandId);

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

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
