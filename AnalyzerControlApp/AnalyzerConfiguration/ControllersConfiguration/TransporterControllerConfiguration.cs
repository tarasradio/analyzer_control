using System.ComponentModel;

namespace AnalyzerConfiguration.ControllersConfiguration
{
    public class TransporterControllerConfiguration
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель вращения пробирки")]
        public int TurnTubeStepper { get; set; }
        [Category("1. Двигатели")]
        [DisplayName("Двигатель движения конвейера")]
        public int TransporterStepper { get; set; }

        [Category("2. Скорость")]
        [DisplayName("Скорость вращения пробирки")]
        public int TurnTubeSpeed { get; set; }
        [Category("2. Скорость")]
        [DisplayName("Скорость движения конвейера")]
        public int TransporterMoveSpeed { get; set; }
        [Category("2. Скорость")]
        [DisplayName("Скорость движения конвейера домой")]
        public int TransporterHomeSpeed { get; set; }

        [Category("3. Шаги")]
        [DisplayName("Шагов для сдвига на одну пробирку")]
        public int StepsPerTube { get; set; }
        [Category("3. Шаги")]
        [DisplayName("Шагов для вращения пробирки")]
        public int StepsPerTubeRotate { get; set; }

        public TransporterControllerConfiguration()
        {

        }
    }
}
