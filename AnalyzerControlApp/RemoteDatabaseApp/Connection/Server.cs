using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteDatabaseApp.Connection
{
    public class Server
    {
        public String ServerAddress { get; set; } = String.Empty;
        public int ServerPort { get; set; } = 8888;

        public bool ServerStarted { get; set; } = false;

        public event Action<String> RequestReceived;

        static TcpListener listener;
        Thread serverThread;

        private String getHostAddress()
        {
            string strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();
        }

        public Server()
        {
            ServerAddress = $"{getHostAddress()}:{ServerPort}";
        }

        public void StartServer()
        {
            serverThread = new Thread(new ThreadStart(serverWorkCycle));
            serverThread.Start();

            ServerStarted = true;
        }

        public void StopServer()
        {
            if(ServerStarted)
                listener.Stop();
        }

        private void serverWorkCycle()
        {
            try
            {
                listener = new TcpListener(IPAddress.Parse(getHostAddress()), ServerPort);
                listener.Start();
                Console.WriteLine("Ожидание подключений...");

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);
                    clientObject.RequestReceived += RequestReceived;

                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }

    }
}
