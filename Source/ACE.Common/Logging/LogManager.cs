using System;

using Microsoft.Extensions.Logging;

namespace ACE.Common.Logging
{
    /// <summary>
    /// Process-wide logger factory for an application that does not use dependency injection.<para />
    /// Loggers may be requested at any time - including from static field initializers that run before
    /// Program.Main - because LoggerFactory.AddProvider wires a new provider into every logger the factory
    /// has already handed out. Ordering therefore does not matter.<para />
    /// Messages logged before a provider has been added are dropped, exactly as log4net dropped them before
    /// XmlConfigurator ran.
    /// </summary>
    public static class LogManager
    {
        public static ILoggerFactory Factory { get; } = new LoggerFactory();

        /// <summary>
        /// Category name is the type's full name, matching log4net's logger naming.
        /// </summary>
        public static ILogger GetLogger(Type type) => Factory.CreateLogger(type);

        public static ILogger GetLogger(string name) => Factory.CreateLogger(name);
    }
}
