
namespace ACE.Common
{
    /// <summary>
    /// This section can trigger events that may happen before the world starts up, or after it shuts down
    /// The shard should be in a disconnected state from any active ACE world
    /// </summary>
    public class OfflineConfiguration
    {
        /// <summary>
        /// Purge characters that have been deleted longer than PruneDeletedCharactersDays
        /// These characters, and their associated biotas, will be deleted permanantly!
        /// </summary>
        public bool PurgeDeletedCharacters { get; set; } = false;

        /// <summary>
        /// Number of days a character must have been deleted for before eligible for purging
        /// </summary>
        public int PurgeDeletedCharactersDays { get; set; } = 30;

        /// <summary>
        /// This will purge biotas that are completely disconnected from the world
        /// These may have been items that were never deleted properly, items that were given to the town crier before delete was implemented, etc...
        /// This can be time consuming so it's not something you would have set to true for every server startup. You might run this once every few months
        /// </summary>
        public bool PurgeOrphanedBiotas { get; set; } = false;

        /// <summary>
        /// Prune deleted characters from all friend lists
        /// </summary>
        public bool PruneDeletedCharactersFromFriendLists { get; set; } = true;

        /// <summary>
        /// Prune deleted objects from all shortcut bars
        /// </summary>
        public bool PruneDeletedObjectsFromShortcutBars { get; set; } = false;

        /// <summary>
        /// Prune deleted characters from all squelch lists, excluding those used to squelch entire accounts
        /// </summary>
        public bool PruneDeletedCharactersFromSquelchLists { get; set; } = false;

        /// <summary>
        /// Automatically check for and update to latest available world database
        /// </summary>
        public bool AutoUpdateWorldDatabase { get; set; } = false;

        /// <summary>
        /// Automatically check for updated server binaries
        /// </summary>
        public bool AutoServerUpdateCheck { get; set; } = true;

        /// <summary>
        /// After updating to latest world database, automatically import further customizations
        /// AutoUpdateWorldDatabase must be true for this option to be used
        /// SQL files will be executed given the sort order of the full paths of the files
        /// </summary>
        public bool AutoApplyWorldCustomizations { get; set; } = false;

        /// <summary>
        /// When AutoApplyWorldCustomizations is set to true, the auto apply process will search for
        /// all .sql files in the following directories.
        /// This process will still use ./Content by default, or the config_properties_string
        /// value for 'content_folder' if it exists
        /// </summary>
        public string[] WorldCustomizationAddedPaths { get; set; } = { };

        /// <summary>
        /// When retrieving a file list of .sql files in the AutoApplyWorldCustomizations process
        /// this will cause the file search to retrieve all files recursively from each directory
        /// </summary>
        public bool RecurseWorldCustomizationPaths { get; set; } = false;

        /// <summary>
        /// Automatically apply new updates to databases upon startup if they haven't yet been applied
        /// </summary>
        public bool AutoApplyDatabaseUpdates { get; set; } = false;
    }
}
