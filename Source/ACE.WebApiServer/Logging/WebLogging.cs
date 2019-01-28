using Microsoft.Extensions.Logging;

namespace ACE.WebApiServer.Logging
{
    /// <summary>
    /// https://stackify.com/net-core-loggerfactory-use-correctly/
    /// license: ???
    /// </summary>
    public class WebLogging
    {
        private static ILoggerFactory _Factory = null;

        public static void ConfigureLogger(ILoggerFactory factory)
        {
            factory.AddDebug().AddLog4Net();
        }
        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                    ConfigureLogger(_Factory);
                }
                return _Factory;
            }
            set => _Factory = value;
        }
        public static ILogger CreateLogger(string name)
        {
            return LoggerFactory.CreateLogger(name);
        }
    }
}
