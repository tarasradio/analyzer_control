using System.ComponentModel;

namespace AnalyzerConfiguration.UnitsConfiguration
{
    public class RotorConfiguration
    {
        [DisplayName("Двигатель вращения ротора"), Category("1. Номера двигателей")]
        public int RotorStepper { get; set; } = 0;

        [DisplayName("Скорость ротора при движении домой"), Category("2. Скорость")]
        public int RotorHomeSpeed { get; set; } = 0;
        [DisplayName("Скорость ротора при движении"), Category("2. Скорость")]
        public int RotorSpeed { get; set; } = 0;

        [DisplayName("Шагов на одну ячейку"), Category("3.1 Шаги (общие задачи)")]
        public int StepsPerCell { get; set; } = 0;
        [DisplayName("Шагов до загрузки (картриджа)"), Category("3.1 Шаги (общие задачи)")]
        public int[] StepsToLoad { get; set; } = new int[10];
        [DisplayName("Шагов до выгрузки"), Category("3.1 Шаги (общие задачи)")]
        public int StepsToUnload { get; set; } = 0;
        [DisplayName("Шагов до wash-буфера"), Category("3.1 Шаги (общие задачи)")]
        public int StepsToWashBuffer { get; set; } = 0;
        [DisplayName("Шагов до оптического блока"), Category("3.1 Шаги (общие задачи)")]
        public int StepsToOM { get; set; } = 0;

        [DisplayName("Шагов до иглы прозрачной ячейки (центр)"), Category("3.2 Шаги (работа с иглой)")]
        public int StepsToNeedleResultCenter { get; set; } = 0;
        [DisplayName("Шагов до иглы белой ячейки (центр)"), Category("3.2 Шаги (работа с иглой)")]
        public int StepsToNeedleWhiteCenter { get; set; } = 0;
        [DisplayName("Шагов до иглы 1-й ячейки (лево)"), Category("3.2 Шаги (работа с иглой)")]
        public int StepsToNeedleLeft1 { get; set; } = 0;
        [DisplayName("Шагов до иглы 1-й ячейки (право)"), Category("3.2 Шаги (работа с иглой)")]
        public int StepsToNeedleRight1 { get; set; } = 0;
        [DisplayName("Шагов до иглы 2-й ячейки (лево)"), Category("3.2 Шаги (работа с иглой)")]
        public int StepsToNeedleLeft2 { get; set; } = 0;
        [DisplayName("Шагов до иглы 2-й ячейки (право)"), Category("3.2 Шаги (работа с иглой)")]
        public int StepsToNeedleRight2 { get; set; } = 0;
        [DisplayName("Шагов до иглы 3-й ячейки (лево)"), Category("3.2 Шаги (работа с иглой)")]
        public int StepsToNeedleLeft3 { get; set; } = 0;
        [DisplayName("Шагов до иглы 3-й ячейки (право)"), Category("3.2 Шаги (работа с иглой)")]
        public int StepsToNeedleRight3 { get; set; } = 0;
        
        public RotorConfiguration()
        {

        }
    }
}
