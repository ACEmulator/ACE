DELETE FROM `weenie` WHERE `class_Id` = 42128708;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (42128708, 'ace42128708-holtburgtowntreasurer', 12, '2022-01-17 02:08:31') /* Vendor */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42128708,   1,         16) /* ItemType - Creature */
     , (42128708,   2,         31) /* CreatureType - Human */
     , (42128708,   6,         -1) /* ItemsCapacity */
     , (42128708,   7,         -1) /* ContainersCapacity */
     , (42128708,   8,        120) /* Mass */
     , (42128708,  16,         32) /* ItemUseable - Remote */
     , (42128708,  25,        275) /* Level */
     , (42128708,  27,          0) /* ArmorType - None */
     , (42128708,  74,     270496) /* MerchandiseItemTypes - Food, Misc, Writable, PromissoryNote */
     , (42128708,  75,          0) /* MerchandiseMinValue */
     , (42128708,  76,    1000000) /* MerchandiseMaxValue */
     , (42128708,  93,    2098200) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity, ReportCollisionsAsEnvironment */
     , (42128708, 113,          1) /* Gender - Male */
     , (42128708, 126,        500) /* VendorHappyMean */
     , (42128708, 127,        500) /* VendorHappyVariance */
     , (42128708, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (42128708, 134,         16) /* PlayerKillerStatus - RubberGlue */
     , (42128708, 146,         97) /* XpOverride */
     , (42128708, 188,          1) /* HeritageGroup - Aluvian */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42128708,   1, True ) /* Stuck */
     , (42128708,  12, True ) /* ReportCollisions */
     , (42128708,  13, False) /* Ethereal */
     , (42128708,  19, False) /* Attackable */
     , (42128708,  39, True ) /* DealMagicalItems */
     , (42128708,  41, True ) /* ReportCollisionsAsEnvironment */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (42128708,   1,       5) /* HeartbeatInterval */
     , (42128708,   2,       0) /* HeartbeatTimestamp */
     , (42128708,   3,    0.16) /* HealthRate */
     , (42128708,   4,       5) /* StaminaRate */
     , (42128708,   5,       1) /* ManaRate */
     , (42128708,  11,     300) /* ResetInterval */
     , (42128708,  13,     0.9) /* ArmorModVsSlash */
     , (42128708,  14,       1) /* ArmorModVsPierce */
     , (42128708,  15,     1.1) /* ArmorModVsBludgeon */
     , (42128708,  16,     0.4) /* ArmorModVsCold */
     , (42128708,  17,     0.4) /* ArmorModVsFire */
     , (42128708,  18,       1) /* ArmorModVsAcid */
     , (42128708,  19,     0.6) /* ArmorModVsElectric */
     , (42128708,  37,       1) /* BuyPrice */
     , (42128708,  38,       1) /* SellPrice */
     , (42128708,  39,       2) /* DefaultScale */
     , (42128708,  54,       5) /* UseRadius */
     , (42128708,  64,       1) /* ResistSlash */
     , (42128708,  65,       1) /* ResistPierce */
     , (42128708,  66,       1) /* ResistBludgeon */
     , (42128708,  67,       1) /* ResistFire */
     , (42128708,  68,       1) /* ResistCold */
     , (42128708,  69,       1) /* ResistAcid */
     , (42128708,  70,       1) /* ResistElectric */
     , (42128708,  71,       1) /* ResistHealthBoost */
     , (42128708,  72,       1) /* ResistStaminaDrain */
     , (42128708,  73,       1) /* ResistStaminaBoost */
     , (42128708,  74,       1) /* ResistManaDrain */
     , (42128708,  75,       1) /* ResistManaBoost */
     , (42128708, 104,      10) /* ObviousRadarRange */
     , (42128708, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42128708,   1, 'Holtburg Town Treasurer') /* Name */
     , (42128708,   3, 'Male') /* Sex */
     , (42128708,   4, 'Aluvian') /* HeritageGroup */
     , (42128708,   5, 'Vendor') /* Template */
     , (42128708,  24, 'Holtburg') /* TownName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42128708,   1, 0x02000001) /* Setup */
     , (42128708,   2, 0x09000001) /* MotionTable */
     , (42128708,   3, 0x20000001) /* SoundTable */
     , (42128708,   4, 0x30000000) /* CombatTable */
     , (42128708,   8, 0x06001036) /* Icon */
