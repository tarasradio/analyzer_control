using AnalyzerConfiguration;
using AnalyzerControl;
using AnalyzerControl.Services;
using AnalyzerControlGUI.Commands;
using AnalyzerControlGUI.Views;
using AnalyzerService;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControlGUI.ViewModels
{
    public class mainViewModel : ViewModel
    {
        static IConfigurationProvider provider = new XmlConfigurationProvider();
        static Analyzer analyzer = null;
        static ConveyorService conveyor = null;
        static RotorService rotor = null;
        static AnalyzerDemoController demoController = null;

        const string controllerFileName = "DemoControllerConfiguration";

        public mainViewModel()
        {
            Logger.DebugMessageAdded += onDebugMessageAdded;

            tryCreateController();

            UpdateConnectionState(Analyzer.Serial.IsOpen());
        }

        private void tryCreateController()
        {
            try {
                analyzer = new Analyzer(provider);
                conveyor = new ConveyorService(54);
                rotor = new RotorService(40);
                demoController = new AnalyzerDemoController(provider, conveyor, rotor);
                demoController.LoadConfiguration(controllerFileName);

                Analyzer.Serial.ConnectionChanged += UpdateConnectionState;
            } catch {
                Logger.Debug("Возникла ошибка при запуске!");
            }
        }

        private void UpdateConnectionState(bool state)
        {
            if (state)
            {
                ConnectionState = "Соединение установлено";
            }
            else
            {
                ConnectionState = "Соединение не установлено";
            }
        }

        private void onDebugMessageAdded(string message)
        {
            LogText += $"{ message }";
        }

        private string _logText;

        public string LogText
        {
            get { return _logText; }
            set {
                _logText = value;
                NotifyPropertyChanged();
            }
        }

        private string _connectionState;

        public string ConnectionState {
            get {
                return _connectionState;
            }
            set {
                _connectionState = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _connectionCommand;

        public RelayCommand ConnectionCommand
        {
            get
            {
                return _connectionCommand ??
                   (_connectionCommand = new RelayCommand( obj =>
                   {
                       ConnectionViewModel viewModel = new ConnectionViewModel(Analyzer.Serial);
                       ConnectionWindow connectionWindow = new ConnectionWindow();
                       connectionWindow.DataContext = viewModel;

                       connectionWindow.ShowDialog();
                   }));
            }
        }


        private RelayCommand _cartridgesManagementCommand;

        public RelayCommand CartridgesManagementCommand
        {
            get
            {
                return _cartridgesManagementCommand ??
                  (_cartridgesManagementCommand = new RelayCommand(obj =>
                  {
                      CartridgesWindow cartridgesWindow = new CartridgesWindow();
                      cartridgesWindow.ShowDialog();
                  }));
            }
        }

        private RelayCommand _analysisTypesManagementCommand;

        public RelayCommand AnalysisTypesManagementCommand
        {
            get
            {
                return _analysisTypesManagementCommand ??
                  (_analysisTypesManagementCommand = new RelayCommand(obj =>
                  {
                      AnalysisTypesWindow analysisTypesWindow = new AnalysisTypesWindow();
                      analysisTypesWindow.ShowDialog();
                  }));
            }
        }
    }
}
