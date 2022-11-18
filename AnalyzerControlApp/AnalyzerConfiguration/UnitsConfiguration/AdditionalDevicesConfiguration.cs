using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerConfiguration.UnitsConfiguration
{
    public class AdditionalDevicesConfiguration
    {
        [DisplayName("Двигатель поворота экрана"), Category("1. Номера двигателей")]
        public int ScreenTurnStepper { get; set; } = 0;
        [DisplayName("Двигатель подъема / опускания экрана"), Category("1. Номера двигателей")]
        public int ScreenUpDownStepper { get; set; } = 0;

        [DisplayName("Двигатель подъема / опускания wash-буффера"), Category("1. Номера двигателей")]
        public int WashBufferUpDownStepper { get; set; } = 0;

        [DisplayName("Скорость движения wash-буффера домой"), Category("2. Скорости движения")]
        public int WashBufferHomeSpeed { get; set; } = 100;
        [DisplayName("Скорость движения wash-буффера при опускании"), Category("2. Скорости движения")]
        public int WashBufferPutDownSpeed { get; set; } = 100;
        [DisplayName("Шагов для опускания wash-буффера"), Category("3. Шаги")]
        public int WashBufferPutDownSteps { get; set; } = 0;

        public AdditionalDevicesConfiguration()
        {

        }
    }
}
