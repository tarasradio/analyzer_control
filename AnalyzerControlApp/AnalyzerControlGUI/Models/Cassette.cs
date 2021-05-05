using AnalyzerDomain.Models;

namespace AnalyzerControlGUI.Models
{
    public class Cassette : ModelBase
    {
        private string _barcode;

        public string Barcode {
            get => _barcode;
            set {
                _barcode = value;
                OnPropertyChanged();
            }
        }

        private int _countLeft;

        public int CountLeft {
            get => _countLeft;
            set {
                _countLeft = value;
                OnPropertyChanged();
            }
        }

        public Cassette()
        {
            Barcode = "RedMary4590";
            CountLeft = 0;
        }
    }
}
