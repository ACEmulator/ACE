using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

using ACE.Common.Logging;

namespace ACE.Server.Logging
{
    /// <summary>
    /// Configures Serilog from serilog.json and plugs it into <see cref="LogManager.Factory"/>.<para />
    /// serilog.json follows the same lifecycle log4net.config did: it is looked for in the working directory, then next to the
    /// executable, and in a container it is kept in the config volume. When it is missing, an existing log4net.config is
    /// converted into it; failing that, serilog.json.example (or serilog.json.docker in a container) is copied.
    /// </summary>
    public static class SerilogSetup
    {
        public const string ConfigFileName = "serilog.json";
        public const string Log4NetConfigFileName = "log4net.config";

        private static Serilog.Core.Logger logger;

        /// <summary>
        /// Something to report once logging is running, such as the result of a log4net.config conversion
        /// </summary>
        public readonly record struct StartupNote(LogLevel Level, string Message);

        public static void Initialize(string exeLocation, bool isRunningInContainer, string containerConfigDirectory)
        {
            var notes = new List<StartupNote>();

            // Serilog reports its own problems, such as a log file it cannot open, here rather than throwing
            Serilog.Debugging.SelfLog.Enable(Console.Error);

            var configFile = ResolveConfigFile(Directory.GetCurrentDirectory(), exeLocation, isRunningInContainer, containerConfigDirectory, notes);

            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(configFile, optional: false, reloadOnChange: true)
                    .Build();

                logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{configFile} could not be loaded: {ex.Message}");
                Console.WriteLine("Logging to the console only until it is fixed.");

                logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss,fff} {Level:u3}: {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();

                notes.Add(new StartupNote(LogLevel.Error, $"{configFile} could not be loaded, so only the console is being logged to: {ex.Message}"));
            }

            // Every logger created so far (static fields initialised before Main) picks this provider up retroactively.
            LogManager.Factory.AddProvider(new SerilogLoggerProvider(logger, dispose: true));

            var log = LogManager.GetLogger(typeof(SerilogSetup));

            foreach (var note in notes)
                log.Log(note.Level, "{Message:l}", note.Message);
        }

        /// <summary>
        /// Finds serilog.json, creating it if needed, and returns its path. Throws if it cannot be found or created.
        /// </summary>
        public static string ResolveConfigFile(string workingDirectory, string exeLocation, bool isRunningInContainer, string containerConfigDirectory, List<StartupNote> notes)
        {
            var configFile = Path.Combine(exeLocation, ConfigFileName);
            var containerConfigFile = Path.Combine(containerConfigDirectory, ConfigFileName);

            if (isRunningInContainer && File.Exists(containerConfigFile))
                File.Copy(containerConfigFile, configFile, true);

            var workingDirectoryConfigFile = Path.Combine(workingDirectory, ConfigFileName);
            if (File.Exists(workingDirectoryConfigFile))
                return workingDirectoryConfigFile;

            if (File.Exists(configFile))
                return configFile;

            if (TryConvertLog4NetConfig(workingDirectory, exeLocation, isRunningInContainer, containerConfigDirectory, configFile, notes))
            {
                if (isRunningInContainer)
                    TryCopy(configFile, containerConfigFile, notes);

                return configFile;
            }

            var exampleFile = Path.Combine(exeLocation, ConfigFileName + ".example");

            if (!File.Exists(exampleFile))
            {
                Console.WriteLine($"Serilog configuration file is missing. Please copy the file {ConfigFileName}.example to {ConfigFileName} and edit it to match your needs before running ACE.");
                throw new Exception("missing Serilog configuration file");
            }

            if (!isRunningInContainer)
            {
                Console.WriteLine("Serilog configuration file is missing, cloning from example file.");
                File.Copy(exampleFile, configFile);
            }
            else
            {
                Console.WriteLine("Serilog configuration file is missing, ACEmulator is running in a container, cloning from docker file.");
                var dockerFile = Path.Combine(exeLocation, ConfigFileName + ".docker");
                File.Copy(dockerFile, configFile);
                TryCopy(dockerFile, containerConfigFile, notes);
            }

            return configFile;
        }

        private static bool TryConvertLog4NetConfig(string workingDirectory, string exeLocation, bool isRunningInContainer, string containerConfigDirectory, string configFile, List<StartupNote> notes)
        {
            var candidates = new List<string>
            {
                Path.Combine(workingDirectory, Log4NetConfigFileName),
                Path.Combine(exeLocation, Log4NetConfigFileName),
            };

            if (isRunningInContainer)
                candidates.Add(Path.Combine(containerConfigDirectory, Log4NetConfigFileName));

            var log4NetConfig = candidates.Find(File.Exists);
            if (log4NetConfig == null)
                return false;

            try
            {
                var result = Log4NetConfigConverter.Convert(File.ReadAllText(log4NetConfig));

                File.WriteAllText(configFile, result.Json);

                Console.WriteLine($"Converted {log4NetConfig} to {configFile}.");

                notes.Add(new StartupNote(LogLevel.Warning, $"ACE now logs with Serilog. {log4NetConfig} was converted to {configFile}, which is used from now on; {Log4NetConfigFileName} is no longer read and can be deleted."));

                foreach (var warning in result.Warnings)
                    notes.Add(new StartupNote(LogLevel.Warning, $"log4net conversion: {warning}"));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{log4NetConfig} could not be converted: {ex.Message}");

                notes.Add(new StartupNote(LogLevel.Error, $"{log4NetConfig} could not be converted to {ConfigFileName}, so the default configuration is used instead: {ex.Message}"));

                return false;
            }
        }

        private static void TryCopy(string source, string destination, List<StartupNote> notes)
        {
            try
            {
                File.Copy(source, destination, true);
            }
            catch (Exception ex)
            {
                notes.Add(new StartupNote(LogLevel.Warning, $"Could not copy {source} to {destination}: {ex.Message}"));
            }
        }

        /// <summary>
        /// Writes out anything still buffered, such as the async sink's queue. Logging stops afterwards.
        /// </summary>
        public static void CloseAndFlush()
        {
            logger?.Dispose();
        }
    }
}
