using ACE.Common;
using log4net;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace ACE.Server.Plugin
{
    /// <summary>
    /// initializes and manages plugins
    /// </summary>
    public static class PluginManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly PluginList Plugins = new PluginList();
        private static string curPlugPath = "";
        private static string curPlugNam = "";
        private static readonly List<Tuple<string, Assembly>> PluginDlls = new List<Tuple<string, Assembly>>();
        private static readonly List<Tuple<string, Assembly>> ACEDlls = new List<Tuple<string, Assembly>>();
        private static readonly List<Assembly> ReferencedAssemblies = new List<Assembly>();
        public static string PathToACEFolder { get; } = new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Directory.FullName;
        private static string DpPlugins { get; } = Path.Combine(PathToACEFolder, PluginFolderName);
        public const string PluginFolderName = "Plugins";

        //public delegate void AllPluginsInitCompletedDele();
        //public static event AllPluginsInitCompletedDele AllPluginsInitialized;

        /// <summary>
        /// make lists of all the DLLs in the main ACE dir and the plugin dirs
        /// </summary>
        private static void GatherDlls()
        {
            ACEDlls.AddRange(new DirectoryInfo(PathToACEFolder).GetFiles("*.dll", SearchOption.TopDirectoryOnly).Select(k => new Tuple<string, Assembly>(k.FullName, null)));

            DirectoryInfo[] plugDirs = new DirectoryInfo(DpPlugins).GetDirectories();

            foreach (DirectoryInfo pd in plugDirs)
            {
                if (ConfigManager.Config.Plugins.Plugins.Any(k => k.ToLower() == pd.Name.ToLower()))
                {
                    PluginDlls.AddRange(pd.GetFiles("*.dll",
                        new EnumerationOptions() { ReturnSpecialDirectories = false, RecurseSubdirectories = false, IgnoreInaccessible = true })
                        .Select(k => new Tuple<string, Assembly>(k.FullName, null)));
                }
            }
        }
        private static ActivatedPlugin CurrentPlugin = null;
        public static void Initialize()
        {
            try
            {
                if (ConfigManager.Config.Plugins.Enabled && DpPlugins != null)
                {
                    // get dependency list
                    GatherDlls();

                    // wire up the dependency resolution function
                    AssemblyLoadContext.Default.Resolving += Default_Resolving;

                    // attempt to load each plugin
                    // start plugins in the sequence seen in the plugin configuration plugin list, blocking each until initialized
                    foreach (string pl in ConfigManager.Config.Plugins.Plugins)
                    {
                        curPlugNam = Path.GetFileName(pl);
                        string fnPlDll = curPlugNam + ".dll";
                        string dpPl = Path.Combine(DpPlugins, curPlugNam);
                        string fp = Path.Combine(dpPl, fnPlDll);
                        curPlugPath = dpPl;

                        if (!Directory.Exists(dpPl))
                        {
                            log.Warn($"Plugin {curPlugNam} failure: missing plugin directory");
                            Plugins.Add(new ActivatedPlugin(curPlugNam, "", "", ""));
                            continue;
                        }

                        if (!File.Exists(fp))
                        {
                            log.Warn($"Plugin {curPlugNam} failure: missing plugin file {fp}");
                            Plugins.Add(new ActivatedPlugin(curPlugNam, "", "", ""));
                            continue;
                        }


                        CurrentPlugin = new ActivatedPlugin(curPlugNam, fnPlDll, dpPl, Path.Combine(PluginFolderName, curPlugNam));
                        log.Debug($"Loading: {CurrentPlugin.PluginFileRelativePath}");
                        Plugins.Add(CurrentPlugin);
                        try
                        {
                            CurrentPlugin.PluginAssembly = Assembly.LoadFile(fp);

                            IEnumerable<Type> types =
                                from typ in CurrentPlugin.PluginAssembly.GetTypes()
                                where typeof(IACEPlugin).IsAssignableFrom(typ)
                                select typ;

                            if (types.Count() < 1)
                            {
                                log.Warn($"Plugin {curPlugNam} failure: couldn't find any IACEPlugin interfaces.");
                                continue;
                            }

                            // TODO (low priority): add support for parallel Type start
                            foreach (Type type in types)
                            {
                                // not sure if it's necessary to enforce case sensitive exact naming scheme, so keep this commented out for now,
                                // thinking about PluginConfigManager<T> and how it uses the caller assembly name to form the file path
                                // if (!type.Name.StartsWith(curPlugNam)) { log.Info($"Type {type} is being skipped, it is not and does not belong to {curPlugNam}"); }

                                TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
                                ACEPluginType atyp = new ACEPluginType(type, tsc);
                                CurrentPlugin.Types.Add(atyp);
                                atyp.Instance = (IACEPlugin)Activator.CreateInstance(type);
                                log.Debug($"Instance created for: {type}");
                                try
                                {
                                    atyp.Instance.Start(atyp.ResultOfInitSink); // non blocking!
                                }
                                catch (Exception ex)
                                {
                                    log.Error($"Startup failure for {type}", ex);
                                    atyp.InitException = ex;
                                    tsc.SetResult(false);
                                }
                                finally
                                {
                                    tsc.Task.Wait();
                                    atyp.InitTimeTaken.Stop();
                                    if (atyp.InitException == null && !tsc.Task.Result)
                                    {
                                        log.Error($"{type} reported failure to start.");
                                    }
                                }
                            }
                            if (CurrentPlugin.Types.Any() && CurrentPlugin.Types.All(k => k.ResultOfInit))
                            {
                                CurrentPlugin.StartupSuccess = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Warn($"Plugin {curPlugNam} failure: {ex.Message}", ex);
                            CurrentPlugin.ResolverException = ex;
                        }
                        CurrentPlugin = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Plugin manager initialization failure", ex);
            }
            curPlugNam = null;
            curPlugPath = null;
            CurrentPlugin = null;
            IEnumerable<ActivatedPlugin> goodPlugins = Plugins.Where(k => k.StartupSuccess);
            if (log.IsDebugEnabled && Plugins.Any() && goodPlugins.Any())
            {
                log.Debug($"calling AllPluginsStarted for {goodPlugins.Count()} plugins");
            }

            foreach (ActivatedPlugin plug in goodPlugins)
            {
                try
                {
                    foreach (ACEPluginType typ in plug.Types)
                    {
                        TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
                        typ.Instance.AllPluginsStarted(tsc);
                        if (!tsc.Task.Wait(10000))
                        {
                            throw new Exception($"Type {typ} AllPluginsStarted implementation failed to report before 10s");
                        }
                        else if (!tsc.Task.Result)
                        {
                            throw new Exception($"Type {typ} AllPluginsStarted implementation reported failure");
                        }
                    }
                    plug.AllSuccess = true;
                }
                catch (Exception ex)
                {
                    log.Warn($"Plugin {plug.PluginName} failure: {ex.Message}", ex);
                }
            }

            goodPlugins = Plugins.Where(k => k.AllSuccess);
            if (log.IsInfoEnabled && Plugins.Any())
            {
                IEnumerable<ActivatedPlugin> badPlugins = Plugins.Except(goodPlugins);
                List<string> blurb = new List<string>() { "Plugin initialization summary:" };
                if (goodPlugins.Count() > 0)
                {
                    blurb.Add($"Success: [ {goodPlugins.Select(k => k.PluginAssembly.GetName().Name).DefaultIfEmpty().Aggregate((a, b) => a + ", " + b)} ]");
                }
                if (badPlugins.Count() > 0)
                {
                    blurb.Add($"Failure: [ {badPlugins.Select(k => k.PluginName).DefaultIfEmpty().Aggregate((a, b) => a + ", " + b)} ]");
                }
                log.Info(blurb.Aggregate((a, b) => a + " " + b));
            }
        }

        // https://stackoverflow.com/questions/40908568/assembly-loading-in-net-core
        private static Assembly Default_Resolving(AssemblyLoadContext context, AssemblyName name)
        {
            // avoid loading *.resources dlls, because of: https://github.com/dotnet/coreclr/issues/8416
            if (name.Name.EndsWith("resources"))
            {
                return null;
            }
            if (LastRequestedAssembly != null && name.FullName == LastRequestedAssembly.FullName)
            {
                if (AssemblyRequestedCount > 3)
                {
                    return null; // prevent stack overflow, resolution failed...
                }
                else
                {
                    AssemblyRequestedCount++;
                }
            }
            else
            {
                LastRequestedAssembly = name;
                AssemblyRequestedCount = 1;
            }



            // already loaded?
            Assembly[] curAsems = AppDomain.CurrentDomain.GetAssemblies();
            Assembly asem = curAsems.FirstOrDefault(k => name.FullName.StartsWith(k.GetName().Name + ","));
            if (asem != null)
            {
                CurrentPlugin?.AssemblyResolutionSuggestions.Add(new Tuple<AssemblyName, string, Assembly>(name, asem.ManifestModule.FullyQualifiedName, asem));
                return asem;
            }

            // library files
            List<Tuple<string, Assembly>> filList = null;
            Tuple<string, Assembly> fil = GetFavoredDependencyDll(name, ref filList);
            if (fil != null && !string.IsNullOrWhiteSpace(fil.Item1))
            {
                if (fil.Item2 != null)  //TEST: see if the runtime library loop always pick up on this already loaded library
                {
                    CurrentPlugin?.AssemblyResolutionSuggestions.Add(new Tuple<AssemblyName, string, Assembly>(name, fil.Item1, fil.Item2));
                    return fil.Item2;
                }
                Assembly assem = context.LoadFromAssemblyPath(fil.Item1);
                filList[filList.IndexOf(fil)] = new Tuple<string, Assembly>(filList[filList.IndexOf(fil)].Item1, assem);
                log.Debug($"Loaded {fil.Item1}");
                CurrentPlugin?.AssemblyResolutionSuggestions.Add(new Tuple<AssemblyName, string, Assembly>(name, fil.Item1, assem));
                return assem;
            }

            // default runtime libraries and their dependencies
            IReadOnlyList<RuntimeLibrary> dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (RuntimeLibrary library in dependencies)
            {
                if (library.Name == name.Name || library.Dependencies.Any(d => d.Name.StartsWith(name.Name)))
                {

                    Assembly assem = context.LoadFromAssemblyName(new AssemblyName(library.Name));
                    CurrentPlugin?.RuntimeLibrarySuggestions.Add(new Tuple<AssemblyName, RuntimeLibrary, Assembly>(name, library, assem));
                    return assem;
                }
            }

            log.Fatal($"dependency missing: {name}");
            return null;
        }

        private static AssemblyName LastRequestedAssembly = null;
        private static ushort AssemblyRequestedCount = 0;

        /// <summary>
        /// TODO: rotate through our DLLs, don't just keep suggesting the first match
        /// TODO: better failure detection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileList"></param>
        /// <returns></returns>
        private static Tuple<string, Assembly> GetFavoredDependencyDll(AssemblyName name, ref List<Tuple<string, Assembly>> fileList)
        {
            // first, locate all copies of the required library in ACE folder
            List<Tuple<string, Assembly>> foundDlls = ACEDlls.Where(k => Path.GetFileNameWithoutExtension(k.Item1) == name.Name).ToList();
            if (foundDlls.Count < 1)
            {
                // fallback and favor this plugin's folder
                foundDlls = PluginDlls.Where(k => Path.GetDirectoryName(k.Item1) == curPlugPath && Path.GetFileNameWithoutExtension(k.Item1) == name.Name).ToList();
                if (foundDlls.Count < 1)
                {
                    // finally, fallback to the dlls in the other plugins' directories
                    foundDlls = PluginDlls.Where(k => Path.GetDirectoryName(k.Item1) != curPlugPath && Path.GetFileNameWithoutExtension(k.Item1) == name.Name).ToList();
                    if (foundDlls.Count < 1)
                    {
                        return null;
                    }
                    else { fileList = PluginDlls; }
                }
                else { fileList = PluginDlls; }
            }
            else { fileList = ACEDlls; }
            return foundDlls.First();  // sort this?
        }
    }
}
