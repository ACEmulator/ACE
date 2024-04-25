using ACE.Server.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ACE.Server.Mods
{
    public static class ModHelpers
    {
        /// <summary>
        /// Adds all commands with a CommandHandlerAttribute to server commands
        /// </summary>
        public static void RegisterAllCommands(this ModContainer container, bool overrides = true)
        {
            if (container?.ModAssembly is null)
                return;

            var types = container.ModAssembly.GetTypes().ToList();
            RegisterCommands(container, types, overrides);
        }
        /// <summary>
        /// Adds all commands with a CommandHandlerAttribute and not in a CommandCategory to server commands
        /// </summary>
        public static void RegisterUncategorizedCommands(this ModContainer container, bool overrides = true)
        {
            if (container?.ModAssembly is null)
                return;

            var types = container.ModAssembly.GetTypes().Where(t => !Attribute.IsDefined(t, typeof(CommandCategoryAttribute))).ToList();
            RegisterCommands(container, types, overrides);
        }
        /// <summary>
        /// Adds all commands with a CommandHandlerAttribute matching a pattern to server commands
        /// </summary>
        public static void RegisterCommandCategory(this ModContainer container, string categoryPattern, bool overrides = true)
        {
            if (container?.ModAssembly is null)
                return;

            try
            {
                Regex pattern = new(categoryPattern, RegexOptions.IgnoreCase);
                List<Type> types = new();
                foreach(var type in container.ModAssembly.GetTypes())
                {
                    var attr = type.GetCustomAttribute<CommandCategoryAttribute>();
                    var cat = attr?.Category ?? "";
                    if (pattern.IsMatch(cat))
                        types.Add(type);
                }

                RegisterCommands(container, types, overrides);
            }
            catch (Exception ex) { ModManager.Log(ex.Message, ModManager.LogLevel.Error); }
        }
        /// <summary>
        /// Adds all commands in a set of Types with a CommandHandlerAttribute to server commands
        /// </summary>
        public static void RegisterCommands(this ModContainer container, List<Type> types, bool overrides = true)
        {
            foreach (var type in types)
            {
                foreach (var method in type.GetMethods())
                {
                    foreach (var attribute in method.GetCustomAttributes<CommandHandlerAttribute>())
                    {
                        var commandHandler = new CommandHandlerInfo()
                        {
                            Handler = (CommandHandler)Delegate.CreateDelegate(typeof(CommandHandler), method),
                            Attribute = attribute
                        };

                        if (CommandManager.TryAddCommand(commandHandler, overrides))
                            ModManager.Log($"{container.Meta.Name} added command: {method.Name}");
                        else
                            ModManager.Log($"{container.Meta.Name} failed to add command: {method.Name}");
                    }
                }
            }
        }

        /// <summary>
        /// Removes all commands with a CommandHandlerAttribute to server commands
        /// </summary>
        public static void UnregisterAllCommands(this ModContainer container, bool overrides = true)
        {
            if (container?.ModAssembly is null)
                return;

            var types = container.ModAssembly.GetTypes().ToList();
            UnregisterCommands(container, types, overrides);
        }
        /// <summary>
        /// Removes all commands with a CommandHandlerAttribute and not in a CommandCategory to server commands
        /// </summary>
        public static void UnregisterUncategorizedCommands(this ModContainer container, bool overrides = true)
        {
            if (container?.ModAssembly is null)
                return;

            var types = container.ModAssembly.GetTypes().Where(t => !Attribute.IsDefined(t, typeof(CommandCategoryAttribute))).ToList();
            UnregisterCommands(container, types, overrides);
        }
        /// <summary>
        /// Removes  all commands with a CommandHandlerAttribute matching a pattern to server commands
        /// </summary>
        public static void UnregisterCommandCategory(this ModContainer container, string categoryPattern, bool overrides = true)
        {
            if (container?.ModAssembly is null)
                return;

            try
            {
                Regex pattern = new(categoryPattern, RegexOptions.IgnoreCase);
                var types = container.ModAssembly.GetTypes().Where(t => pattern.IsMatch(t.GetCustomAttribute<CommandCategoryAttribute>()?.Category)).ToList();
                UnregisterCommands(container, types, overrides);
            }
            catch (Exception ex) { ModManager.Log(ex.Message, ModManager.LogLevel.Error); }
        }
        /// <summary>
        /// Removes all commands in a set of Types with a CommandHandlerAttribute to server commands
        /// </summary>
        public static void UnregisterCommands(this ModContainer container, List<Type> types, bool overrides = true)
        {
            if (container?.ModAssembly is null)
                return;

            foreach (var type in types)
            {
                foreach (var method in type.GetMethods())
                {
                    foreach (var attribute in method.GetCustomAttributes<CommandHandlerAttribute>())
                    {
                        if (CommandManager.TryRemoveCommand(attribute.Command))
                            ModManager.Log($"{container.Meta.Name} removed command: {method.Name}");
                        else
                            ModManager.Log($"{container.Meta.Name} failed to remove command: {method.Name}");
                    }
                }
            }
        }

        //Todo: Look into proper way to retry with file locks
        //Don't like this, but hacky way of retrying for access to settings on hot loads
        /// <summary>
        /// Attempts every second for a number of tries to write to a file.
        /// </summary>
        /// <returns>True if content was successfully written to file</returns>
        public static bool RetryWrite(this FileInfo file, string content, int retryCount = 5)
        {
            try
            {
                Task.Run(async () => await file.WriteWithRetryAsync(TimeSpan.FromSeconds(1), content, retryCount));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Attempts every second for a number of tries to write to a file.
        /// </summary>
        /// <returns>True if content was successfully read from file</returns>
        public static bool RetryRead(this FileInfo file, out string content, int retryCount = 5)
        {
            content = null;
            try
            {
                var task = Task.Run<string>(async () =>
                {
                    return await file.ReadWithRetryAsync(TimeSpan.FromSeconds(1), retryCount);
                });
                content = task.Result;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Retries a number of times writing content to a file
        /// </summary>
        public static async Task WriteWithRetryAsync(this FileInfo file, TimeSpan retryDelay, string content, int retryCount = 5)
        {
            for (; ; )
            {
                try
                {
                    //Get a stream.  OpenRead only allows read share
                    await File.WriteAllTextAsync(file.FullName, content);
                    return;
                }
                //https://learn.microsoft.com/en-us/dotnet/standard/io/handling-io-errors
                catch (Exception) //when ((ex.HResult & 0x0000FFFF) == 32)
                {
                    //Eat IO exceptions while retrying
                    //ModManager.Log($"Unable to write to {file.FullName}, {retryCount} retries...");

                    if (--retryCount <= 0)
                    {
                        throw;
                    }
                }

                await Task.Delay(retryDelay);
            }
        }
        /// <summary>
        /// Retries a number of times reading the content of a file
        /// </summary>
        public static async Task<string> ReadWithRetryAsync(this FileInfo file, TimeSpan retryDelay, int retryCount = 5)
        {
            for (; ; )
            {
                try
                {
                    //Get a stream.  OpenRead only allows read share
                    StreamReader reader = new(file.OpenRead());

                    //Return stream if there's no problem
                    var content = await reader.ReadToEndAsync();
                    reader.Close();
                    return content;
                }
                //https://learn.microsoft.com/en-us/dotnet/standard/io/handling-io-errors
                catch (IOException) //when ((ex.HResult & 0x0000FFFF) == 32)
                {
                    //Eat IO exceptions while retrying
                    //ModManager.Log($"Unable to read {file.FullName}, {retryCount} retries...");

                    if (--retryCount <= 0)
                    {
                        throw;
                    }
                }

                await Task.Delay(retryDelay);
            }
        }
    }
}
