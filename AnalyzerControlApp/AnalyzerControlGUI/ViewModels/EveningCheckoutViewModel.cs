using AnalyzerControlGUI.Commands;
using AnalyzerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControlGUI.ViewModels
{
    public class EveningCheckoutViewModel : ViewModel
    {
        private bool _notFinished = true;

        public bool NotFinished
        {
            get => _notFinished;
            set
            {
                _notFinished = value;
                NotifyPropertyChanged();
            }
        }

        private enum MorningChecoutState
        {
            Started,
            RotorUnloaded,
            CassetesUnloaded,
            TubesUnloaded,
            Finished
        }

        private MorningChecoutState State = MorningChecoutState.Started;

        private string _dialogText;

        public string DialogText
        {
            get => _dialogText;
            set
            {
                _dialogText = value;
                NotifyPropertyChanged();
            }
        }

        public EveningCheckoutViewModel()
        {
            DialogText = "Нажмите \"Продолжить\" для разгрузки ротора.";
        }

        RelayCommand _okCommand;

        public RelayCommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new RelayCommand(
                       param => ok(),
                       param => canOkExecute()
                       );
                }
                return _okCommand;
            }
        }

        private bool canOkExecute()
        {
            return true;
        }

        private void ok()
        {
            switch (State)
            {
                case MorningChecoutState.Started:
                    unloadRotor();
                    DialogText = "Ротор разгружен. Выгрузите кассеты и нажмите \"Продолжить\".";
                    State = MorningChecoutState.RotorUnloaded;
                    break;
                case MorningChecoutState.RotorUnloaded:
                    DialogText = "Нажмите \"Продолжить\" для выгрузки пробирок.";
                    State = MorningChecoutState.CassetesUnloaded;
                    break;
                case MorningChecoutState.CassetesUnloaded:
                    unloadTubes();
                    DialogText = "Пробирки выгружены. Нажмите \"Продолжить\" для завершения работы.";
                    State = MorningChecoutState.TubesUnloaded;
                    break;
                case MorningChecoutState.TubesUnloaded:
                    DialogText = "Пробирки выгружены. Нажмите \"Продолжить\" для завершения работы.";
                    State = MorningChecoutState.Finished;
                    break;
                case MorningChecoutState.Finished:
                    shutdown();
                    NotFinished = false;
                    break;
            }
        }

        private void shutdown()
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = "1";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown",
                                               mboShutdownParams, null);
            }
        }

        private void unloadTubes()
        {
            Analyzer.Conveyor.Move(25);
        }

        private void unloadRotor()
        {
            Analyzer.Charger.HomeHook();
            Analyzer.Rotor.PlaceCellAtDischarge(0);
            Analyzer.Charger.HomeRotator();
            Analyzer.Charger.TurnToDischarge();
            Analyzer.Charger.HomeHook();
            Analyzer.Charger.MoveHookAfterHome();
            Analyzer.Charger.ChargeCartridge();
        }
    }
}
