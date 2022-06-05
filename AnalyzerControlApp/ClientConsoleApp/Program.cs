using System;
using System.Net.Sockets;
using System.Text;

namespace ClientConsoleApp
{
    class Program
    {
        static Client client;

        static void Main(string[] args)
        {
            client = new Client();
            try
            {
                bool isConnected = client.Connect();

                if (!isConnected) throw new Exception();

                while (true)
                {
                    Console.Write("Введите код команды: ");
                    int commandCode = int.Parse(Console.ReadLine());
                    Console.Write("Введите штрихкод: ");
                    String barcode = Console.ReadLine();

                    String cartridgeBarcode = client.GetCartridgeBarcode(barcode);

                    if(barcode != null)
                    {
                        Console.WriteLine($"Анализ найден, штрихкод картриджа: {cartridgeBarcode}.");
                    } else {
                        Console.WriteLine($"Анализ не найден!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Disconnect();
            }
        }
    }
}
