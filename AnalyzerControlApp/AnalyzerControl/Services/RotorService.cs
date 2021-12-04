using AnalyzerDomain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Services
{
    /// <summary>
    /// Сервис управления ротором
    /// </summary>
    public class RotorService
    {
        public ObservableCollection<RotorCell> Cells { get; private set; }

        public RotorService(int cellsCount)
        {
            Cells = new ObservableCollection<RotorCell>();

            for (int i = 0; i < cellsCount; i++)
            {
                Cells.Add(new RotorCell());
            }
        }

        public bool ExistEmptyCells()
        {
            return Cells.Count(c => c.IsEmpty) > 0;
        }

        private (bool, int?) findFreeCellIndex()
        {
            for (int i = 0; i < Cells.Count; i++) {
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
