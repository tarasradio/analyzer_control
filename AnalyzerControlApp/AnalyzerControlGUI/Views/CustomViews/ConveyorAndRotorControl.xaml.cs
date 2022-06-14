using AnalyzerControlGUI.ViewsHelpers;
using AnalyzerDomain.Models;
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
    public partial class ConveyorAndRotorControl : UserControl
    {
        private ConveyorController conveyor;
        private RotorController rotor;

        readonly DispatcherTimer timer = new DispatcherTimer();

        #region ConveyorCellsDefinition
        public static readonly DependencyProperty ConveyorCellsProperty 
            = DependencyProperty.Register(
                "ConveyorCells",
                typeof(ObservableCollection<ConveyorCell>),
                typeof(ConveyorAndRotorControl),
                new PropertyMetadata(null, new PropertyChangedCallback(ConveyorCellsChanged))
                );

        private static void ConveyorCellsChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            ConveyorAndRotorControl s = (ConveyorAndRotorControl)depObj;
            s.CreateHelper();
        }

        public ObservableCollection<ConveyorCell> ConveyorCells
        {
            get => (ObservableCollection<ConveyorCell>)GetValue(ConveyorCellsProperty);
            set {
                SetValue(ConveyorCellsProperty, value);
            }
        }
        #endregion

        #region RotorCellsDefinition
        public static readonly DependencyProperty RotorCellsProperty
            = DependencyProperty.Register(
                "RotorCells",
                typeof(ObservableCollection<RotorCell>),
                typeof(ConveyorAndRotorControl),
                new PropertyMetadata(null, new PropertyChangedCallback(RotorCellsChanged))
                );

        private static void RotorCellsChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            ConveyorAndRotorControl s = (ConveyorAndRotorControl)depObj;
            s.CreateHelper();
        }

        public ObservableCollection<RotorCell> RotorCells
        {
            get => (ObservableCollection<RotorCell>)GetValue(RotorCellsProperty);
            set
            {
                SetValue(RotorCellsProperty, value);
            }
        }
        #endregion

        public ConveyorAndRotorControl()
        {
            InitializeComponent();
        }

        public void CreateHelper()
        {
            if(ConveyorCells != null && RotorCells != null)
            {
                conveyor = new ConveyorController(CanvasTubes, 0.01, 1.7, ConveyorCells);
                rotor = new RotorController(CanvasTubes, 0.01, 1.7, RotorCells, ConveyorCells.Count);
                
                ConvHelp.DataContext = conveyor;

                timer.Tick += conveyor.ConveyorLoopStep;
                timer.Tick += rotor.RotorLoopStep;
                timer.Interval = TimeSpan.FromMilliseconds(30);
                timer.Start();
            }
        }
    }
}
