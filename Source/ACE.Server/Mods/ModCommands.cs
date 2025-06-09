using System;

//using log4net;

using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Mods;
using HarmonyLib;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ACE.Server.Command.Handlers
{
    public static class ModCommands
    {
        enum ModCommand
        {
            List, L = List,
            Enable, E = Enable,
            Disable, D = Disable,
            Toggle, T = Toggle,
            Restart, R = Restart,
            Method, M = Method,
            Find, F = Find,
            Settings, S = Settings
        }
        static string USAGE = $"/mod {String.Join('|', Enum.GetNames(typeof(ModCommand)))}";

        [CommandHandler("mod", AccessLevel.Developer, CommandHandlerFlag.None, -1,
            "Lazy mod control")]
        public static void HandleListMods(Session session, params string[] parameters)
        {
            if (parameters.Length < 1 || !Enum.TryParse<ModCommand>(parameters[0], true, out ModCommand verb))
            {
                Log(USAGE, session);
                return;
            }

            ModContainer match = null;
            if (parameters.Length > 1)
                match = ModManager.GetModContainerByName(parameters[1]);

            if (match is null && (verb == ModCommand.Enable || verb == ModCommand.Disable || verb == ModCommand.Toggle || verb == ModCommand.Restart || verb == ModCommand.Settings))
            {
                Log(USAGE, session);
                return;
            }

            switch (verb)
            {
                case ModCommand.Enable:
                    EnableMod(session, match).GetAwaiter().GetResult();
                    return;
                case ModCommand.Disable:
                    DisableMod(session, match).GetAwaiter().GetResult();
                    return;
                case ModCommand.Restart:
                    Log($"Restarting {match.Meta.Name}", session);
                    match.Restart();
                    return;
                case ModCommand.Toggle:
                    if (match.Status == ModStatus.Inactive || match.Status == ModStatus.Unloaded)
                        EnableMod(session, match).GetAwaiter().GetResult();
                    else if (match.Status == ModStatus.Active)
                        DisableMod(session, match).GetAwaiter().GetResult();
                    return;

                //List mod status
                case ModCommand.List:
                    ModManager.ListMods(session?.Player);
                    return;

                //Prints out some information about a method and its params/types
                case ModCommand.Method:
                    if (parameters.Length < 3)
                        return;

                    var type = AccessTools.TypeByName(parameters[1]);
                    var method = parameters[2];
                    if (type is null || method is null)
                        return;
                    var mcMethod = AccessTools.FirstMethod(type, m => m.Name.Contains(method));

                    const int spacing = -40;
                    var sb = new StringBuilder($"Method {mcMethod.Name} found:");

                    foreach (var param in mcMethod.GetParameters())
                    {
                        sb.AppendLine($"Name: {param.Name,spacing}\r\nType: {param.ParameterType,spacing}\r\nDflt: {param.DefaultValue,spacing}\r\n");
                    }
                    Log(sb.ToString(), session);

                    return;

                //Full reload
                case ModCommand.Find:
                    ModManager.FindMods(true);
                    ModManager.ListMods();
                    return;

                //Lazy opening of mod settings
                case ModCommand.Settings:
                    var settingsPath = System.IO.Path.Combine(match.FolderPath, "Settings.json");

                    if (!System.IO.File.Exists(settingsPath))
                    {
                        Log($"Settings missing: {settingsPath}", session);
                        return;
                    }

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // On Windows we use explorer.exe to open the settings file
                        // because it automatically selects the file in the containing
                        // folder, giving a familiar experience to users.
                        using var settings = new Process();
                        settings.StartInfo.FileName = "explorer";
                        settings.StartInfo.Arguments = $"\"{settingsPath}\"";
                        settings.Start();
                    }
                    else
                    {
                        // For non-Windows platforms we fall back to Process.Start with
                        // UseShellExecute to let the OS decide how to open the file.
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = settingsPath,
                            UseShellExecute = true
                        });
                    }
                    return;
            }

            Log(USAGE, session);
        }

        /// <summary>
        /// Disable Mod and save choice in Metadata
        /// </summary>
        private static async Task DisableMod(Session session, ModContainer match)
        {
            // Returning Task instead of using 'async void' allows the caller to
            // await completion and observe any exceptions that occur during
            // disabling.
            Log($"Disabling {match.Meta.Name}", session);
            match.Meta.Enabled = false;
            await match.SaveMetadata();
            match.Disable();
        }

        /// <summary>
        /// Enable Mod and save choice in Metadata
        /// </summary>
        private static async Task EnableMod(Session session, ModContainer match)
        {
            // Same reasoning as DisableMod: return Task so callers can await and
            // handle failures when enabling a mod.
            Log($"Enabling {match.Meta.Name}", session);
            match.Meta.Enabled = true;
            await match.SaveMetadata();
            match.Enable();
        }

        private static void Log(string message, Session session)
        {
            if (session?.Player is not null)
                session.Player.SendMessage(message);
            Console.WriteLine(message);
        }
    }
}
