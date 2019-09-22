using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.Controllers;
using SteppersControlCore.MachineControl;
using SteppersControlCore.SerialCommunication;

namespace SteppersControlCore
{
    public class Core
    {
        private static Logger _logger;
        public static Configuration _configuration { get; private set; }

        public static PackageReceiver PackReceiver { get; private set; }
        public static PackageHandler PackHandler { get; private set; }
        public static SerialHelper Serial { get; private set; }
        public static CncExecutor CNCExecutor { get; private set; }
        public static TaskExecutor Executor { get; private set; }

        public static ArmController Arm { get; private set; }
        public static TransporterController Transporter { get; private set; }
        public static RotorController Rotor { get; set; }
        public static LoadController Loader { get; set; }
        public static PompController Pomp { get; set; }

        public static DemoExecutor Demo { get; set; }

        private static System.Threading.Mutex _mutex = new System.Threading.Mutex();
        
        public Core(string configurationFilename)
        {
            _logger = new Logger();
            _configuration = new Configuration();

            if(!_configuration.LoadFromFile(configurationFilename))
            {
                throw new System.IO.FileLoadException();
            }

            Arm = new ArmController();
            Arm.ReadXml();

            Transporter = new TransporterController();
            Transporter.ReadXml();

            Loader = new LoadController();
            Loader.ReadXml();

            Rotor = new RotorController();
            Rotor.ReadXml();

            Pomp = new PompController();
            Pomp.ReadXml();

            Demo = new DemoExecutor();
            Demo.ReadXml();
            
            PackReceiver = new PackageReceiver(Protocol.PacketHeader, Protocol.PacketEnd);
            PackHandler = new PackageHandler();

            PackReceiver.PackageReceived += PackHandler.ProcessPacket;
            
            Serial = new SerialHelper(PackReceiver);
            CNCExecutor = new CncExecutor();
            Executor = new TaskExecutor();
            
            Logger.AddMessage("Запись работы системы начата");
        }
        
        public void SaveSettings()
        {
            Arm.WriteXml();
            Transporter.WriteXml();
            Loader.WriteXml();
            Rotor.WriteXml();
            Pomp.WriteXml();
            Demo.WriteXml();
        }

        public static Logger GetLogger()
        {
            return _logger;
        }

        public static Configuration GetConfig()
        {
            return _configuration;
        }

        private static ushort[] _sensorsValues = null;

        public static void InitSensorsValues()
        {
            _sensorsValues = new ushort[_configuration.Sensors.Count];
        }

        public static void UpdateSensorsValues(ushort[] values)
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

        public static void UpdateBarCode(string barCode)
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
