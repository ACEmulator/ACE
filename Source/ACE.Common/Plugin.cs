using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ACE.Common
{
    /// <summary>
    /// plugin implementation requirements
    /// </summary>
    public interface IACEPlugin
    {
        void Start(TaskCompletionSource<bool> StartedSink);
        void AllPluginsStarted(TaskCompletionSource<bool> AllPluginsStartedSink);
    }
    /// <summary>
    /// Plugin enabled in config, found, and either successfully initialized or failed to initialize.
    /// </summary>
    public class ActivatedPlugin
    {
        public ActivatedPlugin(string PluginName, string PluginFilePath, string PluginFolderPath, string relPath)
        {
            this.PluginFolderPath = PluginFolderPath;
            this.PluginName = PluginName;
            this.PluginFilePath = PluginFilePath;
            PluginRelativeFolderPath = relPath;
            PluginFileRelativePath = Path.Combine(PluginRelativeFolderPath, PluginName + ".dll");
        }
        /// <summary>
        /// relative (to ACE base path) to the plugin's folder
        /// </summary>
        public string PluginRelativeFolderPath { get; private set; }
        /// <summary>
        /// full path to the plugin's folder
        /// </summary>
        public string PluginFolderPath { get; private set; }
        /// <summary>
        /// name of the plugin from config file
        /// </summary>
        public string PluginName { get; private set; }
        /// <summary>
        /// relative (to ACE base path) to the main plugin DLL
        /// </summary>
        public string PluginFileRelativePath { get; private set; } = null;
        /// <summary>
        /// full path to the main plugin DLL
        /// </summary>
        public string PluginFilePath { get; private set; } = null;
        /// <summary>
        /// This references the assembly or assembly part that was loaded from the main DLL
        /// The main DLL is named &lt;PluginName&gt;.dll and should be located in /Plugins/&lt;PluginName&gt;
        /// </summary>
        public Assembly PluginAssembly { get; set; } = null;
        /// <summary>
        /// The exposed classes in the assembly that implement IACEPlugin
        /// </summary>
        public List<ACEPluginType> Types { get; set; } = new List<ACEPluginType>();
        /// <summary>
        /// True indicates successful initialization
        /// False indicates a fault occured during outer initialization
        /// </summary>
        public bool StartupSuccess { get; set; } = false;
        /// <summary>
        /// False indicates there was either a failed init or a good init but a failed AllPluginsStarted call
        /// </summary>
        public bool AllSuccess { get; set; } = false;
        /// <summary>
        /// exception that occured within the plugin manager while trying to resolve dependency assemblies, activate the assembly, and identify valid IACEPlugin implementors
        /// </summary>
        public Exception ResolverException { get; set; } = null;
        /// <summary>
        /// The list of library files and assemblies suggested as candidates during dependency assembly resolution requested
        /// by the assembly load context during assembly activation and type construction and initialization
        /// </summary>
        public List<Tuple<AssemblyName, string, Assembly>> AssemblyResolutionSuggestions { get; set; } = new List<Tuple<AssemblyName, string, Assembly>>();
        /// <summary>
        /// DependencyContext.Default.RuntimeLibraries suggested as candidates during dependency assembly resolution requested
        /// by the assembly load context during assembly activation and type construction and initialization
        /// </summary>
        public List<Tuple<AssemblyName, RuntimeLibrary, Assembly>> RuntimeLibrarySuggestions { get; set; } = new List<Tuple<AssemblyName, RuntimeLibrary, Assembly>>();

    }
    /// <summary>
    /// Each plugin DLL can expose multiple IACEPlugin implementing classes
    /// and can have its own threads and host full applications or merely do something during init and return.
    /// </summary>
    public class ACEPluginType
    {
        public ACEPluginType(Type Type, TaskCompletionSource<bool> ResultOfInitSink)
        {
            this.Type = Type;
            this.ResultOfInitSink = ResultOfInitSink;
            InitTimeTaken = Stopwatch.StartNew(); // should be last in the constructor
        }
        public Type Type { get; private set; } = null;
        /// <summary>
        /// the plugin object instance,
        /// </summary>
        public IACEPlugin Instance { get; set; } = null;
        /// <summary>
        /// True indicates successful initialization
        /// False indicates a fault occured during initialization
        /// Property is invalid until the result task result is set by the plugin during initialization or the plugin throws an exception during initialization
        /// </summary>
        public bool ResultOfInit => ResultOfInitSink.Task.Result;
        /// <summary>
        /// backing store for ResultOfInit passed to IACEPlugin type constructors
        /// True indicates successful initialization
        /// False indicates a fault occured during initialization
        /// It's imperative that the plugin reliably set the task result no matter what
        /// </summary>
        public TaskCompletionSource<bool> ResultOfInitSink { get; private set; } = null;
        /// <summary>
        /// any exception that occured within init and bubbled up to the try/catch of the plugin init loop of the plugin manager
        /// </summary>
        public Exception InitException { get; set; } = null;
        /// <summary>
        /// time taken to init the plugin
        /// </summary>
        public Stopwatch InitTimeTaken { get; private set; }
    }
    /// <summary>
    /// List of activated plugins
    /// </summary>
    public class PluginList : List<ActivatedPlugin>
    {
        public ActivatedPlugin this[string pluginName]
        {
            get
            {
                if (pluginName == null)
                {
                    return null;
                }
                pluginName = pluginName.ToLower();
                foreach (ActivatedPlugin plugin in this)
                {
                    if (plugin.PluginName.ToLower() == pluginName)
                    {
                        return plugin;
                    }
                }
                return null;
            }
        }
        public ActivatedPlugin this[IACEPlugin plug]
        {
            get
            {
                if (plug == null)
                {
                    return null;
                }
                foreach (ActivatedPlugin plugin in this)
                {
                    if (plugin.Types.Any(k => k.Instance == plug))
                    {
                        return plugin;
                    }
                }
                return null;
            }
        }
    }
}
