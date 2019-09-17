using System;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.Structure;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects.Managers
{
    public class SquelchManager
    {
        /// <summary>
        /// The player who owns these squelches
        /// </summary>
        public Player Player;

        /// <summary>
        /// The SquelchDB contains the account and character squelches,
        /// in the network protocol Dictionary format
        /// </summary>
        public SquelchDB Squelches;

        /// <summary>
        /// Constructs a new SquelchManager for a Player
        /// </summary>
        /// <param name="player"></param>
        public SquelchManager(Player player)
        {
            Player = player;

            UpdateSquelchDB();
        }

        /// <summary>
        /// Returns TRUE if this player has squelched any accounts / characters / globals
        /// </summary>
        public bool HasSquelches => Squelches.Accounts.Count > 0 || Squelches.Characters.Count > 0 || Squelches.Globals.Filters.Count > 0;

        /// <summary>
        /// Returns TRUE if this channel can be squelched
        /// </summary>
        public static bool IsLegalChannel(ChatMessageType channel)
        {
            switch (channel)
            {
                case ChatMessageType.Speech:
                case ChatMessageType.Tell:
                case ChatMessageType.Combat:
                case ChatMessageType.Magic:
                case ChatMessageType.Emote:
                case ChatMessageType.Appraisal:
                case ChatMessageType.Spellcasting:
                case ChatMessageType.Allegiance:
                case ChatMessageType.Fellowship:
                case ChatMessageType.CombatEnemy:
                case ChatMessageType.CombatSelf:
                case ChatMessageType.Recall:
                case ChatMessageType.Craft:
                case ChatMessageType.Salvaging:

                    return true;
            }
            return false;
        }

        /// <summary>
        /// Called when adding or removing a character squelch
        /// </summary>
        public void HandleActionModifyCharacterSquelch(bool squelch, uint playerGuid, string playerName, ChatMessageType messageType)
        {
            //Console.WriteLine($"{Player.Name}.HandleActionModifyCharacterSquelch({squelch}, {playerGuid:X8}, {playerName}, {messageType})");

            if (messageType != ChatMessageType.AllChannels && !IsLegalChannel(messageType))
            {
                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{messageType} is not a legal squelch channel", ChatMessageType.Broadcast));
                return;
            }

            IPlayer player;

            if (playerGuid != 0)
            {
                player = PlayerManager.FindByGuid(new ObjectGuid(playerGuid));

                if (player == null)
                {
                    Player.Session.Network.EnqueueSend(new GameMessageSystemChat("Couldn't find player to squelch.", ChatMessageType.Broadcast));
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(playerName)) return;

                player = PlayerManager.FindByName(playerName);

                if (player == null)
                {
                    Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found.", ChatMessageType.Broadcast));
                    return;
                }
            }

            if (player.Guid == Player.Guid)
            {
                Player.Session.Network.EnqueueSend(new GameMessageSystemChat("You can't squelch yourself!", ChatMessageType.Broadcast));
                return;
            }


            if (squelch)
                SquelchCharacter(player, messageType);
            else
                UnsquelchCharacter(player, messageType);

            UpdateSquelchDB();

            SendSquelchDB();
        }

        public void SquelchCharacter(IPlayer player, ChatMessageType messageType)
        {
            var squelches = Player.Character.GetSquelches(Player.CharacterDatabaseLock);

            var existing = squelches.FirstOrDefault(i => i.SquelchCharacterId == player.Guid.Full);

            var newMask = messageType.ToMask();

            var channelMsg = messageType == ChatMessageType.AllChannels ? "" : $" on the {messageType} channel";

            if (existing != null)
            {
                var existingMask = (SquelchMask)existing.Type;

                newMask = existingMask.Add(newMask);

                if (existingMask == newMask)
                {
                    Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is already squelched{channelMsg}.", ChatMessageType.Broadcast));
                    return;
                }
            }

            Player.Character.AddOrUpdateSquelch(player.Guid.Full, 0, (uint)newMask, Player.CharacterDatabaseLock);
            Player.CharacterChangesDetected = true;

            Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been squelched{channelMsg}.", ChatMessageType.Broadcast));
        }

        public void UnsquelchCharacter(IPlayer player, ChatMessageType messageType)
        {
            var squelches = Player.Character.GetSquelches(Player.CharacterDatabaseLock);

            var existing = squelches.FirstOrDefault(i => i.SquelchCharacterId == player.Guid.Full);

            if (existing == null)
            {
                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is not squelched.", ChatMessageType.Broadcast));
                return;
            }

            if (messageType == ChatMessageType.AllChannels)
            {
                Player.Character.TryRemoveSquelch(player.Guid.Full, 0, out _, Player.CharacterDatabaseLock);
                Player.CharacterChangesDetected = true;

                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been unsquelched.", ChatMessageType.Broadcast));
            }
            else
            {
                var existingMask = (SquelchMask)existing.Type;
                var removeMask = messageType.ToMask();

                if (!existingMask.HasFlag(removeMask))
                {
                    Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} is not squelched on the {messageType} channel.", ChatMessageType.Broadcast));
                    return;
                }

                var newMask = existingMask.Remove(removeMask);

                if (newMask != SquelchMask.None)
                    Player.Character.AddOrUpdateSquelch(player.Guid.Full, 0, (uint)newMask, Player.CharacterDatabaseLock);
                else
                    Player.Character.TryRemoveSquelch(player.Guid.Full, 0, out _, Player.CharacterDatabaseLock);

                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name} has been unsquelched on the {messageType} channel.", ChatMessageType.Broadcast));
            }
        }

        /// <summary>
        /// Called when adding or removing an account squelch
        /// </summary>
        public void HandleActionModifyAccountSquelch(bool squelch, string playerName)
        {
            //Console.WriteLine($"{Player.Name}.HandleActionModifyAccountSquelch({squelch}, {playerName})");

            if (string.IsNullOrWhiteSpace(playerName)) return;

            var player = PlayerManager.FindByName(playerName);

            if (player == null)
            {
                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{playerName} not found.", ChatMessageType.Broadcast));
                return;
            }

            if (player.Account.AccountId == Player.Account.AccountId)
            {
                Player.Session.Network.EnqueueSend(new GameMessageSystemChat("You can't squelch yourself!", ChatMessageType.Broadcast));
                return;
            }

            var squelches = Player.Character.GetSquelches(Player.CharacterDatabaseLock);

            var existing = squelches.FirstOrDefault(i => i.SquelchAccountId == player.Account.AccountId);

            if (squelch)
            {
                if (existing != null)
                {
                    Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s account is already squelched.", ChatMessageType.Broadcast));
                    return;
                }

                // always all channels?
                Player.Character.AddOrUpdateSquelch(player.Guid.Full, player.Account.AccountId, (uint)SquelchMask.AllChannels, Player.CharacterDatabaseLock);
                Player.CharacterChangesDetected = true;

                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s account has been squelched.", ChatMessageType.Broadcast));
            }
            else
            {
                if (existing == null)
                {
                    Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s account is not squelched.", ChatMessageType.Broadcast));
                    return;
                }

                Player.Character.TryRemoveSquelch(existing.SquelchCharacterId, player.Account.AccountId, out _, Player.CharacterDatabaseLock);
                Player.CharacterChangesDetected = true;

                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{player.Name}'s account has been unsquelched.", ChatMessageType.Broadcast));
            }

            UpdateSquelchDB();

            SendSquelchDB();
        }

        /// <summary>
        /// Called when modifying the global squelches - @filter
        /// </summary>
        public void HandleActionModifyGlobalSquelch(bool squelch, ChatMessageType messageType)
        {
            //Console.WriteLine($"{Player.Name}.HandleActionModifyGlobalSquelch({squelch}, {messageType})");

            var mask = messageType.ToMask();

            var existingMask = Player.SquelchGlobal;
            var newMask = squelch ? existingMask.Add(mask) : existingMask.Remove(mask);

            var op = squelch ? "squelch" : "unsquelch";

            if (existingMask == newMask)
            {
                Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {messageType} channel is already {op}ed.", ChatMessageType.Broadcast));
                return;
            }

            Player.SquelchGlobal = newMask;

            Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"The {messageType} channel has been {op}ed.", ChatMessageType.Broadcast));

            UpdateSquelchDB();

            SendSquelchDB();
        }

        /// <summary>
        /// Builds the SquelchDB for network sending
        /// </summary>
        /// <returns></returns>
        public void UpdateSquelchDB()
        {
            Squelches = new SquelchDB(Player.Character.GetSquelches(Player.CharacterDatabaseLock), Player.SquelchGlobal);
        }

        /// <summary>
        /// Sends the SquelchDB to the player
        /// </summary>
        public void SendSquelchDB()
        {
            Player.Session.Network.EnqueueSend(new GameEventSetSquelchDB(Player.Session, Squelches));
        }
    }
}
