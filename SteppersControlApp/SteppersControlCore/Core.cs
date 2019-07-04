using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SteppersControlCore
{
    public class Core
    {
        private static Logger _logger;
        public static Configuration _configuration;
        private static uint lastPacketId = 0;

        public static uint GetPacketId()
        {
            return lastPacketId++;
        }

        public Core()
        {
            _logger = new Logger();
            _configuration = new Configuration();

            _logger.AddMessage("Запись работы системы начата");
        }

        public Logger GetLogger()
        {
            return _logger;
        }

        public Configuration getConfig()
        {
            return _configuration;
        }
    }
}
