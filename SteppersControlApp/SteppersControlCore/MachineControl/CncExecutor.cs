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
        public event CommandExecutedDelegate CommandExecuted;

        private SerialHelper _helper;

        private static List<IAbstractCommand> _commandsToSend = new List<IAbstractCommand>();
        private Thread _executionThread;

        private static uint _lastSuccesCommandId;
        private static Protocol.CommandStates _lastCommandState;
        private static Mutex _mutex = new Mutex();

        private static int countBadPackets = 0;

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
            _mutex.WaitOne();
            _lastSuccesCommandId = commandId;
            _mutex.ReleaseMutex();
        }

        private static void setSuccesCommandState(Protocol.CommandStates state)
        {
            _mutex.WaitOne();
            _lastCommandState = state;
            _mutex.ReleaseMutex();
        }

        private static uint getSuccesCommandId()
        {
            uint commandId;

            _mutex.WaitOne();
            commandId = _lastSuccesCommandId;
            _mutex.ReleaseMutex();

            return commandId;
        }

        private static Protocol.CommandStates getSuccesCommandState()
        {
            Protocol.CommandStates commandState;

            _mutex.WaitOne();
            commandState = _lastCommandState;
            _mutex.ReleaseMutex();

            return commandState;
        }

        public void StartExecution(List<IAbstractCommand> commands)
        {
            _commandsToSend = commands;

            _executionThread = new Thread(commandsExecution);
            _executionThread.Start();

            Logger.AddMessage("Запущено выполнение программы.");
        }

        public void AbortExecution()
        {
            _executionThread?.Abort();
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
            Logger.AddMessage("Все команды выполнены успешно !");
            Logger.AddMessage($"Пакетов с ошибками {countBadPackets}!");
            Logger.AddMessage("Выполнение программы завершено.");
        }
        
        private void executeCommand(IAbstractCommand command)
        {
            setSuccesCommandId(0);

            if(Protocol.CommandTypes.HOST_COMMAND == command.GetType())
            {
                executeHostCommand(command);
            }
            else
            {
                executeDeviceCommand(command);
            }
        }

        private void executeDeviceCommand(IAbstractCommand command)
        {
            _helper.SendBytes(((IDeviceCommand)command).GetBytes());

            bool executionFinished = false;

            stopWatch.Start();

            while (!executionFinished)
            {
                TimeSpan ts = stopWatch.Elapsed;

                if(ts.Seconds >= 5)
                {
                    Logger.AddMessage("Слишком долгое ожидание ответа от устройства");

                    _helper.SendBytes(((IDeviceCommand)command).GetBytes());

                    stopWatch.Restart();
                }

                if (Protocol.CommandTypes.SIMPLE_COMMAND == command.GetType())
                {
                    if (getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_OK)
                    {
                        executionFinished = true;
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
                            executionFinished = true;
                            break;
                        }
                    }
                }
            }

            stopWatch.Stop();
        }

        private void executeHostCommand(IAbstractCommand command)
        {
            ((IHostCommand)command).Execute();
        }
    }
}
