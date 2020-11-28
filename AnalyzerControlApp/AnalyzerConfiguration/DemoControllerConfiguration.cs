using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AnalyzerConfiguration
{
    public enum CartridgeCell
    {
        FirstCell, // Ячейка с первым реагентом
        SecondCell, // Ячейка со вторым реагентом
        ThirdCell, // Ячейка с третьим реагентом
        MixCell, // Белая ячейка, в которой происходит смешивание реагентов
        ResultCell // Прозрачная ячейка, куда помещается конечный результат
    };

    public class Stage
    {
        [DisplayName("Номер картриджа")]
        public int CartridgePosition { get; set; } = 0;
        [DisplayName("Номер ячейки в картридже")]
        public CartridgeCell Cell { get; set; } = CartridgeCell.MixCell;
        [DisplayName("Время выполнения (минут)")]
        public int TimeToPerform { get; set; } = 0;

        public Stage()
        {
            Cell = CartridgeCell.MixCell;
        }
    }

    public class AnalysisInfo
    {
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

        public void SetNewIncubationTime()
        {
            TimeToStageComplete = Stages[CurrentStage].TimeToPerform;
        }

        public bool IsNotFinishStage()
        {
            return CurrentStage < Stages.Count;
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
    }

    [Serializable]
    public class DemoControllerConfiguration
    {
        public List<AnalysisInfo> AnalysisList { get; set; }

        public DemoControllerConfiguration() 
        {
            AnalysisList = new List<AnalysisInfo>();
        }
    }
}
