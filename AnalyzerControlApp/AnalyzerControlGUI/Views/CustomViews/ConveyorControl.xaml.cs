using AnalyzerControlGUI.Models;
using AnalyzerControlGUI.ViewsHelpers;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AnalyzerControlGUI.Views.CustomViews
{
    /// <summary>
    /// Логика взаимодействия для ConveyorControl.xaml
    /// </summary>
    public partial class ConveyorControl : UserControl
    {
        private ConveyorHelper conveyor;
        readonly DispatcherTimer tubeTimer = new DispatcherTimer();

        public static readonly DependencyProperty CellsProperty 
            = DependencyProperty.Register(
                "Cells",
                typeof(ObservableCollection<ConveyorCell>),
                typeof(ConveyorControl),
                new PropertyMetadata(null, new PropertyChangedCallback(CellsChanged))
                );

        private static void CellsChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            ConveyorControl s = (ConveyorControl)depObj;
            s.CreateHelper();
        }

        public ObservableCollection<ConveyorCell> Cells
        {
            get => (ObservableCollection<ConveyorCell>)GetValue(CellsProperty);
            set {
                SetValue(CellsProperty, value);
            }
        }

        public ConveyorControl()
        {
            InitializeComponent();
        }

        public void CreateHelper()
        {
            if(Cells != null)
            {
                conveyor = new ConveyorHelper(CanvasTubes, 0.01, 1.7, Cells);
                ConvHelp.DataContext = conveyor;

                tubeTimer.Tick += conveyor.ConveyorLoopStep;
                tubeTimer.Interval = TimeSpan.FromMilliseconds(30);
                tubeTimer.Start();
            }
        }
    }
}
