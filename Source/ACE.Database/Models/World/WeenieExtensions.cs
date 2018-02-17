using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

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

        public static WeeniePropertiesAttribute GetAttribute(this Weenie weenie, Ability ability)
        {
            return weenie.WeeniePropertiesAttribute.FirstOrDefault(x => x.Type == (uint)ability);
        }

        public static WeeniePropertiesAttribute2nd GetAttribute2nd(this Weenie weenie, Ability ability)
        {
            return weenie.WeeniePropertiesAttribute2nd.FirstOrDefault(x => x.Type == (uint)ability);
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

        public static int? GetProperty(this Weenie weenie, PropertyInstanceId property)
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

            return new Position(result.Landblock ?? 0, result.OriginX, result.OriginY, result.OriginZ, result.AnglesX, result.AnglesY, result.AnglesZ, result.AnglesW);
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

        public static WeeniePropertiesTextureMap GetTextureMap(this Weenie weenie, byte index)
        {
            return weenie.WeeniePropertiesTextureMap.FirstOrDefault(x => x.Index == index);
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


            var actions = new List<BiotaPropertiesEmoteAction>();

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

                foreach (var subValue in value.WeeniePropertiesEmoteAction)
                {
                    var action = new BiotaPropertiesEmoteAction
                    {
                        ObjectId = biota.Id,
                        // EmoteId
                        Type = subValue.Type,
                        Delay = subValue.Delay,
                        Extent = subValue.Extent,
                        Motion = subValue.Motion,
                        Message = subValue.Message,
                        TestString = subValue.TestString,
                        Min = subValue.Min,
                        Max = subValue.Max,
                        Min64 = subValue.Min64,
                        Max64 = subValue.Max64,
                        MinDbl = subValue.MinDbl,
                        MaxDbl = subValue.MaxDbl,
                        Stat = subValue.Stat,
                        Display = subValue.Display,
                        Amount = subValue.Amount,
                        Amount64 = subValue.Amount64,
                        HeroXP64 = subValue.HeroXP64,
                        Percent = subValue.Percent,
                        SpellId = subValue.SpellId,
                        WealthRating = subValue.WealthRating,
                        TreasureClass = subValue.TreasureClass,
                        TreasureType = subValue.TreasureType,
                        PScript = subValue.PScript,
                        Sound = subValue.Sound,
                        DestinationType = subValue.DestinationType,
                        WeenieClassId = subValue.WeenieClassId,
                        StackSize = subValue.StackSize,
                        Palette = subValue.Palette,
                        Shade = subValue.Shade,
                        TryToBond = subValue.TryToBond,
                        ObjCellId = subValue.ObjCellId,
                        OriginX = subValue.OriginX,
                        OriginY = subValue.OriginY,
                        OriginZ = subValue.OriginZ,
                        AnglesW = subValue.AnglesW,
                        AnglesX = subValue.AnglesX,
                        AnglesY = subValue.AnglesY,
                        AnglesZ = subValue.AnglesZ,
                    };

                    emote.BiotaPropertiesEmoteAction.Add(action);

                    actions.Add(action);
                }

                biota.BiotaPropertiesEmote.Add(emote);
            }

            foreach (var action in actions)
                biota.BiotaPropertiesEmoteAction.Add(action);


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
                    Landblock = value.Landblock,
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
                    AdjustPP = value.AdjustPP,
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
    }
}
