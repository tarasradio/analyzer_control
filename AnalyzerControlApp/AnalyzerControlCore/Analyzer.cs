using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.SerialCommunication;
using AnalyzerConfiguration;
using AnalyzerService;
using AnalyzerService.ExecutionControl;
using AnalyzerService.Units;
using Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AnalyzerService
{
    public class Analyzer : Configurable<AnalyzerServiceConfiguration>
    {
        public static IAnalyzerState State { get; private set; }

        public static IPacketFinder PackFinder { get; private set; }
        public static IPacketHandler PackHandler { get; private set; }
        public static ResponseHandler ResponseHandler { get; set; }
        public static ISerialAdapter Serial { get; private set; }

        public static ICommandExecutor CommandExecutor { get; private set; }
        public static TaskExecutor TaskExecutor { get; private set; }

        public static NeedleUnit Needle { get; private set; }
        public static ConveyorUnit Conveyor { get; private set; }
        public static RotorUnit Rotor { get; private set; }
        public static ChargerUnit Charger { get; private set; }
        public static PompUnit Pomp { get; private set; }

        public Analyzer(IConfigurationProvider provider) : base(provider)
        {
            provider = new XmlConfigurationProvider();
            
            CommandExecutor = new CommandExecutor();
            TaskExecutor = new TaskExecutor();
            
            Needle = new NeedleUnit(CommandExecutor, provider);
            Conveyor = new ConveyorUnit(CommandExecutor, provider);
            Charger = new ChargerUnit(CommandExecutor, provider);
            Rotor = new RotorUnit(CommandExecutor, provider);
            Pomp = new PompUnit(CommandExecutor, provider);

            LoadConfiguration("AnalyzerServiceConfiguration");
            LoadUnitsConfiguration();
            
            SerialCommunicationInit();

            Logger.Info("Запись работы системы начата");
        }

        private void SerialCommunicationInit()
        {
            PackHandler = new PacketHandler();
            PackFinder = new PacketFinder(PackHandler);
            Serial = new SerialAdapter(PackFinder);
            
            State = new AnalyzerState(Options.Sensors.Count, Options.Steppers.Count);

            ResponseHandler = new ResponseHandler(PackHandler, State);

            ResponseHandler.DebugMessageReceived += Logger.Info;
            ResponseHandler.CommandStateReceived += CommandExecutor.UpdateExecutedCommandInfo;
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

        public static void AbortExecution()
        {
            TaskExecutor.AbortExecution();
            CommandExecutor.AbortExecution();
            //Demo.AbortWork();

            Serial.SendPacket(new AbortExecutionCommand().GetBytes());
        }
    }
}
