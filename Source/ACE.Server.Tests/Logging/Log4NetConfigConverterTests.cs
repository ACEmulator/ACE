using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Extensions.Logging;

using ACE.Server.Logging;

namespace ACE.Server.Tests.Logging
{
    [TestClass]
    public class Log4NetConfigConverterTests
    {
        private static readonly string TestData = Path.Combine(AppContext.BaseDirectory, "Logging", "TestData");

        private static readonly JsonDocumentOptions JsonOptions = new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true };

        private static string ReadTestData(string name) => File.ReadAllText(Path.Combine(TestData, name));

        // ---------------------------------------------------------------- patterns

        [TestMethod]
        public void File_pattern_converts_to_the_shipped_file_template()
        {
            var template = Log4NetConfigConverter.ConvertPattern("%date [%thread] %-5level (%logger) %message%newline", out var warnings);

            Assert.AreEqual("{Timestamp:yyyy-MM-dd HH:mm:ss,fff} [{Thread}] {Level:u3} ({SourceContext}) {Message:lj}{NewLine}{Exception}", template);
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void Console_pattern_converts_to_the_shipped_console_template()
        {
            var template = Log4NetConfigConverter.ConvertPattern("%date %-5level: %message%newline", out var warnings);

            Assert.AreEqual("{Timestamp:yyyy-MM-dd HH:mm:ss,fff} {Level:u3}: {Message:lj}{NewLine}{Exception}", template);
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void Pattern_literals_options_and_case_sensitive_names_convert()
        {
            var template = Log4NetConfigConverter.ConvertPattern("{x} 100%% %d{HH:mm} %p %P{user} %-20c %exception %n", out var warnings);

            Assert.AreEqual("{{x}} 100% {Timestamp:HH:mm} {Level:u3} {user} {SourceContext,-20} {Exception} {NewLine}", template);
            Assert.AreEqual(0, warnings.Count);
        }

        [TestMethod]
        public void Unsupported_pattern_names_are_dropped_with_a_warning()
        {
            var template = Log4NetConfigConverter.ConvertPattern("%message (%file:%line)%newline", out var warnings);

            Assert.AreEqual("{Message:lj} (:){NewLine}{Exception}", template);
            Assert.AreEqual(2, warnings.Count);
        }

        // ---------------------------------------------------------------- shipped configs

        [TestMethod]
        public void Converting_the_old_default_config_gives_the_new_default_config()
        {
            var result = Log4NetConfigConverter.Convert(ReadTestData("log4net.config.example"));

            Assert.AreEqual(Normalize(ReadTestData("serilog.json.example")), Normalize(result.Json));
            Assert.AreEqual(1, result.Warnings.Count, string.Join(Environment.NewLine, result.Warnings));
            StringAssert.Contains(result.Warnings[0], "colours");
        }

        [TestMethod]
        public void Converting_the_old_docker_config_gives_the_new_docker_config()
        {
            var result = Log4NetConfigConverter.Convert(ReadTestData("log4net.config.docker"));

            Assert.AreEqual(Normalize(ReadTestData("serilog.json.docker")), Normalize(result.Json));
        }

        private static readonly Action<ILoggerFactory> Scenario = factory =>
        {
            factory.CreateLogger("Packets").LogDebug("packet debug");
            factory.CreateLogger("Packets").LogInformation("packet info");
            factory.CreateLogger("ACE.Server.WorldObjects.Corpse").LogDebug("[CORPSE] corpse made");
            factory.CreateLogger("ACE.Server.Factories.LootGenerationFactory").LogDebug("[LOOT] loot made");
            factory.CreateLogger("ACE.Database.ShardDatabase").LogDebug("[DATABASE] saved");
            factory.CreateLogger("ACE.Server.Managers.HouseManager").LogDebug("house debug");
            factory.CreateLogger("Test").LogDebug("plain debug");
            factory.CreateLogger("Test").LogInformation("info line");
            factory.CreateLogger("Test").LogWarning("warn line");
            factory.CreateLogger("Test").LogError("error line");
        };

        [TestMethod]
        public void Old_and_new_default_configs_route_messages_the_same_way()
        {
            var converted = Run(Log4NetConfigConverter.Convert(ReadTestData("log4net.config.example")).Json, Scenario);
            var shipped = Run(ReadTestData("serilog.json.example"), Scenario);

            var expected = new Dictionary<string, string[]>
            {
                ["console.txt"] = new[] { "INF|packet info", "INF|info line", "WRN|warn line", "ERR|error line" },
                ["ACE_Log.txt"] = new[] { "INF|packet info", "DBG|[CORPSE] corpse made", "DBG|[LOOT] loot made", "DBG|[DATABASE] saved", "DBG|house debug", "DBG|plain debug", "INF|info line", "WRN|warn line", "ERR|error line" },
            };

            AssertFiles(expected, converted);
            AssertFiles(expected, shipped);
        }

        [TestMethod]
        public void Old_and_new_console_examples_route_messages_the_same_way()
        {
            var result = Log4NetConfigConverter.Convert(ReadTestData("log4net.config.ConsoleExample"));
            var converted = Run(result.Json, Scenario);
            var shipped = Run(ReadTestData("serilog.json.ConsoleExample"), Scenario);

            var expected = new Dictionary<string, string[]>
            {
                ["console.txt"] = new[] { "INF|packet info", "DBG|[CORPSE] corpse made", "DBG|[LOOT] loot made", "DBG|house debug", "INF|info line", "WRN|warn line", "ERR|error line" },
            };

            AssertFiles(expected, converted);
            AssertFiles(expected, shipped);
        }

        [TestMethod]
        public void Old_and_new_files_examples_route_messages_the_same_way()
        {
            var converted = Run(Log4NetConfigConverter.Convert(ReadTestData("log4net.config.FilesExample")).Json, Scenario);
            var shipped = Run(ReadTestData("serilog.json.FilesExample"), Scenario);

            var all = new[] { "DBG|packet debug", "INF|packet info", "DBG|[CORPSE] corpse made", "DBG|[LOOT] loot made", "DBG|[DATABASE] saved", "DBG|house debug", "DBG|plain debug", "INF|info line", "WRN|warn line", "ERR|error line" };

            var expected = new Dictionary<string, string[]>
            {
                ["ACE_Log.txt"] = all,
                ["ACE_Log-INFO.txt"] = new[] { "INF|packet info", "INF|info line", "WRN|warn line", "ERR|error line" },
                ["ACE_Log-WARN.txt"] = new[] { "WRN|warn line", "ERR|error line" },
                ["ACE_Log-CORPSE.txt"] = new[] { "DBG|[CORPSE] corpse made" },
                ["ACE_Log-DATABASE.txt"] = new[] { "DBG|[DATABASE] saved" },
            };

            AssertFiles(expected, converted);
            AssertFiles(expected, shipped);
        }

        // ---------------------------------------------------------------- semantics

        private const string Layout = "<layout type=\"log4net.Layout.PatternLayout\"><conversionPattern value=\"%message%newline\" /></layout>";

        [TestMethod]
        public void Logger_levels_appenders_and_additivity_follow_the_log4net_hierarchy()
        {
            var xml = $@"<log4net>
                <appender name=""Main"" type=""log4net.Appender.FileAppender""><file value=""main.txt"" />{Layout}</appender>
                <appender name=""Net"" type=""log4net.Appender.FileAppender""><file value=""net.txt"" />{Layout}</appender>
                <logger name=""ACE.Server.Network"" additivity=""false""><level value=""INFO"" /><appender-ref ref=""Net"" /></logger>
                <root><level value=""DEBUG"" /><appender-ref ref=""Main"" /></root>
            </log4net>";

            var files = Run(Log4NetConfigConverter.Convert(xml).Json, factory =>
            {
                factory.CreateLogger("ACE.Server.Network.ClientPacket").LogDebug("net debug");
                factory.CreateLogger("ACE.Server.Network.ClientPacket").LogInformation("net info");
                factory.CreateLogger("ACE.Server.Network").LogInformation("net root info");
                factory.CreateLogger("ACE.Server.NetworkTools").LogInformation("not network");
                factory.CreateLogger("Test").LogDebug("debug");
                factory.CreateLogger("Test").LogTrace("trace");
            });

            AssertFiles(new Dictionary<string, string[]>
            {
                ["net.txt"] = new[] { "INF|net info", "INF|net root info" },
                ["main.txt"] = new[] { "INF|not network", "DBG|debug" },
            }, files);
        }

        [TestMethod]
        public void Filter_chains_are_evaluated_in_order_like_log4net()
        {
            // "secret" is denied first; then anything outside INFO..WARN is denied; what is left is accepted by default
            var xml = $@"<log4net>
                <appender name=""F"" type=""log4net.Appender.FileAppender"">
                    <file value=""f.txt"" />
                    <filter type=""log4net.Filter.StringMatchFilter""><stringToMatch value=""secret"" /><acceptOnMatch value=""false"" /></filter>
                    <filter type=""log4net.Filter.LevelRangeFilter""><levelMin value=""INFO"" /><levelMax value=""WARN"" /></filter>
                    {Layout}
                </appender>
                <root><level value=""ALL"" /><appender-ref ref=""F"" /></root>
            </log4net>";

            var files = Run(Log4NetConfigConverter.Convert(xml).Json, factory =>
            {
                var log = factory.CreateLogger("Test");
                log.LogDebug("d");
                log.LogInformation("i");
                log.LogInformation("has a secret");
                log.LogWarning("w");
                log.LogError("e");
            });

            AssertFiles(new Dictionary<string, string[]> { ["f.txt"] = new[] { "INF|i", "WRN|w" } }, files);
        }

        [TestMethod]
        public void LevelRangeFilter_accepts_by_default_so_later_filters_do_not_run()
        {
            // Checked against log4net 2.0.17: in range is accepted outright unless acceptOnMatch is false
            var xml = $@"<log4net>
                <appender name=""Default"" type=""log4net.Appender.FileAppender"">
                    <file value=""default.txt"" />
                    <filter type=""log4net.Filter.LevelRangeFilter""><levelMin value=""INFO"" /><levelMax value=""WARN"" /></filter>
                    <filter type=""log4net.Filter.DenyAllFilter"" />
                    {Layout}
                </appender>
                <appender name=""PassOn"" type=""log4net.Appender.FileAppender"">
                    <file value=""passon.txt"" />
                    <filter type=""log4net.Filter.LevelRangeFilter""><levelMin value=""INFO"" /><levelMax value=""WARN"" /><acceptOnMatch value=""false"" /></filter>
                    <filter type=""log4net.Filter.StringMatchFilter""><stringToMatch value=""secret"" /><acceptOnMatch value=""false"" /></filter>
                    {Layout}
                </appender>
                <root><level value=""ALL"" /><appender-ref ref=""Default"" /><appender-ref ref=""PassOn"" /></root>
            </log4net>";

            var files = Run(Log4NetConfigConverter.Convert(xml).Json, factory =>
            {
                var log = factory.CreateLogger("Test");
                log.LogDebug("d");
                log.LogInformation("i");
                log.LogInformation("has a secret");
                log.LogWarning("w");
                log.LogError("e");
            });

            AssertFiles(new Dictionary<string, string[]>
            {
                ["default.txt"] = new[] { "INF|i", "INF|has a secret", "WRN|w" },
                ["passon.txt"] = new[] { "INF|i", "WRN|w" },
            }, files);
        }

        [TestMethod]
        public void A_non_additive_logger_also_hides_its_events_from_its_ancestors_appenders()
        {
            var xml = $@"<log4net>
                <appender name=""Root"" type=""log4net.Appender.FileAppender""><file value=""root.txt"" />{Layout}</appender>
                <appender name=""Server"" type=""log4net.Appender.FileAppender""><file value=""server.txt"" />{Layout}</appender>
                <appender name=""Net"" type=""log4net.Appender.FileAppender""><file value=""net.txt"" />{Layout}</appender>
                <logger name=""ACE.Server""><appender-ref ref=""Server"" /></logger>
                <logger name=""ACE.Server.Network"" additivity=""false""><appender-ref ref=""Net"" /></logger>
                <root><level value=""ALL"" /><appender-ref ref=""Root"" /></root>
            </log4net>";

            var files = Run(Log4NetConfigConverter.Convert(xml).Json, factory =>
            {
                factory.CreateLogger("ACE.Server.Network.ClientPacket").LogInformation("net");
                factory.CreateLogger("ACE.Server.Managers.HouseManager").LogInformation("house");
                factory.CreateLogger("ACE.Database").LogInformation("database");
            });

            AssertFiles(new Dictionary<string, string[]>
            {
                ["net.txt"] = new[] { "INF|net" },
                ["server.txt"] = new[] { "INF|house" },
                ["root.txt"] = new[] { "INF|house", "INF|database" },
            }, files);
        }

        [TestMethod]
        public void Console_appenders_get_a_theme_that_writes_each_line_once()
        {
            var xml = $@"<log4net>
                <appender name=""Coloured"" type=""log4net.Appender.ManagedColoredConsoleAppender"">{Layout}</appender>
                <appender name=""Plain"" type=""log4net.Appender.ConsoleAppender"">{Layout}</appender>
                <root><appender-ref ref=""Coloured"" /><appender-ref ref=""Plain"" /></root>
            </log4net>";

            var sinks = JsonNode.Parse(Log4NetConfigConverter.Convert(xml).Json)["Serilog"]["WriteTo"].AsArray().Select(s => s["Args"].AsObject()).ToList();

            Assert.AreEqual(Log4NetConfigConverter.ColouredConsoleTheme, (string)sinks[0]["theme"]);
            Assert.AreEqual(Log4NetConfigConverter.PlainConsoleTheme, (string)sinks[1]["theme"]);
        }

        [TestMethod]
        public void A_date_pattern_that_ends_with_the_extension_moves_it_to_the_path()
        {
            // log4net wrote logs/ace-2026-11-25.txt; Serilog writes logs/ace-20261125.txt
            var xml = $@"<log4net>
                <appender name=""D"" type=""log4net.Appender.RollingFileAppender""><file value=""logs/ace-"" /><rollingStyle value=""Date"" /><datePattern value=""yyyy-MM-dd'.txt'"" /><staticLogFileName value=""false"" />{Layout}</appender>
                <root><appender-ref ref=""D"" /></root>
            </log4net>";

            var args = JsonNode.Parse(Log4NetConfigConverter.Convert(xml).Json)["Serilog"]["WriteTo"][0]["Args"];

            Assert.AreEqual("logs/ace-.txt", (string)args["path"]);
            Assert.AreEqual("Day", (string)args["rollingInterval"]);
        }

        [TestMethod]
        public void Unsupported_appenders_are_skipped_with_a_warning()
        {
            var xml = $@"<log4net>
                <appender name=""Mail"" type=""log4net.Appender.SmtpAppender"" />
                <appender name=""F"" type=""log4net.Appender.FileAppender""><file value=""f.txt"" />{Layout}</appender>
                <root><appender-ref ref=""Mail"" /><appender-ref ref=""F"" /><appender-ref ref=""Missing"" /></root>
            </log4net>";

            var result = Log4NetConfigConverter.Convert(xml);

            Assert.IsTrue(result.Warnings.Any(w => w.Contains("SmtpAppender")), string.Join(Environment.NewLine, result.Warnings));
            Assert.IsTrue(result.Warnings.Any(w => w.Contains("\"Missing\"")), string.Join(Environment.NewLine, result.Warnings));

            // log4net's default root level is DEBUG
            var files = Run(result.Json, factory =>
            {
                factory.CreateLogger("Test").LogTrace("trace");
                factory.CreateLogger("Test").LogDebug("debug");
            });

            AssertFiles(new Dictionary<string, string[]> { ["f.txt"] = new[] { "DBG|debug" } }, files);
        }

        [TestMethod]
        public void Rolling_settings_follow_the_rolling_style()
        {
            var xml = $@"<log4net>
                <appender name=""Composite"" type=""log4net.Appender.RollingFileAppender""><file value=""c.txt"" />{Layout}</appender>
                <appender name=""Date"" type=""log4net.Appender.RollingFileAppender""><file value=""d.txt"" /><rollingStyle value=""Date"" /><datePattern value=""yyyyMMdd-HH"" />{Layout}</appender>
                <appender name=""Size"" type=""log4net.Appender.RollingFileAppender""><file value=""s.txt"" /><rollingStyle value=""Size"" /><maximumFileSize value=""5MB"" /><maxSizeRollBackups value=""-1"" />{Layout}</appender>
                <root><appender-ref ref=""Composite"" /><appender-ref ref=""Date"" /><appender-ref ref=""Size"" /></root>
            </log4net>";

            var sinks = JsonNode.Parse(Log4NetConfigConverter.Convert(xml).Json)["Serilog"]["WriteTo"].AsArray().Select(s => s["Args"].AsObject()).ToList();

            Assert.AreEqual("Day", (string)sinks[0]["rollingInterval"]);
            Assert.AreEqual(true, (bool)sinks[0]["rollOnFileSizeLimit"]);
            Assert.AreEqual(10L * 1024 * 1024, (long)sinks[0]["fileSizeLimitBytes"]);
            Assert.IsNull(sinks[0]["retainedFileCountLimit"]);

            Assert.AreEqual("Hour", (string)sinks[1]["rollingInterval"]);
            Assert.IsNull(sinks[1]["fileSizeLimitBytes"]);
            Assert.IsNull(sinks[1]["retainedFileCountLimit"]);

            Assert.IsNull(sinks[2]["rollingInterval"]);
            Assert.AreEqual(5L * 1024 * 1024, (long)sinks[2]["fileSizeLimitBytes"]);
            Assert.IsNull(sinks[2]["retainedFileCountLimit"]);
        }

        // ---------------------------------------------------------------- helpers

        /// <summary>
        /// Key order and the order of "Using" don't matter
        /// </summary>
        private static string Normalize(string json)
        {
            static JsonNode Sort(JsonNode node, string key)
            {
                switch (node)
                {
                    case JsonObject obj:
                        var sorted = new JsonObject();
                        foreach (var kvp in obj.OrderBy(k => k.Key, StringComparer.Ordinal))
                            sorted[kvp.Key] = Sort(kvp.Value, kvp.Key);
                        return sorted;
                    case JsonArray array:
                        var items = array.Select(i => Sort(i, null)).ToList();
                        if (key == "Using")
                            items = items.OrderBy(i => i.ToJsonString(), StringComparer.Ordinal).ToList();
                        return new JsonArray(items.ToArray());
                    default:
                        return node?.DeepClone();
                }
            }

            return Sort(JsonNode.Parse(json, documentOptions: JsonOptions), null).ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        }

        /// <summary>
        /// Runs a serilog.json with every sink writing "LEVEL|message" lines into a temporary folder: each File sink to its own
        /// file name, and every Console sink to console.txt. Returns the lines per file.
        /// </summary>
        private static Dictionary<string, string[]> Run(string json, Action<ILoggerFactory> emit)
        {
            var directory = Path.Combine(Path.GetTempPath(), "ace-serilog-test-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);

            try
            {
                var root = JsonNode.Parse(json, documentOptions: JsonOptions);
                Redirect(root, directory);

                var configuration = new ConfigurationBuilder()
                    .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(root.ToJsonString())))
                    .Build();

                var logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

                using (var factory = new LoggerFactory())
                {
                    factory.AddProvider(new SerilogLoggerProvider(logger, dispose: true));
                    emit(factory);
                }

                return Directory.GetFiles(directory).ToDictionary(Path.GetFileName, f => File.ReadAllLines(f));
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        private static void Redirect(JsonNode node, string directory)
        {
            if (node is JsonArray array)
            {
                foreach (var item in array)
                    Redirect(item, directory);
                return;
            }

            if (node is not JsonObject obj)
                return;

            var name = (obj["Name"] as JsonValue)?.GetValue<string>();
            var args = obj["Args"] as JsonObject;

            if ((name == "File" || name == "Console") && args != null)
            {
                var file = name == "Console" ? "console.txt" : Path.GetFileName((string)args["path"]);

                obj["Name"] = "File";
                args.Remove("standardErrorFromLevel");
                args.Remove("theme");
                args["path"] = Path.Combine(directory, file);
                args["shared"] = true;  // several sinks may write the same file here
                args["outputTemplate"] = "{Level:u3}|{Message:lj}{NewLine}";
                return;
            }

            foreach (var kvp in obj.ToList())
                Redirect(kvp.Value, directory);
        }

        private static void AssertFiles(Dictionary<string, string[]> expected, Dictionary<string, string[]> actual)
        {
            CollectionAssert.AreEquivalent(expected.Keys.ToList(), actual.Keys.ToList(), "files: " + string.Join(", ", actual.Keys));

            foreach (var kvp in expected)
                CollectionAssert.AreEqual(kvp.Value, actual[kvp.Key], $"{kvp.Key}:{Environment.NewLine}{string.Join(Environment.NewLine, actual[kvp.Key])}");
        }
    }
}
