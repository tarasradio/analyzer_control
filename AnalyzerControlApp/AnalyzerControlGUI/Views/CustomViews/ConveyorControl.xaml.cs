using AnalyzerControlGUI.ViewsHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public ConveyorControl()
        {
            InitializeComponent();

            conveyor = new ConveyorHelper(CanvasTubes, 0.01, 1.7, 55);
            ConvHelp.DataContext = conveyor;

            tubeTimer.Tick += conveyor.TubeLoopStep;
            tubeTimer.Interval = TimeSpan.FromMilliseconds(30);
            tubeTimer.Start();
        }
    }
}
