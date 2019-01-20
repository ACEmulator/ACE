using Microsoft.Extensions.Logging;

namespace ACE.WebApiServer.Logging
{
    /// <summary>
    /// https://dotnetthoughts.net/how-to-use-log4net-with-aspnetcore-for-logging/
    /// MIT license.
    /// </summary>
    internal static class Log4NetAspExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile)
        {
            factory.AddProvider(new Log4NetProvider(log4NetConfigFile));
            return factory;
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            factory.AddProvider(new Log4NetProvider("log4net.config"));
            return factory;
        }
    }
}
