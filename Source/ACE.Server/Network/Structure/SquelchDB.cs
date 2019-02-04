using System.Collections.Generic;
using System.IO;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.Structure
{
    public class SquelchDB
    {
        /// <summary>
        /// Account squelches
        /// </summary>
        public Dictionary<string, uint> Accounts;

        /// <summary>
        /// Character squelches
        /// </summary>
        public Dictionary<ObjectGuid, SquelchInfo> Characters;

        /// <summary>
        /// Global squelches
        /// </summary>
        public SquelchInfo Globals;

        /// <summary>
        /// Constructs a new empty Squelch database
        /// </summary>
        public SquelchDB()
        {
            Accounts = new Dictionary<string, uint>();
            Characters = new Dictionary<ObjectGuid, SquelchInfo>();
            Globals = new SquelchInfo();
        }

        /// <summary>
        /// Merges accounts + character for sending to clients
        /// </summary>
        public Dictionary<ObjectGuid, SquelchInfo> CharactersPlus
        {
            get
            {
                var charactersPlus = new Dictionary<ObjectGuid, SquelchInfo>();

                foreach (var character in Characters)
                    charactersPlus.Add(character.Key, new SquelchInfo(character.Value));

                foreach (var account in Accounts)
                {
                    var guid = new ObjectGuid(account.Value);
                    var accountPlayer = PlayerManager.FindByGuid(guid);
                    if (accountPlayer == null)
                        continue;

                    if (!charactersPlus.TryGetValue(guid, out var existing))
                        charactersPlus.Add(guid, new SquelchInfo(ChatMessageType.AllChannels, accountPlayer.Name, true));
                    else
                        existing.Account = true;
                }
                return charactersPlus;
            }
        }

        /// <summary>
        /// Returns TRUE if the input player is filtered by this player
        /// </summary>
        public bool Contains(Player player, ChatMessageType messageType = ChatMessageType.AllChannels)
        {
            if (Accounts.ContainsKey(player.Session.Account))
                return true;

            if (Characters.TryGetValue(player.Guid, out var squelchInfo))
                return true;

            return false;
        }
    }

    public static class SquelchDBExtensions
    {
        public static void Write(this BinaryWriter writer, SquelchDB squelches)
        {
            //writer.Write(squelches.Accounts);
            writer.Write(new Dictionary<string, uint>());   // this part of the structure is always empty in retail pcaps, even with account squelches
                                                            // perhaps for security reasons, always send account squelches in the characters section + account bool?
            writer.Write(squelches.CharactersPlus);
            writer.Write(squelches.Globals);
        }

        public static void Write(this BinaryWriter writer, Dictionary<string, uint> accountHash)
        {
            PackableHashTable.WriteHeader(writer, accountHash.Count);
            foreach (var kvp in accountHash)
            {
                writer.WriteString16L(kvp.Key);
                writer.Write(kvp.Value);
            }
        }

        public static void Write(this BinaryWriter writer, Dictionary<ObjectGuid, SquelchInfo> characterHash)
        {
            PackableHashTable.WriteHeader(writer, characterHash.Count);
            foreach (var kvp in characterHash)
            {
                writer.Write(kvp.Key.Full);
                writer.Write(kvp.Value);
            }
        }
    }
}
