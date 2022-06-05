using AnalyzerDomain.Models;
using AnalyzerService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Services
{
    /// <summary>
    /// Сервис управления кассетницей с картриджами
    /// </summary>
    public class CartridgesDeckService
    {
        public ObservableCollection<CartridgeCassette> Cassettes { get; private set; }

        public CartridgesDeckService(int deckSize)
        {
            Cassettes = new ObservableCollection<CartridgeCassette>(Enumerable.Repeat(new CartridgeCassette(), deckSize));
        }

        public void ScanCassette(int index)
        {
            Analyzer.Charger.HomeHook(false);
            Analyzer.Charger.HomeRotator();
            Analyzer.Charger.TurnToCell(index);
            Analyzer.Charger.TurnScanner(true);
            Analyzer.Charger.ScanBarcode();
            System.Threading.Thread.Sleep(2000); // Типа ожидаем, когда бар-код будет прочитан
            Analyzer.Charger.TurnScanner(false);
        }
    }

    
}
