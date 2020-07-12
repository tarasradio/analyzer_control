using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AnalyzerConfiguration.ControllersConfiguration
{
    public enum CartridgeCell
    {
        WhiteCell,
        FirstCell,
        SecondCell,
        ThirdCell
    };

    /// <summary>
    /// Ячейка (с пробиркой) в конвейере
    /// </summary>
    public class TubeCell
    {
        public bool HaveTube { get; set; } = false;
        public string BarCode { get; set; } = "";
        /// <summary>
        /// Анализ, закрепелнный за этой пробиркой в ячейке
        /// </summary>
        public TubeInfo Tube { get; set; } = null;

        public TubeCell()
        {

        }
    };

    public class Stage
    {
        [DisplayName("Номер картриджа")]
        public int CartridgePosition { get; set; } = 0;
        [DisplayName("Номер ячейки в картридже")]
        public CartridgeCell Cell { get; set; } = CartridgeCell.WhiteCell;
        [DisplayName("Время выполнения (минут)")]
        public int TimeToPerform { get; set; } = 0;

        public Stage()
        {

        }
    }

    public class TubeInfo
    {
        [DisplayName("Позиция в транспортере")]
        public int Position { get; set; } = 0;
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

        public TubeInfo()
        {

        }

        public void Clear()
        {
            IsFind = false;
            CurrentStage = -1;
            TimeToStageComplete = 0;
        }
    }

    [Serializable]
    public class DemoControllerConfiguration
    {
        public List<TubeInfo> Tubes { get; set; } = new List<TubeInfo>();

        public DemoControllerConfiguration() { }
    }
}
