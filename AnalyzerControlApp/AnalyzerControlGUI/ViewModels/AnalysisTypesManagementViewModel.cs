using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzerControlGUI.Commands;
using AnalyzerControlGUI.Views;
using AnalyzerDomain;
using AnalyzerDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyzerControlGUI.ViewModels
{
    public class AnalysisTypesManagementViewModel : ViewModel
    {
        EditAnalysisTypeViewModel editAnalysisTypeViewModel;

        private ObservableCollection<AnalysisType> _analysisTypes;

        public ObservableCollection<AnalysisType> AnalysisTypes
        {
            get
            {
                return LoadAnalysisTypesDetails();
            }
            private set
            {
                _analysisTypes = value;
                NotifyPropertyChanged("AnalysisTypes");
            }
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        private RelayCommand addAnalysisTypeCommand;

        public RelayCommand AddAnalysisTypeCommand
        {
            get
            {
                return addAnalysisTypeCommand ??
                  (addAnalysisTypeCommand = new RelayCommand(obj =>
                  {
                      editAnalysisTypeViewModel = new EditAnalysisTypeViewModel();
                      EditAnalysisTypeWindow editAnalysisTypeWindow = new EditAnalysisTypeWindow();

                      editAnalysisTypeWindow.DataContext = editAnalysisTypeViewModel;

                      if (editAnalysisTypeWindow.ShowDialog() == true)
                      {
                          using (AnalyzerContext db = new AnalyzerContext())
                          {
                              AnalysisType analysisType = new AnalysisType()
                              {
                                  Description = editAnalysisTypeViewModel.Description,
                                  Cartridge = db.Cartridges.FirstOrDefault(c => c.Barcode == editAnalysisTypeViewModel.CartridgeBarcode)
                              };

                              db.AnalysisTypes.Add(analysisType);
                              db.SaveChanges();
                          }

                          NotifyPropertyChanged("AnalysisTypes");
                      }
                  }));
            }
        }

        private RelayCommand editAnalysisTypeCommand;

        public RelayCommand EditAnalysisTypeCommand
        {
            get
            {
                return editAnalysisTypeCommand ??
                  (editAnalysisTypeCommand = new RelayCommand(obj =>
                  {
                      editAnalysisTypeViewModel = new EditAnalysisTypeViewModel();
                      EditAnalysisTypeWindow editAnalysisTypeWindow = new EditAnalysisTypeWindow();

                      editAnalysisTypeWindow.DataContext = editAnalysisTypeViewModel;

                      editAnalysisTypeViewModel.Description = AnalysisTypes[SelectedIndex].Description;
                      editAnalysisTypeViewModel.CartridgeBarcode = AnalysisTypes[SelectedIndex].Cartridge.Barcode;

                      if (editAnalysisTypeWindow.ShowDialog() == true)
                      {
                          using (AnalyzerContext db = new AnalyzerContext())
                          {
                              AnalysisType analysisType = new AnalysisType()
                              {
                                  Id = AnalysisTypes[SelectedIndex].Id,
                                  Description = editAnalysisTypeViewModel.Description,
                                  Cartridge = db.Cartridges.FirstOrDefault(c => c.Barcode == editAnalysisTypeViewModel.CartridgeBarcode)
                              };
                              db.AnalysisTypes.Update(analysisType);
                              db.SaveChanges();
                          }
                          NotifyPropertyChanged("AnalysisTypes");
                      }
                  },
                  canExecute =>
                  {
                      return SelectedIndex >= 0;
                  }));
            }
        }

        private RelayCommand removeAnalysisTypeCommand;

        public RelayCommand RemoveAnalysisTypeCommand
        {
            get
            {
                return removeAnalysisTypeCommand ??
                  (removeAnalysisTypeCommand = new RelayCommand(obj =>
                  {
                      using (AnalyzerContext db = new AnalyzerContext())
                      {
                          db.AnalysisTypes.Remove(new AnalysisType() { Id = AnalysisTypes[SelectedIndex].Id });
                          db.SaveChanges();
                      }
                      NotifyPropertyChanged("AnalysisTypes");
                  },
                  canExecute =>
                  {
                      return SelectedIndex >= 0;
                  }));
            }
        }

        private ObservableCollection<AnalysisType> LoadAnalysisTypesDetails()
        {
            using (AnalyzerContext db = new AnalyzerContext())
            {
                db.AnalysisTypes.Include(a => a.Cartridge).Load();
                return db.AnalysisTypes.Local.ToObservableCollection();
            }
        }
    }
}
