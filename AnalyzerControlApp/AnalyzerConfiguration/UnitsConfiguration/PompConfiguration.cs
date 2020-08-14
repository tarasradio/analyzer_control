using System.ComponentModel;

namespace AnalyzerConfiguration.UnitsConfiguration
{
    public class PompConfiguration
    {
        [DisplayName("Двигатель большого плунжера"), Category("1. Номера двигателей")]
        public int BigPistonStepper { get; set; } = 0;
        [DisplayName("Двигатель малого плунжера"), Category("1. Номера двигателей")]
        public int SmallPistonStepper { get; set; } = 0;

        [DisplayName("Скорость движения большого плунжера"), Category("2.1 Скорость (большой плунжер)")]
        public int BigPistonSpeed { get; set; }
        [DisplayName("Скорость движения большого плунжера домой"), Category("2.1 Скорость (большой плунжер)")]
        public int BigPistonSpeedAtHoming { get; set; }
        [DisplayName("Скорость движения большого плунжера при промывке"), Category("2.1 Скорость (большой плунжер)")]
        public int BigPistonSpeedAtWashing { get; set; }

        [DisplayName("Скорость движения малого плунжера"), Category("2.2 Скорость (малый плунжер)")]
        public int SmallPistonSpeed { get; set; }
        [DisplayName("Скорость движения малого плунжера домой"), Category("2.2 Скорость (малый плунжер)")]
        public int SmallPistonSpeedAtHoming { get; set; }
        [DisplayName("Скорость движения малого плунжера при промывке"), Category("2.2 Скорость (малый плунжер)")]
        public int SmallPistonSpeedAtWashing { get; set; }
        [DisplayName("Скорость движения малого плунжера при заборе"), Category("2.2 Скорость (малый плунжер)")]
        public int SmallPistonSpeedAtSuction { get; set; }

        [DisplayName("Шаги большого плунжера при промывке"), Category("3.1 Шаги (большой плунжер)")]
        public int BigPistonStepsAtWashing { get; set; }

        [DisplayName("Шаги малого плунжера при заборе"), Category("3.2 Шаги (малый плунжер)")]
        public int SmallPistonStepsAtSuction { get; set; }
        [DisplayName("Шаги малого плунжера при промывке"), Category("3.2 Шаги (малый плунжер)")]
        public int SmallPistonStepsAtWashing { get; set; }


        public PompConfiguration()
        {

        }
    }
}
