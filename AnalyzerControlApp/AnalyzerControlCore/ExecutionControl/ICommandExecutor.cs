using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using System;
using System.Collections.Generic;

namespace AnalyzerService.ExecutionControl
{
    public interface ICommandExecutor
    {
        event Action<int> CommandExecuted;

        /// <summary>
        /// Запускает выполнение списка команд и ожидает завершения их выполнения (блокирующая).
        /// </summary>
        /// <param name="commands"></param>
        void WaitExecution(List<ICommand> commands);
        void RunExecution(List<ICommand> commands);
        void AbortExecution();

        void UpdateExecutedCommandInfo(uint commandId, CommandStateResponse.CommandStates state);
    }
}
