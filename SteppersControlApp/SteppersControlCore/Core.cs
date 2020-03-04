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
        private static string FirmwareVersion = "04.03.2020";
        public static Configuration Settings { get; private set; }

        public static PacketFinder PackFinder { get; private set; } = new PacketFinder();
        public static PackageHandler PackHandler { get; private set; } = new PackageHandler();
        public static SerialHelper Serial { get; private set; }

        public static CommandExecutor CmdExecutor { get; private set; }
        public static TaskExecutor Executor { get; private set; }

        public static NeedleController Needle { get; private set; }
        public static TransporterController Transporter { get; private set; }
        public static RotorController Rotor { get; private set; }
        public static ChargeController Charger { get; private set; }
        public static PompController Pomp { get; private set; }

        public static DemoController Demo { get; private set; }

        private static object locker = new object();

        private static ushort[] _sensorsValues = null;
        private static ushort[] _steppersStates = null;

        private static string lastTubeBarCode = String.Empty;
        private static string lastCartridgeBarCode = String.Empty;
        private static string lastFirmwareVersionResponse = String.Empty;
        private static string path = String.Empty;

        public Core(string configurationFilename)
        {
            Settings = new Configuration();

            if(!Settings.LoadFromFile(configurationFilename))
            {
                throw new System.IO.FileLoadException();
            }

            _sensorsValues = new ushort[Settings.Sensors.Count];
            _steppersStates = new ushort[Settings.Steppers.Count];

            path = System.IO.Path.GetDirectoryName(configurationFilename);
            
            PackFinder.PacketReceived += PackHandler.ProcessPacket;

            Serial = new SerialHelper(PackFinder);

            PackHandler.TubeBarCodeReceived += PackHandler_TubeBarCodeReceived;
            PackHandler.CartridgeBarCodeReceived += PackHandler_CartridgeBarCodeReceived;

            PackHandler.FirmwareVersionReceived += PackHandler_FirmwareVersionReceived;
            PackHandler.SensorsValuesReceived += PackHandler_SensorsValuesReceived;
            PackHandler.SteppersStatesReceived += PackHandler_SteppersStatesReceived;

            PackHandler.MessageReceived += Logger.AddMessage;

            CmdExecutor = new CommandExecutor();
            Executor = new TaskExecutor();

            Needle = new NeedleController(CmdExecutor);
            Needle.ReadXml(path);

            Transporter = new TransporterController(CmdExecutor);
            Transporter.ReadXml(path);

            Charger = new ChargeController(CmdExecutor);
            Charger.ReadXml(path);

            Rotor = new RotorController(CmdExecutor);
            Rotor.ReadXml(path);

            Pomp = new PompController(CmdExecutor);
            Pomp.ReadXml(path);

            Demo = new DemoController();
            Demo.ReadXml(path);

            PackHandler.CommandStateResponseReceived += PackHandler_CommandStateResponseReceived;
            
            Logger.Info("Запись работы системы начата");
        }

        private void PackHandler_TubeBarCodeReceived(string message)
        {
            lock (locker)
            {
                lastTubeBarCode = message;
            }
        }

        private void PackHandler_CartridgeBarCodeReceived(string message)
        {
            lock (locker)
            {
                lastCartridgeBarCode = message;
            }
        }

        private void PackHandler_CommandStateResponseReceived(uint commandId, Protocol.CommandStates state)
        {
            CmdExecutor.UpdateExecutedCommandState(commandId, state);
        }

        private void PackHandler_SteppersStatesReceived(ushort[] states)
        {
            if (states.Length != Settings.Steppers.Count)
                return;
            lock (locker)
            {
                Array.Copy(states, _steppersStates, states.Length);
            }
        }

        private void PackHandler_SensorsValuesReceived(ushort[] states)
        {
            if (states.Length != Settings.Sensors.Count)
                return;
            lock (locker)
            {
                Array.Copy(states, _sensorsValues, states.Length);
            }
        }

        private void PackHandler_FirmwareVersionReceived(string message)
        {
            lastFirmwareVersionResponse = message;
        }

        public void SaveConfiguration()
        {
            Needle.WriteXml(path);
            Transporter.WriteXml(path);
            Charger.WriteXml(path);
            Rotor.WriteXml(path);
            Pomp.WriteXml(path);
            Demo.WriteXml(path);
        }
        
        public async static void CheckFirmwareVersion()
        {
            lastFirmwareVersionResponse = String.Empty;

            for(int i = 0; i < 3; i++)
            {
                Serial.SendPacket(new GetFirmwareVersionCommand().GetBytes());
                await Task.Delay(100);
            }

            await Task.Run( async()=>{
                await Task.Delay(1000);

                bool received = String.IsNullOrWhiteSpace(lastFirmwareVersionResponse);

                if(received)
                {
                    if (String.Equals(FirmwareVersion, lastFirmwareVersionResponse))
                    {
                        Logger.Info("[System] - Версия платы совпадает с требуемой.");
                    }
                    else
                    {
                        Logger.Info("[System] - Версия платы не совпадает с требуемой. " +
                            "Подключено несовместимое устройство или требуется обновить прошивку. ");
                    }
                }
                else
                {
                    Logger.Info("[System] - Версия платы не совпадает с требуемой. " +
                            "Подключено несовместимое устройство или требуется обновить прошивку. ");
                }
            });
        }

        public static void UpdateSensorsValues(ushort[] values)
        {
            if (values.Length != Settings.Sensors.Count)
                return;
            lock(locker)
            {
                Array.Copy(values, _sensorsValues, values.Length);
            }
        }

        public static ushort[] GetSteppersStates()
        {
            ushort[] states = new ushort[Settings.Steppers.Count];

            lock (locker)
            {
                Array.Copy(_steppersStates, states, _steppersStates.Length);
            }

            return states;
        }

        public static ushort[] GetSensorsValues()
        {
            ushort[] values = new ushort[Settings.Sensors.Count];

            lock(locker)
            {
                Array.Copy(_sensorsValues, values, _sensorsValues.Length);
            }

            return values;
        }

        public static ushort GetSensorValue(uint sensor)
        {
            ushort value = 0;

            lock(locker)
            {
                value = _sensorsValues[sensor];
            }

            return value;
        }
        
        public static string GetLastTubeBarCode()
        {
            string barCode;

            lock(locker)
            {
                barCode = lastTubeBarCode;
                lastTubeBarCode = String.Empty;
            }

            return barCode;
        }

        public static string GetLastCartridgeBarCode()
        {
            string barCode;

            lock (locker)
            {
                barCode = lastCartridgeBarCode;
                lastCartridgeBarCode = String.Empty;
            }

            return barCode;
        }

        public static void AbortExecution()
        {
            Executor.AbortExecution();
            CmdExecutor.AbortExecution();
            Demo.AbortExecution();

            Serial.SendPacket(new AbortExecutionCommand().GetBytes());
        }
    }
}
