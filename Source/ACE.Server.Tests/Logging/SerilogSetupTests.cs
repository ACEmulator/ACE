using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Events;

using ACE.Server.Logging;

namespace ACE.Server.Tests.Logging
{
    [TestClass]
    public class SerilogSetupTests
    {
        private static readonly string TestData = Path.Combine(AppContext.BaseDirectory, "Logging", "TestData");

        private string root, work, exe, container;

        [TestInitialize]
        public void Initialize()
        {
            root = Path.Combine(Path.GetTempPath(), "ace-serilog-setup-" + Guid.NewGuid().ToString("N"));
            work = Directory.CreateDirectory(Path.Combine(root, "work")).FullName;
            exe = Directory.CreateDirectory(Path.Combine(root, "exe")).FullName;
            container = Directory.CreateDirectory(Path.Combine(root, "container")).FullName;

            File.Copy(Path.Combine(TestData, "serilog.json.example"), Path.Combine(exe, "serilog.json.example"));
            File.Copy(Path.Combine(TestData, "serilog.json.docker"), Path.Combine(exe, "serilog.json.docker"));
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(root, true);
        }

        private const string CustomLog4Net = @"<log4net>
            <appender name=""F"" type=""log4net.Appender.FileAppender""><file value=""custom.txt"" /></appender>
            <root><level value=""WARN"" /><appender-ref ref=""F"" /></root>
        </log4net>";

        [TestMethod]
        public void An_existing_log4net_config_is_converted_when_there_is_no_serilog_json()
        {
            File.WriteAllText(Path.Combine(exe, "log4net.config"), CustomLog4Net);
            var notes = new List<SerilogSetup.StartupNote>();

            var file = SerilogSetup.ResolveConfigFile(work, exe, false, container, notes);

            Assert.AreEqual(Path.Combine(exe, "serilog.json"), file);
            StringAssert.Contains(File.ReadAllText(file), "custom.txt");
            StringAssert.Contains(File.ReadAllText(file), "\"Default\": \"Warning\"");
            Assert.IsTrue(notes.Any(n => n.Level == LogLevel.Warning && n.Message.Contains("converted")));
            Assert.IsTrue(File.Exists(Path.Combine(exe, "log4net.config")), "the log4net file is left in place");
        }

        [TestMethod]
        public void A_log4net_config_in_the_working_directory_is_converted_too()
        {
            File.WriteAllText(Path.Combine(work, "log4net.config"), CustomLog4Net);

            var file = SerilogSetup.ResolveConfigFile(work, exe, false, container, new List<SerilogSetup.StartupNote>());

            StringAssert.Contains(File.ReadAllText(file), "custom.txt");
        }

        [TestMethod]
        public void An_existing_serilog_json_wins_over_log4net_config()
        {
            File.WriteAllText(Path.Combine(exe, "log4net.config"), CustomLog4Net);
            File.WriteAllText(Path.Combine(exe, "serilog.json"), "{ \"mine\": true }");
            var notes = new List<SerilogSetup.StartupNote>();

            var file = SerilogSetup.ResolveConfigFile(work, exe, false, container, notes);

            Assert.AreEqual("{ \"mine\": true }", File.ReadAllText(file));
            Assert.AreEqual(0, notes.Count);
        }

        [TestMethod]
        public void A_serilog_json_in_the_working_directory_is_used_first()
        {
            File.WriteAllText(Path.Combine(work, "serilog.json"), "{ }");
            File.WriteAllText(Path.Combine(exe, "serilog.json"), "{ }");

            var file = SerilogSetup.ResolveConfigFile(work, exe, false, container, new List<SerilogSetup.StartupNote>());

            Assert.AreEqual(Path.Combine(work, "serilog.json"), file);
        }

        [TestMethod]
        public void Without_either_file_the_example_is_copied()
        {
            var notes = new List<SerilogSetup.StartupNote>();

            var file = SerilogSetup.ResolveConfigFile(work, exe, false, container, notes);

            Assert.AreEqual(File.ReadAllText(Path.Combine(exe, "serilog.json.example")), File.ReadAllText(file));
            Assert.AreEqual(0, notes.Count);
        }

