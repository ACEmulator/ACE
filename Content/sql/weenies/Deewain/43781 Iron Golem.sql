DELETE FROM `weenie` WHERE `class_Id` = 43781;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (43781, 'ace43781-irongolem', 10, '2021-11-01 00:00:00') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (43781,   1,         16) /* ItemType - Creature */
     , (43781,   2,         13) /* CreatureType - Golem */
     , (43781,   6,         -1) /* ItemsCapacity */
     , (43781,   7,         -1) /* ContainersCapacity */
     , (43781,  16,         32) /* ItemUseable - Remote */
     , (43781,  25,        350) /* Level */
     , (43781,  40,          2) /* CombatMode - Melee */
     , (43781,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (43781,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (43781, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (43781, 146,       7000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (43781,   1, True ) /* Stuck */
     , (43781,   6, True ) /* AiUsesMana */
     , (43781,  11, False) /* IgnoreCollisions */
     , (43781,  12, True ) /* ReportCollisions */
     , (43781,  13, False) /* Ethereal */
     , (43781,  14, True ) /* GravityStatus */
     , (43781,  19, True ) /* Attackable */
     , (43781,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (43781,   1,       5) /* HeartbeatInterval */
     , (43781,   2,       0) /* HeartbeatTimestamp */
     , (43781,   3,     0.5) /* HealthRate */
     , (43781,   4,     0.5) /* StaminaRate */
     , (43781,   5,       2) /* ManaRate */
     , (43781,   6,     0.1) /* HealthUponResurrection */
     , (43781,   7,    0.25) /* StaminaUponResurrection */
     , (43781,   8,     0.3) /* ManaUponResurrection */
     , (43781,  12,     0.5) /* Shade */
     , (43781,  13,    0.44) /* ArmorModVsSlash */
     , (43781,  14,    0.58) /* ArmorModVsPierce */
     , (43781,  15,    0.86) /* ArmorModVsBludgeon */
     , (43781,  16,    0.33) /* ArmorModVsCold */
     , (43781,  17,    0.33) /* ArmorModVsFire */
     , (43781,  18,    0.89) /* ArmorModVsAcid */
     , (43781,  19,       1) /* ArmorModVsElectric */
     , (43781,  31,      25) /* VisualAwarenessRange */
     , (43781,  34,     2.5) /* PowerupTime */
     , (43781,  39,     2.1) /* DefaultScale */
     , (43781,  64,    0.33) /* ResistSlash */
     , (43781,  65,     0.5) /* ResistPierce */
     , (43781,  66,    0.83) /* ResistBludgeon */
     , (43781,  67,     0.2) /* ResistFire */
     , (43781,  68,     0.2) /* ResistCold */
     , (43781,  69,    0.89) /* ResistAcid */
     , (43781,  70,    0.65) /* ResistElectric */
     , (43781,  71,       1) /* ResistHealthBoost */
     , (43781,  72,       1) /* ResistStaminaDrain */
     , (43781,  73,       1) /* ResistStaminaBoost */
     , (43781,  74,       1) /* ResistManaDrain */
     , (43781,  75,       1) /* ResistManaBoost */
     , (43781,  80,       3) /* AiUseMagicDelay */
     , (43781, 104,      10) /* ObviousRadarRange */
     , (43781, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (43781,   1, 'Iron Golem') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (43781,   1, 0x020007CA) /* Setup */
     , (43781,   2, 0x09000081) /* MotionTable */
     , (43781,   3, 0x20000015) /* SoundTable */
     , (43781,   4, 0x30000008) /* CombatTable */
     , (43781,   8, 0x06001224) /* Icon */
     , (43781,  22, 0x3400005B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
VALUES (43781,  16, 0x77E0305F) /* ActivationTarget */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (43781,   1, 500, 0, 0) /* Strength */
     , (43781,   2, 500, 0, 0) /* Endurance */
     , (43781,   3, 500, 0, 0) /* Quickness */
     , (43781,   4, 500, 0, 0) /* Coordination */
     , (43781,   5, 500, 0, 0) /* Focus */
     , (43781,   6, 500, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (43781,   1, 200000, 0, 0, 200250) /* MaxHealth */
     , (43781,   3, 15000, 0, 0, 15500) /* MaxStamina */
     , (43781,   5, 50000, 0, 0, 50500) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (43781,  6, 0, 3, 0, 383, 0, 0) /* MeleeDefense        Specialized */
     , (43781,  7, 0, 3, 0, 270, 0, 0) /* MissileDefense      Specialized */
     , (43781, 15, 0, 3, 0, 257, 0, 0) /* MagicDefense        Specialized */
     , (43781, 20, 0, 2, 0, 100, 0, 0) /* Deception           Trained */
     , (43781, 24, 0, 2, 0, 200, 0, 0) /* Run                 Trained */
     , (43781, 31, 0, 3, 0, 298, 0, 0) /* CreatureEnchantment Specialized */
     , (43781, 33, 0, 3, 0, 298, 0, 0) /* LifeMagic           Specialized */
     , (43781, 34, 0, 3, 0, 298, 0, 0) /* WarMagic            Specialized */
     , (43781, 45, 0, 3, 0, 382, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (43781,  0,  4,  0,    0,  400,  300,  350,  350,  350, 20000,  350,  350,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (43781,  1,  4,  0,    0,  400,  300,  350,  350,  350, 20000,  350,  350,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (43781,  2,  4,  0,    0,  400,  300,  350,  350,  350, 20000,  350,  350,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (43781,  3,  4,  0,    0,  400,  300,  350,  350,  350, 20000,  350,  350,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (43781,  4,  4,  0,    0,  400,  300,  350,  350,  350, 20000,  350,  350,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (43781,  5,  4, 160,  0.6,  400,  300,  350,  350,  350, 20000,  350,  350,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (43781,  6,  4,  0,    0,  400,  350,  350,  350,  350, 20000,  350,  350,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (43781,  7,  4,  0,    0,  400,  300,  350,  350,  350, 20000,  350,  350,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (43781,  8,  4, 170,  0.6,  400,  300,  350,  350,  350, 20000,  350,  350,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (43781,  2039,   2.05)  /* Sparking Fury */
     , (43781,  2708,   2.03)  /* Stasis Field */
     , (43781,  2992,   2.05)  /* Depletion */
     , (43781,  3079,   2.05)  /* Thin Skin */
     , (43781,  3462,   2.03)  /* Canker Flesh */
     , (43781,  3916,   2.03)  /* Flayed Flesh */
     , (43781,  3946,   2.05)  /* Acid Wave */
     , (43781,  3947,   2.05)  /* Blade Wave */
     , (43781,  3948,   2.05)  /* Flame Wave */
     , (43781,  3949,   2.05)  /* Force Wave */
     , (43781,  3951,   2.05)  /* Lightning Wave */
     , (43781,  3952,   2.05)  /* Shock Waves */
     , (43781,  3969,   2.05)  /* Acid Bomb */
     , (43781,  3970,   2.05)  /* Blade Bomb */
     , (43781,  3975,   2.05)  /* Shock Bomb */
     , (43781,  4433,   2.06)  /* Incantation of Acid Stream */
     , (43781,  4443,   2.06)  /* Incantation of Force Bolt */
     , (43781,  4447,   2.05)  /* Incantation of Frost Bolt */
     , (43781,  4457,   2.05)  /* Incantation of Whirling Blade */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (43781,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  15 /* Activate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */
     , (43781, 9, 43792,  0, 0, 1, False) /* Create Energy Infused Rock (43792) for ContainTreasure */