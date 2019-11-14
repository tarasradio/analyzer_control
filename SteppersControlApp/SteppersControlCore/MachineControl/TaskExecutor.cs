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
        private static object locker = new object();
        private Thread _executionThread;

        public TaskExecutor()
        {

        }

        public void AbortExecution()
        {
            lock(locker)
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
            AbortExecution();

            _executionThread = new Thread(new ThreadStart(task))
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };

            _executionThread.Start();
        }
    }
}
