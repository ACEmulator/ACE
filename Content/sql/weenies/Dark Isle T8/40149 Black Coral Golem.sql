DELETE FROM `weenie` WHERE `class_Id` = 40149;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40149, 'ace40149-blackcoralgolem', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40149,   1,         16) /* ItemType - Creature */
     , (40149,   2,         13) /* CreatureType - Golem */
     , (40149,   3,         39) /* PaletteTemplate - Black */
     , (40149,   6,         -1) /* ItemsCapacity */
     , (40149,   7,         -1) /* ContainersCapacity */
     , (40149,  16,          1) /* ItemUseable - No */
     , (40149,  25,        185) /* Level */
     , (40149,  81,          1) /* MaxGeneratedObjects */
     , (40149,  82,          0) /* InitGeneratedObjects */
     , (40149,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (40149, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40149, 146,     200000) /* XpOverride */
     , (40149, 307,          2) /* DamageRating */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40149,   1, True ) /* Stuck */
     , (40149,   6, True ) /* AiUsesMana */
     , (40149,  11, False) /* IgnoreCollisions */
     , (40149,  12, True ) /* ReportCollisions */
     , (40149,  13, False) /* Ethereal */
     , (40149,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40149,   1,       5) /* HeartbeatInterval */
     , (40149,   2,       0) /* HeartbeatTimestamp */
     , (40149,   3,     0.9) /* HealthRate */
     , (40149,   4,     0.5) /* StaminaRate */
     , (40149,   5,       2) /* ManaRate */
     , (40149,   6,     0.1) /* HealthUponResurrection */
     , (40149,   7,    0.25) /* StaminaUponResurrection */
     , (40149,   8,     0.3) /* ManaUponResurrection */
     , (40149,  12,     0.5) /* Shade */
     , (40149,  13,    0.79) /* ArmorModVsSlash */
     , (40149,  14,     0.9) /* ArmorModVsPierce */
     , (40149,  15,       1) /* ArmorModVsBludgeon */
     , (40149,  16,    0.84) /* ArmorModVsCold */
     , (40149,  17,    0.84) /* ArmorModVsFire */
     , (40149,  18,    0.84) /* ArmorModVsAcid */
     , (40149,  19,    0.84) /* ArmorModVsElectric */
     , (40149,  31,      13) /* VisualAwarenessRange */
     , (40149,  34,     3.3) /* PowerupTime */
     , (40149,  39,     1.1) /* DefaultScale */
     , (40149,  43,       4) /* GeneratorRadius */
     , (40149,  64,    0.33) /* ResistSlash */
     , (40149,  65,    0.67) /* ResistPierce */
     , (40149,  66,       1) /* ResistBludgeon */
     , (40149,  67,     0.5) /* ResistFire */
     , (40149,  68,     0.5) /* ResistCold */
     , (40149,  69,     0.5) /* ResistAcid */
     , (40149,  70,     0.5) /* ResistElectric */
     , (40149,  71,       1) /* ResistHealthBoost */
     , (40149,  72,       1) /* ResistStaminaDrain */
     , (40149,  73,       1) /* ResistStaminaBoost */
     , (40149,  74,       1) /* ResistManaDrain */
     , (40149,  75,       1) /* ResistManaBoost */
     , (40149,  80,       3) /* AiUseMagicDelay */
     , (40149, 104,      10) /* ObviousRadarRange */
     , (40149, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40149,   1, 'Black Coral Golem') /* Name */
     , (40149,  45, 'BlackMarketBlackCoralGolemKilltask') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40149,   1, 0x020007CA) /* Setup */
     , (40149,   2, 0x09000081) /* MotionTable */
     , (40149,   3, 0x20000015) /* SoundTable */
     , (40149,   4, 0x30000008) /* CombatTable */
     , (40149,   6, 0x04000F47) /* PaletteBase */
     , (40149,   7, 0x10000229) /* ClothingBase */
     , (40149,   8, 0x06001224) /* Icon */
     , (40149,  22, 0x3400005B) /* PhysicsEffectTable */
     , (40149,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40149,   1, 290, 0, 0) /* Strength */
     , (40149,   2, 300, 0, 0) /* Endurance */
     , (40149,   3, 190, 0, 0) /* Quickness */
     , (40149,   4, 200, 0, 0) /* Coordination */
     , (40149,   5, 190, 0, 0) /* Focus */
     , (40149,   6, 190, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40149,   1,  1350, 0, 0, 1500) /* MaxHealth */
     , (40149,   3,  1200, 0, 0, 1500) /* MaxStamina */
     , (40149,   5,  1000, 0, 0, 1190) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40149,  6, 0, 3, 0, 300, 0, 0) /* MeleeDefense        Specialized */
     , (40149,  7, 0, 3, 0, 400, 0, 0) /* MissileDefense      Specialized */
     , (40149, 15, 0, 3, 0, 300, 0, 0) /* MagicDefense        Specialized */
     , (40149, 20, 0, 2, 0, 100, 0, 0) /* Deception           Trained */
     , (40149, 24, 0, 2, 0, 200, 0, 0) /* Run                 Trained */
     , (40149, 31, 0, 3, 0, 140, 0, 0) /* CreatureEnchantment Specialized */
     , (40149, 33, 0, 3, 0, 450, 0, 0) /* LifeMagic           Specialized */
     , (40149, 34, 0, 3, 0, 450, 0, 0) /* WarMagic            Specialized */
     , (40149, 45, 0, 3, 0, 400, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40149,  0,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (40149,  1,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (40149,  2,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (40149,  3,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (40149,  4,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (40149,  5, 12, 120, 0.75,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (40149,  6,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (40149,  7,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (40149,  8, 20, 150, 0.75,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40149,  2074,   2.15)  /* Gossamer Flesh */
     , (40149,  2136,   2.18)  /* Icy Torment */
     , (40149,  2138,   2.15)  /* Blizzard */
     , (40149,  1839,   2.15)  /* Blistering Creeper */
     , (40149,  1843,   2.15)  /* Foon-Ki's Glacial Floe */
     , (40149,  2137,   2.03)  /* Sudden Frost */
     , (40149,  2135,   2.15)  /* Winter's Embrace */
     , (40149,  2123,   2.02)  /* Celdiseth's Searing */
     , (40149,  2122,   2.15)  /* Disintegration */
     , (40149,  2120,   2.02)  /* Dissolving Vortex */
     , (40149,  2168,   2.15)  /* Gelidite's Gift */
     , (40149,   526,   2.02)  /* Acid Vulnerability Other VI */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (40149,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (40149,  5 /* HeartBeat */,  0.075, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (40149,  5 /* HeartBeat */,      1, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,   5 /* Motion */, 0, 1, 0x41000014 /* Sleeping */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40149, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40149, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40149, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40149, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40149, 9, 42348,  0, 0, 0.05, False) /* Create Black Coral Heart (42348) for ContainTreasure */
     , (40149, 9,     0,  0, 0, 0.95, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (40149, -1, 40147, 0, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Black Coral Golem Viceroy (40147) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
