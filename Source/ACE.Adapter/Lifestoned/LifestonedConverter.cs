using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Adapter.Enum;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Adapter.Lifestoned
{
    public static class LifestonedConverter
    {
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
                    result.ClassName = "ace" + input.WeenieClassId + "-" + input.Name.Replace("'", "").Replace(" ", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace(":", "").Replace("_", "").Replace("-", "").Replace(",", "").ToLower();

                result.Type = input.WeenieTypeId;

                if (input.Book != null)
                {
                    result.WeeniePropertiesBook = new WeeniePropertiesBook();
                    result.WeeniePropertiesBook.MaxNumPages = result.WeeniePropertiesBook.MaxNumPages;
                    result.WeeniePropertiesBook.MaxNumCharsPerPage = result.WeeniePropertiesBook.MaxNumCharsPerPage;

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
                    result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Strength, InitLevel = input.Attributes.Strength.Ranks ?? 0, LevelFromCP = input.Attributes.Strength.LevelFromCp, CPSpent = input.Attributes.Strength.XpSpent ?? 0 });
                    result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Endurance, InitLevel = input.Attributes.Endurance.Ranks ?? 0, LevelFromCP = input.Attributes.Endurance.LevelFromCp, CPSpent = input.Attributes.Endurance.XpSpent ?? 0 });
                    result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Quickness, InitLevel = input.Attributes.Quickness.Ranks ?? 0, LevelFromCP = input.Attributes.Quickness.LevelFromCp, CPSpent = input.Attributes.Quickness.XpSpent ?? 0 });
                    result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Coordination, InitLevel = input.Attributes.Coordination.Ranks ?? 0, LevelFromCP = input.Attributes.Coordination.LevelFromCp, CPSpent = input.Attributes.Coordination.XpSpent ?? 0 });
                    result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Focus, InitLevel = input.Attributes.Focus.Ranks ?? 0, LevelFromCP = input.Attributes.Focus.LevelFromCp, CPSpent = input.Attributes.Focus.XpSpent ?? 0 });
                    result.WeeniePropertiesAttribute.Add(new WeeniePropertiesAttribute { Type = (ushort)PropertyAttribute.Self, InitLevel = input.Attributes.Self.Ranks ?? 0, LevelFromCP = input.Attributes.Self.LevelFromCp, CPSpent = input.Attributes.Self.XpSpent ?? 0 });

                    result.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxHealth, InitLevel = input.Attributes.Health.Ranks ?? 0, LevelFromCP = input.Attributes.Health.LevelFromCp ?? 0, CPSpent = input.Attributes.Health.XpSpent ?? 0, CurrentLevel = input.Attributes.Health.Current ?? 0 });
                    result.WeeniePropertiesAttribute2nd.Add(new WeeniePropertiesAttribute2nd { Type = (ushort)PropertyAttribute2nd.MaxStamina, InitLevel = input.Attributes.Stamina.Ranks ?? 0, LevelFromCP = input.Attributes.Stamina.LevelFromCp ?? 0, CPSpent = input.Attributes.Stamina.XpSpent ?? 0, CurrentLevel = input.Attributes.Stamina.Current ?? 0 });
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
                                    var oldMotion = (ACE.Entity.Enum.MotionCommand)efAction.Motion;
                                    var index = efAction.Motion.Value & 0xFFFF;
                                    if (index >= 0x115)
                                    {
                                        var newMotion = (ACE.Entity.Enum.MotionCommand)efAction.Motion + 3;
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
    }
}
