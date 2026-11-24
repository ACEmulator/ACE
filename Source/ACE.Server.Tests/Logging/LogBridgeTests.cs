using System;
using System.Linq;

using log4net.Appender;
using log4net.Config;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Server.Logging;

namespace ACE.Server.Tests.Logging
{
    /// <summary>
    /// Proves standard ILogger calls, routed into log4net by the bridge, render the same text the log4net
    /// calls they replaced did: template values, escaped and literal braces, correct levels, exceptions,
    /// no formatting at a disabled level, and level checks that follow configuration. Each test uses an
    /// isolated log4net repository.
    /// </summary>
    [TestClass]
    public class LogBridgeTests
    {
        private static (ILoggerFactory factory, MemoryAppender appender, log4net.Repository.Hierarchy.Hierarchy repo) Setup(log4net.Core.Level threshold)
        {
            var repo = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());
            var appender = new MemoryAppender();
            BasicConfigurator.Configure(repo, appender);
            repo.Root.Level = threshold;
            repo.RaiseConfigurationChanged(EventArgs.Empty);

            var factory = new LoggerFactory();
            factory.AddProvider(new Log4NetBridgeProvider(repo));
            return (factory, appender, repo);
        }

        [TestMethod]
        public void Template_values_are_rendered_and_escaped_braces_survive()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger(typeof(LogBridgeTests));

            log.LogInformation("hello {World} {{literal}} \"quoted\" 0x{Guid:X8}", "earth", 0xABCu);

            var e = appender.GetEvents().Single();
            Assert.AreEqual("hello earth {literal} \"quoted\" 0x00000ABC", e.RenderedMessage);
            Assert.AreEqual(log4net.Core.Level.Info, e.Level);
            Assert.AreEqual(typeof(LogBridgeTests).FullName, e.LoggerName, "category must match log4net's logger naming so config entries still apply");
        }

        [TestMethod]
        public void Prebuilt_text_passed_as_a_value_is_rendered_literally()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger("literal");

            log.LogInformation("{Message:l}", "hello {world} {0} }{");

            Assert.AreEqual("hello {world} {0} }{", appender.GetEvents().Single().RenderedMessage);
        }

        [TestMethod]
        public void Every_level_maps_to_its_log4net_counterpart()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger("levels");

            log.LogDebug("d"); log.LogInformation("i"); log.LogWarning("w"); log.LogError("e"); log.LogCritical("f");

            var levels = appender.GetEvents().Select(e => e.Level).ToArray();
            CollectionAssert.AreEqual(new[] { log4net.Core.Level.Debug, log4net.Core.Level.Info, log4net.Core.Level.Warn, log4net.Core.Level.Error, log4net.Core.Level.Fatal }, levels);
        }

        [TestMethod]
        public void Exception_is_carried_as_the_exception()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger("ex");
            var ex = new InvalidOperationException("boom");

            log.LogError(ex, "{Message:l}", ex.Message);
            log.LogWarning(ex, "context");

            var events = appender.GetEvents();
            Assert.AreSame(ex, events[0].ExceptionObject); Assert.AreEqual("boom", events[0].RenderedMessage);
            Assert.AreSame(ex, events[1].ExceptionObject); Assert.AreEqual("context", events[1].RenderedMessage);
        }

        [TestMethod]
        public void Values_are_not_formatted_when_the_level_is_disabled()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.Info);
            var log = factory.CreateLogger("disabled");

            log.LogDebug("{Value}", new ThrowsOnToString());

            Assert.AreEqual(0, appender.GetEvents().Length);
        }

        [TestMethod]
        public void IsEnabled_follows_the_configured_level()
        {
            var (factory, _, repo) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger("enabled");
            Assert.IsTrue(log.IsEnabled(LogLevel.Debug));

            repo.Root.Level = log4net.Core.Level.Info;
            repo.RaiseConfigurationChanged(EventArgs.Empty);
            Assert.IsFalse(log.IsEnabled(LogLevel.Debug));
            Assert.IsTrue(log.IsEnabled(LogLevel.Information));
        }

        [TestMethod]
        public void Loggers_created_before_the_provider_is_added_still_receive_it()
        {
            // This is what makes the static-field pattern safe without DI: static initialisers run before
            // Main wires providers, and LoggerFactory.AddProvider updates loggers already handed out.
            var repo = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.CreateRepository(Guid.NewGuid().ToString());
            var appender = new MemoryAppender();
            BasicConfigurator.Configure(repo, appender);

            var factory = new LoggerFactory();
            var early = factory.CreateLogger("early");
            early.LogInformation("dropped - no provider yet");
            Assert.AreEqual(0, appender.GetEvents().Length);

            factory.AddProvider(new Log4NetBridgeProvider(repo));
            early.LogInformation("delivered");
            Assert.AreEqual("delivered", appender.GetEvents().Single().RenderedMessage);
        }

        private sealed class ThrowsOnToString
        {
            public override string ToString() => throw new InvalidOperationException("formatted while disabled");
        }
    }
}
