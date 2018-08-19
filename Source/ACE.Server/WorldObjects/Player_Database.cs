using System;
using System.Collections.ObjectModel;
using System.Threading;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public static TimeSpan PlayerSaveInterval = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets the ActionChain to save a character
        /// </summary>
        public ActionChain GetSaveChain(bool showMsg = true)
        {
            return new ActionChain(this, () => SavePlayer(showMsg));
        }

        /// <summary>
        /// Creates and Enqueues an ActionChain to save a character
        /// </summary>
        public void EnqueueSaveChain(bool showMsg = true)
        {
            GetSaveChain(showMsg).EnqueueChain();
        }

        public void SaveDatabase(bool showMsg = true)
        {
            var saveChain = GetSaveChain(showMsg);
            saveChain.EnqueueChain();
        }

        /// <summary>
        /// This will set the LastRequestedDatabaseSave to UtcNow and ChangesDetected to false.<para />
        /// If enqueueSave is set to true, DatabaseManager.Shard.SaveBiota() will be called for the biota.<para />
        /// Set enqueueSave to false if you want to perform all the normal routines for a save but not the actual save. This is useful if you're going to collect biotas in bulk for bulk saving.
        /// </summary>
        public override void SaveBiotaToDatabase(bool enqueueSave = true)
        {
            // Save the current position to persistent storage, only during the server update interval
            SetPhysicalCharacterPosition();

            base.SaveBiotaToDatabase(enqueueSave);
        }

        /// <summary>
        /// Internal save character functionality<para  />
        /// Saves the character to the persistent database. Includes Stats, Position, Skills, etc.<para />
        /// Will also save any possessions that are marked with ChangesDetected.
        /// </summary>
        private void SavePlayer(bool showMsg = true)
        {
            DatabaseManager.Shard.SaveCharacter(Character, null);

            var biotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();

            SaveBiotaToDatabase(false);
            biotas.Add((Biota, BiotaDatabaseLock));

            var allPosessions = GetAllPossessions();

            foreach (var possession in allPosessions)
            {
                if (possession.ChangesDetected)
                {
                    possession.SaveBiotaToDatabase(false);
                    biotas.Add((possession.Biota, possession.BiotaDatabaseLock));
                }
            }

            var requestedTime = DateTime.UtcNow;

            DatabaseManager.Shard.SaveBiotas(biotas, result =>
            {
                #if DEBUG
                if (Session.Player != null && showMsg)
                    Session.Network.EnqueueSend(new GameMessageSystemChat($"{Session.Player.Name} has been saved. It took {(DateTime.UtcNow - requestedTime).TotalMilliseconds:N0} ms to process the request.", ChatMessageType.Broadcast));
                #endif
            });
        }
    }
}
