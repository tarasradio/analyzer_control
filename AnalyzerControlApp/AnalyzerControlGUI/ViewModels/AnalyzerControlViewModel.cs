using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerConfiguration;
using AnalyzerControl;
using AnalyzerControl.Services;
using AnalyzerControlGUI.Views;
using AnalyzerControlGUI.Views.DialogWindows;
using AnalyzerDomain;
using AnalyzerDomain.Models;
using AnalyzerService;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MVVM.Commands;
using MVVM.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace AnalyzerControlGUI.ViewModels
{
    public class AnalyzerControlViewModel : ViewModel
    {
        private const int cassettesCount = 10;
        private const int conveyorCellsCount = 52;
        private const int rotorCellsCount = 40;

        public ObservableCollection<CartridgeCassette> Cassettes { get; set; }
        public ObservableCollection<ConveyorCell> ConveyorCells { get; set; }
        public ObservableCollection<RotorCell> RotorCells { get;  set; }

        private readonly int[] cassettesSensors = { 14, 11, 9, 10, 8, 12, 13, 7, 5, 6 };
        private const int sensorTreshold = 512;

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
                || conveyor.State == ConveyorService.States.AnalyzesProcessing);
                // ю&& ConnectionState;
        }

        private void load()
        {
            Logger.Debug($"Загрузка...");
            Logger.Info($"Загрузка...");

            LoadAnalysisViewModel viewModel = new LoadAnalysisViewModel();
            viewModel.CartridgesDeck = cartridgesDeck;
            viewModel.Conveyor = conveyor;
            viewModel.Rotor = rotor;

            viewModel.Init();

            LoadAnalysisWindow dialog = new LoadAnalysisWindow();
            dialog.DataContext = viewModel;

            dialog.ShowDialog();
            NotifyPropertyChanged("SheduledAnalyzes");
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

        #region CheckCassetteCommand
        RelayCommand _CheckCassetteCommand;

        public RelayCommand CheckCassetteCommand
        {
            get
            {
                if (_CheckCassetteCommand == null)
                {
                    _CheckCassetteCommand = new RelayCommand(
                       param => checkCassette(),
                       param => canCheckCassetteExecute()
                       );
                }
                return _CheckCassetteCommand;
            }
        }

        private bool canCheckCassetteExecute()
        {
            return ConnectionState;
        }

        private void checkCassette()
        {
            scanCassette();
        }
        #endregion

        #region Analyzes

        private ObservableCollection<AnalysisDescription> _analyzes;

        public ObservableCollection<AnalysisDescription> Analyzes
        {
            get {
                if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) {
                    return new ObservableCollection<AnalysisDescription>();
                }
                else
                    return LoadAnalyzes();
            }
            private set {
                _analyzes = value;
                NotifyPropertyChanged("Analyzes");
            }
        }

        private ObservableCollection<AnalysisDescription> LoadAnalyzes()
        {
            using (AnalyzerContext db = new AnalyzerContext()) {
                db.Analyses.Load();
                return db.Analyses.Local.ToObservableCollection();
            }
        }

        #endregion

        #region SelectedAnalysisIndex

        private int selectedAnalysisIndex;

        public int SelectedAnalysisIndex {
            get { return selectedAnalysisIndex; }
            set {
                if(selectedAnalysisIndex != -1) {
                    rotor.Cells[Analyzes[selectedAnalysisIndex].RotorPosition].Selected = false;
                    conveyor.Cells[Analyzes[selectedAnalysisIndex].ConveyorPosition].Selected = false;
                }

                selectedAnalysisIndex = value;

                if (selectedAnalysisIndex != -1) {
                    if(Analyzes[selectedAnalysisIndex].IsCompleted == false) {
                        int rotorPosition = Analyzes[selectedAnalysisIndex].RotorPosition;
                        int conveyorPosition = Analyzes[selectedAnalysisIndex].ConveyorPosition;

                        rotor.Cells[rotorPosition].Selected = true;
                        conveyor.Cells[conveyorPosition].Selected = true;
                    }
                } 
            }
        }

        #endregion

        static IConfigurationProvider provider = new XmlConfigurationProvider();
        static Analyzer analyzer = null;
        static ConveyorService conveyor = null;
        static RotorService rotor = null;
        static CartridgesDeckService cartridgesDeck = null;
        static AnalyzerDemoController demoController = null;

        const string controllerFileName = "DemoControllerConfiguration";

        public AnalyzerControlViewModel()
        {
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
                rotor = new RotorService(rotorCellsCount);
                cartridgesDeck = new CartridgesDeckService(cassettesCount);

                demoController = new AnalyzerDemoController(provider, conveyor, rotor, cartridgesDeck);

                conveyor.SetController(demoController);
                demoController.LoadConfiguration(controllerFileName);

                ConveyorCells = conveyor.Cells;
                RotorCells = rotor.Cells;
                Cassettes = cartridgesDeck.Cassettes;

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
                //openScreen();
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
                NotifyPropertyChanged();
            }
        }

        private void openScreen()
        {
            Analyzer.TaskExecutor.StartTask(() =>
            {
                Analyzer.AdditionalDevices.OpenScreen();
            });
        }

        private void closeScreen()
        {
            Analyzer.TaskExecutor.StartTask(() =>
            {
                Analyzer.AdditionalDevices.CloseScreen();
            });
        }

        // Сканирование кассеты
        private void scanCassette()
        {
            if (SelectedCassette == -1)
                return;

            int sensorNumber = cassettesSensors[SelectedCassette];
            bool cartridgeInserted = true;// Analyzer.State.SensorsValues[sensorNumber] > sensorTreshold;

            Cassettes[SelectedCassette].Inserted = cartridgeInserted;

            if(cartridgeInserted)
            {
                cartridgesDeck.ScanCassette(SelectedCassette);

                string barcode = Analyzer.State.CartridgeBarcode;

                if(barcode != null)
                {
                    if(string.IsNullOrEmpty(barcode) || string.IsNullOrWhiteSpace(barcode) || barcode.Contains("\u0002"))
                    {
                        cartridgesDeck.Cassettes[SelectedCassette].Barcode = "No barcode";
                        cartridgesDeck.Cassettes[SelectedCassette].CountLeft = 0;
                        Analyzer.Serial.SendPacket(new SetLedColorCommand(SelectedCassette, LEDColor.Red()).GetBytes());
                    } else {
                        cartridgesDeck.Cassettes[SelectedCassette].Barcode = barcode.Trim();
                        cartridgesDeck.Cassettes[SelectedCassette].CountLeft = 10;
                        Analyzer.Serial.SendPacket(new SetLedColorCommand(SelectedCassette, LEDColor.Green()).GetBytes());
                    }
                
                } else {
                    cartridgesDeck.Cassettes[SelectedCassette].Barcode = "No barcode";
                    cartridgesDeck.Cassettes[SelectedCassette].CountLeft = 0;

                    Analyzer.Serial.SendPacket(new SetLedColorCommand(SelectedCassette, LEDColor.NoColor()).GetBytes());
                }
            } else {
                Analyzer.Serial.SendPacket(new SetLedColorCommand(SelectedCassette, LEDColor.Red()).GetBytes());
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

        private RelayCommand _testCommand;

        public RelayCommand TestCommand
        {
            get
            {
                return _testCommand ?? (
                    _testCommand = new RelayCommand(obj =>
                    {
                        test();
                    }));
            }
        }

        void test()
        {
            using (AnalyzerContext db = new AnalyzerContext())
            {
                db.Analyses.Add(new AnalysisDescription() { Date = DateTime.Now, RotorPosition = 5, ConveyorPosition = 10, IsCompleted = false });
                db.SaveChanges();
                NotifyPropertyChanged("Analyzes");
            }

            if(conveyor.Cells[0].State == CellState.Empty) {
                conveyor.Cells[0].State = CellState.Processing;
                rotor.Cells[0].State = CellState.Processing;
            } else {
                conveyor.Cells[0].State = CellState.Empty;
                rotor.Cells[0].State = CellState.Empty;
            }
        }
    }
}
