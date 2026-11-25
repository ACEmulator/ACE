using System;
using System.Globalization;
using System.Threading;

using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace ACE.Server.Logging
{
    /// <summary>
    /// Adds the Thread property: the name of the logging thread, or its managed id when it has no name.<para />
    /// This is what log4net's %thread wrote, e.g. [World Manager] or [1].
    /// </summary>
    public sealed class ThreadEnricher : ILogEventEnricher
    {
        public const string PropertyName = "Thread";

        [ThreadStatic]
        private static string cachedName;

        [ThreadStatic]
        private static LogEventProperty cachedProperty;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var name = Thread.CurrentThread.Name;
            var property = cachedProperty;

            // a thread's name can be set once, after it has already logged
            if (property == null || !ReferenceEquals(name, cachedName))
            {
                var value = string.IsNullOrEmpty(name) ? Environment.CurrentManagedThreadId.ToString(CultureInfo.InvariantCulture) : name;

                cachedName = name;
                cachedProperty = property = new LogEventProperty(PropertyName, new ScalarValue(value));
            }

            logEvent.AddPropertyIfAbsent(property);
        }
    }

    public static class ThreadEnricherExtensions
    {
        /// <summary>
        /// "Enrich": [ "WithThread" ] in serilog.json, with "ACE.Server" listed under "Using"
        /// </summary>
        public static LoggerConfiguration WithThread(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            return enrichmentConfiguration.With<ThreadEnricher>();
        }
    }
}
