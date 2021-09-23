using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using ACE.Adapter.Enum;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Adapter.Lifestoned
{
    public static class LifestonedConverter
    {
        /// <summary>
        /// Converts LSD weenie to ACE weenie
        /// </summary>
        public static bool TryConvert(global::Lifestoned.DataModel.Gdle.Weenie input, out Weenie result, bool correctForEnumShift = false)
        {
            if (input.WeenieId == 0)
            {
                result = null;
                return false;
            }

            try
            {
                result = new Weenie();

                result.ClassId = input.WeenieId;

                if (input.WeenieClassId <= ushort.MaxValue && System.Enum.IsDefined(typeof(WeenieClassId), (ushort)input.WeenieClassId))
                    result.ClassName = System.Enum.GetName(typeof(WeenieClassId), (ushort)input.WeenieClassId).ToLower();
                else if (input.WeenieClassId <= ushort.MaxValue && System.Enum.IsDefined(typeof(WeenieClassName), (ushort)input.WeenieClassId))
                {
                    var clsName = System.Enum.GetName(typeof(WeenieClassName), input.WeenieClassId).ToLower().Substring(2);
                    result.ClassName = clsName.Substring(0, clsName.Length - 6);
                }
                else
                    result.ClassName = "ace" + input.WeenieClassId + "-" + input.Name.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(":", "").Replace("_", "").Replace("-", "").Replace(",", "").Replace("\"", "").ToLower();

                result.Type = input.WeenieTypeId;

                if (input.Book != null)
                {
                    result.WeeniePropertiesBook = new WeeniePropertiesBook();
                    result.WeeniePropertiesBook.MaxNumPages = input.Book.MaxNumberPages; 
                    result.WeeniePropertiesBook.MaxNumCharsPerPage = input.Book.MaxCharactersPerPage;

                    if (input.Book.Pages != null)
                    {
                        result.WeeniePropertiesBookPageData = new List<WeeniePropertiesBookPageData>();

                        uint pageId = 0;

                        foreach (var value in input.Book.Pages)
                        {
                            result.WeeniePropertiesBookPageData.Add(new WeeniePropertiesBookPageData
                            {
                                PageId = pageId,

                                AuthorId = value.AuthorId ?? 0,
                                AuthorName = value.AuthorName,
                                AuthorAccount = value.AuthorAccount,
                                IgnoreAuthor = value.IgnoreAuthor ?? false,
                                PageText = value.PageText,
                            });

                            pageId++;
                        }
                    }
                }

                // LandblockInstance

                // PointsOfInterest

                // WeeniePropertiesAnimPart

                if (input.Attributes != null)
                {
                    if (input.Attributes.Strength != null)
                        result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Strength, InitLevel = input.Attributes.Strength.Ranks ?? 0, LevelFromCP = input.Attributes.Strength.LevelFromCp, CPSpent = input.Attributes.Strength.XpSpent ?? 0 });
                    if (input.Attributes.Endurance != null)
                        result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Endurance, InitLevel = input.Attributes.Endurance.Ranks ?? 0, LevelFromCP = input.Attributes.Endurance.LevelFromCp, CPSpent = input.Attributes.Endurance.XpSpent ?? 0 });
                    if (input.Attributes.Quickness != null)
                        result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Quickness, InitLevel = input.Attributes.Quickness.Ranks ?? 0, LevelFromCP = input.Attributes.Quickness.LevelFromCp, CPSpent = input.Attributes.Quickness.XpSpent ?? 0 });
                    if (input.Attributes.Coordination != null)
                        result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Coordination, InitLevel = input.Attributes.Coordination.Ranks ?? 0, LevelFromCP = input.Attributes.Coordination.LevelFromCp, CPSpent = input.Attributes.Coordination.XpSpent ?? 0 });
                    if (input.Attributes.Focus != null)
                        result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Focus, InitLevel = input.Attributes.Focus.Ranks ?? 0, LevelFromCP = input.Attributes.Focus.LevelFromCp, CPSpent = input.Attributes.Focus.XpSpent ?? 0 });
                    if (input.Attributes.Self != null)
                        result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Self, InitLevel = input.Attributes.Self.Ranks ?? 0, LevelFromCP = input.Attributes.Self.LevelFromCp, CPSpent = input.Attributes.Self.XpSpent ?? 0 });

                    if (input.Attributes.Health != null)
                        result.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxHealth, InitLevel = input.Attributes.Health.Ranks ?? 0, LevelFromCP = input.Attributes.Health.LevelFromCp ?? 0, CPSpent = input.Attributes.Health.XpSpent ?? 0, CurrentLevel = input.Attributes.Health.Current ?? 0 });
                    if (input.Attributes.Stamina != null)
                        result.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxStamina, InitLevel = input.Attributes.Stamina.Ranks ?? 0, LevelFromCP = input.Attributes.Stamina.LevelFromCp ?? 0, CPSpent = input.Attributes.Stamina.XpSpent ?? 0, CurrentLevel = input.Attributes.Stamina.Current ?? 0 });
                    if (input.Attributes.Mana != null)
                        result.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxMana, InitLevel = input.Attributes.Mana.Ranks ?? 0, LevelFromCP = input.Attributes.Mana.LevelFromCp ?? 0, CPSpent = input.Attributes.Mana.XpSpent ?? 0, CurrentLevel = input.Attributes.Mana.Current ?? 0 });
                }

                if (input.Body != null)
                {
                    foreach (var value in input.Body.BodyParts)
                    {
                        result.WeeniePropertiesBodyPart.Add(new WeeniePropertiesBodyPart
                        {
                            Key = (ushort)value.Key,

                            DType = value.BodyPart.DType,
                            DVal = value.BodyPart.DVal,
                            DVar = value.BodyPart.DVar,

                            BaseArmor = value.BodyPart.ArmorValues.BaseArmor,
                            ArmorVsSlash = value.BodyPart.ArmorValues.ArmorVsSlash,
                            ArmorVsPierce = value.BodyPart.ArmorValues.ArmorVsPierce,
                            ArmorVsBludgeon = value.BodyPart.ArmorValues.ArmorVsBludgeon,
                            ArmorVsCold = value.BodyPart.ArmorValues.ArmorVsCold,
                            ArmorVsFire = value.BodyPart.ArmorValues.ArmorVsFire,
                            ArmorVsAcid = value.BodyPart.ArmorValues.ArmorVsAcid,
                            ArmorVsElectric = value.BodyPart.ArmorValues.ArmorVsElectric,
                            ArmorVsNether = value.BodyPart.ArmorValues.ArmorVsNether,

                            BH = value.BodyPart.BH,

                            HLF = (float?)value.BodyPart.SD.HLF ?? 0,
                            MLF = (float?)value.BodyPart.SD.MLF ?? 0,
                            LLF = (float?)value.BodyPart.SD.LLF ?? 0,

                            HRF = (float?)value.BodyPart.SD.HRF ?? 0,
                            MRF = (float?)value.BodyPart.SD.MRF ?? 0,
                            LRF = (float?)value.BodyPart.SD.LRF ?? 0,

                            HLB = (float?)value.BodyPart.SD.HLB ?? 0,
                            MLB = (float?)value.BodyPart.SD.MLB ?? 0,
                            LLB = (float?)value.BodyPart.SD.LLB ?? 0,

                            HRB = (float?)value.BodyPart.SD.HRB ?? 0,
                            MRB = (float?)value.BodyPart.SD.MRB ?? 0,
                            LRB = (float?)value.BodyPart.SD.LRB ?? 0,
                        });
                    }
                }

                if (input.BoolStats != null)
                {
                    foreach (var value in input.BoolStats)
                    {
                        if (result.WeeniePropertiesBool.FirstOrDefault(x => x.Type == (ushort)value.Key) == null)
                            result.WeeniePropertiesBool.Add(new WeeniePropertiesBool { Type = (ushort)value.Key, Value = (value.Value != 0) });
                    }
                }

                if (input.CreateList != null)
                {
                    foreach (var value in input.CreateList)
                        result.WeeniePropertiesCreateList.Add(new WeeniePropertiesCreateList { WeenieClassId = value.WeenieClassId ?? 0, Palette = (sbyte?)value.Palette ?? 0, Shade = (float?)value.Shade ?? 0, DestinationType = (sbyte?)value.Destination ?? 0, StackSize = value.StackSize ?? 0, TryToBond = (value.TryToBond != 0) });
                }

                if (input.DidStats != null)
                {
                    foreach (var value in input.DidStats)
                    {
                        if (!correctForEnumShift)
                            result.WeeniePropertiesDID.Add(new WeeniePropertiesDID { Type = (ushort)value.Key, Value = value.Value });
                        else
                        {
                            var valCorrected = value.Value;

                            // Fix PhysicsScript ENUM shift post 16PY data
                            if (value.Key == (int)PropertyDataId.PhysicsScript)
                            {
                                // These are the only ones in 16PY database, not entirely certain where the shift started but the change below is correct for end of retail enum
                                if (valCorrected >= 83 && valCorrected <= 89)
                                    valCorrected++;
                            }

                            // Fix PhysicsScript ENUM shift post 16PY data
                            if (value.Key == (int)PropertyDataId.RestrictionEffect)
                            {
                                if (valCorrected >= 83)
                                    valCorrected++;
                            }

                            if (result.WeeniePropertiesDID.FirstOrDefault(x => x.Type == (ushort)value.Key) == null)
                                result.WeeniePropertiesDID.Add(new WeeniePropertiesDID { Type = (ushort)value.Key, Value = valCorrected });
                        }
                    }
                }

                if (input.EmoteTable != null)
                {
                    foreach (var kvp in input.EmoteTable)
                    {
                        // kvp.key not used

                        foreach (var value in kvp.Emotes)
                        {
                            var efEmote = new WeeniePropertiesEmote
                            {
                                Category = value.Category,

                                Probability = value.Probability ?? 0,

                                WeenieClassId = value.ClassId,

                                Style = value.Style,
                                Substyle = value.SubStyle,

                                Quest = value.Quest,

                                VendorType = (int?)value.VendorType,

                                MinHealth = value.MinHealth,
                                MaxHealth = value.MaxHealth
                            };

                            // Fix MotionCommand ENUM shift post 16PY data
                            if (correctForEnumShift && efEmote.Style.HasValue)
                            {
                                var oldStyle = (ACE.Entity.Enum.MotionCommand)efEmote.Style;
                                var index = efEmote.Style.Value & 0xFFFF;
                                if (index >= 0x115)
                                {
                                    var newStyle = (ACE.Entity.Enum.MotionCommand)efEmote.Style + 3;
                                    efEmote.Style += 3;
                                }
                            }
                            if (correctForEnumShift && efEmote.Substyle.HasValue)
                            {
                                var oldSubstyle = (ACE.Entity.Enum.MotionCommand)efEmote.Substyle;
                                var index = efEmote.Substyle.Value & 0xFFFF;
                                if (index >= 0x115)
                                {
                                    var newSubstyle = (ACE.Entity.Enum.MotionCommand)efEmote.Substyle + 3;
                                    efEmote.Substyle += 3;
                                }
                            }

                            uint order = 0;

                            foreach (var action in value.Actions)
                            {
                                var efAction = new WeeniePropertiesEmoteAction
                                {
                                    Order = order, // This is an ACE specific value to maintain the correct order of EmoteActions

                                    Type = action.EmoteActionType,
                                    Delay = action.Delay ?? 0,
                                    Extent = action.Extent ?? 0,

                                    Motion = (int?)action.Motion,

                                    Message = action.Message,
                                    TestString = action.TestString,

                                    Min = (int?)action.Min,
                                    Max = (int?)action.Max,
                                    Min64 = action.Minimum64,
                                    Max64 = action.Maximum64,
                                    MinDbl = action.FMin,
                                    MaxDbl = action.FMax,

                                    Stat = (int?)action.Stat,
                                    Display = action.Display,

                                    Amount = (int?)action.Amount,
                                    Amount64 = (long?)action.Amount64,
                                    HeroXP64 = (long?)action.HeroXp64,

                                    Percent = action.Percent,

                                    SpellId = (int?)action.SpellId,

                                    WealthRating = (int?)action.WealthRating,
                                    TreasureClass = (int?)action.TreasureClass,
                                    TreasureType = action.TreasureType,

                                    PScript = (int?)action.PScript,

                                    Sound = (int?)action.Sound
                                };

                                // Fix MotionCommand ENUM shift post 16PY data
                                if (correctForEnumShift && efAction.Motion.HasValue)
                                {
                                    var oldMotion = (MotionCommand)efAction.Motion;
                                    var index = efAction.Motion.Value & 0xFFFF;
                                    if (index >= 0x115)
                                    {
                                        var newMotion = (MotionCommand)efAction.Motion + 3;
                                        efAction.Motion += 3;
                                    }
                                }

                                // Fix PhysicsScript ENUM shift post 16PY data
                                if (correctForEnumShift && efAction.PScript.HasValue)
                                {
                                    if (efAction.PScript.Value >= 83)
                                        efAction.PScript++;
                                }

                                order++;

                                if (action.Item != null)
                                {
                                    efAction.WeenieClassId = action.Item.WeenieClassId;
                                    efAction.Palette = (int?)action.Item.Palette;
                                    efAction.Shade = (float?)action.Item.Shade;
                                    efAction.DestinationType = (sbyte?)action.Item.Destination;
                                    efAction.StackSize = action.Item.StackSize;
                                    efAction.TryToBond = action.Item.TryToBond.HasValue ? (action.Item.TryToBond != 0) : (bool?)null;
                                }

                                if (action.Frame != null)
                                {
                                    efAction.OriginX = action.Frame.Position.X;
                                    efAction.OriginY = action.Frame.Position.Y;
                                    efAction.OriginZ = action.Frame.Position.Z;

                                    efAction.AnglesW = action.Frame.Rotations.W;
                                    efAction.AnglesX = action.Frame.Rotations.X;
                                    efAction.AnglesY = action.Frame.Rotations.Y;
                                    efAction.AnglesZ = action.Frame.Rotations.Z;
                                }

                                if (action.MPosition != null)
                                {
                                    efAction.ObjCellId = action.MPosition.LandCellId;

                                    efAction.OriginX = action.MPosition.Frame.Position.X;
                                    efAction.OriginY = action.MPosition.Frame.Position.Y;
                                    efAction.OriginZ = action.MPosition.Frame.Position.Z;

                                    efAction.AnglesW = action.MPosition.Frame.Rotations.W;
                                    efAction.AnglesX = action.MPosition.Frame.Rotations.X;
                                    efAction.AnglesY = action.MPosition.Frame.Rotations.Y;
                                    efAction.AnglesZ = action.MPosition.Frame.Rotations.Z;
                                }

                                efEmote.WeeniePropertiesEmoteAction.Add(efAction);
                            }

                            result.WeeniePropertiesEmote.Add(efEmote);
                        }
                    }
                }

                // WeeniePropertiesEventFilter

                if (input.FloatStats != null)
                {
                    foreach (var value in input.FloatStats)
                    {
                        if (result.WeeniePropertiesFloat.FirstOrDefault(x => x.Type == (ushort)value.Key) == null)
                            result.WeeniePropertiesFloat.Add(new WeeniePropertiesFloat { Type = (ushort)value.Key, Value = value.Value });
                    }
                }

                if (input.GeneratorTable != null)
                {
                    foreach (var value in input.GeneratorTable)
                    {
                        result.WeeniePropertiesGenerator.Add(new WeeniePropertiesGenerator
                        {
                            Probability = (float)value.Probability,
                            WeenieClassId = value.WeenieClassId,
                            Delay = value.Delay,

                            InitCreate = (int)value.InitCreate,
                            MaxCreate = (int)value.MaxNumber,

                            WhenCreate = value.WhenCreate,
                            WhereCreate = value.WhereCreate,

                            StackSize = value.StackSize,

                            PaletteId = value.PaletteId,
                            Shade = value.Shade,

                            ObjCellId = value.ObjectCell,
                            OriginX = value.Frame.Position.X,
                            OriginY = value.Frame.Position.Y,
                            OriginZ = value.Frame.Position.Z,
                            AnglesW = value.Frame.Rotations.W,
                            AnglesX = value.Frame.Rotations.X,
                            AnglesY = value.Frame.Rotations.Y,
                            AnglesZ = value.Frame.Rotations.Z,

                            // Slot
                        });
                    }
                }

                if (input.IidStats != null)
                {
                    foreach (var value in input.IidStats)
                    {
                        if (result.WeeniePropertiesIID.FirstOrDefault(x => x.Type == (ushort)value.Key) == null)
                            result.WeeniePropertiesIID.Add(new WeeniePropertiesIID { Type = (ushort)value.Key, Value = (uint)value.Value });
                    }
                }

                if (input.IntStats != null)
                {
                    foreach (var value in input.IntStats)
                    {
                        if (result.WeeniePropertiesInt.FirstOrDefault(x => x.Type == (ushort)value.Key) == null)
                            result.WeeniePropertiesInt.Add(new WeeniePropertiesInt { Type = (ushort)value.Key, Value = value.Value });
                    }
                }

                if (input.Int64Stats != null)
                {
                    foreach (var value in input.Int64Stats)
                    {
                        if (result.WeeniePropertiesInt64.FirstOrDefault(x => x.Type == (ushort)value.Key) == null)
                            result.WeeniePropertiesInt64.Add(new WeeniePropertiesInt64 { Type = (ushort)value.Key, Value = value.Value });
                    }
                }

                // WeeniePropertiesPalette

                if (input.Positions != null)
                {
                    foreach (var value in input.Positions)
                    {
                        if (result.WeeniePropertiesPosition.FirstOrDefault(x => x.PositionType == (ushort)value.PositionType) == null)
                            result.WeeniePropertiesPosition.Add(new WeeniePropertiesPosition()
                            {
                                PositionType = (ushort)value.PositionType,

                                ObjCellId = value.Position.LandCellId,
                                OriginX = value.Position.Frame.Position.X,
                                OriginY = value.Position.Frame.Position.Y,
                                OriginZ = value.Position.Frame.Position.Z,
                                AnglesW = value.Position.Frame.Rotations.W,
                                AnglesX = value.Position.Frame.Rotations.X,
                                AnglesY = value.Position.Frame.Rotations.Y,
                                AnglesZ = value.Position.Frame.Rotations.Z,
                            });
                    }
                }

                if (input.Skills != null)
                {
                    foreach (var value in input.Skills)
                    {
                        if (result.WeeniePropertiesSkill.FirstOrDefault(x => x.Type == (ushort)value.SkillId) == null)
                            result.WeeniePropertiesSkill.Add(new WeeniePropertiesSkill { Type = (ushort)value.SkillId, LevelFromPP = (ushort)value.Skill.LevelFromPp, SAC = (uint?)value.Skill.TrainedLevel ?? 0, PP = value.Skill.XpInvested ?? 0, InitLevel = value.Skill.Ranks ?? 0, ResistanceAtLastCheck = value.Skill.ResistanceOfLastCheck ?? 0, LastUsedTime = value.Skill.LastUsed ?? 0 });
                    }
                }

                if (input.Spells != null)
                {
                    foreach (var value in input.Spells)
                    {
                        if (result.WeeniePropertiesDID.FirstOrDefault(x => x.Type == (int)PropertyDataId.Spell && x.Value == value.SpellId) == null)
                            result.WeeniePropertiesSpellBook.Add(new WeeniePropertiesSpellBook { Spell = value.SpellId, Probability = (float?)value.Stats.CastingChance ?? 0f });
                    }
                }

                if (input.StringStats != null)
                {
                    foreach (var value in input.StringStats)
                    {
                        if (result.WeeniePropertiesString.FirstOrDefault(x => x.Type == (ushort)value.Key) == null)
                            result.WeeniePropertiesString.Add(new WeeniePropertiesString { Type = (ushort)value.Key, Value = value.Value });
                    }
                }

                // WeeniePropertiesTextureMap

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Converts ACE weenie to LSD weenie
        /// </summary>
        public static bool TryConvert(Weenie input, out global::Lifestoned.DataModel.Gdle.Weenie result, bool correctForEnumShift = false)
        {
            if (input.ClassId == 0)
            {
                result = null;
                return false;
            }

            try
            {
                result = new global::Lifestoned.DataModel.Gdle.Weenie();

                result.WeenieId = input.ClassId;

                //if (input.WeenieClassId <= ushort.MaxValue && System.Enum.IsDefined(typeof(WeenieClassId), (ushort)input.WeenieClassId))
                //    result.ClassName = System.Enum.GetName(typeof(WeenieClassId), (ushort)input.WeenieClassId).ToLower();
                //else if (input.WeenieClassId <= ushort.MaxValue && System.Enum.IsDefined(typeof(WeenieClassName), (ushort)input.WeenieClassId))
                //{
                //    var clsName = System.Enum.GetName(typeof(WeenieClassName), input.WeenieClassId).ToLower().Substring(2);
                //    result.ClassName = clsName.Substring(0, clsName.Length - 6);
                //}
                //else
                //    result.ClassName = "ace" + input.WeenieClassId + "-" + input.Name.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(":", "").Replace("_", "").Replace("-", "").Replace(",", "").ToLower();

                //result.Name = input.ClassName;

                result.WeenieTypeId = input.Type;

                if (input.WeeniePropertiesBook != null)
                {
                    result.Book = new global::Lifestoned.DataModel.Gdle.Book();
                    result.Book.MaxNumberPages = input.WeeniePropertiesBook.MaxNumPages;
                    result.Book.MaxCharactersPerPage = input.WeeniePropertiesBook.MaxNumCharsPerPage;

                    if (input.WeeniePropertiesBookPageData != null && input.WeeniePropertiesBookPageData.Count > 0)
                    {
                        result.Book.Pages = new List<global::Lifestoned.DataModel.Gdle.Page>();

                        //uint pageId = 0;

                        foreach (var value in input.WeeniePropertiesBookPageData.OrderBy(p => p.PageId))
                        {
                            result.Book.Pages.Add(new global::Lifestoned.DataModel.Gdle.Page
                            {
                                 //= pageId,

                                AuthorId = value.AuthorId,
                                AuthorName = value.AuthorName,
                                AuthorAccount = value.AuthorAccount,
                                IgnoreAuthor = value.IgnoreAuthor,
                                PageText = value.PageText,
                            });

                            //pageId++;
                        }
                    }
                }

                // LandblockInstance

                // PointsOfInterest

                // WeeniePropertiesAnimPart

                if (input.WeeniePropertiesAttribute != null && input.WeeniePropertiesAttribute.Count > 0)
                {
                    if (result.Attributes == null)
                        result.Attributes = new global::Lifestoned.DataModel.Gdle.AttributeSet();

                    result.Attributes.Strength = new global::Lifestoned.DataModel.Gdle.Attribute
                    {
                        Ranks = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Strength).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Strength).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Strength).CPSpent
                    };

                    result.Attributes.Endurance = new global::Lifestoned.DataModel.Gdle.Attribute
                    {
                        Ranks = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Endurance).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Endurance).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Endurance).CPSpent
                    };

                    result.Attributes.Quickness = new global::Lifestoned.DataModel.Gdle.Attribute
                    {
                        Ranks = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Quickness).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Quickness).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Quickness).CPSpent
                    };

                    result.Attributes.Coordination = new global::Lifestoned.DataModel.Gdle.Attribute
                    {
                        Ranks = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Coordination).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Coordination).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Coordination).CPSpent
                    };

                    result.Attributes.Focus = new global::Lifestoned.DataModel.Gdle.Attribute
                    {
                        Ranks = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Focus).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Focus).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Focus).CPSpent
                    };

                    result.Attributes.Self = new global::Lifestoned.DataModel.Gdle.Attribute
                    {
                        Ranks = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Self).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Self).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute.Self).CPSpent
                    };
                }

                if (input.WeeniePropertiesAttribute2nd != null && input.WeeniePropertiesAttribute2nd.Count > 0)
                {
                    if (result.Attributes == null)
                        result.Attributes = new global::Lifestoned.DataModel.Gdle.AttributeSet();

                    result.Attributes.Health = new global::Lifestoned.DataModel.Gdle.Vital
                    {
                        Ranks = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxHealth).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxHealth).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxHealth).CPSpent,
                        Current = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxHealth).CurrentLevel
                    };

                    result.Attributes.Stamina = new global::Lifestoned.DataModel.Gdle.Vital
                    {
                        Ranks = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxStamina).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxStamina).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxStamina).CPSpent,
                        Current = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxStamina).CurrentLevel
                    };

                    result.Attributes.Mana = new global::Lifestoned.DataModel.Gdle.Vital
                    {
                        Ranks = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxMana).InitLevel,
                        LevelFromCp = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxMana).LevelFromCP,
                        XpSpent = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxMana).CPSpent,
                        Current = input.WeeniePropertiesAttribute2nd.FirstOrDefault(a => a.Type == (ushort)PropertyAttribute2nd.MaxMana).CurrentLevel
                    };
                }

                if (input.WeeniePropertiesBodyPart != null && input.WeeniePropertiesBodyPart.Count > 0)
                {
                    result.Body = new global::Lifestoned.DataModel.Gdle.Body();
                    result.Body.BodyParts = new List<global::Lifestoned.DataModel.Gdle.BodyPartListing>();

                    foreach (var value in input.WeeniePropertiesBodyPart)
                    {
                        result.Body.BodyParts.Add(new global::Lifestoned.DataModel.Gdle.BodyPartListing
                        {
                            Key = value.Key,
                            BodyPartType = (global::Lifestoned.DataModel.Shared.BodyPartType)value.Key,
                            BodyPart = new global::Lifestoned.DataModel.Gdle.BodyPart
                            {
                                DType = value.DType,
                                DVal = value.DVal,
                                DVar = value.DVar,
                                ArmorValues = new global::Lifestoned.DataModel.Gdle.ArmorValues
                                {
                                    BaseArmor = value.BaseArmor,
                                    ArmorVsSlash = value.ArmorVsSlash,
                                    ArmorVsPierce = value.ArmorVsPierce,
                                    ArmorVsBludgeon = value.ArmorVsBludgeon,
                                    ArmorVsCold = value.ArmorVsCold,
                                    ArmorVsFire = value.ArmorVsFire,
                                    ArmorVsAcid = value.ArmorVsAcid,
                                    ArmorVsElectric = value.ArmorVsElectric,
                                    ArmorVsNether = value.ArmorVsNether
                                },
                                BH = value.BH,
                                SD = new global::Lifestoned.DataModel.Gdle.Zones
                                {
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
                                }
                            }
                        });
                    }
                }

                if (input.WeeniePropertiesBool != null && input.WeeniePropertiesBool.Count > 0)
                {
                    result.BoolStats = new List<global::Lifestoned.DataModel.Gdle.BoolStat>();

                    foreach (var value in input.WeeniePropertiesBool)
                    {
                        if (result.BoolStats.FirstOrDefault(x => x.Key == value.Type) == null)
                            result.BoolStats.Add(new global::Lifestoned.DataModel.Gdle.BoolStat { Key = value.Type, Value = Convert.ToInt32(value.Value) });
                    }
                }

                if (input.WeeniePropertiesCreateList != null && input.WeeniePropertiesCreateList.Count > 0)
                {
                    result.CreateList = new List<global::Lifestoned.DataModel.Gdle.CreateItem>();

                    foreach (var value in input.WeeniePropertiesCreateList)
                        result.CreateList.Add(new global::Lifestoned.DataModel.Gdle.CreateItem
                        {
                            WeenieClassId = value.WeenieClassId,
                            Palette = (uint)value.Palette,
                            Shade = value.Shade,
                            Destination = (uint)value.DestinationType,
                            StackSize = value.StackSize,
                            TryToBond = Convert.ToByte(value.TryToBond)
                        });
                }

                if (input.WeeniePropertiesDID != null && input.WeeniePropertiesDID.Count > 0)
                {
                    result.DidStats = new List<global::Lifestoned.DataModel.Gdle.DidStat>();

                    foreach (var value in input.WeeniePropertiesDID)
                    {
                        result.DidStats.Add(new global::Lifestoned.DataModel.Gdle.DidStat { Key = value.Type, Value = value.Value });
                    }
                }

                if (input.WeeniePropertiesEmote != null && input.WeeniePropertiesEmote.Count > 0)
                {
                    result.EmoteTable = new List<global::Lifestoned.DataModel.Gdle.EmoteCategoryListing>();

                    var eorder = 0;
                    foreach (var emote in input.WeeniePropertiesEmote)
                    {
                        var ec = new global::Lifestoned.DataModel.Gdle.EmoteCategoryListing
                        {
                            EmoteCategoryId = (int)emote.Category,
                            Emotes = new List<global::Lifestoned.DataModel.Gdle.Emote>(),
                        };

                        var em = new global::Lifestoned.DataModel.Gdle.Emote
                        {
                            SortOrder = eorder,
                            Category = emote.Category,
                            EmoteCategory = (global::Lifestoned.DataModel.Shared.EmoteCategory)emote.Category,
                            ClassId = emote.WeenieClassId,
                            MaxHealth = emote.MaxHealth,
                            MinHealth = emote.MinHealth,
                            Probability = emote.Probability,
                            Quest = emote.Quest,
                            Style = emote.Style,
                            SubStyle = emote.Substyle,
                            VendorType = (uint?)emote.VendorType,
                            Actions = new List<global::Lifestoned.DataModel.Gdle.EmoteAction>()
                        };

                        var aorder = 0;
                        foreach (var action in emote.WeeniePropertiesEmoteAction.OrderBy(e => e.Order))
                        {
                            var ea = new global::Lifestoned.DataModel.Gdle.EmoteAction
                            {
                                SortOrder = aorder,
                                Amount = (uint?)action.Amount,
                                Amount64 = (uint?)action.Amount64,
                                Delay = action.Delay,
                                EmoteActionType = action.Type,
                                Extent = action.Extent,
                                FMax = (float?)action.MaxDbl,
                                FMin = (float?)action.MinDbl,
                                //Frame = new global::Lifestoned.DataModel.Gdle.Frame
                                //{
                                //    Position = new global::Lifestoned.DataModel.Gdle.XYZ
                                //    {
                                //        X = action.OriginX ?? 0,
                                //        Y = action.OriginY ?? 0,
                                //        Z = action.OriginZ ?? 0
                                //    },
                                //    Rotations = new global::Lifestoned.DataModel.Gdle.Quaternion
                                //    {
                                //        W = action.AnglesW ?? 0,
                                //        X = action.AnglesX ?? 0,
                                //        Y = action.AnglesY ?? 0,
                                //        Z = action.AnglesZ ?? 0
                                //    }
                                //},
                                HeroXp64 = (ulong?)action.HeroXP64,
                                //Item = new global::Lifestoned.DataModel.Gdle.CreateItem
                                //{
                                //    Destination = (uint?)action.DestinationType,
                                //    Palette = (uint?)action.Palette,
                                //    Shade = action.Shade,
                                //    StackSize = action.StackSize,
                                //    TryToBond = Convert.ToByte(action.TryToBond),
                                //    WeenieClassId = action.WeenieClassId
                                //},
                                Max = (uint?)action.Max,
                                Min = (uint?)action.Min,
                                Maximum64 = action.Max64,
                                Minimum64 = action.Min64,
                                Message = action.Message,
                                Motion = (uint?)action.Motion,
                                //MPosition = new global::Lifestoned.DataModel.Gdle.Position
                                //{
                                //    LandCellId = action.ObjCellId ?? 0,
                                //    Frame = new global::Lifestoned.DataModel.Gdle.Frame
                                //    {
                                //        Position = new global::Lifestoned.DataModel.Gdle.XYZ
                                //        {
                                //            X = action.OriginX ?? 0,
                                //            Y = action.OriginY ?? 0,
                                //            Z = action.OriginZ ?? 0
                                //        },
                                //        Rotations = new global::Lifestoned.DataModel.Gdle.Quaternion
                                //        {
                                //            W = action.AnglesW ?? 0,
                                //            X = action.AnglesX ?? 0,
                                //            Y = action.AnglesY ?? 0,
                                //            Z = action.AnglesZ ?? 0
                                //        }
                                //    }
                                //},
                                Percent = (float?)action.Percent,
                                PScript = (uint?)action.PScript,
                                Sound = (uint?)action.Sound,
                                SpellId = (uint?)action.SpellId,
                                Stat = (uint?)action.Stat,
                                TestString = action.TestString,
                                TreasureClass = (uint?)action.TreasureClass,
                                WealthRating = (uint?)action.WealthRating,
                                TreasureType = action.TreasureType,
                            };

                            if (action.ObjCellId.HasValue)
                            {
                                ea.MPosition = new global::Lifestoned.DataModel.Gdle.Position
                                {
                                    LandCellId = action.ObjCellId ?? 0,
                                    Frame = new global::Lifestoned.DataModel.Gdle.Frame
                                    {
                                        Position = new global::Lifestoned.DataModel.Gdle.XYZ
                                        {
                                            X = action.OriginX ?? 0,
                                            Y = action.OriginY ?? 0,
                                            Z = action.OriginZ ?? 0
                                        },
                                        Rotations = new global::Lifestoned.DataModel.Gdle.Quaternion
                                        {
                                            W = action.AnglesW ?? 0,
                                            X = action.AnglesX ?? 0,
                                            Y = action.AnglesY ?? 0,
                                            Z = action.AnglesZ ?? 0
                                        }
                                    }
                                };
                            }
                            else if (action.OriginX.HasValue || action.OriginY.HasValue || action.OriginZ.HasValue || action.AnglesW.HasValue || action.AnglesX.HasValue || action.AnglesY.HasValue || action.AnglesZ.HasValue)
                            {
                                ea.Frame = new global::Lifestoned.DataModel.Gdle.Frame
                                {
                                    Position = new global::Lifestoned.DataModel.Gdle.XYZ
                                    {
                                        X = action.OriginX ?? 0,
                                        Y = action.OriginY ?? 0,
                                        Z = action.OriginZ ?? 0
                                    },
                                    Rotations = new global::Lifestoned.DataModel.Gdle.Quaternion
                                    {
                                        W = action.AnglesW ?? 0,
                                        X = action.AnglesX ?? 0,
                                        Y = action.AnglesY ?? 0,
                                        Z = action.AnglesZ ?? 0
                                    }
                                };
                            }

                            if (action.DestinationType.HasValue)
                            {
                                ea.Item = new global::Lifestoned.DataModel.Gdle.CreateItem
                                {
                                    Destination = (uint?)action.DestinationType,
                                    Palette = (uint?)action.Palette,
                                    Shade = action.Shade,
                                    StackSize = action.StackSize,
                                    TryToBond = Convert.ToByte(action.TryToBond),
                                    WeenieClassId = action.WeenieClassId
                                };
                            }

                            aorder++;

                            em.Actions.Add(ea);
                        }

                        ec.Emotes.Add(em);

                        var existingEC = result.EmoteTable.FirstOrDefault(e => e.EmoteCategoryId == (int)emote.Category);

                        if (existingEC == null)
                            result.EmoteTable.Add(ec);
                        else
                            existingEC.Emotes.Add(em);

                        eorder++;
                    }
                }

                // WeeniePropertiesEventFilter

                if (input.WeeniePropertiesFloat != null && input.WeeniePropertiesFloat.Count > 0)
                {
                    foreach (var value in input.WeeniePropertiesFloat)
                    {
                        if (result.FloatStats.FirstOrDefault(x => x.Key == value.Type) == null)
                            result.FloatStats.Add(new global::Lifestoned.DataModel.Gdle.FloatStat { Key = value.Type, Value = (float)value.Value });
                    }
                }

                if (input.WeeniePropertiesGenerator != null && input.WeeniePropertiesGenerator.Count > 0)
                {
                    result.GeneratorTable = new List<global::Lifestoned.DataModel.Gdle.GeneratorTable>();
                    uint slot = 0;
                    foreach (var value in input.WeeniePropertiesGenerator)
                    {
                        result.GeneratorTable.Add(new global::Lifestoned.DataModel.Gdle.GeneratorTable
                        {
                            Slot = slot,
                            Probability = value.Probability,
                            WeenieClassId = value.WeenieClassId,
                            Delay = value.Delay ?? 0,

                            InitCreate = (uint)value.InitCreate,
                            MaxNumber = (uint)value.MaxCreate,

                            WhenCreate = value.WhenCreate,
                            WhereCreate = value.WhereCreate,

                            StackSize = value.StackSize ?? 0,

                            PaletteId = value.PaletteId ?? 0,
                            Shade = value.Shade ?? 0,

                            ObjectCell = value.ObjCellId ?? 0,
                            Frame = new global::Lifestoned.DataModel.Gdle.Frame
                            {
                                Position = new global::Lifestoned.DataModel.Gdle.XYZ
                                {
                                    X = value.OriginX ?? 0,
                                    Y = value.OriginY ?? 0,
                                    Z = value.OriginZ ?? 0
                                },
                                Rotations = new global::Lifestoned.DataModel.Gdle.Quaternion
                                {
                                    W = value.AnglesW ?? 0,
                                    X = value.AnglesX ?? 0,
                                    Y = value.AnglesY ?? 0,
                                    Z = value.AnglesZ ?? 0
                                }
                            }
                        });
                        slot++;
                    }
                }

                if (input.WeeniePropertiesIID != null && input.WeeniePropertiesIID.Count > 0)
                {
                    result.IidStats = new List<global::Lifestoned.DataModel.Gdle.IidStat>();

                    foreach (var value in input.WeeniePropertiesIID)
                    {
                        if (result.IidStats.FirstOrDefault(x => x.Key == value.Type) == null)
                            result.IidStats.Add(new global::Lifestoned.DataModel.Gdle.IidStat { Key = value.Type, Value = (int)value.Value });
                    }
                }

                if (input.WeeniePropertiesInt != null && input.WeeniePropertiesInt.Count > 0)
                {
                    result.IntStats = new List<global::Lifestoned.DataModel.Gdle.IntStat>();

                    foreach (var value in input.WeeniePropertiesInt)
                    {
                        if (result.IntStats.FirstOrDefault(x => x.Key == value.Type) == null)
                            result.IntStats.Add(new global::Lifestoned.DataModel.Gdle.IntStat { Key = value.Type, Value = value.Value });
                    }
                }

                if (input.WeeniePropertiesInt64 != null && input.WeeniePropertiesInt64.Count > 0)
                {
                    result.Int64Stats = new List<global::Lifestoned.DataModel.Gdle.Int64Stat>();

                    foreach (var value in input.WeeniePropertiesInt64)
                    {
                        if (result.Int64Stats.FirstOrDefault(x => x.Key == value.Type) == null)
                            result.Int64Stats.Add(new global::Lifestoned.DataModel.Gdle.Int64Stat { Key = value.Type, Value = value.Value });
                    }
                }

                // WeeniePropertiesPalette

                if (input.WeeniePropertiesPosition != null && input.WeeniePropertiesPosition.Count > 0)
                {
                    result.Positions = new List<global::Lifestoned.DataModel.Gdle.PositionListing>();

                    foreach (var value in input.WeeniePropertiesPosition)
                    {
                        if (result.Positions.FirstOrDefault(x => x.PositionType == value.PositionType) == null)
                            result.Positions.Add(new global::Lifestoned.DataModel.Gdle.PositionListing
                            {
                                PositionType = value.PositionType,
                                Position = new global::Lifestoned.DataModel.Gdle.Position
                                {
                                    LandCellId = value.ObjCellId,
                                    Frame = new global::Lifestoned.DataModel.Gdle.Frame
                                    {
                                        Position = new global::Lifestoned.DataModel.Gdle.XYZ
                                        {
                                            X = value.OriginX,
                                            Y = value.OriginY,
                                            Z = value.OriginZ,
                                        },
                                        Rotations = new global::Lifestoned.DataModel.Gdle.Quaternion
                                        {
                                            W = value.AnglesW,
                                            X = value.AnglesX,
                                            Y = value.AnglesY,
                                            Z = value.AnglesZ
                                        }
                                    }
                                }
                            });
                    }
                }

                if (input.WeeniePropertiesSkill != null && input.WeeniePropertiesSkill.Count > 0)
                {
                    result.Skills = new List<global::Lifestoned.DataModel.Gdle.SkillListing>();

                    foreach (var value in input.WeeniePropertiesSkill)
                    {
                        if (result.Skills.FirstOrDefault(x => x.SkillId == value.Type) == null)
                            result.Skills.Add(new global::Lifestoned.DataModel.Gdle.SkillListing
                            {
                                SkillId = value.Type,
                                Skill = new global::Lifestoned.DataModel.Gdle.Skill
                                {
                                    LevelFromPp = value.LevelFromPP,
                                    TrainedLevel = (int)value.SAC,
                                    XpInvested = value.PP,
                                    Ranks = value.InitLevel,
                                    ResistanceOfLastCheck = value.ResistanceAtLastCheck,
                                    LastUsed = (float)value.LastUsedTime
                                }
                            });
                    }
                }

                if (input.WeeniePropertiesSpellBook != null && input.WeeniePropertiesSpellBook.Count > 0)
                {
                    result.Spells = new List<global::Lifestoned.DataModel.Gdle.SpellbookEntry>();

                    foreach (var value in input.WeeniePropertiesSpellBook)
                    {
                        if (result.Spells.FirstOrDefault(x => x.SpellId == value.Spell) == null)
                            result.Spells.Add(new global::Lifestoned.DataModel.Gdle.SpellbookEntry
                            {
                                SpellId = value.Spell,
                                Stats = new global::Lifestoned.DataModel.Gdle.SpellCastingStats
                                {
                                    CastingChance = value.Probability
                                }
                            });
                    }
                }

                if (input.WeeniePropertiesString != null && input.WeeniePropertiesString.Count > 0)
                {
                    result.StringStats = new List<global::Lifestoned.DataModel.Gdle.StringStat>();

                    foreach (var value in input.WeeniePropertiesString)
                    {
                        if (result.StringStats.FirstOrDefault(x => x.Key == value.Type) == null)
                            result.StringStats.Add(new global::Lifestoned.DataModel.Gdle.StringStat { Key = value.Type, Value = value.Value });
                    }
                }

                // WeeniePropertiesTextureMap

                result.Changelog = new List<global::Lifestoned.DataModel.Shared.ChangelogEntry>();
                result.Changelog.Add(new global::Lifestoned.DataModel.Shared.ChangelogEntry
                {
                    Author = "ACE.Adapter",
                    Comment = "Weenie exported from ACEmulator world database using ACE.Adapter",
                    Created = DateTime.UtcNow
                });

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryConvertACEWeeniesToLSDJSON(List<Weenie> weenies, out List<string> results)
        {
            try
            {
                results = new List<string>();

                foreach (var weenie in weenies)
                {
                    if (TryConvert(weenie, out var result))
                    {
                        results.Add(JsonConvert.SerializeObject(result));
                    }
                }

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings();

        static LifestonedConverter()
        {
            //SerializerSettings.Converters.Add(new JavaScriptDateTimeConverter());
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

            SerializerSettings.Formatting = Formatting.Indented;
        }

        public static bool TryConvertACEWeenieToLSDJSON(Weenie weenie, out string result, out global::Lifestoned.DataModel.Gdle.Weenie lsdWeenie)
        {
            try
            {
                if (TryConvert(weenie, out lsdWeenie))
                {
                    try
                    {
                        result = JsonConvert.SerializeObject(lsdWeenie, SerializerSettings);
                    }
                    catch
                    {
                        result = "serialize failed";
                        return false;
                    }
                    return true;
                }

                result = "try convert failed";
                return false;
            }
            catch
            {
                lsdWeenie = null;
                result = null;
                return false;
            }
        }
    }
}
