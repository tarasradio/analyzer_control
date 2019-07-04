using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.SerialCommunication;

namespace SteppersControlCore.MachineControl
{
    public class CncExecutor
    {
        private Logger _logger;
        private SerialHelper _helper;

        private static List<ICommand> _commandsToSend = new List<ICommand>();
        private Thread _executionThread;

        private static uint _lastSuccesCommandId;
        private static Protocol.CommandStates _lastCommandState;
        private static Mutex _mutex = new Mutex();

        private static int countBadPackets = 0;

        public CncExecutor(Logger logger, SerialHelper helper)
        {
            _logger = logger;
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

        public void StartExecution(List<ICommand> commands)
        {
            _commandsToSend = commands;

            _executionThread = new Thread(commandsExecution);
            _executionThread.Start();

            _logger.AddMessage("Запущено выполнение программы.");
        }

        public void AbortExecution()
        {
            _executionThread?.Abort();
            _logger.AddMessage("Выполнение программы было прерванно.");
        }

        static int stage = 0;

        private void commandsExecution()
        {
            stage = 0;
            int commandNumber = 0;
            foreach (ICommand command in _commandsToSend)
            {
                _logger.AddMessage("Команда " + commandNumber + " отправленна !");
                executeCommand(command);
                
                _logger.AddMessage("Команда " + commandNumber + " выполнена успешно !");

                commandNumber++;
            }
            _logger.AddMessage("Все команды выполнены успешно !");
            _logger.AddMessage($"Пакетов с ошибками {countBadPackets}!");
            _logger.AddMessage("Выполнение программы завершено.");
        }
        
        private void executeCommand(ICommand command)
        {
            setSuccesCommandId(0);

            _helper.SendBytes(command.GetBytes());
            
            bool isOk = false;

            while (!isOk)
            {
                switch(command.GetType())
                {
                    case Protocol.CommandType.SIMPLE_COMMAND:
                        {
                            if(getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_OK)
                            {
                                isOk = true;
                                break;
                            }
                        }
                        break;
                    case Protocol.CommandType.WAITING_COMMAND:
                        {
                            switch(stage)
                            {
                                case 0:
                                    {
                                        if (getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_OK)
                                        {
                                            stage++;
                                        }
                                    }
                                    break;
                                case 1:
                                    {
                                        if (getSuccesCommandId() == command.GetId() && getSuccesCommandState() == Protocol.CommandStates.COMMAND_DONE)
                                        {
                                            stage = 0;
                                            isOk = true;
                                            break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }
        
    }
}
