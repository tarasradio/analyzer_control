using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SteppersControlCore.MachineControl;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.AdditionalCommands;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.SerialCommunication;

namespace SteppersControlCore.MachineControl
{
    public class TaskExecutor
    {
        private static object _syncRoot = new object();
        private Thread _executionThread;

        private IAbstractCommand command;
        private Dictionary<int, int> steppers = new Dictionary<int, int>();
        private List<IAbstractCommand> commands = new List<IAbstractCommand>();

        CncExecutor _cncExecutor;

        private const int allTubesCount = 54;

        class TubeInfo
        {
            public int id = 0; // номер пробирки
            public bool needHandle = false; // нужно ли обрабатывать пробирку
            public string barCode = "";

            public TubeInfo()
            {

            }
        };

        private TubeInfo[] tubes = new TubeInfo[allTubesCount];

        public TaskExecutor(CncExecutor executor)
        {
            _cncExecutor = executor;
        }

        public int GetCommandsCount()
        {
            return commands.Count;
        }

        public void AbortExecution()
        {
            lock(_syncRoot)
            {
                _executionThread?.Abort();
            }
        }

        public ThreadState GetState()
        {
            return _executionThread.ThreadState;
        }

        public void StartScanTubesTask()
        {
            lock(_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
                _executionThread = new Thread(ScanTubesTask);
                _executionThread.Priority = ThreadPriority.Lowest;
                _executionThread.IsBackground = true;
                _executionThread.Start();
            }
            
        }

        public void StartWashingPompTask()
        {
            lock(_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
                _executionThread = new Thread(WashingPompTask);
                _executionThread.Priority = ThreadPriority.Lowest;
                _executionThread.IsBackground = true;
                _executionThread.Start();
            }
        }

        public void StartMoveLoadTask(int place)
        {
            needPlace = place;
            lock(_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
                _executionThread = new Thread(MoveLoadTask);
                _executionThread.IsBackground = true;
                _executionThread.Priority = ThreadPriority.Lowest;
                _executionThread.Start();
            }
        }

        public void StartHomingLoadShuttleTask()
        {
            lock(_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
                _executionThread = new Thread(HomingLoadShuttleTask);
                _executionThread.Priority = ThreadPriority.Lowest;
                _executionThread.IsBackground = true;
                _executionThread.Start();
            }
        }

        public void StartLoadingTask()
        {
            lock(_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
                _executionThread = new Thread(LoadingTask);
                _executionThread.Priority = ThreadPriority.Lowest;
                _executionThread.IsBackground = true;
                _executionThread.Start();
            }
        }

        private void WaitExecution(List<IAbstractCommand> task)
        {
            _cncExecutor.ExecuteTask(task);
        }

        private void ScanTubesTask()
        {
            int lastScanTube = 0;
            int countScanTubes = 0;

            WaitExecution(NeedleHoming());

            WaitExecution(PrepareTubesBeforeScanning());

            for (int i = 0; i < allTubesCount; i++)
            {
                WaitExecution(GoToNextTube());

                tubes[i] = new TubeInfo();

                if (Core.GetLastBarCode() != null)
                {
                    tubes[i].needHandle = true;
                    tubes[i].barCode = Core.GetLastBarCode();
                    Logger.AddMessage($"Пробирка под номером {i} прочитана!");
                    lastScanTube = i;
                    countScanTubes++;
                }

                if(i >= 7) // первая пробирка уже под иглой
                {
                    if(true == tubes[i - 7].needHandle)
                    {
                        // обработка пробирки
                        WaitExecution(MoveTube(oneTubeSteps / 2));

                        WaitExecution(MoveNeedleOnTube()); // подъезд иглы и опускание в пробирку

                        WaitExecution(moveCartridgeToNeedle());

                        WaitExecution(Suction()); // набор из пробирки

                        WaitExecution(MoveNeedleToCartridge(false));

                        WaitExecution(Unsuction()); // Слив из иглы в картридж
                        
                        WashingPompTask(); // промывка иглы

                        WaitExecution(MoveNeedleToCartridge(true));

                        WaitExecution(Suction()); // набор из картриджа

                        WaitExecution(NeedleHoming());

                        WaitExecution(MoveTube(-oneTubeSteps / 2));

                        Logger.AddMessage($"Пробирка под номером {i - 7} обработана!");
                    }
                }
            }

            Logger.AddMessage($"Все пробирки отсканированны!");

            //int countTubes = lastScanTube - countScanTubes;

            //Logger.AddMessage($"Нужно отъехать на {countTubes} пробирок.");

            //MoveFirstTubeUnderNeedle(countTubes); WaitExecution();

            //MoveNeedleOnTube(); WaitExecution();
        }

