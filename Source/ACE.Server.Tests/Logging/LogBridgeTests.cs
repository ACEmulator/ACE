using System;
using System.Linq;

using log4net.Appender;
using log4net.Config;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Common.Logging;
using ACE.Server.Logging;

namespace ACE.Server.Tests.Logging
{
    /// <summary>
    /// Proves the log4net-shaped shim over ILogger, routed back into log4net by the bridge, behaves like
    /// the ILog calls it replaces: literal text, correct levels, exception idiom, format tolerance, and
    /// level checks that follow configuration. Each test uses an isolated log4net repository.
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
        public void Message_is_rendered_literally_with_braces_and_quotes_intact()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger(typeof(LogBridgeTests));

            log.Info("hello {world} {0} \"quoted\" }{");

            var e = appender.GetEvents().Single();
            Assert.AreEqual("hello {world} {0} \"quoted\" }{", e.RenderedMessage);
            Assert.AreEqual(log4net.Core.Level.Info, e.Level);
            Assert.AreEqual(typeof(LogBridgeTests).FullName, e.LoggerName, "category must match log4net's logger naming so config entries still apply");
        }

        [TestMethod]
        public void Every_level_maps_to_its_log4net_counterpart()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger("levels");

            log.Debug("d"); log.Info("i"); log.Warn("w"); log.Error("e"); log.Fatal("f");

            var levels = appender.GetEvents().Select(e => e.Level).ToArray();
            CollectionAssert.AreEqual(new[] { log4net.Core.Level.Debug, log4net.Core.Level.Info, log4net.Core.Level.Warn, log4net.Core.Level.Error, log4net.Core.Level.Fatal }, levels);
        }

        [TestMethod]
        public void Exception_passed_as_the_message_is_carried_as_the_exception()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger("ex");
            var ex = new InvalidOperationException("boom");

            log.Error(ex);                 // log4net idiom: the exception IS the message
            log.Warn("context", ex);       // explicit (message, exception) overload

            var events = appender.GetEvents();
            Assert.AreSame(ex, events[0].ExceptionObject); Assert.AreEqual("boom", events[0].RenderedMessage);
            Assert.AreSame(ex, events[1].ExceptionObject); Assert.AreEqual("context", events[1].RenderedMessage);
        }

        [TestMethod]
        public void Format_calls_are_deferred_when_disabled_and_never_throw_on_a_bad_format()
        {
            var (factory, appender, _) = Setup(log4net.Core.Level.Info);
            var log = factory.CreateLogger("fmt");

            log.DebugFormat("{0}", new ThrowsOnToString());   // disabled level: argument must never be formatted
            log.InfoFormat("{0} and {1}", "only-one-arg");    // bad format: must not throw

            var e = appender.GetEvents().Single();
            Assert.AreEqual("{0} and {1}", e.RenderedMessage, "a bad format string surfaces raw rather than crashing");
        }

        [TestMethod]
        public void IsDebugEnabled_follows_the_configured_level()
        {
            var (factory, _, repo) = Setup(log4net.Core.Level.All);
            var log = factory.CreateLogger("enabled");
            Assert.IsTrue(log.IsDebugEnabled);

            repo.Root.Level = log4net.Core.Level.Info;
            repo.RaiseConfigurationChanged(EventArgs.Empty);
            Assert.IsFalse(log.IsDebugEnabled);
            Assert.IsTrue(log.IsInfoEnabled);
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
            early.Info("dropped - no provider yet");
            Assert.AreEqual(0, appender.GetEvents().Length);

            factory.AddProvider(new Log4NetBridgeProvider(repo));
            early.Info("delivered");
            Assert.AreEqual("delivered", appender.GetEvents().Single().RenderedMessage);
        }

        private sealed class ThrowsOnToString
        {
            public override string ToString() => throw new InvalidOperationException("formatted while disabled");
        }
    }
}
