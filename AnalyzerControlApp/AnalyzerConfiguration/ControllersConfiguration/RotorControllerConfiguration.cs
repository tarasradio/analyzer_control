using System.ComponentModel;

namespace AnalyzerConfiguration.ControllersConfiguration
{
    public class RotorControllerConfiguration
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель вращения ротора")]
        public int RotorStepper { get; set; }

        [Category("2. Скорость")]
        [DisplayName("Скорость ротора при движении домой")]
        public int RotorHomeSpeed { get; set; }

        [Category("2. Скорость")]
        [DisplayName("Скорость ротора при движении")]
        public int RotorSpeed { get; set; }

        [Category("3.1 Шаги")]
        [DisplayName("Шагов на одну ячейку")]
        public int StepsPerCell { get; set; }

        [Category("3.1 Шаги")]
        [DisplayName("Шагов до загрузки (картриджа)")]
        public int[] StepsToLoad { get; set; }

        [Category("3.1 Шаги")]
        [DisplayName("Шагов до выгрузки")]
        public int StepsToUnload { get; set; }

        [Category("3.1 Шаги")]
        [DisplayName("Шагов до wash-буфера")]
        public int StepsToWashBuffer { get; set; }

#region StepsToNeedle

        [Category("3.2 Шаги до иглы")]
        [DisplayName("Шагов до иглы белой ячейки (центр)")]
        public int StepsToNeedleWhiteCenter { get; set; }

        [Category("3.2 Шаги до иглы")]
        [DisplayName("Шагов до иглы 1-й ячейки (лево)")]
        public int StepsToNeedleLeft1 { get; set; }
        [Category("3.2 Шаги до иглы")]
        [DisplayName("Шагов до иглы 1-й ячейки (право)")]
        public int StepsToNeedleRight1 { get; set; }

        [Category("3.2 Шаги до иглы")]
        [DisplayName("Шагов до иглы 2-й ячейки (лево)")]
        public int StepsToNeedleLeft2 { get; set; }
        [Category("3.2 Шаги до иглы")]
        [DisplayName("Шагов до иглы 2-й ячейки (право)")]
        public int StepsToNeedleRight2 { get; set; }

        [Category("3.2 Шаги до иглы")]
        [DisplayName("Шагов до иглы 3-й ячейки (лево)")]
        public int StepsToNeedleLeft3 { get; set; }
        [Category("3.2 Шаги до иглы")]
        [DisplayName("Шагов до иглы 3-й ячейки (право)")]
        public int StepsToNeedleRight3 { get; set; }

#endregion

        public RotorControllerConfiguration()
        {

        }
    }
}
