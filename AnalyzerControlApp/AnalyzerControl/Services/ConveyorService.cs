using AnalyzerDomain.Models;
using AnalyzerService;
using Infrastructure;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyzerControl.Services
{

    /// <summary>
    /// Сервис управления конвейером
    /// </summary>
    public class ConveyorService
    {
        public ObservableCollection<ConveyorCell> Cells { get; private set; }

        /// <summary>
        /// Количество ячеек между позицией сканирования и позицией забора материала (при движении по часовой стрелке)
        /// </summary>
        private const int cellsBetweenScanAndSampling = 27;

        /// <summary>
        /// Ячейка в позиции перед сканером ( инкремент при каждом сдвиге)
        /// </summary>
        public int CellInScanPosition { get; set; } = 0;

        /// <summary>
        /// Ячейка в позиции забора материала
        /// </summary>
        public int CellInSamplePosition { 
            get {
                int cell = CellInScanPosition - cellsBetweenScanAndSampling;
                if (cell < 0) cell += Cells.Count;

                return cell;
            }
        }

        public enum States
        {
            Waiting,
            Loading,
            Unloading,
            AnalyzesProcessing
        }
        public States State { get; private set; }

        /// <summary>
        /// Флаг повторной выгрузки / загрузки
        /// </summary>
        public bool FirstRequest { get; set; } = false;

        private AnalyzerDemoController _controller;

        public ConveyorService(int cellsCount)
        {
            Cells = new ObservableCollection<ConveyorCell>();

            for(int i = 0; i < cellsCount; i++) {
                Cells.Add(new ConveyorCell());
            }

            State = States.Waiting;
        }

        public void SetController(AnalyzerDemoController controller)
        {
            _controller = controller;
        }

        /// <summary>
        /// Проверка, что проден полный круг, обнуляет индекс ячейки перед сканером
        /// </summary>
        public void checkFullCycle()
        {
            if (CellInScanPosition == Cells.Count) {
                CellInScanPosition = 0;
            }
        }

        public bool ExistEmptyCells()
        {
            return Cells.Count(c => c.IsEmpty) > 0;
        }

        public (bool, int?) findFreeCellIndex()
        {
            for (int i = 0; i < Cells.Count; i++) {
                if (Cells[i].IsEmpty) {
                    return (true, i);
                }
            }
            return (false, null);
        }

        private (bool, int?) findCompletedIndex()
        {
            for (int i = 0; i < Cells.Count; i++) {
                if (!Cells[i].IsEmpty) {
                    //TODO: добавить проверку на завершенность
                    return (true, i);
                }
            }
            return (false, null);
        }

        public void FreeCell(int cellIndex)
        {
            Cells[cellIndex].SetEmpty();
        }

        public void PlaceCellInScanPosition(int cell)
        {
            int cellsOffset = calcCellsOffset(cell);
            Analyzer.Conveyor.Shift2(cellsOffset);
            incrementCellsPosition(cellsOffset);
        }

        public void ShiftNextCell()
        {
            Analyzer.Conveyor.Shift2(1);
            incrementCellsPosition(1);
        }

        public async void Load()
        {
            //  Проверяем или есть свободная ячейка до загрузки
            var (exist, index) = findFreeCellIndex();
            if(exist)
            {
                State = States.Loading; // Деактивировать кнопку "Выгрузка" и "Продолжить"
                if (!FirstRequest)
                {
                    Logger.Info("Ожидайте, следующая свободная ячейка выехала...");
                }
                else
                {
                    Logger.Info("Ожидание завершения критических задач...");
                    _controller.InterruptWork(); // Отправляем запрос на прерывание работы алгоритма
                    await Task.Run(waitControllerFinished);
                }

                int cellsOffset = calcCellsOffset((int)index);
                Analyzer.Conveyor.Shift2(cellsOffset); // Отправляем конвейеру запрос на загрузку

                incrementCellsPosition(cellsOffset);

                Logger.Info("Ожидайте, ячейка уже выехала...");
                await Task.Run(cellArrived);
                State = States.Waiting; // Aктивировать кнопку "Продолжить"
                Logger.Info(
                    "Загрузите пробирку в слот\n" +
                    "Для загрузки следующей пробирки нажмите кнопку \"Загрузка\"\n" +
                    "Для выхода их режима загрузки нажмите кнопку \"Продолжить\"");
                // Моргаем на интерфейсе слотом и включаем лампочку подсветки слота
                FirstRequest = false;
            }
            else {
                Logger.Info("Нет свободных слотов для загрузки!");
            }
        }

        private void incrementCellsPosition(int cellsOffset)
        {
            CellInScanPosition += cellsOffset;

            if (CellInScanPosition >= Cells.Count)
            {
                CellInScanPosition -= Cells.Count;
            }
        }

        private int calcCellsOffset(int cell)
        {
            int cells = cell - CellInScanPosition;
            if (cells < 0) cells += Cells.Count;
            return cells;
        }

        public async void Unload()
        {
            //  Проверяем или есть свободная ячейка для выгрузки (нераспознанные/завершенные)
            var (exist, index) = findCompletedIndex();

            if (exist) {
                State = States.Unloading; // Деактивировать кнопку "Загрузка" и "Продолжить"
                if (!FirstRequest) {
                    Logger.Info("Ожидайте, следующая пробирка выехала...");
                } else {
                    Logger.Info("Ожидание завершения критических задач...");
                    _controller.InterruptWork(); // Отправляем запрос на прерывание работы алгоритма
                    await Task.Run(waitControllerFinished);
                }

                int cellsOffset = calcCellsOffset((int)index);
                Analyzer.Conveyor.Shift2(cellsOffset); // Отправляем конвейеру запрос на загрузку

                incrementCellsPosition(cellsOffset);

                Logger.Info("Ожидайте, пробирка уже выехала...");
                await Task.Run(cellArrived);

                FreeCell((int)index); // Помечаем ячейку как свободную

                State = States.Waiting; // Aктивировать кнопку "Продолжить"
                Logger.Info(
                    "Выгрузите пробирку из слота\n" +
                    "Для выгрузки следующей пробирки снова нажмите кнопку \"Выгрузка\"\n" +
                    "Для выхода их режима выгрузки нажмите кнопку \"Продолжить\"");
                // Моргаем на интерфейсе слотом и включаем лампочку подсветки слота (красный)
                FirstRequest = false;
            } else {
                Logger.Info("Нет пробирок для выгрузки!");
            }
        }

        public void Resume()
        {
            FirstRequest = true;
            State = States.AnalyzesProcessing;// Активируем кнопки "Загрузка" и "Выгрузка"
            Logger.Info("Возврат к обработке анализов...");
            _controller.ResumeWork(); // Возвращаем управление основному алгоритму
        }

        private void waitControllerFinished()
        {
            // Ожидаем завершения прерывания (по завершению поднимает иглу в безопасное положение)
            while (_controller.state != AnalyzerDemoController.States.Interrupted);
        }

        private void cellArrived()
        {
            // Ожидаем, пока ячейка не подъехала

        }
    }
}
