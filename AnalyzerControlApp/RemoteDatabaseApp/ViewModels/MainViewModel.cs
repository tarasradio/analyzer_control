using AnalyzerDomain;
using AnalyzerDomain.Models;
using Microsoft.EntityFrameworkCore;
using MVVM.Commands;
using MVVM.ViewModels;
using RemoteDatabaseApp.Connection;
using RemoteDatabaseApp.Views.DialogWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace RemoteDatabaseApp.ViewModels
{
    public class MainViewModel : ViewModel
    {
        Server Server;

        public MainViewModel()
        {
            Server = new Server();
            ServerAddress = Server.ServerAddress;

            Server.RequestReceived += (String message) =>
            {
                InformationText = message;
            };
        }

        #region SheduledAnalyzes
        private ObservableCollection<Analysis> _sheduledAnalyzes;

        public ObservableCollection<Analysis> SheduledAnalyzes
        {
            get
            {
                if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                {
                    return new ObservableCollection<Analysis>();
                }
                else
                    return LoadSheduledAnalyzesDetails();
            }
            private set
            {
                _sheduledAnalyzes = value;
                NotifyPropertyChanged("SheduledAnalyzes");
            }
        }

        private ObservableCollection<Analysis> LoadSheduledAnalyzesDetails()
        {
            using (AnalyzerContext db = new AnalyzerContext())
            {
                db.SheduledAnalyzes.Load();
                return db.SheduledAnalyzes.Local.ToObservableCollection();
            }
        }
        #endregion

        #region SheduledAnalysisIndex
        private int _sheduledAnalysisIndex;

        public int SheduledAnalysisIndex
        {
            get { return _sheduledAnalysisIndex; }
            set { _sheduledAnalysisIndex = value; }
        }
        #endregion

        #region InformationText
        private string _informationText;

        public string InformationText
        {
            get { return _informationText; }
            set
            {
                _informationText = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region ServerAddress
        private string _serverAddress;

        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                _serverAddress = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region StartServerCommand
        private RelayCommand _startServerCommand;

        public RelayCommand StartServerCommand
        {
            get
            {
                if (_startServerCommand == null)
                {
                    _startServerCommand = new RelayCommand(
                       param => serverStart(),
                       param => true
                       );
                }
                return _startServerCommand;
            }
        }

        private void serverStart()
        {
            Server.StartServer();
            Console.WriteLine("Сервер запущен");
            InformationText = "Сервер запущен.";
        }
        #endregion

        #region StopServerCommand
        private RelayCommand _stopServerCommand;

        public RelayCommand StopServerCommand
        {
            get
            {
                if (_stopServerCommand == null)
                {
                    _stopServerCommand = new RelayCommand(
                       param => serverStop(),
                       param => true
                       );
                }
                return _stopServerCommand;
            }
        }

        private void serverStop()
        {
            Server.StopServer();
        }
        #endregion

        #region PatientInputCommand
        private RelayCommand _patientInputCommand;

        public RelayCommand PatientInputCommand
        {
            get
            {
                if (_patientInputCommand == null)
                {
                    _patientInputCommand = new RelayCommand(
                       param => addPatientShowDialog(),
                       param => true
                       );
                }
                return _patientInputCommand;
            }
        }

        private void addPatientShowDialog()
        {
            PatientInputDialog dialog = new PatientInputDialog();
            dialog.ShowDialog();
            NotifyPropertyChanged("SheduledAnalyzes");
        }
        #endregion

        #region RemoveItemCommand
        private RelayCommand _removeItemCommand;

        public RelayCommand RemoveItemCommand
        {
            get
            {
                if (_removeItemCommand == null)
                {
                    _removeItemCommand = new RelayCommand(
                       param => removeItem(),
                       param => true
                       );
                }
                return _removeItemCommand;
            }
        }

        private void removeItem()
        {
            using (AnalyzerContext db = new AnalyzerContext())
            {
                db.SheduledAnalyzes.Load();
                db.SheduledAnalyzes.Local.Remove(db.SheduledAnalyzes.FirstOrDefault(d => d.Id == SheduledAnalyzes[SheduledAnalysisIndex].Id));
                db.SaveChanges();

                NotifyPropertyChanged("SheduledAnalyzes");
            }
        }
        #endregion
    }
}
