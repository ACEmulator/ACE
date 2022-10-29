DELETE FROM `weenie` WHERE `class_Id` = 1028707;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1028707, 'ace1028707-dalomar', 12, '2021-12-28 03:22:54') /* Vendor */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1028707,   1,         16) /* ItemType - Creature */
     , (1028707,   2,         31) /* CreatureType - Human */
     , (1028707,   6,         -1) /* ItemsCapacity */
     , (1028707,   7,         -1) /* ContainersCapacity */
     , (1028707,   8,        120) /* Mass */
     , (1028707,  16,         32) /* ItemUseable - Remote */
     , (1028707,  25,        275) /* Level */
     , (1028707,  27,          0) /* ArmorType - None */
     , (1028707,  74,     270496) /* MerchandiseItemTypes - Food, Misc, Writable, PromissoryNote */
     , (1028707,  75,          0) /* MerchandiseMinValue */
     , (1028707,  76,    1000000) /* MerchandiseMaxValue */
     , (1028707,  93,    2098200) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity, ReportCollisionsAsEnvironment */
     , (1028707, 113,          1) /* Gender - Male */
     , (1028707, 126,        500) /* VendorHappyMean */
     , (1028707, 127,        500) /* VendorHappyVariance */
     , (1028707, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (1028707, 134,         16) /* PlayerKillerStatus - RubberGlue */
     , (1028707, 146,         97) /* XpOverride */
     , (1028707, 188,          3) /* HeritageGroup - Sho */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1028707,   1, True ) /* Stuck */
     , (1028707,  12, True ) /* ReportCollisions */
     , (1028707,  13, False) /* Ethereal */
     , (1028707,  19, False) /* Attackable */
     , (1028707,  39, True ) /* DealMagicalItems */
     , (1028707,  41, True ) /* ReportCollisionsAsEnvironment */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1028707,   1,       5) /* HeartbeatInterval */
     , (1028707,   2,       0) /* HeartbeatTimestamp */
     , (1028707,   3,    0.16) /* HealthRate */
     , (1028707,   4,       5) /* StaminaRate */
     , (1028707,   5,       1) /* ManaRate */
     , (1028707,  11,     300) /* ResetInterval */
     , (1028707,  13,     0.9) /* ArmorModVsSlash */
     , (1028707,  14,       1) /* ArmorModVsPierce */
     , (1028707,  15,     1.1) /* ArmorModVsBludgeon */
     , (1028707,  16,     0.4) /* ArmorModVsCold */
     , (1028707,  17,     0.4) /* ArmorModVsFire */
     , (1028707,  18,       1) /* ArmorModVsAcid */
     , (1028707,  19,     0.6) /* ArmorModVsElectric */
     , (1028707,  37,       1) /* BuyPrice */
     , (1028707,  38,       1) /* SellPrice */
     , (1028707,  39,     1.5) /* DefaultScale */
     , (1028707,  54,       5) /* UseRadius */
     , (1028707,  64,       1) /* ResistSlash */
     , (1028707,  65,       1) /* ResistPierce */
     , (1028707,  66,       1) /* ResistBludgeon */
     , (1028707,  67,       1) /* ResistFire */
     , (1028707,  68,       1) /* ResistCold */
     , (1028707,  69,       1) /* ResistAcid */
     , (1028707,  70,       1) /* ResistElectric */
     , (1028707,  71,       1) /* ResistHealthBoost */
     , (1028707,  72,       1) /* ResistStaminaDrain */
     , (1028707,  73,       1) /* ResistStaminaBoost */
     , (1028707,  74,       1) /* ResistManaDrain */
     , (1028707,  75,       1) /* ResistManaBoost */
     , (1028707, 104,      10) /* ObviousRadarRange */
     , (1028707, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1028707,   1, 'Dalomar') /* Name */
     , (1028707,   3, 'Male') /* Sex */
     , (1028707,   4, 'Sho') /* HeritageGroup */
     , (1028707,   5, 'Vendor') /* Template */
     , (1028707,  24, 'Throwdown Arena') /* TownName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1028707,   1, 0x02000001) /* Setup */
     , (1028707,   2, 0x09000001) /* MotionTable */
     , (1028707,   3, 0x20000001) /* SoundTable */
     , (1028707,   4, 0x30000000) /* CombatTable */
     , (1028707,   8, 0x06001036) /* Icon */
     , (1028707,  57,    1000002) /* AlternateCurrency - PK Trophy */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (1028707,   1, 290, 0, 0) /* Strength */
     , (1028707,   2, 290, 0, 0) /* Endurance */
     , (1028707,   3, 200, 0, 0) /* Quickness */
     , (1028707,   4, 200, 0, 0) /* Coordination */
     , (1028707,   5, 290, 0, 0) /* Focus */
     , (1028707,   6, 200, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (1028707,   1,   310, 0, 0, 455) /* MaxHealth */
     , (1028707,   3,   250, 0, 0, 540) /* MaxStamina */
     , (1028707,   5,   240, 0, 0, 440) /* MaxMana */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (1028707,  0,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (1028707,  1,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (1028707,  2,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (1028707,  3,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (1028707,  4,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (1028707,  5,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (1028707,  6,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (1028707,  7,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (1028707,  8,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,    0.6, NULL, NULL, NULL, NULL, 4 /* Buy */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Best looking outfit choice I''ve seen all day!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,    0.8, NULL, NULL, NULL, NULL, 2 /* Close */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Don''t let me catch that beautiful suit, laying around in the dirt', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,    0.6, NULL, NULL, NULL, NULL, 3 /* Sell */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'What am I suppose to do with all this junk', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,    0.8, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Welcome to my dressing room, hope you find an outfit you like!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,    0.4, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Welcome to my dressing room, hope you find an outfit you like!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,  0.125, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000087 /* Wave */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,   0.25, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x1300007D /* BowDeep */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,  0.375, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000086 /* Shrug */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (1028707,  2 /* Vendor */,    0.5, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000083 /* Nod */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (1028707, 2,    74,  0, 92, 0, False) /* Create Heaume (74) for Wield */
     , (1028707, 2,    57,  0, 92, 0, False) /* Create Platemail Gauntlets (57) for Wield */
     , (1028707, 2,   107,  0, 92, 0, False) /* Create Sollerets (107) for Wield */
     , (1028707, 2, 37201,  0, 27, 0, False) /* Create Olthoi Amuli Leggings (37201) for Wield */
     , (1028707, 2, 37215,  0, 27, 0, False) /* Create Olthoi Koujia Breastplate (37215) for Wield */
     , (1028707, 2, 37205,  0, 27, 0, False) /* Create Olthoi Celdon Sleeves (37205) for Wield */
     , (1028707, 2,   130,  0, 92, 0, False) /* Create Shirt (130) for Wield */
     , (1028707, 4, 1004981,  0, 0, 0, False) /* Create  (1004981) for Shop */
     , (1028707, 4, 1003715,  0, 0, 0, False) /* Create  (1003715) for Shop */
     , (1028707, 4, 1012236,  0, 0, 0, False) /* Create Energy Crystal (1012236) for Shop */
     , (1028707, 4, 1009064,  0, 0, 0, False) /* Create Hieromancer's Orb (1009064) for Shop */
     , (1028707, 4, 1011305,  0, 0, 0, False) /* Create Stave of Palenqual (1011305) for Shop */
     , (1028707, 4, 1049901,  0, 0, 0, False) /* Create Prismatic Shadow Bracers (1049901) for Shop */
     , (1028707, 4, 1002031,  0, 0, 0, False) /* Create  (1002031) for Shop */
     , (1028707, 4, 1049905,  0, 0, 0, False) /* Create Prismatic Shadow Breastplate (1049905) for Shop */
     , (1028707, 4, 1049909,  0, 0, 0, False) /* Create Prismatic Shadow Gauntlets (1049909) for Shop */
     , (1028707, 4, 1049913,  0, 0, 0, False) /* Create Prismatic Shadow Girth (1049913) for Shop */
     , (1028707, 4, 1049917,  0, 0, 0, False) /* Create Prismatic Shadow Greaves (1049917) for Shop */
     , (1028707, 4, 1049921,  0, 0, 0, False) /* Create Prismatic Shadow Helm (1049921) for Shop */
     , (1028707, 4, 1049925,  0, 0, 0, False) /* Create Prismatic Shadow Pauldrons (1049925) for Shop */
     , (1028707, 4, 1049929,  0, 0, 0, False) /* Create Prismatic Shadow Sollerets (1049929) for Shop */
     , (1028707, 4, 1049933,  0, 0, 0, False) /* Create Prismatic Shadow Tassets (1049933) for Shop */
     , (1028707, 4, 2046345,  0, 0, 0, False) /* Create  (2046345) for Shop */
     , (1028707, 4, 2046551,  0, 0, 0, False) /* Create  (2046551) for Shop */
     , (1028707, 4, 2046552,  0, 0, 0, False) /* Create  (2046552) for Shop */
     , (1028707, 4, 2046553,  0, 0, 0, False) /* Create  (2046553) for Shop */
     , (1028707, 4, 2046615,  0, 0, 0, False) /* Create  (2046615) for Shop */
     , (1028707, 4, 13241329,  0, 6, 0, False) /* Create Hulking Bunny Slippers (13241329) for Shop */
     , (1028707, 4, 1032149,  0, 0, 0, False) /* Create White Rabbit Mask (1032149) for Shop */
     , (1028707, 4, 32927,  0, 0, 0, False) /* Create White Rabbit Girth (32927) for Shop */
     , (1028707, 4, 1036229,  0, 0, 0, False) /* Create Rift Orb (1036229) for Shop */
     , (1028707, 4, 1036230,  0, 0, 0, False) /* Create Rift Orb (1036230) for Shop */
     , (1028707, 4, 1014932,  0, 0, 0, False) /* Create Crimped Hat (1014932) for Shop */
     , (1028707, 4, 1025703,  0, 0, 0, False) /* Create Dapper Suit (1025703) for Shop */
     , (1028707, 4, 1010731,  0, 0, 0, False) /* Create Quiddity Orb (1010731) for Shop */
     , (1028707, 4, 1035297,  0, 0, 0, False) /* Create Greatsword of Flame (1035297) for Shop */
     , (1028707, 4, 10352971,  0, 0, 0, False) /* Create Greatsword of Frost (10352971) for Shop */
     , (1028707, 4, 10352972,  0, 0, 0, False) /* Create Greatsword of Lightning (10352972) for Shop */
     , (1028707, 4, 10352973,  0, 0, 0, False) /* Create Greatsword of Acid (10352973) for Shop */
     , (1028707, 4, 10352974,  0, 0, 0, False) /* Create Greatsword of Slashing and Piercing (10352974) for Shop */
     , (1028707, 4, 10416111,  0, 0, 0, False) /* Create Greatsword of Iron Flame (10416111) for Shop */
     , (1028707, 4, 10416112,  0, 0, 0, False) /* Create Greatsword of Iron Frost (10416112) for Shop */
     , (1028707, 4, 10416113,  0, 0, 0, False) /* Create Greatsword of Iron Lightning (10416113) for Shop */
     , (1028707, 4, 10416114,  0, 0, 0, False) /* Create Greatsword of Iron Acid (10416114) for Shop */
     , (1028707, 4, 10416115,  0, 0, 0, False) /* Create Greatsword of Iron Slashing and Piercing (10416115) for Shop */
     , (1028707, 4, 10416116,  0, 0, 0, False) /* Create Greatsword of Iron Bludgeoning (10416116) for Shop */
     , (1028707, 4, 1037585,  0, 0, 0, False) /* Create Soul Bound Staff (1037585) for Shop */
     , (1028707, 4, 1051988,  0, 0, 0, False) /* Create Rynthid Tentacle Bow (1051988) for Shop */
     , (1028707, 4, 1051989,  0, 0, 0, False) /* Create Rynthid Tentacle Wand (1051989) for Shop */
     , (1028707, 4,     0,  0, 0, 0, False) /* Create nothing for Shop */
     , (1028707, 4, 10049811,  0, 0, 0, False) /* Create Ice Heaume of Frore (10049811) for Shop */
     , (1028707, 4, 10049812,  0, 0, 0, False) /* Create Ice Heaume of Frore (10049812) for Shop */
     , (1028707, 4, 10049813,  0, 0, 0, False) /* Create Ice Heaume of Frore (10049813) for Shop */
     , (1028707, 4, 1031330,  0, 0, 0, False) /* Create Scribe's Quill (1031330) for Shop */
     , (1028707, 4, 1031331,  0, 0, 0, False) /* Create Scribe's Quill (1031331) for Shop */
     , (1028707, 4, 1031332,  0, 0, 0, False) /* Create Scribe's Quill (1031332) for Shop */
     , (1028707, 4, 1031333,  0, 0, 0, False) /* Create Scribe's Quill (1031333) for Shop */
     , (1028707, 4, 1030376,  0, 0, 0, False) /* Create Orb of the Ironsea (1030376) for Shop */
     , (1028707, 4, 1043848,  0, 0, 0, False) /* Create Heart of Darkest Flame (1043848) for Shop */
     , (1028707, 4, 33104,  0, 0, 0, False) /* Create Helm of Isin Dule (33104) for Shop */
     , (1028707, 4, 1052444,  0, 0, 0, False) /* Create Holiday Present (1052444) for Shop */
     , (1028707, 4, 1009047,  0, 0, 0, False) /* Create Globe of Auberean (1009047) for Shop */
     , (1028707, 4, 1023774,  0, 0, 0, False) /* Create Casting Stein (1023774) for Shop */
     , (1028707, 4, 1025895,  0, 0, 0, False) /* Create Puppeteer's Skull (1025895) for Shop */
     , (1028707, 4, 1004231,  0, 0, 0, False) /* Create  (1004231) for Shop */
     , (1028707, 4, 1007705,  0, 0, 0, False) /* Create Greater Celdon Shadow Leggings (1007705) for Shop */
     , (1028707, 4, 1006801,  0, 0, 0, False) /* Create Nexus Amuli Leggings (1006801) for Shop */
     , (1028707, 4, 1006799,  0, 0, 0, False) /* Create Nexus Amuli Coat (1006799) for Shop */
     , (1028707, 4, 1008807,  0, 0, 0, False) /* Create Caulnalain Helm of the Lightbringer (1008807) for Shop */
     , (1028707, 4, 1008806,  0, 0, 0, False) /* Create Fenmalain Helm of the Lightbringer (1008806) for Shop */
     , (1028707, 4, 1008805,  0, 0, 0, False) /* Create Nexus Helm of the Lightbringer (1008805) for Shop */
     , (1028707, 4, 1008808,  0, 0, 0, False) /* Create Shendolain Helm of the Lightbringer (1008808) for Shop */
     , (1028707, 4, 1008809,  0, 0, 0, False) /* Create Herald's Helm of the Lightbringer (1008809) for Shop */
     , (1028707, 4, 1008799,  0, 0, 0, False) /* Create Great Work Staff of the Lightbringer (1008799) for Shop */
     , (1028707, 4, 1025842,  0, 0, 0, False) /* Create Plaguefang's Robe (1025842) for Shop */
     , (1028707, 4, 1031304,  0, 0, 0, False) /* Create Luminous Robe (1031304) for Shop */
     , (1028707, 4, 1041910,  0, 0, 0, False) /* Create Enhanced Eye of the Quiddity (1041910) for Shop */
     , (1028707, 4, 51868,  0, 0, 0, False) /* Create Rynthid Energy Tentacles (51868) for Shop */
     , (1028707, 4, 44840,  0, 0, 0, False) /* Create Cloak (44840) for Shop */
     , (1028707, 4, 13241326,  0, 0, 0, False) /* Create Legendary Robe of Utter Darkness (13241326) for Shop */
     , (1028707, 4, 13241329,  0, 0, 0, False) /* Create Hulking Bunny Slippers (13241329) for Shop */
     , (1028707, 4, 13241330,  0, 0, 0, False) /* Create Quartered Cloak (13241330) for Shop */
     , (1028707, 4, 13241334,  0, 0, 0, False) /* Create The Helm of the Golden Flame (13241334) for Shop */
     , (1028707, 4, 34026,  0, 0, 0, False) /* Create Mask of the Hopeslayer (34026) for Shop */
     , (1028707, 4, 1006600,  0, 0, 0, False) /* Create Greater Amuli Shadow Coat (1006600) for Shop */
     , (1028707, 4, 1006606,  0, 0, 0, False) /* Create Greater Amuli Shadow Leggings (1006606) for Shop */
     , (1028707, 4, 1006594,  0, 0, 0, False) /* Create Greater Celdon Shadow Breastplate (1006594) for Shop */
     , (1028707, 4, 1006603,  0, 0, 0, False) /* Create Greater Celdon Shadow Girth (1006603) for Shop */
     , (1028707, 4, 1006615,  0, 0, 0, False) /* Create Greater Celdon Shadow Sleeves (1006615) for Shop */
     , (1028707, 4, 1006609,  0, 0, 0, False) /* Create Greater Celdon Shadow Leggings (1006609) for Shop */
     , (1028707, 4, 1005851,  0, 0, 0, False) /* Create Faran Robe with Hood (1005851) for Shop */
     , (1028707, 4, 32155,  0, 0, 0, False) /* Create Ursuin Guise (32155) for Shop */
     , (1028707, 4, 1002628,  0, 0, 0, False) /* Create  (1002628) for Shop */
     , (1028707, 4, 1002629,  0, 0, 0, False) /* Create  (1002629) for Shop */
     , (1028707, 4, 1002630,  0, 0, 0, False) /* Create  (1002630) for Shop */
     , (1028707, 4, 1002631,  0, 0, 0, False) /* Create  (1002631) for Shop */
     , (1028707, 4, 1002632,  0, 0, 0, False) /* Create  (1002632) for Shop */
     , (1028707, 4, 1002633,  0, 0, 0, False) /* Create  (1002633) for Shop */
     , (1028707, 4, 1003594,  0, 0, 0, False) /* Create  (1003594) for Shop */
     , (1028707, 4, 1003942,  0, 0, 0, False) /* Create  (1003942) for Shop */
     , (1028707, 4, 1038922,  0, 0, 0, False) /* Create T'thuun Shield (1038922) for Shop */
     , (1028707, 4, 1043130,  0, 0, 0, False) /* Create Iron Blade Aegis (1043130) for Shop */
     , (1028707, 4, 1043131,  0, 0, 0, False) /* Create Iron Blade Shield (1043131) for Shop */
     , (1028707, 4, 1030372,  0, 0, 0, False) /* Create Shield of Engorgement (1030372) for Shop */
     , (1028707, 4, 1035295,  0, 0, 0, False) /* Create Shield of Perfect Light (1035295) for Shop */
     , (1028707, 4, 1033105,  0, 0, 0, False) /* Create Shield of Isin Dule (1033105) for Shop */
     , (1028707, 4, 1032163,  0, 0, 0, False) /* Create Two Headed Snowman Mask (1032163) for Shop */
     , (1028707, 4, 1036524,  0, 0, 0, False) /* Create Pumpkin Shield (1036524) for Shop */
     , (1028707, 4, 1036228,  0, 0, 0, False) /* Create Coral Shield (1036228) for Shop */
     , (1028707, 4, 1036227,  0, 0, 0, False) /* Create Coral Shield (1036227) for Shop */
     , (1028707, 4, 1043141,  0, 0, 0, False) /* Create Aegis of the Gold Gear (1043141) for Shop */
     , (1028707, 4, 1035982,  0, 0, 0, False) /* Create Aegis of the Golden Flame (1035982) for Shop */
     , (1028707, 4, 1025335,  0, 0, 0, False) /* Create Virindi Consul Mask (1025335) for Shop */
     , (1028707, 4, 1024879,  0, 0, 0, False) /* Create Virindi Profatrix Mask (1024879) for Shop */
     , (1028707, 4, 1011998,  0, 0, 0, False) /* Create Virindi Inquisitor's Mask (1011998) for Shop */
     , (1028707, 4, 1008153,  0, 0, 0, False) /* Create Virindi Mask (1008153) for Shop */
     , (1028707, 4, 1012207,  0, 0, 0, False) /* Create Inviso Mask (1012207) for Shop */
     , (1028707, 4, 1012269,  0, 0, 0, False) /* Create  (1012269) for Shop */
     , (1028707, 4, 1005937,  0, 0, 0, False) /* Create  (1005937) for Shop */
     , (1028707, 4, 1032148,  0, 0, 0, False) /* Create Shadow Wings Breastplate (1032148) for Shop */
     , (1028707, 4, 46832,  0, 0, 0, False) /* Create  (46832) for Shop */
     , (1028707, 4, 1052269,  0, 0, 0, False) /* Create  (1052269) for Shop */
     , (1028707, 4, 1034144,  0, 0, 0, False) /* Create  (1034144) for Shop */
     , (1028707, 4, 1034214,  0, 0, 0, False) /* Create  (1034214) for Shop */
     , (1028707, 4, 1034255,  0, 0, 0, False) /* Create  (1034255) for Shop */
     , (1028707, 4, 1004137,  0, 0, 0, False) /* Create  (1004137) for Shop */
     , (1028707, 4, 1004138,  0, 0, 0, False) /* Create  (1004138) for Shop */
     , (1028707, 4, 1030872,  0, 0, 0, False) /* Create Eye of the Fallen (1030872) for Shop */
     , (1028707, 4, 1022256,  0, 0, 0, False) /* Create Fishing Pole (1022256) for Shop */
     , (1028707, 4, 1052193,  0, 0, 0, False) /* Create Mukkir Wings (1052193) for Shop */
     , (1028707, 4, 105280,  0, 0, 0, False) /* Create Holiday Sweater (105280) for Shop */
     , (1028707, 4, 1051858,  0, 0, 0, False) /* Create Rynthid Sorcerer of Torment's Mask (1051858) for Shop */
     , (1028707, 4, 1051854,  0, 0, 0, False) /* Create Rynthid Minion of Torment's Mask (1051854) for Shop */
     , (1028707, 4, 1051855,  0, 0, 0, False) /* Create Rynthid Minion of Rage's Mask (1051855) for Shop */
     , (1028707, 4, 1051857,  0, 0, 0, False) /* Create Rynthid Ravager's Mask (1051857) for Shop */
     , (1028707, 4, 1051856,  0, 0, 0, False) /* Create Rynthid Berserker's Mask (1051856) for Shop */
     , (1028707, 4, 1042667,  0, 0, 0, False) /* Create Top Hat (1042667) for Shop */
     , (1028707, 4, 1043197,  0, 0, 0, False) /* Create Apostate Grand Director's Mask (1043197) for Shop */
     , (1028707, 4, 1040801,  0, 0, 0, False) /* Create Nexus Crawler's Mask (1040801) for Shop */
     , (1028707, 4, 1043040,  0, 0, 0, False) /* Create Nexus Crawler's Mask (1043040) for Shop */
     , (1028707, 4, 1009622,  0, 0, 0, False) /* Create Chef's Hat (1009622) for Shop */
     , (1028707, 4, 1008400,  0, 0, 0, False) /* Create  (1008400) for Shop */
     , (1028707, 4, 1052699,  0, 0, 0, False) /* Create Wooden Top (1052699) for Shop */
     , (1028707, 4, 1040517,  0, 0, 0, False) /* Create Olthoibane Sword of Lost Light (1040517) for Shop */
     , (1028707, 4, 1011986,  0, 0, 0, False) /* Create Energy Crown (1011986) for Shop */
     , (1028707, 4, 1032159,  0, 0, 0, False) /* Create Penguin Mask (1032159) for Shop */
     , (1028707, 4, 1032160,  0, 0, 0, False) /* Create Uber Penguin Mask (1032160) for Shop */
     , (1028707, 4, 1022080,  0, 0, 0, False) /* Create Impious Staff (1022080) for Shop */
     , (1028707, 4, 1033050,  0, 0, 0, False) /* Create Red Rune Silveran Dagger (1033050) for Shop */
     , (1028707, 4, 1033102,  0, 0, 0, False) /* Create Red Rune Slashing Silveran Wand (1033102) for Shop */
     , (1028707, 4, 1027839,  0, 0, 0, False) /* Create Ultimate Singularity Scepter of War Magic (1027839) for Shop */
     , (1028707, 4, 1027840,  0, 0, 0, False) /* Create Singularity Scepter of War Magic (1027840) for Shop */
     , (1028707, 4, 1027841,  0, 0, 0, False) /* Create Bound Singularity Scepter of War Magic (1027841) for Shop */
     , (1028707, 4, 1052514,  0, 0, 0, False) /* Create Painter's Palette (1052514) for Shop */
     , (1028707, 4, 1011985,  0, 0, 0, False) /* Create Heavy Ursuin Coat (1011985) for Shop */
     , (1028707, 4, 1026498,  0, 0, 0, False) /* Create Crown of Anointed Blood (1026498) for Shop */
     , (1028707, 4, 1021373,  0, 0, 0, False) /* Create Martine's Mask (1021373) for Shop */
     , (1028707, 4, 1043046,  0, 0, 0, False) /* Create Paradox-touched Olthoi Dagger (1043046) for Shop */
     , (1028707, 4, 1029818,  0, 0, 0, False) /* Create Badlands Siraluun Headdress (1029818) for Shop */
     , (1028707, 4, 1029819,  0, 0, 0, False) /* Create Kithless Siraluun Headdress (1029819) for Shop */
     , (1028707, 4, 1029820,  0, 0, 0, False) /* Create Littoral Siraluun Headdress (1029820) for Shop */
     , (1028707, 4, 1029821,  0, 0, 0, False) /* Create Marsh Siraluun Headdress (1029821) for Shop */
     , (1028707, 4, 1029822,  0, 0, 0, False) /* Create Strand Siraluun Headdress (1029822) for Shop */
     , (1028707, 4, 1029823,  0, 0, 0, False) /* Create Tidal Siraluun Headdress (1029823) for Shop */
     , (1028707, 4, 1029824,  0, 0, 0, False) /* Create Timber Siraluun Headdress (1029824) for Shop */
     , (1028707, 4, 1029825,  0, 0, 0, False) /* Create Untamed Siraluun Headdress (1029825) for Shop */
     , (1028707, 4, 34217,  0, 0, 0, False) /* Create Mask of the Malik (34217) for Shop */
     , (1028707, 4, 34142,  0, 0, 0, False) /* Create The Royal Bull of Sanamar (34142) for Shop */
     , (1028707, 4, 34255,  0, 0, 0, False) /* Create Karlun's Visage (34255) for Shop */
     , (1028707, 4, 34143,  0, 0, 0, False) /* Create The Boar of Cinghalle (34143) for Shop */
     , (1028707, 4, 34214,  0, 0, 0, False) /* Create The Dragon of Power (34214) for Shop */
     , (1028707, 4, 34144,  0, 0, 0, False) /* Create The Stag of Bellenesse (34144) for Shop */
     , (1028707, 4, 1009598,  0, 0, 0, False) /* Create Fist of the Quiddity (1009598) for Shop */
     , (1028707, 4, 1033058,  0, 0, 0, False) /* Create Red Rune Silveran Axe (1033058) for Shop */
     , (1028707, 4, 1030866,  0, 0, 0, False) /* Create Hammer of the Fallen (1030866) for Shop */
     , (1028707, 4, 1008473,  0, 0, 0, False) /* Create Fine Spine Axe (1008473) for Shop */
     , (1028707, 4, 1026031,  0, 0, 0, False) /* Create Bone Dagger (1026031) for Shop */
     , (1028707, 4, 1028218,  0, 0, 0, False) /* Create Sable Tooth Dirk (1028218) for Shop */
     , (1028707, 4, 1023547,  0, 0, 0, False) /* Create Fang Mace (1023547) for Shop */
     , (1028707, 4, 1029910,  0, 0, 0, False) /* Create Marsh Siraluun Waaika (1029910) for Shop */
     , (1028707, 4, 1011431,  0, 0, 0, False) /* Create Audetaunga's Taiaha of the Mountains (1011431) for Shop */
     , (1028707, 4, 1030634,  0, 0, 0, False) /* Create Cyphis Suldow's Half Moon Spear (1030634) for Shop */
     , (1028707, 4, 1032773,  0, 0, 0, False) /* Create Spinning Staff of Death (1032773) for Shop */
     , (1028707, 4, 1023542,  0, 0, 0, False) /* Create Staff of the Nomads (1023542) for Shop */
     , (1028707, 4, 1031291,  0, 0, 0, False) /* Create Renlen's Grace (1031291) for Shop */
     , (1028707, 4, 1035949,  0, 0, 0, False) /* Create Tusker Bone Sword (1035949) for Shop */
     , (1028707, 4, 1026599,  0, 0, 0, False) /* Create Esorcelled Falchion (1026599) for Shop */
     , (1028707, 4, 1025905,  0, 0, 0, False) /* Create Needletooth (1025905) for Shop */
     , (1028707, 4, 1030862,  0, 0, 0, False) /* Create Banished Nekode (1030862) for Shop */
     , (1028707, 4, 1030313,  0, 0, 0, False) /* Create Dripping Death (1030313) for Shop */
     , (1028707, 4, 1030335,  0, 0, 0, False) /* Create Hevelio's Half-Moon (1030335) for Shop */
     , (1028707, 4, 1030323,  0, 0, 0, False) /* Create Tri-Blade Spear (1030323) for Shop */
     , (1028707, 4, 1020227,  0, 0, 0, False) /* Create Iasparailaun (1020227) for Shop */
     , (1028707, 4, 1030312,  0, 0, 0, False) /* Create Baton of Tirethas (1030312) for Shop */
     , (1028707, 4, 1030332,  0, 0, 0, False) /* Create Guardian of Pwyll (1030332) for Shop */
     , (1028707, 4, 1030339,  0, 0, 0, False) /* Create Thunderhead (1030339) for Shop */
     , (1028707, 4, 1023538,  0, 0, 0, False) /* Create Basalt Blade (1023538) for Shop */
     , (1028707, 4, 1028067,  0, 0, 0, False) /* Create Superior Ashbane (1028067) for Shop */
     , (1028707, 4, 1035804,  0, 0, 0, False) /* Create Demon Swarm Sword (1035804) for Shop */
     , (1028707, 4, 1029047,  0, 0, 0, False) /* Create Repugnant Staff (1029047) for Shop */
     , (1028707, 4, 1041912,  0, 0, 0, False) /* Create Enhanced Stave of the Quiddity (1041912) for Shop */
     , (1028707, 4, 1011329,  0, 0, 0, False) /* Create Aun Tanua's War Taiaha (1011329) for Shop */
     , (1028707, 4, 1023539,  0, 0, 0, False) /* Create Serpent's Fang (1023539) for Shop */
     , (1028707, 4, 1030860,  0, 0, 0, False) /* Create Banished Mace (1030860) for Shop */
     , (1028707, 4, 1024027,  0, 0, 0, False) /* Create Scepter of Thunderous Might (1024027) for Shop */
     , (1028707, 4, 1025906,  0, 0, 0, False) /* Create Mace of Dissonance (1025906) for Shop */
     , (1028707, 4, 1008788,  0, 0, 0, False) /* Create Obsidian Dagger (1008788) for Shop */
     , (1028707, 4, 1024557,  0, 0, 0, False) /* Create Quadruple-bladed Axe (1024557) for Shop */
     , (1028707, 4, 1032499,  0, 0, 0, False) /* Create Axe of Winter Flame (1032499) for Shop */
     , (1028707, 4, 1030342,  0, 0, 0, False) /* Create Count Renari's Equalizer (1030342) for Shop */
     , (1028707, 4, 1030343,  0, 0, 0, False) /* Create Smite (1030343) for Shop */
     , (1028707, 4, 1038926,  0, 0, 0, False) /* Create T'thuun Mace (1038926) for Shop */
     , (1028707, 4, 1038910,  0, 0, 0, False) /* Create Blighted Claw (1038910) for Shop */
     , (1028707, 4, 1024028,  0, 0, 0, False) /* Create Crescent Moons (1024028) for Shop */
     , (1028707, 4, 1023522,  0, 0, 0, False) /* Create Overlord's Sword (1023522) for Shop */
     , (1028707, 4, 1040519,  0, 0, 0, False) /* Create Skeletonbane Sword of Lost Light (1040519) for Shop */
     , (1028707, 4, 1040089,  0, 0, 0, False) /* Create Empowered Sword of Lost Hope (1040089) for Shop */
     , (1028707, 4, 1030863,  0, 0, 0, False) /* Create Banished Spear (1030863) for Shop */
     , (1028707, 4, 1035615,  0, 0, 0, False) /* Create Blessed Spear of the Mosswart Gods (1035615) for Shop */
     , (1028707, 4, 1031500,  0, 0, 0, False) /* Create Worn Old Mace (1031500) for Shop */
     , (1028707, 4, 1035407,  0, 0, 0, False) /* Create Burnja's Board with Nails (1035407) for Shop */
     , (1028707, 4, 1023536,  0, 0, 0, False) /* Create Fetid Dirk (1023536) for Shop */
     , (1028707, 4, 1035547,  0, 0, 0, False) /* Create Doom Hammer (1035547) for Shop */
     , (1028707, 4, 1025904,  0, 0, 0, False) /* Create Atakir's Blade (1025904) for Shop */
     , (1028707, 4, 1026593,  0, 0, 0, False) /* Create Sickle of Writhing Fury (1026593) for Shop */
     , (1028707, 4, 1028490,  0, 0, 0, False) /* Create Noble War Maul (1028490) for Shop */
     , (1028707, 4, 1034024,  0, 0, 0, False) /* Create Silifi of Crimson Night (1034024) for Shop */
     , (1028707, 4, 1042663,  0, 0, 0, False) /* Create Revenant's Scythe (1042663) for Shop */
     , (1028707, 4, 1042662,  0, 0, 0, False) /* Create Chitin Cracker (1042662) for Shop */
     , (1028707, 4, 1041794,  0, 0, 0, False) /* Create Greatblade of the Quiddity (1041794) for Shop */
     , (1028707, 4, 1042932,  0, 0, 0, False) /* Create Well-Balanced Lugian Greataxe (1042932) for Shop */
     , (1028707, 4, 1038923,  0, 0, 0, False) /* Create T'thuun Bow (1038923) for Shop */
     , (1028707, 4, 1030304,  0, 0, 0, False) /* Create Black Cloud Bow (1030304) for Shop */
     , (1028707, 4, 1030303,  0, 0, 0, False) /* Create Serpent's Flight (1030303) for Shop */
     , (1028707, 4, 1030351,  0, 0, 0, False) /* Create Dragonspine Bow (1030351) for Shop */
     , (1028707, 4, 1030516,  0, 0, 0, False) /* Create Tassets of Leikotha's Tears (1030516) for Shop */
     , (1028707, 4, 503715,  0, 0, 0, False) /* Create  (503715) for Shop */
     , (1028707, 4, 505937,  0, 0, 0, False) /* Create  (505937) for Shop */
     , (1028707, 4, 522550,  0, 0, 0, False) /* Create  (522550) for Shop */
     , (1028707, 4, 527889,  0, 0, 0, False) /* Create  (527889) for Shop */
     , (1028707, 4, 528861,  0, 0, 0, False) /* Create  (528861) for Shop */
     , (1028707, 4, 528866,  0, 0, 0, False) /* Create  (528866) for Shop */
     , (1028707, 4, 528868,  0, 0, 0, False) /* Create  (528868) for Shop */
     , (1028707, 4, 528869,  0, 0, 0, False) /* Create  (528869) for Shop */
     , (1028707, 4, 532168,  0, 0, 0, False) /* Create  (532168) for Shop */
     , (1028707, 4, 545031,  0, 0, 0, False) /* Create  (545031) for Shop */
     , (1028707, 4, 2031668,  0, 0, 0, False) /* Create  (2031668) for Shop */
     , (1028707, 4, 1006797,  0, 0, 0, False) /* Create  (1006797) for Shop */
     , (1028707, 4, 1006800,  0, 0, 0, False) /* Create  (1006800) for Shop */
     , (1028707, 4, 1006802,  0, 0, 0, False) /* Create  (1006802) for Shop */
     , (1028707, 4, 1006804,  0, 0, 0, False) /* Create  (1006804) for Shop */
     , (1028707, 4, 42149813,  0, 0, 0, False) /* Create  (42149813) for Shop */
     , (1028707, 4, 1910618,  0, 0, 0, False) /* Create  (1910618) for Shop */
     , (1028707, 4, 1910619,  0, 0, 0, False) /* Create  (1910619) for Shop */
	 , (1028707, 4, 42130741,  0, 0, 0, False) /* Create  (42130741) for Shop */
     , (1028707, 4, 4200094,  0, 0, 0, False) /* Create  Spear of Winter Flame 2h (4200094) for Shop */
	 , (1028707, 4, 4200095,  0, 0, 0, False) /* Create  Harbinger Arm Guard (4200095) for Shop */
     , (1028707, 4, 4200096,  0, 0, 0, False) /* Create  Tracker Boots (4200096) for Shop */
     , (1028707, 4, 4200097,  0, 0, 0, False) /* Create  Footman's Boots (4200097) for Shop */
     , (1028707, 4, 4200098,  0, 0, 0, False) /* Create  Enhanced Robe of the Tundra (4200098) for Shop */
     , (1028707, 4, 4200099,  0, 0, 0, False) /* Create  Hearty Reedshark Robe (4200099) for Shop */
     , (1028707, 4, 4200100,  0, 0, 0, False) /* Create  Undead Sailor Mask (4200100) for Shop */
     , (1028707, 4, 4200101,  0, 0, 0, False) /* Create  Turquoise Winged Helmet (4200101) for Shop */
     , (1028707, 4, 4200102,  0, 0, 0, False) /* Create  Lustrous Winged Leggings (4200102) for Shop */
     , (1028707, 4, 4200103,  0, 0, 0, False) /* Create  Dusky Winged Coat (4200103) for Shop */
     , (1028707, 4, 4200104,  0, 0, 0, False) /* Create  Koji's Beast (4200104) for Shop */
     , (1028707, 4, 4200105,  0, 0, 0, False) /* Create  Veil of Darkness (4200105) for Shop */
     , (1028707, 4, 4200106,  0, 0, 0, False) /* Create  Royal Runed Partizan (4200106) for Shop */
     , (1028707, 4, 4200107,  0, 0, 0, False) /* Create  Assault Spear (4200107) for Shop */
     , (1028707, 4, 4200108,  0, 0, 0, False) /* Create  Auroch Horn Spear (4200108) for Shop */
     , (1028707, 4, 4200109,  0, 0, 0, False) /* Create  Banished Spear (4200109) for Shop */
     , (1028707, 4, 4200110,  0, 0, 0, False) /* Create  Battered Old Spear (4200110) for Shop */
     , (1028707, 4, 4200111,  0, 0, 0, False) /* Create  Bound Singularity Spear (4200111) for Shop */
     , (1028707, 4, 4200112,  0, 0, 0, False) /* Create  Burun Slaying Swordstaff (4200112) for Shop */
     , (1028707, 4, 4200113,  0, 0, 0, False) /* Create  Champion's Demise (4200113) for Shop */
     , (1028707, 4, 4200114,  0, 0, 0, False) /* Create  Chimeric Lance of the Quiddity (4200114) for Shop */
     , (1028707, 4, 4200115,  0, 0, 0, False) /* Create  Tibri's Fire Spear (4200115) for Shop */
     , (1028707, 4, 4200116,  0, 0, 0, False) /* Create  Kreerg's Spear (4200116) for Shop */
     , (1028707, 4, 4200117,  0, 0, 0, False) /* Create  Lance of the Quiddity (4200117) for Shop */
     , (1028707, 4, 4200118,  0, 0, 0, False) /* Create  Star of Gharu'n (4200118) for Shop */
     , (1028707, 4, 4200119,  0, 0, 0, False) /* Create  Spectral  Spear (4200119) for Shop */
	 , (1028707, 4, 4200120,  0, 0, 0, False) /* Create  Singularity Spear (4200120) for Shop */
	 , (1028707, 4, 4200121,  0, 0, 0, False) /* Create  Red Rune Silveran Spear (4200121) for Shop */
	 , (1028707, 4, 4200122,  0, 0, 0, False) /* Create  Pillar of Fearlessness (4200122) for Shop */
	 , (1028707, 4, 4200136,  0, 0, 0, False) /* Create  Aphus Sun Guard (4200136) for Shop */
     , (1028707, 4, 4200160,  0, 0, 0, False) /* Create  Assassin's Whisper (4200160) for Shop */
     , (1028707, 4, 4200161,  0, 0, 0, False) /* Create  Bloodmark Crossbow (4200161) for Shop */
     , (1028707, 4, 4200162,  0, 0, 0, False) /* Create  Iron Bull (4200162) for Shop */
     , (1028707, 4, 4200163,  0, 0, 0, False) /* Create  Zefir's Breath (4200163) for Shop */
     , (1028707, 4, 4200164,  0, 0, 0, False) /* Create  Wretched Crossbow (420064) for Shop */
     , (1028707, 4, 4200165,  0, 0, 0, False) /* Create  Feathered Razor (4200165) for Shop */
     , (1028707, 4, 4200166,  0, 0, 0, False) /* Create  Bone Crossbow (4200166) for Shop */
     , (1028707, 4, 4200167,  0, 0, 0, False) /* Create  Red Rune Silveran Crossbow (4200167) for Shop */
     , (1028707, 4, 4200168,  0, 0, 0, False) /* Create  Gear Crossbow Dark Grey (4200168) for Shop */
     , (1028707, 4, 4200169,  0, 0, 0, False) /* Create  Gear Crossbow Black (4200169) for Shop */
	 , (1028707, 4, 4200170,  0, 0, 0, False) /* Create  Flaming Skull (4200170) for Shop */
	 , (1028707, 4, 4200172,  0, 0, 0, False) /* Create  Aerbax's Defeat (4200172) for Shop */
	 , (1028707, 4, 4200173,  0, 0, 0, False) /* Create  Aerlinthe Cynosure (4200173) for Shop */;



/* Lifestoned Changelog:
{
  "LastModified": "2021-12-26T05:43:59.2082974Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2021-12-26T05:43:59.1767957Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
