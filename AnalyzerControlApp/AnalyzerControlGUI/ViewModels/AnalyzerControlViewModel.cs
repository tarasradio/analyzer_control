using AnalyzerConfiguration;
using AnalyzerControl;
using AnalyzerControl.Services;
using AnalyzerControlGUI.Commands;
using AnalyzerControlGUI.Models;
using AnalyzerControlGUI.Views;
using AnalyzerControlGUI.Views.DialogWindows;
using AnalyzerService;
using Infrastructure;
using System;
using System.Management;
using System.Collections.ObjectModel;
using ConveyorCell = AnalyzerControlGUI.Models.ConveyorCell;

namespace AnalyzerControlGUI.ViewModels
{
    public class AnalyzerControlViewModel : ViewModel
    {
        private const int cassettesCount = 10;
        private const int conveyorCellsCount = 54;

        public ObservableCollection<Cassette> Cassettes { get; set; }
        public ObservableCollection<ConveyorCell> ConveyorCells { get; set; }

        RelayCommand _morningCheckoutCommand;

        public RelayCommand MorningCheckoutCommand {
            get {
                if (_morningCheckoutCommand == null) {
                    _morningCheckoutCommand = new RelayCommand(
                       param => morningCheckout(),
                       param => canMorningCheckoutExecute()
                       );
                }
                return _morningCheckoutCommand;
            }
        }

        private bool canMorningCheckoutExecute()
        {
            return true;// ConnectionState;
        }

        private void morningCheckout()
        {
            MorningCheckoutViewModel viewModel = new MorningCheckoutViewModel();
            MorningChechoutWindow morningChechoutWindow = new MorningChechoutWindow();
            morningChechoutWindow.DataContext = viewModel;

            morningChechoutWindow.ShowDialog();
        }

        RelayCommand _eveningCheckoutCommand;

        public RelayCommand EveningCheckoutCommand
        {
            get
            {
                if (_eveningCheckoutCommand == null)
                {
                    _eveningCheckoutCommand = new RelayCommand(
                       param => eveningCheckout(),
                       param => canEveningCheckoutExecute()
                       );
                }
                return _eveningCheckoutCommand;
            }
        }

        private bool canEveningCheckoutExecute()
        {
            return true;
        }

        private void eveningCheckout()
        {
            EveningCheckoutViewModel viewModel = new EveningCheckoutViewModel();
            EveningCheckoutWindow eveningCheckoutWindow = new EveningCheckoutWindow();
            eveningCheckoutWindow.DataContext = viewModel;

            eveningCheckoutWindow.ShowDialog();
        }

        RelayCommand _startCommand;

        public RelayCommand StartCommand {
            get {
                if (_startCommand == null) {
                    _startCommand = new RelayCommand(
                       param => start(),
                       param => canStartExecute()
                       );
                }
                return _startCommand;
            }
        }

        private bool canStartExecute()
        {
            return ConnectionState;
        }

        private void start()
        {
            Logger.Debug($"Загрузка...");
            Logger.Info($"Загрузка...");

            demoController.StartWork();
        }

        RelayCommand _LoadCommand;

        public RelayCommand LoadCommand {
            get {
                if (_LoadCommand == null) {
                    _LoadCommand = new RelayCommand(
                       param => load(),
                       param => canLoadExecute()
                       );
                }
                return _LoadCommand;
            }
        }

        private bool canLoadExecute()
        {
            return (conveyor.State == ConveyorService.States.Waiting 
                || conveyor.State == ConveyorService.States.AnalyzesProcessing) 
                && ConnectionState;
        }

        private void load()
        {
            Logger.Debug($"Загрузка...");
            Logger.Info($"Загрузка...");

            conveyor.Load();
        }

        RelayCommand _UnloadCommand;

        public RelayCommand UnloadCommand {
            get {
                if (_UnloadCommand == null) {
                    _UnloadCommand = new RelayCommand(
                       param => unload(),
                       param => canUnloadExecute()
                       );
                }
                return _UnloadCommand;
            }
        }

        private bool canUnloadExecute()
        {
            return (conveyor.State == ConveyorService.States.Waiting
                || conveyor.State == ConveyorService.States.AnalyzesProcessing)
                && ConnectionState;
        }

        private void unload()
        {
            Logger.Debug($"Выгрузка...");
            Logger.Info($"Выгрузка...");

            conveyor.Unload();
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

        private void abort()
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
                       param => resume(),
                       param => canResumeExecute()
                       );
                }
                return _ResumeCommand;
            }
        }

        private bool canResumeExecute()
        {
            return (conveyor.State == ConveyorService.States.Waiting) 
                && ConnectionState;
        }

        private void resume()
        {
            Logger.Info($"Продолжение работы...");
            Logger.Debug($"Продолжение работы...");

            conveyor.Resume();
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
        static CartridgesDeckService cartridgesDeck = null;
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
                conveyor = new ConveyorService(conveyorCellsCount);
                cartridgesDeck = new CartridgesDeckService(cassettesCount);
                demoController = new AnalyzerDemoController(provider, conveyor);
                conveyor.SetController(demoController);
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

        private int _selectedCassette;
        public int SelectedCassette {
            get => _selectedCassette;
            set
            {
                _selectedCassette = value;
                if(ConnectionState)
                    scanCassette();
                NotifyPropertyChanged();
            }
        }

        // Сканирование кассеты
        private void scanCassette()
        {
            if (SelectedCassette == -1)
                return;
            cartridgesDeck.ScanCassette(SelectedCassette);
            string barcode = Analyzer.State.CartridgeBarcode;
            Analyzer.State.CartridgeBarcode = string.Empty;
            if(barcode != null)
            {
                if(!String.IsNullOrEmpty(barcode))
                {
                    Cassettes[SelectedCassette].Barcode = barcode;
                    Cassettes[SelectedCassette].CountLeft = 10;
                } else {
                    Cassettes[SelectedCassette].Barcode = "Пусто";
                    Cassettes[SelectedCassette].CountLeft = 0;
                }
                
            } else {
                Cassettes[SelectedCassette].Barcode = "Пусто";
                Cassettes[SelectedCassette].CountLeft = 0;
            }
            
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
