using System;

namespace Infrastructure
{
    public static class Logger
    {
        public static event Action<string> InfoMessageAdded;
        public static event Action<string> DebugMessageAdded;

        private static object locker = new object();

        private static string wrapMessage(string message)
        {
            return $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} : {message} \n";
        }

        public static void Debug(string message)
        {
            lock (locker) {
                DebugMessageAdded?.Invoke(wrapMessage(message));
            }
        }

        public static void Info(string message)
        {
            lock (locker) {
                InfoMessageAdded?.Invoke(message);
            }
        }
    }
}
