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
        private Dictionary<int, int> _steppers;
        const int BytesPerStepper = 5;

        uint _sensor = 0;
        uint _value = 0;
        Protocol.ValueEdge _valueEdge = Protocol.ValueEdge.RisingEdge;

        public RunCncCommand(Dictionary<int, int> speeds, uint sensor, uint value, Protocol.ValueEdge edge) : base()
        {
            _steppers = speeds;

            _sensor = sensor;
            _value = value;
            _valueEdge = edge;
        }

        public byte[] GetBytes()
        {
            SendPacket2 packet = new SendPacket2(_steppers.Count * BytesPerStepper + 4 + 2);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.CncCommands.CNC_RUN);
            packet.SetData(1, (byte)_steppers.Count);

            int i = 0;

            foreach (var stepper in _steppers.Keys)
            {
                packet.SetData(i * BytesPerStepper + 2, (byte)stepper);
                
                byte[] speedBytes = BitConverter.GetBytes(_steppers[stepper]);
                packet.SetData(i * BytesPerStepper + 3, speedBytes);
                i++;
            }

            packet.SetData(_steppers.Count * BytesPerStepper + 2, (byte)_sensor);
            byte[] valueBytes = BitConverter.GetBytes((ushort)_value);
            packet.SetData(_steppers.Count * BytesPerStepper + 3, valueBytes);
            packet.SetData(_steppers.Count * BytesPerStepper + 5, (byte)_valueEdge);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.WAITING_COMMAND;
        }
    }
}
