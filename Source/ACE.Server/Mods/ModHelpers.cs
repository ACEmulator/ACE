using ACE.Server.Command;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ACE.Server.Mods
{
    public static class ModHelpers
    {
        /// <summary>
        /// Adds all commands with a CommandHandlerAttribute in an assembly to server commands
        /// </summary>
        public static void RegisterCommandHandlers(this ModContainer container, bool overrides = true)
        {
            if (container?.ModAssembly is null)
                return;

            foreach (var type in container.ModAssembly.GetTypes())
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
        /// Removes all commands with a CommandHandlerAttribute in an assembly to server commands
        /// </summary>
        public static void UnregisterCommandHandlers(this ModContainer container)
        {
            if (container?.ModAssembly is null)
                return;

            foreach (var type in container.ModAssembly.GetTypes())
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
