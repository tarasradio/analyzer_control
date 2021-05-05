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

        RelayCommand _wtfCommand;

        public RelayCommand WtfCommand
        {
            get
            {
                if (_wtfCommand == null)
                {
                    _wtfCommand = new RelayCommand(
                       param => { wtf(); },
                       param => { return (ConveyorCells != null); });
                }
                return _wtfCommand;
            }
        }

        public void wtf()
        {
            Cassettes[1].CountLeft = 6;
            Cassettes[4].CountLeft = 2 ;
            Cassettes[5].CountLeft = 3 ;

            if (ConveyorCells[4].State == ConveyorCellState.Empty)
            {
                ConveyorCells[4].State = ConveyorCellState.Error;
            } else
            {
                ConveyorCells[4].State = ConveyorCellState.Empty;
            }
        }

        static IConfigurationProvider provider = new XmlConfigurationProvider();
        static Analyzer analyzer = null;
        static ConveyorService conveyor = null;
        static AnalyzerDemoController demoController = null;

        const string controllerFileName = "DemoControllerConfiguration";

        public AnalyzerControlViewModel()
        {
            InitCustomControls();

            Logger.NewInfoMessageAdded += Logger_NewInfoMessageAdded;
            Logger.NewControllerInfoMessageAdded += Logger_NewControllerInfoMessageAdded;
            Logger.NewDemoInfoMessageAdded += Logger_NewDemoInfoMessageAdded;

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
                Logger.Info("Возникла ошибка при запуске!");
            }
        }

        private void UpdateConnectionState(bool state)
        {
            if (state) {
                ConnectionState = "Соединение установлено";
            } else {
                ConnectionState = "Соединение не установлено";
            }
        }

        private void Logger_NewDemoInfoMessageAdded(string message)
        {
            LogText += $"{ message }";
        }

        private void Logger_NewControllerInfoMessageAdded(string message)
        {
            LogText += $"{ message }";
        }

        private void Logger_NewInfoMessageAdded(string message)
        {
            LogText += $"{ message }";
        }

        private string _logText;

        public string LogText {
            get => _logText;
            set {
                _logText = value;
                NotifyPropertyChanged();
            }
        }

        private string _connectionState;

        public string ConnectionState {
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
