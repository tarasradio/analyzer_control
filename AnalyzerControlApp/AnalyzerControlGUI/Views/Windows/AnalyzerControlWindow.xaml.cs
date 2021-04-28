using AnalyzerControlGUI.Views.CustomViews;
using AnalyzerControlGUI.ViewsHelpers;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace AnalyzerControlGUI
{

    public class Cassette
    {
        public string Barcode { get; set; } = "RedMary4590";
        public int CountLeft { get; set; } = 6;
    }
    /// <summary>
    /// Логика взаимодействия для AnalyzerControlWindow.xaml
    /// </summary>
    public partial class AnalyzerControlWindow : Window
    {
        private const int cassettesCount = 10;
        private ConveyorHelper conveyor;
        readonly DispatcherTimer tubeTimer = new DispatcherTimer();

        public ObservableCollection<Cassette> Cassettes { get; set; }

        public AnalyzerControlWindow()
        {
            InitializeComponent();

            Cassettes = new ObservableCollection<Cassette>
            {
                new Cassette { Barcode="12", CountLeft = 1 },
                new Cassette { Barcode="13", CountLeft = 2 },
                new Cassette { Barcode="14", CountLeft = 3 },
                new Cassette { Barcode="15", CountLeft = 4 },
                new Cassette { Barcode="16", CountLeft = 5 },
                new Cassette { Barcode="17", CountLeft = 6 },
                new Cassette { Barcode="18", CountLeft = 7 },
                new Cassette { Barcode="19", CountLeft = 8 },
                new Cassette { Barcode="20", CountLeft = 9 },
                new Cassette { Barcode="21", CountLeft = 10 },
            };

            cassettesLV.ItemsSource = Cassettes;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            conveyor = new ConveyorHelper(CanvasTubes, 0.01, 1.7, 55);
            ConvHelp.DataContext = conveyor;

            tubeTimer.Tick += conveyor.TubeLoopStep;
            tubeTimer.Interval = TimeSpan.FromMilliseconds(30);
            tubeTimer.Start();
        }
    }
}
