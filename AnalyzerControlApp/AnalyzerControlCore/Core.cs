using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.SerialCommunication;
using AnalyzerConfiguration;
using AnalyzerControlCore.MachineControl;
using AnalyzerControlCore.Units;
using Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AnalyzerControlCore
{
    public class Core
    {
        private static string FirmwareVersion = "04.03.2020";
        
        public static IPacketFinder PackFinder { get; private set; }
        public static IPacketHandler PackHandler { get; private set; }
        public static ISerialAdapter Serial { get; private set; }

        public static CommandExecutor CmdExecutor { get; private set; }
        public static TaskExecutor Executor { get; private set; }

        public static NeedleUnit Needle { get; private set; }
        public static ConveyorUnit Conveyor { get; private set; }
        public static RotorUnit Rotor { get; private set; }
        public static ChargerUnit Charger { get; private set; }
        public static PompUnit Pomp { get; private set; }

        public static DemoController Demo { get; private set; }

        private static IConfigurationProvider provider;

        private static object locker = new object();

        private static ushort[] sensorsValues = null;
        private static ushort[] steppersStates = null;

        private static string lastTubeBarCode = String.Empty;
        private static string lastCartridgeBarCode = String.Empty;
        private static string lastFirmwareVersionResponse = String.Empty;
        
        public static AnalyzerAppConfiguration AppConfig { get; private set; }

        public Core()
        {
            provider = new XmlConfigurationProvider();
            
            CmdExecutor = new CommandExecutor();
            Executor = new TaskExecutor();
            
            Needle = new NeedleUnit(CmdExecutor, provider);
            Conveyor = new ConveyorUnit(CmdExecutor, provider);
            Charger = new ChargerUnit(CmdExecutor, provider);
            Rotor = new RotorUnit(CmdExecutor, provider);
            Pomp = new PompUnit(CmdExecutor, provider);

            Demo = new DemoController(provider);
            
            AppConfig = new AnalyzerAppConfiguration();

            LoadAppConfiguration();
            LoadUnitsConfiguration();

            sensorsValues = new ushort[AppConfig.Sensors.Count];
            steppersStates = new ushort[AppConfig.Steppers.Count];

            SerialCommunicationInit();

            Logger.Info("Запись работы системы начата");
        }

        private void SerialCommunicationInit()
        {
            PackHandler = new PacketHandler();
            PackFinder = new PacketFinder(PackHandler);
            Serial = new SerialAdapter(PackFinder);

            PackHandler.TubeBarCodeReceived += PackHandler_TubeBarCodeReceived;
            PackHandler.CartridgeBarCodeReceived += PackHandler_CartridgeBarCodeReceived;

            PackHandler.FirmwareVersionReceived += PackHandler_FirmwareVersionReceived;
            PackHandler.SensorsValuesReceived += PackHandler_SensorsValuesReceived;
            PackHandler.SteppersStatesReceived += PackHandler_SteppersStatesReceived;

            PackHandler.DebugMessageReceived += Logger.Info;
            PackHandler.CommandStateReceived += CmdExecutor.UpdateExecutedCommandState;
        }

        public static void SaveAppConfiguration()
        {
            try
            {
                provider.SaveConfiguration(AppConfig, "AppConfiguration");
            }
            catch (Exception exeption)
            {
                throw new IOException("Ошибка при сохранении файла конфигурации.", innerException: exeption);
            }
        }

        public static void LoadAppConfiguration()
        {
            try
            {
                AppConfig = provider.LoadConfiguration<AnalyzerAppConfiguration>("AppConfiguration");
            }
            catch
            {
                Logger.Info($"Ошибка при загрузке файла конфигурации. Используется конфигурация по умолчанию.");
            }
        }

        private void PackHandler_TubeBarCodeReceived(string message)
        {
            lock (locker)
            {
                lastTubeBarCode = message;
                Logger.Info($"New tube barcode received: {message}");
            }
        }

        private void PackHandler_CartridgeBarCodeReceived(string message)
        {
            lock (locker)
            {
                lastCartridgeBarCode = message;
                Logger.Info($"New cartridge barcode received: {message}");
            }
        }

        private void PackHandler_SteppersStatesReceived(ushort[] states)
        {
            if (states.Length != AppConfig.Steppers.Count)
                return;
            lock (locker)
            {
                Array.Copy(states, steppersStates, states.Length);
            }
        }

        private void PackHandler_SensorsValuesReceived(ushort[] states)
        {
            if (states.Length != AppConfig.Sensors.Count)
                return;
            lock (locker)
            {
                Array.Copy(states, sensorsValues, states.Length);
            }
        }

        private void PackHandler_FirmwareVersionReceived(string message)
        {
            lastFirmwareVersionResponse = message;
        }

        public void LoadUnitsConfiguration()
        {
            Needle.LoadConfiguration("NeedleConfiguration");
            Conveyor.LoadConfiguration("ConveyorConfiguration");
            Charger.LoadConfiguration("ChargerConfiguration");
            Rotor.LoadConfiguration("RotorConfiguration");
            Pomp.LoadConfiguration("PompConfiguration");

            Demo.LoadConfiguration("DemoConfiguration");
        }

        public void SaveUnitsConfiguration()
        {
            Needle.SaveConfiguration("NeedleConfiguration");
            Conveyor.SaveConfiguration("ConveyorConfiguration");
            Charger.SaveConfiguration("ChargerConfiguration");
            Rotor.SaveConfiguration("RotorConfiguration");
            Pomp.SaveConfiguration("PompConfiguration");

            Demo.SaveConfiguration("DemoConfiguration");
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

                bool received = string.IsNullOrWhiteSpace(lastFirmwareVersionResponse);

                if(received)
                {
                    if (string.Equals(FirmwareVersion, lastFirmwareVersionResponse))
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
            if (values.Length != AppConfig.Sensors.Count)
                return;
            lock(locker)
            {
                Array.Copy(values, sensorsValues, values.Length);
            }
        }

        public static ushort[] GetSteppersStates()
        {
            ushort[] states = new ushort[AppConfig.Steppers.Count];

            lock (locker)
            {
                Array.Copy(steppersStates, states, steppersStates.Length);
            }

            return states;
        }

        public static ushort[] GetSensorsValues()
        {
            ushort[] values = new ushort[AppConfig.Sensors.Count];

            lock(locker)
            {
                Array.Copy(sensorsValues, values, sensorsValues.Length);
            }

            return values;
        }

        public static ushort GetSensorValue(uint sensor)
        {
            ushort value = 0;

            lock(locker)
            {
                value = sensorsValues[sensor];
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
