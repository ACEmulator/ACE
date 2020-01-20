using System.Collections.Generic;
using System.Linq;

using ACE.Common.Extensions;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;

using Biota = ACE.Database.Models.Shard.Biota;

namespace ACE.Database.Models.World
{
    public static class WeenieExtensions
    {
        // LandblockInstances

        // PointsOfInterest

        public static uint? GetAnimationId(this Weenie weenie, byte index)
        {
            return weenie.WeeniePropertiesAnimPart.FirstOrDefault(x => x.Index == index)?.AnimationId;
        }

        public static WeeniePropertiesAttribute GetAttribute(this Weenie weenie, PropertyAttribute attribute)
        {
            return weenie.WeeniePropertiesAttribute.FirstOrDefault(x => x.Type == (uint)attribute);
        }

        public static WeeniePropertiesAttribute2nd GetAttribute2nd(this Weenie weenie, PropertyAttribute2nd attribute)
        {
            return weenie.WeeniePropertiesAttribute2nd.FirstOrDefault(x => x.Type == (uint)attribute);
        }

        public static WeeniePropertiesBodyPart GetBodyPart(this Weenie weenie, ushort key)
        {
            return weenie.WeeniePropertiesBodyPart.FirstOrDefault(x => x.Key == key);
        }

        public static WeeniePropertiesBookPageData GetBookPageData(this Weenie weenie, uint pageId)
        {
            return weenie.WeeniePropertiesBookPageData.FirstOrDefault(x => x.PageId == pageId);
        }

