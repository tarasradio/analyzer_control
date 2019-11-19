using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.ControllersProperties
{
    public class NeedleControllerProperties
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель подъема")]
        public int LiftStepper { get; set; } = 17;

        [Category("1. Двигатели")]
        [DisplayName("Двигатель поворота")]
        public int RotatorStepper { get; set; } = 8;


        [Category("2. Скорость")]
        [DisplayName("Скорость подъема / опускания")]
        public int LiftSpeed { get; set; } = 500;
        [Category("2. Скорость")]
        [DisplayName("Скорость поворота")]
        public int RotatorSpeed { get; set; } = 50;

        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до пробирки")]
        public int RotatorStepsTurnToTube { get; set; } = 15500;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до промывки")]
        public int RotatorStepsTurnToWashing { get; set; } = 55900;


        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до белой ячейки")]
        public int RotatorStepsTurnToMixCell { get; set; } = 46600;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 1-й ячейки")]
        public int RotatorStepsTurnToFirstCell { get; set; } = 47000;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 2-й ячейки")]
        public int RotatorStepsTurnToSecondCell { get; set; } = 48000;
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 3-й ячейки")]
        public int RotatorStepsTurnToThirdCell { get; set; } = 48900;

        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов после касания жидкости в пробирке")]
        public int LiftStepsGoDownAfterTouch { get; set; } = 500;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов опускания до промывки")]
        public int LiftStepsGoDownToWashing { get; set; } = 5500;

        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до 1-3-й ячеек")]
        public int LiftStepsGoDownToCell { get; set; } = 297000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до белой ячейки")]
        public int LiftStepsGoDownToMixCell { get; set; } = 272000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до белой ячейки (при заборе)")]
        public int LiftStepsGoDownToMixCellAtSuction { get; set; } = 272000;
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов до безопасной высоты над картриджем")]
        public int LiftStepsGoDownToSafeLevel { get; set; } = 0;

        public NeedleControllerProperties()
        {

        }
    }
}
