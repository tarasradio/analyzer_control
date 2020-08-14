using System.ComponentModel;

namespace AnalyzerConfiguration.UnitsConfiguration
{
    public class ConveyorConfiguration
    {
        [DisplayName("Двигатель вращения пробирки"), Category("1. Номера двигателей")]
        public int TubeRotatorStepper { get; set; } = 0;
        [DisplayName("Двигатель движения конвейера"), Category("1. Номера двигателей")]
        public int ConveyorStepper { get; set; } = 0;

        [DisplayName("Скорость вращения пробирки"), Category("2.1 Скорость (вращатель пробирки)")]
        public int TubeRotatorSpeed { get; set; } = 0;

        [DisplayName("Скорость движения конвейера"), Category("2.2 Скорость (конвейер)")]
        public int ConveyorSpeed { get; set; } = 0;
        [DisplayName("Скорость движения конвейера домой"), Category("2.2 Скорость (конвейер)")]
        public int ConveyorSpeedAtHoming { get; set; } = 0;

        [DisplayName("Шаги на оборот пробирки"), Category("3.1 Шаги (вращатель пробирки)")]
        public int RotatorStepsPerTubeRotate { get; set; } = 0;

        [DisplayName("Шаги для сдвига на одну пробирку"), Category("3.2 Шаги (конвейер)")]
        public int ConveyorStepsPerSingleTube { get; set; } = 0;

        public ConveyorConfiguration()
        {

        }
    }
}
