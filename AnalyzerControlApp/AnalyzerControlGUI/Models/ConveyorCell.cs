using AnalyzerDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControlGUI.Models
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
        private ConveyorCellState _state;
        public ConveyorCellState State { 
            get { return _state; }
            set { 
                _state = value;
                OnPropertyChanged();
            }
        }

        public ConveyorCell()
        {
            State = ConveyorCellState.Empty;
        }
    }
}
