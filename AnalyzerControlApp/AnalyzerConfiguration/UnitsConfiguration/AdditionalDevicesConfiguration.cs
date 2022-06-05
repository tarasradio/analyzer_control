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

        public AdditionalDevicesConfiguration()
        {

        }
    }
}
