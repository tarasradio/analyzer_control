using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Models
{
    public class AnalysisDescription : ModelBase
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

        private int _currentStage;
        public int CurrentStage { get { return _currentStage; } set { _currentStage = value; OnPropertyChanged(); } }

        private bool _incubationStarted;
        public bool IncubationStarted { get { return _incubationStarted; } set { _incubationStarted = value; OnPropertyChanged(); } }

        private int _remIncTime;
        public int RemainingIncubationTime { get { return _remIncTime; } set { _remIncTime = value; OnPropertyChanged(); } }
        public double OM1Value { get; set; }
        public double OM2Value { get; set; }

        private bool _isCompleted;
        public bool IsCompleted { get { return _isCompleted; } set { _isCompleted = value; OnPropertyChanged(); } }

        private string _result;
        public string Result { get { return _result; } set { _result = value; OnPropertyChanged(); } }
    }
}
