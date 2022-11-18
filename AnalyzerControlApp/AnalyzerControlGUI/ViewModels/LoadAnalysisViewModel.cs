using AnalyzerCommunication;
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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;

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
            WaitCartridgesInserted, 
            BadTubeScanned,
            ServerConnectionError,
            WaitCassettePulledOut,
            Finished,
        }

        public ConveyorService Conveyor = null;
        public RotorService Rotor = null;
        public CartridgesDeckService CartridgesDeck = null;

        public AnalyzesRepository AnalyzesRepository = null;

        private readonly int[] cassettesSensors = { 14, 11, 9, 10, 8, 12, 13, 7, 5, 6 };
        private const int sensorTreshold = 512;

        private LoadAnalysisState state = LoadAnalysisState.Started;

        private string analysisBarcode = String.Empty;

        private int conveyorCellIndex = 0;

        String[] cartridgesIDs;
        AssayParameters[] analyzesParameters;

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
                    state = LoadAnalysisState.WaitCartridgesInserted;
                    break;
                case LoadAnalysisState.WaitCartridgesInserted:
                    await Task.Run(() => { 
                        checkCartridges();
                    });
                    
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
                case LoadAnalysisState.WaitCartridgesInserted:

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
                
                LigthOffCells();
                
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
                    analysisBarcode = barcode;
                    checkBarcode();
                });
            }
        }

        private void checkBarcode()
        {
            DialogText = "Подключение к серверу...";
            Task.Delay(2000).Wait();

            DatabaseClient client = new DatabaseClient(Analyzer.ServerAddress, Analyzer.ServerPort);
            if (client.Connect())
            {
                DialogText = "Подключение к серверу выполнено, запрос анализа из БД.";
                Task.Delay(500).Wait();

                cartridgesIDs = client.GetCatridgesIDs(analysisBarcode.Trim());
                client.Disconnect();

                if (cartridgesIDs.Length > 0)
                {
                    DialogText = "Данные анализа загружены с сервера.";
                    Task.Delay(500).Wait();

                    state = LoadAnalysisState.TubeScanned;

                    FirstButtonEnabled = false;
                    DialogText = "Сканирование завершено. Поместите следующие картриджи в подсвеченные ячейки: \n\r";

                    Task.Delay(500).Wait();

                    checkCartridges();
                } else {
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

        private void checkCartridges()
        {
            int cell = 0;

            foreach (var cartridge in cartridgesIDs) {
                Analyzer.Serial.SendPacket(new SetLedColorCommand(cell, LEDColor.Blue()).GetBytes());
                Task.Delay(100).Wait();

                DialogText += ($"[{cartridge}] ");

                cell++;
            }

            Task.Delay(500).Wait();

            waitAllCassettesInserted(cartridgesIDs.Length);

            DialogText = "Все ячейки заполнены, запуск сканирования картриджей";
            Task.Delay(500).Wait();

            bool[] checkedCells = new bool[cartridgesIDs.Length];

            bool haveWrongCartridges = false;

            analyzesParameters = new AssayParameters[cartridgesIDs.Length];

            for (int i = 0; i < cartridgesIDs.Length; i++)
            {
                AssayParameters parameters = scanCassette(i);
                analyzesParameters[i] = parameters;

                if (parameters != null)
                {
                    bool cartridgeIsFound = false;
                    for (int j = 0; j < cartridgesIDs.Length; j++)
                    {
                        cartridgeIsFound = parameters.assayName.Equals(cartridgesIDs[i]);
                        if (cartridgeIsFound)
                            break;
                    }

                    checkedCells[i] = cartridgeIsFound;

                    if (cartridgeIsFound) {
                        DialogText += $"\n\r В ячейке [{i + 1}] найден картридж [{parameters.assayName}].";
                    } else {
                        DialogText += $"\n\r В ячейке [{i + 1}] найден неверный картридж!";

                        haveWrongCartridges = true;
                    }
                }
                else
                {
                    DialogText += $"\n\r В ячейке [{i + 1}] картридж не был отсканирован!";

                    haveWrongCartridges = true;
                }
            }

            if(haveWrongCartridges) {
                DialogText += $"\n\r Проверьте картриджи и нажмите \"Продолжить\".";

                FirstButtonEnabled = true;
                FirstButtonText = "Продолжить";

                state = LoadAnalysisState.WaitCartridgesInserted;
            } else {
                DialogText = $"Все картриджи найдены, запуск загрузки картриджей.";

                loadCartridges();

                Task.Delay(500).Wait();

                FirstButtonEnabled = true;
                SecondButtonEnabled = true;

                FirstButtonText = "Завершить загрузку.";
                SecondButtonText = "Следующая пробирка";

                state = LoadAnalysisState.NextTube;
            }
        }

        private void loadCartridges()
        {
            // Проверка наличия свободных ячеек в роторе
            bool existEmptyCells = Rotor.ExistEmptyCells(cartridgesIDs.Length);

            if(existEmptyCells) {
                // Найдены свободные ячейки в роторе

                for (int i = 0; i < cartridgesIDs.Length; i++) {
                    loadCartridge(i);
                }

                DialogText = "Картриджи загружены, вытащите кассеты из ячеек.";
                Task.Delay(500).Wait();

                waitAllCassetesPulledOut(cartridgesIDs.Length);

                LigthOffCells();

                Conveyor.Cells[Conveyor.CellInScanPosition].AnalysisBarcode = analysisBarcode;
                Conveyor.Cells[Conveyor.CellInScanPosition].State = CellState.Processing;

                NotifyPropertyChanged("ConveyorCells");

                DialogText = "Загрузка анализов завершена. Выберите, что делать далее.";
            }
            else {
                // Ячейки ротора заняты
                DialogText = "Все ячейки ротора заняты!";
            }
        }

        private void loadCartridge(int i) // Загрузка картриджа из ячейки
        {
            var (isOk, rotorCellPosition) = Rotor.AddAnalysis(analysisBarcode, cartridgesIDs[i]);

            if (isOk)
            {
                Task.Run(() =>
                {
                    Analyzer.Rotor.Home();
                    Analyzer.Rotor.PlaceCellAtCharge((int)rotorCellPosition, i);

                    Analyzer.Charger.HomeHook(false);
                    Analyzer.Charger.MoveHookAfterHome();

                    Analyzer.Charger.HomeRotator();
                    Analyzer.Charger.TurnToCell(i);

                    Analyzer.Charger.ChargeCartridge();
                    Analyzer.Charger.HomeHook(true);
                    Analyzer.Charger.MoveHookAfterHome();
                }).Wait();

                Rotor.Cells[(int)rotorCellPosition].State = CellState.Processing;

                addAnalysis(analysisBarcode, analyzesParameters[i], (int)rotorCellPosition, Conveyor.CellInScanPosition);
            }
        }

        private void waitAllCassettesInserted(int cells)
        {
            bool cartridgesInserted;

            do {
                cartridgesInserted = true;
                for (int i = 0; i < cells; i++) {
                    bool cartridgeInserted = Analyzer.State.SensorsValues[cassettesSensors[i]] > sensorTreshold;

                    if (cartridgeInserted) {
                        Analyzer.Serial.SendPacket(new SetLedColorCommand(i, LEDColor.Red()).GetBytes());
                    } else {
                        Analyzer.Serial.SendPacket(new SetLedColorCommand(i, LEDColor.Blue()).GetBytes());
                    }

                    Task.Delay(100).Wait();
                    cartridgesInserted &= cartridgeInserted;
                }
            } while (!cartridgesInserted);
        }

        private void waitAllCassetesPulledOut(int cells)
        {
            bool cartridgesInserted;

            do
            {
                cartridgesInserted = true;
                for (int i = 0; i < cells; i++)
                {
                    bool cartridgeInserted = Analyzer.State.SensorsValues[cassettesSensors[i]] > sensorTreshold;

                    if (cartridgeInserted)
                    {
                        Analyzer.Serial.SendPacket(new SetLedColorCommand(i, LEDColor.Red()).GetBytes());
                    }
                    else
                    {
                        Analyzer.Serial.SendPacket(new SetLedColorCommand(i, LEDColor.Blue()).GetBytes());
                    }

                    Task.Delay(100).Wait();
                    cartridgesInserted &= cartridgeInserted;
                }
            } while (cartridgesInserted);
        }

        private static void LigthOffCells()
        {
            List<ICommand> commands = new List<ICommand>();

            for (int i = 0; i < 10; i++) {
                commands.Add(new SetLedColorCommand(i, LEDColor.NoColor()));
            }

            Analyzer.CommandExecutor.WaitExecution(commands);
        }

        private void addAnalysis(string patientID, AssayParameters parameters, int rotorPosition, int conveyorPosition)
        {
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
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
                analysis.Inc1Duration = 5;//parameters.incubationTimes.inc_1;
                analysis.inc2Duration = 5;//parameters.incubationTimes.inc_2;

                analysis.CurrentStage = -1;
                analysis.IsCompleted = false;

                AnalyzesRepository.Add(analysis);
                AnalyzesRepository.Save();
            });
        }

        private AssayParameters scanCassette(int cell)
        {
            AssayParameters parameters = null;

            CartridgesDeck.ScanCassette(cell);

            string barcode = Analyzer.State.CartridgeBarcode;

            if (barcode != null)
            {
                parameters = AssayParametersBarcodeParser.Parse(barcode);

                if(parameters != null)
                {
                    CartridgesDeck.Cassettes[cell].Parameters = parameters;
                    CartridgesDeck.Cassettes[cell].Barcode = parameters.assayShortName;
                    CartridgesDeck.Cassettes[cell].CountLeft = 10;

                    Analyzer.Serial.SendPacket(new SetLedColorCommand(cell, LEDColor.Green()).GetBytes());
                } else {
                    CartridgesDeck.Cassettes[cell].Parameters = null;
                    CartridgesDeck.Cassettes[cell].Barcode = "No cartridge";
                    CartridgesDeck.Cassettes[cell].CountLeft = 0;
                    Analyzer.Serial.SendPacket(new SetLedColorCommand(cell, LEDColor.Red()).GetBytes());
                }
            }
            else
            {
                Analyzer.Serial.SendPacket(new SetLedColorCommand(cell, LEDColor.NoColor()).GetBytes());
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
