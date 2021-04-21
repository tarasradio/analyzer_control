using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Services
{
    public class RotorCell
    {
        public string AnalysisBarcode { get; set; }
        public bool IsEmpty {
            get {
                return AnalysisBarcode == string.Empty;
            }
            private set { }
        }

        public RotorCell()
        {
            AnalysisBarcode = string.Empty;
        }

        public void SetEmpty() {
            AnalysisBarcode = string.Empty;
        }
    }
    /// <summary>
    /// Сервис управления ротором
    /// </summary>
    public class RotorService
    {
        public RotorCell[] Cells { get; private set; }

        public RotorService(int cellsCount)
        {
            Cells = Enumerable.Repeat(new RotorCell(), cellsCount).ToArray();
        }

        public bool ExistEmptyCells()
        {
            return Cells.Count(c => c.IsEmpty) > 0;
        }

        private (bool, int?) findFreeCellIndex()
        {
            for (int i = 0; i < Cells.Length; i++) {
                if(Cells[i].IsEmpty) {
                    return (true, i);
                }
            }
            return (false, null);
        }

        public (bool, int?) AddAnalysis(string barcode)
        {
            var (existFreeCells, cellIndex) = findFreeCellIndex();

            if(existFreeCells) {
                Cells[(int)cellIndex].AnalysisBarcode = barcode;
            }
            return (existFreeCells, cellIndex);
        }

        public void RemoveAnalysis(int cellIndex)
        {
            Cells[cellIndex].SetEmpty();
        }
    }
}
