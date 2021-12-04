using System.ComponentModel;

namespace AnalyzerDomain.Entyties
{
    public class Stage
    {
        [DisplayName("Номер картриджа")]
        public int CartridgePosition { get; set; } = 0;
        [DisplayName("Номер ячейки в картридже")]
        public Models.CartridgeCell Cell { get; set; } = Models.CartridgeCell.MixCell;
        [DisplayName("Время выполнения (минут)")]
        public int TimeToPerform { get; set; } = 0;

        public Stage()
        {
            Cell = Models.CartridgeCell.MixCell;
        }
    }
}
