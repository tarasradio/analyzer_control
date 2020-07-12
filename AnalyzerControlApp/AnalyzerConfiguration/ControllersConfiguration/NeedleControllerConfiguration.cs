using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerConfiguration.ControllersConfiguration
{
    public class NeedleControllerConfiguration
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель подъема")]
        public int LiftStepper { get; set; }

        [Category("1. Двигатели")]
        [DisplayName("Двигатель поворота")]
        public int RotatorStepper { get; set; }


        [Category("2. Скорость")]
        [DisplayName("Скорость подъема / опускания")]
        public int LiftSpeed { get; set; }
        [Category("2. Скорость")]
        [DisplayName("Скорость поворота")]
        public int RotatorSpeed { get; set; }

        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до пробирки")]
        public int RotatorStepsTurnToTube { get; set; }
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до промывки")]
        public int RotatorStepsTurnToWashing { get; set; }


        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до белой ячейки")]
        public int RotatorStepsTurnToMixCell { get; set; }
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 1-й ячейки")]
        public int RotatorStepsTurnToFirstCell { get; set; }
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 2-й ячейки")]
        public int RotatorStepsTurnToSecondCell { get; set; }
        [Category("3.1 Шаги двигателя поворота")]
        [DisplayName("Шагов до 3-й ячейки")]
        public int RotatorStepsTurnToThirdCell { get; set; }

        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов после касания жидкости в пробирке")]
        public int LiftStepsGoDownAfterTouch { get; set; }
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов опускания до промывки")]
        public int LiftStepsGoDownToWashing { get; set; }

        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до 1-3-й ячеек")]
        public int LiftStepsGoDownToCell { get; set; }
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до белой ячейки")]
        public int LiftStepsGoDownToMixCell { get; set; }
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов для опускания до белой ячейки (при заборе)")]
        public int LiftStepsGoDownToMixCellAtSuction { get; set; }
        [Category("3.2 Шаги двигателя подъема / опускания")]
        [DisplayName("Шагов до безопасной высоты над картриджем")]
        public int LiftStepsGoDownToSafeLevel { get; set; }

        public NeedleControllerConfiguration()
        {

        }
    }
}
