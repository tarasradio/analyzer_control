using System.Collections.Generic;

namespace AnalyzerDomain.Models
{
    /// <summary>
    /// Тип анализа
    /// </summary>
    public class AnalysisType : ModelBase
    {
        /// <summary>
        /// Идентификатор типа анализа
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Описание типа анализа
        /// </summary>
        private string _description;

        public string Description
        {
            get { return _description; }
            set {
                _description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Модель картриджа для выполнения анализа
        /// </summary>
        private Cartridge _cartridge;

        public Cartridge Cartridge
        {
            get { return _cartridge; }
            set {
                _cartridge = value;
                OnPropertyChanged();
            }
        }

        private AnalysisStage _samplingStage;

        public AnalysisStage SamplingStage
        {
            get { return _samplingStage; }
            set { 
                _samplingStage = value;
                OnPropertyChanged();
            }
        }

        private AnalysisStage _conjugateStage;

        public AnalysisStage ConjugateStage
        {
            get { return _conjugateStage; }
            set
            {
                _conjugateStage = value;
                OnPropertyChanged();
            }
        }

        private AnalysisStage _enzymeComplexStage;

        public AnalysisStage EnzymeComplexStage
        {
            get { return _enzymeComplexStage; }
            set
            {
                _enzymeComplexStage = value;
                OnPropertyChanged();
            }
        }

        private AnalysisStage _substrateStage;

        public AnalysisStage SubstrateStage
        {
            get { return _substrateStage; }
            set
            {
                _substrateStage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Список стадий анализа
        /// </summary>
        //public List<AnalysisStage> Stages { get; set; }

        /// <summary>
        /// Создание нового типа анализа
        /// </summary>
        /// <param name="description">Описание типа анализа</param>
        /// <param name="cartridge">Модель картриджа для выполнения анализа</param>
        public AnalysisType(string description, Cartridge cartridge)
        {
            //Stages = new List<AnalysisStage>();
            Description = description;
            Cartridge = cartridge;
        }

        public AnalysisType()
        {

        }
    }
}
