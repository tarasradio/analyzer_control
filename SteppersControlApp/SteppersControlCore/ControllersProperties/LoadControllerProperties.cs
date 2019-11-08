using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.ControllersProperties
{
    public class LoadControllerProperties
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель поворота загрузки")]
        public int LoadStepper { get; set; } = 10;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель челнока")]
        public int ShuttleStepper { get; set; } = 15;
        [Category("1. Двигатели")]
        [DisplayName("Двигатель толкателя")]
        public int PushStepper { get; set; } = 0;

        [Category("2.1 Скорость двигателя поворота загрузки")]
        [DisplayName("Скорость движения загрузки домой")]
        public int LoadStepperHomeSpeed { get; set; } = 50;
        [Category("2.1 Скорость двигателя поворота загрузки")]
        [DisplayName("Скорость движения загрузки")]
        public int LoadStepperSpeed { get; set; } = 50;

        [Category("2.2 Скорость двигателя челнока")]
        [DisplayName("Скорость движения челнока домой")]
        public int ShuttleStepperHomeSpeed { get; set; } = 500;
        [Category("2.2 Скорость двигателя челнока")]
        [DisplayName("Скорость движения челнока")]
        public int ShuttleStepperSpeed { get; set; } = 200;

        [Category("2.3 Скорость двигателя толкателя")]
        [DisplayName("Скорость движения толкателя домой")]
        public int PushStepperHomeSpeed { get; set; } = 500;
        [Category("2.3 Скорость двигателя толкателя")]
        [DisplayName("Скорость движения толкателя")]
        public int PushStepperSpeed { get; set; } = 200;

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

        [Category("3.2 Шаги двигателя челнока")]
        [DisplayName("Шаги отъезда челнока от дома")]
        public int StepsShuttleToStart { get; set; } = -20000;

        [Category("3.2 Шаги двигателя челнока")]
        [DisplayName("Шаги движения челнока к кассете")]
        public int StepsShuttleToCassette { get; set; } = -2840000;

        public LoadControllerProperties()
        {

        }
    }
}
