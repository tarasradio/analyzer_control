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

        private bool isExecute = false;
        private static object _syncRoot = new object();

        private static List<IAbstractCommand> _commandsToSend = new List<IAbstractCommand>();
        private Thread _executionThread;
        private Thread _taskThread;

        private static uint _lastSuccesCommandId;
        private static Protocol.CommandStates _lastCommandState;
        
        public CncExecutor()
        {

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

            isExecute = true;

            while (isExecute)
            {

            }
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
        }

        public void AbortExecution()
        {
            isExecute = false;
            if (_executionThread != null && _executionThread.IsAlive)
                _executionThread.Abort();
            Logger.AddMessage("Выполнение программы было прерванно.");
        }
        
        Stopwatch stopWatch = new Stopwatch();
        
        private void commandsExecution()
        {
            int commandNumber = 0;
            foreach (IAbstractCommand command in _commandsToSend)
            {
                Logger.AddMessage("Команда " + commandNumber + " запущена !");

                executeCommand(command);

                Logger.AddMessage("Команда " + commandNumber + " выполнена успешно !");
                
                commandNumber++;
                CommandExecuted?.Invoke(commandNumber);
            }

            isExecute = false;
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
            Core.Serial.SendPacket(command.GetBytes());

            stopWatch.Restart();

            while (true)
            {
                if(stopWatch.ElapsedMilliseconds >= 2000)
                {
                    Logger.AddMessage("Слишком долгое ожидание ответа от устройства");

                    Core.Serial.SendPacket(command.GetBytes());

                    stopWatch.Restart();
                }

                if (Protocol.CommandTypes.SIMPLE_COMMAND == command.GetType())
                {
                    if (getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_RECEIVED)
                    {
                        break;
                    }
                }
                else if(Protocol.CommandTypes.WAITING_COMMAND == command.GetType())
                {
                    if (getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_EXECUTED)
                    {
                        break; 
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
