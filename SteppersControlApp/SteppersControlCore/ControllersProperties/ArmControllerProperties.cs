using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.ControllersProperties
{
    public class ArmControllerProperties
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель подъема")]
        public int LiftStepper { get; set; } = 17;

        [Category("1. Двигатели")]
        [DisplayName("Двигатель поворота")]
        public int TurnStepper { get; set; } = 8;


        [Category("2. Скорость")]
        [DisplayName("Скорость подъема / опускания")]
        public int LiftStepperSpeed { get; set; } = 500;
        [Category("2. Скорость")]
        [DisplayName("Скорость поворота")]
        public int TurnStepperSpeed { get; set; } = 50;

        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до пробирки")]
        public int StepsToTube { get; set; } = 15500;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до промывки")]
        public int StepsTurnToWashing { get; set; } = 55900;


        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до белой ячейки")]
        public int StepsToMixCell { get; set; } = 46600;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 1-й ячейки")]
        public int StepsToFirstCell { get; set; } = 47000;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 2-й ячейки")]
        public int StepsToSecondCell { get; set; } = 48000;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 3-й ячейки")]
        public int StepsToThirdCell { get; set; } = 48900;

        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов после касания жидкости в пробирке")]
        public int StepsToTubeAfterTouch { get; set; } = 500;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов опускания до промывки")]
        public int StepsDownToWashing { get; set; } = 5500;

        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до 1-3-й ячеек")]
        public int StepsDownToCell { get; set; } = 297000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до белой ячейки")]
        public int StepsDownToMixCell { get; set; } = 272000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до белой ячейки (при заборе)")]
        public int StepsDownToMixCellOnSuction { get; set; } = 272000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов не доходя до картриджа")]
        public int StepsOnBroke { get; set; } = 70000;

        public ArmControllerProperties()
        {

        }
    }
}
