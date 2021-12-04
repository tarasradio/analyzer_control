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
    public partial class ConveyorControl : UserControl
    {
        private ConveyorController conveyor;
        private RotorController rotor;
        readonly DispatcherTimer conveyorTimer = new DispatcherTimer();
        readonly DispatcherTimer rotorTimer = new DispatcherTimer();

        #region ConveyorCellsDefinition
        public static readonly DependencyProperty ConveyorCellsProperty 
            = DependencyProperty.Register(
                "ConveyorCells",
                typeof(ObservableCollection<ConveyorCell>),
                typeof(ConveyorControl),
                new PropertyMetadata(null, new PropertyChangedCallback(ConveyorCellsChanged))
                );

        private static void ConveyorCellsChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            ConveyorControl s = (ConveyorControl)depObj;
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
                typeof(ConveyorControl),
                new PropertyMetadata(null, new PropertyChangedCallback(RotorCellsChanged))
                );

        private static void RotorCellsChanged(DependencyObject depObj,
            DependencyPropertyChangedEventArgs args)
        {
            ConveyorControl s = (ConveyorControl)depObj;
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

        public ConveyorControl()
        {
            InitializeComponent();
        }

        public void CreateHelper()
        {
            if(ConveyorCells != null && RotorCells != null)
            {
                conveyor = new ConveyorController(CanvasTubes, 0.01, 1.7, ConveyorCells);
                rotor = new RotorController(CanvasTubes, 0.01, 1.7, RotorCells);
                
                ConvHelp.DataContext = conveyor;

                conveyorTimer.Tick += conveyor.ConveyorLoopStep;
                conveyorTimer.Interval = TimeSpan.FromMilliseconds(30);
                conveyorTimer.Start();

                rotorTimer.Tick += rotor.RotorLoopStep;
                rotorTimer.Interval = TimeSpan.FromMilliseconds(30);
                rotorTimer.Start();
            }
        }
    }
}
