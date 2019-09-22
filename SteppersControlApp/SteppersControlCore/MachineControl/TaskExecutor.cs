using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SteppersControlCore.MachineControl;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.Controllers;
using SteppersControlCore.Elements;

namespace SteppersControlCore.MachineControl
{
    public class TaskExecutor
    {
        public delegate void TaskDelegate();
        private static object _syncRoot = new object();
        private Thread _executionThread;

        private const int allTubesCount = 54;

        class TubeCell
        {
            public int Id = 0; // номер пробирки
            public bool NeedHandle = false; // нужно ли обрабатывать пробирку
            public string BarCode = "";

            public TubeCell()
            {

            }
        };

        private TubeCell[] tubes = new TubeCell[allTubesCount];


        public TaskExecutor()
        {

        }

        public void AbortExecution()
        {
            lock(_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
            }
        }

        public ThreadState GetState()
        {
            return _executionThread.ThreadState;
        }

        public void StartTask(TaskDelegate task)
        {
            lock (_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
                _executionThread = new Thread(new ThreadStart(task));
                _executionThread.Priority = ThreadPriority.Lowest;
                _executionThread.IsBackground = true;
                _executionThread.Start();
            }
        }

        public void StartScanTubesTask()
        {
            StartTask(ScanTubesTask);
        }

        public void StartWashingPompTask()
        {
            StartTask(WashingNeedleTask);
        }

        public void StartMoveLoadTask(int place)
        {
            targetCell = place;
            StartTask(MoveLoadTask);
        }

        public void StartHomingLoadShuttleTask()
        {
            StartTask(HomingLoadShuttleTask);
        }

        public void StartLoadingTask()
        {
            StartTask(LoadingTask);
        }

        private void WaitExecution(List<IAbstractCommand> task)
        {
            Core.CNCExecutor.ExecuteTask(task);
        }

        private void ScanTubesTask()
        {
            WashingNeedleTask(); // Промывка иглы

            WaitExecution( Core.Arm.Home() );

            WaitExecution(Core.Transporter.PrepareBeforeScanning() );

            for (int i = 0; i < allTubesCount; i++)
            {
                WaitExecution(Core.Transporter.Shift(false) );

                WaitExecution(Core.Transporter.TurnAndScanTube() );

                tubes[i] = new TubeCell();

                if (Core.GetLastBarCode() != null)
                {
                    tubes[i].NeedHandle = true;
                    tubes[i].BarCode = Core.GetLastBarCode();
                    Logger.AddMessage($"Пробирка под номером {i} прочитана!");
                }

                if(i >= 7) // первая пробирка уже под иглой
                {
                    if(true == tubes[i - 7].NeedHandle)
                    {
                        // обработка пробирки
                        WaitExecution( Core.Transporter.Shift(false, TransporterController.ShiftType.HalfTube) );

                        WaitExecution( Core.Arm.MoveOnTube() ); // подъезд иглы и опускание в пробирку

                        WaitExecution( Core.Rotor.MoveCellUnderNeedle(
                            0, 
                            CartridgeCell.WhiteCell, 
                            RotorController.CellPosition.CenterCell) );

                        WaitExecution( Core.Pomp.Suction(0) ); // набор из пробирки

                        WaitExecution( Core.Arm.MoveToCartridge(
                            ArmController.FromPosition.Tube,
                            CartridgeCell.WhiteCell) );

                        WaitExecution( Core.Pomp.Unsuction(0) ); // Слив из иглы в картридж
                        
                        WashingNeedleTask(); // Промывка иглы

                        WaitExecution( Core.Arm.MoveToCartridge(
                            ArmController.FromPosition.Washing,
                            CartridgeCell.FirstCell) );

                        WaitExecution( Core.Pomp.Suction(0) ); // набор из картриджа

                        WaitExecution( Core.Arm.Home() );

                        WaitExecution( Core.Transporter.Shift(true, TransporterController.ShiftType.HalfTube) );

                        Logger.AddMessage($"Пробирка под номером {i - 7} обработана!");
                    }
                }
            }

            Logger.AddMessage($"Все пробирки отсканированны!");
        }

        private void WashingNeedleTask()
        {
            WaitExecution( Core.Arm.Home() );
            WaitExecution( Core.Arm.MoveOnWashing() );
            WaitExecution( Core.Pomp.Washing(2) );
            WaitExecution( Core.Pomp.Home() );
        }

        private void HomingLoadShuttleTask()
        {
            WaitExecution( Core.Loader.HomeShuttle() );
        }

        private void LoadingTask()
        {
            WaitExecution( Core.Loader.MoveShuttleToCassette() );
        }
        
        private int targetCell = 0;

        private void MoveLoadTask()
        {
            WaitExecution( Core.Loader.HomeLoad() );
            WaitExecution( Core.Loader.TurnLoadToCell(targetCell) );
        }
    }
}
