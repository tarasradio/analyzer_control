using AnalyzerControlGUI.Commands;
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
    public class EditAnalysisTypeViewModel : ViewModel
    {

        public EditAnalysisTypeViewModel()
        {
            _analysisStages = InitAnalysisStagesList();
        }

        private string _cartridgeBarcode;
        public string CartridgeBarcode
        {
            get { return _cartridgeBarcode; }
            set { 
                _cartridgeBarcode = value;
                NotifyPropertyChanged("CartridgeBarcode");
            }
        }

        private List<Cartridge> _cartridges;
        public List<Cartridge> Cartridges
        {
            get { return LoadCartridgesDetails(); }
            set
            {
                _cartridges = value;
                NotifyPropertyChanged("Cartridges");
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { 
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        public int PipettingVolume {
            get { return _analysisStages[_selectedStage].PipettingVolume; }
            set {
                _analysisStages[_selectedStage].PipettingVolume = value;
                NotifyPropertyChanged();
            }
        }

        public bool NeedIncubation
        {
            get { return _analysisStages[_selectedStage].NeedIncubation; }
            set
            {
                _analysisStages[_selectedStage].NeedIncubation = value;
                NotifyPropertyChanged();
            }
        }

        public int IncubationTimeInMinutes {
            get { return _analysisStages[_selectedStage].IncubationTimeInMinutes; }
            set {
                _analysisStages[_selectedStage].IncubationTimeInMinutes = value;
                NotifyPropertyChanged();
            }
        }

        public bool NeedWashStep
        {
            get { return _analysisStages[_selectedStage].NeedWashStep; }
            set
            {
                _analysisStages[_selectedStage].NeedWashStep = value;
                NotifyPropertyChanged();
            }
        }

        public int NumberOfWashStep { 
            get { return _analysisStages[_selectedStage].NumberOfWashStep; }
            set {
                _analysisStages[_selectedStage].NumberOfWashStep = value;
                NotifyPropertyChanged();
            } 
        }

        private int _selectedStage = 0;
        public int SelectedStage
        {
            get { return _selectedStage; }
            set
            {
                _selectedStage = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("PipettingVolume");
                NotifyPropertyChanged("NeedIncubation");
                NotifyPropertyChanged("IncubationTimeInMinutes");
                NotifyPropertyChanged("NeedWashStep");
                NotifyPropertyChanged("NumberOfWashStep");
            }
        }

        private AnalysisStage[] _analysisStages = new AnalysisStage[4];
        public AnalysisStage[] AnalysisStages {
            get { return _analysisStages; }
            set {
                _analysisStages = value;
                NotifyPropertyChanged();
            }
        }

        private List<Cartridge> LoadCartridgesDetails()
        {
            using (AnalyzerContext db = new AnalyzerContext())
            {
                db.Cartridges.Load();
                return db.Cartridges.Local.ToList();
            }
        }

        public AnalysisStage[] InitAnalysisStagesList()
        {
            AnalysisStage[] stages = new AnalysisStage[4];
            for(int i = 0; i < stages.Length; ++i)
                stages[i] = new AnalysisStage();
            return stages;
        }

        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                NotifyPropertyChanged();
            }
        }

        RelayCommand _okCommand;

        public RelayCommand OkCommand
        {
            get
            {
                if (_okCommand == null)
                {
                    _okCommand = new RelayCommand(
                       param => { DialogResult = true; },
                       param => true);
                }
                return _okCommand;
            }
        }

        RelayCommand _cancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(
                       param => { DialogResult = false; },
                       param => true);
                }
                return _cancelCommand;
            }
        }
    }
}
