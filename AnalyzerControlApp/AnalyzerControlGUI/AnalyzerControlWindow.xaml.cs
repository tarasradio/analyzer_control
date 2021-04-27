using AnalyzerControlGUI.CustomControls;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalyzerControlGUI
{
    /// <summary>
    /// Логика взаимодействия для AnalyzerControlWindow.xaml
    /// </summary>
    public partial class AnalyzerControlWindow : Window
    {
        private ConveyorHelper conveyor;
        readonly DispatcherTimer tubeTimer = new DispatcherTimer();

        public AnalyzerControlWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            conveyor = new ConveyorHelper(CanvasTubes, 0.01, 1.7, 55);
            ConvHelp.DataContext = conveyor;

            for (int i = 0; i < 10; i++)
            {
                StackPanelCartriges.Children.Add(new CartridgeCassetteControl(10, 10, $"Картридж {i + 1}"));
            }
            ((CartridgeCassetteControl)StackPanelCartriges.Children[2]).CountLeft = 8;
            ((CartridgeCassetteControl)StackPanelCartriges.Children[4]).CountLeft = 6;
            ((CartridgeCassetteControl)StackPanelCartriges.Children[5]).CountLeft = 2;

            tubeTimer.Tick += conveyor.TubeLoopStep;
            tubeTimer.Interval = TimeSpan.FromMilliseconds(30);
            tubeTimer.Start();
        }
    }
}
