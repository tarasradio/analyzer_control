using AnalyzerControlGUI.Commands;
using AnalyzerControlGUI.Models;
using System.Collections.ObjectModel;

namespace AnalyzerControlGUI.ViewModels
{
    public class AnalyzerControlViewModel : ViewModel
    {
        private const int cassettesCount = 10;
        private const int conveyorCellsCount = 55;

        public ObservableCollection<Cassette> Cassettes { get; set; }
        public ObservableCollection<ConveyorCell> ConveyorCells { get; set; }

        RelayCommand _wtfCommand;

        public RelayCommand WtfCommand
        {
            get
            {
                if (_wtfCommand == null)
                {
                    _wtfCommand = new RelayCommand(
                       param => { wtf(); },
                       param => { return (ConveyorCells != null); });
                }
                return _wtfCommand;
            }
        }

        public void wtf()
        {
            Cassettes[1].CountLeft = 6;
            Cassettes[4].CountLeft = 2 ;
            Cassettes[5].CountLeft = 3 ;

            if (ConveyorCells[4].State == ConveyorCellState.Empty)
            {
                ConveyorCells[4].State = ConveyorCellState.Error;
            } else
            {
                ConveyorCells[4].State = ConveyorCellState.Empty;
            }
        }

        public AnalyzerControlViewModel()
        {
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

            for (int i = 0; i < conveyorCellsCount; ++i)
                ConveyorCells.Add(new ConveyorCell());

            ConveyorCells[5].State = ConveyorCellState.Processed;
            ConveyorCells[6].State = ConveyorCellState.Error;
            ConveyorCells[7].State = ConveyorCellState.Processing;
        }

    }
}
