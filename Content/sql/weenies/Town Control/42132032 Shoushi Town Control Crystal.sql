DELETE FROM `weenie` WHERE `class_Id` = 42132032;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (42132032, 'shoushiconflictcrystal', 10, '2022-01-01 05:23:54') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42132032,   1,         16) /* ItemType - Creature */
     , (42132032,   2,         47) /* CreatureType - Crystal */
     , (42132032,   6,         -1) /* ItemsCapacity */
     , (42132032,   7,         -1) /* ContainersCapacity */
     , (42132032,  16,          1) /* ItemUseable - No */
     , (42132032,  25,       1000) /* Level */
     , (42132032,  27,          0) /* ArmorType - None */
     , (42132032,  40,          2) /* CombatMode - Melee */
     , (42132032,  67,          1) /* Tolerance - NoAttack */
     , (42132032,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (42132032, 101,          1) /* AiAllowedCombatStyle - Unarmed */
     , (42132032, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42132032,   1, True ) /* Stuck */
     , (42132032,   6, True ) /* AiUsesMana */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (42132032,   1,       5) /* HeartbeatInterval */
     , (42132032,   3,      10) /* HealthRate */
     , (42132032,   4,       5) /* StaminaRate */
     , (42132032,   5,       2) /* ManaRate */
     , (42132032,  12,       0) /* Shade */
     , (42132032,  13,       1) /* ArmorModVsSlash */
     , (42132032,  14,       1) /* ArmorModVsPierce */
     , (42132032,  15,       1) /* ArmorModVsBludgeon */
     , (42132032,  16,       1) /* ArmorModVsCold */
     , (42132032,  17,       1) /* ArmorModVsFire */
     , (42132032,  18,       1) /* ArmorModVsAcid */
     , (42132032,  19,       1) /* ArmorModVsElectric */
     , (42132032,  31,      12) /* VisualAwarenessRange */
     , (42132032,  34,       1) /* PowerupTime */
     , (42132032,  36,       1) /* ChargeSpeed */
     , (42132032,  39,       3) /* DefaultScale */
     , (42132032,  54,       3) /* UseRadius */
     , (42132032,  64,     0.3) /* ResistSlash */
     , (42132032,  65,     0.3) /* ResistPierce */
     , (42132032,  66,     0.3) /* ResistBludgeon */
     , (42132032,  67,     0.3) /* ResistFire */
     , (42132032,  68,     0.3) /* ResistCold */
     , (42132032,  69,     0.3) /* ResistAcid */
     , (42132032,  70,     0.3) /* ResistElectric */
     , (42132032,  71,       1) /* ResistHealthBoost */
     , (42132032,  72,       0) /* ResistStaminaDrain */
     , (42132032,  73,       1) /* ResistStaminaBoost */
     , (42132032,  74,       0) /* ResistManaDrain */
     , (42132032,  75,       1) /* ResistManaBoost */
     , (42132032,  80,       2) /* AiUseMagicDelay */
     , (42132032, 104,      10) /* ObviousRadarRange */
     , (42132032, 125,       0) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42132032,   1, 'Town Control Crystal') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42132032,   1, 0x02000AAF) /* Setup */
     , (42132032,   2, 0x09000160) /* MotionTable */
     , (42132032,   8, 0x0600218C) /* Icon */
     , (42132032,  22, 0x3400009D) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (42132032,  0,  4, 50, 0.75,  350,  175,  175,  175,  175,  175,  175,  175,    0, 1,  0.5,  0.2,    0,  0.5,  0.2,    0,    0,    0,    0,    0,    0,    0) /* Head */
     , (42132032, 10,  4,  0,    0,  350,  175,  175,  175,  175,  175,  175,  175,    0, 2,  0.2,  0.4,  0.5,  0.2,  0.4,  0.5,    0,    0,    0,    0,    0,    0) /* FrontLeg */
     , (42132032, 12,  4, 50, 0.75,  350,  175,  175,  175,  175,  175,  175,  175,    0, 3,    0,    0, 0.25,    0,    0, 0.25,    0,    0,    0,    0,    0,    0) /* FrontFoot */
     , (42132032, 13,  4,  0,    0,  350,  175,  175,  175,  175,  175,  175,  175,    0, 2,    0,    0,    0,    0,    0,    0,  0.3,  0.4,  0.5,  0.3,  0.4,  0.5) /* RearLeg */
     , (42132032, 15,  4, 50, 0.75,  350,  175,  175,  175,  175,  175,  175,  175,    0, 3,    0,    0,    0,    0,    0,    0,    0,    0, 0.25,    0,    0, 0.25) /* RearFoot */
     , (42132032, 16,  4,  0,    0,  350,  175,  175,  175,  175,  175,  175,  175,    0, 2,  0.3,  0.4, 0.25,  0.3,  0.4, 0.25,  0.6,  0.5, 0.25,  0.6,  0.5, 0.25) /* Torso */
     , (42132032, 17,  4, 50, 0.75,  350,  175,  175,  175,  175,  175,  175,  175,    0, 2,    0,    0,    0,    0,    0,    0,  0.1,  0.1,    0,  0.1,  0.1,    0) /* Tail */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (42132032,   1, 500, 0, 0) /* Strength */
     , (42132032,   2, 500, 0, 0) /* Endurance */
     , (42132032,   3, 500, 0, 0) /* Quickness */
     , (42132032,   4, 500, 0, 0) /* Coordination */
     , (42132032,   5, 500, 0, 0) /* Focus */
     , (42132032,   6, 500, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (42132032,   1, 499750, 0, 0, 499750) /* MaxHealth */
     , (42132032,   3,     0, 0, 0,    1) /* MaxStamina */
     , (42132032,   5,     0, 0, 0,  500) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (42132032,  6, 0, 3, 0, 150, 0, 0) /* MeleeDefense        Specialized */
     , (42132032,  7, 0, 3, 0, 250, 0, 0) /* MissileDefense      Specialized */
     , (42132032, 15, 0, 3, 0, 300, 0, 0) /* MagicDefense        Specialized */
     , (42132032, 20, 0, 3, 0, 100, 0, 0) /* Deception           Specialized */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42132032, 9 /* Generation */, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id, 0, 5 /* Motion */, 0, 1, 0x4000000B /* On */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42132032, 3 /* Death */, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id, 0, 77 /* DeleteSelf */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

