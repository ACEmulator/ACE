using System;
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

        public static List<CharacterPropertiesContractRegistry> GetContracts(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesContractRegistry.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static int GetContractsCount(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesContractRegistry.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<uint> GetContractsIds(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesContractRegistry.Select(r => r.ContractId).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static CharacterPropertiesContractRegistry GetContract(this Character character, uint contractId, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesContractRegistry.FirstOrDefault(c => c.ContractId == contractId);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static CharacterPropertiesContractRegistry GetOrCreateContract(this Character character, uint contractId, ReaderWriterLockSlim rwLock, out bool contractWasCreated)
        {
            rwLock.EnterWriteLock();
            try
            {
                var entity = character.CharacterPropertiesContractRegistry.FirstOrDefault(c => c.ContractId == contractId);

                if (entity == null)
                {
                    entity = new CharacterPropertiesContractRegistry
                    {
                        ContractId = contractId
                    };

                    character.CharacterPropertiesContractRegistry.Add(entity);

                    contractWasCreated = true;
                }
                else
                    contractWasCreated = false;

                return entity;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool EraseContract(this Character character, uint contractId, out CharacterPropertiesContractRegistry contractErased, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                contractErased = character.CharacterPropertiesContractRegistry.FirstOrDefault(c => c.ContractId == contractId);

                if (contractErased == null)
                    return false;

                character.CharacterPropertiesContractRegistry.Remove(contractErased);

                return true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void EraseAllContracts(this Character character, out List<CharacterPropertiesContractRegistry> contractsErased, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                contractsErased = character.CharacterPropertiesContractRegistry.ToList();

                character.CharacterPropertiesContractRegistry.Clear();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


        // =====================================
        // CharacterPropertiesFillCompBook
        // =====================================

        public static CharacterPropertiesFillCompBook GetFillComponent(this Character character, uint wcid, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesFillCompBook.FirstOrDefault(i => i.SpellComponentId == wcid);
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static List<CharacterPropertiesFillCompBook> GetFillComponents(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesFillCompBook.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static CharacterPropertiesFillCompBook AddFillComponent(this Character character, uint wcid, uint amount, ReaderWriterLockSlim rwLock, out bool alreadyExists)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var entity = character.CharacterPropertiesFillCompBook.FirstOrDefault(i => i.SpellComponentId == wcid);
                if (entity != null)
                {
                    alreadyExists = true;
                    return entity;
                }

                rwLock.EnterWriteLock();
                try
                {
                    entity = new CharacterPropertiesFillCompBook { CharacterId = character.Id, SpellComponentId = (int)wcid, QuantityToRebuy = (int)amount };
                    character.CharacterPropertiesFillCompBook.Add(entity);
                    alreadyExists = false;
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

        public static bool TryRemoveFillComponent(this Character character, uint wcid, out CharacterPropertiesFillCompBook entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = character.CharacterPropertiesFillCompBook.FirstOrDefault(i => i.SpellComponentId == wcid);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        character.CharacterPropertiesFillCompBook.Remove(entity);
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
        // CharacterPropertiesFriendList
        // =====================================

        public static List<CharacterPropertiesFriendList> GetFriends(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesFriendList.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static bool HasAsFriend(this Character character, uint friendId, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
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
                rwLock.ExitReadLock();
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

        public static List<CharacterPropertiesQuestRegistry> GetQuests(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesQuestRegistry.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static CharacterPropertiesQuestRegistry GetQuest(this Character character, string questName, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesQuestRegistry.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static CharacterPropertiesQuestRegistry GetOrCreateQuest(this Character character, string questName, ReaderWriterLockSlim rwLock, out bool questRegistryWasCreated)
        {
            rwLock.EnterWriteLock();
            try
            {
                var entity = character.CharacterPropertiesQuestRegistry.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));

                if (entity == null)
                {
                    entity = new CharacterPropertiesQuestRegistry
                    {
                        QuestName = questName
                    };

                    character.CharacterPropertiesQuestRegistry.Add(entity);

                    questRegistryWasCreated = true;
                }
                else
                    questRegistryWasCreated = false;

                return entity;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static bool EraseQuest(this Character character, string questName, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                var entity = character.CharacterPropertiesQuestRegistry.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));

                if (entity == null)
                    return false;

                character.CharacterPropertiesQuestRegistry.Remove(entity);

                return true;
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }

        public static void EraseAllQuests(this Character character, out List<string> questNamesErased, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                questNamesErased = character.CharacterPropertiesQuestRegistry.Select(r => r.QuestName).ToList();

                character.CharacterPropertiesQuestRegistry.Clear();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }


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

        public static void AddOrUpdateShortcut(this Character character, uint index, uint objectId, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var entity = character.CharacterPropertiesShortcutBar.FirstOrDefault(x => x.ShortcutBarIndex == index + 1);
                rwLock.EnterWriteLock();
                try
                {
                    if (entity == null)
                    {
                        entity = new CharacterPropertiesShortcutBar { CharacterId = character.Id, ShortcutObjectId = objectId, ShortcutBarIndex = index + 1, Character = character };
                        character.CharacterPropertiesShortcutBar.Add(entity);
                    }
                    else
                        entity.ShortcutObjectId = objectId;
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

        public static bool TryRemoveShortcut(this Character character, uint index, out CharacterPropertiesShortcutBar entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = character.CharacterPropertiesShortcutBar.FirstOrDefault(x => x.ShortcutBarIndex == index + 1);

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
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesSpellBar.Where(x => x.SpellBarNumber == barNumber + 1).OrderBy(x => x.SpellBarIndex).ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// This will incrememt all existing spells on the same barNumber at or after indexInBar by one before adding the new spell.
        /// </summary>
        public static void AddSpellToBar(this Character character, uint barNumber, uint indexInBar, uint spell, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            try
            {
                var spellCountInThisBar = character.CharacterPropertiesSpellBar.Count(x => x.SpellBarNumber == barNumber + 1);

                //Console.WriteLine($"Character.AddSpellToBar.Entry: barNumber = {barNumber} ({barNumber + 1}) | indexInBar = {indexInBar} ({indexInBar + 1}) | spell = {spell} | spellCountInThisBar = {spellCountInThisBar}");

                if (indexInBar > spellCountInThisBar)
                    indexInBar = (uint)(spellCountInThisBar);

                // We must increment the position of existing spells in the bar that exist on or after this position
                foreach (var property in character.CharacterPropertiesSpellBar.OrderBy(x => x.SpellBarIndex))
                {
                    if (property.SpellBarNumber == barNumber + 1 && property.SpellBarIndex >= indexInBar + 1)
                    {
                        property.SpellBarIndex++;
                        //Console.WriteLine($"Character.AddSpellToBar.Adjust: SpellBarNumber = {property.SpellBarNumber} | SpellBarIndex = {property.SpellBarIndex} ({property.SpellBarIndex - 1}) | SpellId = {property.SpellId}");
                    }
                }

                var entity = new CharacterPropertiesSpellBar { CharacterId = character.Id, SpellBarNumber = barNumber + 1, SpellBarIndex = indexInBar + 1, SpellId = spell, Character = character };

                character.CharacterPropertiesSpellBar.Add(entity);

                //Console.WriteLine($"Character.AddSpellToBar.Add: barNumber = {barNumber + 1} | indexInBar = {indexInBar + 1} | spell = {spell}");
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
                //Console.WriteLine($"Character.TryRemoveSpellFromBar.Entry: barNumber = {barNumber} ({barNumber + 1}) | spell = {spell}");
                entity = character.CharacterPropertiesSpellBar.FirstOrDefault(x => x.SpellBarNumber == barNumber + 1 && x.SpellId == spell);
                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        //Console.WriteLine($"Character.TryRemoveSpellFromBar.Remove: SpellBarNumber = {entity.SpellBarNumber} | SpellBarIndex = {entity.SpellBarIndex} | SpellId = {entity.SpellId}");
                        character.CharacterPropertiesSpellBar.Remove(entity);
                        entity.Character = null;

                        // We must decrement the position of existing spells in the bar that exist after this position
                        foreach (var property in character.CharacterPropertiesSpellBar.OrderBy(x => x.SpellBarIndex))
                        {
                            if (property.SpellBarNumber == barNumber + 1 && property.SpellBarIndex > entity.SpellBarIndex)
                            {
                                property.SpellBarIndex--;
                                //Console.WriteLine($"Character.TryRemoveSpellFromBar.Adjust: SpellBarNumber = {property.SpellBarNumber} | SpellBarIndex = {property.SpellBarIndex} ({property.SpellBarIndex + 1}) | SpellId = {property.SpellId}");
                            }
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
        // CharacterPropertiesSquelch
        // =====================================

        public static List<CharacterPropertiesSquelch> GetSquelches(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesSquelch.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        public static void AddOrUpdateSquelch(this Character character, uint squelchCharacterId, uint squelchAccountId, uint type, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                var entity = character.CharacterPropertiesSquelch.FirstOrDefault(x => x.SquelchCharacterId == squelchCharacterId);
                rwLock.EnterWriteLock();
                try
                {
                    if (entity == null)
                    {
                        entity = new CharacterPropertiesSquelch { CharacterId = character.Id, SquelchCharacterId = squelchCharacterId, SquelchAccountId = squelchAccountId, Type = type, Character = character };
                        character.CharacterPropertiesSquelch.Add(entity);
                    }
                    else
                    {
                        entity.SquelchAccountId = squelchAccountId;
                        entity.Type = type;
                    }
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

        public static bool TryRemoveSquelch(this Character character, uint squelchCharacterId, uint squelchAccountId, out CharacterPropertiesSquelch entity, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterUpgradeableReadLock();
            try
            {
                entity = character.CharacterPropertiesSquelch.FirstOrDefault(x => x.SquelchCharacterId == squelchCharacterId && x.SquelchAccountId == squelchAccountId);

                if (entity != null)
                {
                    rwLock.EnterWriteLock();
                    try
                    {
                        character.CharacterPropertiesSquelch.Remove(entity);
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
        // CharacterPropertiesTitleBook
        // =====================================

        public static List<CharacterPropertiesTitleBook> GetTitles(this Character character, ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            try
            {
                return character.CharacterPropertiesTitleBook.ToList();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

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
