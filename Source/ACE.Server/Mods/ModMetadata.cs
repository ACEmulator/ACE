using System.Text.Json.Serialization;

namespace ACE.Server.Mods
{
    public class ModMetadata
    {
        [JsonIgnore]
        public const string FILENAME = "Meta.json";
        [JsonIgnore]
        public const string TYPENAME = "Mod";

        public string Name { get; set; } = "SomeMod";
        public string Author { get; set; } = "";
        public string Description { get; set; }
        public string Version { get; set; } = "1.0";
        public uint Priority { get; set; } = 0u;

        /// <summary>
        /// Determines whether mod is patched on load.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Reload the mod when the assembly changes
        /// </summary>
        public bool HotReload { get; set; } = true;

        /// <summary>
        /// Loads/unloads methods with CommandHandler attributes
        /// </summary>
        public bool RegisterCommands { get; set; } = true;

        #region Requirements/Conflicts
        ////Todo
        ///// <summary>
        ///// Mods that must be available
        ///// </summary>
        //public IList<ModMetadata> Dependencies { get; set; }

        ///// <summary>
        ///// 
        ///// Mods that cannot be available
        ///// </summary>
        //public IList<ModMetadata> Conflicts { get; set; }

        ///// <summary>
        ///// Server requirements
        ///// </summary>
        //public string ACEVersion { get; set; } = "0.0"; 
        #endregion
    }
}
