namespace AnalyzerDomain.Entyties
{
    public enum CartridgeCell
    {
        FirstCell, // Ячейка с первым реагентом
        SecondCell, // Ячейка со вторым реагентом
        ThirdCell, // Ячейка с третьим реагентом
        MixCell, // Белая ячейка, в которой происходит смешивание реагентов
        ResultCell // Прозрачная ячейка, куда помещается конечный результат
    };
}
