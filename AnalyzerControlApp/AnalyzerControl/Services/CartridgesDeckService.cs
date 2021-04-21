using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Services
{
    public class CartridgeCassette
    {
        public string CartridgeBarcode { get; set; }
        public int Amount { get; set; }
        public int Volume { get; set; }

        public bool IsEmpty {
            get {
                return CartridgeBarcode == string.Empty;
            }
            private set { }
        }

        public CartridgeCassette() {
            CartridgeBarcode = string.Empty;
        }

        public void SetEmpty() {
            CartridgeBarcode = string.Empty;
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
