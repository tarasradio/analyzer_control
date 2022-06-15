using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public class AnalysisDescription
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string PatientId { get; set; }

        public string AnalysisType { get; set; }

        public int RotorPosition { get; set; }
        public int ConveyorPosition { get; set; }

        // Данные для выполнения анализа
        public int SampleVolume { get; set; }
        public int Tw2Volume { get; set; }
        public int Tw3Volume { get; set; }
        public int TacwVolume { get; set; }
        public int Inc1Duration { get; set; }
        public int inc2Duration { get; set; }

        public int CurrentStage { get; set; }
        public bool IncubationStarted { get; set; }
        public int RemainingIncubationTime { get; set; }
        public double OM1Value { get; set; }
        public double OM2Value { get; set; }

        public bool IsCompleted { get; set; }
        public string Result { get; set; }
    }
}
