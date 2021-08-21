namespace AnalyzerCommunication.CommunicationProtocol.AdditionalCommands
{
    public enum BarcodeScanner
    {
        TubeScanner = 0x00,
        CartridgeScanner = 0x01
    }

    public class ScanBarcodeCommand : AbstractCommand, IRemoteCommand
    {
        public ScanBarcodeCommand(BarcodeScanner scanner) : base() {
            _scanner = (byte)scanner;
        }

        private byte _scanner;

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(2);
            packet.SetPacketId(commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.START_BARCODE_SCAN);
            packet.SetData(1, _scanner);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}
