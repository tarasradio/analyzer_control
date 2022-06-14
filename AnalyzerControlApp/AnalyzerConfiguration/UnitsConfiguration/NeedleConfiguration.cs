using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerConfiguration.UnitsConfiguration
{
    public class NeedleConfiguration
    {
        [DisplayName("Двигатель подъема / опускания"), Category("1. Номера двигателей")]
        public int LifterStepper { get; set; } = 0;
        [DisplayName("Двигатель поворота"), Category("1. Номера двигателей")]
        public int RotatorStepper { get; set; } = 0;

        [DisplayName("Скорость подъема / опускания"), Category("2. Скорость (подъем / опускание)")]
        public int LifterSpeed { get; set; } = 0;
        [DisplayName("Скорость поворота"), Category("2. Скорость (поворот)")]
        public int RotatorSpeed { get; set; } = 0;

        [DisplayName("Шагов до пробирки"), Category("3.1 Шаги (поворот)")]
        public int RotatorStepsToTube { get; set; } = 0;
        [DisplayName("Шагов до промывки (вода)"), Category("3.1 Шаги (поворот)")]
        public int RotatorStepsToWashing { get; set; } = 0;
        [DisplayName("Шагов до промывки (щёлочь)"), Category("3.1 Шаги (поворот)")]
        public int RotatorStepsToAlkaliWashing { get; set; } = 0;
        [DisplayName("Шагов до прозрачной ячейки"), Category("3.1 Шаги (поворот)")]
        public int RotatorStepsToResultCell { get; set; } = 0;
        [DisplayName("Шагов до белой ячейки"), Category("3.1 Шаги (поворот)")]
        public int RotatorStepsToMixCell { get; set; } = 0;
        [DisplayName("Шагов до 1-й ячейки"), Category("3.1 Шаги (поворот)")]
        public int RotatorStepsToFirstCell { get; set; } = 0;
        [DisplayName("Шагов до 2-й ячейки"), Category("3.1 Шаги (поворот)")]
        public int RotatorStepsToSecondCell { get; set; } = 0;
        [DisplayName("Шагов до 3-й ячейки"), Category("3.1 Шаги (поворот)")]
        public int RotatorStepsToThirdCell { get; set; } = 0;

        [DisplayName("Шагов после касания жидкости в пробирке"), Category("3.2 Шаги (подъем / опускание)")]
        public int LifterStepsAfterTouch { get; set; } = 0;
        [DisplayName("Шагов опускания до промывки"), Category("3.2 Шаги (подъем / опускание)")]
        public int LifterStepsToWashing { get; set; } = 0;
        [DisplayName("Шагов для опускания до 1-3-й ячеек"), Category("3.2 Шаги (подъем / опускание)")]
        public int LifterStepsToCell { get; set; } = 0;
        [DisplayName("Шагов для опускания до прозрачной ячейки"), Category("3.2 Шаги (подъем / опускание)")]
        public int LifterStepsToResultCell { get; set; } = 0;
        [DisplayName("Шагов для опускания до белой ячейки"), Category("3.2 Шаги (подъем / опускание)")]
        public int LifterStepsToMixCell { get; set; } = 0;
        [DisplayName("Шагов для опускания до белой ячейки (при заборе)"), Category("3.2 Шаги (подъем / опускание)")]
        public int LifterStepsToMixCellAtSuction { get; set; } = 0;
        [DisplayName("Шагов до безопасной высоты над картриджем"), Category("3.2 Шаги (подъем / опускание)")]
        public int LifterStepsToSafeLevel { get; set; } = 0;

        public NeedleConfiguration()
        {

        }
    }
}
