namespace AnalyzerService
{
    public interface IAnalyzerState
    {
        string TubeBarcode { get; set; }
        string CartridgeBarcode { get; set; }
        string FirmwareVersion { get; set; }
        ushort[] SensorsValues { get; set; }
        ushort[] SteppersStates { get; set; }
    }
}
