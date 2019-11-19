using System;
using System.ComponentModel;

namespace SteppersControlCore.ControllersProperties
{
    public class TransporterControllerProperties
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель вращения пробирки")]
        public int TurnTubeStepper { get; set; } = 5;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель движения пробирок")]
        public int TransporterStepper { get; set; } = 6;

        [Category("2. Скорость")]
        [DisplayName("Скорость вращения пробирки")]
        public int TurnTubeSpeed { get; set; } = 30;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения конвейера")]
        public int TransporterMoveSpeed { get; set; } = 50;
        [Category("2. Скорость")]
        [DisplayName("Скорость движения конвейера домой")]
        public int TransporterHomeSpeed { get; set; } = 100;

        [Category("3. Шаги")]
        [DisplayName("Шагов для сдвига на одну пробирку")]
        public int StepsPerTube { get; set; } = 6400;
        [Category("3. Шаги")]
        [DisplayName("Шагов для вращения пробирки")]
        public int StepsPerTubeRotate { get; set; } = 10000;

        public TransporterControllerProperties()
        {

        }
    }
}
