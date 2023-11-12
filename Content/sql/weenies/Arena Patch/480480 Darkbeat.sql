DELETE FROM `weenie` WHERE `class_Id` = 480480;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480480, 'ace480480-darkbeat', 12, '2022-11-15 04:30:58') /* Vendor */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480480,   1,         16) /* ItemType - Creature */
     , (480480,   2,         51) /* CreatureType - Empyrean */
     , (480480,   6,         -1) /* ItemsCapacity */
     , (480480,   7,         -1) /* ContainersCapacity */
     , (480480,   8,        120) /* Mass */
     , (480480,  16,         32) /* ItemUseable - Remote */
     , (480480,  25,        275) /* Level */
     , (480480,  27,          0) /* ArmorType - None */
     , (480480,  74,     270496) /* MerchandiseItemTypes - Food, Misc, Writable, PromissoryNote */
     , (480480,  75,          1) /* MerchandiseMinValue */
     , (480480,  76,    100) /* MerchandiseMaxValue */
     , (480480,  93,    2098200) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity, ReportCollisionsAsEnvironment */
     , (480480, 113,          1) /* Gender - Male */
     , (480480, 126,        500) /* VendorHappyMean */
     , (480480, 127,        500) /* VendorHappyVariance */
     , (480480, 133,          4) /* ShowableOnRadar - ShowAlways */
	 , (480480,  95,          5) /* RadarBlipColor - RED */
     , (480480, 134,         16) /* PlayerKillerStatus - RubberGlue */
     , (480480, 146,         97) /* XpOverride */
     , (480480, 188,          3) /* HeritageGroup - Sho */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480480,   1, True ) /* Stuck */
     , (480480,  12, True ) /* ReportCollisions */
     , (480480,  13, False) /* Ethereal */
     , (480480,  19, False) /* Attackable */
     , (480480,  39, True ) /* DealMagicalItems */
     , (480480,  41, True ) /* ReportCollisionsAsEnvironment */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480480,   1,       5) /* HeartbeatInterval */
     , (480480,   2,       0) /* HeartbeatTimestamp */
     , (480480,   3,    0.16) /* HealthRate */
     , (480480,   4,       5) /* StaminaRate */
     , (480480,   5,       1) /* ManaRate */
     , (480480,  11,     300) /* ResetInterval */
     , (480480,  13,     0.9) /* ArmorModVsSlash */
     , (480480,  14,       1) /* ArmorModVsPierce */
     , (480480,  15,     1.1) /* ArmorModVsBludgeon */
     , (480480,  16,     0.4) /* ArmorModVsCold */
     , (480480,  17,     0.4) /* ArmorModVsFire */
     , (480480,  18,       1) /* ArmorModVsAcid */
     , (480480,  19,     0.6) /* ArmorModVsElectric */
     , (480480,  37,       1) /* BuyPrice */
     , (480480,  38,       1) /* SellPrice */
     , (480480,  39,     2) /* DefaultScale */
     , (480480,  54,       5) /* UseRadius */
     , (480480,  64,       1) /* ResistSlash */
     , (480480,  65,       1) /* ResistPierce */
     , (480480,  66,       1) /* ResistBludgeon */
     , (480480,  67,       1) /* ResistFire */
     , (480480,  68,       1) /* ResistCold */
     , (480480,  69,       1) /* ResistAcid */
     , (480480,  70,       1) /* ResistElectric */
     , (480480,  71,       1) /* ResistHealthBoost */
     , (480480,  72,       1) /* ResistStaminaDrain */
     , (480480,  73,       1) /* ResistStaminaBoost */
     , (480480,  74,       1) /* ResistManaDrain */
     , (480480,  75,       1) /* ResistManaBoost */
     , (480480, 104,      10) /* ObviousRadarRange */
     , (480480, 125,       1) /* ResistHealthDrain */
	 , (480480,  76,     0.25) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480480,   1, 'Darkbeat') /* Name */
     , (480480,   3, 'Male') /* Sex */
     , (480480,   4, 'Sho') /* HeritageGroup */
     , (480480,   5, 'Ghost') /* Template */
     , (480480,  24, 'Afterlife') /* TownName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480480,   1, 0x02000001) /* Setup */
     , (480480,   2, 0x09000001) /* MotionTable */
     , (480480,   3, 0x200000B6) /* SoundTable */
     , (480480,   6, 0x040018F3) /* PaletteBase */
     , (480480,   7, 0x10000402) /* ClothingBase */
     , (480480,   8, 0x06003447) /* Icon */
     , (480480,  22, 0x340000AB) /* PhysicsEffectTable */
     , (480480,   9, 0x0500111F) /* EyesTexture */
     , (480480,  10, 0x05001160) /* NoseTexture */
     , (480480,  11, 0x050011D1) /* MouthTexture */
     , (480480,  12, 0x0500024C) /* DefaultEyesTexture */
     , (480480,  13, 0x050002F5) /* DefaultNoseTexture */
     , (480480,  14, 0x0500025C) /* DefaultMouthTexture */
     , (480480,  15, 0x04001FC7) /* HairPalette */
     , (480480,  16, 0x040002BD) /* EyesPalette */
     , (480480,  17, 0x0400049F) /* SkinPalette */
     , (480480,  18, 0x010047FB) /* HeadObject */
     , (480480,  57,    1000003) /* AlternateCurrency - PK Trophy */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (480480,   1, 290, 0, 0) /* Strength */
     , (480480,   2, 290, 0, 0) /* Endurance */
     , (480480,   3, 200, 0, 0) /* Quickness */
     , (480480,   4, 200, 0, 0) /* Coordination */
     , (480480,   5, 290, 0, 0) /* Focus */
     , (480480,   6, 200, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (480480,   1,   310, 0, 0, 455) /* MaxHealth */
     , (480480,   3,   250, 0, 0, 540) /* MaxStamina */
     , (480480,   5,   240, 0, 0, 440) /* MaxMana */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (480480,  0,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (480480,  1,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (480480,  2,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (480480,  3,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (480480,  4,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (480480,  5,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (480480,  6,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (480480,  7,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (480480,  8,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;


INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,    0.6, NULL, NULL, NULL, NULL, 4 /* Buy */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Enjoy! You need the help. Please bring me some more, I miss the taste of tears.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,    0.8, NULL, NULL, NULL, NULL, 2 /* Close */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Let me know if you find this key for my Storage Locker, I might even share some of my loot. Tell your Mom I said hello, I might have left it at her Villa. Maggot.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,    0.6, NULL, NULL, NULL, NULL, 3 /* Sell */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'What am I suppose to do with all this crap?', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,    0.8, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Wow you look like you haven''t been doing your pushups, weakling! I''ve been looking for my key to this locker, I misplaced it somewhere. I''ve stored everything from all those losers I''ve slayed through the years. These items are no longer of any use to me, but maybe you can find a use for it.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,    0.4, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'I hope you brought me some more of those tears, maggot.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,  0.125, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x1000004d /* Wave */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,   0.25, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000097 /* BowDeep */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,  0.375, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000060 /* Shrug */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (480480,  2 /* Vendor */,    0.5, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x1000006c /* Nod */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES  (480480, 2,   6046, 0, 93, 1.1, False) /* Greater Shadow Amuli Coat (6600) for Wield */
     , (480480, 2,   6047, 0, 93, 1.1, False) /* Greater Shadow Amuli Leggings (6606) for Wield */
     , (480480, 2,   74, 0, 93, 0.9, False) /* Olthoi Helm (3715) for Wield */
     , (480480, 2,    57,  1, 93, 0.9, False) /* Create Platemail Gauntlets (57) for Wield */
     , (480480, 2,    107,  1, 93, 0.9, False) /* Create Sollerets (107) for Wield */
     , (480480, 2,   480622, 0, 0, 0, False) /* Deadly Hollow Staff (21362) for Wield */
     , (480480, 4, 1910507,  0, 0, 0, False) /* Create Gelidite Robe (450009) for Shop */
     , (480480, 4, 1910506,  0, 0, 0, False) /* Create Vestiri Robe (450010) for Shop */
     , (480480, 4, 1910505,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	 , (480480, 4, 21063,  0, 0, 0, False) /* Create Gelidite Robe (450009) for Shop */
	 , (480480, 4, 4200050,  0, 0, 0, False) /* Create Vestiri Robe (450010) for Shop */
     , (480480, 4, 4200051,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	 , (480480, 4, 480614,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	, (480480, 4, 480618,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	, (480480, 4, 480617,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	, (480480, 4, 480621,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	, (480480, 4, 480616,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	, (480480, 4, 480620,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	, (480480, 4, 480615,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	, (480480, 4, 480619,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	 , (480480, 4, 460000,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	 , (480480, 4, 480485,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	 , (480480, 4, 480486,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	 , (480480, 4, 480638,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	/*, (480480, 4, 490021,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	/* , (480480, 4, 490022,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	/* , (480480, 4, 490023,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	 , (480480, 4, 490015,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
	 , (480480, 4, 490042,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */;
	 
