
using AnalyzerDomain;
using AnalyzerDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace RemoteDatabaseApp.Connection
{
    public class Client
    {
        public event Action<String> RequestReceived;

        enum RequestsTypes
        {
            AnalysisRequest,
            AnalyzesListRequest
        };

        enum ResponcesTypes
        {
            CartridgeBarcodeResponse,
            CartridgesBarcodesResponse,
            UnknownBarcodeResponse
        };

        public TcpClient client;

        public Client(TcpClient tcpClient) {
            client = tcpClient;
        }

        public void Process() {
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                byte[] data = new byte[64]; // буфер для получаемых данных
                while (true)
                {
                    // получаем сообщение
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                    }
                    while (stream.DataAvailable);

                    String message = String.Empty;

                    if (data[0] == (int)RequestsTypes.AnalysisRequest) {
                        handleAnalysisReq(data, stream);
                    }
                    else if (data[0] == (int)RequestsTypes.AnalyzesListRequest) {
                        handleAnalyzesListReq(data, stream);
                    }
                    else {
                        handleUnknownReq(data, stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }

        private void handleAnalyzesListReq(byte[] data, NetworkStream stream)
        {
            StringBuilder builder = new StringBuilder();

            String barcode = Encoding.Unicode.GetString(data, 1, data.Length - 2);

            Console.WriteLine($"Запрошен штрихкод анализа: {barcode}");

            using (AnalyzerContext db = new AnalyzerContext())
            {
                db.SheduledAnalyzes.Include(a => a.AnalysisType).Load();
                db.Cartridges.Load();

                var analyzes = db.SheduledAnalyzes.Where(a => String.Equals(a.Barcode, barcode)).ToList();

                if(analyzes.Count > 0) {

                    List<String> cartridges = new List<string>();

                    List<byte> responseBytesList = new List<byte>();

                    byte[] analyzesCountBytes = BitConverter.GetBytes((UInt32)analyzes.Count);
                    responseBytesList.AddRange(analyzesCountBytes);

                    foreach(var analysis in analyzes) {
                        String cartridgeBarcode = analysis.AnalysisType.Cartridge.Description;

                        byte[] barcodeLengthBytes = BitConverter.GetBytes((UInt32)cartridgeBarcode.Length);
                        byte[] barcodeBytes = Encoding.Unicode.GetBytes(cartridgeBarcode);

                        responseBytesList.AddRange(barcodeLengthBytes);
                        responseBytesList.AddRange(barcodeBytes);
                    }

                    byte[] responseBytes = new byte[responseBytesList.Count + 1];
                    responseBytesList.CopyTo(responseBytes, 1);
                    responseBytes[0] = (byte)ResponcesTypes.CartridgesBarcodesResponse;

                    // отправка сообщения
                    stream.Write(responseBytes, 0, responseBytes.Length);

                } else {
                    Console.WriteLine($"Анализов для запрошенного пациента не найдено!");
                    byte[] responseBytes = new byte[1];
                    responseBytes[0] = (byte)ResponcesTypes.UnknownBarcodeResponse;
                    stream.Write(responseBytes, 0, responseBytes.Length);
                }
                
            }
        }

        private void handleAnalysisReq(byte[] data, NetworkStream stream)
        {
            StringBuilder builder = new StringBuilder();

            String barcode = Encoding.Unicode.GetString(data, 1, data.Length - 2);

            Console.WriteLine($"Запрошен штрихкод анализа: {barcode}");

            using (AnalyzerContext db = new AnalyzerContext())
            {
                db.SheduledAnalyzes.Include(a => a.AnalysisType).Load();
                db.Cartridges.Load();

                Analysis analysis = db.SheduledAnalyzes.FirstOrDefault(a => String.Equals(a.Barcode, barcode));
                if (analysis != null)
                {
                    String cartridgeBarcode = analysis.AnalysisType.Cartridge.Description;

                    Console.WriteLine($"Запрошен пациент: {analysis.Description}.");
                    RequestReceived($"Запрошен пациент: {analysis.Description}.");
                    Console.WriteLine($"Для проведения анализа требуется картридж со штрихкодом: {cartridgeBarcode}.");
                    
                    byte[] barcodeBytes = Encoding.Unicode.GetBytes(cartridgeBarcode);

                    byte[] responseBytes = new byte[barcodeBytes.Length + 1];
                    barcodeBytes.CopyTo(responseBytes, 1);
                    responseBytes[0] = (byte)ResponcesTypes.CartridgeBarcodeResponse;

                    // отправка сообщения
                    stream.Write(responseBytes, 0, responseBytes.Length);

                    analysis.CurrentStage = 1;

                    db.SheduledAnalyzes.Update(analysis);
                    db.SaveChanges();
                } else {
                    Console.WriteLine($"Запрошеный анализ не найден!");
                    byte[] responseBytes = new byte[1];
                    responseBytes[0] = (byte)ResponcesTypes.UnknownBarcodeResponse;
                    stream.Write(responseBytes, 0, responseBytes.Length);
                }
            }

            
        }

        private void handleUnknownReq(byte[] data, NetworkStream stream)
        {
            data = Encoding.Unicode.GetBytes("Принята неизвестная команда!");
            stream.Write(data, 0, data.Length);
        }
    }
}
