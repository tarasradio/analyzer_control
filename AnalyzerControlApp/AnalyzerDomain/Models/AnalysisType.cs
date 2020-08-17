using System.Collections.Generic;

namespace AnalyzerDomain.Models
{
    public class AnalysisType
    {
        public string Description { get; set; }
        public CartridgeModel CartridgeModel { get; set; }
        public List<AnalysisStage> Stages { get; set; }

        public AnalysisType()
        {

        }
    }
}