        private void WashingPompTask()
        {
            WaitExecution(NeedleHoming());
            WaitExecution(MoveNeedleOnWashingPlace());
            WaitExecution(WashingPomp(4));
            WaitExecution(HomingPomp());
            //NeedleHoming(); WaitExecution();
        }

        private void HomingLoadShuttleTask()
        {
            WaitExecution(homingLoadShuttle());
        }

        private void LoadingTask()
        {
            WaitExecution(loadingShuttle());
        }

        List<IAbstractCommand> HomingPomp()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            command = new OnDeviceCncCommand(new List<int>() { 0 });
            commands.Add(command);

            command = new OffDeviceCncCommand(new List<int>() { 1 });
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 11, 950 }, { 12, 950 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 11, 950 }, { 12, 950 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> WashingPomp(int cycles)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            for (int i = 0; i < cycles; i++)
            {
                command = new OnDeviceCncCommand(new List<int>() { 0 });
                commands.Add(command);

                command = new OffDeviceCncCommand(new List<int>() { 1 });
                commands.Add(command);

                steppers = new Dictionary<int, int>() { { 11, 950 }, { 12, 950 } };
                command = new SetSpeedCncCommand(steppers);
                commands.Add(command);

                steppers = new Dictionary<int, int>() { { 11, 950 }, { 12, 950 } };
                command = new HomeCncCommand(steppers);
                commands.Add(command);

                command = new OffDeviceCncCommand(new List<int>() { 0 });
                commands.Add(command);

                command = new OnDeviceCncCommand(new List<int>() { 1 });
                commands.Add(command);

                steppers = new Dictionary<int, int>() { { 11, 280 }, { 12, 280 } };
                command = new SetSpeedCncCommand(steppers);
                commands.Add(command);

                steppers = new Dictionary<int, int>() { { 11, -700000 }, { 12, -700000 } };
                command = new MoveCncCommand(steppers);
                commands.Add(command);
            }

            return commands;
        }

        const int oneTubeSteps = 6400;

        List<IAbstractCommand> PrepareTubesBeforeScanning()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            command = new SetSpeedCommand(6, 100);
            commands.Add(command);

