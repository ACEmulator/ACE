using ACE.Common;
using log4net;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace ACE.Server.Managers
{
    public static class PluginManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly List<ActivatedPlugin> Plugins = new List<ActivatedPlugin>();
        private static string curPlugPath = "";
        private static string curPlugNam = "";
        private static readonly List<Tuple<string, Assembly>> PluginDlls = new List<Tuple<string, Assembly>>();
        private static readonly List<Tuple<string, Assembly>> ACEDlls = new List<Tuple<string, Assembly>>();
        private static readonly List<Assembly> ReferencedAssemblies = new List<Assembly>();
        private static string DpACEBase { get; } = new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Directory.FullName;
        private static string DpPlugins { get; } = Path.Combine(DpACEBase, "Plugins");

        /// <summary>
        /// make lists of all the DLLs in the main ACE dir and the plugin dirs
        /// </summary>
        private static void GatherDlls()
        {
            ACEDlls.AddRange(new DirectoryInfo(DpACEBase).GetFiles("*.dll", SearchOption.TopDirectoryOnly).Select(k => new Tuple<string, Assembly>(k.FullName, null)));

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
                            log.Warn($"Plugin {curPlugNam} failed, missing plugin directory");
                            continue;
                        }

                        if (!File.Exists(fp))
                        {
                            log.Warn($"Plugin {curPlugNam} failed, missing plugin file {fp}");
                            continue;
                        }

                        log.Info($"Plugin {curPlugNam} Loading, path: {fp}");
                        CurrentPlugin = new ActivatedPlugin(curPlugNam, fnPlDll, dpPl);
                        Plugins.Add(CurrentPlugin);
                        try
                        {
                            CurrentPlugin.PluginAssembly = Assembly.LoadFile(fp);

                            IEnumerable<Type> types =
                                from typ in CurrentPlugin.PluginAssembly.GetTypes()
                                where typeof(IACEPlugin).IsAssignableFrom(typ)
                                select typ;

                            foreach (Type type in types)
                            {
                                TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
                                ACEPluginType atyp = new ACEPluginType(type, tsc);
                                CurrentPlugin.Types.Add(atyp);
                                atyp.Instance = (IACEPlugin)Activator.CreateInstance(type);
                                log.Info($"Plugin {type} instance Created");
                                try
                                {
                                    atyp.Instance.Start(atyp.ResultOfInitSink); // non blocking!
                                }
                                catch (Exception ex)
                                {
                                    log.Error($"Plugin {curPlugNam} startup failed", ex);
                                    atyp.InitException = ex;
                                    tsc.SetResult(false);
                                }
                                finally
                                {
                                    tsc.Task.Wait();
                                    atyp.InitTimeTaken.Stop();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Warn($"Plugin {curPlugNam} failed", ex);
                            CurrentPlugin.ResolverException = ex;
                        }
                        CurrentPlugin = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Plugin manager failed to initialize", ex);
            }
            curPlugNam = null;
            curPlugPath = null;
            IEnumerable<ActivatedPlugin> goodPlugins = Plugins.Where(k => k.Types.All(j => j.ResultOfInit));
            if (goodPlugins.Count() < 1)
            {
                return;
            }
            log.Info($"{goodPlugins.Select(k => k.PluginAssembly.GetName().Name).DefaultIfEmpty().Aggregate((a, b) => a + "," + b)} started");
        }

        // https://stackoverflow.com/questions/40908568/assembly-loading-in-net-core
        private static Assembly Default_Resolving(AssemblyLoadContext context, AssemblyName name)
        {
            // avoid loading *.resources dlls, because of: https://github.com/dotnet/coreclr/issues/8416
            if (name.Name.EndsWith("resources"))
            {
                return null;
            }
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
                log.Info($"Loaded {fil.Item1}");
                CurrentPlugin?.AssemblyResolutionSuggestions.Add(new Tuple<AssemblyName, string, Assembly>(name, fil.Item1, assem));
                return assem;
            }
            log.Fatal($"dependency missing: {name}");
            return null;
        }
        private static Tuple<string, Assembly> GetFavoredDependencyDll(AssemblyName name, ref List<Tuple<string, Assembly>> fileList)
        {
            // TO-DO: this needs to keep track of which ones are failing, so it can suggest the next if there are multiple matches, track candidates and return null upon exhaustion or there will be stack overflow

            // first, locate all copies of the required library in ACE folder
            List<Tuple<string, Assembly>> foundDlls = ACEDlls.Where(k => Path.GetFileNameWithoutExtension(k.Item1) == name.Name).ToList();
            if (foundDlls.Count < 1)
            {
                // fallback and favor this plugin's folder
                foundDlls = PluginDlls.Where(k => Path.GetDirectoryName(k.Item1) == curPlugPath && Path.GetFileNameWithoutExtension(k.Item1) == name.Name).ToList();
                if (foundDlls.Count < 1)
                {
                    // finally, fallback to the dlls in the other plugins' directories
                    foundDlls = PluginDlls.Except(foundDlls).Where(k => Path.GetDirectoryName(k.Item1) != curPlugPath && Path.GetFileNameWithoutExtension(k.Item1) == name.Name).ToList();
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
