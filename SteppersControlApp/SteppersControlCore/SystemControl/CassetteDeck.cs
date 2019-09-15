using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.SystemControl
{
    /// <summary>
    /// Картридж.
    /// </summary>
    public class Cartridge
    {

    }

    /// <summary>
    /// Кассета с картриджами.
    /// </summary>
    public class CartridgesCassette
    {
        public List<Cartridge> cartridges;
    }

    /// <summary>
    /// Кассетница для кассет с картриджами.
    /// </summary>
    public class CassetteDeck
    {
        public List<CartridgesCassette> cassettes;
    }
}
