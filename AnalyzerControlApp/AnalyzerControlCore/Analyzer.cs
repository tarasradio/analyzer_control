using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.SerialCommunication;
using AnalyzerConfiguration;
using AnalyzerService.ExecutionControl;
using AnalyzerService.Units;
using Infrastructure;
using System;

namespace AnalyzerService
{
    public class Analyzer : Configurable<AnalyzerServiceConfiguration>
    {
        public static IAnalyzerState State { get; private set; }

        public static IPacketFinder PackFinder { get; private set; }
        public static IPacketHandler PackHandler { get; private set; }
        public static ResponseHandler ResponseHandler { get; set; }
        public static ISerialAdapter Serial { get; private set; }

        public static ConnectionService CommunicationService { get; private set; }

        public static ICommandExecutor CommandExecutor { get; private set; }
        public static TaskExecutor TaskExecutor { get; private set; }

        public static NeedleUnit Needle { get; private set; }
        public static ConveyorUnit Conveyor { get; private set; }
        public static RotorUnit Rotor { get; private set; }
        public static ChargerUnit Charger { get; private set; }
        public static PompUnit Pomp { get; private set; }
        public static AdditionalDevicesUnit AdditionalDevices { get; private set; }

        public static string ServerAddress { get; private set; }
        public static int ServerPort { get; private set; }

        public Analyzer(IConfigurationProvider provider) : base(provider)
        {
            this.provider = provider;
            
            CommandExecutor = new CommandExecutor();
            TaskExecutor = new TaskExecutor();
            
            Needle = new NeedleUnit(CommandExecutor, provider);
            Conveyor = new ConveyorUnit(CommandExecutor, provider);
            Charger = new ChargerUnit(CommandExecutor, provider);
            Rotor = new RotorUnit(CommandExecutor, provider);
            Pomp = new PompUnit(CommandExecutor, provider);
            AdditionalDevices = new AdditionalDevicesUnit(CommandExecutor, provider);

            CommunicationService = new ConnectionService();

            LoadConfiguration("AnalyzerServiceConfiguration");
            LoadUnitsConfiguration();
            
            SerialCommunicationInit();
            SerialCommunicationOpen();

            ServerAddress = Options.ServerAddress;
            ServerPort = Options.ServerPort;

            Logger.Debug("Запись работы системы начата");
        }

        private void SerialCommunicationOpen()
        {
            CommunicationService.Run();
            CommunicationService.DeviceConnectionChanged += onDeviceConnectionChanged;
        }

        private void onDeviceConnectionChanged(bool connected)
        {
            if(connected) {
                Serial.Open(Options.PortName, Options.Baudrate);
            } else {
                Serial.Close();
            }
        }

        private void SerialCommunicationInit()
        {
            PackHandler = new PacketHandler();
            PackFinder = new PacketFinder(PackHandler);
            Serial = new SerialAdapter(PackFinder);
            
            //State = new AnalyzerState(Options.Sensors.Count, Options.Steppers.Count);
            State = new AnalyzerState(Options.Sensors.Count, 17);

            ResponseHandler = new ResponseHandler(PackHandler, State);

            ResponseHandler.DebugMessageReceived += Logger.Debug;
            ResponseHandler.CommandStateReceived += CommandExecutor.UpdateExecutedCommandInfo;
        }

        public void LoadUnitsConfiguration()
        {
            Needle.LoadConfiguration("NeedleConfiguration");
            Conveyor.LoadConfiguration("ConveyorConfiguration");
            Charger.LoadConfiguration("ChargerConfiguration");
            Rotor.LoadConfiguration("RotorConfiguration");
            Pomp.LoadConfiguration("PompConfiguration");
            AdditionalDevices.LoadConfiguration("AdditionalDevicesConfiguration");
        }

        public void SaveUnitsConfiguration()
        {
            Needle.SaveConfiguration("NeedleConfiguration");
            Conveyor.SaveConfiguration("ConveyorConfiguration");
            Charger.SaveConfiguration("ChargerConfiguration");
            Rotor.SaveConfiguration("RotorConfiguration");
            Pomp.SaveConfiguration("PompConfiguration");
            AdditionalDevices.SaveConfiguration("AdditionalDevicesConfiguration");
        }

        public static void AbortExecution()
        {
            TaskExecutor.AbortExecution();
            CommandExecutor.AbortExecution();

            Serial.SendPacket(new AbortExecutionCommand().GetBytes());
        }
    }
}
