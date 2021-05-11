using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Services
{
    public class CartridgeCassette
    {
        public string Barcode { get; set; }
        public int CountLeft { get; set; }
        public int Volume { get; set; }

        public bool IsEmpty {
            get {
                return Barcode == string.Empty;
            }
            private set { }
        }

        public CartridgeCassette() {
            Barcode = string.Empty;
        }

        public void SetEmpty() {
            Barcode = string.Empty;
        }
    }
    /// <summary>
    /// Сервис управления кассетницей с картриджами
    /// </summary>
    public class CartridgesDeckService
    {
        public CartridgeCassette[] Cassettes { get; private set; }

        public CartridgesDeckService(int deckSize)
        {
            Cassettes = Enumerable.Repeat(new CartridgeCassette(), deckSize).ToArray();
        }
    }
}
