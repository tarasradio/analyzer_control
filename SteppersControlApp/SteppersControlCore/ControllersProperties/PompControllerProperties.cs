using System;
using System.ComponentModel;

namespace SteppersControlCore.ControllersProperties
{
    public class PompControllerProperties
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель большого плунжера")]
        public int BigPistonStepper { get; set; } = 11;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель малого плунжера")]
        public int SmallPistonStepper { get; set; } = 12;

        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера домой")]
        public int BigPistonHomeSpeed { get; set; } = 950;
        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера")]
        public int BigPistonSpeed { get; set; } = 50;
        [Category("2.1 Скорость большого плунжера")]
        [DisplayName("Скорость движения большого плунжера при промывке")]
        public int BigPistonWashingSpeed { get; set; } = 280;

        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера домой")]
        public int SmallPistonHomeSpeed { get; set; } = 950;
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера")]
        public int SmallPistonSpeed { get; set; } = 200;
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера при промывке")]
        public int SmallPistonWashingSpeed { get; set; } = 280;
        [Category("2.2 Скорость малого плунжера")]
        [DisplayName("Скорость движения малого плунжера при заборе")]
        public int SmallPistonSuctionSpeed { get; set; } = 200;

        [Category("3.1 Шаги малого плунжера")]
        [DisplayName("Шаги малого плунжера при заборе")]
        public int SmallPistonSuctionSteps { get; set; } = -200000;

        [Category("3.2 Шаги большого плунжера")]
        [DisplayName("Шаги большого плунжера при промывке")]
        public int BigPistonWashingSteps { get; set; } = -700000;
        [Category("3.1 Шаги малого плунжера")]
        [DisplayName("Шаги малого плунжера при промывке")]
        public int SmallPistonWashingSteps { get; set; } = -700000;


        public PompControllerProperties()
        {

        }
    }
}
