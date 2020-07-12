using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Logger
    {
        public delegate void newMessageHandler(string message);
        public static event newMessageHandler NewInfoMessageAdded;
        public static event newMessageHandler NewDemoInfoMessageAdded;
        public static event newMessageHandler NewControllerInfoMessageAdded;

        private static object locker = new object();

        //Directory.CreateDirectory("logs");
        //fileName = "logs/Log_" + DateTime.Now.ToString("dd_MM_yyyy_#_HH_mm_ss") + ".txt";

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
