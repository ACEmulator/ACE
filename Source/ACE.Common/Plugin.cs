using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace ACE.Common
{
    /// <summary>
    /// plugin implementation requirements
    /// </summary>
    public interface IACEPlugin
    {
        void Start(TaskCompletionSource<bool> tsc);
    }
    /// <summary>
    /// Plugin enabled in config, found, and either successfully initialized or failed to initialize.
    /// </summary>
    public class ActivatedPlugin
    {
        public ActivatedPlugin(string PluginName, string PluginFilePath, string PluginFolderPath)
        {
            this.PluginFolderPath = PluginFolderPath;
            this.PluginName = PluginName;
            this.PluginFilePath = PluginFilePath;
        }
        /// <summary>
        /// full path to the plugin's folder
        /// </summary>
        public string PluginFolderPath { get; private set; }
        /// <summary>
        /// name of the plugin from config file
        /// </summary>
        public string PluginName { get; private set; }
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
        /// exception that occured within the plugin manager while trying to resolve dependency assemblies, activate the assembly, and identify valid IACEPlugin implementors
        /// </summary>
        public Exception ResolverException { get; set; } = null;
        /// <summary>
        /// The list of library files and assemblies suggested as candidates during dependency assembly resolution requested
        /// by the assembly load context during assembly activation and type construction and initialization
        /// </summary>
        public List<Tuple<AssemblyName, string, Assembly>> AssemblyResolutionSuggestions { get; set; }
        /// <summary>
        /// DependencyContext.Default.RuntimeLibraries suggested as candidates during dependency assembly resolution requested
        /// by the assembly load context during assembly activation and type construction and initialization
        /// </summary>
        public List<Tuple<AssemblyName, RuntimeLibrary, Assembly>> RuntimeLibrarySuggestions { get; set; }
        
    }
    /// <summary>
    /// Each plugin DLL can expose multiple IACEPlugin implementing classes
    /// and can have its own threads and host full applications or merely do something during init and return.
    /// </summary>
    public class ACEPluginType
    {
        public ACEPluginType(Type Type ,TaskCompletionSource<bool> ResultOfInitTask)
        {
            this.Type = Type;
            this.ResultOfInitTask = ResultOfInitTask;
            InitTimeTaken = Stopwatch.StartNew(); // should be last in the constructor
        }
        public Type Type { get; private set; } = null;
        /// <summary>
        /// the plugin object instance, 
        /// </summary>
        public IACEPlugin Instance { get; set; } = null;
        /// <summary>
        /// True indicates sucessful initialization
        /// False indicates a fault occured during initialization
        /// Property is invalid until the result task result is set by the plugin during initialization or the plugin throws an exception during initialization
        /// </summary>
        public bool ResultOfInit { get { return ResultOfInitTask.Task.Result; } }
        /// <summary>
        /// backing store for ResultOfInit
        /// True indicates sucessful initialization
        /// False indicates a fault occured during initialization
        /// It's imperative that the plugin reliably set the task result no matter what
        /// </summary>
        public TaskCompletionSource<bool> ResultOfInitTask { get; private set; } = null;
        /// <summary>
        /// any exception that occured within init and bubbled up to the try/catch of the plugin init loop of the plugin manager
        /// </summary>
        public Exception InitException { get; set; } = null;
        /// <summary>
        /// time taken to init the plugin
        /// </summary>
        public Stopwatch InitTimeTaken { get; private set; }
    }
}
