DELETE FROM `weenie` WHERE `class_Id` = 72540;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (72540, 'ace72540-firstlieutenant', 10, '2021-11-08 06:01:47') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (72540,   1,         16) /* ItemType - Creature */
     , (72540,   2,         77) /* CreatureType - Ghost */
     , (72540,   3,         39) /* PaletteTemplate - Black */
     , (72540,   6,         -1) /* ItemsCapacity */
     , (72540,   7,         -1) /* ContainersCapacity */
     , (72540,  16,          1) /* ItemUseable - No */
     , (72540,  25,        265) /* Level */
     , (72540,  68,          5) /* TargetingTactic - Random, LastDamager */
     , (72540,  81,          9) /* MaxGeneratedObjects */
     , (72540,  82,          0) /* InitGeneratedObjects */
     , (72540,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (72540, 103,          2) /* GeneratorDestructionType - Destroy */
     , (72540, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (72540, 145,          2) /* GeneratorEndDestructionType - Destroy */
     , (72540, 146,    5500000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (72540,   1, True ) /* Stuck */
     , (72540,   6, True ) /* AiUsesMana */
     , (72540,  11, False) /* IgnoreCollisions */
     , (72540,  12, True ) /* ReportCollisions */
     , (72540,  13, False) /* Ethereal */
     , (72540,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (72540,   1,       5) /* HeartbeatInterval */
     , (72540,   2,       0) /* HeartbeatTimestamp */
     , (72540,   3,       2) /* HealthRate */
     , (72540,   4,       5) /* StaminaRate */
     , (72540,   5,       1) /* ManaRate */
     , (72540,  13,    0.95) /* ArmorModVsSlash */
     , (72540,  14,       1) /* ArmorModVsPierce */
     , (72540,  15,     0.9) /* ArmorModVsBludgeon */
     , (72540,  16,       1) /* ArmorModVsCold */
     , (72540,  17,       1) /* ArmorModVsFire */
     , (72540,  18,       1) /* ArmorModVsAcid */
     , (72540,  19,       1) /* ArmorModVsElectric */
     , (72540,  31,      20) /* VisualAwarenessRange */
     , (72540,  34,       1) /* PowerupTime */
     , (72540,  36,       1) /* ChargeSpeed */
     , (72540,  64,     0.5) /* ResistSlash */
     , (72540,  65,     0.4) /* ResistPierce */
     , (72540,  66,     0.7) /* ResistBludgeon */
     , (72540,  67,     0.5) /* ResistFire */
     , (72540,  68,     0.4) /* ResistCold */
     , (72540,  69,     0.2) /* ResistAcid */
     , (72540,  70,     0.4) /* ResistElectric */
     , (72540,  80,       3) /* AiUseMagicDelay */
     , (72540, 104,      10) /* ObviousRadarRange */
     , (72540, 122,       2) /* AiAcquireHealth */
     , (72540, 125,       1) /* ResistHealthDrain */
     , (72540, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (72540,   1, 'First Lieutenant') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (72540,   1, 0x02001B8D) /* Setup */
     , (72540,   2, 0x090001FF) /* MotionTable */
     , (72540,   3, 0x2000001E) /* SoundTable */
     , (72540,   4, 0x30000000) /* CombatTable */
     , (72540,   7, 0x1000082D) /* ClothingBase */
     , (72540,   8, 0x060016C4) /* Icon */
     , (72540,  22, 0x34000028) /* PhysicsEffectTable */
     , (72540,  35,       1015) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (72540,   1, 350, 0, 0) /* Strength */
     , (72540,   2, 400, 0, 0) /* Endurance */
     , (72540,   3, 350, 0, 0) /* Quickness */
     , (72540,   4, 350, 0, 0) /* Coordination */
     , (72540,   5, 450, 0, 0) /* Focus */
     , (72540,   6, 430, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (72540,   1, 75000, 0, 0, 75200) /* MaxHealth */
     , (72540,   3, 56000, 0, 0, 56400) /* MaxStamina */
     , (72540,   5, 55520, 0, 0, 55950) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (72540,  6, 0, 2, 0, 537, 0, 0) /* MeleeDefense        Trained */
     , (72540,  7, 0, 2, 0, 610, 0, 0) /* MissileDefense      Trained */
     , (72540, 15, 0, 2, 0, 384, 0, 0) /* MagicDefense        Trained */
     , (72540, 31, 0, 2, 0, 280, 0, 0) /* CreatureEnchantment Trained */
     , (72540, 33, 0, 2, 0, 280, 0, 0) /* LifeMagic           Trained */
     , (72540, 34, 0, 2, 0, 280, 0, 0) /* WarMagic            Trained */
     , (72540, 41, 0, 2, 0, 427, 0, 0) /* TwoHandedCombat     Trained */
     , (72540, 43, 0, 2, 0, 280, 0, 0) /* VoidMagic           Trained */
     , (72540, 44, 0, 2, 0, 427, 0, 0) /* HeavyWeapons        Trained */
     , (72540, 45, 0, 2, 0, 427, 0, 0) /* LightWeapons        Trained */
     , (72540, 46, 0, 2, 0, 427, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (72540,  0,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (72540,  1,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (72540,  2,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (72540,  3,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (72540,  4,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (72540,  5,  4, 600, 0.75,  400,  200,  200,  200,  200,  200,  200,  200,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (72540,  6,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (72540,  7,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (72540,  8,  4, 600, 0.75,  400,  200,  200,  200,  200,  200,  200,  200,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (72540,  3043,   2.05)  /* Kiss of the Grave */
     , (72540,  3060,  2.053)  /* Poison Blood */
     , (72540,  4473,  2.111)  /* Incantation of Acid Vulnerability Other */
     , (72540,  5532,  2.375)  /* Incantation of Bloodstone Bolt */
     , (72540,  5535,    2.6)  /* Acidic Blood */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (72540,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  88 /* LocalSignal */, 0, 1, NULL, 'OpenDoor', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (72540,  9 /* Generation */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  88 /* LocalSignal */, 0, 1, NULL, 'CloseDoor', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (72540, 18 /* Scream */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (72540, 2, 46389,  1, 0, 0, False) /* Create Bloodletting Dagger (46389) for Wield */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46622,  1, 0, 0, False) /* Create First Lieutenant's Insignia. (46622) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46345,  1, 0, 0, False) /* Create O-Yoroi Leggings. (46345) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46551,  1, 0, 0, False) /* Create O-Yoroi Gauntlets. (46551) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46552,  1, 0, 0, False) /* Create O-Yoroi Helm. (46552) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46553,  1, 0, 0, False) /* Create O-Yoroi Sandals. (46553) for ContainTreasure */
     , (72540, 9, 46615,  1, 0, 0, False) /* Create O-Yoroi Coat. (46615) for ContainTreasure */
     , (72540, 9, 46615,  1, 0, 0, False) /* Create O-Yoroi Coat. (46615) for ContainTreasure */
     , (72540, 9, 46615,  1, 0, 0, False) /* Create O-Yoroi Coat. (46615) for ContainTreasure */
     , (72540, 9, 46615,  1, 0, 0, False) /* Create O-Yoroi Coat. (46615) for ContainTreasure */
     , (72540, 9, 46615,  1, 0, 0, False) /* Create O-Yoroi Coat. (46615) for ContainTreasure */
     , (72540, 9, 46615,  1, 0, 0, False) /* Create O-Yoroi Coat. (46615) for ContainTreasure */
     , (72540, 9, 46615,  1, 0, 0, False) /* Create O-Yoroi Coat. (46615) for ContainTreasure */
     , (72540, 9, 46615,  1, 0, 0, False) /* Create O-Yoroi Coat. (46615) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46641,  1, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi. (46641) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46642,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku. (46642) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46643,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets. (46643) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46644,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46644) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */
     , (72540, 9, 46645,  1, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers. (46645) for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (72540, -1, 72472, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510100, 6.1, -12.25, 0.005, 1, 0, 0, 0) /* Generate Trap (72472) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (72540, -1, 72472, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510100, 12.1, -6.1, 0.005, 0.707107, 0, 0, 0.707107) /* Generate Trap (72472) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (72540, -1, 72472, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510102, 17.8, -6.1, 0.005, 0.707107, 0, 0, -0.707107) /* Generate Trap (72472) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (72540, -1, 72472, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510102, 23.9, -12.25, 0.005, 1, 0, 0, 0) /* Generate Trap (72472) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (72540, -1, 72472, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510103, 23.9, -17.7, 0.005, 0, 0, 0, -1) /* Generate Trap (72472) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (72540, -1, 72472, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510103, 17.8, -23.9, 0.005, 0.707107, 0, 0, -0.707107) /* Generate Trap (72472) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (72540, -1, 72472, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510101, 12.1, -23.9, 0.005, 0.707107, 0, 0, 0.707107) /* Generate Trap (72472) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (72540, -1, 72472, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510101, 6.1, -17.7, 0.005, 0, 0, 0, -1) /* Generate Trap (72472) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */
     , (72540, -1, 72476, 180, 1, 1, 1, 4, 0, 0, 0, 0x66510102, 15, -15, 0.005, 1, 0, 0, 0) /* Generate Hoshino Tower Guards Gen (72476) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Specific */;
