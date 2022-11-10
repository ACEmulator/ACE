DELETE FROM `weenie` WHERE `class_Id` = 4200005;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200005, 'antiparazi', 10, '2005-02-09 10:00:00') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200005,   1,         16) /* ItemType - Creature */
     , (4200005,   2,         31) /* CreatureType - Human */
     , (4200005,   6,         -1) /* ItemsCapacity */
     , (4200005,   7,         -1) /* ContainersCapacity */
     , (4200005,   8,        120) /* Mass */
     , (4200005,  16,         32) /* ItemUseable - Remote */
     , (4200005,  25,        275) /* Level */
     , (4200005,  27,          0) /* ArmorType - None */
     , (4200005,  30,          1) /* AllegianceRank */
     , (4200005, 113,          1) /* Gender - Male */
     , (4200005,  67,          1) /* Tolerance - Never Attack */
     , (4200005,  93,    6292504) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity, ReportCollisionsAsEnvironment, EdgeSlide */
     , (4200005,  95,          5) /* RadarBlipColor - RED */
     , (4200005, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (4200005, 134,         16) /* PlayerKillerStatus - RubberGlue */
     , (4200005, 146,          0) /* XpOverride */
     , (4200005, 188,          3) /* HeritageGroup - Sho */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200005,   1, True ) /* Stuck */
     , (4200005,   8, True ) /* AllowGive */
     , (4200005,  11, False) /* IgnoreCollisions */
     , (4200005,  12, True ) /* ReportCollisions */
     , (4200005,  13, False) /* Ethereal */
     , (4200005,  19, False) /* Attackable */
     , (4200005,  42, True ) /* AllowEdgeSlide */
     , (4200005,  41, True ) /* ReportCollisionsAsEnvironment */
     , (4200005,  52, True ) /* AiImmobile */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200005,   1,       5) /* HeartbeatInterval */
     , (4200005,   2,       0) /* HeartbeatTimestamp */
     , (4200005,   3,     0.3) /* HealthRate */
     , (4200005,   4,       3) /* StaminaRate */
     , (4200005,   5,       1) /* ManaRate */
     , (4200005,   6,    0.75) /* HealthUponResurrection */
     , (4200005,   7,    0.75) /* StaminaUponResurrection */
     , (4200005,   8,    0.75) /* ManaUponResurrection */
     , (4200005,  13,     0.9) /* ArmorModVsSlash */
     , (4200005,  14,       1) /* ArmorModVsPierce */
     , (4200005,  15,     1.1) /* ArmorModVsBludgeon */
     , (4200005,  16,     0.4) /* ArmorModVsCold */
     , (4200005,  17,     0.4) /* ArmorModVsFire */
     , (4200005,  18,       1) /* ArmorModVsAcid */
     , (4200005,  19,     0.6) /* ArmorModVsElectric */
     , (4200005,  31,      50) /* VisualAwarenessRange */
     , (4200005,  36,     1.5) /* ChargeSpeed */
     , (4200005,  64,       1) /* ResistSlash */
     , (4200005,  65,       1) /* ResistPierce */
     , (4200005,  66,       1) /* ResistBludgeon */
     , (4200005,  67,       1) /* ResistFire */
     , (4200005,  68,       1) /* ResistCold */
     , (4200005,  69,       1) /* ResistAcid */
     , (4200005,  70,       1) /* ResistElectric */
     , (4200005,  71,       1) /* ResistHealthBoost */
     , (4200005,  72,       1) /* ResistStaminaDrain */
     , (4200005,  73,       1) /* ResistStaminaBoost */
     , (4200005,  74,       1) /* ResistManaDrain */
     , (4200005,  75,       1) /* ResistManaBoost */
     , (4200005, 104,      10) /* ObviousRadarRange */
     , (4200005, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200005,   1, 'Anti Parazi') /* Name */
     , (4200005,   3, 'Male') /* Sex */
     , (4200005,   4, 'Sho') /* HeritageGroup */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200005,   1,   33554433) /* Setup */
     , (4200005,   2,  150994945) /* MotionTable */
     , (4200005,   3,  536870913) /* SoundTable */
     , (4200005,   6,   67108990) /* PaletteBase */
     , (4200005,   4,  805306368) /* CombatTable */
     , (4200005,   5,  234881029) /* QualityFilter */
     , (4200005,   8,  100667446) /* Icon */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (4200005,   1,  200, 0, 0) /* Strength */
     , (4200005,   2,  290, 0, 0) /* Endurance */
     , (4200005,   3,  200, 0, 0) /* Quickness */
     , (4200005,   4,  200, 0, 0) /* Coordination */
     , (4200005,   5,  290, 0, 0) /* Focus */
     , (4200005,   6,  290, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (4200005,   1,     264, 0, 0, 409) /* MaxHealth */
     , (4200005,   3,     210, 0, 0, 500) /* MaxStamina */
     , (4200005,   5,     259, 0, 0, 584) /* MaxMana */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (4200005,  0,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (4200005,  1,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (4200005,  2,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (4200005,  3,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (4200005,  4,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (4200005,  5,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (4200005,  6,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (4200005,  7,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (4200005,  8,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (4200005, 1,   273,  5, 0, 0, False) /* Create Pyreal (273) for Contain */ 
     , (4200005, 2,  5893,  1, 0, 1, False) /* Create Hoary Mattekar Robe (5893) for Wield */
     , (4200005, 2,  3715,  1, 13, 0.66, False) /* Create Olthoi Helm (3715) for Wield */
     , (4200005, 2,    57,  1, 13, 0.66, False) /* Create Platemail Gauntlets (57) for Wield */
     , (4200005, 2,   24207, 10, 0, 0, False) /* Weeping Wand (24207) for Wield */;

     INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (4200005,  7 /* Use */,   1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  12 /* TurnToTarget */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,   5 /* Motion */, 0, 1, 0x1300007D /* BowDeep */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  2,  10 /* Tell */, 1, 1, NULL, 'So after 23 year''s I have finally retired... never thought I would be stuck here. Still helping your noob''s win a fight. Give me a PK Trophy and i''ll burn your Vitae for you.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

     INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (4200005,  6 /* Give */,      1, 1000002 /*PK Trophy */, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  13 /* TextDirect */, 0, 1, NULL, '(Vitae Removed) Now get back in there and make me proud...', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,  90 /* RemoveVitae */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
     /*, (@parent_id,  2,  53 /* SetIntStat *//*, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 134, NULL, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);  Will Turn the player PK if uncommented*/

