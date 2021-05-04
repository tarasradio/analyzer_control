using AnalyzerControlGUI.Models;
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
        private const int conveyorCellsCount = 55;

        public ObservableCollection<Cassette> Cassettes { get; set; }
        public ObservableCollection<ConveyorCell> ConveyorCells { get; set; }

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

            ConveyorCells = new ObservableCollection<ConveyorCell>();

            for(int i = 0; i < conveyorCellsCount; ++i)
                ConveyorCells.Add(new ConveyorCell());

            ConveyorCells[5].State = ConveyorCellState.Processed;
            ConveyorCells[6].State = ConveyorCellState.Error;
            ConveyorCells[7].State = ConveyorCellState.Processing;

            cassettesLV.ItemsSource = Cassettes;
            ConveyorView.Cells = ConveyorCells;
        }
    }
}
