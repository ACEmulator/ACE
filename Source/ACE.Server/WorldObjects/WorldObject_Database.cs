using System;

using ACE.Database;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        protected bool ExistsInDatabase { get; private set; }

        protected DateTime LastDatabaseSave { get; set; }

        /// <summary>
        /// This variable is set to true when a change is made, and set to false after a save completed.
        /// </summary>
        public bool ChangesDetected { get; private set; }

        private void AddBiotaToDatabase()
        {
            // We assume the add will be successful
            // By setting this first, it allows other threads to queue up entity remove operations after this save operation completes
            // That way no database operations are lost between the time we tried to save and the time we actually saved.
            ExistsInDatabase = true;

            DatabaseManager.Shard.AddBiota(Biota, result =>
            {
                if (result)
                {
                    LastDatabaseSave = DateTime.UtcNow;
                    ChangesDetected = false;
                }
                else
                    // Uh oh, something went wrong...
                    ExistsInDatabase = false;
            });
        }

        public void SaveBiotaToDatabase()
        {
            if (!ExistsInDatabase)
            {
                AddBiotaToDatabase();
                return;
            }

            DatabaseManager.Shard.SaveBiota(Biota, result =>
            {
                if (result)
                {
                    LastDatabaseSave = DateTime.UtcNow;
                    ChangesDetected = false;
                }
                else
                    // Uh oh, something went wrong...
                    ExistsInDatabase = false;
            });
        }

        public void RemoveBiotaFromDatabase()
        {
            if (ExistsInDatabase && Biota.Id != 0)
            {
                ExistsInDatabase = false;
                DatabaseManager.Shard.RemoveBiota(Biota, null);
            }
        }
    }
}
