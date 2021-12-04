﻿using AnalyzerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerControl
{
    public class AnalyzerController
    {
        public AnalyzerController()
        {

        }

        private void ProcessAnalisisStage()
        {
            // Нужно понять, какая операция из 4-х выполняется.

            bool stageIsSapling = true;
            if(stageIsSapling) {
                // Для sampling (забор материала из пробирки):
                makeSampling();
            } else {
                // Для остальных стадий (не sampling):
                makeMaterialExtraction();
            }

            // Для всех стадий (слив):
            makeNaterialInjection();

            // Для всех стадий (инкубация).
            bool needIncubation = false;
            if(needIncubation) {
                // Добавить задачу в список на ожидание инкубации.
                // Если время инкубации истекло, переходим к следующему шагу.
            }

            // Для всех стадий (промывка результата).
            bool needWashing = false;
            if(needWashing) // Промывка результата в Wash-буфере
            {
                makeMaterialWashing();
            }

            // Измерение результата
            bool analysisPrecessed = true;
            if (analysisPrecessed)
            {
                // Для стадии сбора результата (substrate) требуется выполнить измерение прозрачности итогового материала.
                makeResultMeasurment();

                // Для последней стадии, после сбора результатов (выгрузка):
                makeCartridgeEjection();
            }
        }

        void makeSampling()
        {
            // sampling.1(a) Требуется проверить, загружен ли необходимый для выполнения анализа картридж
            // (!!! возможно, это стоит сделать еще при назначении анализа на выполнение !!!)

            // sampling.1(б) требуется проверить, если ли незанятые картриджами ячейки в роторе
            // (!!! возможно, это стоит сделать еще при назначении анализа на выполнение !!!)

            // sampling.2 Требуется выполнить загрузку необходимого картриджа из кассетницы в свободную ячейку ротора
            loadCartridgeFromDeckToRotor();
            // (!!! возможно, следует при подъезде к кассетнице, просканировать ее и убедиться, что она вставлена и что ее триш-код совпадает с требуемым !!!)

            // sampling.3 Требуется выполнить подъезд иглы к позиции над пробиркой и начать опускание до касания материала.

            // sampling.4 Требуется выполнить забор материала из пробирки (заданное количество)
            int volume = 0;
            takeSampleFromTube(volume);
        }

        void makeMaterialExtraction()
        {
            // забор.1 Требуется поместить ячейку ротора (согласно стадии) с загруженным картриджем в место, где будет произведен забор материала иглой.

            // забор.2 Требуется переместить иглу в позицию над ячейкой ротора (согласно стадии) и опустить до необходимого уровня.

            // забор.3 Требуется забрать из ячейки (согласно стадии) заданное количество материала (согласно стадии).
            int volume = 0;
            takeMaterialFromCartridge(volume);
        }

        void makeNaterialInjection()
        {
            // слив.1 Требуется поместить ячейку ротора (согласно стадии) с загруженным картриджем в место, где будет произведен слив материала с иглы.

            // слив.2 Требуется переместить иглу в позицию над ячейкой ротора (согласно стадии) и опустить до необходимого уровня.

            // слив.3 Требуется слить в ячейку (согласно стадии) заданное количество материала (согласно стадии).
            int volume = 0;
            moveMaterialToCartridge(volume);

            // слив.4 Требуется переместить иглу в позицию для промывки и выполнить промывку.
            AnalyzerOperations.WashNeedle();
        }

        void makeMaterialWashing()
        {
            // промывка.1 Требуется поместить ячейку ротора в позицию для промывки (wash-buffer).

            // промывка.2 Требуется выполнить промывку ячейки (согласно стадии) заданное количество раз (согласно стадии).
        }

        void makeResultMeasurment()
        {
            // результат.1 Требуется поместить ячейку ротора в позицию для измерения прозрачности.
            // результат.2 Требуется произвести измерение прозрачности материала и записать полученное значение в БД.
        }

        void makeCartridgeEjection()
        {
            // выгрузка.1 Требуется поместить ячейку ротора в позицию для выгрузки картриджа.
            // выгрузка.2 Требуется произвести выгрузку картриджа из ячейки ротора.
            // выгрузка.3 Требуется пометить ячейку ротора как свободную для загрузки следующего картриджа.
        }

        /// <summary>
        /// Загрузка картриджа из кассетницы в свободную ячейку ротора
        /// </summary>
        void loadCartridgeFromDeckToRotor()
        {

        }

        /// <summary>
        /// Забор материала из пробирки (заданное количество)
        /// </summary>
        void takeSampleFromTube(int volume)
        {

        }

        /// <summary>
        /// Забрать из ячейки (согласно стадии) заданного количества материала (согласно стадии).
        /// </summary>
        /// <param name="volume"></param>
        void takeMaterialFromCartridge(int volume)
        {

        }

        /// <summary>
        /// Слив в ячейку (согласно стадии) заданного количества материала (согласно стадии).
        /// </summary>
        /// <param name="volume"></param>
        void moveMaterialToCartridge(int volume)
        {

        }
    }
}
