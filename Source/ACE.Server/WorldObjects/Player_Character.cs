using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        // =====================================
        // Character Options
        // =====================================

        public bool GetCharacterOption(CharacterOption option)
        {
            if (option.GetCharacterOptions1Attribute() != null)
                return GetCharacterOptions1((CharacterOptions1)Enum.Parse(typeof(CharacterOptions1), option.ToString()));

            return GetCharacterOptions2((CharacterOptions2)Enum.Parse(typeof(CharacterOptions2), option.ToString()));
        }

        private bool GetCharacterOptions1(CharacterOptions1 option)
        {
            return (Character.CharacterOptions1 & (int)option) != 0;
        }

        private bool GetCharacterOptions2(CharacterOptions2 option)
        {
            return (Character.CharacterOptions2 & (int)option) != 0;
        }

        public void SetCharacterOption(CharacterOption option, bool value)
        {
            if (option.GetCharacterOptions1Attribute() != null)
                SetCharacterOptions1((CharacterOptions1)Enum.Parse(typeof(CharacterOptions1), option.ToString()), value);
            else
                SetCharacterOptions2((CharacterOptions2)Enum.Parse(typeof(CharacterOptions2), option.ToString()), value);
        }

        private void SetCharacterOptions1(CharacterOptions1 option, bool value)
        {
            var options = Character.CharacterOptions1;

            if (value)
                options |= (int)option;
            else
                options &= ~(int)option;

            SetCharacterOptions1(options);
        }

        private void SetCharacterOptions2(CharacterOptions2 option, bool value)
        {
            var options = Character.CharacterOptions2;

            if (value)
                options |= (int)option;
            else
                options &= ~(int)option;

            SetCharacterOptions2(options);
        }

        public void SetCharacterOptions1(int value)
        {
            Character.CharacterOptions1 = value;
            CharacterChangesDetected = true;
        }

        public void SetCharacterOptions2(int value)
        {
            Character.CharacterOptions2 = value;
            CharacterChangesDetected = true;
        }


        // =====================================
        // CharacterPropertiesContract
        // =====================================


        // =====================================
        // CharacterPropertiesFillCompBook
        // =====================================


        // =====================================
        // Friends
        // =====================================

        /// <summary>
        /// Adds a friend and updates the database.
        /// </summary>
        /// <param name="friendName">The name of the friend that is being added.</param>
        public void HandleActionAddFriend(string friendName)
        {
            if (string.Equals(friendName, Name, StringComparison.CurrentCultureIgnoreCase))
                ChatPacket.SendServerMessage(Session, "Sorry, but you can't be friends with yourself.", ChatMessageType.Broadcast);

            // get friend player info
            var friendInfo = WorldManager.AllPlayers.FirstOrDefault(p => p.Name.Equals(friendName));

            if (friendInfo == null)
            {
                ChatPacket.SendServerMessage(Session, "That character does not exist", ChatMessageType.Broadcast);
                return;
            }

            // already exists in friends list?
            if (Character.CharacterPropertiesFriendList.FirstOrDefault(f => f.FriendId == friendInfo.Guid.Full) != null)
                ChatPacket.SendServerMessage(Session, "That character is already in your friends list", ChatMessageType.Broadcast);

            var newFriend = new CharacterPropertiesFriendList();
            newFriend.CharacterId = Biota.Id;      // current player id
            //newFriend.AccountId = Biota.Character.AccountId;    // current player account id
            newFriend.FriendId = friendInfo.Biota.Id;

            // add friend to DB
            Character.CharacterPropertiesFriendList.Add(newFriend);

            // send network message
            Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendAdded, newFriend));
        }

        /// <summary>
        /// Remove a single friend and update the database.
        /// </summary>
        /// <param name="friendId">The ObjectGuid of the friend that is being removed</param>
        public void HandleActionRemoveFriend(ObjectGuid friendId)
        {
            var friendToRemove = Character.CharacterPropertiesFriendList.SingleOrDefault(f => f.FriendId == friendId.Full);

            // Not in friend list
            if (friendToRemove == null)
            {
                ChatPacket.SendServerMessage(Session, "That character is not in your friends list!", ChatMessageType.Broadcast);
                return;
            }

            // remove friend in DB
            if (Character.TryRemoveFriend(friendId, out var entity) && entity.Id != 0)
                CharacterChangesDetected = true;

            // send network message
            Session.Network.EnqueueSend(new GameEventFriendsListUpdate(Session, GameEventFriendsListUpdate.FriendsUpdateTypeFlag.FriendRemoved, friendToRemove));
        }

        /// <summary>
        /// Delete all friends and update the database.
        /// </summary>
        public void HandleActionRemoveAllFriends()
        {
            // Remove all from DB
            log.Warn("HandleActionRemoveAllFriends is not implemented.");
        }

        /// <summary>
        /// Set the AppearOffline option to the provided value.  It will also send out an update to all online clients that have this player as a friend. This option does not save to the database.
        /// </summary>
        public void AppearOffline(bool appearOffline)
        {
            SetCharacterOption(CharacterOption.AppearOffline, appearOffline);
            SendFriendStatusUpdates();
        }


        // =====================================
        // CharacterPropertiesQuestRegistry
        // =====================================


        // =====================================
        // CharacterPropertiesShortcutBar
        // =====================================

        public List<Shortcut> GetShortcuts()
        {
            var shortcuts = new List<Shortcut>();

            foreach (var shortcut in Character.CharacterPropertiesShortcutBar)
                shortcuts.Add(new Shortcut(shortcut));

            return shortcuts;
        }

        /// <summary>
        /// Handles the adding of items to 1-9 shortcut bar in lower-right corner.<para />
        /// Note that there are two rows. The top row is 1-9, the bottom row has no hotkeys.
        /// </summary>
        public void HandleActionAddShortcut(Shortcut shortcut)
        {
            // When a shortcut is added on top of an existing item, the client automatically sends the RemoveShortcut command for that existing item first, then will add the new item, and re-add the existing item to the appropriate place.

            var entity = new CharacterPropertiesShortcutBar { CharacterId = Biota.Id, ShortcutBarIndex = shortcut.Index, ShortcutObjectId = shortcut.ObjectId };

            Character.CharacterPropertiesShortcutBar.Add(entity);
            CharacterChangesDetected = true;
        }

        /// <summary>
        /// Handles the removing of items from 1-9 shortcut bar in lower-right corner
        /// </summary>
        public void HandleActionRemoveShortcut(uint index)
        {
            var entity = Character.CharacterPropertiesShortcutBar.FirstOrDefault(x => x.ShortcutBarIndex == index);

            if (entity != null)
            {
                Character.CharacterPropertiesShortcutBar.Remove(entity);
                CharacterChangesDetected = true;
            }
        }


        // =====================================
        // Spell Bar
        // =====================================

        /// <summary>
        /// Will return the spells in the bar, sorted by their position
        /// </summary>
        public List<SpellBarPositions> GetSpellsInSpellBar(int barId)
        {
            var spells = new List<SpellBarPositions>();

            var results = Character.CharacterPropertiesSpellBar.Where(x => x.SpellBarNumber == barId);

            foreach (var result in results)
            {
                var entity = new SpellBarPositions(result.SpellBarNumber, result.SpellBarIndex, result.SpellId);

                spells.Add(entity);
            }

            spells.Sort((a, b) => a.SpellBarPositionId.CompareTo(b.SpellBarPositionId));

            return spells;
        }

        /// <summary>
        /// This method implements player spell bar management for - adding a spell to a specific spell bar (0 based) at a specific slot (0 based).
        /// </summary>
        public void HandleActionAddSpellFavorite(uint spellId, uint spellBarPositionId, uint spellBarId)
        {
            var spells = GetSpellsInSpellBar((int)spellBarId);

            if (spellBarPositionId > spells.Count + 1)
                spellBarPositionId = (uint)(spells.Count + 1);

            // We must increment the position of existing spells in the bar that exist on or after this position
            foreach (var property in Character.CharacterPropertiesSpellBar)
            {
                if (property.SpellBarNumber == spellBarId && property.SpellBarIndex >= spellBarPositionId)
                    property.SpellBarIndex++;
            }

            var entity = new CharacterPropertiesSpellBar { CharacterId = Biota.Id, SpellBarNumber = spellBarId, SpellBarIndex = spellBarPositionId, SpellId = spellId };

            Character.CharacterPropertiesSpellBar.Add(entity);
            CharacterChangesDetected = true;
        }

        /// <summary>
        /// This method implements player spell bar management for - removing a spell to a specific spell bar (0 based)
        /// </summary>
        public void HandleActionRemoveSpellFavorite(uint spellId, uint spellBarId)
        {
            var entity = Character.CharacterPropertiesSpellBar.FirstOrDefault(x => x.SpellBarNumber == spellBarId && x.SpellId == spellId);

            if (entity != null)
            {
                // We must decrement the position of existing spells in the bar that exist after this position
                foreach (var property in Character.CharacterPropertiesSpellBar)
                {
                    if (property.SpellBarNumber == spellBarId && property.SpellBarIndex > entity.SpellBarIndex)
                    {
                        property.SpellBarIndex--;
                        CharacterChangesDetected = true;
                    }
                }

                Character.CharacterPropertiesSpellBar.Remove(entity);
                CharacterChangesDetected = true;
            }
        }


        // =====================================
        // CharacterPropertiesTitleBook
        // =====================================

        /// <summary>
        /// Add Title to Title Registry
        /// </summary>
        /// <param name="titleId">Id of Title to Add</param>
        /// <param name="setAsDisplayTitle">If this is true, make this the player's current title</param>
        public void AddTitle(uint titleId, bool setAsDisplayTitle = false)
        {
            if (!Enum.IsDefined(typeof(CharacterTitle), titleId))
                return;

            var titlebook = new List<uint>();

            foreach (var title in Character.CharacterPropertiesTitleBook)
                titlebook.Add(title.TitleId);

            NumCharacterTitles = titlebook.Count();

            bool sendMsg = false;
            bool notifyNewTitle = false;

            if (!titlebook.Contains(titleId))
            {
                Character.CharacterPropertiesTitleBook.Add(new Database.Models.Shard.CharacterPropertiesTitleBook { CharacterId = Guid.Full, TitleId = titleId });
                titlebook.Add(titleId);
                NumCharacterTitles++;
                sendMsg = true;
                notifyNewTitle = true;
            }

            if (setAsDisplayTitle && CharacterTitleId != titleId)
            {
                CharacterTitleId = (int)titleId;
                sendMsg = true;
            }

            if (sendMsg && FirstEnterWorldDone.Value)
            {
                var message = new GameEventUpdateTitle(Session, titleId, setAsDisplayTitle);
                Session.Network.EnqueueSend(message);
                if (notifyNewTitle)
                    Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, "You have been granted a new title."));
            }
        }

        public void AddTitle(CharacterTitle title, bool setAsDisplayTitle = false)
        {
            AddTitle((uint)title, setAsDisplayTitle);
        }

        public void HandleActionSetTitle(uint title)
        {
            AddTitle(title, true);
        }

        public void SetTitle(CharacterTitle title)
        {
            AddTitle(title, true);
        }
    }
}
