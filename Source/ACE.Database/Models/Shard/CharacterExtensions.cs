using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ACE.Database.Models.Shard
{
    public static class CharacterExtensions
    {
        // =====================================
        // CharacterPropertiesContract
        // =====================================


        // =====================================
        // CharacterPropertiesFillCompBook
        // =====================================


        // =====================================
        // CharacterPropertiesFriendList
        // =====================================

        public static bool HasAsFriend(this Character character, uint friendId, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                foreach (var record in character.CharacterPropertiesFriendList)
                {
                    if (record.FriendId == friendId)
                        return true;
                }

                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static CharacterPropertiesFriendList AddFriend(this Character character, uint friendId, ReaderWriterLockSlim rwLock, out bool friendAlreadyExists)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var entity = character.CharacterPropertiesFriendList.FirstOrDefault(x => x.FriendId == friendId);
                if (entity != null)
                {
                    friendAlreadyExists = true;
                    return entity;
                }

                rwLock.EnterWriteLock();
                try
                {
                    entity = new CharacterPropertiesFriendList { CharacterId = character.Id, FriendId = friendId, Character = character};
                    character.CharacterPropertiesFriendList.Add(entity);
                    friendAlreadyExists = false;
                    return entity;
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }

        public static bool TryRemoveFriend(this Character character, uint friendId, out CharacterPropertiesFriendList entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = character.CharacterPropertiesFriendList.FirstOrDefault(x => x.FriendId == friendId);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        character.CharacterPropertiesFriendList.Remove(entity);
                        entity.Character = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }


        // =====================================
        // CharacterPropertiesQuestRegistry
        // =====================================


        // =====================================
        // CharacterPropertiesShortcutBar
        // =====================================

        public static List<CharacterPropertiesShortcutBar> GetShortcuts(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesShortcutBar.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static CharacterPropertiesShortcutBar AddShortcut(this Character character, uint index, uint objectId, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                var entity = new CharacterPropertiesShortcutBar { CharacterId = character.Id, ShortcutBarIndex = index, ShortcutObjectId = objectId, Character = character };
                character.CharacterPropertiesShortcutBar.Add(entity);
                return entity;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool TryRemoveShortcut(this Character character, uint index, out CharacterPropertiesShortcutBar entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = character.CharacterPropertiesShortcutBar.FirstOrDefault(x => x.ShortcutBarIndex == index);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        character.CharacterPropertiesShortcutBar.Remove(entity);
                        entity.Character = null;
                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }


        // =====================================
        // CharacterPropertiesSpellBar
        // =====================================

        public static List<CharacterPropertiesSpellBar> GetSpellsInBar(this Character character, int barNumber, ReaderWriterLockSlim rwLock)
        {
            IEnumerable<CharacterPropertiesSpellBar> results;
            rwLock.EnterReadLock();
            try
            {
                results = character.CharacterPropertiesSpellBar.Where(x => x.SpellBarNumber == barNumber);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
            return results.ToList();
        }

        /// <summary>
        /// This will incrememt all existing spells on the same barNumber at or after indexInBar by one before adding the new spell.
        /// </summary>
        public static void AddSpellToBar(this Character character, uint barNumber, uint indexInBar, uint spell, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                var spellCountInThisBar = character.CharacterPropertiesSpellBar.Count(x => x.SpellBarNumber == barNumber);

                if (indexInBar > spellCountInThisBar + 1)
                    indexInBar = (uint)(spellCountInThisBar + 1);

                // We must increment the position of existing spells in the bar that exist on or after this position
                foreach (var property in character.CharacterPropertiesSpellBar)
                {
                    if (property.SpellBarNumber == barNumber && property.SpellBarIndex >= indexInBar)
                        property.SpellBarIndex++;
                }

                var entity = new CharacterPropertiesSpellBar { CharacterId = character.Id, SpellBarNumber = barNumber, SpellBarIndex = indexInBar, SpellId = spell, Character = character };

                character.CharacterPropertiesSpellBar.Add(entity);
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// This will decrement all existing spells on the same barNumber after indexInBar by one after removing the existing spell.
        /// </summary>
        public static bool TryRemoveSpellFromBar(this Character character, uint barNumber, uint spell, out CharacterPropertiesSpellBar entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = character.CharacterPropertiesSpellBar.FirstOrDefault(x => x.SpellBarNumber == barNumber && x.SpellId == spell);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        character.CharacterPropertiesSpellBar.Remove(entity);
                        entity.Character = null;

                        // We must decrement the position of existing spells in the bar that exist after this position
                        foreach (var property in character.CharacterPropertiesSpellBar)
                        {
                            if (property.SpellBarNumber == barNumber && property.SpellBarIndex > entity.SpellBarIndex)
                                property.SpellBarIndex--;
                        }

                        return true;
                    }
                    finally
                    {
                        rwLock.ExitWriteLock();
                    }
                }
                return false;
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }


        // =====================================
        // CharacterPropertiesTitleBook
        // =====================================

        public static void AddTitleToRegistry(this Character character, uint title, ReaderWriterLockSlim rwLock, out bool titleAlreadyExists, out int numCharacterTitles)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var entity = character.CharacterPropertiesTitleBook.FirstOrDefault(x => x.TitleId == title);
                if (entity != null)
                {
                    titleAlreadyExists = true;
                    numCharacterTitles = character.CharacterPropertiesTitleBook.Count;
                    return;
                }

                rwLock.EnterWriteLock();
                try
                {
                    entity = new CharacterPropertiesTitleBook { CharacterId = character.Id, TitleId = title, Character = character };
                    character.CharacterPropertiesTitleBook.Add(entity);
                    titleAlreadyExists = false;
                    numCharacterTitles = character.CharacterPropertiesTitleBook.Count;
                }
                finally
                {
                    rwLock.ExitWriteLock();
                }
            }
            finally
            {
                rwLock.ExitUpgradeableReadLock();
            }
        }
    }
}
