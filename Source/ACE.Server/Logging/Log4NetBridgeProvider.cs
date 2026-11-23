using System;

using log4net.Repository;

using Microsoft.Extensions.Logging;

namespace ACE.Server.Logging
{
    /// <summary>
    /// TEMPORARY BRIDGE. Routes Microsoft.Extensions.Logging into log4net so that log4net.config keeps
    /// governing output (levels, appenders, the Packets override) while every call site moves to ILogger.<para />
    /// Logger names are the MEL category - the type's full name - so existing &lt;logger name="..."&gt; config
    /// entries still match. Delete this file when Serilog replaces log4net.
    /// </summary>
    public sealed class Log4NetBridgeProvider : ILoggerProvider
    {
        private readonly ILoggerRepository repository;

        public Log4NetBridgeProvider(ILoggerRepository repository)
        {
            this.repository = repository;
        }

        public ILogger CreateLogger(string categoryName)
        {
            // Same wrapping log4net.LogManager.GetLogger performs internally.
            return new Log4NetBridgeLogger(new log4net.Core.LogImpl(repository.GetLogger(categoryName)));
        }

        public void Dispose()
        {
        }

        private sealed class Log4NetBridgeLogger : ILogger
        {
            private readonly log4net.ILog log;

            public Log4NetBridgeLogger(log4net.ILog log)
            {
                this.log = log;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                switch (logLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:       return log.IsDebugEnabled;
                    case LogLevel.Information: return log.IsInfoEnabled;
                    case LogLevel.Warning:     return log.IsWarnEnabled;
                    case LogLevel.Error:       return log.IsErrorEnabled;
                    case LogLevel.Critical:    return log.IsFatalEnabled;
                    default:                   return false;
                }
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                    return;

                var message = formatter(state, exception);

                switch (logLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:       log.Debug(message, exception); break;
                    case LogLevel.Information: log.Info (message, exception); break;
                    case LogLevel.Warning:     log.Warn (message, exception); break;
                    case LogLevel.Error:       log.Error(message, exception); break;
                    case LogLevel.Critical:    log.Fatal(message, exception); break;
                }
            }

            public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new NullScope();

            public void Dispose()
            {
            }
        }
    }
}
