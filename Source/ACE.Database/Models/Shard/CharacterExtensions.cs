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

        public static List<CharacterPropertiesContractRegistry> GetContracts(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesContractRegistry.ToList();
            }
        }

        public static int GetContractsCount(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesContractRegistry.Count;
            }
        }

        public static List<uint> GetContractsIds(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesContractRegistry.Select(r => r.ContractId).ToList();
            }
        }

        public static CharacterPropertiesContractRegistry GetContract(this Character character, uint contractId, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesContractRegistry.FirstOrDefault(c => c.ContractId == contractId);
            }
        }

        public static CharacterPropertiesContractRegistry GetOrCreateContract(this Character character, uint contractId, Object rwLock, out bool contractWasCreated)
        {
            lock (rwLock)
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
        }

        public static bool EraseContract(this Character character, uint contractId, out CharacterPropertiesContractRegistry contractErased, Object rwLock)
        {
            lock (rwLock)
            {
                contractErased = character.CharacterPropertiesContractRegistry.FirstOrDefault(c => c.ContractId == contractId);

                if (contractErased == null)
                    return false;

                character.CharacterPropertiesContractRegistry.Remove(contractErased);

                return true;
            }
        }

        public static void EraseAllContracts(this Character character, out List<CharacterPropertiesContractRegistry> contractsErased, Object rwLock)
        {
            lock (rwLock)
            {
                contractsErased = character.CharacterPropertiesContractRegistry.ToList();

                character.CharacterPropertiesContractRegistry.Clear();
            }
        }


        // =====================================
        // CharacterPropertiesFillCompBook
        // =====================================

        public static CharacterPropertiesFillCompBook GetFillComponent(this Character character, uint wcid, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesFillCompBook.FirstOrDefault(i => i.SpellComponentId == wcid);
            }
        }

        public static List<CharacterPropertiesFillCompBook> GetFillComponents(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesFillCompBook.ToList();
            }
        }

        public static CharacterPropertiesFillCompBook AddFillComponent(this Character character, uint wcid, uint amount, Object rwLock, out bool alreadyExists)
        {
            lock (rwLock)
            {
                var entity = character.CharacterPropertiesFillCompBook.FirstOrDefault(i => i.SpellComponentId == wcid);
                if (entity != null)
                {
                    alreadyExists = true;
                    return entity;
                }

                    entity = new CharacterPropertiesFillCompBook { CharacterId = character.Id, SpellComponentId = (int)wcid, QuantityToRebuy = (int)amount };
                    character.CharacterPropertiesFillCompBook.Add(entity);
                    alreadyExists = false;
                    return entity;
            }
        }

        public static bool TryRemoveFillComponent(this Character character, uint wcid, out CharacterPropertiesFillCompBook entity, Object rwLock)
        {
            lock (rwLock)
            {
                entity = character.CharacterPropertiesFillCompBook.FirstOrDefault(i => i.SpellComponentId == wcid);
                if (entity != null)
                {
                        character.CharacterPropertiesFillCompBook.Remove(entity);
                        entity.Character = null;
                        return true;

                }
                return false;
            }
        }


        // =====================================
        // CharacterPropertiesFriendList
        // =====================================

        public static List<CharacterPropertiesFriendList> GetFriends(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesFriendList.ToList();
            }
        }

        public static bool HasAsFriend(this Character character, uint friendId, Object rwLock)
        {
            lock (rwLock)
            {
                foreach (var record in character.CharacterPropertiesFriendList)
                {
                    if (record.FriendId == friendId)
                        return true;
                }

                return false;
            }
        }

        public static CharacterPropertiesFriendList AddFriend(this Character character, uint friendId, Object rwLock, out bool friendAlreadyExists)
        {
            lock (rwLock)
            {
                var entity = character.CharacterPropertiesFriendList.FirstOrDefault(x => x.FriendId == friendId);
                if (entity != null)
                {
                    friendAlreadyExists = true;
                    return entity;
                }

                    entity = new CharacterPropertiesFriendList { CharacterId = character.Id, FriendId = friendId, Character = character};
                    character.CharacterPropertiesFriendList.Add(entity);
                    friendAlreadyExists = false;
                    return entity;
            }
        }

        public static bool TryRemoveFriend(this Character character, uint friendId, out CharacterPropertiesFriendList entity, Object rwLock)
        {
            lock (rwLock)
            {
                entity = character.CharacterPropertiesFriendList.FirstOrDefault(x => x.FriendId == friendId);
                if (entity != null)
                {
                        character.CharacterPropertiesFriendList.Remove(entity);
                        entity.Character = null;
                        return true;
                }
                return false;
            }
        }

        public static bool ClearAllFriends(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                try
                {
                    character.CharacterPropertiesFriendList.Clear();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        // =====================================
        // CharacterPropertiesQuestRegistry
        // =====================================

        public static List<CharacterPropertiesQuestRegistry> GetQuests(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesQuestRegistry.ToList();
            }
        }

        public static CharacterPropertiesQuestRegistry GetQuest(this Character character, string questName, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesQuestRegistry.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static CharacterPropertiesQuestRegistry GetOrCreateQuest(this Character character, string questName, Object rwLock, out bool questRegistryWasCreated)
        {
            lock (rwLock)
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
        }

        public static bool EraseQuest(this Character character, string questName, Object rwLock)
        {
            lock (rwLock)
            {
                var entity = character.CharacterPropertiesQuestRegistry.FirstOrDefault(q => q.QuestName.Equals(questName, StringComparison.OrdinalIgnoreCase));

                if (entity == null)
                    return false;

                character.CharacterPropertiesQuestRegistry.Remove(entity);

                return true;
            }
        }

        public static void EraseAllQuests(this Character character, out List<string> questNamesErased, Object rwLock)
        {
            lock (rwLock)
            {
                questNamesErased = character.CharacterPropertiesQuestRegistry.Select(r => r.QuestName).ToList();

                character.CharacterPropertiesQuestRegistry.Clear();
            }
        }


        // =====================================
        // CharacterPropertiesShortcutBar
        // =====================================

        public static List<CharacterPropertiesShortcutBar> GetShortcuts(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesShortcutBar.ToList();
            }
        }

        public static void AddOrUpdateShortcut(this Character character, uint index, uint objectId, Object rwLock)
        {
            lock (rwLock)
            {
                var entity = character.CharacterPropertiesShortcutBar.FirstOrDefault(x => x.ShortcutBarIndex == index + 1);

                {
                    if (entity == null)
                    {
                        entity = new CharacterPropertiesShortcutBar { CharacterId = character.Id, ShortcutObjectId = objectId, ShortcutBarIndex = index + 1, Character = character };
                        character.CharacterPropertiesShortcutBar.Add(entity);
                    }
                    else
                        entity.ShortcutObjectId = objectId;
                }
            }
        }

        public static bool TryRemoveShortcut(this Character character, uint index, out CharacterPropertiesShortcutBar entity, Object rwLock)
        {
            lock (rwLock)
            {
                entity = character.CharacterPropertiesShortcutBar.FirstOrDefault(x => x.ShortcutBarIndex == index + 1);

                if (entity != null)
                {
                        character.CharacterPropertiesShortcutBar.Remove(entity);
                        entity.Character = null;
                        return true;
                }
                return false;
            }
        }


        // =====================================
        // CharacterPropertiesSpellBar
        // =====================================

        public static List<CharacterPropertiesSpellBar> GetSpellsInBar(this Character character, int barNumber, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesSpellBar.Where(x => x.SpellBarNumber == barNumber + 1).OrderBy(x => x.SpellBarIndex).ToList();
            }
        }

        /// <summary>
        /// This will incrememt all existing spells on the same barNumber at or after indexInBar by one before adding the new spell.
        /// </summary>
        public static bool AddSpellToBar(this Character character, uint barNumber, uint indexInBar, uint spell, Object rwLock)
        {
            lock (rwLock)
            {
                var entity = character.CharacterPropertiesSpellBar.FirstOrDefault(x => x.SpellBarNumber == barNumber + 1 && x.SpellId == spell);

                if (entity == null)
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

                        entity = new CharacterPropertiesSpellBar { CharacterId = character.Id, SpellBarNumber = barNumber + 1, SpellBarIndex = indexInBar + 1, SpellId = spell, Character = character };

                        character.CharacterPropertiesSpellBar.Add(entity);

                        //Console.WriteLine($"Character.AddSpellToBar.Add: barNumber = {barNumber + 1} | indexInBar = {indexInBar + 1} | spell = {spell}");

                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// This will decrement all existing spells on the same barNumber after indexInBar by one after removing the existing spell.
        /// </summary>
        public static bool TryRemoveSpellFromBar(this Character character, uint barNumber, uint spell, out CharacterPropertiesSpellBar entity, Object rwLock)
        {
            lock (rwLock)
            {
                //Console.WriteLine($"Character.TryRemoveSpellFromBar.Entry: barNumber = {barNumber} ({barNumber + 1}) | spell = {spell}");
                entity = character.CharacterPropertiesSpellBar.FirstOrDefault(x => x.SpellBarNumber == barNumber + 1 && x.SpellId == spell);

                if (entity != null)
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
                return false;
            }
        }

        // =====================================
        // CharacterPropertiesSquelch
        // =====================================

        public static List<CharacterPropertiesSquelch> GetSquelches(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesSquelch.ToList();
            }
        }

        public static void AddOrUpdateSquelch(this Character character, uint squelchCharacterId, uint squelchAccountId, uint type, Object rwLock)
        {
            lock (rwLock)
            {
                var entity = character.CharacterPropertiesSquelch.FirstOrDefault(x => x.SquelchCharacterId == squelchCharacterId);

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
        }

        public static bool TryRemoveSquelch(this Character character, uint squelchCharacterId, uint squelchAccountId, out CharacterPropertiesSquelch entity, Object rwLock)
        {
            lock (rwLock)
            {
                entity = character.CharacterPropertiesSquelch.FirstOrDefault(x => x.SquelchCharacterId == squelchCharacterId && x.SquelchAccountId == squelchAccountId);

                if (entity != null)
                {
                        character.CharacterPropertiesSquelch.Remove(entity);
                        entity.Character = null;
                        return true;

                }
                return false;
            }
        }
        
        // =====================================
        // CharacterPropertiesTitleBook
        // =====================================

        public static List<CharacterPropertiesTitleBook> GetTitles(this Character character, Object rwLock)
        {
            lock (rwLock)
            {
                return character.CharacterPropertiesTitleBook.ToList();
            }
        }

        public static void AddTitleToRegistry(this Character character, uint title, Object rwLock, out bool titleAlreadyExists, out int numCharacterTitles)
        {
            lock (rwLock)
            {
                var entity = character.CharacterPropertiesTitleBook.FirstOrDefault(x => x.TitleId == title);
                if (entity != null)
                {
                    titleAlreadyExists = true;
                    numCharacterTitles = character.CharacterPropertiesTitleBook.Count;
                    return;
                }

                    entity = new CharacterPropertiesTitleBook { CharacterId = character.Id, TitleId = title, Character = character };
                    character.CharacterPropertiesTitleBook.Add(entity);
                    titleAlreadyExists = false;
                    numCharacterTitles = character.CharacterPropertiesTitleBook.Count;
            }
        }
    }
}
