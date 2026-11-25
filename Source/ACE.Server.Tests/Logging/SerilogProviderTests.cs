using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Display;

namespace ACE.Server.Tests.Logging
{
    /// <summary>
    /// ACE logs through Microsoft.Extensions.Logging; these check what reaches Serilog through the provider
    /// </summary>
    [TestClass]
    public class SerilogProviderTests
    {
        private sealed class CollectingSink : ILogEventSink
        {
            public readonly List<LogEvent> Events = new List<LogEvent>();

            public void Emit(LogEvent logEvent) => Events.Add(logEvent);
        }

        private static (ILoggerFactory factory, CollectingSink sink) Setup(Action<LoggerConfiguration> configure = null)
        {
            var sink = new CollectingSink();
            var configuration = new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Sink(sink);
            configure?.Invoke(configuration);

            var factory = new LoggerFactory();
            factory.AddProvider(new SerilogLoggerProvider(configuration.CreateLogger(), dispose: true));
            return (factory, sink);
        }

        private static string Render(LogEvent logEvent, string template = "{Message:lj}")
        {
            var writer = new StringWriter();
            new MessageTemplateTextFormatter(template).Format(logEvent, writer);
            return writer.ToString();
        }

        [TestMethod]
        public void Template_values_and_format_specifiers_are_rendered_and_escaped_braces_survive()
        {
            var (factory, sink) = Setup();

            factory.CreateLogger("Test").LogInformation("{Name} hit 0x{Guid:X8} for {Damage} {{not a hole}}", "Bob", 0x8000_1234u, 1234);

            Assert.AreEqual("Bob hit 0x80001234 for 1234 {not a hole}", Render(sink.Events.Single()));
        }

        [TestMethod]
        public void Prebuilt_text_passed_as_a_value_is_rendered_literally()
        {
            var (factory, sink) = Setup();

            factory.CreateLogger("Test").LogInformation("{Message:l}", "text with {braces} and \"quotes\"");

            Assert.AreEqual("text with {braces} and \"quotes\"", Render(sink.Events.Single()));
        }

        [TestMethod]
        public void Every_level_maps_to_its_Serilog_counterpart()
        {
            var (factory, sink) = Setup();
            var log = factory.CreateLogger("Test");

            log.LogTrace("t");
            log.LogDebug("d");
            log.LogInformation("i");
            log.LogWarning("w");
            log.LogError("e");
            log.LogCritical("c");

            CollectionAssert.AreEqual(
                new[] { LogEventLevel.Verbose, LogEventLevel.Debug, LogEventLevel.Information, LogEventLevel.Warning, LogEventLevel.Error, LogEventLevel.Fatal },
                sink.Events.Select(e => e.Level).ToArray());
        }

        [TestMethod]
        public void Exception_is_carried_as_the_exception()
        {
            var (factory, sink) = Setup();
            var ex = new InvalidOperationException("boom");

            factory.CreateLogger("Test").LogError(ex, "{Message:l}", ex.Message);

            Assert.AreSame(ex, sink.Events.Single().Exception);
            Assert.AreEqual("boom", Render(sink.Events.Single()));
        }

        private sealed class CountingToString
        {
            public int Count;

            public override string ToString()
            {
                Count++;
                return "value";
            }
        }

        [TestMethod]
        public void Values_are_not_formatted_when_the_level_is_disabled()
        {
            var (factory, sink) = Setup(c => c.MinimumLevel.Information());
            var log = factory.CreateLogger("Test");
            var value = new CountingToString();

            Assert.IsFalse(log.IsEnabled(LogLevel.Debug));
            log.LogDebug("{Value}", value);

            Assert.AreEqual(0, sink.Events.Count);
            Assert.AreEqual(0, value.Count);
        }

        [TestMethod]
        public void Category_becomes_SourceContext_and_overrides_apply_to_it()
        {
            var (factory, sink) = Setup(c => c.MinimumLevel.Override("Packets", LogEventLevel.Information));

            factory.CreateLogger("Packets").LogDebug("packet debug");
            factory.CreateLogger("Packets").LogInformation("packet info");
            factory.CreateLogger("ACE.Server.Managers.HouseManager").LogDebug("house debug");

            CollectionAssert.AreEqual(new[] { "packet info", "house debug" }, sink.Events.Select(e => Render(e)).ToArray());
            Assert.AreEqual("\"ACE.Server.Managers.HouseManager\"", sink.Events[1].Properties["SourceContext"].ToString());
        }

        [TestMethod]
        public void Loggers_created_before_the_provider_is_added_still_receive_it()
        {
            // ACE's loggers are static fields, created before Program.Main adds the Serilog provider
            var factory = new LoggerFactory();
            var early = factory.CreateLogger("Early");

            var sink = new CollectingSink();
            factory.AddProvider(new SerilogLoggerProvider(new LoggerConfiguration().WriteTo.Sink(sink).CreateLogger(), dispose: true));

            early.LogInformation("hello");

            Assert.AreEqual("hello", Render(sink.Events.Single()));
        }
    }
}
