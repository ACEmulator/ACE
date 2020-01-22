using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Models;

namespace ACE.Database.Adapter
{
    public static class BiotaConverter
    {
        public static ACE.Entity.Models.Biota ConvertToEntityBiota(ACE.Database.Models.Shard.Biota biota)
        {
            var result = new ACE.Entity.Models.Biota();

            result.Id = biota.Id;
            result.WeenieClassId = biota.WeenieClassId;
            result.WeenieType = biota.WeenieType;

            if (biota.BiotaPropertiesBool != null)
            {
                result.PropertiesBool = new Dictionary<ushort, bool>();
                foreach (var value in biota.BiotaPropertiesBool)
                    result.PropertiesBool[value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesDID != null)
            {
                result.PropertiesDID = new Dictionary<ushort, uint>();
                foreach (var value in biota.BiotaPropertiesDID)
                    result.PropertiesDID[value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesFloat != null)
            {
                result.PropertiesFloat = new Dictionary<ushort, double>();
                foreach (var value in biota.BiotaPropertiesFloat)
                    result.PropertiesFloat[value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesIID != null)
            {
                result.PropertiesIID = new Dictionary<ushort, uint>();
                foreach (var value in biota.BiotaPropertiesIID)
                    result.PropertiesIID[value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesInt != null)
            {
                result.PropertiesInt = new Dictionary<ushort, int>();
                foreach (var value in biota.BiotaPropertiesInt)
                    result.PropertiesInt[value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesInt64 != null)
            {
                result.PropertiesInt64 = new Dictionary<ushort, long>();
                foreach (var value in biota.BiotaPropertiesInt64)
                    result.PropertiesInt64[value.Type] = value.Value;
            }
            if (biota.BiotaPropertiesString != null)
            {
                result.PropertiesString = new Dictionary<ushort, string>();
                foreach (var value in biota.BiotaPropertiesString)
                    result.PropertiesString[value.Type] = value.Value;
            }


            if (biota.BiotaPropertiesPosition != null)
            {
                result.PropertiesPosition = new Dictionary<ushort, PropertiesPosition>();

                foreach (var record in biota.BiotaPropertiesPosition)
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

                    result.PropertiesPosition[record.PositionType] = newEntity;
                }
            }


            if (biota.BiotaPropertiesSpellBook != null)
            {
                result.PropertiesSpellBook = new Dictionary<int, float>();
                foreach (var value in biota.BiotaPropertiesSpellBook)
                    result.PropertiesSpellBook[value.Spell] = value.Probability;
            }


            if (biota.BiotaPropertiesAnimPart != null)
            {
                result.PropertiesAnimPart = new List<PropertiesAnimPart>();

                foreach (var record in biota.BiotaPropertiesAnimPart)
                {
                    var newEntity = new PropertiesAnimPart
                    {
                        Index = record.Index,
                        AnimationId = record.AnimationId,
                    };

                    result.PropertiesAnimPart.Add(newEntity);
                }
            }

            if (biota.BiotaPropertiesPalette != null)
            {
                result.PropertiesPalette = new List<PropertiesPalette>();

                foreach (var record in biota.BiotaPropertiesPalette)
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

            if (biota.BiotaPropertiesTextureMap != null)
            {
                result.PropertiesTextureMap = new List<PropertiesTextureMap>();

                foreach (var record in biota.BiotaPropertiesTextureMap)
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

            if (biota.BiotaPropertiesCreateList != null)
            {
                result.PropertiesCreateList = new List<PropertiesCreateList>();

                foreach (var record in biota.BiotaPropertiesCreateList)
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

                    result.PropertiesCreateList.Add(newEntity);
                }
            }

            if (biota.BiotaPropertiesEmote != null)
            {
                result.PropertiesEmote = new List<PropertiesEmote>();

                foreach (var record in biota.BiotaPropertiesEmote)
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

                    foreach (var record2 in record.BiotaPropertiesEmoteAction)
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

                    result.PropertiesEmote.Add(newEntity);
                }
            }

            if (biota.BiotaPropertiesEventFilter != null)
            {
                result.PropertiesEventFilter = new List<int>();
                foreach (var value in biota.BiotaPropertiesEventFilter)
                    result.PropertiesEventFilter.Add(value.Event);
            }

            if (biota.BiotaPropertiesGenerator != null)
            {
                result.PropertiesGenerator = new List<PropertiesGenerator>();

                foreach (var record in biota.BiotaPropertiesGenerator)
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

                    result.PropertiesGenerator.Add(newEntity);
                }
            }


            // Properties for creatures

            if (biota.BiotaPropertiesAttribute != null)
            {
                result.PropertiesAttribute = new Dictionary<ushort, PropertiesAttribute>();

                foreach (var record in biota.BiotaPropertiesAttribute)
                {
                    var newEntity = new PropertiesAttribute
                    {
                        InitLevel = record.InitLevel,
                        LevelFromCP = record.LevelFromCP,
                        CPSpent = record.CPSpent,
                    };

                    result.PropertiesAttribute[record.Type] = newEntity;
                }
            }

            if (biota.BiotaPropertiesAttribute2nd != null)
            {
                result.PropertiesAttribute2nd = new Dictionary<ushort, PropertiesAttribute2nd>();

                foreach (var record in biota.BiotaPropertiesAttribute2nd)
                {
                    var newEntity = new PropertiesAttribute2nd
                    {
                        InitLevel = record.InitLevel,
                        LevelFromCP = record.LevelFromCP,
                        CPSpent = record.CPSpent,
                        CurrentLevel = record.CurrentLevel,
                    };

                    result.PropertiesAttribute2nd[record.Type] = newEntity;
                }
            }

            if (biota.BiotaPropertiesBodyPart != null)
            {
                result.PropertiesBodyPart = new Dictionary<ushort, PropertiesBodyPart>();

                foreach (var record in biota.BiotaPropertiesBodyPart)
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

                    result.PropertiesBodyPart[record.Key] = newEntity;
                }
            }

            if (biota.BiotaPropertiesSkill != null)
            {
                result.PropertiesSkill = new Dictionary<ushort, PropertiesSkill>();

                foreach (var record in biota.BiotaPropertiesSkill)
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

                    result.PropertiesSkill[record.Type] = newEntity;
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

            if (biota.BiotaPropertiesBookPageData != null)
            {
                result.PropertiesBookPageData = new List<PropertiesBookPageData>();

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
            // TODO


            return result;
        }

        public static ACE.Database.Models.Shard.Biota ConvertFromEntityBiota(ACE.Entity.Models.Biota biota)
        {
            throw new NotImplementedException();
        }
    }
}
