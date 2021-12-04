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
using AnalyzerDomain.Models;

namespace AnalyzerControlGUI.ViewModels
{
    public class AnalyzerControlViewModel : ViewModel
    {
        private const int cassettesCount = 10;
        private const int conveyorCellsCount = 54;
        private const int rotorCellsCount = 40;

        public ObservableCollection<Cassette> Cassettes { get; set; }
        public ObservableCollection<ConveyorCell> ConveyorCells { get; set; }
        public ObservableCollection<Models.RotorCell> RotorCells { get; set; }

        #region MorningCheckout
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

        #endregion

        #region EveningCheckout
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
        #endregion

        #region Start
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
        #endregion

        #region ManualInputCommand

        RelayCommand _manualInputCommand;

        public RelayCommand ManualInputCommand
        {
            get
            {
                if (_manualInputCommand == null)
                {
                    _manualInputCommand = new RelayCommand(
                       param => manualInput(),
                       param => canManualInputExecute()
                       );
                }
                return _manualInputCommand;
            }
        }

        private bool canManualInputExecute()
        {
            return ConnectionState;
        }

        private void manualInput()
        {
            ManualInputDialog dialog = new ManualInputDialog();
            dialog.ShowDialog();
        }

        #endregion

        #region LoadCommand
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

            //conveyor.Load();\
            changeCell();
        }
        #endregion

        #region UnloadCommand

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
        #endregion

        #region AbortCommand
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
        #endregion

        #region ResumeCommand
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
        #endregion

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

                ConveyorCells = conveyor.Cells;

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

            bool cartridgeInserted = false;
            if(SelectedCassette == 9)
                cartridgeInserted = Analyzer.State.SensorsValues[14] > 512;
            Cassettes[SelectedCassette].Inserted = cartridgeInserted;

            if(cartridgeInserted)
            {
                cartridgesDeck.ScanCassette(SelectedCassette);

                string barcode = Analyzer.State.CartridgeBarcode;

                if(barcode != null)
                {
                    if(!String.IsNullOrEmpty(barcode))
                    {
                        Cassettes[SelectedCassette].Barcode = barcode;
                        Cassettes[SelectedCassette].CountLeft = 10;
                    } else {
                        Cassettes[SelectedCassette].Barcode = "No barcode";
                        Cassettes[SelectedCassette].CountLeft = 0;
                    }
                
                } else {
                    Cassettes[SelectedCassette].Barcode = "No barcode";
                    Cassettes[SelectedCassette].CountLeft = 0;
                }
            } else
            {
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
        
        private void changeCell()
        {
            if(ConveyorCells[0].State == ConveyorCellState.Empty)
                ConveyorCells[0].State = ConveyorCellState.Error;
            else
                ConveyorCells[0].State = ConveyorCellState.Empty;

            if (RotorCells[0].AnalysisBarcode == string.Empty)
                RotorCells[0].AnalysisBarcode = "123";
            else
                RotorCells[0].AnalysisBarcode = string.Empty;
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

            //ConveyorCells = new ObservableCollection<Models.ConveyorCell>();

            //for (int i = 0; i < conveyorCellsCount; ++i)
            //    ConveyorCells.Add(new Models.ConveyorCell());

            //ConveyorCells[5].State = ConveyorCellState.Processed;
            //ConveyorCells[6].State = ConveyorCellState.Error;
            //ConveyorCells[7].State = ConveyorCellState.Processing;

            RotorCells = new ObservableCollection<Models.RotorCell>();

            for(int i = 0; i < rotorCellsCount; ++i)
            {
                RotorCells.Add(new Models.RotorCell());
            }

            //RotorCells[0].AnalysisBarcode = "123";
            //RotorCells[4].AnalysisBarcode = "123";
        }
    }
}
