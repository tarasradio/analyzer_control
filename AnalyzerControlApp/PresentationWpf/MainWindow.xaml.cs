using AnalyzerConfiguration;
using AnalyzerService;
using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;
using MaterialDesignThemes.Wpf;
using PresentationWPF.Models;
using PresentationWPF.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace PresentationWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MaterialWindow
    {
        XmlConfigurationProvider provider;
        Analyzer analyzer;
        SteppersModel steppersModel;

        private List<INavigationItem> m_navigationItems;

        public DialogHost DialogHost
        {
            get
            {
                return m_dialogHost;
            }
        }

        public List<INavigationItem> NavigationItems
        {
            get
            {
                return m_navigationItems;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            provider = new XmlConfigurationProvider();
            analyzer = new Analyzer(provider);
            steppersModel = new SteppersModel(analyzer.Options.Steppers);
            Analyzer.ResponseHandler.SensorsValuesReceived += ResponseHandler_SensorsValuesReceived; ;

            m_navigationItems = new List<INavigationItem>()
            {
                new SubheaderNavigationItem() { Subheader = "Controls" },
                new FirstLevelNavigationItem() { Label = "Connection Control", NavigationItemSelectedCallback = item => 
                    new ConnectionViewModel() },
                new FirstLevelNavigationItem() { Label = "Steppers Control", NavigationItemSelectedCallback = item => 
                    new SteppersControlViewModel(steppersModel) }
            };

            navigationDrawerNav.SelectedItem = m_navigationItems[1];
            sideNav.SelectedItem = m_navigationItems[1];
            m_navigationItems[1].IsSelected = true;



            sideNav.DataContext = this;
            navigationDrawerNav.DataContext = this;
        }

        private void ResponseHandler_SensorsValuesReceived(ushort[] states)
        {
            steppersModel.UpdateSteppersStates(states);
        }

        private void NavigationItemSelectedHandler(object sender, NavigationItemSelectedEventArgs args)
        {
            SelectNavigationItem(args.NavigationItem);
        }

        private void SelectNavigationItem(INavigationItem navigationItem)
        {
            if (navigationItem != null)
            {
                contentControl.Content = navigationItem.NavigationItemSelectedCallback(navigationItem);
            }
            else
            {
                contentControl.Content = null;
            }

            if (appBar != null)
            {
                appBar.IsNavigationDrawerOpen = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
