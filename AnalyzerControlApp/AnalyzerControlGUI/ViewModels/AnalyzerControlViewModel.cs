using AnalyzerConfiguration;
using AnalyzerControl;
using AnalyzerControl.Services;
using AnalyzerControlGUI.Commands;
using AnalyzerControlGUI.Models;
using AnalyzerControlGUI.Views;
using AnalyzerService;
using Infrastructure;
using System.Collections.ObjectModel;
using ConveyorCell = AnalyzerControlGUI.Models.ConveyorCell;

namespace AnalyzerControlGUI.ViewModels
{
    public class AnalyzerControlViewModel : ViewModel
    {
        private const int cassettesCount = 10;
        private const int conveyorCellsCount = 55;

        public ObservableCollection<Cassette> Cassettes { get; set; }
        public ObservableCollection<ConveyorCell> ConveyorCells { get; set; }

        RelayCommand _LoadCommand;

        public RelayCommand LoadCommand {
            get {
                if (_LoadCommand == null) {
                    _LoadCommand = new RelayCommand(
                       param => { load(); },
                       param => { return ConnectionState; });
                }
                return _LoadCommand;
            }
        }

        public void load()
        {
            Logger.Debug($"Загрузка...");
            Logger.Info($"Загрузка...");
        }

        RelayCommand _UnloadCommand;

        public RelayCommand UnloadCommand {
            get {
                if (_UnloadCommand == null) {
                    _UnloadCommand = new RelayCommand(
                       param => { unload(); },
                       param => { return ConnectionState; });
                }
                return _UnloadCommand;
            }
        }

        public void unload()
        {
            Logger.Debug($"Выгрузка...");
            Logger.Info($"Выгрузка...");
        }

        RelayCommand _AbortCommand;

        public RelayCommand AbortCommand {
            get {
                if (_AbortCommand == null) {
                    _AbortCommand = new RelayCommand(
                       param => { abort(); },
                       param => { return ConnectionState; });
                }
                return _AbortCommand;
            }
        }

        public void abort()
        {
            Logger.Debug($"Остановка работы...");
            Logger.Info($"Работа была прервана!");
            Analyzer.AbortExecution();
            demoController.AbortWork();
        }

        RelayCommand _ResumeCommand;

        public RelayCommand ResumeCommand {
            get {
                if (_ResumeCommand == null) {
                    _ResumeCommand = new RelayCommand(
                       param => { resume(); },
                       param => { return ConnectionState; });
                }
                return _ResumeCommand;
            }
        }

        public void resume()
        {
            Logger.Debug($"Продолжение работы...");
        }

        public void wtf()
        {
            //Cassettes[1].CountLeft = 6;
            //Cassettes[4].CountLeft = 2 ;
            //Cassettes[5].CountLeft = 3 ;

            //if (ConveyorCells[4].State == ConveyorCellState.Empty)
            //{
            //    ConveyorCells[4].State = ConveyorCellState.Error;
            //} else
            //{
            //    ConveyorCells[4].State = ConveyorCellState.Empty;
            //}
        }

        static IConfigurationProvider provider = new XmlConfigurationProvider();
        static Analyzer analyzer = null;
        static ConveyorService conveyor = null;
        static AnalyzerDemoController demoController = null;

        const string controllerFileName = "DemoControllerConfiguration";

        public AnalyzerControlViewModel()
        {
            InitCustomControls();

            Logger.InfoMessageAdded += onInfoMessageAdded;
            Logger.DebugMessageAdded += onDebugMessageAdded;

            tryCreateController();

            UpdateConnectionState(Analyzer.Serial.IsOpen());
        }

        private void tryCreateController()
        {
            try {
                analyzer = new Analyzer(provider);
                conveyor = new ConveyorService(54);
                demoController = new AnalyzerDemoController(provider, conveyor);
                demoController.LoadConfiguration(controllerFileName);

                Analyzer.Serial.ConnectionChanged += UpdateConnectionState;
            } catch {
                Logger.Debug("Возникла ошибка при запуске!");
            }
        }

        private void UpdateConnectionState(bool state)
        {
            ConnectionState = state;
            if (state) {
                ConnectionText = "Соединение установлено";
            } else {
                ConnectionText = "Соединение не установлено";
            }
        }

        private void onDebugMessageAdded(string message)
        {
            DebugText += $"{ message }";
        }

        private void onInfoMessageAdded(string message)
        {
            InformationText = message;
        }

        private string _debugText;

        public string DebugText {
            get => _debugText;
            set {
                _debugText = value;
                NotifyPropertyChanged();
            }
        }

        private string _informationText;

        public string InformationText
        {
            get => _informationText;
            set {
                _informationText = value;
                NotifyPropertyChanged();
            }
        }

        private string _connectionText;

        public string ConnectionText {
            get => _connectionText;
            set {
                _connectionText = value;
                NotifyPropertyChanged();
            }
        }

        private bool _connectionState;

        public bool ConnectionState
        {
            get => _connectionState;
            set {
                _connectionState = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _connectionCommand;

        public RelayCommand ConnectionCommand
        {
            get {
                return _connectionCommand ??
                   (_connectionCommand = new RelayCommand(obj =>
                   {
                       ConnectionViewModel viewModel = new ConnectionViewModel(Analyzer.Serial);
                       ConnectionWindow connectionWindow = new ConnectionWindow();
                       connectionWindow.DataContext = viewModel;

                       connectionWindow.ShowDialog();
                   }));
            }
        }

        private void InitCustomControls()
        {
            Cassettes = new ObservableCollection<Cassette>
            {
                new Cassette { Barcode="12", CountLeft = 1 },
                new Cassette { Barcode="13", CountLeft = 2 },
                new Cassette { Barcode="14", CountLeft = 3 },
                new Cassette { Barcode="15", CountLeft = 4 },
                new Cassette { Barcode="16", CountLeft = 5 },
                new Cassette { Barcode="17", CountLeft = 6 },
                new Cassette { Barcode="18", CountLeft = 7 },
                new Cassette { Barcode="19", CountLeft = 8 },
                new Cassette { Barcode="20", CountLeft = 9 },
                new Cassette { Barcode="21", CountLeft = 10 },
            };

            ConveyorCells = new ObservableCollection<ConveyorCell>();

            for (int i = 0; i < conveyorCellsCount; ++i)
                ConveyorCells.Add(new ConveyorCell());

            ConveyorCells[5].State = ConveyorCellState.Processed;
            ConveyorCells[6].State = ConveyorCellState.Error;
            ConveyorCells[7].State = ConveyorCellState.Processing;
        }
    }
}
