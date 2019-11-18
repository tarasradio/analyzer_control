using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;
using SteppersControlCore.Interfaces;

namespace SteppersControlCore
{
    public class Logger
    {
        public delegate void newMessageHandler(string message);
        public static event newMessageHandler OnNewMessageAdded;
        public static event newMessageHandler NewInfoMessageAdded;
        public static event newMessageHandler NewDemoInfoMessageAdded;
        public static event newMessageHandler NewControllerInfoMessageAdded;

        private static object locker = new object();

        public Logger()
        {
            //Directory.CreateDirectory("logs");
            //fileName = "logs/Log_" + DateTime.Now.ToString("dd_MM_yyyy_#_HH_mm_ss") + ".txt";
        }
        
        private static string wrapMessage(string message)
        {
            
            string text = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()} Info: {message} \n";
            return text;
        }

        public static void AddMessage(string text)
        {
            lock(locker)
            {
                //StreamWriter writer = new StreamWriter(fileName, true);

                string line = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") +
                    " - " + text + "\n";

                //writer.WriteLine(line);
                //writer.Close();
                OnNewMessageAdded?.Invoke(line);
            }
        }

        public static void Info(string message)
        {
            lock (locker)
            {
                NewInfoMessageAdded?.Invoke(wrapMessage(message));
            }
        }

        public static void DemoInfo(string message)
        {
            lock(locker)
            {
                NewDemoInfoMessageAdded?.Invoke(wrapMessage(message));
            }
        }

        public static void ControllerInfo(string message)
        {
            lock(locker)
            {
                NewControllerInfoMessageAdded?.Invoke(wrapMessage(message));
            }
        }
    }
}
