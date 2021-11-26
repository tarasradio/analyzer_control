using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using System.Text;

namespace AnalyzerCommunication.CommunicationProtocol.Responses
{
    public class BarcodeResponse : ResponseBase
    {
        public BarcodeResponse(byte[] buffer) : base (buffer) { }

        public string Barcode { 
            get => getBarcode();
        }

        private string getBarcode()
        {
            return Encoding.UTF8.GetString(buffer, 1, buffer.Length - 1);
        }

        public BarcodeScanner ScannerType {
            get => (BarcodeScanner)buffer[0];
        }
    }
}
