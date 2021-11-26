using System;

namespace AnalyzerDomain.Models
{
    public class Analysis
    {
        public int Id { get; set; }
        /// <summary>
        /// Дата добавления анализа
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Описание анализа
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Штрих-код анализа (для связи с пациэнтом)
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Тип анализа
        /// </summary>
        public AnalysisType AnalysisType { get; set; }

        /// <summary>
        /// Текущая стадия
        /// </summary>
        public int CurrentStage { get; set; }

        /// <summary>
        /// Результат анализа
        /// </summary>
        public string Result { get; set; }

        public Analysis()
        {

        }
    }
}
