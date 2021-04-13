using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.Responses;
using System;
using System.Collections.Generic;

namespace AnalyzerService.MachineControl
{
    public interface ICommandExecutor
    {
        event Action<int> CommandExecuted;

        void WaitExecution(List<ICommand> commands);
        void RunExecution(List<ICommand> commands);
        void AbortExecution();

        void UpdateExecutedCommandInfo(uint commandId, CommandStateResponse.CommandStates state);
    }
}
