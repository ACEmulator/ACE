using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using ACE.Database.Models.World;
using ACE.Entity.Enum.Properties;

namespace ACE.Adapter.Lifestoned
{
    public static class Test
    {
        public static bool TryLoadWeenie(string file, out global::Lifestoned.DataModel.Gdle.Weenie result)
        {
            try
            {
                var fileText = File.ReadAllText(file);

                result = JsonConvert.DeserializeObject<global::Lifestoned.DataModel.Gdle.Weenie>(fileText);

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static bool TryLoadWeenies(string folder, out List<global::Lifestoned.DataModel.Gdle.Weenie> results)
        {
            results = new List<global::Lifestoned.DataModel.Gdle.Weenie>();

            try
            {
                var files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories).OrderByDescending(f => new FileInfo(f).CreationTime).ToList();

                foreach (var file in files)
                {
                    if (TryLoadWeenie(file, out var result))
                        results.Add(result);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryLoadWeeniesInParallel(string folder, out List<global::Lifestoned.DataModel.Gdle.Weenie> results)
        {
            try
            {
                var files = Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories);

                var weenies = new ConcurrentBag<global::Lifestoned.DataModel.Gdle.Weenie>();

                Parallel.ForEach(files, file =>
                {
                    if (TryLoadWeenie(file, out var result))
                        weenies.Add(result);
                });

                results = new List<global::Lifestoned.DataModel.Gdle.Weenie>(weenies);

                return true;
            }
            catch
            {
                results = null;
                return false;
            }
        }

        public static bool TryConvert(global::Lifestoned.DataModel.Gdle.Weenie input, out Weenie result)
        {
            try
            {
                result = new Weenie();

                result.ClassId = input.WeenieId;
                //weenie.ClassName;
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
                        result.WeeniePropertiesBool.Add(new WeeniePropertiesBool { Type = (ushort)value.Key, Value = (value.Value != 0) });
                }

                if (input.CreateList != null)
                {
                    foreach (var value in input.CreateList)
                        result.WeeniePropertiesCreateList.Add(new WeeniePropertiesCreateList { WeenieClassId = value.WeenieClassId ?? 0, Palette = (sbyte?)value.Palette ?? 0, Shade = (float?)value.Shade ?? 0, DestinationType = (sbyte?)value.Destination ?? 0, StackSize = value.StackSize ?? 0, TryToBond = (value.TryToBond != 0) });
                }

                if (input.DidStats != null)
                {
                    foreach (var value in input.DidStats)
                        result.WeeniePropertiesDID.Add(new WeeniePropertiesDID { Type = (ushort)value.Key, Value = value.Value });
                }

                // WeeniePropertiesEmote

                // WeeniePropertiesEventFilter

                if (input.FloatStats != null)
                {
                    foreach (var value in input.FloatStats)
                        result.WeeniePropertiesFloat.Add(new WeeniePropertiesFloat { Type = (ushort)value.Key, Value = value.Value });
                }

                // WeeniePropertiesGenerator

                if (input.IidStats != null)
                {
                    foreach (var value in input.IidStats)
                        result.WeeniePropertiesIID.Add(new WeeniePropertiesIID { Type = (ushort)value.Key, Value = (uint)value.Value });
                }

                if (input.IntStats != null)
                {
                    foreach (var value in input.IntStats)
                        result.WeeniePropertiesInt.Add(new WeeniePropertiesInt { Type = (ushort)value.Key, Value = value.Value });
                }

                if (input.Int64Stats != null)
                {
                    foreach (var value in input.Int64Stats)
                        result.WeeniePropertiesInt64.Add(new WeeniePropertiesInt64 { Type = (ushort)value.Key, Value = value.Value });
                }

                // WeeniePropertiesPalette

                if (input.Positions != null)
                {
                    foreach (var value in input.Positions)
                    {
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
                        result.WeeniePropertiesSkill.Add(new WeeniePropertiesSkill { Type = (ushort)value.SkillId, LevelFromPP = (ushort)value.Skill.LevelFromPp, SAC = (uint?)value.Skill.TrainedLevel ?? 0, PP = value.Skill.XpInvested ?? 0, InitLevel = value.Skill.Ranks ?? 0, ResistanceAtLastCheck = value.Skill.ResistanceOfLastCheck ?? 0, LastUsedTime = value.Skill.LastUsed ?? 0 });
                }

                if (input.Spells != null)
                {
                    foreach (var value in input.Spells)
                        result.WeeniePropertiesSpellBook.Add(new WeeniePropertiesSpellBook { Spell = value.SpellId, Probability = (float?)value.Stats.CastingChance ?? 0f });
                }

                if (input.StringStats != null)
                {
                    foreach (var value in input.StringStats)
                        result.WeeniePropertiesString.Add(new WeeniePropertiesString { Type = (ushort)value.Key, Value = value.Value });
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
