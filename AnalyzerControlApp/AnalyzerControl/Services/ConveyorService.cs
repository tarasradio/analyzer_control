using AnalyzerService;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl.Services
{
    public class ConveyorCell
    {
        public string AnalysisBarcode { get; set; }
        public bool IsEmpty {
            get {
                return AnalysisBarcode == string.Empty;
            }
            private set { }
        }

        public ConveyorCell() {
            AnalysisBarcode = string.Empty;
        }

        public void SetEmpty() {
            AnalysisBarcode = string.Empty;
        }
    }

    /// <summary>
    /// Сервис управления конвейером
    /// </summary>
    public class ConveyorService
    {
        public ConveyorCell[] Cells { get; private set; }

        private const int cellsBetweenScanAndSampling = 7;
        private const int cellsBetweenScanAndLoading = 25; // TODO:проверить, сколько реально

        /// <summary>
        /// Ячейка в позиции перед сканером
        /// </summary>
        public int CellInScanPosition { get; set; } = 0;

        /// <summary>
        /// Ячейка в позиции забора материала
        /// </summary>
        public int CellInSamplingPosition { 
            get {
                int cell = CellInScanPosition - cellsBetweenScanAndSampling;
                if (cell < 0) cell += Cells.Length;

                return cell;
            }
        }

        /// <summary>
        /// Ячейка в позиции загрузки/выгрузки
        /// </summary>
        public int CellInLoadingPosition
        {
            get {
                int cell = CellInScanPosition - cellsBetweenScanAndLoading;
                if (cell < 0) cell += Cells.Length;

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
            Cells = Enumerable.Repeat(new ConveyorCell(), cellsCount).ToArray();
            State = States.Waiting;
        }

        public void SetController(AnalyzerDemoController controller)
        {
            _controller = controller;
        }

        /// <summary>
        /// Проверка, что проден полный круг, обнуляет индекс ячейки перед сканером
        /// </summary>
        public void CheckFullCycle()
        {
            if (CellInScanPosition == Cells.Length) {
                CellInScanPosition = 0;
            }
        }

        public bool ExistEmptyCells()
        {
            return Cells.Count(c => c.IsEmpty) > 0;
        }

        private (bool, int?) findFreeCellIndex()
        {
            for (int i = 0; i < Cells.Length; i++) {
                if (Cells[i].IsEmpty) {
                    return (true, i);
                }
            }
            return (false, null);
        }

        private (bool, int?) findCompletedIndex()
        {
            for (int i = 0; i < Cells.Length; i++) {
                if (!Cells[i].IsEmpty) {
                    //TODO: добавить проверку на завершенность
                    return (true, i);
                }
            }
            return (false, null);
        }

        public void RemoveAnalysis(int cellIndex)
        {
            Cells[cellIndex].SetEmpty();
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

                int cellsOffset = calcCellsOffset(index);
                Analyzer.Conveyor.Move(cellsOffset); // Отправляем конвейеру запрос на загрузку

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

        private int calcCellsOffset(int? index)
        {
            int cellsBetweenScanAndIndex = (CellInScanPosition - (int)index);
            int cells = cellsBetweenScanAndLoading - cellsBetweenScanAndIndex;
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

                int cellsOffset = calcCellsOffset(index);
                Analyzer.Conveyor.Move(cellsOffset); // Отправляем конвейеру запрос на загрузку

                Logger.Info("Ожидайте, пробирка уже выехала...");
                await Task.Run(cellArrived);

                RemoveAnalysis((int)index); // Помечаем ячейку как свободную

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

        }

        private void cellArrived()
        {
            // Ожидаем, пока ячейка не подъехала

        }
    }
}
