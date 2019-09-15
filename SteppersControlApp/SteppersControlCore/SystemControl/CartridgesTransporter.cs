using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.SystemControl
{
    /// <summary>
    /// Ячейка картриджа.
    /// </summary>
    public class CartridgeCell : Cell
    {
        public CartridgeCell() : base()
        {

        }
    }

    /// <summary>
    /// Ротор. Может содержать картриджи.
    /// </summary>
    public class CartridgesTransporter
    {
        private uint _numberOfCells;

        public CartridgeCell[] Cells;

        public CartridgesTransporter(uint numberOfCells)
        {
            _numberOfCells = numberOfCells;
            Cells = new CartridgeCell[numberOfCells];
        }
    }
}
