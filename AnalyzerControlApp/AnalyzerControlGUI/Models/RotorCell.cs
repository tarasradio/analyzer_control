using AnalyzerDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControlGUI.Models
{
    public class RotorCell : ModelBase
    {
        private string _analysisBarcode;
        public string AnalysisBarcode
        {
            get => _analysisBarcode;
            set {
                _analysisBarcode = value;
                OnPropertyChanged();
            }
        }

        public RotorCell()
        {
            AnalysisBarcode = string.Empty;
        }

        public void SetEmpty() {
            AnalysisBarcode = string.Empty;
        }
    }
}
