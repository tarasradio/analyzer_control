using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.Interfaces
{
    public interface ICommandExecutor
    {
        void WaitExecution(List<ICommand> commands);
        void RunExecution(List<ICommand> commands);
        void AbortExecution();
    }
}
