using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public enum CartridgeWell
    {
        /// <summary>
        /// Ячейка с первым реагентом
        /// </summary>
        W1,
        /// <summary>
        /// Ячейка со вторым реагентом
        /// </summary>
        W2,
        /// <summary>
        /// Ячейка с третьим реагентом
        /// </summary>
        W3,
        /// <summary>
        /// Белая ячейка, в которой происходит смешивание реагентов
        /// </summary>
        ACW,
        /// <summary>
        /// Прозрачная кювета, куда помещается конечный результат
        /// </summary>
        CUV
    };
}
