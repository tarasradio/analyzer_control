using AnalyzerService;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl
{
    /// <summary>
    /// Выполнение базовых действий
    /// </summary>
    public static class AnalyzerOperations
    {
        public static void MoveAllToHome()
        {
            Logger.Debug($"Запуск возврата всех устройств в начальную позицию.");

            Analyzer.Needle.GoHome();
            Analyzer.Charger.HomeHook();
            Analyzer.Charger.MoveHookAfterHome();
            Analyzer.Charger.HomeRotator();
            Analyzer.Rotor.Home();
            Analyzer.Pomp.Home();

            Logger.Debug($"Возврат устройств в начальную позицию завершен.");
        }

        /// <summary>
        /// Промывка иглы
        /// </summary>
        public static void WashNeedle()
        {
            Logger.Debug($"Запуск промывки иглы.");

            Analyzer.Needle.HomeLifter();
            Analyzer.Needle.TurnAndGoDownToWashing();
            Analyzer.Pomp.WashTheNeedle(2);
            Analyzer.Pomp.Home();
            Analyzer.Pomp.CloseValves();

            Logger.Debug($"Промывка иглы завершена.");
        }

        /// <summary>
        /// Загрузка картриджа из кассетницы в ротор
        /// </summary>
        /// <param name="cartirdgePosition">Позиция ячейки в роторе</param>
        /// <param name="chargePosition">Позиция кассеты в кассетнице</param>
        public static void ChargeCartridge(int cartirdgePosition, int chargePosition)
        {
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellAtCharge(cartirdgePosition, chargePosition);

            Analyzer.Charger.HomeHook();
            Analyzer.Charger.MoveHookAfterHome();
            Analyzer.Charger.HomeRotator();

            Analyzer.Charger.TurnToCell(chargePosition);

            Analyzer.Charger.ChargeCartridge();
            Analyzer.Charger.HomeHook();
            Analyzer.Charger.MoveHookAfterHome();
        }

        /// <summary>
        /// Выгрузка картриджа из ротора после выполнения анализа
        /// </summary>
        /// <param name="cartridgePosition">Позиция ячейки в роторе</param>
        public static void DischargeCartridge(int cartridgePosition)
        {
            Analyzer.Rotor.Home();
            Analyzer.Rotor.PlaceCellAtDischarge(cartridgePosition);

            Analyzer.Charger.HomeHook();
            Analyzer.Charger.MoveHookAfterHome();
            Analyzer.Charger.HomeRotator();

            Analyzer.Charger.TurnToDischarge();

            //AnalyzerGateway.Charger.ChargeCartridge();
            //AnalyzerGateway.Charger.HomeHook();
            //AnalyzerGateway.Charger.MoveHookAfterHome();
        }
    }
}
