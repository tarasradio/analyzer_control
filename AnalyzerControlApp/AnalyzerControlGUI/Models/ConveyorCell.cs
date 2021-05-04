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

    public class ConveyorCell
    {
        public ConveyorCellState State { get; set; }

        public ConveyorCell()
        {
            State = ConveyorCellState.Empty;
        }
    }
}
