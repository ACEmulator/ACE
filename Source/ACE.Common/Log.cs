using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ACE.Common
{
    public static class Log
    {
        public static void Debug(ILog log, string message)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        public static void Debug(ILog log, string message, params object[] args)
        {
            if(log.IsDebugEnabled)
            {
                log.DebugFormat(message, args);
            }
        }

        public static void Debug(ILog log, string message, Exception exception)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug(message, exception);
            }
        }

        public static void Info(ILog log, string message)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        public static void Info(ILog log, string message, params object[] args)
        {
            if (log.IsInfoEnabled)
            {
                log.InfoFormat(message, args);
            }
        }
        public static void Info(ILog log, string message, Exception exception)
        {
            if (log.IsInfoEnabled)
            {
                log.Info(message, exception);
            }
        }


        public static void Warn(ILog log, string message, params object[] args)
        {
            if (log.IsWarnEnabled)
            {
                log.WarnFormat(message, args);
            }
        }

        public static void Warn(ILog log, string message)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message);
            }
        }

        public static void Warn(ILog log, string message, Exception exception)
        {
            if (log.IsWarnEnabled)
            {
                log.Warn(message, exception);
            }
        }

        public static void Error(ILog log, string message, params object[] args)
        {
            if (log.IsErrorEnabled)
            {
                log.ErrorFormat(message, args);
            }
        }

        public static void Error(ILog log, string message)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message);
            }
        }

        public static void Error(ILog log, string message, Exception exception)
        {
            if (log.IsErrorEnabled)
            {
                log.Error(message, exception);
            }
        }

        public static void Fatal(ILog log, string message, params object[] args)
        {
            if (log.IsFatalEnabled)
            {
                log.FatalFormat(message, args);
            }
        }

        public static void Fatal(ILog log, string message)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message);
            }
        }

        public static void Fatal(ILog log, string message, Exception exception)
        {
            if (log.IsFatalEnabled)
            {
                log.Fatal(message, exception);
            }
        }
    }
}
