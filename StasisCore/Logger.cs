using System;
using System.Text;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace StasisCore
{
    public class Logger
    {
        private static ILog _logger;

        public static void initialize()
        {
            _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// A helper method for logging (will make disabling logging easier)
        /// </summary>
        public static void log(string message)
        {
            _logger.Info(string.Format("--- {0}", message));
        }

        public static void log(string message, Exception exception)
        {
            _logger.Info(string.Format("--- {0}", message), exception);
        }
    }
}
