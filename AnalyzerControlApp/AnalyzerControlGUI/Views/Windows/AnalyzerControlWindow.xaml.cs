using AnalyzerControlGUI.Views.CustomViews;
using AnalyzerControlGUI.ViewsHelpers;
using System;
using System.Windows;
using System.Windows.Threading;

namespace AnalyzerControlGUI
{
    /// <summary>
    /// Логика взаимодействия для AnalyzerControlWindow.xaml
    /// </summary>
    public partial class AnalyzerControlWindow : Window
    {
        private const int cassettesCount = 10;
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

            for (int i = 0; i < cassettesCount; i++)
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
