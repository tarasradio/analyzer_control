using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SteppersControlCore.MachineControl;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.Controllers;
using SteppersControlCore.Elements;

namespace SteppersControlCore.MachineControl
{
    public class TaskExecutor
    {
        public delegate void TaskDelegate();
        private static object _syncRoot = new object();
        private Thread _executionThread;

        public TaskExecutor()
        {

        }

        public void AbortExecution()
        {
            lock(_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
            }
        }

        public ThreadState GetState()
        {
            return _executionThread.ThreadState;
        }

        public void StartTask(TaskDelegate task)
        {
            lock (_syncRoot)
            {
                if (_executionThread != null && _executionThread.IsAlive)
                    _executionThread.Abort();
                _executionThread = new Thread(new ThreadStart(task));
                _executionThread.Priority = ThreadPriority.Lowest;
                _executionThread.IsBackground = true;
                _executionThread.Start();
            }
        }

        private void WaitExecution(List<IAbstractCommand> task)
        {
            Core.CncExecutor.ExecuteTask(task);
        }
    }
}
