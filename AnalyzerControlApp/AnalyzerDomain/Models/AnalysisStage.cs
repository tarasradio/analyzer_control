
namespace AnalyzerDomain.Models
{
    /// <summary>
    /// Стадия анализа
    /// </summary>
    public class AnalysisStage
    {
        /// <summary>
        /// Описание стадии
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ячейка картриджа
        /// </summary>
        public AnalysisStages Type { get; set; }
        
        /// <summary>
        /// Объем материала для выполнения
        /// </summary>
        public int PipettingVolume { get; set; }

        /// <summary>
        /// Требуется ли инкубация
        /// </summary>
        public bool NeedIncubation { get; set; } = false;

        /// <summary>
        /// Время инкубации
        /// </summary>
        public int IncubationTimeInMinutes { get; set; }

        /// <summary>
        /// Требуется ли шаг промывки
        /// </summary>
        public bool NeedWashStep { get; set; } = false;

        /// <summary>
        /// Число шагов промывки
        /// </summary>
        public int NumberOfWashStep { get; set; }

        public AnalysisStage()
        {

        }
    }
}
