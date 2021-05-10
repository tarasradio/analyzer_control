﻿using System;
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
            ManualControl,
            AnalysisProcessing
        }
        public States State { get; private set; }

        public ConveyorService(int cellsCount)
        {
            Cells = Enumerable.Repeat(new ConveyorCell(), cellsCount).ToArray();
            State = States.ManualControl;
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

        public void RemoveAnalysis(int cellIndex)
        {
            Cells[cellIndex].SetEmpty();
        }

        public void SwitchToManualControl()
        {
            State = States.ManualControl;
        }

        public void SwitchToAnalysisProcessing()
        {
            State = States.AnalysisProcessing;
        }

        public void Load()
        {
            //  Pseudocode:
            //
            //  Проверяем или есть свободная ячейка до загрузки
            //      если есть:
            //          Деактивировать кнопку "Выгрузка" (sic!)
            //          Деактивируем кнопку "Продолжить"
            //          "Проверям на предмет повторной загрузки:"
            //              если да:
            //                  Вывод сообщения: Ожидайте, следующая свободная ячейка выехала..."
            //              если нет:
            //                  Вывод сообщения "Ожидание завершения критических задач..."
            //                  Отправляем запрос на прерывание работы алгоритма
            //                  Ожидаем завершения прерывания (по завершению поднимает иглу в безопасное положение)
            //              Отправляем конвейеру запрос на загрузку
            //              Вывод сообщения "Ожидайте, ячейка уже выехала..."
            //              Пока ячейка не подъехала {}
            //          Aктивируем кнопку "Продолжить"
            //          Вывод сообщения
            //          "
            //          Загрузите пробирку в слот"
            //              Для загрузки следующей пробирки нажмите кнопку "Загрузка"
            //              Для выхода их режима загрузки нажмите кнопку "Продолжить"
            //          "
            //          Моргаем на интерфейсе слотом и включаем лампочку подсветки слота  
            //          Устанавливаем состояние как повторная загрузка (кнопка "Продолжить сбрасывает его")
            //      если нет:
            //          Вывод сообщения "Нет свободных слотов для загрузки!"
            //          Вывод сообщения "И вообще идите нахуй с такими желаниями!"
            //         
        }

        public void Unload()
        {
            //  Pseudocode:
            //
            //  Проверяем или есть ячейки до выгрузки (нераспознанные/завершенные)
            //      если есть:
            //          Деактивировать кнопку "Загрузка" (sic!)
            //          Деактивируем кнопОчку "Продолжить"
            //          "Проверям на предмет повторной выгрузки:"
            //              если да:
            //                  Вывод сообщения: Ожидайте, следующая пробирка выехала..."
            //              если нет:
            //                  Вывод сообщения "Ожидание завершения критических задач..."
            //                  Отправляем запрос на прерывание работы алгоритма
            //                  Ожидаем завершения прерывания (по завершению поднимает иглу в безопасное положение)
            //              Отправляем конвейеру запрос на выгрузку 
            //              Вывод сообщения "Ожидайте, пробирка выехала..."
            //              Пока ячейка не подъехала {}
            //          Aктивируем кнопку "Продолжить"
            //          Вывод сообщения
            //          "
            //          Выгрузите пробирку 
            //              Для выгрузки следующей пробирки нажмите кнопку "Выгрузка"
            //              Для выхода их режима выгрузки нажмите кнопку "Продолжить"
            //          "
            //          Моргаем на интерфейсе слотом и включаем лампочку подсветки слота (красный)  
            //          Устанавливаем состояние как повторная выгрузка (кнопка "Продолжить сбрасывает его")
            //      если нет:
            //          Вывод сообщения "Нет пробирок для выгрузки!"
            //          Вывод сообщения "И вообще идите нахуй с такими желаниями!"
            // 
        }
    }
}