<<<<<<< Updated upstream
     , (42128708,  57,      20630) /* AlternateCurrency - Trade Note (250,000) */;
=======
     , (42128708,  57,      1000002) /* AlternateCurrency - Trade Note (250,000) */;
>>>>>>> Stashed changes

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (42128708,   1, 290, 0, 0) /* Strength */
     , (42128708,   2, 290, 0, 0) /* Endurance */
     , (42128708,   3, 200, 0, 0) /* Quickness */
     , (42128708,   4, 200, 0, 0) /* Coordination */
     , (42128708,   5, 290, 0, 0) /* Focus */
     , (42128708,   6, 200, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (42128708,   1,   310, 0, 0, 455) /* MaxHealth */
     , (42128708,   3,   250, 0, 0, 540) /* MaxStamina */
     , (42128708,   5,   240, 0, 0, 440) /* MaxMana */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (42128708,  0,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (42128708,  1,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (42128708,  2,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (42128708,  3,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (42128708,  4,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (42128708,  5,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (42128708,  6,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (42128708,  7,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (42128708,  8,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,    0.6, NULL, NULL, NULL, NULL, 4 /* Buy */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Wise choise, that should help you!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,    0.8, NULL, NULL, NULL, NULL, 2 /* Close */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Have a nice day, make sure you defend your town!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,    0.6, NULL, NULL, NULL, NULL, 3 /* Sell */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'I dont buy junk!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,    0.8, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Congrats on controlling the town, I hope there is something here you might find useful.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,    0.4, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Are you looking for something in particular?', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,  0.125, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000087 /* Wave */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,   0.25, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x1300007D /* BowDeep */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,  0.375, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000086 /* Shrug */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42128708,  2 /* Vendor */,    0.5, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000083 /* Nod */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (42128708, 2, 22015,  0, 21, 0.9, False) /* Create Pwyll's Guard (22015) for Wield */
     , (42128708, 2,  2594,  0, 14, 0.9, False) /* Create Tunic (2594) for Wield */
     , (42128708, 2, 29514,  0, 21, 0.9, False) /* Create Noble Coat (29514) for Wield */
     , (42128708, 2, 29521,  0, 21, 0.9, False) /* Create Noble Gauntlets (29521) for Wield */
     , (42128708, 2, 29535,  0, 21, 0.9, False) /* Create Noble Leggings (29535) for Wield */
     , (42128708, 2, 29545,  0, 21, 0.9, False) /* Create Noble Sollerets (29545) for Wield */
<<<<<<< Updated upstream
     , (42128708, 4, 524470,  0, 0, 0, False) /* Create Serial Killer's Satchel (524470) for Shop */
     , (42128708, 4, 4200017,  0, 0, 0, False) /* Create Town Control Trinket of Experience (4200017) for Shop */
     , (42128708, 4, 4200018,  0, 0, 0, False) /* Create Ground Anus (4200018) for Shop */
     , (42128708, 4, 4200019,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
     , (42128708, 4, 4200012,  0, 0, 0, False) /* Create Burglers Tools (4200012) for Shop */;
=======
	 , (42128708, 4, 4200021,  0, 0, 0, False) /* Create Town Control Trinket of Experience (4200017) for Shop */
	 , (42128708, 4, 4200017,  0, 0, 0, False) /* Create Town Control Trinket of Experience (4200017) for Shop */
	 , (42128708, 4, 4200020,  0, 0, 0, False) /* Create Town Control Trinket of Experience (4200017) for Shop */
     , (42128708, 4, 450619,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	 , (42128708, 4, 450621,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	 , (42128708, 4, 450622,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	 , (42128708, 4, 480000,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	 , (42128708, 4, 480001,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	 , (42128708, 4, 2031668,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	 , (42128708, 4, 480008,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	, (42128708, 4, 480009,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	, (42128708, 4, 480010,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	, (42128708, 4, 480011,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	 , (42128708, 4, 480014,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	 , (42128708, 4, 480017,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	, (42128708, 4, 480019,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	, (42128708, 4, 1910506,  0, 0, 0, False) /* Create A Fuckin' Bang (4200019) for Shop */
	, (42128708, 4, 4200141,  0, 0, 0, False) /* Create Town Control Trinket of Experience (4200017) for Shop */;
>>>>>>> Stashed changes

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2022-01-16T16:45:27.238096Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
