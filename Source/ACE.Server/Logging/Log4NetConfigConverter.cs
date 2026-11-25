using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ACE.Server.Logging
{
    /// <summary>
    /// Converts a log4net XML configuration into the equivalent serilog.json (Serilog.Settings.Configuration format).<para />
    /// Supported: root and logger levels, console and file appenders, forwarding and async forwarding appenders,
    /// appender thresholds, PatternLayout patterns, and the Level/LevelRange/String/Logger/DenyAll filters.<para />
    /// Anything else is skipped and reported in <see cref="Log4NetConversionResult.Warnings"/>.
    /// </summary>
    public static class Log4NetConfigConverter
    {
        private static readonly string[] SerilogLevels = { "Verbose", "Debug", "Information", "Warning", "Error", "Fatal" };

        private const string DefaultLog4NetDateFormat = "yyyy-MM-dd HH:mm:ss,fff";

        public static Log4NetConversionResult Convert(string log4NetXml)
        {
            var context = new ConversionContext();

            var document = XDocument.Parse(log4NetXml);
            var root = document.Root?.DescendantsAndSelf().FirstOrDefault(e => Is(e, "log4net"))
                ?? throw new FormatException("No <log4net> element was found.");

            foreach (var appender in root.Elements().Where(e => Is(e, "appender")))
            {
                var name = Attribute(appender, "name");
                if (!string.IsNullOrEmpty(name))
                    context.Appenders[name] = appender;
            }

            // Levels

            var repositoryThreshold = ParseLevel(Attribute(root, "threshold"), context, "<log4net threshold>");

            var rootElement = root.Elements().FirstOrDefault(e => Is(e, "root"));
            var rootLevel = ParseLevel(LevelOf(rootElement), context, "<root>") ?? new Level(1); // log4net's default root level is DEBUG

            if (repositoryThreshold != null && repositoryThreshold.Value.Index > rootLevel.Index)
                rootLevel = repositoryThreshold.Value;

            var overrides = new JsonObject();
            var loggers = root.Elements().Where(e => Is(e, "logger") || Is(e, "category")).ToList();

            foreach (var logger in loggers)
            {
                var name = Attribute(logger, "name");
                if (string.IsNullOrEmpty(name))
                    continue;

                var level = ParseLevel(LevelOf(logger), context, $"<logger name=\"{name}\">");
                if (level != null)
                    overrides[name] = level.Value.Name;
            }

            // Appender paths: every route from <root> or a <logger> to a console or file appender

            var nonAdditiveLoggers = new List<string>();

            foreach (var logger in loggers)
            {
                var name = Attribute(logger, "name");
                if (string.IsNullOrEmpty(name))
                    continue;

                var loggerCondition = LoggerCondition(name);

                foreach (var appenderRef in AppenderRefs(logger))
                    Walk(appenderRef, new PathState(loggerCondition), context, new HashSet<string>());

                if (string.Equals(Attribute(logger, "additivity"), "false", StringComparison.OrdinalIgnoreCase))
                    nonAdditiveLoggers.Add(loggerCondition);
            }

            if (rootElement != null)
            {
                var rootCondition = nonAdditiveLoggers.Count == 0 ? null : And(nonAdditiveLoggers.Select(Not));

                foreach (var appenderRef in AppenderRefs(rootElement))
                    Walk(appenderRef, new PathState(rootCondition), context, new HashSet<string>());
            }

            if (context.Sinks.Count == 0)
                context.Warn("No console or file appender is referenced from <root> or a <logger>, so nothing will be logged.");

            // Build the Serilog section

            var asyncSinks = new List<JsonNode>();
            var syncSinks = new List<JsonNode>();

            foreach (var sink in context.Sinks.Values)
                (sink.IsAsync ? asyncSinks : syncSinks).Add(sink.Build(context));

            var writeTo = new JsonArray();

            if (asyncSinks.Count > 0)
            {
                context.Using.Add("Serilog.Sinks.Async");
                writeTo.Add(new JsonObject { ["Name"] = "Async", ["Args"] = new JsonObject { ["configure"] = new JsonArray(asyncSinks.ToArray()) } });
            }

            foreach (var sink in syncSinks)
                writeTo.Add(sink);

            if (context.UsesThreadId)
                context.Using.Add("Serilog.Enrichers.Thread");

            var minimumLevel = new JsonObject { ["Default"] = rootLevel.Name };
            if (overrides.Count > 0)
                minimumLevel["Override"] = overrides;

            var serilog = new JsonObject
            {
                ["Using"] = new JsonArray(context.Using.OrderBy(u => u, StringComparer.Ordinal).Select(u => (JsonNode)u).ToArray()),
                ["MinimumLevel"] = minimumLevel,
            };

            if (context.UsesThreadId)
                serilog["Enrich"] = new JsonArray("WithThreadId");

            serilog["WriteTo"] = writeTo;

            var json = new JsonObject { ["Serilog"] = serilog }.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            });

            return new Log4NetConversionResult(json, context.Warnings);
        }

        // ---------------------------------------------------------------- appender graph

        private static void Walk(string appenderName, PathState path, ConversionContext context, HashSet<string> visited)
        {
            if (!context.Appenders.TryGetValue(appenderName, out var appender))
            {
                context.Warn($"appender-ref \"{appenderName}\" does not match any <appender>; skipped.");
                return;
            }

            if (!visited.Add(appenderName))
            {
                context.Warn($"Appender \"{appenderName}\" forwards to itself; the loop was skipped.");
                return;
            }

            var type = ShortTypeName(Attribute(appender, "type"));
            var kind = KindOf(type);

            if (kind == AppenderKind.Unsupported)
            {
                context.Warn($"Appender \"{appenderName}\" ({Attribute(appender, "type")}) has no Serilog equivalent here; skipped.");
                return;
            }

            // threshold and filters apply to everything passing through this appender, forwarding or not
            var threshold = ParseLevel(Param(appender, "threshold"), context, $"appender \"{appenderName}\" threshold");
            if (threshold?.IsOff == true)
                return;

            var stage = path.Then(threshold, FilterExpression(appender, appenderName, context), kind == AppenderKind.AsyncForwarding);

            if (kind == AppenderKind.Forwarding || kind == AppenderKind.AsyncForwarding)
            {
                if (type.StartsWith("Buffering", StringComparison.OrdinalIgnoreCase))
                    context.Warn($"Appender \"{appenderName}\" ({type}) is converted as a plain forwarder; its buffering and trigger settings are not carried over.");

                foreach (var appenderRef in AppenderRefs(appender))
                    Walk(appenderRef, stage, context, new HashSet<string>(visited));

                return;
            }

            if (!context.Sinks.TryGetValue(appenderName, out var sink))
            {
                sink = new SinkDefinition(appenderName, appender, kind);
                context.Sinks[appenderName] = sink;
            }

            sink.Paths.Add(stage);
        }

        private enum AppenderKind { Unsupported, Console, File, Forwarding, AsyncForwarding }

        private static AppenderKind KindOf(string type)
        {
            switch (type.ToLowerInvariant())
            {
                case "consoleappender":
                case "coloredconsoleappender":
                case "managedcoloredconsoleappender":
                case "ansicolorterminalappender":
                    return AppenderKind.Console;

                case "fileappender":
                case "rollingfileappender":
                    return AppenderKind.File;

                case "forwardingappender":
                case "bufferingforwardingappender":
                    return AppenderKind.Forwarding;

                case "asyncforwardingappender":     // Log4Net.Async
                case "parallelforwardingappender":  // Log4Net.Async
                case "asyncappender":
                    return AppenderKind.AsyncForwarding;

                default:
                    return AppenderKind.Unsupported;
            }
        }

        private sealed class PathState
        {
            public readonly Level? MinLevel;
            public readonly List<string> Conditions;
            public readonly bool IsAsync;

            public PathState(string condition)
            {
                Conditions = new List<string>();
                if (condition != null)
                    Conditions.Add(condition);
            }

            private PathState(Level? minLevel, List<string> conditions, bool isAsync)
            {
                MinLevel = minLevel;
                Conditions = conditions;
                IsAsync = isAsync;
            }

            public PathState Then(Level? threshold, string filter, bool isAsync)
            {
                var minLevel = MinLevel;
                if (threshold != null && (minLevel == null || threshold.Value.Index > minLevel.Value.Index))
                    minLevel = threshold;

                var conditions = new List<string>(Conditions);
                if (filter != null)
                    conditions.Add(filter);

                return new PathState(minLevel, conditions, IsAsync || isAsync);
            }

            /// <summary>
            /// The path as one expression, including its level threshold
            /// </summary>
            public string ToExpression()
            {
                var parts = new List<string>();
                if (MinLevel != null && MinLevel.Value.Index > 0)
                    parts.Add(LevelAtLeast(MinLevel.Value));
                parts.AddRange(Conditions);
                return parts.Count == 0 ? null : And(parts);
            }
        }

        private sealed class SinkDefinition
        {
            private readonly string name;
            private readonly XElement appender;
            private readonly AppenderKind kind;

            public readonly List<PathState> Paths = new List<PathState>();

            public SinkDefinition(string name, XElement appender, AppenderKind kind)
            {
                this.name = name;
                this.appender = appender;
                this.kind = kind;
            }

            public bool IsAsync => Paths.Any(p => p.IsAsync);

            public JsonObject Build(ConversionContext context)
            {
                var args = kind == AppenderKind.Console ? ConsoleArgs(context) : FileArgs(context);

                args["outputTemplate"] = ConvertLayout(appender, name, context);

                string filter;

                if (Paths.Count == 1)
                {
                    // a single route keeps its threshold as the sink's own minimum level, which reads more naturally
                    var path = Paths[0];
                    if (path.MinLevel != null && path.MinLevel.Value.Index > 0)
                        args["restrictedToMinimumLevel"] = path.MinLevel.Value.Name;
                    filter = path.Conditions.Count == 0 ? null : And(path.Conditions);
                }
                else
                {
                    // several routes reach the same appender: log4net writes each event once per accepting route,
                    // Serilog writes it once if any route accepts it
                    var routes = Paths.Select(p => p.ToExpression()).ToList();
                    filter = routes.Any(r => r == null) ? null : Or(routes);

                    context.Warn($"Appender \"{name}\" is reached by {Paths.Count} routes. log4net wrote a message once per route that accepted it; Serilog writes it once.");
                }

                var sink = new JsonObject
                {
                    ["Name"] = kind == AppenderKind.Console ? "Console" : "File",
                    ["Args"] = args,
                };

                context.Using.Add(kind == AppenderKind.Console ? "Serilog.Sinks.Console" : "Serilog.Sinks.File");

                if (filter == null)
                    return sink;

                context.Using.Add("Serilog.Expressions");

                return new JsonObject
                {
                    ["Name"] = "Logger",
                    ["Args"] = new JsonObject
                    {
                        ["configureLogger"] = new JsonObject
                        {
                            ["Filter"] = new JsonArray(new JsonObject
                            {
                                ["Name"] = "ByIncludingOnly",
                                ["Args"] = new JsonObject { ["expression"] = filter },
                            }),
                            ["WriteTo"] = new JsonArray(sink),
                        },
                    },
                };
            }

            private JsonObject ConsoleArgs(ConversionContext context)
            {
                var args = new JsonObject();

                var target = Param(appender, "target");
                if (target != null && target.Contains("Error", StringComparison.OrdinalIgnoreCase))
                    args["standardErrorFromLevel"] = "Verbose";

                if (appender.Elements().Any(e => Is(e, "mapping")))
                    context.Warn($"Appender \"{name}\": per-level console colours are not converted; Serilog's console theme colours the level and values instead.");

                return args;
            }

            private JsonObject FileArgs(ConversionContext context)
            {
                var args = new JsonObject();

                var path = Param(appender, "file");
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = $"{name}.txt";
                    context.Warn($"Appender \"{name}\" has no file; using \"{path}\".");
                }
                else if (path.Contains('%') || path.Contains("${", StringComparison.Ordinal))
                {
                    var cleaned = Regex.Replace(path, @"%[a-zA-Z]+(\{[^}]*\})?|\$\{[^}]*\}", "");
                    context.Warn($"Appender \"{name}\": the file name pattern \"{path}\" cannot be converted; using \"{cleaned}\".");
                    path = cleaned;
                }

                args["path"] = path;

                var type = ShortTypeName(Attribute(appender, "type"));
                var isRolling = type.Equals("RollingFileAppender", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(Param(appender, "appendToFile"), "false", StringComparison.OrdinalIgnoreCase))
                    context.Warn($"Appender \"{name}\": appendToFile=false is not supported; Serilog always appends to an existing file.");

                var lockingModel = appender.Elements().FirstOrDefault(e => Is(e, "lockingModel"));
                var lockingType = ShortTypeName(lockingModel == null ? null : Attribute(lockingModel, "type"));
                if (lockingType.Contains("MinimalLock", StringComparison.OrdinalIgnoreCase) || lockingType.Contains("InterProcessLock", StringComparison.OrdinalIgnoreCase))
                    args["shared"] = true;

                if (string.Equals(Param(appender, "immediateFlush"), "false", StringComparison.OrdinalIgnoreCase))
                    args["buffered"] = true;

                if (!isRolling)
                {
                    args["fileSizeLimitBytes"] = null;  // a plain FileAppender never stops writing
                    return args;
                }

                var style = (Param(appender, "rollingStyle") ?? "Composite").Trim();

                var bySize = style.Equals("Size", StringComparison.OrdinalIgnoreCase) || style.Equals("Composite", StringComparison.OrdinalIgnoreCase) || style.Equals("Once", StringComparison.OrdinalIgnoreCase);
                var byDate = style.Equals("Date", StringComparison.OrdinalIgnoreCase) || style.Equals("Composite", StringComparison.OrdinalIgnoreCase);

                if (style.Equals("Once", StringComparison.OrdinalIgnoreCase))
                    context.Warn($"Appender \"{name}\": rollingStyle Once (a new file per start) has no Serilog equivalent; rolling by size instead.");

                if (byDate)
                    args["rollingInterval"] = RollingInterval(Param(appender, "datePattern") ?? ".yyyy-MM-dd");

                if (bySize)
                {
                    args["rollOnFileSizeLimit"] = true;
                    args["fileSizeLimitBytes"] = ParseFileSize(Param(appender, "maximumFileSize") ?? Param(appender, "maxFileSize"), name, context);
                }
                else
                    args["fileSizeLimitBytes"] = null;

                var backups = Param(appender, "maxSizeRollBackups");
                if (bySize && !byDate)
                {
                    var count = int.TryParse(backups, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : 0;   // log4net default: 0 backups
                    args["retainedFileCountLimit"] = count < 0 ? null : count + 1;                                               // Serilog counts the current file too
                }
                else
                    args["retainedFileCountLimit"] = null;  // log4net never deletes date-rolled files

                return args;
            }
        }

        private static string RollingInterval(string datePattern)
        {
            // characters inside quotes are literals in a .NET date pattern
            var pattern = Regex.Replace(datePattern, "'[^']*'", "");

            if (pattern.Contains("mm", StringComparison.Ordinal))
                return "Minute";
            if (pattern.Contains("HH", StringComparison.Ordinal) || pattern.Contains("hh", StringComparison.Ordinal))
                return "Hour";
            if (pattern.Contains("dd", StringComparison.Ordinal))
                return "Day";
            if (pattern.Contains("MM", StringComparison.Ordinal))
                return "Month";
            return "Year";
        }

        private static long ParseFileSize(string value, string appenderName, ConversionContext context)
        {
            const long log4NetDefault = 10L * 1024 * 1024;

            if (string.IsNullOrWhiteSpace(value))
                return log4NetDefault;

            var match = Regex.Match(value.Trim(), @"^(\d+)\s*(KB|MB|GB)?$", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                context.Warn($"Appender \"{appenderName}\": maximumFileSize \"{value}\" was not understood; using 10MB.");
                return log4NetDefault;
            }

            var size = long.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            return match.Groups[2].Value.ToUpperInvariant() switch
            {
                "KB" => size * 1024,
                "MB" => size * 1024 * 1024,
                "GB" => size * 1024 * 1024 * 1024,
                _ => size,
            };
        }

        // ---------------------------------------------------------------- layouts

        private static string ConvertLayout(XElement appender, string appenderName, ConversionContext context)
        {
            var layout = appender.Elements().FirstOrDefault(e => Is(e, "layout"));
            string pattern;

            if (layout == null)
                pattern = "%message%newline";
            else
            {
                var layoutType = ShortTypeName(Attribute(layout, "type"));

                if (layoutType.Equals("SimpleLayout", StringComparison.OrdinalIgnoreCase))
                    pattern = "%level - %message%newline";
                else if (layoutType.StartsWith("PatternLayout", StringComparison.OrdinalIgnoreCase))
                    pattern = Param(layout, "conversionPattern") ?? Attribute(layout, "value") ?? "%message%newline";
                else
                {
                    context.Warn($"Appender \"{appenderName}\": layout {Attribute(layout, "type")} is not supported; using a default message template.");
                    pattern = "%date [%thread] %-5level %logger - %message%newline";
                }
            }

            return ConvertPattern(pattern, appenderName, context);
        }

        private static readonly Regex PatternToken = new Regex(@"%(?<align>-?\d+)?(?:\.(?<trunc>\d+))?(?<name>[a-zA-Z]+)(?:\{(?<option>[^}]*)\})?", RegexOptions.Compiled);

        /// <summary>
        /// Converts a log4net PatternLayout conversion pattern into a Serilog output template
        /// </summary>
        public static string ConvertPattern(string pattern, out IReadOnlyList<string> warnings)
        {
            var context = new ConversionContext();
            var template = ConvertPattern(pattern, "pattern", context);
            warnings = context.Warnings;
            return template;
        }

        private static string ConvertPattern(string pattern, string appenderName, ConversionContext context)
        {
            var result = new StringBuilder();
            var hasException = false;
            var position = 0;

            while (position < pattern.Length)
            {
                var percent = pattern.IndexOf('%', position);
                if (percent < 0)
                {
                    AppendLiteral(result, pattern.Substring(position));
                    break;
                }

                AppendLiteral(result, pattern.Substring(position, percent - position));

                if (percent + 1 < pattern.Length && pattern[percent + 1] == '%')
                {
                    result.Append('%');
                    position = percent + 2;
                    continue;
                }

                var match = PatternToken.Match(pattern, percent);
                if (!match.Success || match.Index != percent)
                {
                    AppendLiteral(result, "%");
                    position = percent + 1;
                    continue;
                }

                position = match.Index + match.Length;

                var name = match.Groups["name"].Value;
                var option = match.Groups["option"].Success ? match.Groups["option"].Value : null;
                var align = match.Groups["align"].Success ? "," + match.Groups["align"].Value : "";

                if (match.Groups["trunc"].Success)
                    context.Warn($"Appender \"{appenderName}\": truncation in %{match.Value.TrimStart('%')} is not supported and was dropped.");

                // single-letter names are case-sensitive in log4net: %p is the level, %P a property
                switch (name.Length == 1 ? name : name.ToLowerInvariant())
                {
                    case "d":
                    case "date":
                        result.Append("{Timestamp").Append(align).Append(':').Append(DateFormat(option)).Append('}');
                        break;

                    case "utcdate":
                        result.Append("{UtcTimestamp").Append(align).Append(':').Append(DateFormat(option)).Append('}');
                        break;

                    case "p":
                    case "level":
                        result.Append("{Level:u3}");
                        break;

                    case "m":
                    case "message":
                        result.Append("{Message").Append(align).Append(":lj}");
                        break;

                    case "n":
                    case "newline":
                        result.Append("{NewLine}");
                        break;

                    case "exception":
                        result.Append("{Exception}");
                        hasException = true;
                        break;

                    case "t":
                    case "thread":
                        result.Append("{ThreadId").Append(align).Append('}');
                        context.UsesThreadId = true;
                        break;

                    case "c":
                    case "logger":
                        result.Append("{SourceContext").Append(align).Append('}');
                        if (option != null)
                            context.Warn($"Appender \"{appenderName}\": the logger name precision {{{option}}} is not supported; the full name is written.");
                        break;

                    case "property":
                    case "P":
                    case "mdc":
                    case "X":
                        if (string.IsNullOrEmpty(option))
                            result.Append("{Properties}");
                        else
                            result.Append('{').Append(option).Append(align).Append('}');
                        break;

                    default:
                        context.Warn($"Appender \"{appenderName}\": %{name} has no Serilog equivalent and was dropped.");
                        break;
                }
            }

            // PatternLayout ignores exceptions, so log4net appended them after the formatted line
            if (!hasException)
                result.Append("{Exception}");

            return result.ToString();
        }

        private static string DateFormat(string option)
        {
            if (string.IsNullOrEmpty(option))
                return DefaultLog4NetDateFormat;

            switch (option.ToUpperInvariant())
            {
                case "ISO8601":
                    return DefaultLog4NetDateFormat;
                case "ABSOLUTE":
                    return "HH:mm:ss,fff";
                case "DATE":
                    return "dd MMM yyyy HH:mm:ss,fff";
                default:
                    return option;
            }
        }

        private static void AppendLiteral(StringBuilder builder, string text)
        {
            builder.Append(text.Replace("{", "{{").Replace("}", "}}"));
        }

        // ---------------------------------------------------------------- filters

        /// <summary>
        /// Converts an appender's filter chain into one Serilog.Expressions expression, or null when it accepts everything.<para />
        /// log4net runs the filters in order: the first to accept or deny decides, and an event no filter decides is accepted.
        /// </summary>
        private static string FilterExpression(XElement appender, string appenderName, ConversionContext context)
        {
            var filters = appender.Elements().Where(e => Is(e, "filter")).ToList();
            if (filters.Count == 0)
                return null;

            // built from the last filter back: "expr" is what happens to an event that reaches this point undecided
            string expr = "true";

            for (var i = filters.Count - 1; i >= 0; i--)
            {
                var filter = filters[i];
                var type = ShortTypeName(Attribute(filter, "type"));
                var acceptOnMatch = !string.Equals(Param(filter, "acceptOnMatch"), "false", StringComparison.OrdinalIgnoreCase);

                switch (type.ToLowerInvariant())
                {
                    case "denyallfilter":
                        expr = "false";
                        break;

                    case "levelmatchfilter":
                    {
                        var level = ParseLevel(Param(filter, "levelToMatch"), context, $"appender \"{appenderName}\" LevelMatchFilter");
                        if (level == null)
                            break;
                        expr = Decide($"@l = '{level.Value.Name}'", acceptOnMatch, expr);
                        break;
                    }

                    case "levelrangefilter":
                    {
                        // outside the range is denied; inside is accepted, or passed on when acceptOnMatch is false (its default)
                        acceptOnMatch = string.Equals(Param(filter, "acceptOnMatch"), "true", StringComparison.OrdinalIgnoreCase);
                        var min = ParseLevel(Param(filter, "levelMin"), context, $"appender \"{appenderName}\" LevelRangeFilter") ?? new Level(0);
                        var max = ParseLevel(Param(filter, "levelMax"), context, $"appender \"{appenderName}\" LevelRangeFilter") ?? new Level(SerilogLevels.Length - 1);
                        var inRange = LevelIn(min.Index, max.Index);
                        expr = acceptOnMatch ? inRange : And(new[] { inRange, expr });
                        break;
                    }

                    case "stringmatchfilter":
                    {
                        var regex = Param(filter, "regexToMatch");
                        var text = Param(filter, "stringToMatch");
                        string condition;
                        if (regex != null)
                            condition = $"IsMatch(@m, {Quote(regex)})";
                        else if (text != null)
                            condition = $"IsMatch(@m, {Quote(Regex.Escape(text))})";
                        else
                            break;
                        expr = Decide(condition, acceptOnMatch, expr);
                        break;
                    }

                    case "loggermatchfilter":
                    {
                        var logger = Param(filter, "loggerToMatch");
                        if (logger == null)
                            break;
                        expr = Decide($"StartsWith(SourceContext, {Quote(logger)})", acceptOnMatch, expr);
                        break;
                    }

                    default:
                        context.Warn($"Appender \"{appenderName}\": filter {Attribute(filter, "type")} is not supported and was ignored.");
                        break;
                }
            }

            return expr == "true" ? null : expr;
        }

        /// <summary>
        /// A filter that accepts (or denies) on match and otherwise passes the event on to "rest"
        /// </summary>
        private static string Decide(string condition, bool acceptOnMatch, string rest)
        {
            if (acceptOnMatch)
            {
                if (rest == "true") return "true";
                if (rest == "false") return condition;
                return $"({condition}) or ({rest})";
            }

            if (rest == "false") return "false";
            if (rest == "true") return $"not ({condition})";
            return $"not ({condition}) and ({rest})";
        }

        private static string LoggerCondition(string loggerName)
        {
            // log4net's logger hierarchy is dot-delimited: "A.B" covers "A.B" and "A.B.C", not "A.BC"
            return $"(SourceContext = {Quote(loggerName)} or StartsWith(SourceContext, {Quote(loggerName + ".")}))";
        }

        private static string LevelAtLeast(Level level) => LevelIn(level.Index, SerilogLevels.Length - 1);

        private static string LevelIn(int min, int max)
        {
            var levels = SerilogLevels.Skip(min).Take(max - min + 1).Select(l => $"'{l}'");
            return $"@l in [{string.Join(", ", levels)}]";
        }

        private static string And(IEnumerable<string> parts)
        {
            var list = parts.Where(p => p != null && p != "true").ToList();
            if (list.Contains("false")) return "false";
            if (list.Count == 0) return "true";
            return list.Count == 1 ? list[0] : string.Join(" and ", list.Select(p => $"({p})"));
        }

        private static string Or(IEnumerable<string> parts)
        {
            var list = parts.Where(p => p != null && p != "false").ToList();
            if (list.Contains("true")) return "true";
            if (list.Count == 0) return "false";
            return list.Count == 1 ? list[0] : string.Join(" or ", list.Select(p => $"({p})"));
        }

        private static string Not(string condition) => $"not {condition}";

        private static string Quote(string value) => "'" + value.Replace("'", "''") + "'";

        // ---------------------------------------------------------------- levels

        private readonly struct Level
        {
            public readonly int Index;  // into SerilogLevels; SerilogLevels.Length means OFF

            public Level(int index) { Index = index; }

            public bool IsOff => Index >= SerilogLevels.Length;

            public string Name => IsOff ? "Fatal" : SerilogLevels[Index];
        }

        private static Level? ParseLevel(string value, ConversionContext context, string where)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            switch (value.Trim().ToUpperInvariant())
            {
                case "ALL":
                case "FINEST":
                case "VERBOSE":
                case "TRACE":
                case "FINER":
                    return new Level(0);
                case "FINE":
                case "DEBUG":
                    return new Level(1);
                case "INFO":
                case "NOTICE":
                    return new Level(2);
                case "WARN":
                    return new Level(3);
                case "ERROR":
                    return new Level(4);
                case "SEVERE":
                case "CRITICAL":
                case "ALERT":
                case "FATAL":
                case "EMERGENCY":
                    return new Level(5);
                case "OFF":
                    context.Warn($"{where}: level OFF has no Serilog equivalent; Fatal messages are still logged.");
                    return new Level(SerilogLevels.Length);
                default:
                    context.Warn($"{where}: level \"{value}\" was not understood and was ignored.");
                    return null;
            }
        }

        private static string LevelOf(XElement element)
        {
            if (element == null)
                return null;

            var level = element.Elements().FirstOrDefault(e => Is(e, "level") || Is(e, "priority"));
            return level == null ? null : Attribute(level, "value");
        }

        // ---------------------------------------------------------------- XML helpers

        private static bool Is(XElement element, string name) => string.Equals(element.Name.LocalName, name, StringComparison.OrdinalIgnoreCase);

        private static string Attribute(XElement element, string name)
        {
            return element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, name, StringComparison.OrdinalIgnoreCase))?.Value;
        }

        /// <summary>
        /// A log4net setting: &lt;name value="..."/&gt;, or the older &lt;param name="name" value="..."/&gt;
        /// </summary>
        private static string Param(XElement element, string name)
        {
            foreach (var child in element.Elements())
            {
                if (Is(child, name))
                    return Attribute(child, "value") ?? child.Value;

                if (Is(child, "param") && string.Equals(Attribute(child, "name"), name, StringComparison.OrdinalIgnoreCase))
                    return Attribute(child, "value");
            }
            return null;
        }

        private static IEnumerable<string> AppenderRefs(XElement element)
        {
            return element.Elements().Where(e => Is(e, "appender-ref")).Select(e => Attribute(e, "ref")).Where(r => !string.IsNullOrEmpty(r));
        }

        private static string ShortTypeName(string type)
        {
            if (string.IsNullOrEmpty(type))
                return "";

            var name = type.Split(',')[0].Trim();
            var dot = name.LastIndexOf('.');
            return dot < 0 ? name : name.Substring(dot + 1);
        }

        private sealed class ConversionContext
        {
            internal readonly Dictionary<string, XElement> Appenders = new Dictionary<string, XElement>(StringComparer.Ordinal);
            internal readonly Dictionary<string, SinkDefinition> Sinks = new Dictionary<string, SinkDefinition>(StringComparer.Ordinal);
            internal readonly HashSet<string> Using = new HashSet<string>(StringComparer.Ordinal);
            internal readonly List<string> Warnings = new List<string>();
            internal bool UsesThreadId;

            internal void Warn(string warning)
            {
                if (!Warnings.Contains(warning))
                    Warnings.Add(warning);
            }

        }
    }

    public sealed class Log4NetConversionResult
    {
        public string Json { get; }

        /// <summary>
        /// Settings that could not be converted exactly. Logged at startup after a conversion.
        /// </summary>
        public IReadOnlyList<string> Warnings { get; }

        public Log4NetConversionResult(string json, IReadOnlyList<string> warnings)
        {
            Json = json;
            Warnings = warnings;
        }
    }
}
