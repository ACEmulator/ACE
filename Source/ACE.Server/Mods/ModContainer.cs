using ACE.Common;

using log4net;

using McMaster.NETCore.Plugins;

using System;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ACE.Server.Mods
{
    public class ModContainer
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TimeSpan RELOAD_TIMEOUT = TimeSpan.FromSeconds(3);

        public ModMetadata Meta { get; set; }
        public ModStatus Status = ModStatus.Unloaded;

        public Assembly ModAssembly { get; set; }
        public Type ModType { get; set; }
        public IHarmonyMod Instance { get; set; }

        // C:\ACE\Mods\SomeMod
        public string FolderPath { get; set; }
        // SomeMod
        public string FolderName //{ get; private set; }      
                => new DirectoryInfo(FolderPath).Name;
        // C:\ACE\Mods\SomeMod\SomeMod.dll
        public string DllPath =>
                Path.Combine(FolderPath, FolderName + ".dll");
        // C:\ACE\Mods\SomeMod\Meta.json
        public string MetadataPath =>
                Path.Combine(FolderPath, "Meta.json");
        // MyModNamespace.Mod
        public string TypeName =>
            ModAssembly.ManifestModule.ScopeName.Replace(".dll", "." + ModMetadata.TYPENAME);

        public PluginLoader Loader { get; private set; }
        private DateTime _lastChange = DateTime.Now;

        /// <summary>
        /// Sets up mod watchers for a valid mod Meta.json
        /// </summary>
        public void Initialize()
        {
            if (Meta is null)
            {
                log.Warn($"Unable to initialize.  Check Meta.json...");
                return;
            }

            Loader = PluginLoader.CreateFromAssemblyFile(
                assemblyFile: DllPath,
                isUnloadable: true,
                sharedTypes: new Type[] { },
                configure: config =>
                {
                    config.EnableHotReload = Meta.HotReload;
                    config.IsLazyLoaded = false;
                    config.PreferSharedTypes = true;
                }
            );
            Loader.Reloaded += Reload;

            log.Info($"Set up {FolderName}");
        }

        /// <summary>
        /// Loads assembly and activates an instance of the mod
        /// </summary>
        public void Enable()
        {
            if (Status == ModStatus.Active)
            {
                log.Info($"Mod is already enabled: {Meta.Name}");
                return;
            }
            if (Status == ModStatus.LoadFailure)
            {
                log.Info($"Unable to activate mod that failed to load: {Meta.Name}");
                return;
            }

            //Load assembly and create an instance if needed  (always?)
            if (!TryLoadModAssembly())
            {
                return;
            }

            if (!TryCreateModInstance())
            {
                return;
            }

            //Only mods with loaded assemblies that aren't active can be enabled
            if (Status != ModStatus.Inactive)
            {
                log.Info($"{Meta.Name} is not inactive.");
                return;
            }

            //Start mod and set status
            try
            {
                Instance?.Initialize();
                Status = ModStatus.Active;

                if (Meta.RegisterCommands)
                    this.RegisterUncategorizedCommands();

                log.Info($"Enabled mod `{Meta.Name} (v{Meta.Version})`.");
            }
            catch (Exception ex)
            {
                log.Error($"Error enabling {Meta.Name}: {ex}");
                Status = ModStatus.Inactive;    //Todo: what status?  Something to prevent reload attempts?
            }
        }

        public void RegisterCommands()
        {
            if (Meta.RegisterCommands)
                this.RegisterUncategorizedCommands();
        }

        //Todo: decide about removing the assembly?
        /// <summary>
        /// Disposes a mod and removes its chat commands
        /// </summary>
        public void Disable()
        {
            if (Status != ModStatus.Active)
                return;

            log.Info($"{FolderName} shutting down @ {DateTime.Now}");

            this.UnregisterAllCommands();

            try
            {
                Instance?.Dispose();
                Instance = null;
            }
            catch (TypeInitializationException ex)
            {
                ModManager.Log($"Failed to dispose {FolderName}: {ex.Message}", ModManager.LogLevel.Error);
            }
            Status = ModStatus.Inactive;
        }

        public void Restart()
        {
            Disable();
            Enable();
        }

        private bool TryLoadModAssembly()
        {
            if (!File.Exists(DllPath))
            {
                log.Warn($"Missing mod: {DllPath}");
                return false;
            }

            try
            {
                //Todo: check if an assembly is loaded?
                //ModAssembly = Loader.LoadAssemblyFromPath(DllPath);
                ModAssembly = Loader.LoadDefaultAssembly();

                //Safer to use the dll to get the type than using convention
                //ModType = ModAssembly.GetTypes().Where(x => x.IsAssignableFrom(typeof(IHarmonyMod))).FirstOrDefault();
                ModType = ModAssembly.GetType(TypeName);

                if (ModType is null)
                {
                    Status = ModStatus.LoadFailure;
                    log.Warn($"Missing IHarmonyMod Type {TypeName} from {ModAssembly}");
                    return false;
                }
            }
            catch (Exception e)
            {
                Status = ModStatus.LoadFailure;
                log.Error($"Failed to load mod file `{DllPath}`: {e}");
                return false;
            }

            Status = ModStatus.Inactive;
            return true;
        }

        private bool TryCreateModInstance()
        {
            try
            {
                Instance = Activator.CreateInstance(ModType) as IHarmonyMod;
                log.Info($"Created instance of {Meta.Name}");
            }
            catch (Exception ex)
            {
                Status = ModStatus.LoadFailure;
                log.Error($"Failed to create Mod instance: {Meta.Name}: {ex}");
                return false;
            }

            return true;
        }

        public void SaveMetadata()
        {
            var json = JsonSerializer.Serialize(Meta, ConfigManager.SerializerOptions);
            var info = new FileInfo(MetadataPath);

            if (!info.RetryWrite(json))
                log.Error($"Saving metadata failed: {MetadataPath}");
        }

        #region Events
        //If Loader has hot reload enabled this triggers after the assembly is loaded again (after GC)
        private void Reload(object sender, PluginReloadedEventArgs eventArgs)
        {
            var lapsed = DateTime.Now - _lastChange;
            if (lapsed < RELOAD_TIMEOUT)
            {
                log.Info($"Not reloading {FolderName}: {lapsed.TotalSeconds}/{RELOAD_TIMEOUT.TotalSeconds}");
                return;
                //Shutdown();
            }

            Restart();
            log.Info($"Reloaded {FolderName} @ {DateTime.Now} after {lapsed.TotalSeconds}/{RELOAD_TIMEOUT.TotalSeconds} seconds");
        }


        private void ModDll_Changed(object sender, FileSystemEventArgs e)
        {
            //Todo: Rethink reload in progress?
            var lapsed = DateTime.Now - _lastChange;
            if (lapsed < RELOAD_TIMEOUT)
            {
                //log.Info($"Not reloading {FolderName}: {lapsed.TotalSeconds}/{RELOAD_TIMEOUT.TotalSeconds}");
                return;
            }

            log.Info($"{FolderName} changed @ {DateTime.Now} after {lapsed.TotalMilliseconds}ms");
            _lastChange = DateTime.Now;

            Disable();
        }
        #endregion
    }

    public enum ModStatus
    {
        /// <summary>
        /// Assembly not loaded
        /// </summary>
        Unloaded,
        /// <summary>
        /// Assembly loaded but an instance is not active
        /// </summary>
        Inactive,
        /// <summary>
        /// Assembly is loaded and an instance is active
        /// </summary>
        Active,
        /// <summary>
        /// Assembly failed to load
        /// </summary>
        LoadFailure,

        //Todo: Decide on how to represent future conflicts/errors
        //NameConflict,       //Mod loaded but a higher priority mod has the same name
        //MissingDependency,  //Keeping it simple for now
        //Conflict,           //Loaded and conflict detected
    }
}
