using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore;
using System.Threading;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    class WaitSensorValueCommand : AbstractCommand, IHostCommand
    {
        uint _sensor = 0;
        uint _value = 0;
        Protocol.ValueEdge _valueEdge = Protocol.ValueEdge.RisingEdge;

        public WaitSensorValueCommand(uint sensor, uint value, Protocol.ValueEdge edge) : base()
        {
            _sensor = sensor;
            _value = value;
            _valueEdge = edge;
        }

        public void Execute()
        {
            bool isComplete = false;
            while(!isComplete)
            {
                ushort sensorValue = Core.GetSensorValue(_sensor);
                Logger.AddMessage($"Wait value = {_value}, real value = {sensorValue}");

                if(Protocol.ValueEdge.RisingEdge == _valueEdge)
                {
                    if(_value <= sensorValue)
                        isComplete = true;
                }
                else if(Protocol.ValueEdge.FallingEdge == _valueEdge)
                {
                    if (_value >= sensorValue)
                        isComplete = true;
                }
                Thread.Sleep(20);
            }
        }
    }
}
