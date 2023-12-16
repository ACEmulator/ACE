DELETE FROM `weenie` WHERE `class_Id` = 490064;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490064, 'ace490064-RoyalOlthoiRanger', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490064,   1,         16) /* ItemType - Creature */
     , (490064,   2,         92) /* CreatureType - ParadoxOlthoi */
     , (490064,   3,         39) /* PaletteTemplate - Black */
     , (490064,   6,         -1) /* ItemsCapacity */
     , (490064,   7,         -1) /* ContainersCapacity */
     , (490064,  16,          1) /* ItemUseable - No */
     , (490064,  25,        265) /* Level */
     , (490064,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (490064,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (490064, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (490064, 146,    2500000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490064,   1, True ) /* Stuck */
     , (490064,  65, True ) /* IgnoreMagicResist */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490064,   1,       5) /* HeartbeatInterval */
     , (490064,   2,       0) /* HeartbeatTimestamp */
     , (490064,   3,     100) /* HealthRate */
     , (490064,   4,       4) /* StaminaRate */
     , (490064,   5,       2) /* ManaRate */
     , (490064,  13,    2.25) /* ArmorModVsSlash */
     , (490064,  14,    1.75) /* ArmorModVsPierce */
     , (490064,  15,    1.12) /* ArmorModVsBludgeon */
     , (490064,  16,     3.5) /* ArmorModVsCold */
     , (490064,  17,     3.5) /* ArmorModVsFire */
     , (490064,  18,       4) /* ArmorModVsAcid */
     , (490064,  19,     3.5) /* ArmorModVsElectric */
     , (490064,  31,      50) /* VisualAwarenessRange */
     , (490064,  34,       1) /* PowerupTime */
	 , (490064,  55,      110) /* HomeRadius */
     , (490064,  36,       1) /* ChargeSpeed */
     , (490064,  64,       1) /* ResistSlash */
     , (490064,  65,    0.95) /* ResistPierce */
     , (490064,  66,       1) /* ResistBludgeon */
     , (490064,  67,    0.75) /* ResistFire */
     , (490064,  68,     0.5) /* ResistCold */
     , (490064,  69,     0.5) /* ResistAcid */
     , (490064,  70,    0.75) /* ResistElectric */
     , (490064,  77,       1) /* PhysicsScriptIntensity */
     , (490064, 104,      40) /* ObviousRadarRange */
     , (490064, 125,       1) /* ResistHealthDrain */
     , (490064, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490064,   1, 'Royal Olthoi Ranger') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490064,   1, 0x0200170A) /* Setup */
     , (490064,   2, 0x09000135) /* MotionTable */
     , (490064,   3, 0x200000A1) /* SoundTable */
     , (490064,   4, 0x30000039) /* CombatTable */
     , (490064,   6, 0x04001606) /* PaletteBase */
     , (490064,   7, 0x100004C7) /* ClothingBase */
     , (490064,   8, 0x06002D3E) /* Icon */
     , (490064,  19, 0x00000056) /* ActivationAnimation */
     , (490064,  22, 0x340000A8) /* PhysicsEffectTable */
     , (490064,  30,         86) /* PhysicsScript - BreatheAcid */
     , (490064,  35,         2111) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490064,   1, 400, 0, 0) /* Strength */
     , (490064,   2, 500, 0, 0) /* Endurance */
     , (490064,   3, 500, 0, 0) /* Quickness */
     , (490064,   4, 350, 0, 0) /* Coordination */
     , (490064,   5, 490, 0, 0) /* Focus */
     , (490064,   6, 490, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490064,   1, 15000, 0, 0, 15250) /* MaxHealth */
     , (490064,   3,  5000, 0, 0, 5500) /* MaxStamina */
     , (490064,   5,  5000, 0, 0, 5490) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490064,  6, 0, 2, 0, 460, 0, 0) /* MeleeDefense        Trained */
     , (490064,  7, 0, 2, 0, 420, 0, 0) /* MissileDefense      Trained */
     , (490064, 15, 0, 2, 0, 400, 0, 0) /* MagicDefense        Trained */
     , (490064, 41, 0, 2, 0, 519, 0, 0) /* TwoHandedCombat     Trained */
     , (490064, 44, 0, 2, 0, 919, 0, 0) /* HeavyWeapons        Trained */
     , (490064, 45, 0, 2, 0, 519, 0, 0) /* LightWeapons        Trained */
     , (490064, 46, 0, 2, 0, 519, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490064,  0,  2, 450, 0.75,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 1,  0.1,    0,    0,  0.1,    0,    0,  0.1,    0,    0,  0.1,    0,    0) /* Head */
     , (490064, 16,  4,  0,    0,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 2, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45) /* Torso */
     , (490064, 18,  1, 350,  0.5,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 2,    0,  0.2,  0.1,    0,  0.2,  0.1,    0,  0.2,  0.1,    0,  0.2,  0.1) /* Arm */
     , (490064, 19,  1,  0,    0,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 3,    0,  0.2, 0.45,    0,  0.2, 0.45,    0,  0.2, 0.45,    0,  0.2, 0.45) /* Leg */
     , (490064, 20,  1, 500, 0.75,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 2, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0) /* Claw */
     , (490064, 22, 32, 475,  0.5,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0) /* Breath */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490064,  5 /* HeartBeat */,   0.25, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490064,  5 /* HeartBeat */,    0.7, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  21 /* InqQuest */, 0, 1, NULL, 'CrossedRoad@16', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490064, 12 /* QuestSuccess */,      1, NULL, NULL, NULL, 'CrossedRoad@16', NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   6 /* Move */, 10, 0.5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 40, 0, 1, 0, 0, 0)
     , (@parent_id,  1,  31 /* EraseQuest */, 0, 1, NULL, 'CrossedRoad', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490064, 13 /* QuestFailure */,      1, NULL, NULL, NULL, 'CrossedRoad@16', NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   6 /* Move */, 10, 0.5, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 1)
     , (@parent_id,  1,  22 /* StampQuest */, 0, 1, NULL, 'CrossedRoad', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (490064, 9,     0,  1, 0, 0.05, False) /* Create nothing for ContainTreasure */
     , (490064, 9, 36376,  1, 0, 0.2, False) /* Create Small Olthoi Venom Sac (36376) for ContainTreasure */
     , (490064, 9,     0,  1, 0, 0.8, False) /* Create nothing for ContainTreasure */;
