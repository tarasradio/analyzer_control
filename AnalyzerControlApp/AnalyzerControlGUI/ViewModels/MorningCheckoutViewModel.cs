using AnalyzerControlGUI.Commands;
using AnalyzerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControlGUI.ViewModels
{
    public class MorningCheckoutViewModel : ViewModel
    {
        private bool _notFinished = true;

        public bool NotFinished
        {
            get => _notFinished;
            set {
                _notFinished = value;
                NotifyPropertyChanged();
            }
        }

        private enum MorningChecoutState 
        {
            Started,
            RotorUnloadSelected,
            RotorUnloadScipped,
            CassetesUnloadSelected,
            CassetesUnloadScipped,
            TubesUnloadSelected,
            Finished
        }

        private MorningChecoutState State = MorningChecoutState.Started;

        private string _dialogText;

        public string DialogText
        {
            get => _dialogText;
            set {
                _dialogText = value;
                NotifyPropertyChanged();
            }
        }

        public MorningCheckoutViewModel()
        {
            DialogText = "Разгрузить ротор?";
        }

        RelayCommand _okCommand;

        public RelayCommand OkCommand {
            get {
                if (_okCommand == null) {
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
            return State != MorningChecoutState.Finished;
        }

        private void ok()
        {
            switch(State)
            {
                case MorningChecoutState.Started:
                    unloadRotor();
                    DialogText = "Ротор разгружен, перезарядить кассеты?";
                    State = MorningChecoutState.RotorUnloadSelected;
                    break;
                case MorningChecoutState.RotorUnloadSelected:
                    DialogText = "Кассеты перезаряжены, заменить пробирки?";
                    State = MorningChecoutState.CassetesUnloadSelected;
                    break;
                case MorningChecoutState.CassetesUnloadSelected:
                    unloadTubes();
                    DialogText = "Пробирки заменены.";
                    State = MorningChecoutState.Finished;
                    NotFinished = false;
                    break;
            }
        }

        RelayCommand _cancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(
                       param => cancel(),
                       param => canCancelExecute()
                       );
                }
                return _cancelCommand;
            }
        }

        private bool canCancelExecute()
        {
            return State != MorningChecoutState.Finished;
        }

        private void cancel()
        {
            switch (State)
            {
                case MorningChecoutState.Started:
                    DialogText = "Перезарядить кассеты?";
                    State = MorningChecoutState.RotorUnloadSelected;
                    break;
                case MorningChecoutState.RotorUnloadSelected:
                    DialogText = "Хранение кассет не соотвествует требованиям, заменить пробирки?";
                    State = MorningChecoutState.CassetesUnloadSelected;
                    break;
                case MorningChecoutState.CassetesUnloadSelected:
                    DialogText = "Работа будет продолжена, невыгруженные пробирки не будут обработаны.";
                    State = MorningChecoutState.Finished;
                    NotFinished = false;
                    break;
            }
        }

        private async void unloadTubes()
        {
            await Task.Run(() => { 
                Analyzer.Conveyor.Move(25); 
            });
        }

        private async void unloadRotor()
        {
            await Task.Run(() =>
            {
                Analyzer.Charger.HomeHook(false);
                Analyzer.Charger.MoveHookAfterHome();
                Analyzer.Rotor.PlaceCellAtDischarge(0);
                Analyzer.Charger.HomeRotator();
                Analyzer.Charger.TurnToDischarge();
                Analyzer.Charger.HomeHook(true);
                Analyzer.Charger.MoveHookAfterHome();
                Analyzer.Charger.ChargeCartridge();
            });
        }
    }
}
