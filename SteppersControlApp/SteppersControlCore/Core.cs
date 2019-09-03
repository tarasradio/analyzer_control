using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SteppersControlCore
{
    public class Core
    {
        private static Logger _logger;
        public static Configuration _configuration;
        private static uint lastPacketId = 0;

        private static System.Threading.Mutex _mutex = new System.Threading.Mutex();

        public static uint GetPacketId()
        {
            return lastPacketId++;
        }

        public Core()
        {
            _logger = new Logger();
            _configuration = new Configuration();

            Logger.AddMessage("Запись работы системы начата");
        }

        public Logger GetLogger()
        {
            return _logger;
        }

        public Configuration getConfig()
        {
            return _configuration;
        }

        private static ushort[] _sensorsValues = null;

        public void InitSensorsValues()
        {
            _sensorsValues = new ushort[_configuration.Sensors.Count];
        }

        public void UpdateSensorsValues(ushort[] values)
        {
            if (values.Length != _configuration.Sensors.Count)
                return;
            _mutex.WaitOne();

            Array.Copy(values, _sensorsValues, values.Length);

            _mutex.ReleaseMutex();
        }

        public static ushort GetSensorValue(uint sensor)
        {
            ushort value = 0;

            _mutex.WaitOne();
            value = _sensorsValues[sensor];
            _mutex.ReleaseMutex();

            return value;
        }

        private static string _lastBarCode = null;

        public void UpdateBarCode(string barCode)
        {
            _mutex.WaitOne();

            _lastBarCode = barCode;

            _mutex.ReleaseMutex();
        }

        public static string GetLastBarCode()
        {
            string barCode = null;

            _mutex.WaitOne();
            barCode = _lastBarCode;
            _lastBarCode = null;
            _mutex.ReleaseMutex();

            return barCode;
        }
    }
}
