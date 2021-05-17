using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.Adapter
{
    public static class BiotaUpdater
    {
        public static void UpdateDatabaseBiota(ShardDbContext context, ACE.Entity.Models.Biota sourceBiota, ACE.Database.Models.Shard.Biota targetBiota)
        {
            targetBiota.WeenieClassId = sourceBiota.WeenieClassId;
            targetBiota.WeenieType = (int)sourceBiota.WeenieType;


            if (sourceBiota.PropertiesBool != null)
            {
                foreach (var kvp in sourceBiota.PropertiesBool)
                    targetBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in targetBiota.BiotaPropertiesBool)
            {
                if (sourceBiota.PropertiesBool == null || !sourceBiota.PropertiesBool.ContainsKey((PropertyBool)value.Type))
                    context.BiotaPropertiesBool.Remove(value);
            }

            if (sourceBiota.PropertiesDID != null)
            {
                foreach (var kvp in sourceBiota.PropertiesDID)
                    targetBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in targetBiota.BiotaPropertiesDID)
            {
                if (sourceBiota.PropertiesDID == null || !sourceBiota.PropertiesDID.ContainsKey((PropertyDataId)value.Type))
                    context.BiotaPropertiesDID.Remove(value);
            }

            if (sourceBiota.PropertiesFloat != null)
            {
                foreach (var kvp in sourceBiota.PropertiesFloat)
                    targetBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in targetBiota.BiotaPropertiesFloat)
            {
                if (sourceBiota.PropertiesFloat == null || !sourceBiota.PropertiesFloat.ContainsKey((PropertyFloat)value.Type))
                    context.BiotaPropertiesFloat.Remove(value);
            }

            if (sourceBiota.PropertiesIID != null)
            {
                foreach (var kvp in sourceBiota.PropertiesIID)
                    targetBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in targetBiota.BiotaPropertiesIID)
            {
                if (sourceBiota.PropertiesIID == null || !sourceBiota.PropertiesIID.ContainsKey((PropertyInstanceId)value.Type))
                    context.BiotaPropertiesIID.Remove(value);
            }

            if (sourceBiota.PropertiesInt != null)
            {
                foreach (var kvp in sourceBiota.PropertiesInt)
                    targetBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in targetBiota.BiotaPropertiesInt)
            {
                if (sourceBiota.PropertiesInt == null || !sourceBiota.PropertiesInt.ContainsKey((PropertyInt)value.Type))
                    context.BiotaPropertiesInt.Remove(value);
            }

            if (sourceBiota.PropertiesInt64 != null)
            {
                foreach (var kvp in sourceBiota.PropertiesInt64)
                    targetBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in targetBiota.BiotaPropertiesInt64)
            {
                if (sourceBiota.PropertiesInt64 == null || !sourceBiota.PropertiesInt64.ContainsKey((PropertyInt64)value.Type))
                    context.BiotaPropertiesInt64.Remove(value);
            }

            if (sourceBiota.PropertiesString != null)
            {
                foreach (var kvp in sourceBiota.PropertiesString)
                    targetBiota.SetProperty(kvp.Key, kvp.Value);
            }
            foreach (var value in targetBiota.BiotaPropertiesString)
            {
                if (sourceBiota.PropertiesString == null || !sourceBiota.PropertiesString.ContainsKey((PropertyString)value.Type))
                    context.BiotaPropertiesString.Remove(value);
            }


            if (sourceBiota.PropertiesPosition != null)
            {
                foreach (var kvp in sourceBiota.PropertiesPosition)
                {
                    BiotaPropertiesPosition existingValue = targetBiota.BiotaPropertiesPosition.FirstOrDefault(r => r.PositionType == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesPosition { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesPosition.Add(existingValue);
                    }

                    existingValue.PositionType = (ushort)kvp.Key;
                    existingValue.ObjCellId = kvp.Value.ObjCellId;
                    existingValue.OriginX = kvp.Value.PositionX;
                    existingValue.OriginY = kvp.Value.PositionY;
                    existingValue.OriginZ = kvp.Value.PositionZ;
                    existingValue.AnglesW = kvp.Value.RotationW;
                    existingValue.AnglesX = kvp.Value.RotationX;
                    existingValue.AnglesY = kvp.Value.RotationY;
                    existingValue.AnglesZ = kvp.Value.RotationZ;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesPosition)
            {
                if (sourceBiota.PropertiesPosition == null || !sourceBiota.PropertiesPosition.ContainsKey((PositionType)value.PositionType))
                    context.BiotaPropertiesPosition.Remove(value);
            }


            if (sourceBiota.PropertiesSpellBook != null)
            {
                // Optimization to help characters with very large spell books and avoid full iterations inside the foreach
                var existingValues = targetBiota.BiotaPropertiesSpellBook.ToDictionary(r => r.Spell, r => r);

                foreach (var kvp in sourceBiota.PropertiesSpellBook)
                {
                    if (!existingValues.TryGetValue(kvp.Key, out var existingValue))
                    {
                        existingValue = new BiotaPropertiesSpellBook { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesSpellBook.Add(existingValue);
                    }

                    existingValue.Spell = kvp.Key;
                    existingValue.Probability = kvp.Value;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesSpellBook)
            {
                if (sourceBiota.PropertiesSpellBook == null || !sourceBiota.PropertiesSpellBook.ContainsKey(value.Spell))
                    context.BiotaPropertiesSpellBook.Remove(value);
            }


            if (sourceBiota.PropertiesAnimPart != null)
            {
                for (int i = 0; i < sourceBiota.PropertiesAnimPart.Count; i++)
                {
                    var value = sourceBiota.PropertiesAnimPart[i];

                    BiotaPropertiesAnimPart existingValue = targetBiota.BiotaPropertiesAnimPart.FirstOrDefault(r => r.Order == i);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesAnimPart { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesAnimPart.Add(existingValue);
                    }

                    existingValue.Index = value.Index;
                    existingValue.AnimationId = value.AnimationId;
                    existingValue.Order = (byte)i;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesAnimPart)
            {
                if (sourceBiota.PropertiesAnimPart == null || value.Order == null || value.Order >= sourceBiota.PropertiesAnimPart.Count)
                    context.BiotaPropertiesAnimPart.Remove(value);
            }

            if (sourceBiota.PropertiesPalette != null)
            {
                for (int i = 0; i < sourceBiota.PropertiesPalette.Count; i++)
                {
                    var value = sourceBiota.PropertiesPalette[i];

                    BiotaPropertiesPalette existingValue = targetBiota.BiotaPropertiesPalette.FirstOrDefault(r => r.Order == i);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesPalette { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesPalette.Add(existingValue);
                    }

                    existingValue.SubPaletteId = value.SubPaletteId;
                    existingValue.Offset = value.Offset;
                    existingValue.Length = value.Length;
                    existingValue.Order = (byte)i;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesPalette)
            {
                if (sourceBiota.PropertiesPalette == null || value.Order == null || value.Order >= sourceBiota.PropertiesPalette.Count)
                    context.BiotaPropertiesPalette.Remove(value);
            }

            if (sourceBiota.PropertiesTextureMap != null)
            {
                for (int i = 0; i < sourceBiota.PropertiesTextureMap.Count; i++)
                {
                    var value = sourceBiota.PropertiesTextureMap[i];

                    BiotaPropertiesTextureMap existingValue = targetBiota.BiotaPropertiesTextureMap.FirstOrDefault(r => r.Order == i);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesTextureMap { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesTextureMap.Add(existingValue);
                    }

                    existingValue.Index = value.PartIndex;
                    existingValue.OldId = value.OldTexture;
                    existingValue.NewId = value.NewTexture;
                    existingValue.Order = (byte)i;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesTextureMap)
            {
                if (sourceBiota.PropertiesTextureMap == null || value.Order == null || value.Order >= sourceBiota.PropertiesTextureMap.Count)
                    context.BiotaPropertiesTextureMap.Remove(value);
            }


            // Properties for all world objects that typically aren't modified over the original Biota

            // This is a cluster... because there is no key per record, just the record id.
            // That poses a problem because when we add a new record to be saved, we don't know what the record id is yet.
            // It's not until we try to save the record a second time that we will then have the database persisted record (with a valid id), and the entity record (that still has a DatabaseRecordId of 0)
            // We then need to match up the record that was saved with it's entity counterpart
            var processedSourceCreateList = new HashSet<ACE.Entity.Models.PropertiesCreateList>();
            var usedTargetCreateList = new HashSet<BiotaPropertiesCreateList>();
            if (sourceBiota.PropertiesCreateList != null)
            {
                // Process matched up records first
                foreach (var value in sourceBiota.PropertiesCreateList)
                {
                    if (value.DatabaseRecordId == 0)
                        continue;

                    // Source record should already exist in the target
                    BiotaPropertiesCreateList existingValue = targetBiota.BiotaPropertiesCreateList.FirstOrDefault(r => r.Id == value.DatabaseRecordId);

                    // If the existingValue was not found, the database was likely modified outside of ACE after our last save
                    if (existingValue == null)
                        continue;

                    CopyValueInto(value, existingValue);

                    processedSourceCreateList.Add(value);
                    usedTargetCreateList.Add(existingValue);
                }
                foreach (var value in sourceBiota.PropertiesCreateList)
                {
                    if (processedSourceCreateList.Contains(value))
                        continue;

                    // For simplicity, just find the first unused target
                    BiotaPropertiesCreateList existingValue = targetBiota.BiotaPropertiesCreateList.FirstOrDefault(r => !usedTargetCreateList.Contains(r));

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesCreateList { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesCreateList.Add(existingValue);
                    }

                    value.DatabaseRecordId = existingValue.Id;

                    CopyValueInto(value, existingValue);

                    //processedSourceCreateList.Add(value);
                    usedTargetCreateList.Add(existingValue);
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesCreateList)
            {
                if (!usedTargetCreateList.Contains(value))
                    context.BiotaPropertiesCreateList.Remove(value);
            }

            // This is a cluster... because there is no key per record, just the record id.
            // That poses a problem because when we add a new record to be saved, we don't know what the record id is yet.
            // It's not until we try to save the record a second time that we will then have the database persisted record (with a valid id), and the entity record (that still has a DatabaseRecordId of 0)
            // We then need to match up the record that was saved with it's entity counterpart
            var emoteMap = new Dictionary<ACE.Entity.Models.PropertiesEmote, BiotaPropertiesEmote>();
            if (sourceBiota.PropertiesEmote != null)
            {
                // Process matched up records first
                foreach (var value in sourceBiota.PropertiesEmote)
                {
                    if (value.DatabaseRecordId == 0)
                        continue;

                    // Source record should already exist in the target
                    BiotaPropertiesEmote existingValue = targetBiota.BiotaPropertiesEmote.FirstOrDefault(r => r.Id == value.DatabaseRecordId);

                    // If the existingValue was not found, the database was likely modified outside of ACE after our last save
                    if (existingValue == null)
                        continue;

                    CopyValueInto(value, existingValue);

                    emoteMap[value] = existingValue;
                }
                foreach (var value in sourceBiota.PropertiesEmote)
                {
                    if (emoteMap.Keys.Contains(value))
                        continue;

                    // For simplicity, just find the first unused target
                    BiotaPropertiesEmote existingValue = targetBiota.BiotaPropertiesEmote.FirstOrDefault(r => !emoteMap.Values.Contains(r));

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesEmote { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesEmote.Add(existingValue);
                    }

                    value.DatabaseRecordId = existingValue.Id;

                    CopyValueInto(value, existingValue);

                    emoteMap[value] = existingValue;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesEmote)
            {
                if (!emoteMap.Values.Contains(value))
                    context.BiotaPropertiesEmote.Remove(value);
            }
            // Now process the emote actions
            foreach (var kvp in emoteMap)
            {
                for (int i = 0; i < kvp.Key.PropertiesEmoteAction.Count; i++)
                {
                    BiotaPropertiesEmoteAction existingValue = kvp.Value.BiotaPropertiesEmoteAction.FirstOrDefault(r => r.Order == i);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesEmoteAction { EmoteId = kvp.Value.Id };

                        kvp.Value.BiotaPropertiesEmoteAction.Add(existingValue);
                    }

                    CopyValueInto(kvp.Key.PropertiesEmoteAction[i], existingValue, (uint)i);
                }
                foreach (var value in kvp.Value.BiotaPropertiesEmoteAction)
                {
                    if (value.Order >= kvp.Key.PropertiesEmoteAction.Count)
                        context.BiotaPropertiesEmoteAction.Remove(value);
                }
            }

            if (sourceBiota.PropertiesEventFilter != null)
            {
                foreach (var value in sourceBiota.PropertiesEventFilter)
                {
                    BiotaPropertiesEventFilter existingValue = targetBiota.BiotaPropertiesEventFilter.FirstOrDefault(r => r.Event == value);

                    if (existingValue == null)
                    {
                        var entity = new BiotaPropertiesEventFilter { ObjectId = sourceBiota.Id, Event = value };

                        targetBiota.BiotaPropertiesEventFilter.Add(entity);
                    }
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesEventFilter)
            {
                if (sourceBiota.PropertiesEventFilter == null || !sourceBiota.PropertiesEventFilter.Any(p => p == value.Event))
                    context.BiotaPropertiesEventFilter.Remove(value);
            }

            // This is a cluster... because there is no key per record, just the record id.
            // That poses a problem because when we add a new record to be saved, we don't know what the record id is yet.
            // It's not until we try to save the record a second time that we will then have the database persisted record (with a valid id), and the entity record (that still has a DatabaseRecordId of 0)
            // We then need to match up the record that was saved with it's entity counterpart
            var processedSourceGenerators = new HashSet<ACE.Entity.Models.PropertiesGenerator>();
            var usedTargetGenerators = new HashSet<BiotaPropertiesGenerator>();
            if (sourceBiota.PropertiesGenerator != null)
            {
                // Process matched up records first
                foreach (var value in sourceBiota.PropertiesGenerator)
                {
                    if (value.DatabaseRecordId == 0)
                        continue;

                    // Source record should already exist in the target
                    BiotaPropertiesGenerator existingValue = targetBiota.BiotaPropertiesGenerator.FirstOrDefault(r => r.Id == value.DatabaseRecordId);

                    // If the existingValue was not found, the database was likely modified outside of ACE after our last save
                    if (existingValue == null)
                        continue;

                    CopyValueInto(value, existingValue);

                    processedSourceGenerators.Add(value);
                    usedTargetGenerators.Add(existingValue);
                }
                foreach (var value in sourceBiota.PropertiesGenerator)
                {
                    if (processedSourceGenerators.Contains(value))
                        continue;

                    // For simplicity, just find the first unused target
                    BiotaPropertiesGenerator existingValue = targetBiota.BiotaPropertiesGenerator.FirstOrDefault(r => !usedTargetGenerators.Contains(r));

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesGenerator { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesGenerator.Add(existingValue);
                    }

                    value.DatabaseRecordId = existingValue.Id;

                    CopyValueInto(value, existingValue);

                    //processedSourceGenerators.Add(value);
                    usedTargetGenerators.Add(existingValue);
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesGenerator)
            {
                if (!usedTargetGenerators.Contains(value))
                    context.BiotaPropertiesGenerator.Remove(value);
            }


            // Properties for creatures

            if (sourceBiota.PropertiesAttribute != null)
            {
                foreach (var kvp in sourceBiota.PropertiesAttribute)
                {
                    BiotaPropertiesAttribute existingValue = targetBiota.BiotaPropertiesAttribute.FirstOrDefault(r => r.Type == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesAttribute { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesAttribute.Add(existingValue);
                    }

                    existingValue.Type = (ushort)kvp.Key;
                    existingValue.InitLevel = kvp.Value.InitLevel;
                    existingValue.LevelFromCP = kvp.Value.LevelFromCP;
                    existingValue.CPSpent = kvp.Value.CPSpent;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesAttribute)
            {
                if (sourceBiota.PropertiesAttribute == null || !sourceBiota.PropertiesAttribute.ContainsKey((PropertyAttribute)value.Type))
                    context.BiotaPropertiesAttribute.Remove(value);
            }

            if (sourceBiota.PropertiesAttribute2nd != null)
            {
                foreach (var kvp in sourceBiota.PropertiesAttribute2nd)
                {
                    BiotaPropertiesAttribute2nd existingValue = targetBiota.BiotaPropertiesAttribute2nd.FirstOrDefault(r => r.Type == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesAttribute2nd { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesAttribute2nd.Add(existingValue);
                    }

                    existingValue.Type = (ushort)kvp.Key;
                    existingValue.InitLevel = kvp.Value.InitLevel;
                    existingValue.LevelFromCP = kvp.Value.LevelFromCP;
                    existingValue.CPSpent = kvp.Value.CPSpent;
                    existingValue.CurrentLevel = kvp.Value.CurrentLevel;
                }

            }
            foreach (var value in targetBiota.BiotaPropertiesAttribute2nd)
            {
                if (sourceBiota.PropertiesAttribute2nd == null || !sourceBiota.PropertiesAttribute2nd.ContainsKey((PropertyAttribute2nd)value.Type))
                    context.BiotaPropertiesAttribute2nd.Remove(value);
            }

            if (sourceBiota.PropertiesBodyPart != null)
            {
                foreach (var kvp in sourceBiota.PropertiesBodyPart)
                {
                    BiotaPropertiesBodyPart existingValue = targetBiota.BiotaPropertiesBodyPart.FirstOrDefault(r => r.Key == (uint)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesBodyPart { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesBodyPart.Add(existingValue);
                    }

                    existingValue.Key = (ushort)kvp.Key;
                    existingValue.DType = (int)kvp.Value.DType;
                    existingValue.DVal = kvp.Value.DVal;
                    existingValue.DVar = kvp.Value.DVar;
                    existingValue.BaseArmor = kvp.Value.BaseArmor;
                    existingValue.ArmorVsSlash = kvp.Value.ArmorVsSlash;
                    existingValue.ArmorVsPierce = kvp.Value.ArmorVsPierce;
                    existingValue.ArmorVsBludgeon = kvp.Value.ArmorVsBludgeon;
                    existingValue.ArmorVsCold = kvp.Value.ArmorVsCold;
                    existingValue.ArmorVsFire = kvp.Value.ArmorVsFire;
                    existingValue.ArmorVsAcid = kvp.Value.ArmorVsAcid;
                    existingValue.ArmorVsElectric = kvp.Value.ArmorVsElectric;
                    existingValue.ArmorVsNether = kvp.Value.ArmorVsNether;
                    existingValue.BH = kvp.Value.BH;
                    existingValue.HLF = kvp.Value.HLF;
                    existingValue.MLF = kvp.Value.MLF;
                    existingValue.LLF = kvp.Value.LLF;
                    existingValue.HRF = kvp.Value.HRF;
                    existingValue.MRF = kvp.Value.MRF;
                    existingValue.LRF = kvp.Value.LRF;
                    existingValue.HLB = kvp.Value.HLB;
                    existingValue.MLB = kvp.Value.MLB;
                    existingValue.LLB = kvp.Value.LLB;
                    existingValue.HRB = kvp.Value.HRB;
                    existingValue.MRB = kvp.Value.MRB;
                    existingValue.LRB = kvp.Value.LRB;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesBodyPart)
            {
                if (sourceBiota.PropertiesBodyPart == null || !sourceBiota.PropertiesBodyPart.ContainsKey((CombatBodyPart)value.Key))
                    context.BiotaPropertiesBodyPart.Remove(value);
            }

            if (sourceBiota.PropertiesSkill != null)
            {
                foreach (var kvp in sourceBiota.PropertiesSkill)
                {
                    BiotaPropertiesSkill existingValue = targetBiota.BiotaPropertiesSkill.FirstOrDefault(r => r.Type == (ushort)kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesSkill { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesSkill.Add(existingValue);
                    }

                    existingValue.Type = (ushort)kvp.Key;
                    existingValue.LevelFromPP = kvp.Value.LevelFromPP;
                    existingValue.SAC = (uint)kvp.Value.SAC;
                    existingValue.PP = kvp.Value.PP;
                    existingValue.InitLevel = kvp.Value.InitLevel;
                    existingValue.ResistanceAtLastCheck = kvp.Value.ResistanceAtLastCheck;
                    existingValue.LastUsedTime = kvp.Value.LastUsedTime;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesSkill)
            {
                if (sourceBiota.PropertiesSkill == null || !sourceBiota.PropertiesSkill.ContainsKey((Skill)value.Type))
                    context.BiotaPropertiesSkill.Remove(value);
            }


            // Properties for books

            if (sourceBiota.PropertiesBook != null)
            {
                if (targetBiota.BiotaPropertiesBook == null)
                    targetBiota.BiotaPropertiesBook = new BiotaPropertiesBook { ObjectId = sourceBiota.Id, };

                targetBiota.BiotaPropertiesBook.MaxNumPages = sourceBiota.PropertiesBook.MaxNumPages;
                targetBiota.BiotaPropertiesBook.MaxNumCharsPerPage = sourceBiota.PropertiesBook.MaxNumCharsPerPage;
            }
            else
            {
                if (targetBiota.BiotaPropertiesBook != null)
                    context.BiotaPropertiesBook.Remove(targetBiota.BiotaPropertiesBook);
            }

            if (sourceBiota.PropertiesBookPageData != null)
            {
                for (int i = 0; i < sourceBiota.PropertiesBookPageData.Count; i++)
                {
                    var value = sourceBiota.PropertiesBookPageData[i];

                    BiotaPropertiesBookPageData existingValue = targetBiota.BiotaPropertiesBookPageData.FirstOrDefault(r => r.PageId == i);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesBookPageData { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesBookPageData.Add(existingValue);
                    }

                    existingValue.PageId = (uint)i;
                    existingValue.AuthorId = value.AuthorId;
                    existingValue.AuthorName = value.AuthorName;
                    existingValue.AuthorAccount = value.AuthorAccount;
                    existingValue.IgnoreAuthor = value.IgnoreAuthor;
                    existingValue.PageText = value.PageText;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesBookPageData)
            {
                if (sourceBiota.PropertiesBookPageData == null || value.PageId >= sourceBiota.PropertiesBookPageData.Count)
                    context.BiotaPropertiesBookPageData.Remove(value);
            }


            // Biota additions over Weenie

            if (sourceBiota.PropertiesAllegiance != null)
            {
                foreach (var kvp in sourceBiota.PropertiesAllegiance)
                {
                    BiotaPropertiesAllegiance existingValue = targetBiota.BiotaPropertiesAllegiance.FirstOrDefault(r => r.CharacterId == kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesAllegiance { AllegianceId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesAllegiance.Add(existingValue);
                    }

                    existingValue.CharacterId = kvp.Key;
                    existingValue.Banned = kvp.Value.Banned;
                    existingValue.ApprovedVassal = kvp.Value.ApprovedVassal;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesAllegiance)
            {
                if (sourceBiota.PropertiesAllegiance == null || !sourceBiota.PropertiesAllegiance.ContainsKey(value.CharacterId))
                    context.BiotaPropertiesAllegiance.Remove(value);
            }

            if (sourceBiota.PropertiesEnchantmentRegistry != null)
            {
                foreach (var value in sourceBiota.PropertiesEnchantmentRegistry)
                {
                    BiotaPropertiesEnchantmentRegistry existingValue = targetBiota.BiotaPropertiesEnchantmentRegistry.FirstOrDefault(r => r.SpellId == value.SpellId && r.LayerId == value.LayerId && r.CasterObjectId == value.CasterObjectId);

                    if (existingValue == null)
                    {
                        existingValue = new BiotaPropertiesEnchantmentRegistry { ObjectId = sourceBiota.Id };

                        targetBiota.BiotaPropertiesEnchantmentRegistry.Add(existingValue);
                    }

                    existingValue.EnchantmentCategory = value.EnchantmentCategory;
                    existingValue.SpellId = value.SpellId;
                    existingValue.LayerId = value.LayerId;
                    existingValue.HasSpellSetId = value.HasSpellSetId;
                    existingValue.SpellCategory = (ushort)value.SpellCategory;
                    existingValue.PowerLevel = value.PowerLevel;
                    existingValue.StartTime = value.StartTime;
                    existingValue.Duration = value.Duration;
                    existingValue.CasterObjectId = value.CasterObjectId;
                    existingValue.DegradeModifier = value.DegradeModifier;
                    existingValue.DegradeLimit = value.DegradeLimit;
                    existingValue.LastTimeDegraded = value.LastTimeDegraded;
                    existingValue.StatModType = (uint)value.StatModType;
                    existingValue.StatModKey = value.StatModKey;
                    existingValue.StatModValue = value.StatModValue;
                    existingValue.SpellSetId = (uint)value.SpellSetId;
                }
            }
            foreach (var value in targetBiota.BiotaPropertiesEnchantmentRegistry)
            {
                if (sourceBiota.PropertiesEnchantmentRegistry == null || !sourceBiota.PropertiesEnchantmentRegistry.Any(p => p.SpellId == value.SpellId && p.LayerId == value.LayerId && p.CasterObjectId == value.CasterObjectId))
                    context.BiotaPropertiesEnchantmentRegistry.Remove(value);
            }

            if (sourceBiota.HousePermissions != null)
            {
                foreach (var kvp in sourceBiota.HousePermissions)
                {
                    HousePermission existingValue = targetBiota.HousePermission.FirstOrDefault(r => r.PlayerGuid == kvp.Key);

                    if (existingValue == null)
                    {
                        existingValue = new HousePermission { HouseId = sourceBiota.Id };

                        targetBiota.HousePermission.Add(existingValue);
                    }

                    existingValue.PlayerGuid = kvp.Key;
                    existingValue.Storage = kvp.Value;
                }
            }
            foreach (var value in targetBiota.HousePermission)
            {
                if (sourceBiota.HousePermissions == null || !sourceBiota.HousePermissions.ContainsKey(value.PlayerGuid))
                    context.HousePermission.Remove(value);
            }
        }

        private static void CopyValueInto(ACE.Entity.Models.PropertiesCreateList value, ACE.Database.Models.Shard.BiotaPropertiesCreateList existingValue)
        {
            existingValue.DestinationType = (sbyte)value.DestinationType;
            existingValue.WeenieClassId = value.WeenieClassId;
            existingValue.StackSize = value.StackSize;
            existingValue.Palette = value.Palette;
            existingValue.Shade = value.Shade;
            existingValue.TryToBond = value.TryToBond;
        }

        private static void CopyValueInto(ACE.Entity.Models.PropertiesEmote value, ACE.Database.Models.Shard.BiotaPropertiesEmote existingValue)
        {
            existingValue.Category = (uint)value.Category;
            existingValue.Probability = value.Probability;
            existingValue.WeenieClassId = value.WeenieClassId;
            existingValue.Style = (uint?)value.Style;
            existingValue.Substyle = (uint?)value.Substyle;
            existingValue.Quest = value.Quest;
            existingValue.VendorType = (int?)value.VendorType;
            existingValue.MinHealth = value.MinHealth;
            existingValue.MaxHealth = value.MaxHealth;
        }

        private static void CopyValueInto(ACE.Entity.Models.PropertiesEmoteAction value, ACE.Database.Models.Shard.BiotaPropertiesEmoteAction existingValue, uint order)
        {
            //existingValue.EmoteId = value.EmoteId;
            existingValue.Order = order;
            existingValue.Type = value.Type;
            existingValue.Delay = value.Delay;
            existingValue.Extent = value.Extent;
            existingValue.Motion = (int?)value.Motion;
            existingValue.Message = value.Message;
            existingValue.TestString = value.TestString;
            existingValue.Min = value.Min;
            existingValue.Max = value.Max;
            existingValue.Min64 = value.Min64;
            existingValue.Max64 = value.Max64;
            existingValue.MinDbl = value.MinDbl;
            existingValue.MaxDbl = value.MaxDbl;
            existingValue.Stat = value.Stat;
            existingValue.Display = value.Display;
            existingValue.Amount = value.Amount;
            existingValue.Amount64 = value.Amount64;
            existingValue.HeroXP64 = value.HeroXP64;
            existingValue.Percent = value.Percent;
            existingValue.SpellId = value.SpellId;
            existingValue.WealthRating = value.WealthRating;
            existingValue.TreasureClass = value.TreasureClass;
            existingValue.TreasureType = value.TreasureType;
            existingValue.PScript = (int?)value.PScript;
            existingValue.Sound = (int?)value.Sound;
            existingValue.DestinationType = value.DestinationType;
            existingValue.WeenieClassId = value.WeenieClassId;
            existingValue.StackSize = value.StackSize;
            existingValue.Palette = value.Palette;
            existingValue.Shade = value.Shade;
            existingValue.TryToBond = value.TryToBond;
            existingValue.ObjCellId = value.ObjCellId;
            existingValue.OriginX = value.OriginX;
            existingValue.OriginY = value.OriginY;
            existingValue.OriginZ = value.OriginZ;
            existingValue.AnglesW = value.AnglesW;
            existingValue.AnglesX = value.AnglesX;
            existingValue.AnglesY = value.AnglesY;
            existingValue.AnglesZ = value.AnglesZ;
        }

        private static void CopyValueInto(ACE.Entity.Models.PropertiesGenerator value, ACE.Database.Models.Shard.BiotaPropertiesGenerator existingValue)
        {
            existingValue.Probability = value.Probability;
            existingValue.WeenieClassId = value.WeenieClassId;
            existingValue.Delay = value.Delay;
            existingValue.InitCreate = value.InitCreate;
            existingValue.MaxCreate = value.MaxCreate;
            existingValue.WhenCreate = (uint)value.WhenCreate;
            existingValue.WhereCreate = (uint)value.WhereCreate;
            existingValue.StackSize = value.StackSize;
            existingValue.PaletteId = value.PaletteId;
            existingValue.Shade = value.Shade;
            existingValue.ObjCellId = value.ObjCellId;
            existingValue.OriginX = value.OriginX;
            existingValue.OriginY = value.OriginY;
            existingValue.OriginZ = value.OriginZ;
            existingValue.AnglesW = value.AnglesW;
            existingValue.AnglesX = value.AnglesX;
            existingValue.AnglesY = value.AnglesY;
            existingValue.AnglesZ = value.AnglesZ;
        }
    }
}
