using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using MaterialDesignThemes.Wpf;

using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;

using WpfSteppersControlGUI.Models;
using WpfSteppersControlGUI.ViewModels;

using SteppersControlCore;

namespace WpfSteppersControlGUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MaterialWindow
    {
        Core core;
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

            core = new Core("config.xml");
            steppersModel = new SteppersModel(Core.Settings.Steppers);
            Core.PackHandler.SensorsValuesReceived += PackHandler_SensorsValuesReceived; ;

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

        private void PackHandler_SensorsValuesReceived(ushort[] states)
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
