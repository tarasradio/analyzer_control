using System;
using System.Text;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class BarCodeResponse : AbstaractResponse
    {
        public enum ScannerTypes
        {
            TUBE_SCANNER = 1,
            CARTRIDGE_SCANNER
        }

        public BarCodeResponse(byte[] buffer) : base (buffer) { }

        public string GetBarCode()
        {
            string message = Encoding.UTF8.GetString(buffer, 1, buffer.Length - 1);
            return message;
        }

        public ScannerTypes GetScannerType()
        {
            return (ScannerTypes)buffer[0];
        }
    }
}
