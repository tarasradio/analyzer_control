using SteppersControlCore.Interfaces;
using System.Threading;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    class WaitSensorValueCommand : AbstractCommand, IHostCommand
    {
        uint sensor = 0;
        uint value = 0;
        Protocol.ValueEdge valueEdge = Protocol.ValueEdge.RisingEdge;

        public WaitSensorValueCommand(uint sensor, uint value, Protocol.ValueEdge edge) : base()
        {
            this.sensor = sensor;
            this.value = value;
            valueEdge = edge;
        }

        public void Execute()
        {
            bool isComplete = false;
            while(!isComplete)
            {
                ushort sensorValue = Core.GetSensorValue(sensor);
                Logger.Info($"Wait value = {value}, real value = {sensorValue}");

                if(Protocol.ValueEdge.RisingEdge == valueEdge)
                {
                    if(value <= sensorValue)
                        isComplete = true;
                }
                else if(Protocol.ValueEdge.FallingEdge == valueEdge)
                {
                    if (value >= sensorValue)
                        isComplete = true;
                }
                Thread.Sleep(20);
            }
        }
    }
}
