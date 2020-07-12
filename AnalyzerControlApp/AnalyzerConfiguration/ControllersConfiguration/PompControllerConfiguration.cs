using System.ComponentModel;

namespace AnalyzerConfiguration.ControllersConfiguration
{
    public class PompControllerConfiguration
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель большого плунжера")]
        public int BigPistonStepper { get; set; }
        [Category("1. Двигатели")]
        [DisplayName("Двигатель малого плунжера")]
        public int SmallPistonStepper { get; set; }

        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера домой")]
        public int BigPistonHomeSpeed { get; set; }
        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера")]
        public int BigPistonSpeed { get; set; }
        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера при промывке")]
        public int BigPistonWashingSpeed { get; set; }

        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера домой")]
        public int SmallPistonHomeSpeed { get; set; }
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера")]
        public int SmallPistonSpeed { get; set; }
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера при промывке")]
        public int SmallPistonWashingSpeed { get; set; }
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера при заборе")]
        public int SmallPistonSuctionSpeed { get; set; }

        [Category("3.1 Шаги малого плунжера")]
        [DisplayName("Шаги малого плунжера при заборе")]
        public int SmallPistonSuctionSteps { get; set; }

        [Category("3.2 Шаги большого плунжера")]
        [DisplayName("Шаги большого плунжера при промывке")]
        public int BigPistonWashingSteps { get; set; }
        [Category("3.1 Шаги малого плунжера")]
        [DisplayName("Шаги малого плунжера при промывке")]
        public int SmallPistonWashingSteps { get; set; }


        public PompControllerConfiguration()
        {

        }
    }
}
