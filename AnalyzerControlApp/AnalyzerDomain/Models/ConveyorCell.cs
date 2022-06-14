using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public class ConveyorCell : ModelBase
    {
        public string AnalysisBarcode { get; set; }
        public bool IsEmpty {
            get  {
                return string.IsNullOrEmpty(AnalysisBarcode);
            }
            private set { }
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

        public ConveyorCell()
        {
            AnalysisBarcode = string.Empty;
            State = CellState.Empty;
        }

        public void SetEmpty()
        {
            AnalysisBarcode = string.Empty;
            State = CellState.Empty;
        }
    }
}
