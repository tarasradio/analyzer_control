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
            AnalysisRequest
        };

        enum ResponcesTypes
        {
            CartridgeBarcodeResponse
        };

        const int port = 8888;
        const string address = "192.168.219.51";

        TcpClient client = null;
        NetworkStream stream = null;

        public DatabaseClient()
        {

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

            if (data[0] == (int)ResponcesTypes.CartridgeBarcodeResponse)
            {
                return handleCartridgeBarcodeResponse(data);
            }
            else
            {
                return null;
            }
        }

        private string handleCartridgeBarcodeResponse(byte[] data)
        {
            return Encoding.Unicode.GetString(data, 1, data.Length - 2);
        }
    }
}