        public static bool? GetProperty(this Weenie weenie, PropertyBool property)
        {
            return weenie.WeeniePropertiesBool.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static WeeniePropertiesCreateList GetCreateList(this Weenie weenie, sbyte destinationType)
        {
            return weenie.WeeniePropertiesCreateList.FirstOrDefault(x => x.DestinationType == destinationType);
        }

        public static uint? GetProperty(this Weenie weenie, PropertyDataId property)
        {
            return weenie.WeeniePropertiesDID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static WeeniePropertiesEmote GetEmote(this Weenie weenie, uint category)
        {
            return weenie.WeeniePropertiesEmote.FirstOrDefault(x => x.Category == category);
        }

        // WeeniePropertiesEmoteAction

        public static WeeniePropertiesEventFilter GetEventFilter(this Weenie weenie, int eventId)
        {
            return weenie.WeeniePropertiesEventFilter.FirstOrDefault(x => x.Event == eventId);
        }

        public static double? GetProperty(this Weenie weenie, PropertyFloat property)
        {
            return weenie.WeeniePropertiesFloat.FirstOrDefault(x => x.Type == (ushort)property)?.Value;
        }

        // WeeniePropertiesGenerator

        public static uint? GetProperty(this Weenie weenie, PropertyInstanceId property)
        {
            return weenie.WeeniePropertiesIID.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static int? GetProperty(this Weenie weenie, PropertyInt property)
        {
            return weenie.WeeniePropertiesInt.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static long? GetProperty(this Weenie weenie, PropertyInt64 property)
        {
            return weenie.WeeniePropertiesInt64.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static WeeniePropertiesPalette GetPalette(this Weenie weenie, uint subPaletteId)
        {
            return weenie.WeeniePropertiesPalette.FirstOrDefault(x => x.SubPaletteId == subPaletteId);
        }

        public static Position GetPosition(this Weenie weenie, PositionType positionType)
        {
            var result = weenie.WeeniePropertiesPosition.FirstOrDefault(x => x.PositionType == (uint)positionType);

            if (result == null)
                return null;

            return new Position(result.ObjCellId, result.OriginX, result.OriginY, result.OriginZ, result.AnglesX, result.AnglesY, result.AnglesZ, result.AnglesW);
        }

        public static WeeniePropertiesSkill GetProperty(this Weenie weenie, Skill skill)
        {
            return weenie.WeeniePropertiesSkill.FirstOrDefault(x => x.Type == (uint) skill);
        }

        public static WeeniePropertiesSpellBook GetSpell(this Weenie weenie, int spell)
        {
            return weenie.WeeniePropertiesSpellBook.FirstOrDefault(x => x.Spell == spell);
        }

        public static string GetProperty(this Weenie weenie, PropertyString property)
        {
            return weenie.WeeniePropertiesString.FirstOrDefault(x => x.Type == (uint)property)?.Value;
        }

        public static WeeniePropertiesAttribute GetProperty(this Weenie weenie, PropertyAttribute property)
        {
            return weenie.WeeniePropertiesAttribute.FirstOrDefault(x => x.Type == (uint)property);
        }

        public static WeeniePropertiesAttribute2nd GetProperty(this Weenie weenie, PropertyAttribute2nd property)
        {
            return weenie.WeeniePropertiesAttribute2nd.FirstOrDefault(x => x.Type == (uint)property);
        }

        public static WeeniePropertiesTextureMap GetTextureMap(this Weenie weenie, byte index)
        {
            return weenie.WeeniePropertiesTextureMap.FirstOrDefault(x => x.Index == index);
        }

        public static string GetPluralName(this Weenie weenie)
        {
            var pluralName = weenie.GetProperty(PropertyString.PluralName);

            if (pluralName == null)
                pluralName = weenie.GetProperty(PropertyString.Name).Pluralize();

            return pluralName;
        }

        public static Biota CreateCopyAsBiota(this Weenie weenie, uint id)
        {
            var biota = new Biota();

            biota.Id = id;
            biota.WeenieClassId = weenie.ClassId;
            biota.WeenieType = weenie.Type;

            if (weenie.WeeniePropertiesBook != null)
            {
                biota.BiotaPropertiesBook = new BiotaPropertiesBook();
                biota.BiotaPropertiesBook.ObjectId = biota.Id;
                biota.BiotaPropertiesBook.MaxNumPages = weenie.WeeniePropertiesBook.MaxNumPages;
                biota.BiotaPropertiesBook.MaxNumCharsPerPage = weenie.WeeniePropertiesBook.MaxNumCharsPerPage;
            }

            foreach (var value in weenie.WeeniePropertiesAnimPart)
            {
                biota.BiotaPropertiesAnimPart.Add(new BiotaPropertiesAnimPart
                {
                    ObjectId = biota.Id,
                    Index = value.Index,
                    AnimationId = value.AnimationId
                });
            }

            foreach (var value in weenie.WeeniePropertiesAttribute)
            {
                biota.BiotaPropertiesAttribute.Add(new BiotaPropertiesAttribute
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    InitLevel = value.InitLevel,
                    LevelFromCP = value.LevelFromCP,
                    CPSpent = value.CPSpent,
                });
            }

            foreach (var value in weenie.WeeniePropertiesAttribute2nd)
            {
                biota.BiotaPropertiesAttribute2nd.Add(new BiotaPropertiesAttribute2nd
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    InitLevel = value.InitLevel,
                    LevelFromCP = value.LevelFromCP,
                    CPSpent = value.CPSpent,
                    CurrentLevel = value.CurrentLevel,
                });
            }

            foreach (var value in weenie.WeeniePropertiesBodyPart)
            {
                biota.BiotaPropertiesBodyPart.Add(new BiotaPropertiesBodyPart
                {
                    ObjectId = biota.Id,
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

            foreach (var value in weenie.WeeniePropertiesBookPageData)
            {
                biota.BiotaPropertiesBookPageData.Add(new BiotaPropertiesBookPageData
                {
                    ObjectId = biota.Id,
                    PageId = value.PageId,
                    AuthorId = value.AuthorId,
                    AuthorName = value.AuthorName,
                    AuthorAccount = value.AuthorAccount,
                    IgnoreAuthor = value.IgnoreAuthor,
                    PageText = value.PageText,
                });
            }

            foreach (var value in weenie.WeeniePropertiesBool)
            {
                biota.BiotaPropertiesBool.Add(new BiotaPropertiesBool
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in weenie.WeeniePropertiesCreateList)
            {
                biota.BiotaPropertiesCreateList.Add(new BiotaPropertiesCreateList
                {
                    ObjectId = biota.Id,
                    DestinationType = value.DestinationType,
                    WeenieClassId = value.WeenieClassId,
                    StackSize = value.StackSize,
                    Palette = value.Palette,
                    Shade = value.Shade,
                    TryToBond = value.TryToBond,
                });
            }

            foreach (var value in weenie.WeeniePropertiesDID)
            {
                biota.BiotaPropertiesDID.Add(new BiotaPropertiesDID
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }


            foreach (var value in weenie.WeeniePropertiesEmote)
            {
                var emote = new BiotaPropertiesEmote
                {
                    ObjectId = biota.Id,
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
                        // We just want to enter a value that forces EF to update the record with the correft foreign key. If this behavior changes in the future and we set it to 1,
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

                biota.BiotaPropertiesEmote.Add(emote);
            }


            foreach (var value in weenie.WeeniePropertiesEventFilter)
            {
                biota.BiotaPropertiesEventFilter.Add(new BiotaPropertiesEventFilter
                {
                    ObjectId = biota.Id,
                    Event = value.Event,
                });
            }

            foreach (var value in weenie.WeeniePropertiesFloat)
            {
                biota.BiotaPropertiesFloat.Add(new BiotaPropertiesFloat
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in weenie.WeeniePropertiesGenerator)
            {
                biota.BiotaPropertiesGenerator.Add(new BiotaPropertiesGenerator
                {
                    ObjectId = biota.Id,
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

            foreach (var value in weenie.WeeniePropertiesIID)
            {
                biota.BiotaPropertiesIID.Add(new BiotaPropertiesIID
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in weenie.WeeniePropertiesInt)
            {
                biota.BiotaPropertiesInt.Add(new BiotaPropertiesInt
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in weenie.WeeniePropertiesInt64)
            {
                biota.BiotaPropertiesInt64.Add(new BiotaPropertiesInt64
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in weenie.WeeniePropertiesPalette)
            {
                biota.BiotaPropertiesPalette.Add(new BiotaPropertiesPalette
                {
                    ObjectId = biota.Id,
                    SubPaletteId = value.SubPaletteId,
                    Offset = value.Offset,
                    Length = value.Length,
                });
            }

            foreach (var value in weenie.WeeniePropertiesPosition)
            {
                biota.BiotaPropertiesPosition.Add(new BiotaPropertiesPosition
                {
                    ObjectId = biota.Id,
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

            foreach (var value in weenie.WeeniePropertiesSkill)
            {
                biota.BiotaPropertiesSkill.Add(new BiotaPropertiesSkill
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    LevelFromPP = value.LevelFromPP,
                    SAC = value.SAC,
                    PP = value.PP,
                    InitLevel = value.InitLevel,
                    ResistanceAtLastCheck = value.ResistanceAtLastCheck,
                    LastUsedTime = value.LastUsedTime,
                });
            }

            foreach (var value in weenie.WeeniePropertiesSpellBook)
            {
                biota.BiotaPropertiesSpellBook.Add(new BiotaPropertiesSpellBook
                {
                    ObjectId = biota.Id,
                    Spell = value.Spell,
                    Probability = value.Probability,
                });
            }

            foreach (var value in weenie.WeeniePropertiesString)
            {
                biota.BiotaPropertiesString.Add(new BiotaPropertiesString
                {
                    ObjectId = biota.Id,
                    Type = value.Type,
                    Value = value.Value,
                });
            }

            foreach (var value in weenie.WeeniePropertiesTextureMap)
            {
                biota.BiotaPropertiesTextureMap.Add(new BiotaPropertiesTextureMap
                {
                    ObjectId = biota.Id,
                    Index = value.Index,
                    OldId = value.OldId,
                    NewId = value.NewId,
                });
            }

            return biota;
        }

        public static ACE.Entity.Models.Weenie CreateCopyAsEntityWeenie(this Weenie weenie)
        {
            var biota = new ACE.Entity.Models.Weenie();

            biota.WeenieClassId = weenie.ClassId;
            biota.ClassName = weenie.ClassName;
            biota.WeenieType = weenie.Type;

            if (weenie.WeeniePropertiesBool != null)
            {
                biota.PropertiesBool = new Dictionary<ushort, bool>();
                foreach (var value in weenie.WeeniePropertiesBool)
                    biota.PropertiesBool[value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesDID != null)
            { 
                biota.PropertiesDID = new Dictionary<ushort, uint>();
                foreach (var value in weenie.WeeniePropertiesDID)
                    biota.PropertiesDID[value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesFloat != null)
            { 
                biota.PropertiesFloat = new Dictionary<ushort, double>();
                foreach (var value in weenie.WeeniePropertiesFloat)
                    biota.PropertiesFloat[value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesIID != null)
            { 
                biota.PropertiesIID = new Dictionary<ushort, uint>();
                foreach (var value in weenie.WeeniePropertiesIID)
                    biota.PropertiesIID[value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesInt != null)
            { 
                biota.PropertiesInt = new Dictionary<ushort, int>();
                foreach (var value in weenie.WeeniePropertiesInt)
                    biota.PropertiesInt[value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesInt64 != null)
            { 
                biota.PropertiesInt64 = new Dictionary<ushort, long>();
                foreach (var value in weenie.WeeniePropertiesInt64)
                    biota.PropertiesInt64[value.Type] = value.Value;
            }
            if (weenie.WeeniePropertiesString != null)
            { 
                biota.PropertiesString = new Dictionary<ushort, string>();
                foreach (var value in weenie.WeeniePropertiesString)
                    biota.PropertiesString[value.Type] = value.Value;
            }


            if (weenie.WeeniePropertiesPosition != null)
            {
                biota.PropertiesPosition = new Dictionary<ushort, PropertiesPosition>();

                foreach (var record in weenie.WeeniePropertiesPosition)
                {
                    var newEntity = new PropertiesPosition
                    {
                        ObjCellId = record.ObjCellId,
                        OriginX = record.OriginX,
                        OriginY = record.OriginY,
                        OriginZ = record.OriginZ,
                        AnglesW = record.AnglesW,
                        AnglesX = record.AnglesX,
                        AnglesY = record.AnglesY,
                        AnglesZ = record.AnglesZ,

                    };

                    biota.PropertiesPosition[record.PositionType] = newEntity;
                }
            }


            if (weenie.WeeniePropertiesSpellBook != null)
            {
                biota.PropertiesSpellBook = new Dictionary<int, float>();
                foreach (var value in weenie.WeeniePropertiesSpellBook)
                    biota.PropertiesSpellBook[value.Spell] = value.Probability;
            }


            if (weenie.WeeniePropertiesAnimPart != null)
            {
                biota.PropertiesAnimPart = new Dictionary<byte, uint>();
                foreach (var value in weenie.WeeniePropertiesAnimPart)
                    biota.PropertiesAnimPart[value.Index] = value.AnimationId;
            }

            if (weenie.WeeniePropertiesPalette != null)
            {
                biota.PropertiesPalette = new List<PropertiesPalette>();

                foreach (var record in weenie.WeeniePropertiesPalette)
                {
                    var newEntity = new PropertiesPalette
                    {
                        SubPaletteId = record.SubPaletteId,
                        Offset = record.Offset,
                        Length = record.Length,
                    };

                    biota.PropertiesPalette.Add(newEntity);
                }
            }

            if (weenie.WeeniePropertiesTextureMap != null)
            {
                biota.PropertiesTextureMap = new List<PropertiesTextureMap>();

                foreach (var record in weenie.WeeniePropertiesTextureMap)
                {
                    var newEntity = new PropertiesTextureMap
                    {
                        Index = record.Index,
                        OldId = record.OldId,
                        NewId = record.NewId,
                    };

                    biota.PropertiesTextureMap.Add(newEntity);
                }
            }


            // Properties for all world objects that typically aren't modified over the original weenie

            if (weenie.WeeniePropertiesCreateList != null)
            {
                biota.PropertiesCreateList = new List<PropertiesCreateList>();

                foreach (var record in weenie.WeeniePropertiesCreateList)
                {
                    var newEntity = new PropertiesCreateList
                    {
                        DestinationType = record.DestinationType,
                        WeenieClassId = record.WeenieClassId,
                        StackSize = record.StackSize,
                        Palette = record.Palette,
                        Shade = record.Shade,
                        TryToBond = record.TryToBond,
                    };

                    biota.PropertiesCreateList.Add(newEntity);
                }
            }

            if (weenie.WeeniePropertiesEmote != null)
            {
                biota.PropertiesEmote = new List<PropertiesEmote>();

                foreach (var record in weenie.WeeniePropertiesEmote)
                {
                    var newEntity = new PropertiesEmote
                    {
                        Category = record.Category,
                        Probability = record.Probability,
                        WeenieClassId = record.WeenieClassId,
                        Style = record.Style,
                        Substyle = record.Substyle,
                        Quest = record.Quest,
                        VendorType = record.VendorType,
                        MinHealth = record.MinHealth,
                        MaxHealth = record.MaxHealth,
                    };

                    foreach (var record2 in record.WeeniePropertiesEmoteAction)
                    {
                        var newEntity2 = new PropertiesEmoteAction
                        {
                            Order = record2.Order,
                            Type = record2.Type,
                            Delay = record2.Delay,
                            Extent = record2.Extent,
                            Motion = record2.Motion,
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
                            PScript = record2.PScript,
                            Sound = record2.Sound,
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

                    biota.PropertiesEmote.Add(newEntity);
                }
            }

            if (weenie.WeeniePropertiesEventFilter != null)
            {
                biota.PropertiesEventFilter = new List<int>();
                foreach (var value in weenie.WeeniePropertiesEventFilter)
                    biota.PropertiesEventFilter.Add(value.Event);
            }

            if (weenie.WeeniePropertiesGenerator != null)
            {
                biota.PropertiesGenerator = new List<PropertiesGenerator>();

                foreach (var record in weenie.WeeniePropertiesGenerator)
                {
                    var newEntity = new PropertiesGenerator
                    {
                        Probability = record.Probability,
                        WeenieClassId = record.WeenieClassId,
                        Delay = record.Delay,
                        InitCreate = record.InitCreate,
                        MaxCreate = record.MaxCreate,
                        WhenCreate = record.WhenCreate,
                        WhereCreate = record.WhereCreate,
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

                    biota.PropertiesGenerator.Add(newEntity);
                }
            }


            // Properties for creatures

            if (weenie.WeeniePropertiesAttribute != null)
            {
                biota.PropertiesAttribute = new Dictionary<ushort, PropertiesAttribute>();

                foreach (var record in weenie.WeeniePropertiesAttribute)
                {
                    var newEntity = new PropertiesAttribute
                    {
                        InitLevel = record.InitLevel,
                        LevelFromCP = record.LevelFromCP,
                        CPSpent = record.CPSpent,
                    };

                    biota.PropertiesAttribute[record.Type] = newEntity;
                }
            }

            if (weenie.WeeniePropertiesAttribute2nd != null)
            {
                biota.PropertiesAttribute2nd = new Dictionary<ushort, PropertiesAttribute2nd>();

                foreach (var record in weenie.WeeniePropertiesAttribute2nd)
                {
                    var newEntity = new PropertiesAttribute2nd
                    {
                        InitLevel = record.InitLevel,
                        LevelFromCP = record.LevelFromCP,
                        CPSpent = record.CPSpent,
                        CurrentLevel = record.CurrentLevel,
                    };

                    biota.PropertiesAttribute2nd[record.Type] = newEntity;
                }
            }

            if (weenie.WeeniePropertiesBodyPart != null)
            {
                biota.PropertiesBodyPart = new Dictionary<ushort, PropertiesBodyPart>();

                foreach (var record in weenie.WeeniePropertiesBodyPart)
                {
                    var newEntity = new PropertiesBodyPart
                    {
                        DType = record.DType,
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

                    biota.PropertiesBodyPart[record.Key] = newEntity;
                }
            }

            if (weenie.WeeniePropertiesSkill != null)
            {
                biota.PropertiesSkill = new Dictionary<ushort, PropertiesSkill>();

                foreach (var record in weenie.WeeniePropertiesSkill)
                {
                    var newEntity = new PropertiesSkill
                    {
                        LevelFromPP = record.LevelFromPP,
                        SAC = record.SAC,
                        PP = record.PP,
                        InitLevel = record.InitLevel,
                        ResistanceAtLastCheck = record.ResistanceAtLastCheck,
                        LastUsedTime = record.LastUsedTime,
                    };

                    biota.PropertiesSkill[record.Type] = newEntity;
                }
            }


            // Properties for books

            if (weenie.WeeniePropertiesBook != null)
            {
                biota.PropertiesBook = new PropertiesBook
                {
                    MaxNumPages = weenie.WeeniePropertiesBook.MaxNumPages,
                    MaxNumCharsPerPage = weenie.WeeniePropertiesBook.MaxNumCharsPerPage,
                };
            }

            if (weenie.WeeniePropertiesBookPageData != null)
            {
                biota.PropertiesBookPageData = new List<PropertiesBookPageData>();

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

                    biota.PropertiesBookPageData.Add(newEntity);
                }
            }

            return biota;
        }

        public static bool IsStackable(this Weenie weenie)
        {
            var weenieType = (WeenieType)weenie.Type;

            return weenieType == WeenieType.Stackable      || weenieType == WeenieType.Food || weenieType == WeenieType.Coin       || weenieType == WeenieType.CraftTool
                || weenieType == WeenieType.SpellComponent || weenieType == WeenieType.Gem  || weenieType == WeenieType.Ammunition || weenieType == WeenieType.Missile;
        }

        public static bool RequiresBackpackSlotOrIsContainer(this Weenie weenie)
        {
            var requiresBackPackSlot = weenie.GetProperty(PropertyBool.RequiresBackpackSlot) ?? false;

            return requiresBackPackSlot || weenie.Type == (int)WeenieType.Container;
        }

        public static bool IsVendorService(this Weenie weenie)
        {
            var vendorService = weenie.GetProperty(PropertyBool.VendorService) ?? false;

            return vendorService;
        }
    }
}
