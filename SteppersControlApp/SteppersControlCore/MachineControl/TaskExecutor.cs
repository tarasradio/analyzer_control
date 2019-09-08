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
            _executionThread?.Abort();
        }

        public void StartScanTubesTask()
        {
            _executionThread = new Thread(ScanTubesTask);
            _executionThread.Start();
        }

        public void StartWashingPompTask()
        {
            _executionThread = new Thread(WashingPompTask);
            _executionThread.Start();
        }

        public void StartMoveLoadTask(int place)
        {
            needPlace = place;
            _executionThread = new Thread(MoveLoadTask);
            _executionThread.Start();
        }

        public void StartHomingLoadShuttleTask()
        {
            _executionThread = new Thread(HomingLoadShuttleTask);
            _executionThread.Start();
        }

        public void StartLoadingTask()
        {
            _executionThread = new Thread(LoadingTask);
            _executionThread.Start();
        }

        private void WaitExecution()
        {
            _cncExecutor.StartExecution(commands);
            while (_cncExecutor.isExecute)
            {
                Thread.Sleep(100);
            }
        }

        private void ScanTubesTask()
        {
            int lastScanTube = 0;
            int countScanTubes = 0;

            NeedleHoming(); WaitExecution();

            PrepareTubesBeforeScanning(); WaitExecution();

            for (int i = 0; i < allTubesCount; i++)
            {
                GoToNextTube(); WaitExecution();

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
                        MoveTube(oneTubeSteps / 2); WaitExecution();

                        MoveNeedleOnTube(); WaitExecution(); // подъезд иглы и опускание в пробирку

                        moveCartridgeToNeedle(); WaitExecution();

                        Suction(); WaitExecution(); // набор из пробирки

                        MoveNeedleToCartridge(false); WaitExecution();

                        Unsuction(); WaitExecution(); // Слив из иглы в картридж
                        
                        WashingPompTask(); // промывка иглы

                        MoveNeedleToCartridge(true); WaitExecution();

                        Suction(); WaitExecution(); // набор из картриджа

                        NeedleHoming(); WaitExecution();

                        MoveTube(-oneTubeSteps / 2); WaitExecution();

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
            NeedleHoming(); WaitExecution();
            MoveNeedleOnWashingPlace(); WaitExecution();
            WashingPomp(4); WaitExecution();
            HomingPomp(); WaitExecution();
            //NeedleHoming(); WaitExecution();
        }

        private void HomingLoadShuttleTask()
        {
            homingLoadShuttle(); WaitExecution();
        }

        private void LoadingTask()
        {
            loadingShuttle(); WaitExecution();
        }

        void HomingPomp()
        {
            commands.Clear();

            command = new OnDeviceCncCommand(new List<int>() { 0 }, Protocol.GetPacketId());
            commands.Add(command);

            command = new OffDeviceCncCommand(new List<int>() { 1 }, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 11, 950 }, { 12, 950 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 11, 950 }, { 12, 950 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }
        
        void WashingPomp(int cycles)
        {
            commands.Clear();

            for (int i = 0; i < cycles; i++)
            {
                command = new OnDeviceCncCommand(new List<int>() { 0 }, Protocol.GetPacketId());
                commands.Add(command);

                command = new OffDeviceCncCommand(new List<int>() { 1 }, Protocol.GetPacketId());
                commands.Add(command);

                steppers = new Dictionary<int, int>() { { 11, 950 }, { 12, 950 } };
                command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
                commands.Add(command);

                steppers = new Dictionary<int, int>() { { 11, 950 }, { 12, 950 } };
                command = new HomeCncCommand(steppers, Protocol.GetPacketId());
                commands.Add(command);

                command = new OffDeviceCncCommand(new List<int>() { 0 }, Protocol.GetPacketId());
                commands.Add(command);

                command = new OnDeviceCncCommand(new List<int>() { 1 }, Protocol.GetPacketId());
                commands.Add(command);

                steppers = new Dictionary<int, int>() { { 11, 280 }, { 12, 280 } };
                command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
                commands.Add(command);

                steppers = new Dictionary<int, int>() { { 11, -700000 }, { 12, -700000 } };
                command = new MoveCncCommand(steppers, Protocol.GetPacketId());
                commands.Add(command);
            }
        }

        const int oneTubeSteps = 6400;

        void PrepareTubesBeforeScanning()
        {
            commands.Clear();

            command = new SetSpeedCommand(6, 100, Protocol.GetPacketId());
            commands.Add(command);

            // Обнуление ленты конвеера
            steppers = new Dictionary<int, int>() { { 6, 50 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            // Сдвиг на пол пробирки
            steppers = new Dictionary<int, int>() { { 6, oneTubeSteps / 2 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        //сдвиг на одну пробирку
        void MoveTube(int steps)
        {
            commands.Clear();
            
            command = new SetSpeedCommand(6, 50, Protocol.GetPacketId());
            commands.Add(command);
            
            steppers = new Dictionary<int, int>() { { 6, steps } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void GoToNextTube()
        {
            commands.Clear();

            //сдвиг на одну пробирку
            command = new SetSpeedCommand(6, 40, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 6, oneTubeSteps } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            // Сканирование пробирки
            command = new BarStartCommand(Protocol.GetPacketId());
            commands.Add(command);

            // Вращение пробирки
            command = new SetSpeedCommand(5, 30, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 5, 10000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void MoveFirstTubeUnderNeedle(int countTubes)
        {
            commands.Clear();
            
            command = new SetSpeedCommand(6, 30, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 6, oneTubeSteps * countTubes } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int> { { 6, oneTubeSteps / 4 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void NeedleHoming()
        {
            commands.Clear();
             //Поднятие иглы
            command = new SetSpeedCommand(17, 1000, Protocol.GetPacketId());
            commands.Add(command);
            
            steppers = new Dictionary<int, int>() { { 17, -500 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            command = new SetSpeedCommand(8, 1000, Protocol.GetPacketId());
            commands.Add(command);

            //Поворот иглы
            steppers = new Dictionary<int, int>() { { 8, 100 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void MoveNeedleOnWashingPlace()
        {
            commands.Clear();

            //Поворот иглы до очистки
            command = new SetSpeedCommand(8, 100, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 8, -55000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            //Опусание иглы до жидкости в пробирке
            command = new SetSpeedCommand(17, 500, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, 5000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void MoveNeedleOnTube()
        {
            commands.Clear();

            //Поворот иглы до пробирки
            command = new SetSpeedCommand(8, 100, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 8, -14500 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            //Опусание иглы до жидкости в пробирке
            command = new SetSpeedCommand(17, 500, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, 500 } };
            command = new RunCncCommand(steppers, 0, 1000, Protocol.ValueEdge.RisingEdge, Protocol.GetPacketId());
            commands.Add(command);
        }

        void Suction()
        {
            commands.Clear();

            command = new OffDeviceCncCommand(new List<int>() { 0 }, Protocol.GetPacketId());
            commands.Add(command);

            command = new OnDeviceCncCommand(new List<int>() { 1 }, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 12, 200 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 12, -100000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            command = new OffDeviceCncCommand(new List<int>() { 1 }, Protocol.GetPacketId());
            commands.Add(command);
        }

        void Unsuction()
        {
            commands.Clear();

            command = new OnDeviceCncCommand(new List<int>() { 0 }, Protocol.GetPacketId());
            commands.Add(command);

            command = new OffDeviceCncCommand(new List<int>() { 1 }, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 12, 200 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 12, 200 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            command = new OffDeviceCncCommand(new List<int>() { 0 }, Protocol.GetPacketId());
            commands.Add(command);
        }

        void MoveNeedleToCartridge(bool fromWashingPlace)
        {
            commands.Clear();
            
            //Поднятие иглы
            command = new SetSpeedCommand(17, 1000, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, -500 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
            
            //поворот иглы до картриджа
            command = new SetSpeedCommand(8, 100, Protocol.GetPacketId());
            commands.Add(command);

            int steps = -31000;

            if (fromWashingPlace) steps = 10000;

            steppers = new Dictionary<int, int>() { { 8, steps } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            //Протыкание картриджа
            command = new SetSpeedCommand(17, 200, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, 270000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void moveCartridgeToNeedle()
        {
            commands.Clear();
            
            command = new SetSpeedCommand(7, 100, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 7, -100 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            command = new SetSpeedCommand(7, 50, Protocol.GetPacketId());
            commands.Add(command);

            //поворот ротора под загрузку картриджа (5 место)
            steppers = new Dictionary<int, int>() { { 7, 81360 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            // поврот ротора под иглу
            steppers = new Dictionary<int, int>() { { 7, -71600 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        int needPlace = 0;

        private void MoveLoadTask()
        {
            HomingLoad(); WaitExecution();
            MoveLoadToPlace(needPlace); WaitExecution();
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

        void HomingLoad()
        {
            commands.Clear();
            
            steppers = new Dictionary<int, int>() { { 10, 50 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 10, -50 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void MoveLoadToFirstPlace()
        {
            currentRotorPlace = 0;
            commands.Clear();

            steppers = new Dictionary<int, int>() { { 10, 30 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 10, 800 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void MoveLoadToPlace(int i)
        {
            currentRotorPlace = i;
            
            commands.Clear();

            steppers = new Dictionary<int, int>() { { 10, 30 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 10, loadPlaces[i] } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void homingRotor()
        {
            commands.Clear();

            steppers = new Dictionary<int, int>() { { 7, 50 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 7, -50 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void moveRotorToLoad()
        {
            commands.Clear();

            steppers = new Dictionary<int, int>() { { 7, 50 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 7, 20000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void homingLoadShuttle()
        {
            commands.Clear();

            steppers = new Dictionary<int, int>() { { 15, 500 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, 500 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, -20000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void loadingShuttle()
        {
            commands.Clear();

            steppers = new Dictionary<int, int>() { { 15, 200 } };
            command = new SetSpeedCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, -1000000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, -1000000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 15, -840000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }
    }
}
