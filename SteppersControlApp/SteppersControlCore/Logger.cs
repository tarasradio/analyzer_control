using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;

namespace SteppersControlCore
{
    public class Logger
    {
        public delegate void newMessageHandler(string message);
        public static event newMessageHandler OnNewMessageAdded;

        public Logger()
        {
            //Directory.CreateDirectory("logs");
            //fileName = "logs/Log_" + DateTime.Now.ToString("dd_MM_yyyy_#_HH_mm_ss") + ".txt";
        }

        static Mutex mutex = new Mutex();

        public static void AddMessage(string text)
        {
            mutex.WaitOne();

            //StreamWriter writer = new StreamWriter(fileName, true);

            string line = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") +
                " - " + text + "\n";

            //writer.WriteLine(line);
            //writer.Close();

            mutex.ReleaseMutex();

            OnNewMessageAdded?.Invoke(line);
        }
    }
}
