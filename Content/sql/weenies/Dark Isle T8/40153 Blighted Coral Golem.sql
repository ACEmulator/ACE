DELETE FROM `weenie` WHERE `class_Id` = 40153;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40153, 'ace40153-blightedcoralgolem', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40153,   1,         16) /* ItemType - Creature */
     , (40153,   2,         13) /* CreatureType - Golem */
     , (40153,   3,         39) /* PaletteTemplate - Black */
     , (40153,   6,         -1) /* ItemsCapacity */
     , (40153,   7,         -1) /* ContainersCapacity */
     , (40153,  16,          1) /* ItemUseable - No */
     , (40153,  25,        200) /* Level */
     , (40153,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (40153, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40153, 146,     220000) /* XpOverride */
     , (40153, 307,          2) /* DamageRating */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40153,   1, True ) /* Stuck */
     , (40153,   6, True ) /* AiUsesMana */
     , (40153,  11, False) /* IgnoreCollisions */
     , (40153,  12, True ) /* ReportCollisions */
     , (40153,  13, False) /* Ethereal */
     , (40153,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40153,   1,       5) /* HeartbeatInterval */
     , (40153,   2,       0) /* HeartbeatTimestamp */
     , (40153,   3,     0.9) /* HealthRate */
     , (40153,   4,     0.5) /* StaminaRate */
     , (40153,   5,       2) /* ManaRate */
     , (40153,   6,     0.1) /* HealthUponResurrection */
     , (40153,   7,    0.25) /* StaminaUponResurrection */
     , (40153,   8,     0.3) /* ManaUponResurrection */
     , (40153,  12,     0.5) /* Shade */
     , (40153,  13,    0.79) /* ArmorModVsSlash */
     , (40153,  14,     0.9) /* ArmorModVsPierce */
     , (40153,  15,       1) /* ArmorModVsBludgeon */
     , (40153,  16,    0.84) /* ArmorModVsCold */
     , (40153,  17,    0.84) /* ArmorModVsFire */
     , (40153,  18,    0.84) /* ArmorModVsAcid */
     , (40153,  19,    0.84) /* ArmorModVsElectric */
     , (40153,  31,      13) /* VisualAwarenessRange */
     , (40153,  34,     3.3) /* PowerupTime */
     , (40153,  39,     1.2) /* DefaultScale */
     , (40153,  64,    0.33) /* ResistSlash */
     , (40153,  65,    0.67) /* ResistPierce */
     , (40153,  66,       1) /* ResistBludgeon */
     , (40153,  67,     0.5) /* ResistFire */
     , (40153,  68,     0.5) /* ResistCold */
     , (40153,  69,     0.5) /* ResistAcid */
     , (40153,  70,     0.5) /* ResistElectric */
     , (40153,  71,       1) /* ResistHealthBoost */
     , (40153,  72,       1) /* ResistStaminaDrain */
     , (40153,  73,       1) /* ResistStaminaBoost */
     , (40153,  74,       1) /* ResistManaDrain */
     , (40153,  75,       1) /* ResistManaBoost */
     , (40153,  80,       3) /* AiUseMagicDelay */
     , (40153, 104,      10) /* ObviousRadarRange */
     , (40153, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40153,   1, 'Blighted Coral Golem') /* Name */
     , (40153,  45, 'BlackMarketBlightedCoralGolemKilltask') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40153,   1, 0x020007CA) /* Setup */
     , (40153,   2, 0x09000081) /* MotionTable */
     , (40153,   3, 0x20000015) /* SoundTable */
     , (40153,   4, 0x30000008) /* CombatTable */
     , (40153,   6, 0x04000F47) /* PaletteBase */
     , (40153,   7, 0x10000229) /* ClothingBase */
     , (40153,   8, 0x06001224) /* Icon */
     , (40153,  22, 0x3400005B) /* PhysicsEffectTable */
     , (40153,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40153,   1, 300, 0, 0) /* Strength */
     , (40153,   2, 310, 0, 0) /* Endurance */
     , (40153,   3, 200, 0, 0) /* Quickness */
     , (40153,   4, 210, 0, 0) /* Coordination */
     , (40153,   5, 200, 0, 0) /* Focus */
     , (40153,   6, 200, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40153,   1,  1600, 0, 0, 1755) /* MaxHealth */
     , (40153,   3,  1300, 0, 0, 1610) /* MaxStamina */
     , (40153,   5,  1100, 0, 0, 1300) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40153,  6, 0, 3, 0, 300, 0, 0) /* MeleeDefense        Specialized */
     , (40153,  7, 0, 3, 0, 400, 0, 0) /* MissileDefense      Specialized */
     , (40153, 15, 0, 3, 0, 300, 0, 0) /* MagicDefense        Specialized */
     , (40153, 20, 0, 2, 0, 100, 0, 0) /* Deception           Trained */
     , (40153, 24, 0, 2, 0, 200, 0, 0) /* Run                 Trained */
     , (40153, 31, 0, 3, 0, 140, 0, 0) /* CreatureEnchantment Specialized */
     , (40153, 33, 0, 3, 0, 450, 0, 0) /* LifeMagic           Specialized */
     , (40153, 34, 0, 3, 0, 450, 0, 0) /* WarMagic            Specialized */
     , (40153, 45, 0, 3, 0, 400, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40153,  0,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (40153,  1,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (40153,  2,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (40153,  3,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (40153,  4,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (40153,  5, 12, 120, 0.75,  350,  277,  315,  350,  294,  294,  294,  294,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (40153,  6,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (40153,  7,  4,  0,    0,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (40153,  8, 20, 150, 0.75,  350,  277,  315,  350,  294,  294,  294,  294,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40153,  2074,   2.15)  /* Gossamer Flesh */
     , (40153,  2136,   2.18)  /* Icy Torment */
     , (40153,  2138,   2.15)  /* Blizzard */
     , (40153,  1839,   2.15)  /* Blistering Creeper */
     , (40153,  1843,   2.15)  /* Foon-Ki's Glacial Floe */
     , (40153,  2137,   2.03)  /* Sudden Frost */
     , (40153,  2135,   2.15)  /* Winter's Embrace */
     , (40153,  2123,   2.02)  /* Celdiseth's Searing */
     , (40153,  2122,   2.15)  /* Disintegration */
     , (40153,  2120,   2.02)  /* Dissolving Vortex */
     , (40153,  2168,   2.15)  /* Gelidite's Gift */
     , (40153,   526,   2.02)  /* Acid Vulnerability Other VI */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (40153,  5 /* HeartBeat */,  0.075, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (40153,  5 /* HeartBeat */,      1, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,   5 /* Motion */, 0, 1, 0x41000014 /* Sleeping */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40153, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40153, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40153, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40153, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40153, 9, 42348,  0, 0, 0.05, False) /* Create Black Coral Heart (42348) for ContainTreasure */
     , (40153, 9,     0,  0, 0, 0.95, False) /* Create nothing for ContainTreasure */;
