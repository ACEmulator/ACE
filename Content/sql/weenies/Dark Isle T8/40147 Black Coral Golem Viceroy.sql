DELETE FROM `weenie` WHERE `class_Id` = 40147;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40147, 'ace40147-blackcoralgolemviceroy', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40147,   1,         16) /* ItemType - Creature */
     , (40147,   2,         13) /* CreatureType - Golem */
     , (40147,   3,         39) /* PaletteTemplate - Black */
     , (40147,   6,         -1) /* ItemsCapacity */
     , (40147,   7,         -1) /* ContainersCapacity */
     , (40147,  16,          1) /* ItemUseable - No */
     , (40147,  25,        185) /* Level */
     , (40147,  81,          2) /* MaxGeneratedObjects */
     , (40147,  82,          2) /* InitGeneratedObjects */
     , (40147,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (40147, 103,          3) /* GeneratorDestructionType - Kill */
     , (40147, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40147, 146,     750000) /* XpOverride */
     , (40147, 307,          2) /* DamageRating */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40147,   1, True ) /* Stuck */
     , (40147,   6, True ) /* AiUsesMana */
     , (40147,  11, False) /* IgnoreCollisions */
     , (40147,  12, True ) /* ReportCollisions */
     , (40147,  13, False) /* Ethereal */
     , (40147,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40147,   1,       5) /* HeartbeatInterval */
     , (40147,   2,       0) /* HeartbeatTimestamp */
     , (40147,   3,     0.9) /* HealthRate */
     , (40147,   4,     0.5) /* StaminaRate */
     , (40147,   5,       2) /* ManaRate */
     , (40147,   6,     0.1) /* HealthUponResurrection */
     , (40147,   7,    0.25) /* StaminaUponResurrection */
     , (40147,   8,     0.3) /* ManaUponResurrection */
     , (40147,  12,     0.5) /* Shade */
     , (40147,  13,    0.79) /* ArmorModVsSlash */
     , (40147,  14,     0.9) /* ArmorModVsPierce */
     , (40147,  15,       1) /* ArmorModVsBludgeon */
     , (40147,  16,    0.84) /* ArmorModVsCold */
     , (40147,  17,    0.84) /* ArmorModVsFire */
     , (40147,  18,    0.84) /* ArmorModVsAcid */
     , (40147,  19,    0.84) /* ArmorModVsElectric */
     , (40147,  31,      13) /* VisualAwarenessRange */
     , (40147,  34,     3.3) /* PowerupTime */
     , (40147,  39,     1.1) /* DefaultScale */
     , (40147,  41,       0) /* RegenerationInterval */
     , (40147,  43,       4) /* GeneratorRadius */
     , (40147,  64,    0.33) /* ResistSlash */
     , (40147,  65,    0.67) /* ResistPierce */
     , (40147,  66,       1) /* ResistBludgeon */
     , (40147,  67,     0.5) /* ResistFire */
     , (40147,  68,     0.5) /* ResistCold */
     , (40147,  69,     0.5) /* ResistAcid */
     , (40147,  70,     0.5) /* ResistElectric */
     , (40147,  71,       1) /* ResistHealthBoost */
     , (40147,  72,       1) /* ResistStaminaDrain */
     , (40147,  73,       1) /* ResistStaminaBoost */
     , (40147,  74,       1) /* ResistManaDrain */
     , (40147,  75,       1) /* ResistManaBoost */
     , (40147,  80,       3) /* AiUseMagicDelay */
     , (40147, 104,      10) /* ObviousRadarRange */
     , (40147, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40147,   1, 'Black Coral Golem Viceroy') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40147,   1, 0x02001032) /* Setup */
     , (40147,   2, 0x09000081) /* MotionTable */
     , (40147,   3, 0x20000015) /* SoundTable */
     , (40147,   4, 0x30000008) /* CombatTable */
     , (40147,   6, 0x04001799) /* PaletteBase */
     , (40147,   7, 0x10000566) /* ClothingBase */
     , (40147,   8, 0x06001224) /* Icon */
     , (40147,  22, 0x3400005B) /* PhysicsEffectTable */
     , (40147,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40147,   1, 480, 0, 0) /* Strength */
     , (40147,   2, 700, 0, 0) /* Endurance */
     , (40147,   3, 480, 0, 0) /* Quickness */
     , (40147,   4, 480, 0, 0) /* Coordination */
     , (40147,   5, 380, 0, 0) /* Focus */
     , (40147,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40147,   1,  2900, 0, 0, 3250) /* MaxHealth */
     , (40147,   3,  4300, 0, 0, 5000) /* MaxStamina */
     , (40147,   5,  4500, 0, 0, 4980) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40147,  6, 0, 3, 0, 300, 0, 0) /* MeleeDefense        Specialized */
     , (40147,  7, 0, 3, 0, 400, 0, 0) /* MissileDefense      Specialized */
     , (40147, 15, 0, 3, 0, 300, 0, 0) /* MagicDefense        Specialized */
     , (40147, 20, 0, 2, 0, 100, 0, 0) /* Deception           Trained */
     , (40147, 24, 0, 2, 0, 200, 0, 0) /* Run                 Trained */
     , (40147, 31, 0, 3, 0, 140, 0, 0) /* CreatureEnchantment Specialized */
     , (40147, 33, 0, 3, 0, 450, 0, 0) /* LifeMagic           Specialized */
     , (40147, 34, 0, 3, 0, 450, 0, 0) /* WarMagic            Specialized */
     , (40147, 45, 0, 3, 0, 400, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40147,  0,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (40147,  1,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (40147,  2,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (40147,  3,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (40147,  4,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (40147,  5, 12, 250, 0.75,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (40147,  6,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (40147,  7,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (40147,  8, 20, 250, 0.75,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40147,  2074,   2.15)  /* Gossamer Flesh */
     , (40147,  2136,   2.18)  /* Icy Torment */
     , (40147,  2138,   2.15)  /* Blizzard */
     , (40147,  1839,   2.15)  /* Blistering Creeper */
     , (40147,  1843,   2.15)  /* Foon-Ki's Glacial Floe */
     , (40147,  2137,   2.03)  /* Sudden Frost */
     , (40147,  2135,   2.15)  /* Winter's Embrace */
     , (40147,  2123,   2.02)  /* Celdiseth's Searing */
     , (40147,  2122,   2.15)  /* Disintegration */
     , (40147,  2120,   2.02)  /* Dissolving Vortex */
     , (40147,  2168,   2.15)  /* Gelidite's Gift */
     , (40147,   526,   2.02)  /* Acid Vulnerability Other VI */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (40147,  5 /* HeartBeat */,  0.075, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (40147,  5 /* HeartBeat */,      1, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,   5 /* Motion */, 0, 1, 0x41000014 /* Sleeping */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40147, 9, 44469,  1, 0, 0, False) /* Create Lesser Corrupted Essence (44469) for ContainTreasure */
     , (40147, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40147, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40147, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40147, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (40147, -1, 40289, 4, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Black Coral Golem (40289) (x2 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
