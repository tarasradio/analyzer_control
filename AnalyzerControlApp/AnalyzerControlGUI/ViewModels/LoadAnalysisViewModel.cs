using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.ServerCommunication;
using AnalyzerControl.Services;
using AnalyzerControlGUI.Commands;
using AnalyzerDomain;
using AnalyzerDomain.Models;
using AnalyzerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControlGUI.ViewModels
{
    class LoadAnalysisViewModel : ViewModel
    {
        enum LoadAnalysisState
        {
            Started,
            NextTube,
            TubeInserted,
            TubeScanned,
            WaitCartridgeInserted, 
            BadTubeScanned,
            ServerConnectionError,
            WaitCassettePulledOut,
            Finished,
        }

        public ConveyorService Conveyor = null;
        public RotorService Rotor = null;
        public CartridgesDeckService CartridgesDeck = null;

        private readonly int[] cassettesSensors = { 14, 11, 9, 10, 8, 12, 13, 7, 5, 6 };
        private const int sensorTreshold = 512;

        private LoadAnalysisState state = LoadAnalysisState.Started;

        private string analysisBarcode = String.Empty;

        #region DialogResult
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
        #endregion

        #region DialogText
        private string _dialogText;

        public string DialogText
        {
            get => _dialogText;
            set
            {
                _dialogText = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region FirstButtonText
        private string _firstButtonText;

        public string FirstButtonText
        {
            get => _firstButtonText;
            set
            {
                _firstButtonText = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region SecondButtonText
        private string _secondButtonText;

        public string SecondButtonText
        {
            get => _secondButtonText;
            set
            {
                _secondButtonText = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region FirstButtonEnabled
        private bool _firstButtonEnabled;

        public bool FirstButtonEnabled
        {
            get => _firstButtonEnabled;
            set
            {
                _firstButtonEnabled = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region SecondButtonEnabled
        private bool _secondButtonEnabled;

        public bool SecondButtonEnabled
        {
            get => _secondButtonEnabled;
            set
            {
                _secondButtonEnabled = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public LoadAnalysisViewModel()
        {
            DialogText = "Запущена подготовка к загрузке анализа. Ожидайте завершения.";
            FirstButtonText = "Подготовка";
            FirstButtonEnabled = true;
        }

        public void Init()
        {

        }

        #region FirstCommand
        RelayCommand _firstCommand;

        public RelayCommand FirstCommand
        {
            get
            {
                if (_firstCommand == null)
                {
                    _firstCommand = new RelayCommand(
                       param => firstCommand(),
                       param => canFirstCommandExecute()
                       );
                }
                return _firstCommand;
            }
        }

        private bool canFirstCommandExecute()
        {
            return state != LoadAnalysisState.Finished;
        }

        private async void firstCommand()
        {
            switch (state)
            {
                case LoadAnalysisState.Started:
                    DialogText = "Запущена подготовка к загрузке анализа. Ожидайте завершения.";
                    FirstButtonText = "";
                    FirstButtonEnabled = false;
                    prepareBeforeScanning();
                    break;
                case LoadAnalysisState.NextTube:
                    DialogResult = true;
                    break;
                case LoadAnalysisState.TubeInserted:
                    DialogText = "Сканирование пробирки";
                    FirstButtonEnabled = false;
                    scanTube();
                    break;
                case LoadAnalysisState.TubeScanned:
                    state = LoadAnalysisState.WaitCartridgeInserted;
                    break;
                case LoadAnalysisState.WaitCartridgeInserted:

                    state = LoadAnalysisState.BadTubeScanned;
                    break;
                case LoadAnalysisState.BadTubeScanned:

                    state = LoadAnalysisState.TubeInserted;
                    break;
                case LoadAnalysisState.ServerConnectionError:

                    // открыть окно ручного внесения анализа
                    break;
                case LoadAnalysisState.WaitCassettePulledOut:

                    state = LoadAnalysisState.Finished;
                    break;
                case LoadAnalysisState.Finished:

                    break;
            }
        }

        private void addAnalysis(string barcode, string cartridgeBarcode)
        {
            using (AnalyzerContext db = new AnalyzerContext())
            {
                Analysis analysis = new Analysis();

                analysis.Date = DateTime.Now;
                analysis.CurrentStage = 0;
                analysis.Barcode = barcode;

                db.SheduledAnalyzes.Add(analysis);
                db.SaveChanges();
            }
        }
        #endregion

        #region SecondCommand
        RelayCommand _secondCommand;

        public RelayCommand SecondCommand
        {
            get
            {
                if (_secondCommand == null)
                {
                    _secondCommand = new RelayCommand(
                       param => secondCommand(),
                       param => canSecondCommandExecute()
                       );
                }
                return _secondCommand;
            }
        }

        private bool canSecondCommandExecute()
        {
            return state != LoadAnalysisState.Finished;
        }

        private async void secondCommand()
        {
            switch (state)
            {
                case LoadAnalysisState.Started:

                    break;
                case LoadAnalysisState.NextTube:
                    DialogText = "Сканирование пробирки";
                    FirstButtonEnabled = false;
                    SecondButtonEnabled = false;
                    //nextTubePrepare();
                    scanTube();
                    break;
                case LoadAnalysisState.TubeInserted:

                    break;
                case LoadAnalysisState.TubeScanned:

                    break;
                case LoadAnalysisState.WaitCartridgeInserted:

                    break;
                case LoadAnalysisState.BadTubeScanned:

                    break;

                case LoadAnalysisState.ServerConnectionError:
                    DialogText = "Сканирование пробирки";
                    FirstButtonEnabled = false;
                    SecondButtonEnabled = false;
                    //nextTubePrepare();
                    scanTube();
                    break;
                case LoadAnalysisState.WaitCassettePulledOut:

                    break;
                case LoadAnalysisState.Finished:

                    break;
            }
        }
        #endregion

        private void prepareBeforeScanning()
        {
            Task.Run(() =>
            {
                //Analyzer.Conveyor.PrepareBeforeScanning();
                //Analyzer.Conveyor.Shift(false, shiftType: AnalyzerService.Units.ConveyorUnit.ShiftType.HalfTube);
            });
            
            state = LoadAnalysisState.TubeInserted;
            DialogText = "Вставьте пробирку и нажмите - Сканировать пробирку.";
            FirstButtonText = "Сканировать пробирку";
            FirstButtonEnabled = true;
        }

        private void nextTubePrepare()
        {
            Task.Run(() =>
            {
                Analyzer.Conveyor.Shift(false, shiftType: AnalyzerService.Units.ConveyorUnit.ShiftType.OneTube);
            });
        }

        private async void scanTube()
        {
            string barcode = String.Empty;

            await Task.Run(() =>
            {
                Analyzer.Conveyor.RotateAndScanTube();
                System.Threading.Thread.Sleep(2000); // Типа ожидаем, когда бар-код будет прочитан
                barcode = Analyzer.State.CartridgeBarcode;
            });
            
            if (String.IsNullOrEmpty(barcode))
            {
                DialogText = "Пробирка не обнаружена. Нажмите еще раз для сканирования.";
                FirstButtonEnabled = true;
            } else {
                checkBarcode(barcode);
            }
        }

        private async void checkBarcode(string barcode)
        {
            DialogText = "Подключение к серверу...";
            System.Threading.Thread.Sleep(500);

            DatabaseClient client = new DatabaseClient();
            if (client.Connect())
            {
                DialogText = "Подключение к серверу выполнено, запрос анализа из БД.";
                System.Threading.Thread.Sleep(500);
                String cartridgeBarcode = client.GetCartridgeBarcode(barcode.Trim());

                if (cartridgeBarcode != null)
                {
                    DialogText = "Данные анализа загружены с сервера.";
                    System.Threading.Thread.Sleep(500);

                    state = LoadAnalysisState.TubeScanned;

                    DialogText = "Сканирование завершено. Для загрузки картриджа поместите его в ячеку 0.";
                    System.Threading.Thread.Sleep(500);

                    FirstButtonEnabled = false;

                    Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.Blue()).GetBytes());
                    waitCassetteInserted();

                    DialogText = "Картридж вставлен, запуск сканирования картриджа";
                    System.Threading.Thread.Sleep(500);

                    bool isScanned = scanCassette();
                    if (isScanned)
                    {
                        DialogText = "Картридж отсканирован. Запуск загрузки картриджа";
                        System.Threading.Thread.Sleep(500);

                        FirstButtonEnabled = false;

                        var (isOk, cellPosition) = Rotor.AddAnalysis(barcode);

                        if(isOk)
                        {
                            await Task.Run(() =>
                            {
                                Analyzer.Rotor.Home();
                                Analyzer.Rotor.PlaceCellAtCharge((int)cellPosition, 0);

                                Analyzer.Charger.HomeHook(false);
                                Analyzer.Charger.ChargeCartridge();
                                Analyzer.Charger.HomeHook(true);
                                Analyzer.Charger.MoveHookAfterHome();
                            });
                        } else {
                            // Ячейки ротора заняты
                        }
                    }
                    client.Disconnect();

                    DialogText = "Картридж загружен, вытащите кассету из ячейки 0.";
                    System.Threading.Thread.Sleep(500);

                    waitCassettePulledOut();

                    Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.NoColor()).GetBytes());

                    Conveyor.Cells[0].AnalysisBarcode = barcode;
                    NotifyPropertyChanged("ConveyorCells");
                    addAnalysis(barcode, null);

                    DialogText = "Загрузка анализа завершена. Выберите, что делать далее.";
                    System.Threading.Thread.Sleep(500);

                    FirstButtonEnabled = true;
                    SecondButtonEnabled = true;

                    FirstButtonText = "Завершить загрузку.";
                    SecondButtonText = "Следующая пробирка";

                    state = LoadAnalysisState.NextTube;
                }
                else
                {
                    state = LoadAnalysisState.BadTubeScanned;
                    DialogText = "Анализ не найден. Вставьте другую пробирку и нажмите - Сканировать пробирку.";
                    FirstButtonText = "Сканировать пробирку";
                    FirstButtonEnabled = true;
                }
            }
            else
            {
                DialogText = "Ошибка подключения к серверу! Проверьте подключение или введите данные вручную.";
                FirstButtonText = "Ввод данных вручную";
                SecondButtonText = "Попробовать снова";
                FirstButtonEnabled = true;
                SecondButtonEnabled = true;

                state = LoadAnalysisState.ServerConnectionError;
            }
        }

        private bool scanCassette()
        {
            CartridgesDeck.ScanCassette(0);

            string barcode = Analyzer.State.CartridgeBarcode;

            if (barcode != null)
            {
                if (string.IsNullOrEmpty(barcode) || string.IsNullOrWhiteSpace(barcode) || barcode.Contains("\u0002"))
                {
                    CartridgesDeck.Cassettes[0].Barcode = "No barcode";
                    CartridgesDeck.Cassettes[0].CountLeft = 0;
                    Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.Red()).GetBytes());
                }
                else
                {
                    CartridgesDeck.Cassettes[0].Barcode = barcode;
                    CartridgesDeck.Cassettes[0].CountLeft = 10;
                    Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.Green()).GetBytes());
                    return true;
                }
            }
            else
            {
                Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.NoColor()).GetBytes());
            }
            return false;
        }

        private void waitCassetteInserted()
        {
            int sensorNumber = cassettesSensors[0];
            bool cartridgeInserted = false;

            while(!cartridgeInserted)
            {
                cartridgeInserted = Analyzer.State.SensorsValues[sensorNumber] > sensorTreshold;
            }
        }

        private void waitCassettePulledOut()
        {
            int sensorNumber = cassettesSensors[0];
            bool cartridgeInserted = true;

            while (cartridgeInserted)
            {
                cartridgeInserted = Analyzer.State.SensorsValues[sensorNumber] > sensorTreshold;
            }
        }

    }
}
