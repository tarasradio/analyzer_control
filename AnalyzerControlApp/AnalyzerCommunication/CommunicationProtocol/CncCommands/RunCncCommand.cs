using System;
using System.Collections.Generic;

namespace AnalyzerCommunication.CommunicationProtocol.CncCommands
{
    public class RunCncCommand : AbstractCommand, IRemoteCommand
    {
        private Dictionary<int, int> steppers;
        const int BytesPerStepper = 5;

        uint sensor = 0;
        uint value = 0;
        Protocol.ValueEdge valueEdge = Protocol.ValueEdge.RisingEdge;

        public RunCncCommand(Dictionary<int, int> speeds, uint sensor, uint value, Protocol.ValueEdge edge) : base()
        {
            steppers = speeds;

            this.sensor = sensor;
            this.value = value;
            valueEdge = edge;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(steppers.Count * BytesPerStepper + 4 + 2);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.CncCommands.CNC_RUN);
            packet.SetData(1, (byte)steppers.Count);

            int i = 0;

            foreach (var stepper in steppers.Keys)
            {
                packet.SetData(i * BytesPerStepper + 2, (byte)stepper);
                
                byte[] speedBytes = BitConverter.GetBytes(steppers[stepper]);
                packet.SetData(i * BytesPerStepper + 3, speedBytes);
                i++;
            }

            packet.SetData(steppers.Count * BytesPerStepper + 2, (byte)sensor);
            byte[] valueBytes = BitConverter.GetBytes((ushort)value);
            packet.SetData(steppers.Count * BytesPerStepper + 3, valueBytes);
            packet.SetData(steppers.Count * BytesPerStepper + 5, (byte)valueEdge);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
