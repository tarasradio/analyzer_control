using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AnalyzerService.ExecutionControl
{
    public class CommandExecutor : ICommandExecutor
    {
        public event Action<int> CommandExecuted;

        private const int timeToWaitResponse = 2000;
        
        private Thread executionThread;
        Stopwatch timer;

        private List<ICommand> commands;

        private uint ExecutedCommandId { get; set; }

        private CommandStateResponse.CommandStates ExecutedCommandState { get; set; }

        public CommandExecutor()
        {
            commands = new List<ICommand>();
            timer = new Stopwatch();
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
            this.commands = commands;

            AbortExecution();

            executionThread = new Thread(CommandExecutionLoop)
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

        public void UpdateExecutedCommandInfo(uint id, CommandStateResponse.CommandStates state)
        {
            ExecutedCommandId = id;
            ExecutedCommandState = state;
        }

        private void CommandExecutionLoop()
        {
            int commandNumber = 0;

            foreach (ICommand command in commands)
            {
                ExecuteCommand(command);
                commandNumber++;

                CommandExecuted?.Invoke(commandNumber);
            }
        }
        
        private void ExecuteCommand(ICommand command)
        {
            ExecutedCommandId = 0;

            if(command is IHostCommand)
            {
                ExecuteHostCommand((IHostCommand)command);
            }
            else if(command is IRemoteCommand)
            {
                ExecuteRemoteCommand((IRemoteCommand)command);
            }
        }

        private void ExecuteHostCommand(IHostCommand command)
        {
            (command).Execute();
        }

        private void ExecuteRemoteCommand(IRemoteCommand command)
        {
            Analyzer.Serial.SendPacket(command.GetBytes());

            timer.Restart();

            bool commandExecuted = false;

            while (!commandExecuted)
            {
                if(timer.ElapsedMilliseconds >= timeToWaitResponse)
                {
                    Analyzer.Serial.SendPacket(command.GetBytes());

                    timer.Restart();
                }

                if (Protocol.CommandTypes.SIMPLE_COMMAND == command.GetType())
                {
                    commandExecuted = CheckCommandStatus(command, 
                        CommandStateResponse.CommandStates.COMMAND_EXECUTE_STARTED);
                }
                else if(Protocol.CommandTypes.WAITING_COMMAND == command.GetType())
                {
                    commandExecuted = CheckCommandStatus(command,
                        CommandStateResponse.CommandStates.COMMAND_EXECUTE_FINISHED);
                }
            }

            timer.Stop();
        }

        private bool CheckCommandStatus(IRemoteCommand command, CommandStateResponse.CommandStates expectedState)
        {
            return ExecutedCommandId == command.GetId() && ExecutedCommandState == expectedState;
        }
    }
}
