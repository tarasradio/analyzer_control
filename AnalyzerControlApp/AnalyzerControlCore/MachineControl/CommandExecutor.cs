using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AnalyzerControlCore.MachineControl
{
    public delegate void CommandExecutedDelegate(int executedCommandNumber);

    public class CommandExecutor : ICommandExecutor
    {
        public event CommandExecutedDelegate CommandExecuted;

        private const int timeToWait = 2000;
        private static object locker = new object();

        private static List<ICommand> _commands = new List<ICommand>();
        private Thread executionThread;

        private static uint executedCommandId;
        private static CommandStateResponse.CommandStates executedCommandState;

        Stopwatch timer = new Stopwatch();

        public CommandExecutor()
        {

        }

        public void UpdateExecutedCommandState(uint commandId, CommandStateResponse.CommandStates state)
        {
            setExecutedCommandId(commandId);
            setExecutedCommandState(state);
        }

        private static void setExecutedCommandId(uint commandId)
        {
            lock(locker)
            {
                executedCommandId = commandId;
            }
        }

        private static void setExecutedCommandState(CommandStateResponse.CommandStates state)
        {
            lock(locker)
            {
                executedCommandState = state;
            }
        }

        private static uint getExecutedCommandId()
        {
            uint commandId;

            lock (locker)
            {
                commandId = executedCommandId;
            }

            return commandId;
        }

        private static CommandStateResponse.CommandStates getExecutedCommandState()
        {
            CommandStateResponse.CommandStates commandState;

            lock (locker)
            {
                commandState = executedCommandState;
            }

            return commandState;
        }
        
        public void WaitExecution(List<ICommand> commands)
        {
            RunExecution(commands);

            while (executionThread.IsAlive)
            {

            }
        }

        public void RunExecution(List<ICommand> commands)
        {
            _commands = commands;

            AbortExecution();

            executionThread = new Thread(commandsExecution)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };

            executionThread.Start();
        }

        public void AbortExecution()
        {
            if (executionThread != null && executionThread.IsAlive)
                executionThread.Abort();
        }

        private void commandsExecution()
        {
            int commandNumber = 0;
            foreach (ICommand command in _commands)
            {
                //Logger.Info("[Command executor] - Команда " + commandNumber + " запущена !");

                executeCommand(command);

                //Logger.Info("[Command executor] - Команда " + commandNumber + " выполнена успешно !");
                commandNumber++;
                CommandExecuted?.Invoke(commandNumber);
            }
        }
        
        private void executeCommand(ICommand command)
        {
            setExecutedCommandId(0);

            if(command is IHostCommand)
            {
                executeHostCommand((IHostCommand)command);
            }
            else if(command is IRemoteCommand)
            {
                executeRemoteCommand((IRemoteCommand)command);
            }
        }

        private void executeRemoteCommand(IRemoteCommand command)
        {
            Core.Serial.SendPacket(command.GetBytes());

            timer.Restart();

            while (true)
            {
                if(timer.ElapsedMilliseconds >= timeToWait)
                {
                    //Logger.Info("[Command executor] - Слишком долгое ожидание ответа от устройства.");

                    Core.Serial.SendPacket(command.GetBytes());

                    timer.Restart();
                }

                if (Protocol.CommandTypes.SIMPLE_COMMAND == command.GetType())
                {
                    if (getExecutedCommandId() == command.GetId() && 
                        getExecutedCommandState() == CommandStateResponse.CommandStates.COMMAND_EXECUTE_STARTED)
                    {
                        break;
                    }
                }
                else if(Protocol.CommandTypes.WAITING_COMMAND == command.GetType())
                {
                    if (getExecutedCommandId() == command.GetId() && 
                        getExecutedCommandState() == CommandStateResponse.CommandStates.COMMAND_EXECUTE_FINISHED)
                    {
                        break; 
                    }
                }
            }

            timer.Stop();
        }

        private void executeHostCommand(IHostCommand command)
        {
            (command).Execute();
        }
    }
}
