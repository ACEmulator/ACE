using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.SQLFormatters.World
{
    public class WeenieSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.ClassId.ToString("00000") + " " + name
        /// </summary>
        public string GetDefaultFileName(Weenie input)
        {
            var name = input.WeeniePropertiesString.FirstOrDefault(r => r.Type == (int)PropertyString.Name);

            var fileName = input.ClassId.ToString("00000") + " " + (name != null ? name.Value : "");
            fileName = IllegalInFileName.Replace(fileName, "_");
            fileName += ".sql";

            return fileName;
        }

        /// <summary>
        /// This will create a default subfolder path with the following format:<para />
        /// [Weenie Type]\\[Creature Type]\\<para />
        /// or<para />
        /// [Weenie Type]\\[Item Type]\\
        /// </summary>
        public string GetDefaultSubfolder(Weenie input)
        {
            var subFolder = Enum.GetName(typeof(WeenieType), input.Type) + "\\";

            if (input.Type == (int)WeenieType.Creature)
            {
                var property = input.WeeniePropertiesInt.FirstOrDefault(r => r.Type == (int)PropertyInt.CreatureType);

                if (property != null)
                {
                    Enum.TryParse(property.Value.ToString(), out CreatureType ct);

                    if (Enum.IsDefined(typeof(CreatureType), ct))
                        subFolder += Enum.GetName(typeof(CreatureType), property.Value) + "\\";
                    else
                        subFolder += "UnknownCT_" + property.Value + "\\";
                }
                else
                    subFolder += "Unsorted" + "\\";
            }
            else if (input.Type == (int)WeenieType.House)
            {
                var property = input.WeeniePropertiesInt.FirstOrDefault(r => r.Type == (int)PropertyInt.HouseType);

                if (property != null)
                {
                    Enum.TryParse(property.Value.ToString(), out HouseType ht);

                    if (Enum.IsDefined(typeof(HouseType), ht))
                        subFolder += Enum.GetName(typeof(HouseType), property.Value) + "\\";
                    else
                        subFolder += "UnknownHT_" + property.Value + "\\";
                }
                else
                    subFolder += "Unsorted" + "\\";
            }
            else
            {
                var property = input.WeeniePropertiesInt.FirstOrDefault(r => r.Type == (int)PropertyInt.ItemType);

                if (property != null)
                    subFolder += Enum.GetName(typeof(ItemType), property.Value) + "\\";
                else
                    subFolder += Enum.GetName(typeof(ItemType), ItemType.None) + "\\";
            }

            return subFolder;
        }

        public void CreateSQLDELETEStatement(Weenie input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `weenie` WHERE `class_Id` = {input.ClassId};");
        }

        public void CreateSQLINSERTStatement(Weenie input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)");

            var output = $"VALUES ({input.ClassId}, '{input.ClassName}', {input.Type}, '{input.LastModified:yyyy-MM-dd HH:mm:ss}') /* {Enum.GetName(typeof(WeenieType), input.Type)} */;";

            output = FixNullFields(output);

            writer.WriteLine(output);

            if (input.WeeniePropertiesInt != null && input.WeeniePropertiesInt.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesInt.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesInt64 != null && input.WeeniePropertiesInt64.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesInt64.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesBool != null && input.WeeniePropertiesBool.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBool.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesFloat != null && input.WeeniePropertiesFloat.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesFloat.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesString != null && input.WeeniePropertiesString.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesString.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesDID != null && input.WeeniePropertiesDID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesDID.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.WeeniePropertiesPosition != null && input.WeeniePropertiesPosition.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesPosition.OrderBy(r => r.PositionType).ToList(), writer);
            }

            if (input.WeeniePropertiesIID != null && input.WeeniePropertiesIID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesIID.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.WeeniePropertiesAttribute != null && input.WeeniePropertiesAttribute.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAttribute.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.WeeniePropertiesAttribute2nd != null && input.WeeniePropertiesAttribute2nd.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAttribute2nd.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.WeeniePropertiesSkill != null && input.WeeniePropertiesSkill.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesSkill.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.WeeniePropertiesBodyPart != null && input.WeeniePropertiesBodyPart.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBodyPart.OrderBy(r => r.Key).ToList(), writer);
            }

            if (input.WeeniePropertiesSpellBook != null && input.WeeniePropertiesSpellBook.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesSpellBook.OrderBy(r => r.Spell).ToList(), writer);
            }

            if (input.WeeniePropertiesEventFilter != null && input.WeeniePropertiesEventFilter.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesEventFilter.OrderBy(r => r.Event).ToList(), writer);
            }

            if (input.WeeniePropertiesEmote != null && input.WeeniePropertiesEmote.Count > 0)
            {
                //writer.WriteLine(); // This is not needed because CreateSQLINSERTStatement will take care of it for us on each Recipe.
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesEmote.OrderBy(r => r.Category).ToList(), writer);
            }

            if (input.WeeniePropertiesCreateList != null && input.WeeniePropertiesCreateList.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesCreateList.OrderBy(r => r.DestinationType).ToList(), writer);
            }

            if (input.WeeniePropertiesBook != null)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBook, writer);
            }
            if (input.WeeniePropertiesBookPageData != null && input.WeeniePropertiesBookPageData.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesBookPageData.OrderBy(r => r.PageId).ToList(), writer);
            }

            if (input.WeeniePropertiesGenerator != null && input.WeeniePropertiesGenerator.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesGenerator.ToList(), writer);
            }

            if (input.WeeniePropertiesPalette != null && input.WeeniePropertiesPalette.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesPalette.OrderBy(r => r.SubPaletteId).ToList(), writer);
            }
            if (input.WeeniePropertiesTextureMap != null && input.WeeniePropertiesTextureMap.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesTextureMap.OrderBy(r => r.Index).ToList(), writer);
            }
            if (input.WeeniePropertiesAnimPart != null && input.WeeniePropertiesAnimPart.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.ClassId, input.WeeniePropertiesAnimPart.OrderBy(r => r.Index).ToList(), writer);
            }
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesInt> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string propertyValueDescription = GetValueEnumName((PropertyInt)input[i].Type, input[i].Value);

                var comment = Enum.GetName(typeof(PropertyInt), input[i].Type);
                if (propertyValueDescription != null)
                    comment += " - " + propertyValueDescription;

                return $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {comment} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesInt64> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {Enum.GetName(typeof(PropertyInt64), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesBool> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadRight(5)}) /* {Enum.GetName(typeof(PropertyBool), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesFloat> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString(CultureInfo.InvariantCulture).PadLeft(7)}) /* {Enum.GetName(typeof(PropertyFloat), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesString> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {GetSQLString(input[i].Value)}) /* {Enum.GetName(typeof(PropertyString), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesDID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string propertyValueDescription = GetValueEnumName((PropertyDataId)input[i].Type, input[i].Value);

                var comment = Enum.GetName(typeof(PropertyDataId), input[i].Type);
                if (propertyValueDescription != null)
                    comment += " - " + propertyValueDescription;

                return $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {comment} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesPosition> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {(uint)input[i].PositionType}, {input[i].ObjCellId}, {input[i].OriginX}, {input[i].OriginY}, {input[i].OriginZ}, {input[i].AnglesW}, {input[i].AnglesX}, {input[i].AnglesY}, {input[i].AnglesZ}) /* {Enum.GetName(typeof(PositionType), input[i].PositionType)} */" + Environment.NewLine + $"/* @teleloc 0x{input[i].ObjCellId:X8} [{input[i].OriginX:F6} {input[i].OriginY:F6} {input[i].OriginZ:F6}] {input[i].AnglesW:F6} {input[i].AnglesX:F6} {input[i].AnglesY:F6} {input[i].AnglesZ:F6} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesIID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {Enum.GetName(typeof(PropertyInstanceId), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesAttribute> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].InitLevel.ToString().PadLeft(3)}, {input[i].LevelFromCP}, {input[i].CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesAttribute2nd> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Type.ToString().PadLeft(3)}, {input[i].InitLevel.ToString().PadLeft(5)}, {input[i].LevelFromCP}, {input[i].CPSpent}, {input[i].CurrentLevel}) /* {Enum.GetName(typeof(PropertyAttribute2nd), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesSkill> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)");

            var lineGenerator = new Func<int, string>(i =>
                $"{weenieClassID}, " +
                $"{input[i].Type.ToString().PadLeft(2)}, " +
                $"{input[i].LevelFromPP}, " +
                $"{input[i].SAC}, " +
                $"{input[i].PP}, " +
                $"{input[i].InitLevel.ToString().PadLeft(3)}, " +
                $"{input[i].ResistanceAtLastCheck}, " +
                $"{input[i].LastUsedTime}) " +
                // ReSharper disable once PossibleNullReferenceException
                $"/* {Enum.GetName(typeof(Skill), input[i].Type).PadRight(19)} {((SkillAdvancementClass)input[i].SAC).ToString()} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesBodyPart> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, " +
                             "`d_Type`, `d_Val`, `d_Var`, " +
                             "`base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, " +
                             "`b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)");

            var lineGenerator = new Func<int, string>(i =>
                $"{weenieClassID}, " +
                $"{input[i].Key.ToString().PadLeft(2)}, " +
                $"{input[i].DType.ToString().PadLeft(2)}, " +
                $"{input[i].DVal.ToString().PadLeft(2)}, " +
                $"{input[i].DVar.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].BaseArmor.ToString().PadLeft(4)}, " +
                $"{input[i].ArmorVsSlash.ToString().PadLeft(4)}, " +
                $"{input[i].ArmorVsPierce.ToString().PadLeft(4)}, " +
                $"{input[i].ArmorVsBludgeon.ToString().PadLeft(4)}, " +
                $"{input[i].ArmorVsCold.ToString().PadLeft(4)}, " +
                $"{input[i].ArmorVsFire.ToString().PadLeft(4)}, " +
                $"{input[i].ArmorVsAcid.ToString().PadLeft(4)}, " +
                $"{input[i].ArmorVsElectric.ToString().PadLeft(4)}, " +
                $"{input[i].ArmorVsNether.ToString().PadLeft(4)}, " +
                $"{input[i].BH}, " +
                $"{input[i].HLF.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].MLF.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].LLF.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].HRF.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].MRF.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].LRF.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].HLB.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].MLB.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].LLB.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].HRB.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].MRB.ToString(CultureInfo.InvariantCulture).PadLeft(4)}, " +
                $"{input[i].LRB.ToString(CultureInfo.InvariantCulture).PadLeft(4)}) " +
                $"/* {Enum.GetName(typeof(CombatBodyPart), input[i].Key)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesSpellBook> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string label = null;

                if (SpellNames != null)
                    SpellNames.TryGetValue((uint)input[i].Spell, out label);

                return $"{weenieClassID}, {input[i].Spell.ToString().PadLeft(5)}, {input[i].Probability.ToString(CultureInfo.InvariantCulture).PadLeft(6)})  /* {label} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesEventFilter> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_event_filter` (`object_Id`, `event`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string label = null;

                if (PacketOpCodes != null)
                    PacketOpCodes.TryGetValue((uint)input[i].Event, out label);

                return $"{weenieClassID}, {input[i].Event.ToString().PadLeft(3)}) /* {label} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesEmote> input, StreamWriter writer)
        {
            foreach (var value in input)
            {
                writer.WriteLine();
                writer.WriteLine("INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)");

                var categoryLabel = Enum.GetName(typeof(EmoteCategory), value.Category);
                if (categoryLabel != null)
                    categoryLabel = $" /* {categoryLabel} */";

                string weenieClassIdLabel = null;
                if (WeenieNames != null && value.WeenieClassId.HasValue)
                {
                    WeenieNames.TryGetValue(value.WeenieClassId.Value, out weenieClassIdLabel);
                    if (weenieClassIdLabel != null)
                        weenieClassIdLabel = $" /* {weenieClassIdLabel} */";
                }

                string styleLabel = null;
                if (value.Style.HasValue)
                {
                    styleLabel = Enum.GetName(typeof(MotionStance), value.Style.Value);
                    if (styleLabel != null)
                        styleLabel = $" /* {styleLabel} */";
                }

                string substyleLabel = null;
                if (value.Substyle.HasValue)
                {
                    substyleLabel = Enum.GetName(typeof(MotionCommand), value.Substyle.Value);
                    if (substyleLabel != null)
                        substyleLabel = $" /* {substyleLabel} */";
                }

                string vendorTypeLabel = null;
                if (value.VendorType.HasValue)
                {
                    vendorTypeLabel = Enum.GetName(typeof(VendorType), value.VendorType.Value);
                    if (vendorTypeLabel != null)
                        vendorTypeLabel = $" /* {vendorTypeLabel} */";
                }

                var output = "VALUES (" +
                             $"{weenieClassID}, " +
                             $"{value.Category.ToString().PadLeft(2)}{categoryLabel}, " +
                             $"{value.Probability.ToString(CultureInfo.InvariantCulture).PadLeft(6)}, " +
                             $"{value.WeenieClassId}{weenieClassIdLabel}, " +
                             $"{value.Style}{styleLabel}, " +
                             $"{value.Substyle}{substyleLabel}, " +
                             $"{GetSQLString(value.Quest)}, " +
                             $"{value.VendorType}{vendorTypeLabel}, " +
                             $"{value.MinHealth}, " +
                             $"{value.MaxHealth}" +
                             ");";

                output = FixNullFields(output);

                writer.WriteLine(output);

                if (value.WeeniePropertiesEmoteAction != null && value.WeeniePropertiesEmoteAction.Count > 0)
                {
                    writer.WriteLine();
                    writer.WriteLine("SET @parent_id = LAST_INSERT_ID();");

                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.WeeniePropertiesEmoteAction.OrderBy(r => r.Order).ToList(), writer);
                }
            }
        }

        private void CreateSQLINSERTStatement(IList<WeeniePropertiesEmoteAction> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, " +
                             "`stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, " +
                             "`obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string typeLabel = Enum.GetName(typeof(EmoteType), input[i].Type);
                if (typeLabel != null)
                    typeLabel = $" /* {typeLabel} */";

                string motionLabel = null;
                if (input[i].Motion.HasValue)
                {
                    motionLabel = Enum.GetName(typeof(MotionCommand), input[i].Motion.Value);
                    if (motionLabel != null)
                        motionLabel = $" /* {motionLabel} */";
                }

                string spellIdLabel = null;
                if (SpellNames != null && input[i].SpellId.HasValue)
                {
                    SpellNames.TryGetValue((uint)input[i].SpellId.Value, out spellIdLabel);
                    if (spellIdLabel != null)
                        spellIdLabel = $" /* {spellIdLabel} */";
                }

                string pScriptLabel = null;
                if (input[i].PScript.HasValue)
                {
                    pScriptLabel = Enum.GetName(typeof(PlayScript), input[i].PScript.Value);
                    if (pScriptLabel != null)
                        pScriptLabel = $" /* {pScriptLabel} */";
                }

                string soundLabel = null;
                if (input[i].Sound.HasValue)
                {
                    soundLabel = Enum.GetName(typeof(Sound), input[i].Sound.Value);
                    if (soundLabel != null)
                        soundLabel = $" /* {soundLabel} */";
                }

                string weenieClassIdLabel = null;
                if (input[i].WeenieClassId.HasValue && WeenieNames != null)
                {
                    WeenieNames.TryGetValue(input[i].WeenieClassId.Value, out weenieClassIdLabel);
                    if (weenieClassIdLabel != null)
                        weenieClassIdLabel = $" /* {weenieClassIdLabel} */";
                }

                string destinationTypeLabel = null;
                if (input[i].DestinationType.HasValue)
                {
                    destinationTypeLabel = Enum.GetName(typeof(DestinationType), input[i].DestinationType.Value);
                    if (destinationTypeLabel != null)
                        destinationTypeLabel = $" /* {destinationTypeLabel} */";
                }

                string telelocLabel = null;
                if (input[i].ObjCellId.HasValue && input[i].ObjCellId.Value > 0)
                {
                    telelocLabel = $" /* @teleloc 0x{input[i].ObjCellId.Value:X8} [{input[i].OriginX.Value:F6} {input[i].OriginY.Value:F6} {input[i].OriginZ.Value:F6}] {input[i].AnglesW.Value:F6} {input[i].AnglesX.Value:F6} {input[i].AnglesY.Value:F6} {input[i].AnglesZ.Value:F6} */";
                }

                return
                    "@parent_id, " +
                    $"{input[i].Order.ToString().PadLeft(2)}, " +
                    $"{input[i].Type.ToString().PadLeft(3)}{typeLabel}, " +
                    $"{input[i].Delay}, " +
                    $"{input[i].Extent}, " +
                    $"{input[i].Motion}{motionLabel}, " +
                    $"{GetSQLString(input[i].Message)}, " +
                    $"{GetSQLString(input[i].TestString)}, " +
                    $"{input[i].Min}, " +
                    $"{input[i].Max}, " +
                    $"{input[i].Min64}, " +
                    $"{input[i].Max64}, " +
                    $"{input[i].MinDbl}, " +
                    $"{input[i].MaxDbl}, " +
                    $"{input[i].Stat}, " +
                    $"{input[i].Display}, " +
                    $"{input[i].Amount}, " +
                    $"{input[i].Amount64}, " +
                    $"{input[i].HeroXP64}, " +
                    $"{input[i].Percent}, " +
                    $"{input[i].SpellId}{spellIdLabel}, " +
                    $"{input[i].WealthRating}, " +
                    $"{input[i].TreasureClass}, " +
                    $"{input[i].TreasureType}, " +
                    $"{input[i].PScript}{pScriptLabel}, " +
                    $"{input[i].Sound}{soundLabel}, " +
                    $"{input[i].DestinationType}{destinationTypeLabel}, " +
                    $"{input[i].WeenieClassId}{weenieClassIdLabel}, " +
                    $"{input[i].StackSize}, " +
                    $"{input[i].Palette}, " +
                    $"{input[i].Shade}, " +
                    $"{input[i].TryToBond}, " +
                    $"{input[i].ObjCellId}{telelocLabel}, " +
                    $"{input[i].OriginX}, " +
                    $"{input[i].OriginY}, " +
                    $"{input[i].OriginZ}, " +
                    $"{input[i].AnglesW}, " +
                    $"{input[i].AnglesX}, " +
                    $"{input[i].AnglesY}, " +
                    $"{input[i].AnglesZ})";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesCreateList> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string weenieName = null;

                if (WeenieNames != null)
                    WeenieNames.TryGetValue(input[i].WeenieClassId, out weenieName);

                var label = weenieName + $" ({input[i].WeenieClassId})";

                if (input[i].WeenieClassId == 0)
                {
                    //label = GetValueForTreasureData(weenieClassID, true);
                    label = "nothing";
                }

                return $"{weenieClassID}, {input[i].DestinationType}, {input[i].WeenieClassId.ToString().PadLeft(5)}, {input[i].StackSize.ToString().PadLeft(2)}, {input[i].Palette}, {input[i].Shade}, {input[i].TryToBond}) /* Create {label ?? "Unknown"} for {Enum.GetName(typeof(DestinationType), input[i].DestinationType)} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, WeeniePropertiesBook input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_book` (`object_Id`, `max_Num_Pages`, `max_Num_Chars_Per_Page`)");

           writer.WriteLine($"VALUES ({weenieClassID}, {input.MaxNumPages}, {input.MaxNumCharsPerPage});");
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesBookPageData> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_book_page_data` (`object_Id`, `page_Id`, `author_Id`, `author_Name`, `author_Account`, `ignore_Author`, `page_Text`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].PageId}, {input[i].AuthorId}, {GetSQLString(input[i].AuthorName)}, {GetSQLString(input[i].AuthorAccount)}, {input[i].IgnoreAuthor}, {GetSQLString(input[i].PageText)})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesGenerator> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, " +
                             "`delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, " +
                             "`obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string weenieName = null;

                if (WeenieNames != null)
                    WeenieNames.TryGetValue(input[i].WeenieClassId, out weenieName);

                var label = weenieName + $" ({input[i].WeenieClassId})";

                if ((input[i].WhereCreate & (int)RegenLocationType.Treasure) != 0)
                {
                    label = GetValueForTreasureData(input[i].WeenieClassId, false);
                }

                return  $"{weenieClassID}, " +
                        $"{input[i].Probability}, " +
                        $"{input[i].WeenieClassId}, " +
                        $"{input[i].Delay}, " +
                        $"{input[i].InitCreate}, " +
                        $"{input[i].MaxCreate}, " +
                        $"{input[i].WhenCreate}, " +
                        $"{input[i].WhereCreate}, " +
                        $"{input[i].StackSize}, " +
                        $"{input[i].PaletteId}, " +
                        $"{input[i].Shade}, " +
                        $"{input[i].ObjCellId}, " +
                        $"{input[i].OriginX}, " +
                        $"{input[i].OriginY}, " +
                        $"{input[i].OriginZ}, " +
                        $"{input[i].AnglesW}, " +
                        $"{input[i].AnglesX}, " +
                        $"{input[i].AnglesY}, " +
                        $"{input[i].AnglesZ})" +
                        $" /* Generate {label} (x{input[i].InitCreate:N0} up to max of {input[i].MaxCreate:N0}) - Regenerate upon {Enum.GetName(typeof(RegenerationType), input[i].WhenCreate)} - Location to (re)Generate: {Enum.GetName(typeof(RegenLocationType), input[i].WhereCreate)} */";
            });
            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesPalette> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_palette` (`object_Id`, `sub_Palette_Id`, `offset`, `length`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].SubPaletteId}, {input[i].Offset}, {input[i].Length})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesTextureMap> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_texture_map` (`object_Id`, `index`, `old_Id`, `new_Id`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Index}, {input[i].OldId}, {input[i].NewId})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint weenieClassID, IList<WeeniePropertiesAnimPart> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `weenie_properties_anim_part` (`object_Id`, `index`, `animation_Id`)");

            var lineGenerator = new Func<int, string>(i => $"{weenieClassID}, {input[i].Index}, {input[i].AnimationId})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
