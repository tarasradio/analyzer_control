using SteppersControlCore.Controllers;
using SteppersControlCore.Elements;
using SteppersControlCore.Interfaces;
using SteppersControlCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace SteppersControlCore
{
    [Serializable]
    public class DemoControllerProperties
    {
        public List<TubeInfo> Tubes { get; set; } = new List<TubeInfo>();

        public DemoControllerProperties()
        {

        }
    }

    public class DemoController
    {
        public DemoControllerProperties Properties;

        private const string filename = "Tubes";

        // шагов пробирки от точки сканирования до точки с забором 
        private const int tubeCellsCount = 54;
        private const int cellsBetweenScanAndSuction = 7;
        
        private TubeCell[] tubeCells;

        private int currentCellAtScanner = 0;

        private Stopwatch stopWatch = new Stopwatch();
        Timer timer = new Timer();

        private static object locker = new object();

        public DemoController()
        {
            Properties = new DemoControllerProperties();

            tubeCells = new TubeCell[tubeCellsCount];

            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
        }

        public void WriteXml(string path)
        {
            XMLSerializeHelper<DemoControllerProperties>.WriteXml(Properties, 
                Path.Combine(path, filename));
        }

        public void ReadXml(string path)
        {
            Properties = XMLSerializeHelper<DemoControllerProperties>.ReadXML(
                Path.Combine(path, filename));

            if (Properties == null)
                Properties = new DemoControllerProperties();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(stopWatch.ElapsedMilliseconds >= 60000)
            {
                stopWatch.Restart();
                Logger.DemoInfo("Прошла минута");
                
                // уменьшаем оставшееся время инкубации
                foreach (TubeInfo tube in Properties.Tubes)
                {
                    lock (locker)
                    {
                        if(tube.IsFind && tube.TimeToStageComplete != 0)
                            tube.TimeToStageComplete -= 1;
                    }
                }
            }
        }
        
        public void StartDemo()
        {
            timer.Start();
            stopWatch.Reset();
            stopWatch.Start();

            Core.Executor.StartTask(() =>
            {
                DemoTask();
            });
        }

        public void AbortExecution()
        {
            Logger.DemoInfo("Работа демо режима прервана.");

            if(timer.Enabled) timer.Stop();
            if(stopWatch.IsRunning) stopWatch.Stop();
        }

        private void DemoTask()
        {
            foreach (TubeInfo tube in Properties.Tubes)
            {
                lock(locker)
                {
                    tube.Clear();
                }
            }

            initializationTask();
            needleWashingTask();

            Logger.DemoInfo($"Запущена подготовка перед сканированием пробирок");

            Core.Needle.HomeAll();
            Core.Transporter.PrepareBeforeScanning();

            Logger.DemoInfo($"Подготовка завершена");

            currentCellAtScanner = 0;

            while (haveOutstandingTasks()) // пока есть невыполненные задачи
            {
                if (currentCellAtScanner == tubeCellsCount) // прошли полный круг
                {
                    currentCellAtScanner = 0;
                    Logger.DemoInfo($"Круг пройден. Запуск повторного сканирования.");
                }

                if (tubeCells[currentCellAtScanner] == null)
                    tubeCells[currentCellAtScanner] = new TubeCell();

                searchTubeInCell(2);

                // Получение номера ячейки в точке забора материала из пробирки
                int cellUnderNeedle = currentCellAtScanner - cellsBetweenScanAndSuction;
                if (cellUnderNeedle < 0) cellUnderNeedle += tubeCellsCount;
                // Номер получен

                if (tubeCells[cellUnderNeedle] == null)
                    tubeCells[cellUnderNeedle] = new TubeCell();

                TubeInfo tube = tubeCells[cellUnderNeedle].Tube;

                if(tube != null)
                {
                    Logger.DemoInfo($"Пробирка со штрихкодом [{tube.BarCode}] дошла до точки забора материала.");
                    
                    if(tube.CurrentStage == -1)
                    {
                        Logger.DemoInfo($"Забор материала ранее не производился.");

                        tube.CurrentStage = 0;
                        performingPreparatoryTask(tube);
                        tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                    }
                }

                performingOutstandingTasks();
                currentCellAtScanner++;
            }

            Logger.DemoInfo($"Все пробирки обработаны!");
        }

        /// <summary>
        /// Выполнение запланированных задач
        /// </summary>
        private void performingOutstandingTasks()
        {
            foreach (TubeInfo tube in Properties.Tubes)
            {
                if( tube.CurrentStage >= 0 &&
                    tube.CurrentStage < tube.Stages.Count && 
                    tube.TimeToStageComplete == 0)
                {
                    tube.CurrentStage++;
                    
                    if (tube.CurrentStage < tube.Stages.Count)
                    {
                        tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                        Logger.DemoInfo($"Завершена инкубация для пробирки [{tube.BarCode}]!");

                        performingIntermediateTask(tube);
                    }
                    else
                    {
                        Logger.DemoInfo($"Завершены все стадии анализа для пробирки [{tube.BarCode}]!");

                        performingFinishTask(tube);
                    }

                }
            }
        }

        /// <summary>
        /// Поиск пробирки с заданным штрихкодом
        /// </summary>
        /// <param name="barCode">Искомый штрихкод</param>
        /// <returns>Пробирка со штрихкодом или null</returns>
        private TubeInfo searchBarcodeInDatabase(string barCode)
        {
            TubeInfo result = null;

            foreach (TubeInfo tube in Properties.Tubes)
            {
                if (barCode.Contains(tube.BarCode))
                {
                    result = tube;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Проверка наличия невыполненных задач
        /// </summary>
        /// <returns>Наличие невыполненных задач</returns>
        private bool haveOutstandingTasks()
        {
            bool result = false;

            foreach(TubeInfo tube in Properties.Tubes)
            {
                if (tube.CurrentStage <= tube.Stages.Count)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void initializationTask()
        {
            Logger.DemoInfo($"Запущена инициализация всех устройств");

            Core.Needle.HomeAll();
            Core.Charger.HomeHook();
            Core.Charger.HomeRotator();
            Core.Rotor.Home();
            Core.Pomp.Home();

            Logger.DemoInfo($"Инициализация завершена");
        }

        /// <summary>
        /// Промывка иглы
        /// </summary>
        private void needleWashingTask()
        {
            Logger.DemoInfo($"Запущена промывка иглы");

            Core.Needle.HomeLift();
            Core.Needle.TurnAndGoDownToWashing();
            Core.Pomp.WashingNeedle(2);
            Core.Pomp.Home();
            Core.Pomp.CloseValves();

            Logger.DemoInfo($"Промывка иглы завершена.");
        }
        
        /// <summary>
        /// Поиск пробирки в конвейере (поиск штрихкода)
        /// </summary>
        /// <param name="numberAttempts">Количество попыток (при неудаче)</param>
        /// <returns>Успешность выполнения</returns>
        private void searchTubeInCell(int numberAttempts)
        {
            int attempt = 0;
            
            TubeCell tubeCell = tubeCells[currentCellAtScanner];

            Logger.DemoInfo($"Запущен поиск пробирки (сканирование)");

            Core.Transporter.Shift(false);

            while (attempt < numberAttempts)
            {
                Logger.DemoInfo($"Попытка {attempt}.");

                Core.Transporter.RotateAndScanTube();
                string barCode = Core.GetLastTubeBarCode();

                if ( !String.IsNullOrWhiteSpace(barCode) )
                {
                    Logger.DemoInfo($"Обнаружена пробирка со штрихкодом [{barCode}].");

                    tubeCell.Tube = searchBarcodeInDatabase(barCode);

                    if(tubeCell.Tube != null)
                    {
                        Logger.DemoInfo($"Пробирка со штрихкодом [{barCode}] найдена в списке анализов!");
                        tubeCell.Tube.IsFind = true;
                    }
                    else
                    {
                        Logger.DemoInfo($"Пробирка со штрихкодом [{barCode}] не найдена в списке анализов!");
                    }
                    break;
                }
                else
                {
                    Logger.DemoInfo($"Пробирка не обнаружена.");
                }
                attempt++;
            }

            Logger.DemoInfo($"Поиск пробирки (сканирование) завершен.");
        }

        /// <summary>
        /// Выполнение завершающей задачи 
        /// (перенос материала из белой кюветы в прозрачную кювету и снятие показаний с датчика)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void performingFinishTask(TubeInfo tube)
        {
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - запуск выполнения завершающей стадии.");

            needleWashingTask();

            Core.Rotor.Home();
            Core.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.Stages.Count - 1].CartridgePosition, 
                CartridgeCell.WhiteCell);

            Core.Needle.HomeLift();
            Core.Needle.TurnToCartridge(CartridgeCell.WhiteCell);

            Core.Needle.GoDownAndPierceCartridge(CartridgeCell.WhiteCell);
            Core.Pomp.Suction(0);

            Core.Needle.HomeLift();

            // Далее нужно перелить в прозрачную кювету и отправить на анализ.
            
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - выполнения завершающей стадии завершено.");
        }

        /// <summary>
        /// Выполнение промежуточной задачи (добавление реагентов из ячейки в белую кювету)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void performingIntermediateTask(TubeInfo tube)
        {
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - запуск выполнения {tube.CurrentStage}-й стадии.");

            Core.Needle.HomeAll();
            needleWashingTask();

            // Подводим нужную ячейку картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                tube.Stages[tube.CurrentStage].Cell,
                RotorController.CellPosition.CellLeft);

            // Устанавливаем иглу над нужной ячейкой картриджа
            Core.Needle.TurnToCartridge(tube.Stages[tube.CurrentStage].Cell);

            // Прокалываем ячейку картриджа
            Core.Needle.GoDownAndPierceCartridge(tube.Stages[tube.CurrentStage].Cell);

            // Поднимаемся на безопасную высоту над картриджем
            Core.Needle.GoToSafeLevel();

            // Подводим центр ячейки картриджа под иглу
            Core.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                tube.Stages[tube.CurrentStage].Cell,
                RotorController.CellPosition.CellCenter);

            // Прокалываем ячейку картриджа
            Core.Needle.GoDownAndPierceCartridge(tube.Stages[tube.CurrentStage].Cell);

            // Забираем реагент из ячейки картриджа
            Core.Pomp.Suction(0);

            // Поднимаемся на безопасную высоту над картриджем
            Core.Needle.GoToSafeLevel();

            // Устанавливаем иглу над белой ячейкой картриджа
            Core.Needle.TurnToCartridge(CartridgeCell.WhiteCell);

            // Подводим белую кювету картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.PlaceCellUnderNeedle(
                tube.Stages[tube.CurrentStage].CartridgePosition,
                CartridgeCell.WhiteCell);

            // Опускаем иглу в белую кювету
            Core.Needle.GoDownAndPierceCartridge(CartridgeCell.WhiteCell, false);

            // Сливаем реагент в белую кювету
            Core.Pomp.Unsuction(0);

            // Поднимаем иглу до дома
            Core.Needle.HomeLift();

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - завершено выполнение {tube.CurrentStage}-й стадии.");
        }

        private void performingChargeTask()
        {
            Core.Rotor.Home();
            Core.Rotor.PlaceCellAtCharge(0, 5);
            Core.Charger.HomeHook();
            Core.Charger.HomeRotator();
            Core.Charger.TurnToCell(5);
            Core.Charger.ChargeCartridge();
        }


        /// <summary>
        /// Выполнение подготовительной задачи (включает забор материала из пробирки)
        /// </summary>
        /// <param name="tube">Пробирка</param>
        private void performingPreparatoryTask(TubeInfo tube)
        {
            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - запущено выполнение подготовительной стадии.");

            Logger.DemoInfo($"Подготовка к забору материала из пробирки.");
            
            // Смещаем пробирку, чтобы она оказалась под иглой
            Core.Transporter.Shift(false, TransporterController.ShiftType.HalfTube);

            // Поднимаем иглу вверх до дома
            Core.Needle.HomeLift();

            Logger.DemoInfo($"Ожидание загрузки картриджа...");

            performingChargeTask();

            Logger.DemoInfo($"Загрузка картриджа завершена.");

            Logger.DemoInfo($"Ожидание касания жидкости в пробирке...");

            // Устанавливаем иглу над пробиркой и опускаем ее до контакта с материалом в пробирке
            Core.Needle.TurnToTubeAndWaitTouch();

            Logger.DemoInfo($"Забор материала из пробирки.");

            // Набираем материал из пробирки
            Core.Pomp.Suction(0);
            
            // Подводим белую кювету картриджа под иглу
            Core.Rotor.Home();
            Core.Rotor.PlaceCellUnderNeedle( 
                tube.Stages[0].CartridgePosition,
                CartridgeCell.WhiteCell);

            // Поднимаем иглу вверх до дома
            Core.Needle.HomeLift();

            // Устанавливаем иглу над белой ячейкой картриджа
            Core.Needle.TurnToCartridge(CartridgeCell.WhiteCell);

            // Опускаем иглу в кювету
            Core.Needle.GoDownAndPierceCartridge(CartridgeCell.WhiteCell);


            Logger.DemoInfo($"Слив забранного материала в белую кювету.");

            // Сливаем материал в белую кювету
            Core.Pomp.Unsuction(0);

            // Поднимаем иглу вверх до дома
            Core.Needle.HomeLift();
            
            // Промываем иглу
            needleWashingTask();

            Logger.DemoInfo($"Перенос реагента в белую кювету.");

            // Выполняем перенос реагента из нужной ячейки картриджа в белую кювету
            performingIntermediateTask(tube);
            
            // Устанавливаем иглу в домашнюю позицию
            Core.Needle.HomeAll();

            // Смещаем пробирку обратно
            Core.Transporter.Shift(true, TransporterController.ShiftType.HalfTube);

            Logger.DemoInfo($"Пробирка [{tube.BarCode}] - завершено выполнение подготовительной стадии.");
        }
    }
}