        [TestMethod]
        public void An_unreadable_log4net_config_falls_back_to_the_example_and_says_so()
        {
            File.WriteAllText(Path.Combine(exe, "log4net.config"), "<log4net><root>");
            var notes = new List<SerilogSetup.StartupNote>();

            var file = SerilogSetup.ResolveConfigFile(work, exe, false, container, notes);

            Assert.AreEqual(File.ReadAllText(Path.Combine(exe, "serilog.json.example")), File.ReadAllText(file));
            Assert.IsTrue(notes.Any(n => n.Level == LogLevel.Error));
        }

        [TestMethod]
        public void In_a_container_a_log4net_config_in_the_config_volume_is_converted_and_kept_there()
        {
            File.WriteAllText(Path.Combine(container, "log4net.config"), CustomLog4Net);

            var file = SerilogSetup.ResolveConfigFile(work, exe, true, container, new List<SerilogSetup.StartupNote>());

            Assert.AreEqual(Path.Combine(exe, "serilog.json"), file);
            StringAssert.Contains(File.ReadAllText(Path.Combine(container, "serilog.json")), "custom.txt");
        }

        [TestMethod]
        public void In_a_container_without_either_file_the_docker_file_is_copied_to_both_places()
        {
            var file = SerilogSetup.ResolveConfigFile(work, exe, true, container, new List<SerilogSetup.StartupNote>());

            var docker = File.ReadAllText(Path.Combine(exe, "serilog.json.docker"));
            Assert.AreEqual(docker, File.ReadAllText(file));
            Assert.AreEqual(docker, File.ReadAllText(Path.Combine(container, "serilog.json")));
        }

        [TestMethod]
        public void In_a_container_the_config_volume_copy_wins()
        {
            File.WriteAllText(Path.Combine(container, "serilog.json"), "{ \"volume\": true }");
            File.WriteAllText(Path.Combine(exe, "serilog.json"), "{ \"stale\": true }");

            var file = SerilogSetup.ResolveConfigFile(work, exe, true, container, new List<SerilogSetup.StartupNote>());

            Assert.AreEqual("{ \"volume\": true }", File.ReadAllText(file));
        }

        [TestMethod]
        public void Shipped_example_files_load()
        {
            foreach (var name in new[] { "serilog.json.example", "serilog.json.docker", "serilog.json.ConsoleExample", "serilog.json.FilesExample" })
            {
                var configuration = new ConfigurationBuilder().AddJsonFile(Path.Combine(TestData, name)).Build();
                using var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
                Assert.IsTrue(logger.IsEnabled(LogEventLevel.Verbose), name);
            }
        }

        [TestMethod]
        public void Level_changes_in_serilog_json_apply_without_a_restart()
        {
            var file = Path.Combine(exe, "serilog.json");
            File.WriteAllText(file, "{ \"Serilog\": { \"MinimumLevel\": { \"Default\": \"Information\", \"Override\": { \"Packets\": \"Warning\" } } } }");

            var configuration = new ConfigurationBuilder().AddJsonFile(file, optional: false, reloadOnChange: true).Build();
            using var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            var packets = logger.ForContext(Serilog.Core.Constants.SourceContextPropertyName, "Packets");

            Assert.IsFalse(logger.IsEnabled(LogEventLevel.Debug));
            Assert.IsFalse(packets.IsEnabled(LogEventLevel.Information));

            File.WriteAllText(file, "{ \"Serilog\": { \"MinimumLevel\": { \"Default\": \"Debug\", \"Override\": { \"Packets\": \"Information\" } } } }");

            var stopwatch = Stopwatch.StartNew();
            while (!(logger.IsEnabled(LogEventLevel.Debug) && packets.IsEnabled(LogEventLevel.Information)) && stopwatch.Elapsed < TimeSpan.FromSeconds(10))
                Thread.Sleep(50);

            Assert.IsTrue(logger.IsEnabled(LogEventLevel.Debug), "default level reloaded");
            Assert.IsTrue(packets.IsEnabled(LogEventLevel.Information), "override reloaded");
        }
    }
}
