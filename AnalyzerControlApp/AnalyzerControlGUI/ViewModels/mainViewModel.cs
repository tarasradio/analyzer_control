using AnalyzerControlGUI.Commands;
using AnalyzerControlGUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControlGUI.ViewModels
{
    public class mainViewModel : ViewModel
    {
        private RelayCommand _cartridgesManagementCommand;

        public RelayCommand CartridgesManagementCommand
        {
            get
            {
                return _cartridgesManagementCommand ??
                  (_cartridgesManagementCommand = new RelayCommand(obj =>
                  {
                      CartridgesWindow cartridgesWindow = new CartridgesWindow();
                      cartridgesWindow.ShowDialog();
                  }));
            }
        }

        private RelayCommand _analysisTypesManagementCommand;

        public RelayCommand AnalysisTypesManagementCommand
        {
            get
            {
                return _analysisTypesManagementCommand ??
                  (_analysisTypesManagementCommand = new RelayCommand(obj =>
                  {
                      AnalysisTypesWindow analysisTypesWindow = new AnalysisTypesWindow();
                      analysisTypesWindow.ShowDialog();
                  }));
            }
        }
    }
}
