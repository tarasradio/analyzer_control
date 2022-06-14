using MVVM.Commands;
using MVVM.ViewModels;

namespace AnalyzerControlGUI.ViewModels
{
    public class EditCartridgeViewModel : ViewModel
    {
        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                NotifyPropertyChanged();
            }
        }

        private string _barcode;

        public string Barcode
        {
            get => _barcode;
            set 
            {
                _barcode = value;
                NotifyPropertyChanged();
            }
        }

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                NotifyPropertyChanged();
            }
        }

        RelayCommand _okCommand;

        public RelayCommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new RelayCommand(
                       param => { DialogResult = true; },
                       param => true);
                }
                return _okCommand;
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
                       param => { DialogResult = false; },
                       param => true);
                }
                return _cancelCommand;
            }
        }

    }
}
