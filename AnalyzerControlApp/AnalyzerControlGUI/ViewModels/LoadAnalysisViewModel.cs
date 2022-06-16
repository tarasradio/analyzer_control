using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.ServerCommunication;
using AnalyzerControl.Services;
using AnalyzerDomain;
using AnalyzerDomain.Models;
using AnalyzerDomain.Services;
using AnalyzerService;
using MVVM.Commands;
using MVVM.ViewModels;
using System;
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

        private int conveyorCellIndex = 0;

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

        private void firstCommand()
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

        private void secondCommand()
        {
            switch (state)
            {
                case LoadAnalysisState.Started:

                    break;
                case LoadAnalysisState.NextTube:
                    DialogText = "Сканирование пробирки";
                    FirstButtonEnabled = false;
                    SecondButtonEnabled = false;
                    prepareBeforeScanning();
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
                //  Проверяем или есть свободная ячейка
                var (exist, index) = Conveyor.findFreeCellIndex();

                if(exist)
                {
                    Conveyor.PlaceCellInScanPosition((int)index);
                    conveyorCellIndex = (int)index;
                }
                
                state = LoadAnalysisState.TubeInserted;
                DialogText = "Вставьте пробирку и нажмите - Сканировать пробирку.";
                FirstButtonText = "Сканировать пробирку";
                FirstButtonEnabled = true;
            }).Wait();
        }

        private async void scanTube()
        {
            string barcode = String.Empty;

            await Task.Run(() =>
            {
                Analyzer.Conveyor.RotateAndScanTube();
                Task.Delay(2000).Wait();// Типа ожидаем, когда бар-код будет прочитан
                barcode = Analyzer.State.TubeBarcode;
            });
            
            if (String.IsNullOrEmpty(barcode))
            {
                DialogText = "Пробирка не обнаружена. Нажмите еще раз для сканирования.";
                FirstButtonEnabled = true;
            } else {
                await Task.Run(() =>
                {
                    checkBarcode(barcode);
                });
            }
        }

        private void checkBarcode(string tubeBarcode)
        {
            DialogText = "Подключение к серверу...";
            Task.Delay(2000).Wait();

            DatabaseClient client = new DatabaseClient();
            if (client.Connect())
            {
                DialogText = "Подключение к серверу выполнено, запрос анализа из БД.";
                Task.Delay(500).Wait();
                String cartridgeID = client.GetCartridgeID(tubeBarcode.Trim());

                if (cartridgeID != null)
                {
                    DialogText = "Данные анализа загружены с сервера.";
                    Task.Delay(500).Wait();

                    state = LoadAnalysisState.TubeScanned;

                    DialogText = "Сканирование завершено. Для загрузки картриджа поместите его в ячеку 0.";
                    Task.Delay(500).Wait();

                    FirstButtonEnabled = false;

                    Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.Blue()).GetBytes());
                    waitCassetteInserted();

                    DialogText = "Картридж вставлен, запуск сканирования картриджа";
                    Task.Delay(500).Wait();

                    AssayParameters parameters = scanCassette();
                    if (parameters != null)
                    {
                        DialogText = "Картридж отсканирован. Запуск загрузки картриджа";
                        System.Threading.Thread.Sleep(500);

                        FirstButtonEnabled = false;

                        var (isOk, rotorCellPosition) = Rotor.AddAnalysis(tubeBarcode);

                        if(isOk)
                        {
                            Task.Run(() =>
                            {
                                Analyzer.Rotor.Home();
                                Analyzer.Rotor.PlaceCellAtCharge((int)rotorCellPosition, 0);

                                Analyzer.Charger.HomeHook(false);
                                Analyzer.Charger.ChargeCartridge();
                                Analyzer.Charger.HomeHook(true);
                                Analyzer.Charger.MoveHookAfterHome();
                            }).Wait();

                            // Картридж заряжен

                            DialogText = "Картридж загружен, вытащите кассету из ячейки 0.";
                            Task.Delay(500).Wait();

                            waitCassettePulledOut();

                            Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.NoColor()).GetBytes());

                            Conveyor.Cells[Conveyor.CellInScanPosition].AnalysisBarcode = tubeBarcode;
                            NotifyPropertyChanged("ConveyorCells");

                            addAnalysis(tubeBarcode, parameters, (int)rotorCellPosition, Conveyor.CellInScanPosition);

                            DialogText = "Загрузка анализа завершена. Выберите, что делать далее.";
                        } else {
                            // Ячейки ротора заняты
                            DialogText = "Все ячейки ротора заняты!";
                        }
                    }
                    client.Disconnect();

                    Task.Delay(500).Wait();

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

        private void addAnalysis(string patientID, AssayParameters parameters, int rotorPosition, int conveyorPosition)
        {
            using (AnalyzerContext db = new AnalyzerContext())
            {
                AnalysisDescription analysis = new AnalysisDescription();

                analysis.Date = DateTime.Now;
                analysis.PatientId = patientID;

                analysis.RotorPosition = rotorPosition;
                analysis.ConveyorPosition = conveyorPosition;

                analysis.SampleVolume = parameters.usedVolumes.Sample_NDIL;
                analysis.Tw2Volume = parameters.usedVolumes.TW2_Conjugate;
                analysis.Tw3Volume = parameters.usedVolumes.TW3_Substrate;
                analysis.TacwVolume = parameters.usedVolumes.TACW;
                analysis.Inc1Duration = 2;//parameters.incubationTimes.inc_1;
                analysis.inc2Duration = 2;//parameters.incubationTimes.inc_2;

                analysis.CurrentStage = -1;
                analysis.IsCompleted = false;

                db.Analyses.Add(analysis);
                db.SaveChanges();
            }
        }

        private AssayParameters scanCassette()
        {
            AssayParameters parameters = null;

            CartridgesDeck.ScanCassette(0);

            string barcode = Analyzer.State.CartridgeBarcode;

            if (barcode != null)
            {
                parameters = AssayParametersBarcodeParser.Parse(barcode);

                if(parameters != null)
                {
                    CartridgesDeck.Cassettes[0].Parameters = parameters;
                    CartridgesDeck.Cassettes[0].Barcode = parameters.assayShortName;
                    CartridgesDeck.Cassettes[0].CountLeft = 10;

                    Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.Green()).GetBytes());
                } else {
                    CartridgesDeck.Cassettes[0].Parameters = null;
                    CartridgesDeck.Cassettes[0].Barcode = "No cartridge";
                    CartridgesDeck.Cassettes[0].CountLeft = 0;
                    Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.Red()).GetBytes());
                }
            }
            else
            {
                Analyzer.Serial.SendPacket(new SetLedColorCommand(0, LEDColor.NoColor()).GetBytes());
            }
            return parameters;
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
