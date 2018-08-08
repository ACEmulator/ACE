using System;

using ACE.Database;
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

        public void SaveDatabase(bool showMsg = true)
        {
            var saveChain = GetSaveChain(showMsg);
            saveChain.EnqueueChain();
        }

        /// <summary>
        /// Internal save character functionality<para  />
        /// Saves the character to the persistent database. Includes Stats, Position, Skills, etc.<para />
        /// Will also save any possessions that are marked with ChangesDetected.
        /// </summary>
        private void SavePlayer(bool showMsg = true)
        {
            LastRequestedDatabaseSave = DateTime.UtcNow;

            // Save the current position to persistent storage, only during the server update interval
            SetPhysicalCharacterPosition();

            SaveBiotaToDatabase();

            var allPosessions = GetAllPossessions();

            foreach (var possession in allPosessions)
            {
                if (possession.ChangesDetected)
                    possession.SaveBiotaToDatabase();
            }

            DatabaseManager.Shard.SaveCharacter(Character, null);

            #if DEBUG
            if (Session.Player != null && showMsg)
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{Session.Player.Name} has been saved.", ChatMessageType.Broadcast));
            #endif
        }
    }
}
