using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ACE.Entity;

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

        public static bool TryRemoveFriend(this Character character, ObjectGuid friendGuid, out CharacterPropertiesFriendList entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = character.CharacterPropertiesFriendList.FirstOrDefault(x => x.FriendId == friendGuid.Full);
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
            rwLock.EnterWriteLock();
            try
            {
                return character.CharacterPropertiesShortcutBar.ToList();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static CharacterPropertiesShortcutBar AddShortcut(this Character character, uint index, uint objectId, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                var entity = new CharacterPropertiesShortcutBar { CharacterId = character.Id, ShortcutBarIndex = index, ShortcutObjectId = objectId };
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


        // =====================================
        // CharacterPropertiesTitleBook
        // =====================================
    }
}
