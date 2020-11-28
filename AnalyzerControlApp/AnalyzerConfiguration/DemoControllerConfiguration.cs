using AnalyzerDomain.Entyties;
using System;
using System.Collections.Generic;

namespace AnalyzerConfiguration
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
