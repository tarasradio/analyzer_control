using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public enum ConveyorCellState
    {
        Empty,
        Processed,
        Processing,
        Error
    }

    public class ConveyorCell : ModelBase
    {
        public string AnalysisBarcode { get; set; }
        public bool IsEmpty
        {
            get
            {
                return AnalysisBarcode == string.Empty;
            }
            private set { }
        }

        private ConveyorCellState _state;
        public ConveyorCellState State
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
            State = ConveyorCellState.Empty;
        }

        public void SetEmpty()
        {
            AnalysisBarcode = string.Empty;
            State = ConveyorCellState.Empty;
        }
    }
}
