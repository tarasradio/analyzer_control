using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.SerialCommunication;
using System.Diagnostics;

namespace SteppersControlCore.MachineControl
{
    public delegate void CommandExecutedDelegate(int executedCommandNumber);

    public class CncExecutor
    {
        private static object _syncRoot = new object();

        public event CommandExecutedDelegate CommandExecuted;

        private SerialHelper _helper;

        private static List<IAbstractCommand> _commandsToSend = new List<IAbstractCommand>();
        private Thread _executionThread;

        private static uint _lastSuccesCommandId;
        private static Protocol.CommandStates _lastCommandState;
        private static Mutex _mutex = new Mutex();

        private static int countBadPackets = 0;

        public bool isExecute = false;

        public CncExecutor(SerialHelper helper)
        {
            _helper = helper;
        }

        public void ChangeSuccesCommandId(uint commandId, Protocol.CommandStates state)
        {
            setSuccesCommandId(commandId);
            setSuccesCommandState(state);
        }

        private static void setSuccesCommandId(uint commandId)
        {
            lock(_syncRoot)
            {
                _lastSuccesCommandId = commandId;
            }
        }

        private static void setSuccesCommandState(Protocol.CommandStates state)
        {
            lock(_syncRoot)
            {
                _lastCommandState = state;
            }
        }

        private static uint getSuccesCommandId()
        {
            uint commandId;

            lock (_syncRoot)
            {
                commandId = _lastSuccesCommandId;
            }

            return commandId;
        }

        private static Protocol.CommandStates getSuccesCommandState()
        {
            Protocol.CommandStates commandState;

            lock (_syncRoot)
            {
                commandState = _lastCommandState;
            }

            return commandState;
        }

        public void ExecuteTask(List<IAbstractCommand> commands)
        {
            _commandsToSend = commands;

            if (_executionThread != null && _executionThread.IsAlive)
                _executionThread.Abort();
            _executionThread = new Thread(commandsExecution);
            _executionThread.Priority = ThreadPriority.Lowest;
            _executionThread.Start();

            while (_executionThread.ThreadState == System.Threading.ThreadState.Running);
            Logger.AddMessage("Запущено выполнение задания");
        }

        public void StartExecution(List<IAbstractCommand> commands)
        {
            _commandsToSend = commands;

            if (_executionThread != null && _executionThread.IsAlive)
                _executionThread.Abort();
            _executionThread = new Thread(commandsExecution);
            _executionThread.Priority = ThreadPriority.Lowest;
            _executionThread.IsBackground = true;
            _executionThread.Start();

            isExecute = true;

            Logger.AddMessage("Запущено выполнение программы.");
        }

        public void AbortExecution()
        {
            isExecute = false;
            if (_executionThread != null && _executionThread.IsAlive)
                _executionThread.Abort();
            Logger.AddMessage("Выполнение программы было прерванно.");
        }
        
        enum ExecutionStages
        {
            WAIT_OK,
            WAIT_COMPLETE
        }

        static ExecutionStages stage = 0;

        Stopwatch stopWatch = new Stopwatch();
        
        private void commandsExecution()
        {
            stage = 0;
            int commandNumber = 0;
            foreach (IAbstractCommand command in _commandsToSend)
            {
                Logger.AddMessage("Команда " + commandNumber + " запущена !");

                executeCommand(command);

                Logger.AddMessage("Команда " + commandNumber + " выполнена успешно !");
                
                commandNumber++;

                CommandExecuted(commandNumber);
            }

            isExecute = false;
            Logger.AddMessage("Все команды выполнены успешно !");
            Logger.AddMessage($"Пакетов с ошибками {countBadPackets}!");
            Logger.AddMessage("Выполнение программы завершено.");
        }
        
        private void executeCommand(IAbstractCommand command)
        {
            setSuccesCommandId(0);

            if(command is IHostCommand)
            {
                executeHostCommand((IHostCommand)command);
            }
            else if(command is IDeviceCommand)
            {
                executeDeviceCommand((IDeviceCommand)command);
            }
        }

        private void executeDeviceCommand(IDeviceCommand command)
        {
            _helper.SendPacket((command).GetBytes());

            stopWatch.Restart();

            while (true)
            {
                if(stopWatch.ElapsedMilliseconds >= 2000)
                {
                    Logger.AddMessage("Слишком долгое ожидание ответа от устройства");

                    _helper.SendPacket((command).GetBytes());

                    stopWatch.Restart();
                }

                if (Protocol.CommandTypes.SIMPLE_COMMAND == command.GetType())
                {
                    if (getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_OK)
                    {
                        break;
                    }
                }
                else if(Protocol.CommandTypes.WAITING_COMMAND == command.GetType())
                {
                    if(ExecutionStages.WAIT_OK == stage)
                    {
                        if (getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_OK)
                        {
                            stage = ExecutionStages.WAIT_COMPLETE;
                        }
                    }
                    else if(ExecutionStages.WAIT_COMPLETE == stage)
                    {
                        if (getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_DONE)
                        {
                            stage = ExecutionStages.WAIT_OK;
                            break;
                        }
                    }
                }
            }

            stopWatch.Stop();
        }

        private void executeHostCommand(IHostCommand command)
        {
            (command).Execute();
        }
    }
}
