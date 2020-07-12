using AnalyzerCommunication;
using System.Collections.Generic;

namespace AnalyzerControlCore.MachineControl
{
    public interface ICommandExecutor
    {
        void WaitExecution(List<ICommand> commands);
        void RunExecution(List<ICommand> commands);
        void AbortExecution();
    }
}
