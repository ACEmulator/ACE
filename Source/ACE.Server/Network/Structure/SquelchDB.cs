using System.Collections.Generic;
using System.IO;

using log4net;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.Network.Structure
{
    public class SquelchDB
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Account squelches
        /// 
        /// This is defined by the network protocol, but appears to have always been empty in retail pcaps (possibly for security reasons?)
        /// 
        /// When sending the SquelchDB to the player, the account squelches were sent in the Characters table below as SequelchInfo.Account=true
        ///
        /// Key: not sure if this was account name, or character name
        /// Index: not sure if this was account id, or character id
        /// </summary>
        public Dictionary<string, uint> Accounts;

        /// <summary>
        /// Character squelches
        ///
        /// Even though this is called Characters, it contains both the Character and Account squelches (denoted by SquelchInfo.Account)
        /// </summary>
        public Dictionary<uint, SquelchInfo> Characters;

        /// <summary>
        /// Global squelches
        /// </summary>
        public SquelchInfo Globals;

        /// <summary>
        /// Constructs a new SquelchDB for network sending
        /// </summary>
        public SquelchDB(List<CharacterPropertiesSquelch> squelches, SquelchMask globals)
        {
            Accounts = new Dictionary<string, uint>();
            Characters = new Dictionary<uint, SquelchInfo>();
            Globals = new SquelchInfo();

            foreach (var squelch in squelches)
            {
                var squelchPlayer = PlayerManager.FindByGuid(squelch.SquelchCharacterId);
                if (squelchPlayer == null)
                {
                    log.Warn($"BuildSquelchDB(): couldn't find character {squelch.SquelchCharacterId:X8}");
                    continue;
                }

                if (squelch.SquelchAccountId == 0)
                {
                    // chracter squelch
                    var squelchInfo = new SquelchInfo((SquelchMask)squelch.Type, squelchPlayer.Name, false);

                    Characters.Add(squelch.SquelchCharacterId, squelchInfo);
                }
                else
                {
                    // account squelch
                    Accounts.Add(squelchPlayer.Account.AccountName, squelchPlayer.Guid.Full);
                }
            }

            // global squelch
            if (globals != SquelchMask.None)
                Globals.Filters.Add(globals);
        }

        /// <summary>
        /// Merges accounts + character for sending to clients
        /// </summary>
        public Dictionary<uint, SquelchInfo> CharactersPlus
        {
            get
            {
                var charactersPlus = new Dictionary<uint, SquelchInfo>(Characters);

                foreach (var account in Accounts)
                {
                    var guid = account.Value;
                    var accountPlayer = PlayerManager.FindByGuid(guid);
                    if (accountPlayer == null)
                        continue;

                    if (!charactersPlus.TryGetValue(guid, out var existing))
                        charactersPlus.Add(guid, new SquelchInfo(SquelchMask.AllChannels, accountPlayer.Name, true));
                    else
                        existing.Account = true;
                }
                return charactersPlus;
            }
        }

        /// <summary>
        /// Returns TRUE if the input player is filtered by this player
        /// </summary>
        public bool Contains(WorldObject source, ChatMessageType messageType = ChatMessageType.AllChannels)
        {
            // ensure this channel can be squelched
            if (messageType != ChatMessageType.AllChannels && !SquelchManager.IsLegalChannel(messageType))
                return false;

            var squelchMask = messageType.ToMask();

            if (source is Player player)
            {
                // check account squelches

                // the client forces account squelches to be AllMessages,
                // so for these, channel mask is not required

                if (Accounts.ContainsKey(player.Session.Account))
                    return true;

                // check character squelches
                Characters.TryGetValue(player.Guid.Full, out var squelchInfo);

                if (squelchInfo != null && squelchInfo.Filters[0].HasFlag(squelchMask))
                    return true;
            }

            // check global squelches
            if (Globals.Filters.Count > 0 && Globals.Filters[0].HasFlag(squelchMask))
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
            // unused in retail
            PackableHashTable.WriteHeader(writer, 0, 0);

            /*PHashTable.WriteHeader(writer, accountHash.Count);    // verify

            foreach (var kvp in accountHash)
            {
                writer.WriteString16L(kvp.Key);
                writer.Write(kvp.Value);
            }*/
        }

        // retail used either 32 or 128 here, but i could find no fully consistent pattern to discern them

        // seems to be 128 in client constructor?
        public static HashComparer HashComparer = new HashComparer(32);

        public static void Write(this BinaryWriter writer, Dictionary<uint, SquelchInfo> characterHash)
        {
            PackableHashTable.WriteHeader(writer, characterHash.Count, HashComparer.NumBuckets);

            var sorted = new SortedDictionary<uint, SquelchInfo>(characterHash, HashComparer);

            foreach (var kvp in sorted)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value);
            }
        }
    }
}
