using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.AdditionalCommands;
using SteppersControlCore.Controllers;
using SteppersControlCore.MachineControl;
using SteppersControlCore.SerialCommunication;

namespace SteppersControlCore
{
    public class Core
    {
        private static string FirmwareVersion = "07.11.2019";
        public static Configuration Settings { get; private set; }

        public static PackageReceiver PackReceiver { get; private set; }
        public static PackageHandler PackHandler { get; private set; }
        public static SerialHelper Serial { get; private set; }
        public static CncExecutor CNCExecutor { get; private set; }
        public static TaskExecutor Executor { get; private set; }

        public static ArmController Arm { get; private set; }
        public static TransporterController Transporter { get; private set; }
        public static RotorController Rotor { get; private set; }
        public static LoadController Loader { get; private set; }
        public static PompController Pomp { get; private set; }

        public static DemoExecutor Demo { get; private set; }

        private static object _syncRoot = new object();

        private static ushort[] _sensorsValues = null;
        private static ushort[] _steppersStates = null;

        private static string _lastBarCode = null;
        private static string _lastFirmwareVersionResponse = null;

        public Core(string configurationFilename)
        {
            Settings = new Configuration();

            if(!Settings.LoadFromFile(configurationFilename))
            {
                throw new System.IO.FileLoadException();
            }

            _sensorsValues = new ushort[Settings.Sensors.Count];
            _steppersStates = new ushort[Settings.Steppers.Count];

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

            PackHandler.BarCodeReceived += PackHandler_BarCodeReceived;
            PackHandler.FirmwareVersionReceived += PackHandler_FirmwareVersionReceived;
            PackHandler.SensorsValuesReceived += PackHandler_SensorsValuesReceived;
            PackHandler.SteppersStatesReceived += PackHandler_SteppersStatesReceived;

            PackHandler.MessageReceived += Logger.AddMessage;

            CNCExecutor = new CncExecutor();
            Executor = new TaskExecutor();

            PackHandler.CommandStateResponseReceived += PackHandler_CommandStateResponseReceived;
            
            Logger.AddMessage("Запись работы системы начата");
        }

        private void PackHandler_CommandStateResponseReceived(uint commandId, Protocol.CommandStates state)
        {
            CNCExecutor.ChangeSuccesCommandId(commandId, state);
        }

        private void PackHandler_SteppersStatesReceived(ushort[] states)
        {
            if (states.Length != Settings.Steppers.Count)
                return;
            lock (_syncRoot)
            {
                Array.Copy(states, _steppersStates, states.Length);
            }
        }

        private void PackHandler_SensorsValuesReceived(ushort[] states)
        {
            if (states.Length != Settings.Sensors.Count)
                return;
            lock (_syncRoot)
            {
                Array.Copy(states, _sensorsValues, states.Length);
            }
        }

        private void PackHandler_FirmwareVersionReceived(string message)
        {
            _lastFirmwareVersionResponse = message;
        }

        private void PackHandler_BarCodeReceived(string message)
        {
            lock(_syncRoot)
            {
                _lastBarCode = message;
            }
        }

        public void SaveConfiguration()
        {
            Arm.WriteXml();
            Transporter.WriteXml();
            Loader.WriteXml();
            Rotor.WriteXml();
            Pomp.WriteXml();
            Demo.WriteXml();
        }
        
        public async static void CheckFirmwareVersion()
        {
            _lastFirmwareVersionResponse = null;
            for(int i = 0; i < 3; i++)
            {
                Serial.SendPacket(new GetFirmwareVersionCommand().GetBytes());
                await Task.Delay(100);
            }

            await Task.Run( async()=>{
                await Task.Delay(1000);

                bool received = _lastFirmwareVersionResponse != null;

                if(received)
                {
                    if (String.Equals(FirmwareVersion, _lastFirmwareVersionResponse))
                    {
                        Logger.AddMessage("Версия платы совпадает с требуемой");
                    }
                    else
                    {
                        Logger.AddMessage("Версия платы не совпадает с требуемой. " +
                            "Подключено несовместимое устройство или требуется обновить прошивку. ");
                    }
                }
                else
                {
                    Logger.AddMessage("Версия платы не совпадает с требуемой. " +
                            "Подключено несовместимое устройство или требуется обновить прошивку. ");
                }
            });
        }

        public static void UpdateSensorsValues(ushort[] values)
        {
            if (values.Length != Settings.Sensors.Count)
                return;
            lock(_syncRoot)
            {
                Array.Copy(values, _sensorsValues, values.Length);
            }
        }

        public static ushort[] GetSteppersStates()
        {
            ushort[] states = new ushort[Settings.Steppers.Count];

            lock (_syncRoot)
            {
                Array.Copy(_steppersStates, states, _steppersStates.Length);
            }

            return states;
        }

        public static ushort[] GetSensorsValues()
        {
            ushort[] values = new ushort[Settings.Sensors.Count];

            lock(_syncRoot)
            {
                Array.Copy(_sensorsValues, values, _sensorsValues.Length);
            }

            return values;
        }

        public static ushort GetSensorValue(uint sensor)
        {
            ushort value = 0;

            lock(_syncRoot)
            {
                value = _sensorsValues[sensor];
            }

            return value;
        }
        
        public static string GetLastBarCode()
        {
            string barCode = null;

            lock(_syncRoot)
            {
                barCode = _lastBarCode;
                _lastBarCode = null;
            }

            return barCode;
        }

        public static void AbortExecution()
        {
            Executor.AbortExecution();
            CNCExecutor.AbortExecution();
            Serial.SendPacket(new AbortExecutionCommand().GetBytes());
        }
    }
}
