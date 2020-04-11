using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;

namespace ACE.Database.Adapter
{
    public static class BiotaConverter
    {
        public static ACE.Entity.Models.Biota ConvertToEntityBiota(ACE.Database.Models.Shard.Biota biota, bool instantiateEmptyCollections = false)
        {
            var result = new ACE.Entity.Models.Biota();

            result.Id = biota.Id;
            result.WeenieClassId = biota.WeenieClassId;
            result.WeenieType = (WeenieType)biota.WeenieType;

            if (biota.BiotaPropertiesBool != null && (instantiateEmptyCollections || biota.BiotaPropertiesBool.Count > 0))
            {
                result.PropertiesBool = new Dictionary<PropertyBool, bool>(biota.BiotaPropertiesBool.Count);
                foreach (var value in biota.BiotaPropertiesBool)
                    result.PropertiesBool[(PropertyBool)value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesDID != null && (instantiateEmptyCollections || biota.BiotaPropertiesDID.Count > 0))
            {
                result.PropertiesDID = new Dictionary<PropertyDataId, uint>(biota.BiotaPropertiesDID.Count);
                foreach (var value in biota.BiotaPropertiesDID)
                    result.PropertiesDID[(PropertyDataId)value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesFloat != null && (instantiateEmptyCollections || biota.BiotaPropertiesFloat.Count > 0))
            {
                result.PropertiesFloat = new Dictionary<PropertyFloat, double>(biota.BiotaPropertiesFloat.Count);
                foreach (var value in biota.BiotaPropertiesFloat)
                    result.PropertiesFloat[(PropertyFloat)value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesIID != null && (instantiateEmptyCollections || biota.BiotaPropertiesIID.Count > 0))
            {
                result.PropertiesIID = new Dictionary<PropertyInstanceId, uint>(biota.BiotaPropertiesIID.Count);
                foreach (var value in biota.BiotaPropertiesIID)
                    result.PropertiesIID[(PropertyInstanceId)value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesInt != null && (instantiateEmptyCollections || biota.BiotaPropertiesInt.Count > 0))
            {
                result.PropertiesInt = new Dictionary<PropertyInt, int>(biota.BiotaPropertiesInt.Count);
                foreach (var value in biota.BiotaPropertiesInt)
                    result.PropertiesInt[(PropertyInt)value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesInt64 != null && (instantiateEmptyCollections || biota.BiotaPropertiesInt64.Count > 0))
            {
                result.PropertiesInt64 = new Dictionary<PropertyInt64, long>(biota.BiotaPropertiesInt64.Count);
                foreach (var value in biota.BiotaPropertiesInt64)
                    result.PropertiesInt64[(PropertyInt64)value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesString != null && (instantiateEmptyCollections || biota.BiotaPropertiesString.Count > 0))
            {
                result.PropertiesString = new Dictionary<PropertyString, string>(biota.BiotaPropertiesString.Count);
                foreach (var value in biota.BiotaPropertiesString)
                    result.PropertiesString[(PropertyString)value.Type] = value.Value;
            }


            if (biota.BiotaPropertiesPosition != null && (instantiateEmptyCollections || biota.BiotaPropertiesPosition.Count > 0))
            {
                result.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>(biota.BiotaPropertiesPosition.Count);

                foreach (var record in biota.BiotaPropertiesPosition)
                {
                    var newEntity = new PropertiesPosition
                    {
                        ObjCellId = record.ObjCellId,
                        PositionX = record.OriginX,
                        PositionY = record.OriginY,
                        PositionZ = record.OriginZ,
                        RotationW = record.AnglesW,
                        RotationX = record.AnglesX,
                        RotationY = record.AnglesY,
                        RotationZ = record.AnglesZ,

                    };

                    result.PropertiesPosition[(PositionType)record.PositionType] = newEntity;
                }
            }


            if (biota.BiotaPropertiesSpellBook != null && (instantiateEmptyCollections || biota.BiotaPropertiesSpellBook.Count > 0))
            {
                result.PropertiesSpellBook = new Dictionary<int, float>(biota.BiotaPropertiesSpellBook.Count);
                foreach (var value in biota.BiotaPropertiesSpellBook)
                    result.PropertiesSpellBook[value.Spell] = value.Probability;
            }


            if (biota.BiotaPropertiesAnimPart != null && (instantiateEmptyCollections || biota.BiotaPropertiesAnimPart.Count > 0))
            {
                result.PropertiesAnimPart = new List<PropertiesAnimPart>(biota.BiotaPropertiesAnimPart.Count);

                foreach (var record in biota.BiotaPropertiesAnimPart.OrderBy(r => r.Order))
                {
                    var newEntity = new PropertiesAnimPart
                    {
                        Index = record.Index,
                        AnimationId = record.AnimationId,
                    };

                    result.PropertiesAnimPart.Add(newEntity);
                }
            }

            if (biota.BiotaPropertiesPalette != null && (instantiateEmptyCollections || biota.BiotaPropertiesPalette.Count > 0))
            {
                result.PropertiesPalette = new Collection<PropertiesPalette>();

                foreach (var record in biota.BiotaPropertiesPalette.OrderBy(r => r.Order))
                {
                    var newEntity = new PropertiesPalette
                    {
                        SubPaletteId = record.SubPaletteId,
                        Offset = record.Offset,
                        Length = record.Length,
                    };

                    result.PropertiesPalette.Add(newEntity);
                }
            }

            if (biota.BiotaPropertiesTextureMap != null && (instantiateEmptyCollections || biota.BiotaPropertiesTextureMap.Count > 0))
            {
                result.PropertiesTextureMap = new List<PropertiesTextureMap>(biota.BiotaPropertiesTextureMap.Count);

                foreach (var record in biota.BiotaPropertiesTextureMap.OrderBy(r => r.Order))
                {
                    var newEntity = new PropertiesTextureMap
                    {
                        PartIndex = record.Index,
                        OldTexture = record.OldId,
                        NewTexture = record.NewId,
                    };

                    result.PropertiesTextureMap.Add(newEntity);
                }
            }


            // Properties for all world objects that typically aren't modified over the original Biota

            if (biota.BiotaPropertiesCreateList != null && (instantiateEmptyCollections || biota.BiotaPropertiesCreateList.Count > 0))
            {
                result.PropertiesCreateList = new Collection<PropertiesCreateList>();

                foreach (var record in biota.BiotaPropertiesCreateList)
                {
                    var newEntity = new PropertiesCreateList
                    {
                        DatabaseRecordId = record.Id,

                        DestinationType = (DestinationType)record.DestinationType,
                        WeenieClassId = record.WeenieClassId,
                        StackSize = record.StackSize,
                        Palette = record.Palette,
                        Shade = record.Shade,
                        TryToBond = record.TryToBond,
                    };

                    result.PropertiesCreateList.Add(newEntity);
                }
            }

            if (biota.BiotaPropertiesEmote != null && (instantiateEmptyCollections || biota.BiotaPropertiesEmote.Count > 0))
            {
                result.PropertiesEmote = new Collection<PropertiesEmote>();

                foreach (var record in biota.BiotaPropertiesEmote)
                {
                    var newEntity = new PropertiesEmote
                    {
                        DatabaseRecordId = record.Id,

                        Category = (EmoteCategory)record.Category,
                        Probability = record.Probability,
                        WeenieClassId = record.WeenieClassId,
                        Style = (MotionStance?)record.Style,
                        Substyle = (MotionCommand?)record.Substyle,
                        Quest = record.Quest,
                        VendorType = (VendorType?)record.VendorType,
                        MinHealth = record.MinHealth,
                        MaxHealth = record.MaxHealth,
                    };

                    foreach (var record2 in record.BiotaPropertiesEmoteAction.OrderBy(r => r.Order))
                    {
                        var newEntity2 = new PropertiesEmoteAction
                        {
                            DatabaseRecordId = record2.Id,

                            Type = record2.Type,
                            Delay = record2.Delay,
                            Extent = record2.Extent,
                            Motion = (MotionCommand?)record2.Motion,
                            Message = record2.Message,
                            TestString = record2.TestString,
                            Min = record2.Min,
                            Max = record2.Max,
                            Min64 = record2.Min64,
                            Max64 = record2.Max64,
                            MinDbl = record2.MinDbl,
                            MaxDbl = record2.MaxDbl,
                            Stat = record2.Stat,
                            Display = record2.Display,
                            Amount = record2.Amount,
                            Amount64 = record2.Amount64,
                            HeroXP64 = record2.HeroXP64,
                            Percent = record2.Percent,
                            SpellId = record2.SpellId,
                            WealthRating = record2.WealthRating,
                            TreasureClass = record2.TreasureClass,
                            TreasureType = record2.TreasureType,
                            PScript = (PlayScript?)record2.PScript,
                            Sound = (Sound?)record2.Sound,
                            DestinationType = record2.DestinationType,
                            WeenieClassId = record2.WeenieClassId,
                            StackSize = record2.StackSize,
                            Palette = record2.Palette,
                            Shade = record2.Shade,
                            TryToBond = record2.TryToBond,
                            ObjCellId = record2.ObjCellId,
                            OriginX = record2.OriginX,
                            OriginY = record2.OriginY,
                            OriginZ = record2.OriginZ,
                            AnglesW = record2.AnglesW,
                            AnglesX = record2.AnglesX,
                            AnglesY = record2.AnglesY,
                            AnglesZ = record2.AnglesZ,
                        };

                        newEntity.PropertiesEmoteAction.Add(newEntity2);
                    }

                    result.PropertiesEmote.Add(newEntity);
                }
            }

            if (biota.BiotaPropertiesEventFilter != null && (instantiateEmptyCollections || biota.BiotaPropertiesEventFilter.Count > 0))
            {
                result.PropertiesEventFilter = new HashSet<int>();
                foreach (var value in biota.BiotaPropertiesEventFilter)
                    result.PropertiesEventFilter.Add(value.Event);
            }

            if (biota.BiotaPropertiesGenerator != null && (instantiateEmptyCollections || biota.BiotaPropertiesGenerator.Count > 0))
            {
                result.PropertiesGenerator = new List<PropertiesGenerator>(biota.BiotaPropertiesGenerator.Count);

                foreach (var record in biota.BiotaPropertiesGenerator) // TODO do we have the correct order?
                {
                    var newEntity = new PropertiesGenerator
                    {
                        DatabaseRecordId = record.Id,

                        Probability = record.Probability,
                        WeenieClassId = record.WeenieClassId,
                        Delay = record.Delay,
                        InitCreate = record.InitCreate,
                        MaxCreate = record.MaxCreate,
                        WhenCreate = (RegenerationType)record.WhenCreate,
                        WhereCreate = (RegenLocationType)record.WhereCreate,
                        StackSize = record.StackSize,
                        PaletteId = record.PaletteId,
                        Shade = record.Shade,
                        ObjCellId = record.ObjCellId,
                        OriginX = record.OriginX,
                        OriginY = record.OriginY,
                        OriginZ = record.OriginZ,
                        AnglesW = record.AnglesW,
                        AnglesX = record.AnglesX,
                        AnglesY = record.AnglesY,
                        AnglesZ = record.AnglesZ,
                    };

                    result.PropertiesGenerator.Add(newEntity);
                }
            }


            // Properties for creatures

            if (biota.BiotaPropertiesAttribute != null && (instantiateEmptyCollections || biota.BiotaPropertiesAttribute.Count > 0))
            {
                result.PropertiesAttribute = new Dictionary<PropertyAttribute, PropertiesAttribute>(biota.BiotaPropertiesAttribute.Count);

                foreach (var record in biota.BiotaPropertiesAttribute)
                {
                    var newEntity = new PropertiesAttribute
                    {
                        InitLevel = record.InitLevel,
                        LevelFromCP = record.LevelFromCP,
                        CPSpent = record.CPSpent,
                    };

                    result.PropertiesAttribute[(PropertyAttribute)record.Type] = newEntity;
                }
            }

            if (biota.BiotaPropertiesAttribute2nd != null && (instantiateEmptyCollections || biota.BiotaPropertiesAttribute2nd.Count > 0))
            {
                result.PropertiesAttribute2nd = new Dictionary<PropertyAttribute2nd, PropertiesAttribute2nd>(biota.BiotaPropertiesAttribute2nd.Count);

                foreach (var record in biota.BiotaPropertiesAttribute2nd)
                {
                    var newEntity = new PropertiesAttribute2nd
                    {
                        InitLevel = record.InitLevel,
                        LevelFromCP = record.LevelFromCP,
                        CPSpent = record.CPSpent,
                        CurrentLevel = record.CurrentLevel,
                    };

                    result.PropertiesAttribute2nd[(PropertyAttribute2nd)record.Type] = newEntity;
                }
            }

            if (biota.BiotaPropertiesBodyPart != null && (instantiateEmptyCollections || biota.BiotaPropertiesBodyPart.Count > 0))
            {
                result.PropertiesBodyPart = new Dictionary<CombatBodyPart, PropertiesBodyPart>(biota.BiotaPropertiesBodyPart.Count);

                foreach (var record in biota.BiotaPropertiesBodyPart)
                {
                    var newEntity = new PropertiesBodyPart
                    {
                        DType = (DamageType)record.DType,
                        DVal = record.DVal,
                        DVar = record.DVar,
                        BaseArmor = record.BaseArmor,
                        ArmorVsSlash = record.ArmorVsSlash,
                        ArmorVsPierce = record.ArmorVsPierce,
                        ArmorVsBludgeon = record.ArmorVsBludgeon,
                        ArmorVsCold = record.ArmorVsCold,
                        ArmorVsFire = record.ArmorVsFire,
                        ArmorVsAcid = record.ArmorVsAcid,
                        ArmorVsElectric = record.ArmorVsElectric,
                        ArmorVsNether = record.ArmorVsNether,
                        BH = record.BH,
                        HLF = record.HLF,
                        MLF = record.MLF,
                        LLF = record.LLF,
                        HRF = record.HRF,
                        MRF = record.MRF,
                        LRF = record.LRF,
                        HLB = record.HLB,
                        MLB = record.MLB,
                        LLB = record.LLB,
                        HRB = record.HRB,
                        MRB = record.MRB,
                        LRB = record.LRB,
                    };

                    result.PropertiesBodyPart[(CombatBodyPart)record.Key] = newEntity;
                }
            }

            if (biota.BiotaPropertiesSkill != null && (instantiateEmptyCollections || biota.BiotaPropertiesSkill.Count > 0))
            {
                result.PropertiesSkill = new Dictionary<Skill, PropertiesSkill>(biota.BiotaPropertiesSkill.Count);

                foreach (var record in biota.BiotaPropertiesSkill)
                {
                    var newEntity = new PropertiesSkill
                    {
                        LevelFromPP = record.LevelFromPP,
                        SAC = (SkillAdvancementClass)record.SAC,
                        PP = record.PP,
                        InitLevel = record.InitLevel,
                        ResistanceAtLastCheck = record.ResistanceAtLastCheck,
                        LastUsedTime = record.LastUsedTime,
                    };

                    result.PropertiesSkill[(Skill)record.Type] = newEntity;
                }
            }


            // Properties for books

            if (biota.BiotaPropertiesBook != null)
            {
                result.PropertiesBook = new PropertiesBook
                {
                    MaxNumPages = biota.BiotaPropertiesBook.MaxNumPages,
                    MaxNumCharsPerPage = biota.BiotaPropertiesBook.MaxNumCharsPerPage,
                };
            }

            if (biota.BiotaPropertiesBookPageData != null && (instantiateEmptyCollections || biota.BiotaPropertiesBookPageData.Count > 0))
            {
                result.PropertiesBookPageData = new List<PropertiesBookPageData>(biota.BiotaPropertiesBookPageData.Count);

                foreach (var record in biota.BiotaPropertiesBookPageData.OrderBy(r => r.PageId))
                {
                    var newEntity = new PropertiesBookPageData
                    {
                        AuthorId = record.AuthorId,
                        AuthorName = record.AuthorName,
                        AuthorAccount = record.AuthorAccount,
                        IgnoreAuthor = record.IgnoreAuthor,
                        PageText = record.PageText,
                    };

                    result.PropertiesBookPageData.Add(newEntity);
                }
            }


            // Biota additions over Weenie

            if (biota.BiotaPropertiesAllegiance != null && (instantiateEmptyCollections || biota.BiotaPropertiesAllegiance.Count > 0))
            {
                result.PropertiesAllegiance = new Dictionary<uint, PropertiesAllegiance>(biota.BiotaPropertiesAllegiance.Count);

                foreach (var record in biota.BiotaPropertiesAllegiance)
                {
                    var newEntity = new PropertiesAllegiance
                    {
                        Banned = record.Banned,
                        ApprovedVassal = record.ApprovedVassal,
                    };

                    result.PropertiesAllegiance[record.CharacterId] = newEntity;
                }
            }

            if (biota.BiotaPropertiesEnchantmentRegistry != null && (instantiateEmptyCollections || biota.BiotaPropertiesEnchantmentRegistry.Count > 0))
            {
                result.PropertiesEnchantmentRegistry = new Collection<PropertiesEnchantmentRegistry>();

                foreach (var record in biota.BiotaPropertiesEnchantmentRegistry)
                {
                    var newEntity = new PropertiesEnchantmentRegistry
                    {
                        EnchantmentCategory = record.EnchantmentCategory,
                        SpellId = record.SpellId,
                        LayerId = record.LayerId,
                        HasSpellSetId = record.HasSpellSetId,
                        SpellCategory = (SpellCategory)record.SpellCategory,
                        PowerLevel = record.PowerLevel,
                        StartTime = record.StartTime,
                        Duration = record.Duration,
                        CasterObjectId = record.CasterObjectId,
                        DegradeModifier = record.DegradeModifier,
                        DegradeLimit = record.DegradeLimit,
                        LastTimeDegraded = record.LastTimeDegraded,
                        StatModType = (EnchantmentTypeFlags)record.StatModType,
                        StatModKey = record.StatModKey,
                        StatModValue = record.StatModValue,
                        SpellSetId = (EquipmentSet)record.SpellSetId,
                    };

                    result.PropertiesEnchantmentRegistry.Add(newEntity);
                }
            }

            if (biota.HousePermission != null && (instantiateEmptyCollections || biota.HousePermission.Count > 0))
            {
                result.HousePermissions = new Dictionary<uint, bool>(biota.HousePermission.Count);

                foreach (var record in biota.HousePermission)
                    result.HousePermissions[record.PlayerGuid] = record.Storage;
            }


            return result;
        }

        public static ACE.Database.Models.Shard.Biota ConvertFromEntityBiota(ACE.Entity.Models.Biota biota, bool includeDatabaseRecordIds = false)
        {
            var result = new ACE.Database.Models.Shard.Biota();

            result.Id = biota.Id;
            result.WeenieClassId = biota.WeenieClassId;
            result.WeenieType = (int)biota.WeenieType;


            if (biota.PropertiesBool != null)
            {
                foreach (var kvp in biota.PropertiesBool)
                    result.SetProperty(kvp.Key, kvp.Value);
            }
            if (biota.PropertiesDID != null)
            {
                foreach (var kvp in biota.PropertiesDID)
                    result.SetProperty(kvp.Key, kvp.Value);
            }
            if (biota.PropertiesFloat != null)
            {
                foreach (var kvp in biota.PropertiesFloat)
                    result.SetProperty(kvp.Key, kvp.Value);
            }
            if (biota.PropertiesIID != null)
            {
                foreach (var kvp in biota.PropertiesIID)
                    result.SetProperty(kvp.Key, kvp.Value);
            }
            if (biota.PropertiesInt != null)
            {
                foreach (var kvp in biota.PropertiesInt)
                    result.SetProperty(kvp.Key, kvp.Value);
            }
            if (biota.PropertiesInt64 != null)
            {
                foreach (var kvp in biota.PropertiesInt64)
                    result.SetProperty(kvp.Key, kvp.Value);
            }
            if (biota.PropertiesString != null)
            {
                foreach (var kvp in biota.PropertiesString)
                    result.SetProperty(kvp.Key, kvp.Value);
            }
 

            if (biota.PropertiesPosition != null)
            {
                foreach (var kvp in biota.PropertiesPosition)
                {
                    var entity = new BiotaPropertiesPosition { ObjectId = biota.Id, PositionType = (ushort)kvp.Key, ObjCellId = kvp.Value.ObjCellId, OriginX = kvp.Value.PositionX, OriginY = kvp.Value.PositionY, OriginZ = kvp.Value.PositionZ, AnglesW = kvp.Value.RotationW, AnglesX = kvp.Value.RotationX, AnglesY = kvp.Value.RotationY, AnglesZ = kvp.Value.RotationZ };

                    result.BiotaPropertiesPosition.Add(entity);
                }
            }


            if (biota.PropertiesSpellBook != null)
            {
                foreach (var kvp in biota.PropertiesSpellBook)
                {
                    var entity = new BiotaPropertiesSpellBook { ObjectId = biota.Id, Spell = kvp.Key, Probability = kvp.Value };

                    result.BiotaPropertiesSpellBook.Add(entity);
                }
            }


            if (biota.PropertiesAnimPart != null)
            {
                for (int i = 0; i < biota.PropertiesAnimPart.Count; i++)
                {
                    var value = biota.PropertiesAnimPart[i];

                    var entity = new BiotaPropertiesAnimPart { ObjectId = biota.Id, Index = value.Index, AnimationId = value.AnimationId, Order = (byte)i };

                    result.BiotaPropertiesAnimPart.Add(entity);
                }
            }

            if (biota.PropertiesPalette != null)
            {
                for (int i = 0; i < biota.PropertiesPalette.Count; i++)
                {
                    var value = biota.PropertiesPalette[i];

                    var entity = new BiotaPropertiesPalette { ObjectId = biota.Id, SubPaletteId = value.SubPaletteId, Offset = value.Offset, Length = value.Length, Order = (byte)i };

                    result.BiotaPropertiesPalette.Add(entity);
                }
            }

            if (biota.PropertiesTextureMap != null)
            {
                for (int i = 0; i < biota.PropertiesTextureMap.Count ; i++)
                {
                    var value = biota.PropertiesTextureMap[i];

                    var entity = new BiotaPropertiesTextureMap { ObjectId = biota.Id, Index = value.PartIndex, OldId = value.OldTexture, NewId = value.NewTexture, Order = (byte)i };

                    result.BiotaPropertiesTextureMap.Add(entity);
                }
            }


            // Properties for all world objects that typically aren't modified over the original weenie

            if (biota.PropertiesCreateList != null)
            {
                foreach (var value in biota.PropertiesCreateList)
                {
                    var entity = new BiotaPropertiesCreateList
                    {
                        ObjectId = biota.Id,
                        DestinationType = (sbyte)value.DestinationType,
                        WeenieClassId = value.WeenieClassId,
                        StackSize = value.StackSize,
                        Palette = value.Palette,
                        Shade = value.Shade,
                        TryToBond = value.TryToBond
                    };

                    if (includeDatabaseRecordIds)
                        entity.Id = value.DatabaseRecordId;

                    result.BiotaPropertiesCreateList.Add(entity);
                }
            }

            if (biota.PropertiesEmote != null)
            {
                foreach (var value in biota.PropertiesEmote)
                {
                    var entity = new BiotaPropertiesEmote
                    {
                        ObjectId = biota.Id,
                        Category = (uint)value.Category,
                        Probability = value.Probability,
                        WeenieClassId = value.WeenieClassId,
                        Style = (uint?)value.Style,
                        Substyle = (uint?)value.Substyle,
                        Quest = value.Quest,
                        VendorType = (int?)value.VendorType,
                        MinHealth = value.MinHealth,
                        MaxHealth = value.MaxHealth,
                    };

                    if (includeDatabaseRecordIds)
                        entity.Id = value.DatabaseRecordId;

                    foreach (var value2 in value.PropertiesEmoteAction)
                    {
                        var entity2 = new BiotaPropertiesEmoteAction
                        {
                            // EmoteId is a foreign key to Emote.Id.
                            // If we don't set this to a non-zero number, EF will not auto-set this for us when we add this biota to the database.
                            // We set it to uint.MaxValue instead of 1 because 1 is more likely to be a valid foreign key. We don't want to enter a valid foreign key.
                            // We just want to enter a value that forces EF to update the record with the correct foreign key. If this behavior changes in the future and we set it to 1,
                            // we're more likely to run into an unnoticed issue (because 1 would not throw an exception and uint.MaxValue probably would).
                            // We put this here instead of in ShardDatabase for efficiency.
                            // It's possible this might be fixable with a attribute in the Emote or EmoteAction classes.
                            // It's also possible we don't have the schema defined in a way that helps scaffolding identify the relationship.
                            // Mag-nus 2018-08-04
                            EmoteId = uint.MaxValue,

                            Order = (uint)value.PropertiesEmoteAction.IndexOf(value2),
                            Type = value2.Type,
                            Delay = value2.Delay,
                            Extent = value2.Extent,
                            Motion = (int?)value2.Motion,
                            Message = value2.Message,
                            TestString = value2.TestString,
                            Min = value2.Min,
                            Max = value2.Max,
                            Min64 = value2.Min64,
                            Max64 = value2.Max64,
                            MinDbl = value2.MinDbl,
                            MaxDbl = value2.MaxDbl,
                            Stat = value2.Stat,
                            Display = value2.Display,
                            Amount = value2.Amount,
                            Amount64 = value2.Amount64,
                            HeroXP64 = value2.HeroXP64,
                            Percent = value2.Percent,
                            SpellId = value2.SpellId,
                            WealthRating = value2.WealthRating,
                            TreasureClass = value2.TreasureClass,
                            TreasureType = value2.TreasureType,
                            PScript = (int?)value2.PScript,
                            Sound = (int?)value2.Sound,
                            DestinationType = value2.DestinationType,
                            WeenieClassId = value2.WeenieClassId,
                            StackSize = value2.StackSize,
                            Palette = value2.Palette,
                            Shade = value2.Shade,
                            TryToBond = value2.TryToBond,
                            ObjCellId = value2.ObjCellId,
                            OriginX = value2.OriginX,
                            OriginY = value2.OriginY,
                            OriginZ = value2.OriginZ,
                            AnglesW = value2.AnglesW,
                            AnglesX = value2.AnglesX,
                            AnglesY = value2.AnglesY,
                            AnglesZ = value2.AnglesZ,
                        };

                        if (includeDatabaseRecordIds)
                            entity2.Id = value2.DatabaseRecordId;

                        entity.BiotaPropertiesEmoteAction.Add(entity2);
                    }

                    result.BiotaPropertiesEmote.Add(entity);
                }
            }

            if (biota.PropertiesEventFilter != null)
            {
                foreach (var value in biota.PropertiesEventFilter)
                {
                    var entity = new BiotaPropertiesEventFilter { ObjectId = biota.Id, Event = value };

                    result.BiotaPropertiesEventFilter.Add(entity);
                }
            }

            if (biota.PropertiesGenerator != null)
            {
                foreach (var value in biota.PropertiesGenerator)
                {
                    var entity = new BiotaPropertiesGenerator
                    {
                        ObjectId = biota.Id,
                        Probability = value.Probability,
                        WeenieClassId = value.WeenieClassId,
                        Delay = value.Delay,
                        InitCreate = value.InitCreate,
                        MaxCreate = value.MaxCreate,
                        WhenCreate = (uint)value.WhenCreate,
                        WhereCreate = (uint)value.WhereCreate,
                        StackSize = value.StackSize,
                        PaletteId = value.PaletteId,
                        Shade = value.Shade,
                        ObjCellId = value.ObjCellId,
                        OriginX = value.OriginX,
                        OriginY = value.OriginY,
                        OriginZ = value.OriginZ,
                        AnglesW = value.AnglesW,
                        AnglesX = value.AnglesX,
                        AnglesY = value.AnglesY,
                        AnglesZ = value.AnglesZ,
                    };

                    if (includeDatabaseRecordIds)
                        entity.Id = value.DatabaseRecordId;

                    result.BiotaPropertiesGenerator.Add(entity);
                }
            }


            // Properties for creatures

            if (biota.PropertiesAttribute != null)
            {
                foreach (var kvp in biota.PropertiesAttribute)
                {
                    var entity = new BiotaPropertiesAttribute { ObjectId = biota.Id, Type = (ushort)kvp.Key, InitLevel = kvp.Value.InitLevel, LevelFromCP = kvp.Value.LevelFromCP, CPSpent = kvp.Value.CPSpent };

                    result.BiotaPropertiesAttribute.Add(entity);
                }
            }

            if (biota.PropertiesAttribute2nd != null)
            {
                foreach (var kvp in biota.PropertiesAttribute2nd)
                {
                    var entity = new BiotaPropertiesAttribute2nd { ObjectId = biota.Id, Type = (ushort)kvp.Key, InitLevel = kvp.Value.InitLevel, LevelFromCP = kvp.Value.LevelFromCP, CPSpent = kvp.Value.CPSpent, CurrentLevel = kvp.Value.CurrentLevel };

                    result.BiotaPropertiesAttribute2nd.Add(entity);
                }
            }

            if (biota.PropertiesBodyPart != null)
            {
                foreach (var kvp in biota.PropertiesBodyPart)
                {
                    var entity = new BiotaPropertiesBodyPart
                    {
                        ObjectId = biota.Id,
                        Key = (ushort)kvp.Key,
                        DType = (int)kvp.Value.DType,
                        DVal = kvp.Value.DVal,
                        DVar = kvp.Value.DVar,
                        BaseArmor = kvp.Value.BaseArmor,
                        ArmorVsSlash = kvp.Value.ArmorVsSlash,
                        ArmorVsPierce = kvp.Value.ArmorVsPierce,
                        ArmorVsBludgeon = kvp.Value.ArmorVsBludgeon,
                        ArmorVsCold = kvp.Value.ArmorVsCold,
                        ArmorVsFire = kvp.Value.ArmorVsFire,
                        ArmorVsAcid = kvp.Value.ArmorVsAcid,
                        ArmorVsElectric = kvp.Value.ArmorVsElectric,
                        ArmorVsNether = kvp.Value.ArmorVsNether,
                        BH = kvp.Value.BH,
                        HLF = kvp.Value.HLF,
                        MLF = kvp.Value.MLF,
                        LLF = kvp.Value.LLF,
                        HRF = kvp.Value.HRF,
                        MRF = kvp.Value.MRF,
                        LRF = kvp.Value.LRF,
                        HLB = kvp.Value.HLB,
                        MLB = kvp.Value.MLB,
                        LLB = kvp.Value.LLB,
                        HRB = kvp.Value.HRB,
                        MRB = kvp.Value.MRB,
                        LRB = kvp.Value.LRB,
                    };

                    result.BiotaPropertiesBodyPart.Add(entity);
                }
            }

            if (biota.PropertiesSkill != null)
            {
                foreach (var kvp in biota.PropertiesSkill)
                {
                    var entity = new BiotaPropertiesSkill
                    {
                        ObjectId = biota.Id,
                        Type = (ushort)kvp.Key,
                        LevelFromPP = kvp.Value.LevelFromPP,
                        SAC = (uint)kvp.Value.SAC,
                        PP = kvp.Value.PP,
                        InitLevel = kvp.Value.InitLevel,
                        ResistanceAtLastCheck = kvp.Value.ResistanceAtLastCheck,
                        LastUsedTime = kvp.Value.LastUsedTime,
                    };

                    result.BiotaPropertiesSkill.Add(entity);
                }
            }


            // Properties for books

            if (biota.PropertiesBook != null)
            {
                result.BiotaPropertiesBook = new BiotaPropertiesBook
                {
                    ObjectId = biota.Id,
                    MaxNumPages = biota.PropertiesBook.MaxNumPages,
                    MaxNumCharsPerPage = biota.PropertiesBook.MaxNumCharsPerPage
                };
            }

            if (biota.PropertiesBookPageData != null)
            {
                foreach (var value in biota.PropertiesBookPageData)
                {
                    var entity = new BiotaPropertiesBookPageData
                    {
                        ObjectId = biota.Id,
                        PageId = (uint)biota.PropertiesBookPageData.IndexOf(value),
                        AuthorId = value.AuthorId,
                        AuthorName = value.AuthorName,
                        AuthorAccount = value.AuthorAccount,
                        IgnoreAuthor = value.IgnoreAuthor,
                        PageText = value.PageText
                    };

                    result.BiotaPropertiesBookPageData.Add(entity);
                }
            }



            // Biota additions over Weenie

            if (biota.PropertiesAllegiance != null)
            {
                foreach (var kvp in biota.PropertiesAllegiance)
                {
                    var entity = new BiotaPropertiesAllegiance { AllegianceId = biota.Id, CharacterId = kvp.Key, Banned = kvp.Value.Banned, ApprovedVassal = kvp.Value.ApprovedVassal };

                    result.BiotaPropertiesAllegiance.Add(entity);
                }
            }

            if (biota.PropertiesEnchantmentRegistry != null)
            {
                foreach (var value in biota.PropertiesEnchantmentRegistry)
                {
                    var entity = new BiotaPropertiesEnchantmentRegistry
                    {
                        ObjectId = biota.Id,
                        EnchantmentCategory = value.EnchantmentCategory,
                        SpellId = value.SpellId,
                        LayerId = value.LayerId,
                        HasSpellSetId = value.HasSpellSetId,
                        SpellCategory = (ushort)value.SpellCategory,
                        PowerLevel = value.PowerLevel,
                        StartTime = value.StartTime,
                        Duration = value.Duration,
                        CasterObjectId = value.CasterObjectId,
                        DegradeModifier = value.DegradeModifier,
                        DegradeLimit = value.DegradeLimit,
                        LastTimeDegraded = value.LastTimeDegraded,
                        StatModType = (uint)value.StatModType,
                        StatModKey = value.StatModKey,
                        StatModValue = value.StatModValue,
                        SpellSetId = (uint)value.SpellSetId,
                    };

                    result.BiotaPropertiesEnchantmentRegistry.Add(entity);
                }
            }

            if (biota.HousePermissions != null)
            {
                foreach (var kvp in biota.HousePermissions)
                {
                    var entity = new ACE.Database.Models.Shard.HousePermission { HouseId = biota.Id, PlayerGuid = kvp.Key, Storage = kvp.Value };

                    result.HousePermission.Add(entity);
                }
            }

            return result;
        }
    }
}
