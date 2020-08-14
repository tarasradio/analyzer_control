using System.ComponentModel;

namespace AnalyzerConfiguration.UnitsConfiguration
{
    public class ChargerConfiguration
    {
        [DisplayName("Двигатель поворота загрузки"), Category("1. Номера двигателей")]
        public int RotatorStepper { get; set; } = 0;
        [DisplayName("Двигатель крюка"), Category("1. Номера двигателей")]
        public int HookStepper { get; set; } = 0;

        [DisplayName("Скорость движения загрузки"), Category("2.1 Скорость (поворот загрузки)")]
        public int RotatorSpeed { get; set; } = 0;
        [DisplayName("Скорость движения загрузки домой"), Category("2.1 Скорость (поворот загрузки)")]
        public int RotatorHomeSpeed { get; set; } = 0;

        [DisplayName("Скорость движения крюка"), Category("2.2 Скорость (крюк)")]
        public int HookSpeed { get; set; } = 0;
        [DisplayName("Скорость движения крюка домой"), Category("2.2 Скорость (крюк)")]
        public int HookHomeSpeed { get; set; } = 0;

        [DisplayName("Шаги до ячеек с кассетами"), Category("3.1 Шаги (поворот загрузки)")]
        public int[] RotatorStepsToCells { get; set; } = new int[10];
        [DisplayName("Шаги до разгрузки"), Category("3.1 Шаги (поворот загрузки)")]
        public int RotatorStepsToUnload { get; set; } = 0;
        [DisplayName("Шаги для отъезда при загрузке"), Category("3.1 Шаги (поворот загрузки)")]
        public int RotatorStepsToOffsetAtCharging { get; set; } = 0;
        [DisplayName("Шаги движения крюка к картриджу"), Category("3.2 Шаги (крюк)")]
        public int HookStepsToCartridge { get; set; } = 0;
        [DisplayName("Шаги движения крюка после возврата домой"), Category("3.2 Шаги (крюк)")]
        public int HookStepsToOffsetAfterHoming { get; set; } = 0;

        public ChargerConfiguration()
        {

        }
    }
}
