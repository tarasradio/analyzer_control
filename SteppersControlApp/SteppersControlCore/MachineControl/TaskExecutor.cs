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

        private const int tubesCount = 54;

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

            for (int i = 0; i < tubesCount; i++)
            {
                GoToNextTube(); WaitExecution();

                if (Core.GetLastBarCode() != null)
                {
                    Logger.AddMessage($"Пробирка под номером {i} прочитана!");
                    lastScanTube = i;
                    countScanTubes++;
                }
            }

            int countTubes = lastScanTube - countScanTubes + 9;

            MoveFirstTubeUnderNeedle(countTubes); WaitExecution();

            MoveNeedleOnTube(); WaitExecution();
        }

        private void WashingPompTask()
        {
            WashingPomp(50); WaitExecution();
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
            
            command = new SetSpeedCommand(17, 1000, Protocol.GetPacketId());
            commands.Add(command);

            //Поднятие иглы
            steppers = new Dictionary<int, int>() { { 17, -500 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            command = new SetSpeedCommand(8, 1000, Protocol.GetPacketId());
            commands.Add(command);

            //Поворот иглы
            steppers = new Dictionary<int, int>() { { 8, 200 } };
            command = new HomeCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);
        }

        void MoveNeedleOnTube()
        {
            commands.Clear();

            //Поворот иглы до пробирки
            command = new SetSpeedCommand(8, 200, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 8, -14000 } };
            command = new MoveCncCommand(steppers, Protocol.GetPacketId());
            commands.Add(command);

            //Опусание иглы до жидкости в пробирке
            command = new SetSpeedCommand(17, 500, Protocol.GetPacketId());
            commands.Add(command);

            steppers = new Dictionary<int, int>() { { 17, 500 } };
            command = new RunCncCommand(steppers, 0, 1000, Protocol.ValueEdge.RisingEdge, Protocol.GetPacketId());
            commands.Add(command);
        }
    }
}