            // Обнуление ленты конвеера
            steppers = new Dictionary<int, int>() { { 6, 50 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            // Сдвиг на пол пробирки
            steppers = new Dictionary<int, int>() { { 6, oneTubeSteps / 2 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        //сдвиг на одну пробирку
        List<IAbstractCommand> MoveTube(int steps)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            command = new SetSpeedCommand(6, 50);
            commands.Add(command);
            
            steppers = new Dictionary<int, int>() { { 6, steps } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> GoToNextTube()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            //сдвиг на одну пробирку
            command = new SetSpeedCommand(6, 40);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 6, oneTubeSteps } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            // Сканирование пробирки
            command = new BarStartCommand();
            commands.Add(command);

            // Вращение пробирки
            command = new SetSpeedCommand(5, 30);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 5, 10000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> MoveFirstTubeUnderNeedle(int countTubes)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            command = new SetSpeedCommand(6, 30);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 6, oneTubeSteps * countTubes } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int> { { 6, oneTubeSteps / 4 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> NeedleHoming()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            //Поднятие иглы
            command = new SetSpeedCommand(17, 1000);
            commands.Add(command);
            
            steppers = new Dictionary<int, int>() { { 17, -500 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            command = new SetSpeedCommand(8, 1000);
            commands.Add(command);

            //Поворот иглы
            steppers = new Dictionary<int, int>() { { 8, 100 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> MoveNeedleOnWashingPlace()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            //Поворот иглы до очистки
            command = new SetSpeedCommand(8, 100);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 8, -55000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            //Опусание иглы до жидкости в пробирке
            command = new SetSpeedCommand(17, 500);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, 5000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> MoveNeedleOnTube()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            //Поворот иглы до пробирки
            command = new SetSpeedCommand(8, 100);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 8, -14500 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            //Опусание иглы до жидкости в пробирке
            command = new SetSpeedCommand(17, 500);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, 500 } };
            command = new RunCncCommand(steppers, 0, 1000, Protocol.ValueEdge.RisingEdge);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> Suction()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            command = new OffDeviceCncCommand(new List<int>() { 0 });
            commands.Add(command);

            command = new OnDeviceCncCommand(new List<int>() { 1 });
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 12, 200 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 12, -100000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            command = new OffDeviceCncCommand(new List<int>() { 1 });
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> Unsuction()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            command = new OnDeviceCncCommand(new List<int>() { 0 });
            commands.Add(command);

            command = new OffDeviceCncCommand(new List<int>() { 1 });
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 12, 200 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 12, 200 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            command = new OffDeviceCncCommand(new List<int>() { 0 });
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> MoveNeedleToCartridge(bool fromWashingPlace)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            //Поднятие иглы
            command = new SetSpeedCommand(17, 1000);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, -500 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);
            
            //поворот иглы до картриджа
            command = new SetSpeedCommand(8, 100);
            commands.Add(command);

            int steps = -31000;

            if (fromWashingPlace) steps = 10000;

            steppers = new Dictionary<int, int>() { { 8, steps } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            //Протыкание картриджа
            command = new SetSpeedCommand(17, 200);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, 270000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> moveCartridgeToNeedle()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            command = new SetSpeedCommand(7, 100);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 7, -100 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            command = new SetSpeedCommand(7, 50);
            commands.Add(command);

            //поворот ротора под загрузку картриджа (5 место)
            steppers = new Dictionary<int, int>() { { 7, 81360 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            // поврот ротора под иглу
            steppers = new Dictionary<int, int>() { { 7, -71600 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        int needPlace = 0;

        private void MoveLoadTask()
        {
            WaitExecution(HomingLoad());
            WaitExecution(MoveLoadToPlace(needPlace));
        }

        static int currentRotorPlace = 0;
        const int oneRotorPlaseSteps = 3400;

        int[] loadPlaces = 
        {
            800,
            4000,
            6800,
            10000,
            13200,
            16000,
            19300,
            22300,
            25300,
            28500
        };

        List<IAbstractCommand> HomingLoad()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { 10, 50 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 10, -50 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> MoveLoadToFirstPlace()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            currentRotorPlace = 0;

            steppers = new Dictionary<int, int>() { { 10, 30 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 10, 800 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> MoveLoadToPlace(int i)
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            currentRotorPlace = i;
            
            steppers = new Dictionary<int, int>() { { 10, 30 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 10, loadPlaces[i] } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> homingRotor()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { 7, 50 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 7, -50 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> moveRotorToLoad()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { 7, 50 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 7, 20000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> homingLoadShuttle()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { 15, 500 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, 500 } };
            command = new HomeCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, -20000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }

        List<IAbstractCommand> loadingShuttle()
        {
            List<IAbstractCommand> commands = new List<IAbstractCommand>();

            steppers = new Dictionary<int, int>() { { 15, 200 } };
            command = new SetSpeedCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, -1000000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, -1000000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, -840000 } };
            command = new MoveCncCommand(steppers);
            commands.Add(command);

            return commands;
        }
    }
}
