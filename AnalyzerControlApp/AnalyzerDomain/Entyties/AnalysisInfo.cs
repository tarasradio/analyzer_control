using System.Collections.Generic;
using System.ComponentModel;

namespace AnalyzerDomain.Entyties
{
    public class AnalysisInfo
    {
        public enum ProcessingStages
        {
            NoSampleWasTaken = -1,
            Sampling
        }

        [DisplayName("Позиция в конвейере")]
        public int PositionInConveyor { get; set; } = 0;
        [DisplayName("Позиция в роторе")]
        public int PositionInRotor { get; set; } = 0;
        [DisplayName("Штрихкод")]
        public string BarCode { get; set; } = "";
        [DisplayName("Список стадий")]
        public List<Stage> Stages { get; set; } = new List<Stage>();
        public int CurrentStage { get; set; } = 0;
        /// <summary>
        /// Время, оставшееся до завершения инкубации (минуты)
        /// </summary>
        public int TimeToStageComplete { get; set; } = 0;
        /// <summary>
        /// Пробирка с данным штрихкодом найдена
        /// </summary>
        public bool IsFind { get; set; } = false;

        public AnalysisInfo()
        {

        }

        public void Clear()
        {
            IsFind = false;
            CurrentStage = -1;
            TimeToStageComplete = 0;
        }

        public void SetSamplingStage()
        {
            CurrentStage = (int)ProcessingStages.Sampling;
        }

        public void SetNewIncubationTime()
        {
            TimeToStageComplete = Stages[CurrentStage].TimeToPerform;
        }

        public bool IsNotFinishStage()
        {
            return CurrentStage < Stages.Count;
        }

        public bool NoSampleWasTaken()
        {
            return CurrentStage == (int)ProcessingStages.NoSampleWasTaken;
        }

        public bool Finished() // Это еще почему ???
        {
            return CurrentStage <= Stages.Count;
        }

        public bool ProcessingNotFinished()
        {
            return CurrentStage >= 0 && IsNotFinishStage() && TimeToStageComplete == 0;
        }

        public bool InProgress()
        {
            return IsFind && TimeToStageComplete != 0;
        }

        public void DecrementRemainingTime()
        {
            TimeToStageComplete -= 1;
        }

        public void NextStage()
        {
            CurrentStage++;
        }
    }
}
