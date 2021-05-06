using Infrastructure;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyzerService
{
    public class ConnectionService
    {
        private string _portName = "COM3";
        private bool _connectionExtablished = false;

        public Action<bool> DeviceConnectionChanged = null;

        private Thread _thread;

        public ConnectionService()
        {
            
        }

        public void Run()
        {
            _thread = new Thread(checkConnectionCycle)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
            _thread.Start();
        }

        void checkConnectionCycle()
        {
            while (true) {
                checkConnection();
                Thread.Sleep(100);
            }
        }

        private void checkConnection()
        {
            bool deviceConnected = Analyzer.Serial.GetAvailablePorts().Contains(_portName);

            if (deviceConnected)
            {
                if (!_connectionExtablished)
                {
                    Logger.Info($"Устройство на порту {_portName} было подключено.");
                    DeviceConnectionChanged?.Invoke(true);
                }
            }
            else
            {
                if (_connectionExtablished)
                {
                    Logger.Info($"Устройство на порту {_portName} было отключено.");
                    DeviceConnectionChanged?.Invoke(false);
                }
            }

            _connectionExtablished = deviceConnected;
        }
    }
}
