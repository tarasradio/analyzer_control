using AnalyzerControlGUI.Commands;
using AnalyzerControlGUI.Views;
using AnalyzerDomain;
using AnalyzerDomain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControlGUI.ViewModels
{
    public class CartridgesManagementViewModel : ViewModel
    {
        EditCartridgeViewModel editCartridgeViewModel;

        private ObservableCollection<Cartridge> _cartridges;

        public ObservableCollection<Cartridge> Cartridges
        {
            get
            {
                return LoadCartridgesDetails();
            }
            private set
            {
                _cartridges = value;
            }
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        private RelayCommand addCartridgeCommand;

        public RelayCommand AddCartridgeCommand
        {
            get
            {
                return addCartridgeCommand ??
                  (addCartridgeCommand = new RelayCommand(obj =>
                  {
                      editCartridgeViewModel = new EditCartridgeViewModel();
                      EditCartridgeModelWindow editCartridgeWindow = new EditCartridgeModelWindow();

                      editCartridgeWindow.DataContext = editCartridgeViewModel;

                      if (editCartridgeWindow.ShowDialog() == true)
                      {
                          Cartridge cartridge = new Cartridge()
                          {
                              Description = editCartridgeViewModel.Description,
                              Barcode = editCartridgeViewModel.Barcode,
                          };
                          using (AnalyzerContext db = new AnalyzerContext())
                          {
                              db.Cartridges.Add(cartridge);
                              db.SaveChanges();
                          }

                          NotifyPropertyChanged("Cartridges");
                      }
                  }));
            }
        }

        private RelayCommand editCartridgeCommand;

        public RelayCommand EditCartridgeCommand
        {
            get
            {
                return editCartridgeCommand ??
                  (editCartridgeCommand = new RelayCommand(obj =>
                  {
                      editCartridgeViewModel = new EditCartridgeViewModel();
                      EditCartridgeModelWindow editCartridgeWindow = new EditCartridgeModelWindow();

                      editCartridgeWindow.DataContext = editCartridgeViewModel;

                      editCartridgeViewModel.Description = Cartridges[SelectedIndex].Description;
                      editCartridgeViewModel.Barcode = Cartridges[SelectedIndex].Barcode;

                      if (editCartridgeWindow.ShowDialog() == true)
                      {
                          Cartridge cartridge = new Cartridge()
                          {
                              Id = Cartridges[SelectedIndex].Id,
                              Description = editCartridgeViewModel.Description,
                              Barcode = editCartridgeViewModel.Barcode
                          };
                          using (AnalyzerContext db = new AnalyzerContext())
                          {
                              db.Cartridges.Update(cartridge);
                              db.SaveChanges();
                          }
                          NotifyPropertyChanged("Cartridges");
                      }
                  },
                  canExecute =>
                  {
                      return SelectedIndex >= 0;
                  }));
            }
        }

        private RelayCommand removeCartridgeCommand;

        public RelayCommand RemoveCartridgeCommand
        {
            get
            {
                return removeCartridgeCommand ??
                  (removeCartridgeCommand = new RelayCommand(obj =>
                  {
                      using (AnalyzerContext db = new AnalyzerContext())
                      {
                          db.Cartridges.Remove(new Cartridge() { Id = Cartridges[SelectedIndex].Id });
                          db.SaveChanges();
                      }
                      NotifyPropertyChanged("Cartridges");
                  },
                  canExecute =>
                  {
                      return SelectedIndex >= 0;
                  }));
            }
        }

        public CartridgesManagementViewModel()
        {
            _cartridges = LoadCartridgesDetails();
        }

        private ObservableCollection<Cartridge> LoadCartridgesDetails()
        {
            using (AnalyzerContext db = new AnalyzerContext())
            {
                db.Cartridges.Load();
                ObservableCollection<Cartridge> cartridges = db.Cartridges.Local.ToObservableCollection();
                return cartridges;
            }
        }
    }
}
