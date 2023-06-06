using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ACE.Server.Mods
{
    public static class ModManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ModPath { get; } = ACE.Common.ConfigManager.Config.Server.ModsDirectory ??
                Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Mods");

        /// <summary>
        /// Mods with at least metadata loaded
        /// </summary>
        private static List<ModContainer> Mods { get; set; } = new();

        #region Init / Shutdown
        public static void Initialize()
        {
            FindMods();
        }

        internal static void Shutdown()
        {
            //Todo: consider 
            DisableAllMods();
        }
        #endregion

        #region Load Metadata
        /// <summary>
        /// Finds all valid mods in the mod directory and attempts to load them.
        /// </summary>
        public static void FindMods()
        {
            //if (ACE.Common.ConfigManager.Config.Server.ModsDirectory is null)
            //    log.Warn($"You are missing the ModsDirectory setting in your Config.js.  Defaulting to:\r\n{ModPath}");

            if (!Directory.Exists(ModPath))
            {
                try
                {
                    Directory.CreateDirectory(ModPath);
                    log.Info($"Created mod folder at:\r\n{ModPath}");
                }
                catch (Exception ex)
                {
                    log.Error($"Failed to create mod folder:\r\n{ModPath}\n{ex}");
                    return;
                }
            }

            Mods = LoadMods(ModPath);
            Mods = Mods.OrderByDescending(x => x.Meta.Priority).ToList();

            //Todo: Filter out bad mods here or when loading entries?
            //CheckDuplicateNames(_mods);

            EnableMods(Mods);

            ListMods();
        }

        /// <summary>
        /// Shuts down existing mods recreates ModContainers
        /// </summary>
        /// <param name="directory"></param>
        private static List<ModContainer> LoadMods(string directory, bool unpatch = true)
        {
            //Todo: decide if this should always be done?
            if (unpatch)
            {
                DisableAllMods();
            }

            var entries = LoadAllMetadata(directory);
            foreach (var entry in entries)
            {
                entry.Initialize();
            }
            return entries;
        }

        /// <summary>
        /// Loads all valid metadata from folders in a given directory as ModContainer
        /// </summary>
        /// <param name="directory"></param>
        private static List<ModContainer> LoadAllMetadata(string directory)
        {
            var loadedMods = new List<ModContainer>();
            var directories = Directory.GetDirectories(directory);

            //Check for missing and shut them down
            //foreach (var mod in Mods.Where(x => !directories.Contains(x.FolderPath))) {
            //    log.Info($"Shutting down mod {mod.ModMetadata.Name} with missing folder:\r\n\t{mod.FolderPath}");
            //    mod.Shutdown();
            //}
            //Mods.RemoveAll(x => !directories.Contains(x.FolderPath));

            //Check already loaded?
            //Structure is /<ModDir>/<AssemblyName>/<AssemblyName.dll> and Meta.json
            foreach (var modDir in directories)
            {
                var metadataPath = Path.Combine(modDir, ModMetadata.FILENAME);

                if (!TryLoadModContainer(metadataPath, out var container))
                {
                    continue;
                }

                loadedMods.Add(container);
            }

            return loadedMods;
        }

        /// <summary>
        /// Loads metadata from specified ..\Meta.json file.  Fails if missing or invalid.
        /// </summary>
        /// <param name="metadataPath"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        private static bool TryLoadModContainer(string metadataPath, out ModContainer container)
        {
            container = null;

            if (!File.Exists(metadataPath))
            {
                //Log missing metadata
                log.Warn($"Metadata not found at: {metadataPath}");
                return false;
            }

            try
            {
                var metadata = JsonConvert.DeserializeObject<ModMetadata>(File.ReadAllText(metadataPath));

                container = new ModContainer()
                {
                    Meta = metadata,
                    FolderPath = Path.GetDirectoryName(metadataPath),    //Todo: would dll/metadata path make more sense?
                };

                return true;
            }
            catch (Exception ex)
            {
                log.Error($"Unable to deserialize mod metadata from: {metadataPath}\n{ex}");
                return false;
            }
        }

        //private static void CheckDuplicateNames(List<ModContainer> mods)
        //{
        //    foreach (var group in mods.OrderByDescending(x => x.ModMetadata.Priority).GroupBy(m => m.ModMetadata.Name))
        //    {
        //        //First is highest priority mod with a name, flag any others
        //        foreach (var mod in group.Skip(1))
        //        {
        //            log.Error($"Duplicate mod found: {mod.ModMetadata.Name}");
        //            mod.Status = ModStatus.NameConflict;
        //        }
        //    }
        //}
        #endregion

        #region Enable / Disable
        private static void EnableMods(List<ModContainer> containers)
        {
            foreach (var container in containers.Where(m => m.Meta.Enabled && (m.Status == ModStatus.Inactive || m.Status == ModStatus.Unloaded)))
            {
                container.Enable();
            }
        }

        public static void EnableModByName(string modName)
        {
            var mod = Mods.Where(x => x.Meta.Name.Contains(modName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            mod.Enable();
        }

        public static void DisableAllMods()
        {
            foreach (var container in Mods)
            {
                container.Disable();
            }
        }

        public static void DisableModByPath(string modPath)
        {
            var container = Mods.Where(x => x.FolderPath == modPath).FirstOrDefault();

            container?.Disable();
        }

        public static void DisableModByName(string modName)
        {
            var container = Mods.Where(x => x.Meta.Name.Contains(modName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            container?.Disable();
        }
        #endregion

        #region Helpers
        public static void ListMods(Player player = null)
        {
            var sb = new StringBuilder();
            if (Mods.Count < 1)
                sb.AppendLine("No mods to display.");
            else
            {
                sb.AppendLine($"Displaying mods ({Mods.Count}):");
                foreach (var mod in Mods)
                {
                    var meta = mod.Meta;
                    sb.AppendLine($"{meta.Name} is {(meta.Enabled ? "Enabled" : "Disabled")}");
                    sb.AppendLine($"\tSource: {mod.FolderPath}");
                    sb.AppendLine($"\tStatus: {mod.Status}");
                }
            }

            log.Info(sb);
            player?.SendMessage(sb.ToString());
        }

        public enum LogLevel
        {
            Debug,
            Error,
            Fatal,
            Info,
            Warn
        }

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    log.Debug(message);
                    break;
                case LogLevel.Error:
                    log.Error(message);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(message);
                    break;
                case LogLevel.Info:
                    log.Info(message);
                    break;
                case LogLevel.Warn:
                    log.Warn(message);
                    break;
                default:
                    log.Info(message);
                    break;
            };
        }

        public static void Message(string name, string message)
        {
            var player = PlayerManager.FindByName(name, out bool online);
            if (online)
            {
                ((Player)player).SendMessage(message);
            }
        }

        public static string GetFolder(this IHarmonyMod mod)
        {
            var match = Mods.Where(x => x.Instance == mod).FirstOrDefault();
            return match is null ? "" : match.FolderPath;
        }
        public static ModContainer GetModContainer(this IHarmonyMod mod)
        {
            var match = Mods.Where(x => x.Instance == mod).FirstOrDefault();

            return match;
        }

        public static ModContainer GetModContainerByName(string name, bool allowPartial = true) => allowPartial ?
            Mods.Where(x => x.Meta.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() :
        Mods.Where(x => x.Meta.Name == name).FirstOrDefault();

        public static ModContainer GetModContainerByPath(string path) =>
            Mods.Where(x => x.FolderPath == path).FirstOrDefault();
        #endregion
    }
}
