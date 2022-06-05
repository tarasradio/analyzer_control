using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerCommunication.CommunicationProtocol.AdditionalCommands
{
    public enum LEDColors
    {
        LED_OFF = 0x00,
        LED_GREEN,
        LED_RED,
        LED_YELLOW
    }

    public class LEDColor
    {
        public byte R { get; set; } = 0;
        public byte G { get; set; } = 0;
        public byte B { get; set; } = 0;

        public LEDColor()
        {

        }

        public LEDColor(int r, int g, int b)
        {
            (R, G, B) = ((byte)r, (byte)g, (byte)b);
        }

        public static LEDColor Red() {
            return new LEDColor(255, 0, 0);
        }

        public static LEDColor Green() {
            return new LEDColor(0, 255, 0);
        }

        public static LEDColor Blue() {
            return new LEDColor(0, 0, 255);
        }

        public static LEDColor NoColor() {
            return new LEDColor(0, 0, 0);
        }
    }

    public class SetLedColorCommand : AbstractCommand, IRemoteCommand
    {
        private byte led;

        private byte r;
        private byte g;
        private byte b;

        public SetLedColorCommand(int led, LEDColor color) : base()
        {
            this.led = (byte)led;

            this.r = color.R;
            this.g = color.G;
            this.b = color.B;
        }

        public SetLedColorCommand(int led, int r, int g, int b) : base()
        {
            this.led = (byte)led;
            this.r = (byte)r;
            this.g = (byte)g;
            this.b = (byte)b;
        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(5);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.SET_LED_COLOR);
            packet.SetData(1, led);
            packet.SetData(2, r);
            packet.SetData(3, g);
            packet.SetData(4, b);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
