using AnalyzerDomain.Entyties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Entyties
{
    public class ConveyorCell
    {
        public bool HaveTube { get; set; }
        public string BarCode { get; set; }
        public AnalysisInfo AnalysisInfo { get; set; }

        public ConveyorCell()
        {
            AnalysisInfo = null;
            HaveTube = false;
            BarCode = string.Empty;
        }
    }
}
