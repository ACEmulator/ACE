using System;
using System.Globalization;

using Microsoft.Extensions.Logging;

namespace ACE.Common.Logging
{
    /// <summary>
    /// A log4net-shaped surface over Microsoft.Extensions.Logging.ILogger, so existing call sites compile
    /// unchanged while the underlying logger is Microsoft's abstraction.<para />
    /// Pre-formatted text is always passed as the {Message} property, never as the message template: a message
    /// that happens to contain braces (packet dumps, JSON, exception text) would otherwise be parsed as a
    /// template and throw at format time. The ':l' format renders it literally (no quotes) under Serilog.<para />
    /// Every method checks IsEnabled before doing anything else, so a disabled level costs one check and no
    /// allocation. The LoggerMessage.Define delegates are compiled once; nothing is parsed per call.
    /// </summary>
    public static class LogExtensions
    {
        private static readonly Action<ILogger, string, Exception> WriteDebug = LoggerMessage.Define<string>(LogLevel.Debug,       default, "{Message:l}");
        private static readonly Action<ILogger, string, Exception> WriteInfo  = LoggerMessage.Define<string>(LogLevel.Information, default, "{Message:l}");
        private static readonly Action<ILogger, string, Exception> WriteWarn  = LoggerMessage.Define<string>(LogLevel.Warning,     default, "{Message:l}");
        private static readonly Action<ILogger, string, Exception> WriteError = LoggerMessage.Define<string>(LogLevel.Error,       default, "{Message:l}");
        private static readonly Action<ILogger, string, Exception> WriteFatal = LoggerMessage.Define<string>(LogLevel.Critical,    default, "{Message:l}");

        extension(ILogger log)
        {
            public bool IsDebugEnabled => log.IsEnabled(LogLevel.Debug);
            public bool IsInfoEnabled  => log.IsEnabled(LogLevel.Information);
            public bool IsWarnEnabled  => log.IsEnabled(LogLevel.Warning);
            public bool IsErrorEnabled => log.IsEnabled(LogLevel.Error);
            public bool IsFatalEnabled => log.IsEnabled(LogLevel.Critical);
        }

        // object, not string: log4net's ILog takes object, and call sites pass exceptions (log.Error(ex)) and other non-strings.
        public static void Debug(this ILogger log, object message)                      => Write(log, LogLevel.Debug,       WriteDebug, message, null);
        public static void Debug(this ILogger log, object message, Exception exception) => Write(log, LogLevel.Debug,       WriteDebug, message, exception);
        public static void Info (this ILogger log, object message)                      => Write(log, LogLevel.Information, WriteInfo,  message, null);
        public static void Info (this ILogger log, object message, Exception exception) => Write(log, LogLevel.Information, WriteInfo,  message, exception);
        public static void Warn (this ILogger log, object message)                      => Write(log, LogLevel.Warning,     WriteWarn,  message, null);
        public static void Warn (this ILogger log, object message, Exception exception) => Write(log, LogLevel.Warning,     WriteWarn,  message, exception);
        public static void Error(this ILogger log, object message)                      => Write(log, LogLevel.Error,       WriteError, message, null);
        public static void Error(this ILogger log, object message, Exception exception) => Write(log, LogLevel.Error,       WriteError, message, exception);
        public static void Fatal(this ILogger log, object message)                      => Write(log, LogLevel.Critical,    WriteFatal, message, null);
        public static void Fatal(this ILogger log, object message, Exception exception) => Write(log, LogLevel.Critical,    WriteFatal, message, exception);

        public static void DebugFormat(this ILogger log, string format, params object[] args) => WriteFormat(log, LogLevel.Debug,       WriteDebug, format, args);
        public static void InfoFormat (this ILogger log, string format, params object[] args) => WriteFormat(log, LogLevel.Information, WriteInfo,  format, args);
        public static void WarnFormat (this ILogger log, string format, params object[] args) => WriteFormat(log, LogLevel.Warning,     WriteWarn,  format, args);
        public static void ErrorFormat(this ILogger log, string format, params object[] args) => WriteFormat(log, LogLevel.Error,       WriteError, format, args);
        public static void FatalFormat(this ILogger log, string format, params object[] args) => WriteFormat(log, LogLevel.Critical,    WriteFatal, format, args);

        private static void Write(ILogger log, LogLevel level, Action<ILogger, string, Exception> write, object message, Exception exception)
        {
            if (!log.IsEnabled(level))
                return;

            // log4net idiom: log.Error(ex) - the exception IS the message. Carry it as the structured exception
            // so sinks get the stack trace, with its text as the message.
            if (exception == null && message is Exception ex)
            {
                write(log, ex.Message, ex);
                return;
            }

            write(log, message?.ToString(), exception);
        }

        private static void WriteFormat(ILogger log, LogLevel level, Action<ILogger, string, Exception> write, string format, object[] args)
        {
            if (!log.IsEnabled(level))
                return;

            string text;
            try
            {
                text = string.Format(CultureInfo.InvariantCulture, format, args);
            }
            catch (FormatException)
            {
                // log4net never throws from *Format; a bad format string must not take the server down.
                text = format;
            }

            write(log, text, null);
        }
    }
}
