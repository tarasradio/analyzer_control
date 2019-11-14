using System;
using System.ComponentModel;

namespace SteppersControlCore.ControllersProperties
{
    public class LoadControllerProperties
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель поворота загрузки")]
        public int LoadStepper { get; set; } = 10;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель крюка")]
        public int ShuttleStepper { get; set; } = 15;

        [Category("2.1 Скорость двигателя поворота загрузки")]
        [DisplayName("Скорость движения загрузки домой")]
        public int LoadStepperHomeSpeed { get; set; } = 50;
        [Category("2.1 Скорость двигателя поворота загрузки")]
        [DisplayName("Скорость движения загрузки")]
        public int LoadStepperSpeed { get; set; } = 50;

        [Category("2.2 Скорость двигателя крюка")]
        [DisplayName("Скорость движения крюка домой")]
        public int ShuttleStepperHomeSpeed { get; set; } = 500;
        [Category("2.2 Скорость двигателя крюка")]
        [DisplayName("Скорость движения крюка")]
        public int ShuttleStepperSpeed { get; set; } = 200;

        [Category("3.1 Шаги двигателя поворота загрузки")]
        [DisplayName("Шаги до ячеек с кассетами")]
        public int[] CellsSteps { get; set; } =
        {
            800,
            4000,
            6800,
            10000,
            13200,
            16000,
            19300,
            22300,
            25300,
            28500
        };

        [Category("3.1 Шаги двигателя поворота загрузки")]
        [DisplayName("Шаги до разгрузки")]
        public int StepsToUnload { get; set; } = 0;

        [Category("3.1 Шаги двигателя поворота загрузки")]
        [DisplayName("Шаги для отъезда при загрузке")]
        public int StepsTurnToLoad { get; set; } = 1000;

        [Category("3.2 Шаги двигателя челнока")]
        [DisplayName("Шаги движения челнока к картриджу")]
        public int StepsShuttleToCartridge { get; set; } = -613000;

        public LoadControllerProperties()
        {

        }
    }
}
