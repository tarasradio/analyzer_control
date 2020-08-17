
namespace AnalyzerDomain.Models
{
    public class AnalysisStage
    {
        public Cartridge.CartridgeCell Cell { get; set; } // Ячейка картриджа
        public int TimeToPerformInMinutes { get; set; } // Время на выполнение стадии

        public AnalysisStage()
        {

        }
    }
}
