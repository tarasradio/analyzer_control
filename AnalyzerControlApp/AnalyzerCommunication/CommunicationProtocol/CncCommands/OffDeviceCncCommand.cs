using System.Collections.Generic;

namespace AnalyzerCommunication.CommunicationProtocol.CncCommands
{
    public class OffDeviceCncCommand : AbstractCommand, IRemoteCommand
    {
        private List<int> devices;

        public OffDeviceCncCommand(List<int> devices) : base()
        {
            this.devices = devices;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(devices.Count + 2);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.CncCommands.CNC_OFF_DEVICE);
            packet.SetData(1, (byte)devices.Count);

            int i = 0;

            foreach (var device in devices)
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
