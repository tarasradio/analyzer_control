using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.SystemControl
{
    /// <summary>
    /// Ячейка пробирки.
    /// </summary>
    public class TubeCell : Cell
    {
        string BarCode { get; set; }

        public TubeCell() : base()
        {

        }
    }

    /// <summary>
    /// Конвейер пробирок. Может содержать пробирки.
    /// </summary>
    public class TubesTransporter
    {
        private uint _numberCells;

        public TubeCell[] Cells { get; private set; }
        
        public TubesTransporter(uint numberCells)
        {
            _numberCells = numberCells;
            Cells = new TubeCell[_numberCells];
        }
    }
}
