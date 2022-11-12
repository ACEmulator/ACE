DELETE FROM `weenie` WHERE `class_Id` = 51992;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51992, 'ace51992-curatoroftorment', 10, '2021-11-01 00:00:00') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51992,   1,         16) /* ItemType - Creature */
     , (51992,   2,         19) /* CreatureType - Virindi */
     , (51992,   3,         11) /* PaletteTemplate - Maroon */
     , (51992,   6,         -1) /* ItemsCapacity */
     , (51992,   7,         -1) /* ContainersCapacity */
     , (51992,  16,          1) /* ItemUseable - No */
     , (51992,  25,        620) /* Level */
     , (51992,  68,          3) /* TargetingTactic - Random, Focused */
     , (51992,  81,          2) /* MaxGeneratedObjects */
     , (51992,  82,          0) /* InitGeneratedObjects */
     , (51992,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (51992, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (51992, 146,   62000000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51992,   1, True ) /* Stuck */
     , (51992,   6, False) /* AiUsesMana */
     , (51992,  11, False) /* IgnoreCollisions */
     , (51992,  12, True ) /* ReportCollisions */
     , (51992,  13, False) /* Ethereal */
     , (51992,  14, True ) /* GravityStatus */
     , (51992,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51992,   1,       5) /* HeartbeatInterval */
     , (51992,   2,       0) /* HeartbeatTimestamp */
     , (51992,   3,     0.6) /* HealthRate */
     , (51992,   4,     0.5) /* StaminaRate */
     , (51992,   5,       2) /* ManaRate */
     , (51992,  12,       0) /* Shade */
     , (51992,  13,     0.9) /* ArmorModVsSlash */
     , (51992,  14,       1) /* ArmorModVsPierce */
     , (51992,  15,       1) /* ArmorModVsBludgeon */
     , (51992,  16,       1) /* ArmorModVsCold */
     , (51992,  17,     0.9) /* ArmorModVsFire */
     , (51992,  18,       1) /* ArmorModVsAcid */
     , (51992,  19,       1) /* ArmorModVsElectric */
     , (51992,  31,      20) /* VisualAwarenessRange */
     , (51992,  34,       1) /* PowerupTime */
     , (51992,  36,       1) /* ChargeSpeed */
     , (51992,  39,     1.2) /* DefaultScale */
     , (51992,  41,      30) /* RegenerationInterval */
     , (51992,  43,       5) /* GeneratorRadius */
     , (51992,  64,     0.7) /* ResistSlash */
     , (51992,  65,     0.6) /* ResistPierce */
     , (51992,  66,     0.6) /* ResistBludgeon */
     , (51992,  67,     0.7) /* ResistFire */
     , (51992,  68,     0.4) /* ResistCold */
     , (51992,  69,     0.6) /* ResistAcid */
     , (51992,  70,     0.4) /* ResistElectric */
     , (51992,  80,       3) /* AiUseMagicDelay */
     , (51992, 104,      10) /* ObviousRadarRange */
     , (51992, 121,      10) /* GeneratorInitialDelay */
     , (51992, 122,       2) /* AiAcquireHealth */
     , (51992, 125,       1) /* ResistHealthDrain */
     , (51992, 165,       1) /* ArmorModVsNether */
     , (51992, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51992,   1, 'Curator of Torment') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51992,   1, 0x02001C07) /* Setup */
     , (51992,   2, 0x09000028) /* MotionTable */
     , (51992,   3, 0x20000012) /* SoundTable */
     , (51992,   4, 0x3000000D) /* CombatTable */
     , (51992,   6, 0x040009B2) /* PaletteBase */
     , (51992,   7, 0x100007AF) /* ClothingBase */
     , (51992,   8, 0x06001227) /* Icon */
     , (51992,  22, 0x34000029) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (51992,   1, 500, 0, 0) /* Strength */
     , (51992,   2, 500, 0, 0) /* Endurance */
     , (51992,   3, 500, 0, 0) /* Quickness */
     , (51992,   4, 500, 0, 0) /* Coordination */
     , (51992,   5, 600, 0, 0) /* Focus */
     , (51992,   6, 600, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (51992,   1, 299750, 0, 0, 300000) /* MaxHealth */
     , (51992,   3, 99400, 0, 0, 99900) /* MaxStamina */
     , (51992,   5, 99400, 0, 0, 100000) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (51992,  6, 0, 2, 0, 540, 0, 0) /* MeleeDefense        Trained */
     , (51992,  7, 0, 2, 0, 500, 0, 0) /* MissileDefense      Trained */
     , (51992, 15, 0, 2, 0, 380, 0, 0) /* MagicDefense        Trained */
     , (51992, 16, 0, 2, 0, 430, 0, 0) /* ManaConversion      Trained */
     , (51992, 31, 0, 2, 0, 430, 0, 0) /* CreatureEnchantment Trained */
     , (51992, 33, 0, 2, 0, 430, 0, 0) /* LifeMagic           Trained */
     , (51992, 34, 0, 2, 0, 430, 0, 0) /* WarMagic            Trained */
     , (51992, 41, 0, 2, 0, 460, 0, 0) /* TwoHandedCombat     Trained */
     , (51992, 43, 0, 2, 0, 430, 0, 0) /* VoidMagic           Trained */
     , (51992, 44, 0, 2, 0, 460, 0, 0) /* HeavyWeapons        Trained */
     , (51992, 45, 0, 2, 0, 460, 0, 0) /* LightWeapons        Trained */
     , (51992, 46, 0, 2, 0, 460, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (51992,  0,  1,  0,    0,  650,  275,  275,  275,  275,  275,  275,  275,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (51992,  1,  1,  0,    0,  650,  275,  275,  275,  275,  275,  275,  275,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (51992,  2,  1,  0,    0,  650,  275,  275,  275,  275,  275,  275,  275,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (51992,  3,  1,  0,    0,  650,  275,  275,  275,  275,  275,  275,  275,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (51992,  4,  1,  0,    0,  650,  275,  275,  275,  275,  275,  275,  275,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (51992,  5,  1, 220,  0.5,  650,  275,  275,  275,  275,  275,  275,  275,    0, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (51992,  6,  1,  0,    0,  650,  275,  275,  275,  275,  275,  275,  275,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (51992,  7,  1,  0,    0,  650,  275,  275,  275,  275,  275,  275,  275,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (51992,  8,  1, 220,  0.5,  650,  275,  275,  275,  275,  275,  275,  275,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51992,  5972,    2.1)  /* Galvanic Bomb */
     , (51992,  5969,  2.111)  /* Galvanic Strike */
     , (51992,  4312,  2.125)  /* Incantation of Imperil Other */
     , (51992,  4483,  2.143)  /* Incantation of Lightning Vulnerability Other */
     , (51992,  5967,  2.167)  /* Galvanic Arc */
     , (51992,  5968,    2.2)  /* Galvanic Blast */
     , (51992,  5971,   2.25)  /* Galvanic Volley */
     , (51992,  5970,  2.333)  /* Galvanic Streak */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (51992,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  16 /* WorldBroadcast */, 0, 1, NULL, 'Deep in the Catacombs of Torment, %s has struck down the Curator of Torment, forcing him again for a time into his prison in the abyss of Portalspace.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,  88 /* LocalSignal */, 0, 1, NULL, 'BossDead', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (51992,  9 /* Generation */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  17 /* LocalBroadcast */, 0, 1, NULL, 'A voice echos in your mind, "You''ve survived my guards and defeated my lieutenants... Very well. I will deal with you myself."', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 52008,  0, 0, 1, False) /* Create Shard of the Curator of Torment's Mask (52008) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51965,  0, 0, 1, False) /* Create  Rynthid Tentacle Dagger (51965) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51966,  0, 0, 1, False) /* Create Rynthid Tentacle Mace (51966) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51967,  0, 0, 1, False) /* Create Rynthid Tentacle Spear (51967) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51991,  0, 0, 1, False) /* Create Nether-attuned Rynthid Tentacle Wand (51991) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51968,  0, 0, 1, False) /* Create Rynthid Tentacle Greatspear (51968) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51989,  0, 0, 1, False) /* Create Rynthid Tentacle Wand (51989) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51988,  0, 0, 1, False) /* Create Rynthid Tentacle Bow (51988) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */
     , (51992, 9, 51990,  0, 0, 1, False) /* Create Life-attuned Rynthid Tentacle Wand (51990) for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (51992, -1, 51762, 60, 1, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Discorporate Rynthid of Rage (51762) (x1 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
