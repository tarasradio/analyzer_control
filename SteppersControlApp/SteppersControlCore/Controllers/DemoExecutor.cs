using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SteppersControlCore.Controllers
{
    public class DemoExecutor
    {
        string filename = "Tubes.xml";
        private int numberTubeCells = 54;

        public List<TubeInfo> Tubes { get; set; }
        private TubeCell[] cells;

        int currentCell = 0;

        public DemoExecutor()
        {
            Tubes = new List<TubeInfo>();
            cells = new TubeCell[numberTubeCells];
        }

        public void WriteXml()
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<TubeInfo>));

            TextWriter writer = new StreamWriter(filename);
            ser.Serialize(writer, Tubes);
            writer.Close();
        }

        //Чтение насроек из файла
        public void ReadXml()
        {
            if (File.Exists(filename))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<TubeInfo>));
                TextReader reader = new StreamReader(filename);
                Tubes = ser.Deserialize(reader) as List<TubeInfo>;
                reader.Close();
            }
            else
            {
                //можно написать вывод сообщения если файла не существует
            }
        }

        public void StartDemo()
        {
            Core.Executor.StartTask(() =>
            {
                DemoTask();
            });
        }

        private void DemoTask()
        {
            foreach (TubeInfo tube in Tubes)
            {
                tube.IsFind = false;
                tube.CurrentStage = 0;
                tube.TimeToStageComplete = 0;
            }
               
            initTask();
            washingNeedleTask();

            Logger.AddMessage($"Prepare before scanning task [Start]");
            WaitExecution(Core.Arm.Home());
            WaitExecution(Core.Transporter.PrepareBeforeScanning());

            Logger.AddMessage($"Prepare before scanning task [Finish]");

            currentCell = 0;

            while (haveTasks()) // пока есть задачи
            {
                if (currentCell == numberTubeCells) // прошли полный круг
                {
                    currentCell = 0;
                }

                if (findTubeTask(3) == false)
                {
                    Logger.AddMessage($"Пробирка со штрихкодом [{cells[currentCell].BarCode}] не найдена в списке задач!");
                }
                
                if(currentCell >= 7 && cells[currentCell - 7].HaveTube == true) // первая пробирка уже под иглой
                {
                    TubeInfo tube = findBarCode(cells[currentCell - 7].BarCode);
                    
                    if(null != tube)
                    {
                        tube.IsFind = true;
                        Logger.AddMessage($"Пробирка со штрихкодом {tube.BarCode} запущена в обработку!");
                        // постановка на выполнение задач и забор из пробирки в белую кювету

                        FirstStagePrepareTask(tube);

                        tube.TimeToStageComplete = tube.Stages[tube.CurrentStage].TimeToPerform;
                        tube.CurrentStage++;
                    }
                }
                
                if (haveCompleteTask()) // есть завершенные или еще не начатые задачи
                {
                    Logger.AddMessage($"Типа выполняем задачи...");
                }
                currentCell++;
            }
            
            Logger.AddMessage($"Все пробирки обработаны!");
        }

        private bool haveCompleteTask()
        {
            foreach (TubeInfo tube in Tubes)
            {
                if (tube.IsFind && tube.TimeToStageComplete == 0)
                    return true;
            }
            return false;
        }

        private TubeInfo findBarCode(string barCode)
        {
            foreach (TubeInfo tube in Tubes)
            {
                if (barCode.Contains(tube.BarCode))
                    return tube;
            }
            return null;
        }

        private bool haveTasks()
        {
            foreach(TubeInfo tube in Tubes)
            {
                if (tube.CurrentStage < tube.Stages.Count)
                    return true;
            }
            return false;
        }

        private void initTask()
        {
            Logger.AddMessage($"Init task [Start]");
            WaitExecution( Core.Arm.Home() );
            WaitExecution( Core.Loader.HomeShuttle() );
            WaitExecution( Core.Loader.HomeLoad() );
            WaitExecution( Core.Rotor.Home() );
            WaitExecution( Core.Pomp.Home() );
            Logger.AddMessage($"Init task [Finish]");
        }

        private void washingNeedleTask()
        {
            Logger.AddMessage($"Washing needle task [Start]");
            WaitExecution( Core.Arm.Home() );
            WaitExecution( Core.Arm.MoveOnWashing() );
            WaitExecution( Core.Pomp.Washing(2) );
            WaitExecution( Core.Pomp.Home() );
            Logger.AddMessage($"Washing needle task [Finish]");
        }
        
        private bool findTubeTask(int countRepeats)
        {
            int numberRepeat = 0;
            bool result = false;

            Logger.AddMessage($"Scan tube task [Start]");
            WaitExecution(Core.Transporter.Shift(false));

            while (numberRepeat < countRepeats)
            {
                WaitExecution(Core.Transporter.TurnAndScanTube());
                cells[currentCell] = new TubeCell();

                String barCode = Core.GetLastBarCode();

                if (barCode != null)
                {
                    cells[currentCell].HaveTube = true;
                    cells[currentCell].BarCode = barCode;
                    Logger.AddMessage($"В ячейке под номером {currentCell} найдена пробирка!");

                    TubeInfo tube = findBarCode(cells[currentCell].BarCode);

                    if(tube != null)
                    {
                        tube.IsFind = true;
                        Logger.AddMessage($"Штрихкод {tube.BarCode} найден  списке задач!");
                        result = true;
                        break;
                    } 
                }
                numberRepeat++;
            }
            
            Logger.AddMessage($"Scan tube task [Finish]");

            return result;
        }

        private void FirstStagePrepareTask(TubeInfo tube)
        {
            Logger.AddMessage($"First stage prepare task [Start]");
            WaitExecution(Core.Transporter.Shift(false, TransporterController.ShiftType.HalfTube));
            WaitExecution(Core.Arm.MoveOnTube()); // подъезд иглы и опускание в пробирку

            WaitExecution(Core.Rotor.Home());
            WaitExecution(Core.Rotor.MoveCellUnderNeedle( 
                tube.Stages[0].CartridgeNumber,
                CartridgeCell.WhiteCell,
                RotorController.CellPosition.CenterCell));

            WaitExecution(Core.Pomp.Suction(0)); // набор из пробирки

            WaitExecution(Core.Arm.MoveToCartridge(
                ArmController.FromPosition.Tube,
                CartridgeCell.WhiteCell));

            WaitExecution(Core.Arm.BrokeCartridge() );

            WaitExecution(Core.Pomp.Unsuction(0)); // слив из иглы в картридж
            WaitExecution(Core.Arm.Home());

            washingNeedleTask(); // Промывка иглы

            //------------------------------------------------------
            WaitExecution(Core.Rotor.Home());
            WaitExecution(Core.Rotor.MoveCellUnderNeedle(
                tube.Stages[0].CartridgeNumber,
                tube.Stages[0].Cell,
                RotorController.CellPosition.CellLeft));

            WaitExecution(Core.Arm.MoveToCartridge(
                ArmController.FromPosition.Washing,
                tube.Stages[0].Cell));

            WaitExecution(Core.Arm.BrokeCartridge() );
            
            WaitExecution(Core.Arm.MoveToCartridge(
                ArmController.FromPosition.FirstCell,
                tube.Stages[0].Cell));

            WaitExecution(Core.Rotor.Home());
            WaitExecution(Core.Rotor.MoveCellUnderNeedle(
                tube.Stages[0].CartridgeNumber,
                tube.Stages[0].Cell,
                RotorController.CellPosition.CellRight));

            WaitExecution(Core.Arm.BrokeCartridge());

            //------------------------------------------------------

            WaitExecution(Core.Pomp.Suction(0)); // набор из первой ячейки (по алгоритму)

            WaitExecution(Core.Arm.MoveToCartridge(
                ArmController.FromPosition.FirstCell,
                CartridgeCell.WhiteCell));
            WaitExecution(Core.Rotor.Home());
            WaitExecution(Core.Rotor.MoveCellUnderNeedle(
                tube.Stages[0].CartridgeNumber,
                CartridgeCell.WhiteCell,
                RotorController.CellPosition.CenterCell));

            WaitExecution(Core.Arm.BrokeCartridge());

            WaitExecution(Core.Pomp.Unsuction(0)); // слив из первой ячейки (по алгоритму)

            WaitExecution(Core.Arm.Home());

            WaitExecution(Core.Transporter.Shift(true, TransporterController.ShiftType.HalfTube));
            Logger.AddMessage($"First stage prepare task [Start]");
        }

        private void WaitExecution(List<IAbstractCommand> task)
        {
            Core.CNCExecutor.ExecuteTask(task);
        }
    }
}
