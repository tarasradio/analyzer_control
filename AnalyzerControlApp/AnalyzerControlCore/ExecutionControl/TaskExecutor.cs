using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyzerService.ExecutionControl
{
    public class TaskExecutor
    {
        private static object locker = new object();

        private Thread executionThread;

        public TaskExecutor() 
        { 

        }
        
        public void StartTask(Action task)
        {
            AbortExecution();

            executionThread = new Thread(new ThreadStart(task))
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };

            executionThread.Start();
        }
        
        public ThreadState GetState()
        {
            return executionThread.ThreadState;
        }

        public void AbortExecution()
        {
            lock(locker)
            {
                if (executionThread != null && executionThread.IsAlive)
                    executionThread.Abort();
            }
        }
    }
}
