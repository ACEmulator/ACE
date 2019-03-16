using ACE.Common;
using log4net;
using System;
using System.Reflection;

namespace ACE.Server.Plugin
{
    /// <summary>
    /// template for a configuration manager that logs to log4net.
    /// For plugins that need a persistent file based configuration.
    /// To keep from adding log4net dependency to ACE.Common.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Log4netEnabledPluginConfigManager<T>
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static T Config => PluginConfigManager<T>.Config;
        public static void Initialize()
        {
            PluginConfigManager<T>.Initialize(new PluginConfigManager<T>.LogCallbackDelegate(LogHandler));
        }
        private static void LogHandler(string msg, PluginConfigManager<T>.LogLevel lev = PluginConfigManager<T>.LogLevel.Info, Exception ex = null)
        {
            switch (lev)
            {
                case PluginConfigManager<T>.LogLevel.Info:
                    log.Info(msg, ex);
                    break;
                case PluginConfigManager<T>.LogLevel.Error:
                    log.Error(msg, ex);
                    break;
                case PluginConfigManager<T>.LogLevel.Fatal:
                    log.Fatal(msg, ex);
                    break;
                case PluginConfigManager<T>.LogLevel.Debug:
                    log.Debug(msg, ex);
                    break;
            }
        }
    }
}
