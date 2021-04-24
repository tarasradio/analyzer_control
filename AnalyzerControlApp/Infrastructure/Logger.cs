using System;

namespace Infrastructure
{
    public static class Logger
    {
        public static event Action<string> NewInfoMessageAdded;
        public static event Action<string> NewDemoInfoMessageAdded;
        public static event Action<string> NewControllerInfoMessageAdded;

        private static object locker = new object();

        private static string WrapMessage(string message)
        {
            string text = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Info: {message} \n";
            return text;
        }

        public static void Info(string message)
        {
            lock (locker)
            {
                NewInfoMessageAdded?.Invoke(WrapMessage(message));
            }
        }

        public static void DemoInfo(string message)
        {
            lock (locker)
            {
                NewDemoInfoMessageAdded?.Invoke(WrapMessage(message));
            }
        }

        public static void ControllerInfo(string message)
        {
            lock (locker)
            {
                NewControllerInfoMessageAdded?.Invoke(WrapMessage(message));
            }
        }
    }
}
