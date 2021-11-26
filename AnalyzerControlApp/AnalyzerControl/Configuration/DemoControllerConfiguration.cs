using AnalyzerDomain.Entyties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Configuration
{
    [Serializable]
    public class DemoControllerConfiguration
    {
        public List<AnalysisInfo> Analyzes { get; set; }

        public DemoControllerConfiguration()
        {
            Analyzes = new List<AnalysisInfo>();
        }
    }
}
