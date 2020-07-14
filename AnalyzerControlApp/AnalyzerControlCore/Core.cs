using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.SerialCommunication;
using AnalyzerConfiguration;
using AnalyzerControlCore.Units;
using AnalyzerControlCore.MachineControl;
using Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;
using AnalyzerConfiguration.ControllersConfiguration;

namespace AnalyzerControlCore
{
    public class Core
    {
        private static string FirmwareVersion = "04.03.2020";
        
        public static PacketFinder PackFinder { get; private set; } = new PacketFinder();
        public static PacketHandler PackHandler { get; private set; } = new PacketHandler();
        public static SerialAdapter Serial { get; private set; }

        public static CommandExecutor CmdExecutor { get; private set; }
        public static TaskExecutor Executor { get; private set; }

        public static NeedleUnit Needle { get; private set; }
        public static TransporterUnit Transporter { get; private set; }
        public static RotorUnit Rotor { get; private set; }
        public static ChargeUnit Charger { get; private set; }
        public static PompUnit Pomp { get; private set; }

        public static DemoController Demo { get; private set; }

        private static IConfigurationProvider<NeedleControllerConfiguration> needleProvider;
        private static IConfigurationProvider<TransporterControllerConfiguration> transporterProvider;
        private static IConfigurationProvider<RotorControllerConfiguration> rotorProvider;
        private static IConfigurationProvider<ChargeControllerConfiguration> chargeProvider;
        private static IConfigurationProvider<PompControllerConfiguration> pompProvider;
        
        private static IConfigurationProvider<DemoControllerConfiguration> demoProvider;

        private static IConfigurationProvider<AnalyzerAppConfiguration> appProvider;

        private static object locker = new object();

        private static ushort[] sensorsValues = null;
        private static ushort[] steppersStates = null;

        private static string lastTubeBarCode = String.Empty;
        private static string lastCartridgeBarCode = String.Empty;
        private static string lastFirmwareVersionResponse = String.Empty;
        private static string configurationPath = String.Empty;
        
        public static AnalyzerAppConfiguration AppConfig { get; private set; }

        public static void SaveConfiguration(string path)
        {
            appProvider.SaveConfiguration(AppConfig, Path.Combine(path, nameof(AnalyzerAppConfiguration)));
        }

        public static void LoadConfiguration(string path)
        {
            try
            {
                AppConfig = appProvider.LoadConfiguration(Path.Combine(path, nameof(AnalyzerAppConfiguration)));
            }
            catch(FileNotFoundException)
            {
                AppConfig = new AnalyzerAppConfiguration();
            }
        }

        public Core(string configurationPath)
        {
            AppConfig = new AnalyzerAppConfiguration();

            // Создаем провайдеры конфигурации для приложения и для всех контроллеров
            appProvider = new XmlConfigurationProvider<AnalyzerAppConfiguration>();
            demoProvider = new XmlConfigurationProvider<DemoControllerConfiguration>();
            needleProvider = new XmlConfigurationProvider<NeedleControllerConfiguration>();
            transporterProvider = new XmlConfigurationProvider<TransporterControllerConfiguration>();
            rotorProvider = new XmlConfigurationProvider<RotorControllerConfiguration>();
            chargeProvider = new XmlConfigurationProvider<ChargeControllerConfiguration>();
            pompProvider = new XmlConfigurationProvider<PompControllerConfiguration>();

            LoadConfiguration(configurationPath);

            sensorsValues = new ushort[AppConfig.Sensors.Count];
            steppersStates = new ushort[AppConfig.Steppers.Count];

            Core.configurationPath = Path.GetDirectoryName(configurationPath);
            
            PackFinder.PacketReceived += PackHandler.ProcessPacket;

            Serial = new SerialAdapter(PackFinder);

            PackHandler.TubeBarCodeReceived += PackHandler_TubeBarCodeReceived;
            PackHandler.CartridgeBarCodeReceived += PackHandler_CartridgeBarCodeReceived;

            PackHandler.FirmwareVersionReceived += PackHandler_FirmwareVersionReceived;
            PackHandler.SensorsValuesReceived += PackHandler_SensorsValuesReceived;
            PackHandler.SteppersStatesReceived += PackHandler_SteppersStatesReceived;

            PackHandler.DebugMessageReceived += Logger.Info;

            CmdExecutor = new CommandExecutor();
            Executor = new TaskExecutor();

            Needle = new NeedleUnit(CmdExecutor); Needle.SetProvider(needleProvider);
            Transporter = new TransporterUnit(CmdExecutor); Transporter.SetProvider(transporterProvider);
            Charger = new ChargeUnit(CmdExecutor); Charger.SetProvider(chargeProvider);
            Rotor = new RotorUnit(CmdExecutor); Rotor.SetProvider(rotorProvider);
            Pomp = new PompUnit(CmdExecutor); Pomp.SetProvider(pompProvider);
            Demo = new DemoController(); Demo.SetProvider(demoProvider);

            try
            {
                Needle.LoadConfiguration(configurationPath);
                Transporter.LoadConfiguration(configurationPath);
                Charger.LoadConfiguration(configurationPath);
                Rotor.LoadConfiguration(configurationPath);
                Pomp.LoadConfiguration(configurationPath);
                Demo.LoadConfiguration(configurationPath);
            }
            catch(FileNotFoundException)
            {
                throw new FileLoadException();
            }

            PackHandler.CommandStateReceived += CmdExecutor.UpdateExecutedCommandState;
            
            Logger.Info("Запись работы системы начата");
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

        public void SaveConfiguration()
        {
            Needle.SaveConfiguration(configurationPath);
            Transporter.SaveConfiguration(configurationPath);
            Charger.SaveConfiguration(configurationPath);
            Rotor.SaveConfiguration(configurationPath);
            Pomp.SaveConfiguration(configurationPath);
            Demo.SaveConfiguration(configurationPath);
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
