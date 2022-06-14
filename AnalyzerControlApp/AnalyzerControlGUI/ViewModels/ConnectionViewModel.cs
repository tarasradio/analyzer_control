using AnalyzerCommunication.SerialCommunication;
using MVVM.Commands;
using MVVM.ViewModels;

namespace AnalyzerControlGUI.ViewModels
{
    public class ConnectionViewModel : ViewModel
    {
        ISerialAdapter serial = null;

        public ConnectionViewModel()
        {

        }

        public ConnectionViewModel(ISerialAdapter serial)
        {
            this.serial = serial;
            Ports = serial.GetAvailablePorts();

            if(Ports.Length != 0)
            {
                SelectedPort = Ports.Length - 1;
            }

            serial.ConnectionChanged += UpdateConnectionState;
            UpdateConnectionState(serial.IsOpen());
        }

        private void UpdateConnectionState(bool state)
        {
            if(state) {
                ConnectionState = "Соединение установлено";
                ConnectionButtonState = "Отключиться";
            } else {
                ConnectionState = "Соединение не установлено";
                ConnectionButtonState = "Подключиться";
            }
        }

        private string[] _ports;

        public string[] Ports
        {
            get {
                return _ports;
            }
            set {
                _ports = value;
                NotifyPropertyChanged();
            }
        }

        private int _selectedPort;

        public int SelectedPort
        {
            get {
                return _selectedPort;
            }
            set {
                _selectedPort = value;
                NotifyPropertyChanged();
            }
        }

        private int _selectedBaudrate;

        public int SelectedBaudrate
        {
            get
            {
                return _selectedBaudrate;
            }
            set
            {
                _selectedBaudrate = value;
                NotifyPropertyChanged();
            }
        }

        private string _connectionState;

        public string ConnectionState
        {
            get
            {
                return _connectionState;
            }
            set
            {
                _connectionState = value;
                NotifyPropertyChanged();
            }
        }

        private string _connectionButtonState;

        public string ConnectionButtonState
        {
            get
            {
                return _connectionButtonState;
            }
            set
            {
                _connectionButtonState = value;
                NotifyPropertyChanged();
            }
        }

        private RelayCommand _updatePortsCommand;

        public RelayCommand UpdatePortsCommand
        {
            get
            {
                return _updatePortsCommand ?? (_updatePortsCommand = new RelayCommand( obj =>
                {
                    Ports = serial.GetAvailablePorts();

                    if (Ports.Length != 0)
                    {
                        SelectedPort = Ports.Length - 1;
                    }
                },
                canExecute => (!serial?.IsOpen() == true)
                ));
            }
        }

        private RelayCommand _connectionChangeCommand;

        public RelayCommand ConnectionChangeCommand
        {
            get
            {
                return _connectionChangeCommand ?? (_connectionChangeCommand = new RelayCommand(obj => {
                    if(serial.IsOpen()) {
                        serial.Close();
                    } else {
                        string port = Ports[SelectedPort];
                        serial.Open(port, SelectedBaudrate);
                    }
                },
                canExecute => (Ports?.Length > 0)
                ));
            }
        }

    }
}
