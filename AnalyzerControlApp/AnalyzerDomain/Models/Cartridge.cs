namespace AnalyzerDomain.Models
{
    public class Cartridge
    {
        public enum CartridgeCell
        {
            FirstCell, // Ячейка с первым реагентом
            SecondCell, // Ячейка со вторым реагентом
            ThirdCell, // Ячейка с третьим реагентом
            MixCell, // Белая ячейка, в которой происходит смешивание реагентов
            ResultCell // Прозрачная ячейка, куда помещается конечный результат
        };

        public string Barcode { get; set; }
        public CartridgeModel Model { get; set; }

        public Cartridge()
        {

        }
    }
}
