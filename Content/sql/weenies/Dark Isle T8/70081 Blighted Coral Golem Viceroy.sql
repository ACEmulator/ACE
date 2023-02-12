DELETE FROM `weenie` WHERE `class_Id` = 70081;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (70081, 'ace70081-blightedcoralgolemviceroy', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (70081,   1,         16) /* ItemType - Creature */
     , (70081,   2,         13) /* CreatureType - Golem */
     , (70081,   3,         39) /* PaletteTemplate - Black */
     , (70081,   6,         -1) /* ItemsCapacity */
     , (70081,   7,         -1) /* ContainersCapacity */
     , (70081,  16,          1) /* ItemUseable - No */
     , (70081,  25,        195) /* Level */
     , (70081,  81,          2) /* MaxGeneratedObjects */
     , (70081,  82,          2) /* InitGeneratedObjects */
     , (70081,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (70081, 103,          3) /* GeneratorDestructionType - Kill */
     , (70081, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (70081, 146,     750000) /* XpOverride */
     , (70081, 307,          2) /* DamageRating */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (70081,   1, True ) /* Stuck */
     , (70081,   6, True ) /* AiUsesMana */
     , (70081,  11, False) /* IgnoreCollisions */
     , (70081,  12, True ) /* ReportCollisions */
     , (70081,  13, False) /* Ethereal */
     , (70081,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (70081,   1,       5) /* HeartbeatInterval */
     , (70081,   2,       0) /* HeartbeatTimestamp */
     , (70081,   3,     0.9) /* HealthRate */
     , (70081,   4,     0.5) /* StaminaRate */
     , (70081,   5,       2) /* ManaRate */
     , (70081,   6,     0.1) /* HealthUponResurrection */
     , (70081,   7,    0.25) /* StaminaUponResurrection */
     , (70081,   8,     0.3) /* ManaUponResurrection */
     , (70081,  12,     0.5) /* Shade */
     , (70081,  13,    0.79) /* ArmorModVsSlash */
     , (70081,  14,     0.9) /* ArmorModVsPierce */
     , (70081,  15,       1) /* ArmorModVsBludgeon */
     , (70081,  16,    0.84) /* ArmorModVsCold */
     , (70081,  17,    0.84) /* ArmorModVsFire */
     , (70081,  18,    0.84) /* ArmorModVsAcid */
     , (70081,  19,    0.84) /* ArmorModVsElectric */
     , (70081,  31,      13) /* VisualAwarenessRange */
     , (70081,  34,     3.3) /* PowerupTime */
     , (70081,  39,     1.4) /* DefaultScale */
     , (70081,  41,       0) /* RegenerationInterval */
     , (70081,  43,       4) /* GeneratorRadius */
     , (70081,  64,    0.33) /* ResistSlash */
     , (70081,  65,    0.67) /* ResistPierce */
     , (70081,  66,       1) /* ResistBludgeon */
     , (70081,  67,     0.5) /* ResistFire */
     , (70081,  68,     0.5) /* ResistCold */
     , (70081,  69,     0.5) /* ResistAcid */
     , (70081,  70,     0.5) /* ResistElectric */
     , (70081,  71,       1) /* ResistHealthBoost */
     , (70081,  72,       1) /* ResistStaminaDrain */
     , (70081,  73,       1) /* ResistStaminaBoost */
     , (70081,  74,       1) /* ResistManaDrain */
     , (70081,  75,       1) /* ResistManaBoost */
     , (70081,  80,       3) /* AiUseMagicDelay */
     , (70081, 104,      10) /* ObviousRadarRange */
     , (70081, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (70081,   1, 'Blighted Coral Golem Viceroy') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (70081,   1, 0x02001032) /* Setup */
     , (70081,   2, 0x09000081) /* MotionTable */
     , (70081,   3, 0x20000015) /* SoundTable */
     , (70081,   4, 0x30000008) /* CombatTable */
     , (70081,   6, 0x04001799) /* PaletteBase */
     , (70081,   7, 0x10000566) /* ClothingBase */
     , (70081,   8, 0x06001224) /* Icon */
     , (70081,  22, 0x3400005B) /* PhysicsEffectTable */
     , (70081,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (70081,   1, 480, 0, 0) /* Strength */
     , (70081,   2, 700, 0, 0) /* Endurance */
     , (70081,   3, 480, 0, 0) /* Quickness */
     , (70081,   4, 480, 0, 0) /* Coordination */
     , (70081,   5, 380, 0, 0) /* Focus */
     , (70081,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (70081,   1,  2900, 0, 0, 3350) /* MaxHealth */
     , (70081,   3,  4300, 0, 0, 5000) /* MaxStamina */
     , (70081,   5,  4500, 0, 0, 4980) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (70081,  6, 0, 3, 0, 300, 0, 0) /* MeleeDefense        Specialized */
     , (70081,  7, 0, 3, 0, 400, 0, 0) /* MissileDefense      Specialized */
     , (70081, 15, 0, 3, 0, 300, 0, 0) /* MagicDefense        Specialized */
     , (70081, 20, 0, 2, 0, 100, 0, 0) /* Deception           Trained */
     , (70081, 24, 0, 2, 0, 200, 0, 0) /* Run                 Trained */
     , (70081, 31, 0, 3, 0, 140, 0, 0) /* CreatureEnchantment Specialized */
     , (70081, 33, 0, 3, 0, 450, 0, 0) /* LifeMagic           Specialized */
     , (70081, 34, 0, 3, 0, 450, 0, 0) /* WarMagic            Specialized */
     , (70081, 45, 0, 3, 0, 400, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (70081,  0,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (70081,  1,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (70081,  2,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (70081,  3,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (70081,  4,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (70081,  5, 12, 350, 0.75,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (70081,  6,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (70081,  7,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (70081,  8, 20, 350, 0.75,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (70081,  2074,   2.15)  /* Gossamer Flesh */
     , (70081,  2136,   2.18)  /* Icy Torment */
     , (70081,  2138,   2.15)  /* Blizzard */
     , (70081,  1839,   2.15)  /* Blistering Creeper */
     , (70081,  1843,   2.15)  /* Foon-Ki's Glacial Floe */
     , (70081,  2137,   2.03)  /* Sudden Frost */
     , (70081,  2135,   2.15)  /* Winter's Embrace */
     , (70081,  2123,   2.02)  /* Celdiseth's Searing */
     , (70081,  2122,   2.15)  /* Disintegration */
     , (70081,  2120,   2.02)  /* Dissolving Vortex */
     , (70081,  2168,   2.15)  /* Gelidite's Gift */
     , (70081,   526,   2.02)  /* Acid Vulnerability Other VI */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (70081,  5 /* HeartBeat */,  0.075, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (70081,  5 /* HeartBeat */,      1, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,   5 /* Motion */, 0, 1, 0x41000014 /* Sleeping */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (70081, 9, 44470,  1, 0, 0, False) /* Create Corrupted Essence (44470) for ContainTreasure */
     , (70081, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (70081, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (70081, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (70081, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (70081, -1, 40153, 4, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Blighted Coral Golem (40153) (x2 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
