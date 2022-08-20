using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public class RotorCell : ModelBase
    {
        private string _analysisBarcode;
        public string AnalysisBarcode
        {
            get => _analysisBarcode;
            set
            {
                _analysisBarcode = value;
                OnPropertyChanged();
            }
        }

        private string _cartridgeDescription;
        public string CartridgeDescription
        {
            get => _cartridgeDescription;
            set
            {
                _cartridgeDescription = value;
                OnPropertyChanged();
            }
        }

        public bool Selected { get; set; } = false;

        private CellState _state;
        public CellState State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public bool IsEmpty
        {
            get  {
                return AnalysisBarcode == string.Empty && CartridgeDescription == string.Empty;
            }
            private set { }
        }

        public RotorCell()
        {
            AnalysisBarcode = string.Empty;
            CartridgeDescription = string.Empty;
            State = CellState.Empty;
        }

        public void SetEmpty()
        {
            AnalysisBarcode = string.Empty;
            CartridgeDescription = string.Empty;
            State = CellState.Empty;
        }
    }
}
