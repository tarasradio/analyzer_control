using System;

namespace AnalyzerDomain.Models
{
    public class Analysis
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public string Description { get; set; }
        public string Barcode { get; set; }

        public AnalysisType AnalysisType { get; set; }

        public int CurrentStage { get; set; }
        public string Result { get; set; }

        public Analysis()
        {

        }
    }
}
