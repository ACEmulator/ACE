using System;

using ACE.Database;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public DateTime LastRequestedDatabaseSave { get; protected set; }

        /// <summary>
        /// This variable is set to true when a change is made, and set to false before a save is requested.<para />
        /// The primary use for this is to trigger save on add/modify/remove of properties.
        /// </summary>
        public bool ChangesDetected { get; protected set; }

        public void SaveBiotaToDatabase()
        {
            LastRequestedDatabaseSave = DateTime.UtcNow;
            ChangesDetected = false;

            DatabaseManager.Shard.SaveBiota(Biota, BiotaDatabaseLock, null);
        }

        public void RemoveBiotaFromDatabase()
        {
            LastRequestedDatabaseSave = DateTime.MinValue;
            ChangesDetected = true;

            DatabaseManager.Shard.RemoveBiota(Biota, BiotaDatabaseLock, null);
        }
    }
}
