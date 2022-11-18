using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerCommunication.ServerCommunication
{
    public class DatabaseClient
    {
        enum RequestsTypes
        {
            AnalysisRequest,
            AnalyzesListRequest,
        };

        enum ResponcesTypes
        {
            CartridgeBarcodeResponse,
            CartridgesBarcodesResponse,
        };

        int port;
        string address;

        TcpClient client = null;
        NetworkStream stream = null;

        public DatabaseClient(string address = "192.168.219.51", int port = 8888)
        {
            this.address = address;
            this.port = port;
        }

        public bool Connect()
        {
            try
            {
                client = new TcpClient(address, port);
                stream = client.GetStream();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Disconnect()
        {
            client.Close();
        }

        public String GetCartridgeID(string barcode)
        {
            byte[] barcodeBytes = Encoding.Unicode.GetBytes(barcode.Trim());
            byte[] data = new byte[barcodeBytes.Length + 1];
            barcodeBytes.CopyTo(data, 1);
            data[0] = (byte)RequestsTypes.AnalysisRequest;

            // отправка сообщения
            stream.Write(data, 0, data.Length);

            // получаем ответ
            data = new byte[100]; // буфер для получаемых данных

            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
            }
            while (stream.DataAvailable);

            if (data[0] == (int)ResponcesTypes.CartridgeBarcodeResponse) {
                return handleCartridgeBarcodeResponse(data);
            } else {
                return null;
            }
        }

        public string[] GetCatridgesIDs(string barcode)
        {
            byte[] barcodeBytes = Encoding.Unicode.GetBytes(barcode.Trim());
            byte[] data = new byte[barcodeBytes.Length + 1];
            barcodeBytes.CopyTo(data, 1);
            data[0] = (byte)RequestsTypes.AnalyzesListRequest;

            // отправка сообщения
            stream.Write(data, 0, data.Length);

            // получаем ответ
            data = new byte[1000]; // буфер для получаемых данных

            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
            }
            while (stream.DataAvailable);

            if (data[0] == (int)ResponcesTypes.CartridgesBarcodesResponse) {
                return handleCartridgesBarcodesResponse(data);
            } else {
                return null;
            }
        }

        private string handleCartridgeBarcodeResponse(byte[] data)
        {
            return Encoding.Unicode.GetString(data, 1, data.Length - 2);
        }

        private string[] handleCartridgesBarcodesResponse(byte[] data)
        {
            List<string> barcodes = new List<string>();

            int barcodesCount = BitConverter.ToInt32(data, 1);

            int currentByte = 1 + 4;

            for (int i = 0; i < barcodesCount; i++)
            {
                int barcodeLength = BitConverter.ToInt32(data, currentByte);
                currentByte += 4;
                String barcode = Encoding.Unicode.GetString(data, currentByte, barcodeLength * 2);
                barcodes.Add(barcode);
                currentByte += barcodeLength * 2;
            }

            return barcodes.ToArray();
        }
    }
}
