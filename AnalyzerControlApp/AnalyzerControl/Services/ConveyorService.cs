using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Services
{
    public class ConveyorCell
    {
        public string AnalysisBarcode { get; set; }
        public bool IsEmpty {
            get {
                return AnalysisBarcode == string.Empty;
            }
            private set { }
        }

        public ConveyorCell() {
            AnalysisBarcode = string.Empty;
        }

        public void SetEmpty() {
            AnalysisBarcode = string.Empty;
        }
    }

    /// <summary>
    /// Сервис управления конвейером
    /// </summary>
    public class ConveyorService
    {
        public ConveyorCell[] Cells { get; private set; }

        public ConveyorService(int cellsCount)
        {
            Cells = Enumerable.Repeat(new ConveyorCell(), cellsCount).ToArray();
        }

        public bool ExistEmptyCells()
        {
            return Cells.Count(c => c.IsEmpty) > 0;
        }

        private (bool, int?) findFreeCellIndex()
        {
            for (int i = 0; i < Cells.Length; i++) {
                if (Cells[i].IsEmpty) {
                    return (true, i);
                }
            }
            return (false, null);
        }

        public void RemoveAnalysis(int cellIndex)
        {
            Cells[cellIndex].SetEmpty();
        }
    }
}
