using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.SerialCommunication;
using AnalyzerConfiguration;
using AnalyzerService.MachineControl;
using AnalyzerService.Units;
using Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AnalyzerService
{
    public class Analyzer
    {
        private static string FirmwareVersion = "04.03.2020";
        
        public static IPacketFinder PackFinder { get; private set; }
        public static IPacketHandler PackHandler { get; private set; }
        public static ISerialAdapter Serial { get; private set; }

        public static ICommandExecutor CmdExecutor { get; private set; }
        public static TaskExecutor Executor { get; private set; }

        public static NeedleUnit Needle { get; private set; }
        public static ConveyorUnit Conveyor { get; private set; }
        public static RotorUnit Rotor { get; private set; }
        public static ChargerUnit Charger { get; private set; }
        public static PompUnit Pomp { get; private set; }

        private static IConfigurationProvider provider;

        public static IAnalyzerContext Context { get; private set; }

        public static AnalyzerAppConfiguration AppConfig { get; private set; }

        public Analyzer()
        {
            provider = new XmlConfigurationProvider();
            
            CmdExecutor = new CommandExecutor();
            Executor = new TaskExecutor();
            
            Needle = new NeedleUnit(CmdExecutor, provider);
            Conveyor = new ConveyorUnit(CmdExecutor, provider);
            Charger = new ChargerUnit(CmdExecutor, provider);
            Rotor = new RotorUnit(CmdExecutor, provider);
            Pomp = new PompUnit(CmdExecutor, provider);

            LoadAppConfiguration();
            LoadUnitsConfiguration();
            
            SerialCommunicationInit();

            Logger.Info("Запись работы системы начата");
        }

        private void SerialCommunicationInit()
        {
            Context = new AnalyzerContext(AppConfig.Sensors.Count, AppConfig.Steppers.Count);

            PackHandler = new PacketHandler(Context);
            PackFinder = new PacketFinder(PackHandler);
            Serial = new SerialAdapter(PackFinder);

            PackHandler.DebugMessageReceived += Logger.Info;
            PackHandler.CommandStateReceived += CmdExecutor.UpdateExecutedCommandInfo;
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

        public void LoadUnitsConfiguration()
        {
            Needle.LoadConfiguration("NeedleConfiguration");
            Conveyor.LoadConfiguration("ConveyorConfiguration");
            Charger.LoadConfiguration("ChargerConfiguration");
            Rotor.LoadConfiguration("RotorConfiguration");
            Pomp.LoadConfiguration("PompConfiguration");
        }

        public void SaveUnitsConfiguration()
        {
            Needle.SaveConfiguration("NeedleConfiguration");
            Conveyor.SaveConfiguration("ConveyorConfiguration");
            Charger.SaveConfiguration("ChargerConfiguration");
            Rotor.SaveConfiguration("RotorConfiguration");
            Pomp.SaveConfiguration("PompConfiguration");
        }
        
        public async static void CheckFirmwareVersion(string firmwareVersion)
        {
            for(int i = 0; i < 3; i++)
            {
                Serial.SendPacket(new GetFirmwareVersionCommand().GetBytes());
                await Task.Delay(100);
            }

            await Task.Run( async()=>{
                await Task.Delay(1000);

                bool received = string.IsNullOrWhiteSpace(firmwareVersion);

                if(received)
                {
                    if (string.Equals(FirmwareVersion, firmwareVersion))
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

        public static void AbortExecution()
        {
            Executor.AbortExecution();
            CmdExecutor.AbortExecution();
            //Demo.AbortWork();

            Serial.SendPacket(new AbortExecutionCommand().GetBytes());
        }
    }
}
