using System.Collections.Generic;

namespace AnalyzerDomain.Models
{
    /// <summary>
    /// Тип анализа
    /// </summary>
    public class AnalysisType
    {
        /// <summary>
        /// Описание типа анализа
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Модель картриджа для выполнения анализа
        /// </summary>
        public CartridgeModel CartridgeModel { get; set; }

        /// <summary>
        /// Список стадий анализа
        /// </summary>
        public List<AnalysisStage> Stages { get; set; }

        /// <summary>
        /// Создание нового типа анализа
        /// </summary>
        /// <param name="description">Описание типа анализа</param>
        /// <param name="cartridgeModel">Модель картриджа для выполнения анализа</param>
        public AnalysisType(string description, CartridgeModel cartridgeModel)
        {
            Stages = new List<AnalysisStage>();
            this.Description = description;
            this.CartridgeModel = cartridgeModel;
        }
    }
}
