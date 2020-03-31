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
    public static class WeenieConverter
    {
        public static ACE.Entity.Models.Weenie ConvertToEntityWeenie(ACE.Database.Models.World.Weenie weenie, bool instantiateEmptyCollections = false)
        {
            var result = new ACE.Entity.Models.Weenie();

            result.WeenieClassId = weenie.ClassId;
            result.ClassName = weenie.ClassName;
            result.WeenieType = (WeenieType)weenie.Type;

            if (weenie.WeeniePropertiesBool != null && (instantiateEmptyCollections || weenie.WeeniePropertiesBool.Count > 0))
            {
                result.PropertiesBool = new Dictionary<PropertyBool, bool>(weenie.WeeniePropertiesBool.Count);
                foreach (var value in weenie.WeeniePropertiesBool)
                    result.PropertiesBool[(PropertyBool)value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesDID != null && (instantiateEmptyCollections || weenie.WeeniePropertiesDID.Count > 0))
            {
                result.PropertiesDID = new Dictionary<PropertyDataId, uint>(weenie.WeeniePropertiesDID.Count);
                foreach (var value in weenie.WeeniePropertiesDID)
                    result.PropertiesDID[(PropertyDataId)value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesFloat != null && (instantiateEmptyCollections || weenie.WeeniePropertiesFloat.Count > 0))
            {
                result.PropertiesFloat = new Dictionary<PropertyFloat, double>(weenie.WeeniePropertiesFloat.Count);
                foreach (var value in weenie.WeeniePropertiesFloat)
                    result.PropertiesFloat[(PropertyFloat)value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesIID != null && (instantiateEmptyCollections || weenie.WeeniePropertiesIID.Count > 0))
            {
                result.PropertiesIID = new Dictionary<PropertyInstanceId, uint>(weenie.WeeniePropertiesIID.Count);
                foreach (var value in weenie.WeeniePropertiesIID)
                    result.PropertiesIID[(PropertyInstanceId)value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesInt != null && (instantiateEmptyCollections || weenie.WeeniePropertiesInt.Count > 0))
            {
                result.PropertiesInt = new Dictionary<PropertyInt, int>(weenie.WeeniePropertiesInt.Count);
                foreach (var value in weenie.WeeniePropertiesInt)
                    result.PropertiesInt[(PropertyInt)value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesInt64 != null && (instantiateEmptyCollections || weenie.WeeniePropertiesInt64.Count > 0))
            {
                result.PropertiesInt64 = new Dictionary<PropertyInt64, long>(weenie.WeeniePropertiesInt64.Count);
                foreach (var value in weenie.WeeniePropertiesInt64)
                    result.PropertiesInt64[(PropertyInt64)value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesString != null && (instantiateEmptyCollections || weenie.WeeniePropertiesString.Count > 0))
            {
                result.PropertiesString = new Dictionary<PropertyString, string>(weenie.WeeniePropertiesString.Count);
                foreach (var value in weenie.WeeniePropertiesString)
                    result.PropertiesString[(PropertyString)value.Type] = value.Value;
            }


            if (weenie.WeeniePropertiesPosition != null && (instantiateEmptyCollections || weenie.WeeniePropertiesPosition.Count > 0))
            {
                result.PropertiesPosition = new Dictionary<PositionType, PropertiesPosition>(weenie.WeeniePropertiesPosition.Count);

                foreach (var record in weenie.WeeniePropertiesPosition)
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


            if (weenie.WeeniePropertiesSpellBook != null && (instantiateEmptyCollections || weenie.WeeniePropertiesSpellBook.Count > 0))
            {
                result.PropertiesSpellBook = new Dictionary<int, float>(weenie.WeeniePropertiesSpellBook.Count);
                foreach (var value in weenie.WeeniePropertiesSpellBook)
                    result.PropertiesSpellBook[value.Spell] = value.Probability;
            }


            if (weenie.WeeniePropertiesAnimPart != null && (instantiateEmptyCollections || weenie.WeeniePropertiesAnimPart.Count > 0))
            {
                result.PropertiesAnimPart = new List<PropertiesAnimPart>(weenie.WeeniePropertiesAnimPart.Count);

                foreach (var record in weenie.WeeniePropertiesAnimPart)
                {
                    var newEntity = new PropertiesAnimPart
                    {
                        Index = record.Index,
                        AnimationId = record.AnimationId,
                    };

                    result.PropertiesAnimPart.Add(newEntity);
                }
            }

            if (weenie.WeeniePropertiesPalette != null && (instantiateEmptyCollections || weenie.WeeniePropertiesPalette.Count > 0))
            {
                result.PropertiesPalette = new Collection<PropertiesPalette>();

                foreach (var record in weenie.WeeniePropertiesPalette)
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

            if (weenie.WeeniePropertiesTextureMap != null && (instantiateEmptyCollections || weenie.WeeniePropertiesTextureMap.Count > 0))
            {
                result.PropertiesTextureMap = new List<PropertiesTextureMap>(weenie.WeeniePropertiesTextureMap.Count);

                foreach (var record in weenie.WeeniePropertiesTextureMap)
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


            // Properties for all world objects that typically aren't modified over the original weenie

            if (weenie.WeeniePropertiesCreateList != null && (instantiateEmptyCollections || weenie.WeeniePropertiesCreateList.Count > 0))
            {
                result.PropertiesCreateList = new Collection<PropertiesCreateList>();

                foreach (var record in weenie.WeeniePropertiesCreateList)
                {
                    var newEntity = new PropertiesCreateList
                    {
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

            if (weenie.WeeniePropertiesEmote != null && (instantiateEmptyCollections || weenie.WeeniePropertiesEmote.Count > 0))
            {
                result.PropertiesEmote = new Collection<PropertiesEmote>();

                foreach (var record in weenie.WeeniePropertiesEmote)
                {
                    var newEntity = new PropertiesEmote
                    {
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

                    foreach (var record2 in record.WeeniePropertiesEmoteAction.OrderBy(r => r.Order))
                    {
                        var newEntity2 = new PropertiesEmoteAction
                        {
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

            if (weenie.WeeniePropertiesEventFilter != null && (instantiateEmptyCollections || weenie.WeeniePropertiesEventFilter.Count > 0))
            {
                result.PropertiesEventFilter = new HashSet<int>();
                foreach (var value in weenie.WeeniePropertiesEventFilter)
                    result.PropertiesEventFilter.Add(value.Event);
            }

            if (weenie.WeeniePropertiesGenerator != null && (instantiateEmptyCollections || weenie.WeeniePropertiesGenerator.Count > 0))
            {
                result.PropertiesGenerator = new List<PropertiesGenerator>(weenie.WeeniePropertiesGenerator.Count);

                foreach (var record in weenie.WeeniePropertiesGenerator) // TODO do we have the correct order?
                {
                    var newEntity = new PropertiesGenerator
                    {
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

            if (weenie.WeeniePropertiesAttribute != null && (instantiateEmptyCollections || weenie.WeeniePropertiesAttribute.Count > 0))
            {
                result.PropertiesAttribute = new Dictionary<PropertyAttribute, PropertiesAttribute>(weenie.WeeniePropertiesAttribute.Count);

                foreach (var record in weenie.WeeniePropertiesAttribute)
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

            if (weenie.WeeniePropertiesAttribute2nd != null && (instantiateEmptyCollections || weenie.WeeniePropertiesAttribute2nd.Count > 0))
            {
                result.PropertiesAttribute2nd = new Dictionary<PropertyAttribute2nd, PropertiesAttribute2nd>(weenie.WeeniePropertiesAttribute2nd.Count);

                foreach (var record in weenie.WeeniePropertiesAttribute2nd)
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

            if (weenie.WeeniePropertiesBodyPart != null && (instantiateEmptyCollections || weenie.WeeniePropertiesBodyPart.Count > 0))
            {
                result.PropertiesBodyPart = new Dictionary<CombatBodyPart, PropertiesBodyPart>(weenie.WeeniePropertiesBodyPart.Count);

                foreach (var record in weenie.WeeniePropertiesBodyPart)
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

            if (weenie.WeeniePropertiesSkill != null && (instantiateEmptyCollections || weenie.WeeniePropertiesSkill.Count > 0))
            {
                result.PropertiesSkill = new Dictionary<Skill, PropertiesSkill>(weenie.WeeniePropertiesSkill.Count);

                foreach (var record in weenie.WeeniePropertiesSkill)
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

            if (weenie.WeeniePropertiesBook != null)
            {
                result.PropertiesBook = new PropertiesBook
                {
                    MaxNumPages = weenie.WeeniePropertiesBook.MaxNumPages,
                    MaxNumCharsPerPage = weenie.WeeniePropertiesBook.MaxNumCharsPerPage,
                };
            }

            if (weenie.WeeniePropertiesBookPageData != null && (instantiateEmptyCollections || weenie.WeeniePropertiesBookPageData.Count > 0))
            {
                result.PropertiesBookPageData = new List<PropertiesBookPageData>(weenie.WeeniePropertiesBookPageData.Count);

                foreach (var record in weenie.WeeniePropertiesBookPageData.OrderBy(r => r.PageId))
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

            return result;
        }

        public static ACE.Database.Models.World.Weenie ConvertFromEntityWeenie(ACE.Entity.Models.Weenie weenie)
        {
            throw new NotImplementedException();
        }

        public static ACE.Database.Models.Shard.Biota ConvertToDatabaseBiota(ACE.Database.Models.World.Weenie weenie, uint id)
        {
            var result = new ACE.Database.Models.Shard.Biota();

            result.Id = id;
            result.WeenieClassId = weenie.ClassId;
            result.WeenieType = weenie.Type;

            foreach (var value in weenie.WeeniePropertiesBool)
            {
                result.BiotaPropertiesBool.Add(new BiotaPropertiesBool
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }
            foreach (var value in weenie.WeeniePropertiesDID)
            {
                result.BiotaPropertiesDID.Add(new BiotaPropertiesDID
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }
            foreach (var value in weenie.WeeniePropertiesFloat)
            {
                result.BiotaPropertiesFloat.Add(new BiotaPropertiesFloat
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }
            foreach (var value in weenie.WeeniePropertiesIID)
            {
                result.BiotaPropertiesIID.Add(new BiotaPropertiesIID
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }
            foreach (var value in weenie.WeeniePropertiesInt)
            {
                result.BiotaPropertiesInt.Add(new BiotaPropertiesInt
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }
            foreach (var value in weenie.WeeniePropertiesInt64)
            {
                result.BiotaPropertiesInt64.Add(new BiotaPropertiesInt64
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }
            foreach (var value in weenie.WeeniePropertiesString)
            {
                result.BiotaPropertiesString.Add(new BiotaPropertiesString
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }


            foreach (var value in weenie.WeeniePropertiesPosition)
            {
                result.BiotaPropertiesPosition.Add(new BiotaPropertiesPosition
                {
                    ObjectId = result.Id,
                    PositionType = value.PositionType,
                    ObjCellId = value.ObjCellId,
                    OriginX = value.OriginX,
                    OriginY = value.OriginY,
                    OriginZ = value.OriginZ,
                    AnglesW = value.AnglesW,
                    AnglesX = value.AnglesX,
                    AnglesY = value.AnglesY,
                    AnglesZ = value.AnglesZ,
                });
            }


            foreach (var value in weenie.WeeniePropertiesSpellBook)
            {
                result.BiotaPropertiesSpellBook.Add(new BiotaPropertiesSpellBook
                {
                    ObjectId = result.Id,
                    Spell = value.Spell,
                    Probability = value.Probability,
                });
            }



            foreach (var value in weenie.WeeniePropertiesAnimPart)
            {
                result.BiotaPropertiesAnimPart.Add(new BiotaPropertiesAnimPart
                {
                    ObjectId = result.Id,
                    Index = value.Index,
                    AnimationId = value.AnimationId
                });
            }

            foreach (var value in weenie.WeeniePropertiesPalette)
            {
                result.BiotaPropertiesPalette.Add(new BiotaPropertiesPalette
                {
                    ObjectId = result.Id,
                    SubPaletteId = value.SubPaletteId,
                    Offset = value.Offset,
                    Length = value.Length,
                });
            }

            foreach (var value in weenie.WeeniePropertiesTextureMap)
            {
                result.BiotaPropertiesTextureMap.Add(new BiotaPropertiesTextureMap
                {
                    ObjectId = result.Id,
                    Index = value.Index,
                    OldId = value.OldId,
                    NewId = value.NewId,
                });
            }


            // Properties for all world objects that typically aren't modified over the original weenie

            foreach (var value in weenie.WeeniePropertiesCreateList)
            {
                result.BiotaPropertiesCreateList.Add(new BiotaPropertiesCreateList
                {
                    ObjectId = result.Id,
                    DestinationType = value.DestinationType,
                    WeenieClassId = value.WeenieClassId,
                    StackSize = value.StackSize,
                    Palette = value.Palette,
                    Shade = value.Shade,
                    TryToBond = value.TryToBond,
                });
            }

            foreach (var value in weenie.WeeniePropertiesEmote)
            {
                var emote = new BiotaPropertiesEmote
                {
                    ObjectId = result.Id,
                    Category = value.Category,
                    Probability = value.Probability,
                    WeenieClassId = value.WeenieClassId,
                    Style = value.Style,
                    Substyle = value.Substyle,
                    Quest = value.Quest,
                    VendorType = value.VendorType,
                    MinHealth = value.MinHealth,
                    MaxHealth = value.MaxHealth,
                };

                foreach (var value2 in value.WeeniePropertiesEmoteAction)
                {
                    var action = new BiotaPropertiesEmoteAction
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

                        Order = value2.Order,
                        Type = value2.Type,
                        Delay = value2.Delay,
                        Extent = value2.Extent,
                        Motion = value2.Motion,
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
                        PScript = value2.PScript,
                        Sound = value2.Sound,
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

                    emote.BiotaPropertiesEmoteAction.Add(action);
                }

                result.BiotaPropertiesEmote.Add(emote);
            }

            foreach (var value in weenie.WeeniePropertiesEventFilter)
            {
                result.BiotaPropertiesEventFilter.Add(new BiotaPropertiesEventFilter
                {
                    ObjectId = result.Id,
                    Event = value.Event,
                });
            }

            foreach (var value in weenie.WeeniePropertiesGenerator)
            {
                result.BiotaPropertiesGenerator.Add(new BiotaPropertiesGenerator
                {
                    ObjectId = result.Id,
                    Probability = value.Probability,
                    WeenieClassId = value.WeenieClassId,
                    Delay = value.Delay,
                    InitCreate = value.InitCreate,
                    MaxCreate = value.MaxCreate,
                    WhenCreate = value.WhenCreate,
                    WhereCreate = value.WhereCreate,
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
                });
            }


            // Properties for creatures

            foreach (var value in weenie.WeeniePropertiesAttribute)
            {
                result.BiotaPropertiesAttribute.Add(new BiotaPropertiesAttribute
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    InitLevel = value.InitLevel,
                    LevelFromCP = value.LevelFromCP,
                    CPSpent = value.CPSpent,
                });
            }

            foreach (var value in weenie.WeeniePropertiesAttribute2nd)
            {
                result.BiotaPropertiesAttribute2nd.Add(new BiotaPropertiesAttribute2nd
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    InitLevel = value.InitLevel,
                    LevelFromCP = value.LevelFromCP,
                    CPSpent = value.CPSpent,
                    CurrentLevel = value.CurrentLevel,
                });
            }

            foreach (var value in weenie.WeeniePropertiesBodyPart)
            {
                result.BiotaPropertiesBodyPart.Add(new BiotaPropertiesBodyPart
                {
                    ObjectId = result.Id,
                    Key = value.Key,
                    DType = value.DType,
                    DVal = value.DVal,
                    DVar = value.DVar,
                    BaseArmor = value.BaseArmor,
                    ArmorVsSlash = value.ArmorVsSlash,
                    ArmorVsPierce = value.ArmorVsPierce,
                    ArmorVsBludgeon = value.ArmorVsBludgeon,
                    ArmorVsCold = value.ArmorVsCold,
                    ArmorVsFire = value.ArmorVsFire,
                    ArmorVsAcid = value.ArmorVsAcid,
                    ArmorVsElectric = value.ArmorVsElectric,
                    ArmorVsNether = value.ArmorVsNether,
                    BH = value.BH,
                    HLF = value.HLF,
                    MLF = value.MLF,
                    LLF = value.LLF,
                    HRF = value.HRF,
                    MRF = value.MRF,
                    LRF = value.LRF,
                    HLB = value.HLB,
                    MLB = value.MLB,
                    LLB = value.LLB,
                    HRB = value.HRB,
                    MRB = value.MRB,
                    LRB = value.LRB,
                });
            }

            foreach (var value in weenie.WeeniePropertiesSkill)
            {
                result.BiotaPropertiesSkill.Add(new BiotaPropertiesSkill
                {
                    ObjectId = result.Id,
                    Type = value.Type,
                    LevelFromPP = value.LevelFromPP,
                    SAC = value.SAC,
                    PP = value.PP,
                    InitLevel = value.InitLevel,
                    ResistanceAtLastCheck = value.ResistanceAtLastCheck,
                    LastUsedTime = value.LastUsedTime,
                });
            }


            // Properties for books

            if (weenie.WeeniePropertiesBook != null)
            {
                result.BiotaPropertiesBook = new BiotaPropertiesBook();
                result.BiotaPropertiesBook.ObjectId = result.Id;
                result.BiotaPropertiesBook.MaxNumPages = weenie.WeeniePropertiesBook.MaxNumPages;
                result.BiotaPropertiesBook.MaxNumCharsPerPage = weenie.WeeniePropertiesBook.MaxNumCharsPerPage;
            }

            foreach (var value in weenie.WeeniePropertiesBookPageData)
            {
                result.BiotaPropertiesBookPageData.Add(new BiotaPropertiesBookPageData
                {
                    ObjectId = result.Id,
                    PageId = value.PageId,
                    AuthorId = value.AuthorId,
                    AuthorName = value.AuthorName,
                    AuthorAccount = value.AuthorAccount,
                    IgnoreAuthor = value.IgnoreAuthor,
                    PageText = value.PageText,
                });
            }

            return result;
        }
    }
}
