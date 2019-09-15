using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.SystemControl
{
    /// <summary>
    /// Ячейка. Может содержать картридж или пробирку или быть не занята.
    /// </summary>
    public class Cell
    {
        int Id { get; set; } = 0;
        bool Busy { get; set; } = false;

        public Cell()
        {

        }
    }
}
