using System.ComponentModel;

namespace SteppersControlCore.ControllersProperties
{
    public class RotorControllerProperties
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель ротора")]
        public int RotorStepper { get; set; } = 7;

        [Category("2. Скорость")]
        [DisplayName("Скорость ротора при движении домой")]
        public int RotorHomeSpeed { get; set; } = 100;

        [Category("2. Скорость")]
        [DisplayName("Скорость ротора при движении")]
        public int RotorSpeed { get; set; } = 50;

        [Category("3. Шаги")]
        [DisplayName("Шагов на одну ячейку")]
        public int StepsPerCell { get; set; } = 3400;

        [Category("3. Шаги")]
        [DisplayName("Шагов до загрузки (картриджа)")]
        public int[] StepsToLoad { get; set; } =
        {
            20000,
            20000,
            20000,
            20000,
            20000,
            20000,
            20000,
            20000,
            20000,
            20000
        };

        [Category("3. Шаги")]
        [DisplayName("Шагов до выгрузки")]
        public int StepsToUnload { get; set; } = 1000;

        [Category("3. Шаги")]
        [DisplayName("Шагов до wash-буфера")]
        public int StepsToWashBuffer { get; set; } = 1000;

        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы белой ячейки (центр)")]
        public int StepsToNeedleWhiteCenter { get; set; } = 10900;

        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 1-й ячейки (лево)")]
        public int StepsToNeedleLeft1 { get; set; } = 9750;
        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 1-й ячейки (право)")]
        public int StepsToNeedleRight1 { get; set; } = 10700;

        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 2-й ячейки (лево)")]
        public int StepsToNeedleLeft2 { get; set; } = 9200;
        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 2-й ячейки (право)")]
        public int StepsToNeedleRight2 { get; set; } = 9650;

        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 3-й ячейки (лево)")]
        public int StepsToNeedleLeft3 { get; set; } = 8630;
        [Category("3. Шаги")]
        [DisplayName("Шагов до иглы 3-й ячейки (право)")]
        public int StepsToNeedleRight3 { get; set; } = 9200;

        public RotorControllerProperties()
        {

        }
    }
}
