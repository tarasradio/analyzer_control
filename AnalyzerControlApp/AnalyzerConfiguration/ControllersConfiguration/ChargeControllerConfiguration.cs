using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerConfiguration.ControllersConfiguration
{
    public class ChargeControllerConfiguration
    {
        [Category("1. Двигатели")]
        [DisplayName("Двигатель поворота загрузки")]
        public int RotatorStepper { get; set; }
        [Category("1. Двигатели")]
        [DisplayName("Двигатель крюка")]
        public int HookStepper { get; set; }

        [Category("2.1 Скорость двигателя поворота загрузки")]
        [DisplayName("Скорость движения загрузки домой")]
        public int RotatorHomeSpeed { get; set; }
        [Category("2.1 Скорость двигателя поворота загрузки")]
        [DisplayName("Скорость движения загрузки")]
        public int RotatorSpeed { get; set; }

        [Category("2.2 Скорость двигателя крюка")]
        [DisplayName("Скорость движения крюка домой")]
        public int HookHomeSpeed { get; set; }
        [Category("2.2 Скорость двигателя крюка")]
        [DisplayName("Скорость движения крюка")]
        public int HookSpeed { get; set; }

        [Category("3.1 Шаги двигателя поворота загрузки")]
        [DisplayName("Шаги до ячеек с кассетами")]
        public int[] CellsSteps { get; set; }

        [Category("3.1 Шаги двигателя поворота загрузки")]
        [DisplayName("Шаги до разгрузки")]
        public int StepsToUnload { get; set; }

        [Category("3.1 Шаги двигателя поворота загрузки")]
        [DisplayName("Шаги для отъезда при загрузке")]
        public int RotatorStepsLeaveAtCharge { get; set; }

        [Category("3.2 Шаги двигателя крюка")]
        [DisplayName("Шаги движения крюка к картриджу")]
        public int HookStepsToCartridge { get; set; }

        [Category("3.2 Шаги двигателя крюка")]
        [DisplayName("Шаги движения крюка после возврата домой")]
        public int HookStepsAfterHome { get; set; }

        public ChargeControllerConfiguration()
        {

        }
    }
}
