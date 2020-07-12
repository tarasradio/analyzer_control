using System.Threading;

namespace AnalyzerControlCore.MachineControl
{
    public class TaskExecutor
    {
        public delegate void TaskDelegate();
        private static object locker = new object();
        private Thread executionThread;

        public TaskExecutor() { }

        public void AbortExecution()
        {
            lock(locker)
            {
                if (executionThread != null && executionThread.IsAlive)
                    executionThread.Abort();
            }
        }

        public ThreadState GetState()
        {
            return executionThread.ThreadState;
        }

        public void StartTask(TaskDelegate task)
        {
            AbortExecution();

            executionThread = new Thread(new ThreadStart(task))
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };

            executionThread.Start();
        }
    }
}
