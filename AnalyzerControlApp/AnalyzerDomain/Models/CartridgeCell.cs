using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public enum CartridgeCell
    {
        /// <summary>
        /// Ячейка с первым реагентом
        /// </summary>
        FirstCell,
        /// <summary>
        /// Ячейка со вторым реагентом
        /// </summary>
        SecondCell,
        /// <summary>
        /// Ячейка с третьим реагентом
        /// </summary>
        ThirdCell,
        /// <summary>
        /// Белая ячейка, в которой происходит смешивание реагентов
        /// </summary>
        MixCell,
        /// <summary>
        /// Прозрачная ячейка, куда помещается конечный результат
        /// </summary>
        ResultCell
    };
}
