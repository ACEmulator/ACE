using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database.SQLFormatters.Shard
{
    public class BiotaSQLWriter : SQLWriter
    {
        /// <summary>
        /// Default is formed from: input.Id.ToString("X8") + " " + name
        /// </summary>
        public string GetDefaultFileName(Biota input)
        {
            var result = input.Id.ToString("X8");

            var name = input.GetProperty(PropertyString.Name);

            if (!String.IsNullOrWhiteSpace(name))
                result += " " + name;

            return result + ".sql";
        }

        public void CreateSQLDELETEStatement(Biota input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `biota` WHERE `id` = {input.Id};");
        }

        public void CreateSQLINSERTStatement(Biota input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota` (`id`, `weenie_Class_Id`, `weenie_Type`, `populated_Collection_Flags`)");

            // Default to all flags if none are set
            var output = $"VALUES ({input.Id}, {input.WeenieClassId}, {input.WeenieType}, {(input.PopulatedCollectionFlags != 0 ? input.PopulatedCollectionFlags : 4294967295)}) /* {Enum.GetName(typeof(WeenieType), input.WeenieType)} */;";

            output = FixNullFields(output);

            writer.WriteLine(output);

            if (input.BiotaPropertiesInt != null && input.BiotaPropertiesInt.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesInt.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.BiotaPropertiesInt64 != null && input.BiotaPropertiesInt64.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesInt64.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.BiotaPropertiesBool != null && input.BiotaPropertiesBool.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesBool.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.BiotaPropertiesFloat != null && input.BiotaPropertiesFloat.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesFloat.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.BiotaPropertiesString != null && input.BiotaPropertiesString.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesString.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.BiotaPropertiesDID != null && input.BiotaPropertiesDID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesDID.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.BiotaPropertiesPosition != null && input.BiotaPropertiesPosition.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesPosition.OrderBy(r => r.PositionType).ToList(), writer);
            }

            if (input.BiotaPropertiesIID != null && input.BiotaPropertiesIID.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesIID.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.BiotaPropertiesAttribute != null && input.BiotaPropertiesAttribute.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesAttribute.OrderBy(r => r.Type).ToList(), writer);
            }
            if (input.BiotaPropertiesAttribute2nd != null && input.BiotaPropertiesAttribute2nd.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesAttribute2nd.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.BiotaPropertiesSkill != null && input.BiotaPropertiesSkill.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesSkill.OrderBy(r => r.Type).ToList(), writer);
            }

            if (input.BiotaPropertiesBodyPart != null && input.BiotaPropertiesBodyPart.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesBodyPart.OrderBy(r => r.Key).ToList(), writer);
            }

            if (input.BiotaPropertiesSpellBook != null && input.BiotaPropertiesSpellBook.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesSpellBook.OrderBy(r => r.Spell).ToList(), writer);
            }

            if (input.BiotaPropertiesEventFilter != null && input.BiotaPropertiesEventFilter.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesEventFilter.OrderBy(r => r.Event).ToList(), writer);
            }

            if (input.BiotaPropertiesEmote != null && input.BiotaPropertiesEmote.Count > 0)
            {
                //writer.WriteLine(); // This is not needed because CreateSQLINSERTStatement will take care of it for us on each Recipe.
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesEmote.OrderBy(r => r.Category).ToList(), writer);
            }

            if (input.BiotaPropertiesCreateList != null && input.BiotaPropertiesCreateList.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesCreateList.OrderBy(r => r.DestinationType).ToList(), writer);
            }

            if (input.BiotaPropertiesBook != null)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesBook, writer);
            }
            if (input.BiotaPropertiesBookPageData != null && input.BiotaPropertiesBookPageData.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesBookPageData.OrderBy(r => r.PageId).ToList(), writer);
            }

            if (input.BiotaPropertiesGenerator != null && input.BiotaPropertiesGenerator.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesGenerator.ToList(), writer);
            }

            if (input.BiotaPropertiesPalette != null && input.BiotaPropertiesPalette.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesPalette.OrderBy(r => r.SubPaletteId).ToList(), writer);
            }
            if (input.BiotaPropertiesTextureMap != null && input.BiotaPropertiesTextureMap.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesTextureMap.OrderBy(r => r.Index).ToList(), writer);
            }
            if (input.BiotaPropertiesAnimPart != null && input.BiotaPropertiesAnimPart.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesAnimPart.OrderBy(r => r.Index).ToList(), writer);
            }

            if (input.BiotaPropertiesEnchantmentRegistry != null && input.BiotaPropertiesEnchantmentRegistry.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.BiotaPropertiesEnchantmentRegistry.ToList(), writer);
            }
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesInt> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_int` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string propertyValueDescription = GetValueEnumName((PropertyInt)input[i].Type, input[i].Value);

                var comment = Enum.GetName(typeof(PropertyInt), input[i].Type);
                if (propertyValueDescription != null)
                    comment += " - " + propertyValueDescription;

                return $"{id}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {comment} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesInt64> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_int64` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(12)}) /* {Enum.GetName(typeof(PropertyInt64), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesBool> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_bool` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadRight(5)}) /* {Enum.GetName(typeof(PropertyBool), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesFloat> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_float` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString(CultureInfo.InvariantCulture).PadLeft(7)}) /* {Enum.GetName(typeof(PropertyFloat), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesString> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_string` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Type.ToString().PadLeft(3)}, {GetSQLString(input[i].Value)}) /* {Enum.GetName(typeof(PropertyString), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesDID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_d_i_d` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string propertyValueDescription = GetValueEnumName((PropertyDataId)input[i].Type, input[i].Value);

                var comment = Enum.GetName(typeof(PropertyDataId), input[i].Type);
                if (propertyValueDescription != null)
                    comment += " - " + propertyValueDescription;

                return $"{id}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {comment} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesPosition> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {(uint)input[i].PositionType}, {input[i].ObjCellId}, {input[i].OriginX}, {input[i].OriginY}, {input[i].OriginZ}, {input[i].AnglesW}, {input[i].AnglesX}, {input[i].AnglesY}, {input[i].AnglesZ}) /* {Enum.GetName(typeof(PositionType), input[i].PositionType)} */" + Environment.NewLine + $"/* @teleloc 0x{input[i].ObjCellId:X8} [{input[i].OriginX:F6} {input[i].OriginY:F6} {input[i].OriginZ:F6}] {input[i].AnglesW:F6} {input[i].AnglesX:F6} {input[i].AnglesY:F6} {input[i].AnglesZ:F6} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesIID> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_i_i_d` (`object_Id`, `type`, `value`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Type.ToString().PadLeft(3)}, {input[i].Value.ToString().PadLeft(10)}) /* {Enum.GetName(typeof(PropertyInstanceId), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesAttribute> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Type.ToString().PadLeft(3)}, {input[i].InitLevel.ToString().PadLeft(3)}, {input[i].LevelFromCP}, {input[i].CPSpent}) /* {Enum.GetName(typeof(PropertyAttribute), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesAttribute2nd> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Type.ToString().PadLeft(3)}, {input[i].InitLevel.ToString().PadLeft(5)}, {input[i].LevelFromCP}, {input[i].CPSpent}, {input[i].CurrentLevel}) /* {Enum.GetName(typeof(PropertyAttribute2nd), input[i].Type)} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesSkill> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)");

            var lineGenerator = new Func<int, string>(i =>
                $"{id}, " +
                $"{input[i].Type.ToString().PadLeft(2)}, " +
                $"{input[i].LevelFromPP.ToString().PadLeft(3)}, " +
                $"{input[i].SAC}, " +
                $"{input[i].PP.ToString().PadLeft(11)}, " +
                $"{input[i].InitLevel.ToString().PadLeft(3)}, " +
                $"{input[i].ResistanceAtLastCheck.ToString().PadLeft(3)}, " +
                $"{input[i].LastUsedTime}) " +
                // ReSharper disable once PossibleNullReferenceException
                $"/* {Enum.GetName(typeof(Skill), input[i].Type).PadRight(19)} {((SkillAdvancementClass)input[i].SAC).ToString()} */");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesBodyPart> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_body_part` (`object_Id`, `key`, " +
                             "`d_Type`, `d_Val`, `d_Var`, " +
                             "`base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, " +
                             "`b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)");

            var lineGenerator = new Func<int, string>(i =>
                $"{id}, " +
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

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesSpellBook> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_spell_book` (`object_Id`, `spell`, `probability`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string label = null;

                if (SpellNames != null)
                    SpellNames.TryGetValue((uint)input[i].Spell, out label);

                return $"{id}, {input[i].Spell.ToString().PadLeft(5)}, {input[i].Probability.ToString(CultureInfo.InvariantCulture).PadLeft(6)})  /* {label} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesEventFilter> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_event_filter` (`object_Id`, `event`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string label = null;

                if (PacketOpCodes != null)
                    PacketOpCodes.TryGetValue((uint)input[i].Event, out label);

                return $"{id}, {input[i].Event.ToString().PadLeft(3)}) /* {label} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesEmote> input, StreamWriter writer)
        {
            foreach (var value in input)
            {
                writer.WriteLine();
                writer.WriteLine("INSERT INTO `biota_properties_emote` (`object_Id`, `category`, `probability`, `biota_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)");

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
                             $"{id}, " +
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

                if (value.BiotaPropertiesEmoteAction != null && value.BiotaPropertiesEmoteAction.Count > 0)
                {
                    writer.WriteLine();
                    writer.WriteLine("SET @parent_id = LAST_INSERT_ID();");

                    writer.WriteLine();
                    CreateSQLINSERTStatement(value.BiotaPropertiesEmoteAction.OrderBy(r => r.Order).ToList(), writer);
                }
            }
        }

        private void CreateSQLINSERTStatement(IList<BiotaPropertiesEmoteAction> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, " +
                             "`stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `biota_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, " +
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

                string spellweenieClassIdLabel = null;
                if (SpellNames != null && input[i].SpellId.HasValue)
                {
                    SpellNames.TryGetValue((uint)input[i].SpellId.Value, out spellweenieClassIdLabel);
                    if (spellweenieClassIdLabel != null)
                        spellweenieClassIdLabel = $" /* {spellweenieClassIdLabel} */";
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
                if (input[i].WeenieClassId.HasValue)
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
                    $"{input[i].SpellId}{spellweenieClassIdLabel}, " +
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

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesCreateList> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_create_list` (`object_Id`, `destination_Type`, `biota_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string BiotaName = null;

                if (WeenieNames != null)
                    WeenieNames.TryGetValue(input[i].WeenieClassId, out BiotaName);

                var label = BiotaName + $" ({input[i].WeenieClassId})";

                if (input[i].WeenieClassId == 0)
                {
                    //label = GetValueForTreasureData(id, true);
                    label = "nothing";
                }

                return $"{id}, {input[i].DestinationType}, {input[i].WeenieClassId.ToString().PadLeft(5)}, {input[i].StackSize.ToString().PadLeft(2)}, {input[i].Palette}, {input[i].Shade}, {input[i].TryToBond}) /* Create {label ?? "Unknown"} for {Enum.GetName(typeof(DestinationType), input[i].DestinationType)} */";
            });

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, BiotaPropertiesBook input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_book` (`object_Id`, `max_Num_Pages`, `max_Num_Chars_Per_Page`)");

            writer.WriteLine($"VALUES ({id}, {input.MaxNumPages}, {input.MaxNumCharsPerPage});");
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesBookPageData> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_book_page_data` (`object_Id`, `page_Id`, `author_Id`, `author_Name`, `author_Account`, `ignore_Author`, `page_Text`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].PageId}, {input[i].AuthorId}, {GetSQLString(input[i].AuthorName)}, {GetSQLString(input[i].AuthorAccount)}, {input[i].IgnoreAuthor}, {GetSQLString(input[i].PageText)})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesGenerator> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_generator` (`object_Id`, `probability`, `biota_Class_Id`, " +
                             "`delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, " +
                             "`obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)");

            var lineGenerator = new Func<int, string>(i =>
            {
                string BiotaName = null;

                if (WeenieNames != null)
                    WeenieNames.TryGetValue(input[i].WeenieClassId, out BiotaName);

                var label = BiotaName + $" ({input[i].WeenieClassId})";

                if ((input[i].WhereCreate & (int)RegenLocationType.Treasure) != 0)
                {
                    label = GetValueForTreasureData(input[i].WeenieClassId, false);
                }

                return $"{id}, " +
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

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesPalette> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_palette` (`object_Id`, `sub_Palette_Id`, `offset`, `length`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].SubPaletteId}, {input[i].Offset}, {input[i].Length})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesTextureMap> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_texture_map` (`object_Id`, `index`, `old_Id`, `new_Id`, `order`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Index}, {input[i].OldId}, {input[i].NewId}, {input[i].Order})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesAnimPart> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_anim_part` (`object_Id`, `index`, `animation_Id`, `order`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].Index}, {input[i].AnimationId}, {input[i].Order})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint id, IList<BiotaPropertiesEnchantmentRegistry> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `biota_properties_enchantment_registry` (`object_Id`, `enchantment_Category`, `spell_Id`, `layer_Id`" +
                             ", `has_Spell_Set_Id`" +
                             ", `spell_Category`" +
                             ", `power_Level`" +
                             ", `start_Time`" + ", `duration`" +
                             ", `caster_Object_Id`" +
                             ", `degrade_Modifier`" + ", `degrade_Limit`" + ", `last_Time_Degraded`" +
                             ", `stat_Mod_Type`" + ", `stat_Mod_Key`" + ", `stat_Mod_Value`" +
                             ", `spell_Set_Id`)");

            var lineGenerator = new Func<int, string>(i => $"{id}, {input[i].EnchantmentCategory}, {input[i].SpellId}, {input[i].LayerId}, {input[i].HasSpellSetId}, {input[i].SpellCategory}, {input[i].PowerLevel}, {input[i].StartTime}, {input[i].Duration}, {input[i].CasterObjectId}, {input[i].DegradeModifier}, {input[i].DegradeLimit}, {input[i].LastTimeDegraded}, {input[i].StatModType}, {input[i].StatModKey}, {input[i].StatModValue}, {input[i].SpellSetId})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
