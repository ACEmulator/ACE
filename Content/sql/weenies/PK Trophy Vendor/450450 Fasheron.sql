DELETE FROM `weenie` WHERE `class_Id` = 450450;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450450, 'ace450450-fasheron', 12, '2022-11-15 04:30:58') /* Vendor */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450450,   1,         16) /* ItemType - Creature */
     , (450450,   2,         51) /* CreatureType - Empyrean */
     , (450450,   6,         -1) /* ItemsCapacity */
     , (450450,   7,         -1) /* ContainersCapacity */
     , (450450,   8,        120) /* Mass */
     , (450450,  16,         32) /* ItemUseable - Remote */
     , (450450,  25,        275) /* Level */
     , (450450,  27,          0) /* ArmorType - None */
     , (450450,  74,     270496) /* MerchandiseItemTypes - Food, Misc, Writable, PromissoryNote */
     , (450450,  75,          0) /* MerchandiseMinValue */
     , (450450,  76,    1000000) /* MerchandiseMaxValue */
     , (450450,  93,    2098200) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity, ReportCollisionsAsEnvironment */
     , (450450, 113,          1) /* Gender - Male */
     , (450450, 126,        500) /* VendorHappyMean */
     , (450450, 127,        500) /* VendorHappyVariance */
     , (450450, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (450450, 134,         16) /* PlayerKillerStatus - RubberGlue */
     , (450450, 146,         97) /* XpOverride */
     , (450450, 188,          3) /* HeritageGroup - Sho */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450450,   1, True ) /* Stuck */
     , (450450,  12, True ) /* ReportCollisions */
     , (450450,  13, False) /* Ethereal */
     , (450450,  19, False) /* Attackable */
     , (450450,  39, True ) /* DealMagicalItems */
     , (450450,  41, True ) /* ReportCollisionsAsEnvironment */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450450,   1,       5) /* HeartbeatInterval */
     , (450450,   2,       0) /* HeartbeatTimestamp */
     , (450450,   3,    0.16) /* HealthRate */
     , (450450,   4,       5) /* StaminaRate */
     , (450450,   5,       1) /* ManaRate */
     , (450450,  11,     300) /* ResetInterval */
     , (450450,  13,     0.9) /* ArmorModVsSlash */
     , (450450,  14,       1) /* ArmorModVsPierce */
     , (450450,  15,     1.1) /* ArmorModVsBludgeon */
     , (450450,  16,     0.4) /* ArmorModVsCold */
     , (450450,  17,     0.4) /* ArmorModVsFire */
     , (450450,  18,       1) /* ArmorModVsAcid */
     , (450450,  19,     0.6) /* ArmorModVsElectric */
     , (450450,  37,       1) /* BuyPrice */
     , (450450,  38,       1) /* SellPrice */
     , (450450,  39,     1.6) /* DefaultScale */
     , (450450,  54,       5) /* UseRadius */
     , (450450,  64,       1) /* ResistSlash */
     , (450450,  65,       1) /* ResistPierce */
     , (450450,  66,       1) /* ResistBludgeon */
     , (450450,  67,       1) /* ResistFire */
     , (450450,  68,       1) /* ResistCold */
     , (450450,  69,       1) /* ResistAcid */
     , (450450,  70,       1) /* ResistElectric */
     , (450450,  71,       1) /* ResistHealthBoost */
     , (450450,  72,       1) /* ResistStaminaDrain */
     , (450450,  73,       1) /* ResistStaminaBoost */
     , (450450,  74,       1) /* ResistManaDrain */
     , (450450,  75,       1) /* ResistManaBoost */
     , (450450, 104,      10) /* ObviousRadarRange */
     , (450450, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450450,   1, 'Fasheron') /* Name */
     , (450450,   3, 'Male') /* Sex */
     , (450450,   4, 'Sho') /* HeritageGroup */
     , (450450,   5, 'PK Trophy Vendor') /* Template */
     , (450450,  24, 'Hebian-to') /* TownName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450450,   1, 0x020016CB) /* Setup */
     , (450450,   2, 0x0900010E) /* MotionTable */
     , (450450,   3, 0x20000001) /* SoundTable */
     , (450450,   8, 0x06002632) /* Icon */
     , (450450,   9, 0x0500111F) /* EyesTexture */
     , (450450,  10, 0x05001160) /* NoseTexture */
     , (450450,  11, 0x050011D1) /* MouthTexture */
     , (450450,  12, 0x0500024C) /* DefaultEyesTexture */
     , (450450,  13, 0x050002F5) /* DefaultNoseTexture */
     , (450450,  14, 0x0500025C) /* DefaultMouthTexture */
     , (450450,  15, 0x04001FC7) /* HairPalette */
     , (450450,  16, 0x040002BD) /* EyesPalette */
     , (450450,  17, 0x0400049F) /* SkinPalette */
     , (450450,  18, 0x010047FB) /* HeadObject */
     , (450450,  22, 0x34000004) /* PhysicsEffectTable */
     , (450450,  57,    1000002) /* AlternateCurrency - PK Trophy */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (450450,   1, 290, 0, 0) /* Strength */
     , (450450,   2, 290, 0, 0) /* Endurance */
     , (450450,   3, 200, 0, 0) /* Quickness */
     , (450450,   4, 200, 0, 0) /* Coordination */
     , (450450,   5, 290, 0, 0) /* Focus */
     , (450450,   6, 200, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (450450,   1,   310, 0, 0, 455) /* MaxHealth */
     , (450450,   3,   250, 0, 0, 540) /* MaxStamina */
     , (450450,   5,   240, 0, 0, 440) /* MaxMana */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (450450,  0,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (450450,  1,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (450450,  2,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (450450,  3,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (450450,  4,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (450450,  5,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (450450,  6,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (450450,  7,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (450450,  8,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,    0.6, NULL, NULL, NULL, NULL, 4 /* Buy */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Best looking outfit choice I''ve seen all day!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,    0.8, NULL, NULL, NULL, NULL, 2 /* Close */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Don''t let me catch that beautiful suit, laying around in the dirt', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,    0.6, NULL, NULL, NULL, NULL, 3 /* Sell */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'What am I suppose to do with all this junk', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,    0.8, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Welcome to my dressing room, hope you find an outfit you like!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,    0.4, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Welcome to my dressing room, hope you find an outfit you like!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,  0.125, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000087 /* Wave */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,   0.25, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x1300007D /* BowDeep */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,  0.375, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000086 /* Shrug */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,    0.5, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x13000083 /* Nod */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (450450, 4, 450531,  0, 0, 0, False) /* Create Gelidite Robe (450531) for Shop */
     , (450450, 4, 450789,  0, 0, 0, False) /* Create Asteliary Orb (450754) for Shop */
     , (450450, 4, 450009,  0, 0, 0, False) /* Create Gelidite Robe (450009) for Shop */
     , (450450, 4, 450010,  0, 0, 0, False) /* Create Vestiri Robe (450010) for Shop */
     , (450450, 4, 450011,  0, 0, 0, False) /* Create Festival Robe (450011) for Shop */
     , (450450, 4, 450012,  0, 0, 0, False) /* Create Legendary Robe of Utter Darkness (450012) for Shop */
     , (450450, 4, 450013,  0, 0, 0, False) /* Create Hoory Mattekar Over-robe (450013) for Shop */
     , (450450, 4, 450014,  0, 0, 0, False) /* Create Hoary Mattekar Over-robe (450014) for Shop */
     , (450450, 4, 450015,  0, 0, 0, False) /* Create Balor's Over-robe (450015) for Shop */
     , (450450, 4, 450016,  0, 0, 0, False) /* Create Empyrean Over-robe (450016) for Shop */
     , (450450, 4, 450017,  0, 0, 0, False) /* Create Vestiri Over-robe (450017) for Shop */
     , (450450, 4, 450018,  0, 0, 0, False) /* Create Suikan Over-robe (450018) for Shop */
     , (450450, 4, 450019,  0, 0, 0, False) /* Create Dho Vest and Over-Robe (450019) for Shop */
     , (450450, 4, 450706,  0, 0, 0, False) /* Create Radiant Blood Robe (450706) for Shop */
     , (450450, 4, 450708,  0, 0, 0, False) /* Create Eldrytch Web Robe (450708) for Shop */
     , (450450, 4, 450709,  0, 0, 0, False) /* Create Celestial Hand Robe (450709) for Shop */
     , (450450, 4, 450024,  0, 0, 0, False) /* Create Empowered Robe of the Perfect Light (450024) for Shop */
     , (450450, 4, 450025,  0, 0, 0, False) /* Create Empyrean Over-robe (450025) for Shop */
     , (450450, 4, 450026,  0, 0, 0, False) /* Create Colosseum Master's Robe (450026) for Shop */
     , (450450, 4, 450027,  0, 0, 0, False) /* Create Empowered Empyrean Robe (450027) for Shop */
     , (450450, 4, 450028,  0, 0, 0, False) /* Create Enhanced Robe of the Tundra (450028) for Shop */
     , (450450, 4, 450029,  0, 0, 0, False) /* Create Festival Robe (450029) for Shop */
     , (450450, 4, 450030,  0, 0, 0, False) /* Create Bathrobe of Ordinary Comfort (450030) for Shop */
     , (450450, 4, 450040,  0, 0, 0, False) /* Create Bathrobe (450040) for Shop */
     , (450450, 4, 450032,  0, 0, 0, False) /* Create Doppelganger Robe (450032) for Shop */
     , (450450, 4, 450033,  0, 0, 0, False) /* Create Marksman's Robe (450033) for Shop */
     , (450450, 4, 450034,  0, 0, 0, False) /* Create Armsman's Robe (450034) for Shop */
     , (450450, 4, 450035,  0, 0, 0, False) /* Create Enscorcelled Robe (450035) for Shop */
     , (450450, 4, 450036,  0, 0, 0, False) /* Create Vestiri Robe with Hood (450036) for Shop */
     , (450450, 4, 450037,  0, 0, 0, False) /* Create Canescent Mattekar Robe (450037) for Shop */
     , (450450, 4, 450038,  0, 0, 0, False) /* Create Canescent Mattekar Robe (450038) for Shop */
     , (450450, 4, 450039,  0, 0, 0, False) /* Create Nuhumudira's Robe (450039) for Shop */
     , (450450, 4, 450031,  0, 0, 0, False) /* Create Luminous Robe (450031) for Shop */
     , (450450, 4, 450041,  0, 0, 0, False) /* Create Envoy's Robe (450041) for Shop */
     , (450450, 4, 450042,  0, 0, 0, False) /* Create Plaguefang's Robe (450042) for Shop */
     , (450450, 4, 450043,  0, 0, 0, False) /* Create Sturdy Reedshark Robe (450043) for Shop */
     , (450450, 4, 450044,  0, 0, 0, False) /* Create Hearty Reedshark Robe (450044) for Shop */
     , (450450, 4, 450045,  0, 0, 0, False) /* Create Robe of the Tundra (450045) for Shop */
     , (450450, 4, 450046,  0, 0, 0, False) /* Create Swarthy Mattekar Robe (450046) for Shop */
     , (450450, 4, 450047,  0, 0, 0, False) /* Create Empyrean Robe (450047) for Shop */
     , (450450, 4, 450048,  0, 0, 0, False) /* Create Martine's Robe (450048) for Shop */
     , (450450, 4, 450049,  0, 0, 0, False) /* Create Elemental Master Robe (450049) for Shop */
     , (450450, 4, 450050,  0, 0, 0, False) /* Create Elemental Master Robe (450050) for Shop */
     , (450450, 4, 450051,  0, 0, 0, False) /* Create Robe of the Tundra (450051) for Shop */
     , (450450, 4, 450052,  0, 0, 0, False) /* Create Canescent Mattekar Robe (450052) for Shop */
     , (450450, 4, 450053,  0, 0, 0, False) /* Create Canescent Mattekar Robe (450053) for Shop */
     , (450450, 4, 1005851,  0, 0, 0, False) /* Create Faran Robe with Hood (1005851) for Shop */
     , (450450, 4, 450761,  0, 0, 0, False) /* Create Virindi Shroud (450761) for Shop */
     , (450450, 4, 450055,  0, 0, 0, False) /* Create Kiyafa Robe (450055) for Shop */
     , (450450, 4, 450540,  0, 0, 0, False) /* Create Yifan Dress (450540) for Shop */
     , (450450, 4, 450541,  0, 0, 0, False) /* Create Sleek Dress (450541) for Shop */
     , (450450, 4, 450543,  0, 0, 0, False) /* Create Badlands Siraluun Dress (450543) for Shop */
     , (450450, 4, 450544,  0, 0, 0, False) /* Create Kithless Siraluun Dress (450544) for Shop */
     , (450450, 4, 450545,  0, 0, 0, False) /* Create Littoral Siraluun Dress (450545) for Shop */
     , (450450, 4, 450546,  0, 0, 0, False) /* Create Strand Siraluun Dress (450546) for Shop */
     , (450450, 4, 450547,  0, 0, 0, False) /* Create Tidal Siraluun Dress (450547) for Shop */
     , (450450, 4, 450548,  0, 0, 0, False) /* Create Timber Siraluun Dress (450548) for Shop */
     , (450450, 4, 450549,  0, 0, 0, False) /* Create Untamed Siraluun Dress (450549) for Shop */
     , (450450, 4, 450058,  0, 0, 0, False) /* Create Rynthid Energy Tentacles (450058) for Shop */
     , (450450, 4, 450059,  0, 0, 0, False) /* Create Rynthid Energy Field (450059) for Shop */
     , (450450, 4, 450060,  0, 0, 0, False) /* Create Royal Knight Cloak (450060) for Shop */
     , (450450, 4, 450061,  0, 0, 0, False) /* Create Mukkir Wings (450061) for Shop */
     , (450450, 4, 450062,  0, 0, 0, False) /* Create Blackened House Mhoire Cloak (450062) for Shop */
     , (450450, 4, 450063,  0, 0, 0, False) /* Create Strathelar Royal Cloak (450063) for Shop */
     , (450450, 4, 450064,  0, 0, 0, False) /* Create House Mhoire Cloak (450064) for Shop */
     , (450450, 4, 450065,  0, 0, 0, False) /* Create Creeping Blight Cloak (450065) for Shop */
     , (450450, 4, 450703,  0, 0, 0, False) /* Create Cloak (450703) for Shop */
     , (450450, 4, 450704,  0, 0, 0, False) /* Create Cloak (450704) for Shop */
     , (450450, 4, 450705,  0, 0, 0, False) /* Create Cloak (450705) for Shop */
     , (450450, 4, 450214,  0, 0, 0, False) /* Create Eye of Muramm (450214) for Shop */
     , (450450, 4, 450215,  0, 0, 0, False) /* Create Wand of the Frore Crystal (450215) for Shop */
     , (450450, 4, 450216,  0, 0, 0, False) /* Create Orb of the Ironsea (450216) for Shop */
     , (450450, 4, 450217,  0, 0, 0, False) /* Create Wings of Rakhil (450217) for Shop */
     , (450450, 4, 450218,  0, 0, 0, False) /* Create Deru Limb (450218) for Shop */
     , (450450, 4, 450222,  0, 0, 0, False) /* Create Heart of Darkest Flame (450222) for Shop */
     , (450450, 4, 450223,  0, 0, 0, False) /* Create Orb of Black Fire (450223) for Shop */
     , (450450, 4, 450224,  0, 0, 0, False) /* Create Wand of Black Fire (450224) for Shop */
     , (450450, 4, 450225,  0, 0, 0, False) /* Create Essence Flare (450225) for Shop */
     , (450450, 4, 450226,  0, 0, 0, False) /* Create Essence Flicker (450226) for Shop */
     , (450450, 4, 450227,  0, 0, 0, False) /* Create Darker Heart (450227) for Shop */
     , (450450, 4, 450228,  0, 0, 0, False) /* Create Orb of Splendor (450228) for Shop */
     , (450450, 4, 450229,  0, 0, 0, False) /* Create Casting Jack o' Lantern (450229) for Shop */
     , (450450, 4, 450230,  0, 0, 0, False) /* Create Casting Jack o' Lantern (450230) for Shop */
     , (450450, 4, 450231,  0, 0, 0, False) /* Create Casting Jack o' Lantern (450231) for Shop */
     , (450450, 4, 450232,  0, 0, 0, False) /* Create Idol of the Recluse (450232) for Shop */
     , (450450, 4, 450233,  0, 0, 0, False) /* Create Legendary Seed of Mornings (450233) for Shop */
     , (450450, 4, 450234,  0, 0, 0, False) /* Create Legendary Seed of Harvests (450234) for Shop */
     , (450450, 4, 450235,  0, 0, 0, False) /* Create Legendary Seed of Twilight (450235) for Shop */
     , (450450, 4, 450237,  0, 0, 0, False) /* Create Delicate Bloodstone Wand (450237) for Shop */
     , (450450, 4, 450238,  0, 0, 0, False) /* Create Blue Anniversary Sparkler (450238) for Shop */
     , (450450, 4, 450239,  0, 0, 0, False) /* Create Green Anniversary Sparkler (450239) for Shop */
     , (450450, 4, 450240,  0, 0, 0, False) /* Create Orange Anniversary Sparkler (450240) for Shop */
     , (450450, 4, 450241,  0, 0, 0, False) /* Create Purple Anniversary Sparkler (450241) for Shop */
     , (450450, 4, 450242,  0, 0, 0, False) /* Create Red Anniversary Sparkler (450242) for Shop */
     , (450450, 4, 450243,  0, 0, 0, False) /* Create White Anniversary Sparkler (450243) for Shop */
     , (450450, 4, 450244,  0, 0, 0, False) /* Create Yellow Anniversary Sparkler (450244) for Shop */
     , (450450, 4, 450245,  0, 0, 0, False) /* Create Orb of the Bunny Booty (450245) for Shop */
     , (450450, 4, 450250,  0, 0, 0, False) /* Create Orb of the Baby Bunny Booty (450250) for Shop */
     , (450450, 4, 450247,  0, 0, 0, False) /* Create Head of the Homunculus (450247) for Shop */
     , (450450, 4, 450248,  0, 0, 0, False) /* Create Head of the Homunculus (450248) for Shop */
     , (450450, 4, 450249,  0, 0, 0, False) /* Create Detailed Mukkir Orb (450249) for Shop */
     , (450450, 4, 450251,  0, 0, 0, False) /* Create Finger of the Harbinger (450251) for Shop */
     , (450450, 4, 450252,  0, 0, 0, False) /* Create Orb of Eternal Frost (450252) for Shop */
     , (450450, 4, 450253,  0, 0, 0, False) /* Create Stormwood Wand (450253) for Shop */
     , (450450, 4, 450254,  0, 0, 0, False) /* Create Corrupted Heartwood Wand (450254) for Shop */
     , (450450, 4, 450255,  0, 0, 0, False) /* Create Chimeric Eye of the Quiddity (450255) for Shop */
     , (450450, 4, 450316,  0, 0, 0, False) /* Create Eye of the Quiddity (450316) for Shop */
     , (450450, 4, 450550,  0, 0, 0, False) /* Create Assault Orb (450550) for Shop */
     , (450450, 4, 1010731,  0, 0, 0, False) /* Create Quiddity Orb (1010731) for Shop */
     , (450450, 4, 1009047,  0, 0, 0, False) /* Create Globe of Auberean (1009047) for Shop */
     , (450450, 4, 1009064,  0, 0, 0, False) /* Create Hieromancer's Orb (1009064) for Shop */
     , (450450, 4, 1022080,  0, 0, 0, False) /* Create Impious Staff (1022080) for Shop */
     , (450450, 4, 1022256,  0, 0, 0, False) /* Create Fishing Pole (1022256) for Shop */
     , (450450, 4, 1023774,  0, 0, 0, False) /* Create Casting Stein (1023774) for Shop */
     , (450450, 4, 1025703,  0, 0, 0, False) /* Create Dapper Suit (1025703) for Shop */
     , (450450, 4, 450710,  0, 0, 0, False) /* Create Apron (450710) for Shop */
     , (450450, 4, 1025895,  0, 0, 0, False) /* Create Puppeteer's Skull (1025895) for Shop */
     , (450450, 4, 1027839,  0, 0, 0, False) /* Create Ultimate Singularity Scepter of War Magic (1027839) for Shop */
     , (450450, 4, 1027840,  0, 0, 0, False) /* Create Singularity Scepter of War Magic (1027840) for Shop */
     , (450450, 4, 1027841,  0, 0, 0, False) /* Create Bound Singularity Scepter of War Magic (1027841) for Shop */
     , (450450, 4, 1030872,  0, 0, 0, False) /* Create Eye of the Fallen (1030872) for Shop */
     , (450450, 4, 1031330,  0, 0, 0, False) /* Create Scribe's Quill (1031330) for Shop */
     , (450450, 4, 1031331,  0, 0, 0, False) /* Create Scribe's Quill (1031331) for Shop */
     , (450450, 4, 1031332,  0, 0, 0, False) /* Create Scribe's Quill (1031332) for Shop */
     , (450450, 4, 1031333,  0, 0, 0, False) /* Create Scribe's Quill (1031333) for Shop */
     , (450450, 4, 1036229,  0, 0, 0, False) /* Create Rift Orb (1036229) for Shop */
     , (450450, 4, 1036230,  0, 0, 0, False) /* Create Rift Orb (1036230) for Shop */
     , (450450, 4, 1051989,  0, 0, 0, False) /* Create Rynthid Tentacle Wand (1051989) for Shop */
     , (450450, 4, 1052444,  0, 0, 0, False) /* Create Holiday Present (1052444) for Shop */
     , (450450, 4, 1052514,  0, 0, 0, False) /* Create Painter's Palette (1052514) for Shop */
     , (450450, 4, 1052699,  0, 0, 0, False) /* Create Wooden Top (1052699) for Shop */
     , (450450, 4, 4200170,  0, 0, 0, False) /* Create Flaming Skull (4200170) for Shop */
     , (450450, 4, 4200172,  0, 0, 0, False) /* Create Aerbax's Defeat (4200172) for Shop */
     , (450450, 4, 4200173,  0, 0, 0, False) /* Create Aerlinthe Cynosure (4200173) for Shop */
     , (450450, 4, 4200171,  0, 0, 0, False) /* Create  (4200171) for Shop */
     , (450450, 4, 450079,  0, 0, 0, False) /* Create Shendolain Crystal Shield (450079) for Shop */
     , (450450, 4, 450080,  0, 0, 0, False) /* Create Imbued Shield of the Simulacra (450080) for Shop */
     , (450450, 4, 450081,  0, 0, 0, False) /* Create Bandit Shield (450081) for Shop */
     , (450450, 4, 450082,  0, 0, 0, False) /* Create Diamond Shield (450082) for Shop */
     , (450450, 4, 450083,  0, 0, 0, False) /* Create Greater Olthoi Shield (450083) for Shop */
     , (450450, 4, 450084,  0, 0, 0, False) /* Create Shield of Power (450084) for Shop */
     , (450450, 4, 450085,  0, 0, 0, False) /* Create Nefane Shield (450085) for Shop */
     , (450450, 4, 450086,  0, 0, 0, False) /* Create Envoy's Shield (450086) for Shop */
     , (450450, 4, 450087,  0, 0, 0, False) /* Create Chorizite Veined Shield (450087) for Shop */
     , (450450, 4, 450088,  0, 0, 0, False) /* Create Kul'dir (450088) for Shop */
     , (450450, 4, 450089,  0, 0, 0, False) /* Create Kam'shir (450089) for Shop */
     , (450450, 4, 450090,  0, 0, 0, False) /* Create Dread Marauder Shield (450090) for Shop */
     , (450450, 4, 1030372,  0, 0, 0, False) /* Create Shield of Engorgement (1030372) for Shop */
     , (450450, 4, 450212,  0, 0, 0, False) /* Create Twin Ward (450212) for Shop */
     , (450450, 4, 450213,  0, 0, 0, False) /* Create Mirrored Justice (450213) for Shop */
     , (450450, 4, 450091,  0, 0, 0, False) /* Create Doppelganger Shield (450091) for Shop */
     , (450450, 4, 450092,  0, 0, 0, False) /* Create Squalid Shield (450092) for Shop */
     , (450450, 4, 450093,  0, 0, 0, False) /* Create Shield of Yanshi (450093) for Shop */
     , (450450, 4, 450094,  0, 0, 0, False) /* Create Shield of Elysa's Royal Guard (450094) for Shop */
     , (450450, 4, 450096,  0, 0, 0, False) /* Create Shield of Sanamar (450096) for Shop */
     , (450450, 4, 450097,  0, 0, 0, False) /* Create Shield of Silyun (450097) for Shop */
     , (450450, 4, 450105,  0, 0, 0, False) /* Create Shield of Borelean's Royal Guard (450105) for Shop */
     , (450450, 4, 450099,  0, 0, 0, False) /* Create Prismatic Diamond Shield (450099) for Shop */
     , (450450, 4, 450377,  0, 0, 0, False) /* Create Shield of Truth (450377) for Shop */
     , (450450, 4, 450102,  0, 0, 0, False) /* Create Paradox-touched Olthoi Shield (450102) for Shop */
     , (450450, 4, 450101,  0, 0, 0, False) /* Create Celestial Hand Shield (450101) for Shop */
     , (450450, 4, 450103,  0, 0, 0, False) /* Create Eldrytch Web Shield (450103) for Shop */
     , (450450, 4, 450104,  0, 0, 0, False) /* Create Radiant Blood Shield (450104) for Shop */
     , (450450, 4, 450108,  0, 0, 0, False) /* Create Shield of the Gold Gear (450108) for Shop */
     , (450450, 4, 450367,  0, 0, 0, False) /* Create Sanguinary Aegis (450367) for Shop */
     , (450450, 4, 450369,  0, 0, 0, False) /* Create Sanguinary Aegis (450369) for Shop */
     , (450450, 4, 450370,  0, 0, 0, False) /* Create Sanguinary Aegis (450370) for Shop */
     , (450450, 4, 450371,  0, 0, 0, False) /* Create Sanguinary Aegis (450371) for Shop */
     , (450450, 4, 450372,  0, 0, 0, False) /* Create Sanguinary Aegis (450372) for Shop */
     , (450450, 4, 450373,  0, 0, 0, False) /* Create Corrupted Aegis (450373) for Shop */
     , (450450, 4, 450374,  0, 0, 0, False) /* Create Raven Aegis (450374) for Shop */
     , (450450, 4, 450376,  0, 0, 0, False) /* Create Caliginous Aegis (450376) for Shop */
     , (450450, 4, 450375,  0, 0, 0, False) /* Create T'thuun Aegis (450375) for Shop */
     , (450450, 4, 1035982,  0, 0, 0, False) /* Create Aegis of the Golden Flame (1035982) for Shop */
     , (450450, 4, 1036227,  0, 0, 0, False) /* Create Coral Shield (1036227) for Shop */
     , (450450, 4, 1036228,  0, 0, 0, False) /* Create Coral Shield (1036228) for Shop */
     , (450450, 4, 450764,  0, 0, 0, False) /* Create Shield of Isin Dule (450764) for Shop */
     , (450450, 4, 1036254,  0, 0, 0, False) /* Create  (1036254) for Shop */
     , (450450, 4, 1038922,  0, 0, 0, False) /* Create T'thuun Shield (1038922) for Shop */
     , (450450, 4, 1043130,  0, 0, 0, False) /* Create Iron Blade Aegis (1043130) for Shop */
     , (450450, 4, 1043131,  0, 0, 0, False) /* Create Iron Blade Shield (1043131) for Shop */
     , (450450, 4, 1043141,  0, 0, 0, False) /* Create Aegis of the Gold Gear (1043141) for Shop */
     , (450450, 4, 450185,  0, 0, 0, False) /* Create Ebonwood Shortbow (450185) for Shop */
     , (450450, 4, 1030303,  0, 0, 0, False) /* Create Serpent's Flight (1030303) for Shop */
     , (450450, 4, 1030351,  0, 0, 0, False) /* Create Dragonspine Bow (1030351) for Shop */
     , (450450, 4, 1030304,  0, 0, 0, False) /* Create Black Cloud Bow (1030304) for Shop */
     , (450450, 4, 450211,  0, 0, 0, False) /* Create Corsair's Arc (450211) for Shop */
     , (450450, 4, 4200160,  0, 0, 0, False) /* Create Assassin's Whisper (4200160) for Shop */
     , (450450, 4, 4200161,  0, 0, 0, False) /* Create Bloodmark Crossbow (4200161) for Shop */
     , (450450, 4, 4200162,  0, 0, 0, False) /* Create Iron Bull (4200162) for Shop */
     , (450450, 4, 4200163,  0, 0, 0, False) /* Create Zefir's Breath (4200163) for Shop */
     , (450450, 4, 4200165,  0, 0, 0, False) /* Create Feathered Razor (4200165) for Shop */
     , (450450, 4, 450210,  0, 0, 0, False) /* Create Drifter's Atlatl (450210) for Shop */
     , (450450, 4, 450186,  0, 0, 0, False) /* Create Ridgeback Dagger (450186) for Shop */
     , (450450, 4, 450187,  0, 0, 0, False) /* Create Zharalim Crookblade (450187) for Shop */
     , (450450, 4, 450190,  0, 0, 0, False) /* Create Black Thistle (450190) for Shop */
     , (450450, 4, 450191,  0, 0, 0, False) /* Create Moriharu's Kitchen Knife (450191) for Shop */
     , (450450, 4, 450192,  0, 0, 0, False) /* Create Pitfighter's Edge (450192) for Shop */
     , (450450, 4, 450188,  0, 0, 0, False) /* Create Star of Tukal (450188) for Shop */
     , (450450, 4, 450189,  0, 0, 0, False) /* Create Subjugator (450189) for Shop */
     , (450450, 4, 1030313,  0, 0, 0, False) /* Create Dripping Death (1030313) for Shop */
     , (450450, 4, 1030339,  0, 0, 0, False) /* Create Thunderhead (1030339) for Shop */
     , (450450, 4, 1030323,  0, 0, 0, False) /* Create Tri-Blade Spear (1030323) for Shop */
     , (450450, 4, 4200122,  0, 0, 0, False) /* Create Pillar of Fearlessness (4200122) for Shop */
     , (450450, 4, 4200118,  0, 0, 0, False) /* Create Star of Gharu'n (4200118) for Shop */
     , (450450, 4, 450193,  0, 0, 0, False) /* Create Squire's Glaive (450193) for Shop */
     , (450450, 4, 450194,  0, 0, 0, False) /* Create Staff of All Aspects (450194) for Shop */
     , (450450, 4, 450195,  0, 0, 0, False) /* Create Death's Grip Staff (450195) for Shop */
     , (450450, 4, 450196,  0, 0, 0, False) /* Create Staff of Fettered Souls (450196) for Shop */
     , (450450, 4, 450197,  0, 0, 0, False) /* Create Spirit Shifting Staff (450197) for Shop */
     , (450450, 4, 450198,  0, 0, 0, False) /* Create Staff of Tendrils (450198) for Shop */
     , (450450, 4, 450199,  0, 0, 0, False) /* Create Defiler of Milantos (450199) for Shop */
     , (450450, 4, 450200,  0, 0, 0, False) /* Create Desert Wyrm (450200) for Shop */
     , (450450, 4, 1030332,  0, 0, 0, False) /* Create Guardian of Pwyll (1030332) for Shop */
     , (450450, 4, 450201,  0, 0, 0, False) /* Create Morrigan's Vanity (450201) for Shop */
     , (450450, 4, 450202,  0, 0, 0, False) /* Create Fist of Three Principles (450202) for Shop */
     , (450450, 4, 450203,  0, 0, 0, False) /* Create Hevelio's Half-Moon (450203) for Shop */
     , (450450, 4, 450204,  0, 0, 0, False) /* Create Malachite Slasher (450204) for Shop */
     , (450450, 4, 450205,  0, 0, 0, False) /* Create Skullpuncher (450205) for Shop */
     , (450450, 4, 450206,  0, 0, 0, False) /* Create Steel Butterfly (450206) for Shop */
     , (450450, 4, 450207,  0, 0, 0, False) /* Create Bearded Axe of Souia-Vey (450207) for Shop */
     , (450450, 4, 450208,  0, 0, 0, False) /* Create Canfield Cleaver (450208) for Shop */
     , (450450, 4, 450209,  0, 0, 0, False) /* Create Tusked Axe of Ayan Baqur (450209) for Shop */
     , (450450, 4, 1030342,  0, 0, 0, False) /* Create Count Renari's Equalizer (1030342) for Shop */
     , (450450, 4, 1030343,  0, 0, 0, False) /* Create Smite (1030343) for Shop */
     , (450450, 4, 450221,  0, 0, 0, False) /* Create Decapitator's Blade (450221) for Shop */
     , (450450, 4, 1042662,  0, 0, 0, False) /* Create Chitin Cracker (1042662) for Shop */
     , (450450, 4, 1042663,  0, 0, 0, False) /* Create Revenant's Scythe (1042663) for Shop */
     , (450450, 4, 450219,  0, 0, 0, False) /* Create Spear of Lost Truths (450219) for Shop */
     , (450450, 4, 4200113,  0, 0, 0, False) /* Create Champion's Demise (4200113) for Shop */
     , (450450, 4, 450220,  0, 0, 0, False) /* Create Itaka's Naginata (450220) for Shop */
     , (450450, 4, 1008473,  0, 0, 0, False) /* Create Fine Spine Axe (1008473) for Shop */
     , (450450, 4, 1026593,  0, 0, 0, False) /* Create Sickle of Writhing Fury (1026593) for Shop */
     , (450450, 4, 1032499,  0, 0, 0, False) /* Create Axe of Winter Flame (1032499) for Shop */
     , (450450, 4, 1024557,  0, 0, 0, False) /* Create Quadruple-bladed Axe (1024557) for Shop */
     , (450450, 4, 1030866,  0, 0, 0, False) /* Create Hammer of the Fallen (1030866) for Shop */
     , (450450, 4, 1035547,  0, 0, 0, False) /* Create Doom Hammer (1035547) for Shop */
     , (450450, 4, 1034024,  0, 0, 0, False) /* Create Silifi of Crimson Night (1034024) for Shop */
     , (450450, 4, 1028490,  0, 0, 0, False) /* Create Noble War Maul (1028490) for Shop */
     , (450450, 4, 1008788,  0, 0, 0, False) /* Create Obsidian Dagger (1008788) for Shop */
     , (450450, 4, 1023536,  0, 0, 0, False) /* Create Fetid Dirk (1023536) for Shop */
     , (450450, 4, 1026031,  0, 0, 0, False) /* Create Bone Dagger (1026031) for Shop */
     , (450450, 4, 1025904,  0, 0, 0, False) /* Create Atakir's Blade (1025904) for Shop */
     , (450450, 4, 1028218,  0, 0, 0, False) /* Create Sable Tooth Dirk (1028218) for Shop */
     , (450450, 4, 1043046,  0, 0, 0, False) /* Create Paradox-touched Olthoi Dagger (1043046) for Shop */
     , (450450, 4, 1008799,  0, 0, 0, False) /* Create Great Work Staff of the Lightbringer (1008799) for Shop */
     , (450450, 4, 1011329,  0, 0, 0, False) /* Create Aun Tanua's War Taiaha (1011329) for Shop */
     , (450450, 4, 1011431,  0, 0, 0, False) /* Create Audetaunga's Taiaha of the Mountains (1011431) for Shop */
     , (450450, 4, 1020227,  0, 0, 0, False) /* Create Iasparailaun (1020227) for Shop */
     , (450450, 4, 1026599,  0, 0, 0, False) /* Create Esorcelled Falchion (1026599) for Shop */
     , (450450, 4, 1028067,  0, 0, 0, False) /* Create Superior Ashbane (1028067) for Shop */
     , (450450, 4, 1023522,  0, 0, 0, False) /* Create Overlord's Sword (1023522) for Shop */
     , (450450, 4, 1035804,  0, 0, 0, False) /* Create Demon Swarm Sword (1035804) for Shop */
     , (450450, 4, 1035949,  0, 0, 0, False) /* Create Tusker Bone Sword (1035949) for Shop */
     , (450450, 4, 450595,  0, 0, 0, False) /* Create Sword of Lost Light (450595) for Shop */
     , (450450, 4, 450596,  0, 0, 0, False) /* Create Sword of Lost Hope (450596) for Shop */
     , (450450, 4, 1040089,  0, 0, 0, False) /* Create Empowered Sword of Lost Hope (1040089) for Shop */
     , (450450, 4, 1040517,  0, 0, 0, False) /* Create Olthoibane Sword of Lost Light (1040517) for Shop */
     , (450450, 4, 1040519,  0, 0, 0, False) /* Create Skeletonbane Sword of Lost Light (1040519) for Shop */
     , (450450, 4, 1035297,  0, 0, 0, False) /* Create Greatsword of Flame (1035297) for Shop */
     , (450450, 4, 1031291,  0, 0, 0, False) /* Create Renlen's Grace (1031291) for Shop */
     , (450450, 4, 10352971,  0, 0, 0, False) /* Create Greatsword of Frost (10352971) for Shop */
     , (450450, 4, 10352972,  0, 0, 0, False) /* Create Greatsword of Lightning (10352972) for Shop */
     , (450450, 4, 10352973,  0, 0, 0, False) /* Create Greatsword of Acid (10352973) for Shop */
     , (450450, 4, 10352974,  0, 0, 0, False) /* Create Greatsword of Slashing and Piercing (10352974) for Shop */
     , (450450, 4, 10416111,  0, 0, 0, False) /* Create Greatsword of Iron Flame (10416111) for Shop */
     , (450450, 4, 10416112,  0, 0, 0, False) /* Create Greatsword of Iron Frost (10416112) for Shop */
     , (450450, 4, 10416113,  0, 0, 0, False) /* Create Greatsword of Iron Lightning (10416113) for Shop */
     , (450450, 4, 10416114,  0, 0, 0, False) /* Create Greatsword of Iron Acid (10416114) for Shop */
     , (450450, 4, 10416115,  0, 0, 0, False) /* Create Greatsword of Iron Slashing and Piercing (10416115) for Shop */
     , (450450, 4, 10416116,  0, 0, 0, False) /* Create Greatsword of Iron Bludgeoning (10416116) for Shop */
     , (450450, 4, 1024028,  0, 0, 0, False) /* Create Crescent Moons (1024028) for Shop */
     , (450450, 4, 1023538,  0, 0, 0, False) /* Create Basalt Blade (1023538) for Shop */
     , (450450, 4, 1030862,  0, 0, 0, False) /* Create Banished Nekode (1030862) for Shop */
     , (450450, 4, 1038910,  0, 0, 0, False) /* Create Blighted Claw (1038910) for Shop */
     , (450450, 4, 4200137,  0, 0, 0, False) /* Create Hamud's Pyreal Katar (4200137) for Shop */
	 , (450450, 4, 450791,  0, 0, 0, False) /* Create Paradox-touched Olthoi Wand (450765) for Shop */
     , (450450, 4, 1025905,  0, 0, 0, False) /* Create Needletooth (1025905) for Shop */
     , (450450, 4, 1023547,  0, 0, 0, False) /* Create Fang Mace (1023547) for Shop */
     , (450450, 4, 1024027,  0, 0, 0, False) /* Create Scepter of Thunderous Might (1024027) for Shop */
     , (450450, 4, 1025906,  0, 0, 0, False) /* Create Mace of Dissonance (1025906) for Shop */
     , (450450, 4, 1029910,  0, 0, 0, False) /* Create Marsh Siraluun Waaika (1029910) for Shop */
     , (450450, 4, 1030860,  0, 0, 0, False) /* Create Banished Mace (1030860) for Shop */
     , (450450, 4, 1031500,  0, 0, 0, False) /* Create Worn Old Mace (1031500) for Shop */
     , (450450, 4, 1035407,  0, 0, 0, False) /* Create Burnja's Board with Nails (1035407) for Shop */
     , (450450, 4, 1038926,  0, 0, 0, False) /* Create T'thuun Mace (1038926) for Shop */
     , (450450, 4, 1032773,  0, 0, 0, False) /* Create Spinning Staff of Death (1032773) for Shop */
     , (450450, 4, 1030634,  0, 0, 0, False) /* Create Cyphis Suldow's Half Moon Spear (1030634) for Shop */
     , (450450, 4, 1023539,  0, 0, 0, False) /* Create Serpent's Fang (1023539) for Shop */
     , (450450, 4, 1035615,  0, 0, 0, False) /* Create Blessed Spear of the Mosswart Gods (1035615) for Shop */
     , (450450, 4, 4200094,  0, 0, 0, False) /* Create Spear of Winter Flame (4200094) for Shop */
     , (450450, 4, 4200106,  0, 0, 0, False) /* Create Royal Runed Partizan (4200106) for Shop */
     , (450450, 4, 4200108,  0, 0, 0, False) /* Create Auroch Horn Spear (4200108) for Shop */
     , (450450, 4, 4200109,  0, 0, 0, False) /* Create Banished Spear (4200109) for Shop */
     , (450450, 4, 4200110,  0, 0, 0, False) /* Create Battered Old Spear (4200110) for Shop */
     , (450450, 4, 4200112,  0, 0, 0, False) /* Create Burun Slaying Swordstaff (4200112) for Shop */
     , (450450, 4, 4200115,  0, 0, 0, False) /* Create Tibri's Fire Spear (4200115) for Shop */
     , (450450, 4, 4200116,  0, 0, 0, False) /* Create Kreerg's Spear (4200116) for Shop */
     , (450450, 4, 4200119,  0, 0, 0, False) /* Create Spectral  Spear (4200119) for Shop */
     , (450450, 4, 1042932,  0, 0, 0, False) /* Create Well-Balanced Lugian Greataxe (1042932) for Shop */
     , (450450, 4, 4200166,  0, 0, 0, False) /* Create Bone Crossbow (4200166) for Shop */
     , (450450, 4, 4200168,  0, 0, 0, False) /* Create Gear Crossbow (4200168) for Shop */
     , (450450, 4, 4200169,  0, 0, 0, False) /* Create Gear Crossbow (4200169) for Shop */
     , (450450, 4, 4200164,  0, 0, 0, False) /* Create Wretched Crossbow (4200164) for Shop */
     , (450450, 4, 1038923,  0, 0, 0, False) /* Create T'thuun Bow (1038923) for Shop */
     , (450450, 4, 1033050,  0, 0, 0, False) /* Create Red Rune Silveran Dagger (1033050) for Shop */
     , (450450, 4, 1033058,  0, 0, 0, False) /* Create Red Rune Silveran Axe (1033058) for Shop */
     , (450450, 4, 1033102,  0, 0, 0, False) /* Create Red Rune Slashing Silveran Wand (1033102) for Shop */
     , (450450, 4, 1033121,  0, 0, 0, False) /* Create Red Rune Silveran Crossbow (1033121) for Shop */
     , (450450, 4, 450551,  0, 0, 0, False) /* Create Red Rune Silveran Sword (450551) for Shop */
     , (450450, 4, 450552,  0, 0, 0, False) /* Create Red Rune Silveran Spear (450552) for Shop */
     , (450450, 4, 450553,  0, 0, 0, False) /* Create Red Rune Silveran Staff (450553) for Shop */
     , (450450, 4, 450554,  0, 0, 0, False) /* Create Red Rune Silveran Atlatl (450554) for Shop */
     , (450450, 4, 450555,  0, 0, 0, False) /* Create Red Rune Silveran Bow (450555) for Shop */
     , (450450, 4, 450556,  0, 0, 0, False) /* Create Red Rune Silveran Claw (450556) for Shop */
     , (450450, 4, 450557,  0, 0, 0, False) /* Create Red Rune Silveran Greatsword (450557) for Shop */
     , (450450, 4, 450558,  0, 0, 0, False) /* Create Red Rune Silveran Mace (450558) for Shop */
     , (450450, 4, 450301,  0, 0, 0, False) /* Create Stormwood Atlatl (450301) for Shop */
     , (450450, 4, 450302,  0, 0, 0, False) /* Create Stormwood Axe (450302) for Shop */
     , (450450, 4, 450303,  0, 0, 0, False) /* Create Stormwood Bow (450303) for Shop */
     , (450450, 4, 450304,  0, 0, 0, False) /* Create Stormwood Crossbow (450304) for Shop */
     , (450450, 4, 450305,  0, 0, 0, False) /* Create Stormwood Mace (450305) for Shop */
     , (450450, 4, 450306,  0, 0, 0, False) /* Create Stormwood Spear (450306) for Shop */
     , (450450, 4, 450307,  0, 0, 0, False) /* Create Stormwood Staff (450307) for Shop */
     , (450450, 4, 450310,  0, 0, 0, False) /* Create Stormwood Claw (450310) for Shop */
     , (450450, 4, 1037579,  0, 0, 0, False) /* Create Soul Bound Crossbow (1037579) for Shop */
     , (450450, 4, 1037585,  0, 0, 0, False) /* Create Soul Bound Staff (1037585) for Shop */
     , (450450, 4, 450311,  0, 0, 0, False) /* Create Axe of the Quiddity (450311) for Shop */
     , (450450, 4, 450312,  0, 0, 0, False) /* Create Bow of the Quiddity (450312) for Shop */
     , (450450, 4, 450313,  0, 0, 0, False) /* Create Fist of the Quiddity (450313) for Shop */
     , (450450, 4, 450314,  0, 0, 0, False) /* Create Balister of the Quiddity (450314) for Shop */
     , (450450, 4, 450315,  0, 0, 0, False) /* Create Mace of the Quiddity (450315) for Shop */
     , (450450, 4, 450317,  0, 0, 0, False) /* Create Lance of the Quiddity (450317) for Shop */
     , (450450, 4, 450318,  0, 0, 0, False) /* Create Stave of the Quiddity (450318) for Shop */
     , (450450, 4, 450319,  0, 0, 0, False) /* Create Blade of the Quiddity (450319) for Shop */
     , (450450, 4, 1041794,  0, 0, 0, False) /* Create Greatblade of the Quiddity (1041794) for Shop */
     , (450450, 4, 1041912,  0, 0, 0, False) /* Create Enhanced Stave of the Quiddity (1041912) for Shop */
     , (450450, 4, 450320,  0, 0, 0, False) /* Create Assault Axe (450320) for Shop */
     , (450450, 4, 450321,  0, 0, 0, False) /* Create Assault Mace (450321) for Shop */
     , (450450, 4, 450322,  0, 0, 0, False) /* Create Assault Spear (450322) for Shop */
     , (450450, 4, 450323,  0, 0, 0, False) /* Create Assault Staff (450323) for Shop */
     , (450450, 4, 450324,  0, 0, 0, False) /* Create Assault Bow (450324) for Shop */
     , (450450, 4, 450325,  0, 0, 0, False) /* Create Assault Atlatl (450325) for Shop */
     , (450450, 4, 450326,  0, 0, 0, False) /* Create Assault Cestus (450326) for Shop */
     , (450450, 4, 450327,  0, 0, 0, False) /* Create Assault Crossbow (450327) for Shop */
     , (450450, 4, 450328,  0, 0, 0, False) /* Create Assault Dirk (450328) for Shop */
     , (450450, 4, 450329,  0, 0, 0, False) /* Create Assault Sword (450329) for Shop */
     , (450450, 4, 450330,  0, 0, 0, False) /* Create Assault Greatsword (450330) for Shop */
     , (450450, 4, 450331,  0, 0, 0, False) /* Create Chimeric Atlatl of the Quiddity (450331) for Shop */
     , (450450, 4, 450332,  0, 0, 0, False) /* Create Chimeric Axe of the Quiddity (450332) for Shop */
     , (450450, 4, 450333,  0, 0, 0, False) /* Create Chimeric Balister of the Quiddity (450333) for Shop */
     , (450450, 4, 450334,  0, 0, 0, False) /* Create Chimeric Dagger of the Quiddity (450334) for Shop */
     , (450450, 4, 450335,  0, 0, 0, False) /* Create Chimeric Mace of the Quiddity (450335) for Shop */
     , (450450, 4, 450336,  0, 0, 0, False) /* Create Chimeric Lance of the Quiddity (450336) for Shop */
     , (450450, 4, 450337,  0, 0, 0, False) /* Create Chimeric Stave of the Quiddity (450337) for Shop */
     , (450450, 4, 450338,  0, 0, 0, False) /* Create Chimeric Fist of the Quiddity (450338) for Shop */
     , (450450, 4, 450339,  0, 0, 0, False) /* Create Chimeric Two Handed Blade of the Quiddity (450339) for Shop */
     , (450450, 4, 450008,  0, 0, 0, False) /* Create Gelidite Mitre (450008) for Shop */
     , (450450, 4, 450000,  0, 0, 0, False) /* Create Gelidite Boots (450000) for Shop */
     , (450450, 4, 450001,  0, 0, 0, False) /* Create Gelidite Gauntlets (450001) for Shop */
     , (450450, 4, 450002,  0, 0, 0, False) /* Create Gelidite Bracers (450002) for Shop */
     , (450450, 4, 450003,  0, 0, 0, False) /* Create Gelidite Pauldrons (450003) for Shop */
     , (450450, 4, 450004,  0, 0, 0, False) /* Create Gelidite Breastplate (450004) for Shop */
     , (450450, 4, 450005,  0, 0, 0, False) /* Create Gelidite Girth (450005) for Shop */
     , (450450, 4, 450006,  0, 0, 0, False) /* Create Gelidite Tassets (450006) for Shop */
     , (450450, 4, 450007,  0, 0, 0, False) /* Create Gelidite Greaves (450007) for Shop */
     , (450450, 4, 450008,  0, 0, 0, False) /* Create Gelidite Mitre (450008) for Shop */
     , (450450, 4, 450172,  0, 0, 0, False) /* Create Helm of Leikotha's Tears (450172) for Shop */
     , (450450, 4, 450173,  0, 0, 0, False) /* Create Breastplate of Leikotha's Tears (450173) for Shop */
     , (450450, 4, 450174,  0, 0, 0, False) /* Create Pauldrons of Leikotha's Tears (450174) for Shop */
     , (450450, 4, 450175,  0, 0, 0, False) /* Create Bracers of Leikotha's Tears (450175) for Shop */
     , (450450, 4, 450176,  0, 0, 0, False) /* Create Gauntlets of Leikotha's Tears (450176) for Shop */
     , (450450, 4, 450177,  0, 0, 0, False) /* Create Girth of Leikotha's Tears (450177) for Shop */
     , (450450, 4, 450178,  0, 0, 0, False) /* Create Greaves of Leikotha's Tears (450178) for Shop */
     , (450450, 4, 450179,  0, 0, 0, False) /* Create Tassets of Leikotha's Tears (450179) for Shop */
     , (450450, 4, 450166,  0, 0, 0, False) /* Create Patriarch's Twilight Coat (450166) for Shop */
     , (450450, 4, 450167,  0, 0, 0, False) /* Create Patriarch's Twilight Tights (450167) for Shop */
     , (450450, 4, 450164,  0, 0, 0, False) /* Create Dusk Coat (450164) for Shop */
     , (450450, 4, 450165,  0, 0, 0, False) /* Create Dusk Leggings (450165) for Shop */
     , (450450, 4, 450168,  0, 0, 0, False) /* Create Valkeer's Helm (450168) for Shop */
	 , (450450, 4, 450790,  0, 0, 0, False) /* Create Paradox-touched Olthoi Wand (450765) for Shop */
     , (450450, 4, 450169,  0, 0, 0, False) /* Create Adept's Fervor (450169) for Shop */
     , (450450, 4, 450162,  0, 0, 0, False) /* Create Footman's Boots (450162) for Shop */
     , (450450, 4, 450163,  0, 0, 0, False) /* Create Steel Wall Boots (450163) for Shop */
     , (450450, 4, 4200096,  0, 0, 0, False) /* Create Tracker Boots (4200096) for Shop */
	 , (450450, 4, 450783,  0, 0, 0, False) /* Create Paradox-touched Olthoi Wand (450765) for Shop */
	 , (450450, 4, 450784,  0, 0, 0, False) /* Create Banished Orb (450752) for Shop */
     , (450450, 4, 450785,  0, 0, 0, False) /* Create Asteliary Orb (450754) for Shop */
     , (450450, 4, 450786,  0, 0, 0, False) /* Create Paradox-touched Olthoi Wand (450765) for Shop */
	 , (450450, 4, 450787,  0, 0, 0, False) /* Create Paradox-touched Olthoi Wand (450765) for Shop */
	 , (450450, 4, 450788,  0, 0, 0, False) /* Create Banished Orb (450752) for Shop */
     , (450450, 4, 450077,  0, 0, 0, False) /* Create Ursuin Guise (450077) for Shop */
     , (450450, 4, 450110,  0, 0, 0, False) /* Create Wooden Scarecrow Guise (450110) for Shop */
     , (450450, 4, 450112,  0, 0, 0, False) /* Create Mu-miyah Guise (450112) for Shop */
     , (450450, 4, 450113,  0, 0, 0, False) /* Create Sclavus Guise (450113) for Shop */
     , (450450, 4, 450114,  0, 0, 0, False) /* Create Skeletal Guise (450114) for Shop */
     , (450450, 4, 450115,  0, 0, 0, False) /* Create Undead Guise (450115) for Shop */
     , (450450, 4, 450116,  0, 0, 0, False) /* Create Full Mu-miyah Guise (450116) for Shop */
     , (450450, 4, 450121,  0, 0, 0, False) /* Create Armored Skeleton Guise (450121) for Shop */
     , (450450, 4, 450122,  0, 0, 0, False) /* Create Armored Undead Guise (450122) for Shop */
     , (450450, 4, 450123,  0, 0, 0, False) /* Create Scarecrow Guise (450123) for Shop */
     , (450450, 4, 450124,  0, 0, 0, False) /* Create Ghost Guise (450124) for Shop */
     , (450450, 4, 450125,  0, 0, 0, False) /* Create Gurog Guise (450125) for Shop */
     , (450450, 4, 1049901,  0, 0, 0, False) /* Create Prismatic Shadow Bracers (1049901) for Shop */
     , (450450, 4, 1049905,  0, 0, 0, False) /* Create Prismatic Shadow Breastplate (1049905) for Shop */
     , (450450, 4, 1049909,  0, 0, 0, False) /* Create Prismatic Shadow Gauntlets (1049909) for Shop */
     , (450450, 4, 1049913,  0, 0, 0, False) /* Create Prismatic Shadow Girth (1049913) for Shop */
     , (450450, 4, 1049917,  0, 0, 0, False) /* Create Prismatic Shadow Greaves (1049917) for Shop */
     , (450450, 4, 1049921,  0, 0, 0, False) /* Create Prismatic Shadow Helm (1049921) for Shop */
     , (450450, 4, 1049925,  0, 0, 0, False) /* Create Prismatic Shadow Pauldrons (1049925) for Shop */
     , (450450, 4, 1049929,  0, 0, 0, False) /* Create Prismatic Shadow Sollerets (1049929) for Shop */
     , (450450, 4, 1049933,  0, 0, 0, False) /* Create Prismatic Shadow Tassets (1049933) for Shop */
     , (450450, 4, 1910618,  0, 0, 0, False) /* Create Prismatic Amuli Coat (1910618) for Shop */
     , (450450, 4, 1910619,  0, 0, 0, False) /* Create Prismatic Amuli Leggings (1910619) for Shop */
     , (450450, 4, 1006600,  0, 0, 0, False) /* Create Greater Amuli Shadow Coat (1006600) for Shop */
     , (450450, 4, 1006606,  0, 0, 0, False) /* Create Greater Amuli Shadow Leggings (1006606) for Shop */
     , (450450, 4, 1006594,  0, 0, 0, False) /* Create Greater Celdon Shadow Breastplate (1006594) for Shop */
     , (450450, 4, 1006603,  0, 0, 0, False) /* Create Greater Celdon Shadow Girth (1006603) for Shop */
     , (450450, 4, 1006615,  0, 0, 0, False) /* Create Greater Celdon Shadow Sleeves (1006615) for Shop */
     , (450450, 4, 1006609,  0, 0, 0, False) /* Create Greater Celdon Shadow Leggings (1006609) for Shop */
     , (450450, 4, 1006801,  0, 0, 0, False) /* Create Nexus Amuli Leggings (1006801) for Shop */
     , (450450, 4, 1006799,  0, 0, 0, False) /* Create Nexus Amuli Coat (1006799) for Shop */
     , (450450, 4, 1006797,  0, 0, 0, False) /* Create Nexus Celdon Breastplate (1006797) for Shop */
     , (450450, 4, 1006800,  0, 0, 0, False) /* Create Nexus Celdon Girth (1006800) for Shop */
     , (450450, 4, 1006802,  0, 0, 0, False) /* Create Nexus Celdon Leggings (1006802) for Shop */
     , (450450, 4, 1006804,  0, 0, 0, False) /* Create Nexus Celdon Sleeves (1006804) for Shop */
     , (450450, 4, 450133,  0, 0, 0, False) /* Create Exarch Plate Coat (450133) for Shop */
     , (450450, 4, 450134,  0, 0, 0, False) /* Create Exarch Plate Coat (450134) for Shop */
     , (450450, 4, 450135,  0, 0, 0, False) /* Create Exarch Plate Coat (450135) for Shop */
     , (450450, 4, 450136,  0, 0, 0, False) /* Create Exarch Plate Girth (450136) for Shop */
     , (450450, 4, 450137,  0, 0, 0, False) /* Create Exarch Plate Girth (450137) for Shop */
     , (450450, 4, 450138,  0, 0, 0, False) /* Create Exarch Plate Girth (450138) for Shop */
     , (450450, 4, 450139,  0, 0, 0, False) /* Create Exarch Plate Leggings (450139) for Shop */
     , (450450, 4, 450140,  0, 0, 0, False) /* Create Exarch Plate Leggings (450140) for Shop */
     , (450450, 4, 450141,  0, 0, 0, False) /* Create Exarch Plate Leggings (450141) for Shop */
     , (450450, 4, 450142,  0, 0, 0, False) /* Create Auroric Exarch Coat (450142) for Shop */
     , (450450, 4, 450143,  0, 0, 0, False) /* Create Auroric Exarch Coat (450143) for Shop */
     , (450450, 4, 450144,  0, 0, 0, False) /* Create Auroric Exarch Coat (450144) for Shop */
     , (450450, 4, 450145,  0, 0, 0, False) /* Create Auroric Exarch Girth (450145) for Shop */
     , (450450, 4, 450146,  0, 0, 0, False) /* Create Auroric Exarch Girth (450146) for Shop */
     , (450450, 4, 450147,  0, 0, 0, False) /* Create Auroric Exarch Girth (450147) for Shop */
     , (450450, 4, 450148,  0, 0, 0, False) /* Create Auroric Exarch Leggings (450148) for Shop */
     , (450450, 4, 450149,  0, 0, 0, False) /* Create Auroric Exarch Leggings (450149) for Shop */
     , (450450, 4, 450150,  0, 0, 0, False) /* Create Auroric Exarch Leggings (450150) for Shop */
     , (450450, 4, 450151,  0, 0, 0, False) /* Create Ancient Armored Helm (450151) for Shop */
     , (450450, 4, 450152,  0, 0, 0, False) /* Create Ancient Armored Vestment (450152) for Shop */
     , (450450, 4, 450153,  0, 0, 0, False) /* Create Ancient Armored Bracers (450153) for Shop */
     , (450450, 4, 450154,  0, 0, 0, False) /* Create Ancient Armored Long Boots (450154) for Shop */
     , (450450, 4, 450155,  0, 0, 0, False) /* Create Ancient Armored Gauntlets (450155) for Shop */
     , (450450, 4, 450156,  0, 0, 0, False) /* Create Ancient Armored Leggings (450156) for Shop */
     , (450450, 4, 450157,  0, 0, 0, False) /* Create Noble Helm (450157) for Shop */
     , (450450, 4, 450158,  0, 0, 0, False) /* Create Noble Coat (450158) for Shop */
     , (450450, 4, 450159,  0, 0, 0, False) /* Create Noble Gauntlets (450159) for Shop */
     , (450450, 4, 450160,  0, 0, 0, False) /* Create Noble Leggings (450160) for Shop */
     , (450450, 4, 450161,  0, 0, 0, False) /* Create Noble Sollerets (450161) for Shop */
     , (450450, 4, 450263,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Coat (450263) for Shop */
     , (450450, 4, 450256,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Coat (450256) for Shop */
     , (450450, 4, 450257,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Coat (450257) for Shop */
     , (450450, 4, 450258,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Girth (450258) for Shop */
     , (450450, 4, 450259,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Girth (450259) for Shop */
     , (450450, 4, 450264,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Girth (450264) for Shop */
     , (450450, 4, 450260,  0, 0, 0, False) /* Create Luminescent Runic Helm (450260) for Shop */
     , (450450, 4, 450261,  0, 0, 0, False) /* Create Luminescent Runic Helm (450261) for Shop */
     , (450450, 4, 450265,  0, 0, 0, False) /* Create Luminescent Runic Helm (450265) for Shop */
     , (450450, 4, 450262,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Leggings (450262) for Shop */
     , (450450, 4, 450266,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Leggings (450266) for Shop */
     , (450450, 4, 450267,  0, 0, 0, False) /* Create  (450267) for Shop */
     , (450450, 4, 450268,  0, 0, 0, False) /* Create Luminescent Thaumaturgic Leggings (450268) for Shop */
     , (450450, 4, 450285,  0, 0, 0, False) /* Create Shou-jen Shozoku Mask (450285) for Shop */
     , (450450, 4, 450286,  0, 0, 0, False) /* Create Shou-jen Shozoku Jacket (450286) for Shop */
     , (450450, 4, 450287,  0, 0, 0, False) /* Create Shou-jen Shozoku Trousers (450287) for Shop */
     , (450450, 4, 450288,  0, 0, 0, False) /* Create Shou-jen Shozoku Sleeve Gauntlets (450288) for Shop */
     , (450450, 4, 450289,  0, 0, 0, False) /* Create Shou-jen Jika-Tabi (450289) for Shop */
     , (450450, 4, 450290,  0, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Mask (450290) for Shop */
     , (450450, 4, 450291,  0, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Jacket (450291) for Shop */
     , (450450, 4, 450292,  0, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Trousers (450292) for Shop */
     , (450450, 4, 450293,  0, 0, 0, False) /* Create Reinforced Shou-jen Shozoku Gauntlets (450293) for Shop */
     , (450450, 4, 450294,  0, 0, 0, False) /* Create Reinforced Shou-jen Jika-Tabi (450294) for Shop */
     , (450450, 4, 450340,  0, 0, 0, False) /* Create Celestial Hand Breastplate (450340) for Shop */
     , (450450, 4, 450341,  0, 0, 0, False) /* Create Celestial Hand Gauntlets (450341) for Shop */
     , (450450, 4, 450342,  0, 0, 0, False) /* Create Celestial Hand Girth (450342) for Shop */
     , (450450, 4, 450343,  0, 0, 0, False) /* Create Celestial Hand Greaves (450343) for Shop */
     , (450450, 4, 450344,  0, 0, 0, False) /* Create Celestial Hand Helm (450344) for Shop */
     , (450450, 4, 450345,  0, 0, 0, False) /* Create Celestial Hand Pauldrons (450345) for Shop */
     , (450450, 4, 450346,  0, 0, 0, False) /* Create Celestial Hand Tassets (450346) for Shop */
     , (450450, 4, 450347,  0, 0, 0, False) /* Create Celestial Hand Vambraces (450347) for Shop */
     , (450450, 4, 450348,  0, 0, 0, False) /* Create Celestial Hand Sollerets (450348) for Shop */
     , (450450, 4, 450349,  0, 0, 0, False) /* Create Eldrytch Web Breastplate (450349) for Shop */
     , (450450, 4, 450350,  0, 0, 0, False) /* Create Eldrytch Web Gauntlets (450350) for Shop */
     , (450450, 4, 450351,  0, 0, 0, False) /* Create Eldrytch Web Girth (450351) for Shop */
     , (450450, 4, 450352,  0, 0, 0, False) /* Create Eldrytch Web Greaves (450352) for Shop */
     , (450450, 4, 450353,  0, 0, 0, False) /* Create Eldrytch Web Helm (450353) for Shop */
     , (450450, 4, 450354,  0, 0, 0, False) /* Create Eldrytch Web Pauldrons (450354) for Shop */
     , (450450, 4, 450355,  0, 0, 0, False) /* Create Eldrytch Web Tassets (450355) for Shop */
     , (450450, 4, 450356,  0, 0, 0, False) /* Create Eldrytch Web Vambraces (450356) for Shop */
     , (450450, 4, 450357,  0, 0, 0, False) /* Create Eldrytch Web Sollerets (450357) for Shop */
     , (450450, 4, 450358,  0, 0, 0, False) /* Create Radiant Blood Breastplate (450358) for Shop */
     , (450450, 4, 450359,  0, 0, 0, False) /* Create Radiant Blood Gauntlets (450359) for Shop */
     , (450450, 4, 450360,  0, 0, 0, False) /* Create Radiant Blood Girth (450360) for Shop */
     , (450450, 4, 450361,  0, 0, 0, False) /* Create Radiant Blood Greaves (450361) for Shop */
     , (450450, 4, 450362,  0, 0, 0, False) /* Create Radiant Blood Helm (450362) for Shop */
     , (450450, 4, 450363,  0, 0, 0, False) /* Create Radiant Blood Pauldrons (450363) for Shop */
     , (450450, 4, 450364,  0, 0, 0, False) /* Create Radiant Blood Tassets (450364) for Shop */
     , (450450, 4, 450365,  0, 0, 0, False) /* Create Radiant Blood Vambraces (450365) for Shop */
     , (450450, 4, 450366,  0, 0, 0, False) /* Create Radiant Blood Sollerets (450366) for Shop */
     , (450450, 4, 450269,  0, 0, 0, False) /* Create Lugian Armor (450269) for Shop */
     , (450450, 4, 450270,  0, 0, 0, False) /* Create Bastion of Tukal (450270) for Shop */
     , (450450, 4, 450271,  0, 0, 0, False) /* Create Renegade Hauberk (450271) for Shop */
     , (450450, 4, 450272,  0, 0, 0, False) /* Create Renegade Leggings (450272) for Shop */
     , (450450, 4, 1046345,  0, 0, 0, False) /* Create O-Yoroi Leggings (1046345) for Shop */
     , (450450, 4, 1046551,  0, 0, 0, False) /* Create O-Yoroi Gauntlets (1046551) for Shop */
     , (450450, 4, 1046552,  0, 0, 0, False) /* Create O-Yoroi Helm (1046552) for Shop */
     , (450450, 4, 1046553,  0, 0, 0, False) /* Create O-Yoroi Sandals (1046553) for Shop */
     , (450450, 4, 1046615,  0, 0, 0, False) /* Create O-Yoroi Coat (1046615) for Shop */
     , (450450, 4, 2046345,  0, 0, 0, False) /* Create O-Yoroi Leggings (2046345) for Shop */
     , (450450, 4, 2046551,  0, 0, 0, False) /* Create O-Yoroi Gauntlets (2046551) for Shop */
     , (450450, 4, 2046552,  0, 0, 0, False) /* Create O-Yoroi Helm (2046552) for Shop */
     , (450450, 4, 2046553,  0, 0, 0, False) /* Create O-Yoroi Sandals (2046553) for Shop */
     , (450450, 4, 2046615,  0, 0, 0, False) /* Create O-Yoroi Coat (2046615) for Shop */
     , (450450, 4, 450278,  0, 0, 0, False) /* Create Breastplate of Splendor (450278) for Shop */
     , (450450, 4, 450279,  0, 0, 0, False) /* Create Breastplate of Grace (450279) for Shop */
     , (450450, 4, 450280,  0, 0, 0, False) /* Create Breastplate of Power (450280) for Shop */
     , (450450, 4, 450283,  0, 0, 0, False) /* Create Squalid Coat (450283) for Shop */
     , (450450, 4, 450284,  0, 0, 0, False) /* Create Squalid Leggings (450284) for Shop */
     , (450450, 4, 450275,  0, 0, 0, False) /* Create Royal Color (450275) for Shop */
     , (450450, 4, 450276,  0, 0, 0, False) /* Create Royal Paint (450276) for Shop */
     , (450450, 4, 450277,  0, 0, 0, False) /* Create Royal Dye (450277) for Shop */
     , (450450, 4, 450707,  0, 0, 0, False) /* Create Royal Oil (450707) for Shop */
     , (450450, 4, 450711,  0, 0, 0, False) /* Create Swamp Lord's War Paint (450711) for Shop */
     , (450450, 4, 105280,  0, 0, 0, False) /* Create Holiday Sweater (105280) for Shop */
     , (450450, 4, 450532,  0, 0, 0, False) /* Create Mattekar Hide Coat (450532) for Shop */
     , (450450, 4, 450533,  0, 0, 0, False) /* Create Furry Mattekar Hide Coat (450533) for Shop */
     , (450450, 4, 450534,  0, 0, 0, False) /* Create Ursuin Hide Coat (450534) for Shop */
     , (450450, 4, 450535,  0, 0, 0, False) /* Create Ursuin Hide Coat (450535) for Shop */
     , (450450, 4, 450536,  0, 0, 0, False) /* Create Heavy Ursuin Coat (450536) for Shop */
     , (450450, 4, 450538,  0, 0, 0, False) /* Create Snake Skin Coat (450538) for Shop */
     , (450450, 4, 450539,  0, 0, 0, False) /* Create Hea Bone and Hide Shirt (450539) for Shop */
     , (450450, 4, 450712,  0, 0, 0, False) /* Create Armoredillo Hide Coat (450712) for Shop */
     , (450450, 4, 450713,  0, 0, 0, False) /* Create Gromnie Hide Coat (450713) for Shop */
     , (450450, 4, 450714,  0, 0, 0, False) /* Create Gromnie Hide Leggings (450714) for Shop */
     , (450450, 4, 450132,  0, 0, 0, False) /* Create Branith's Shirt (450132) for Shop */
     , (450450, 4, 450273,  0, 0, 0, False) /* Create Jaleh's Chain Shirt (450273) for Shop */
     , (450450, 4, 450274,  0, 0, 0, False) /* Create Ornate Tumerok Breastplate (450274) for Shop */
     , (450450, 4, 450281,  0, 0, 0, False) /* Create Rendeath Coat (450281) for Shop */
     , (450450, 4, 450282,  0, 0, 0, False) /* Create Doomshark Hide Coat (450282) for Shop */
     , (450450, 4, 450717,  0, 0, 0, False) /* Create Cowl of the Sand (450717) for Shop */
     , (450450, 4, 1032148,  0, 0, 0, False) /* Create Shadow Wings Breastplate (1032148) for Shop */
     , (450450, 4, 4200095,  0, 0, 0, False) /* Create Harbinger Arm Guard (4200095) for Shop */
     , (450450, 4, 4200101,  0, 0, 0, False) /* Create Turquoise Winged Helmet (4200101) for Shop */
     , (450450, 4, 4200102,  0, 0, 0, False) /* Create Lustrous Winged Leggings (4200102) for Shop */
     , (450450, 4, 4200103,  0, 0, 0, False) /* Create Dusky Winged Coat (4200103) for Shop */
     , (450450, 4, 450727,  0, 0, 0, False) /* Create Ruddy Winged Boots (450727) for Shop */
     , (450450, 4, 1029818,  0, 0, 0, False) /* Create Badlands Siraluun Headdress (1029818) for Shop */
     , (450450, 4, 1029819,  0, 0, 0, False) /* Create Kithless Siraluun Headdress (1029819) for Shop */
     , (450450, 4, 1029820,  0, 0, 0, False) /* Create Littoral Siraluun Headdress (1029820) for Shop */
     , (450450, 4, 1029821,  0, 0, 0, False) /* Create Marsh Siraluun Headdress (1029821) for Shop */
     , (450450, 4, 1029822,  0, 0, 0, False) /* Create Strand Siraluun Headdress (1029822) for Shop */
     , (450450, 4, 1029823,  0, 0, 0, False) /* Create Tidal Siraluun Headdress (1029823) for Shop */
     , (450450, 4, 1029824,  0, 0, 0, False) /* Create Timber Siraluun Headdress (1029824) for Shop */
     , (450450, 4, 1029825,  0, 0, 0, False) /* Create Untamed Siraluun Headdress (1029825) for Shop */
     , (450450, 4, 1051858,  0, 0, 0, False) /* Create Rynthid Sorcerer of Torment's Mask (1051858) for Shop */
     , (450450, 4, 1051854,  0, 0, 0, False) /* Create Rynthid Minion of Torment's Mask (1051854) for Shop */
     , (450450, 4, 1051855,  0, 0, 0, False) /* Create Rynthid Minion of Rage's Mask (1051855) for Shop */
     , (450450, 4, 1051857,  0, 0, 0, False) /* Create Rynthid Ravager's Mask (1051857) for Shop */
     , (450450, 4, 1051856,  0, 0, 0, False) /* Create Rynthid Berserker's Mask (1051856) for Shop */
     , (450450, 4, 1043197,  0, 0, 0, False) /* Create Apostate Grand Director's Mask (1043197) for Shop */
     , (450450, 4, 1043040,  0, 0, 0, False) /* Create Nexus Crawler's Mask (1043040) for Shop */
     , (450450, 4, 1025335,  0, 0, 0, False) /* Create Virindi Consul Mask (1025335) for Shop */
     , (450450, 4, 1024879,  0, 0, 0, False) /* Create Virindi Profatrix Mask (1024879) for Shop */
     , (450450, 4, 1011998,  0, 0, 0, False) /* Create Virindi Inquisitor's Mask (1011998) for Shop */
     , (450450, 4, 1008153,  0, 0, 0, False) /* Create Virindi Mask (1008153) for Shop */
     , (450450, 4, 450128,  0, 0, 0, False) /* Create Visage of the Shadow Virindi (450128) for Shop */
     , (450450, 4, 450131,  0, 0, 0, False) /* Create Visage of Menilesh (450131) for Shop */
     , (450450, 4, 1012207,  0, 0, 0, False) /* Create Inviso Mask (1012207) for Shop */
     , (450450, 4, 450451,  0, 0, 0, False) /* Create King's Helm (450451) for Shop */
     , (450450, 4, 450455,  0, 0, 0, False) /* Create Pwyll's Crown (450455) for Shop */
     , (450450, 4, 450456,  0, 0, 0, False) /* Create Pwyll's Guard (450456) for Shop */
     , (450450, 4, 450457,  0, 0, 0, False) /* Create Alfric's Bull (450457) for Shop */
     , (450450, 4, 450460,  0, 0, 0, False) /* Create The Red Bull (450460) for Shop */
     , (450450, 4, 450072,  0, 0, 0, False) /* Create The Stag of Bellenesse (450072) for Shop */
     , (450450, 4, 4200104,  0, 0, 0, False) /* Create Koji's Beast (4200104) for Shop */
     , (450450, 4, 4200454,  0, 0, 0, False) /* Create  (4200454) for Shop */
     , (450450, 4, 450459,  0, 0, 0, False) /* Create Koji's Visage (450459) for Shop */
     , (450450, 4, 4200452,  0, 0, 0, False) /* Create  (4200452) for Shop */
     , (450450, 4, 4200105,  0, 0, 0, False) /* Create Veil of Darkness (4200105) for Shop */
     , (450450, 4, 450458,  0, 0, 0, False) /* Create The Poet's Mask (450458) for Shop */
     , (450450, 4, 450070,  0, 0, 0, False) /* Create Mask of the Malik (450070) for Shop */
     , (450450, 4, 450069,  0, 0, 0, False) /* Create The Dragon of Power (450069) for Shop */
     , (450450, 4, 450074,  0, 0, 0, False) /* Create The Royal Bull of Sanamar (450074) for Shop */
     , (450450, 4, 450071,  0, 0, 0, False) /* Create Karlun's Visage (450071) for Shop */
     , (450450, 4, 450119,  0, 0, 0, False) /* Create Koji's Visage (450119) for Shop */
     , (450450, 4, 450073,  0, 0, 0, False) /* Create The Boar of Cinghalle (450073) for Shop */
     , (450450, 4, 1021373,  0, 0, 0, False) /* Create Martine's Mask (1021373) for Shop */
     , (450450, 4, 1008805,  0, 0, 0, False) /* Create Nexus Helm of the Lightbringer (1008805) for Shop */
     , (450450, 4, 1008806,  0, 0, 0, False) /* Create Fenmalain Helm of the Lightbringer (1008806) for Shop */
     , (450450, 4, 1008807,  0, 0, 0, False) /* Create Caulnalain Helm of the Lightbringer (1008807) for Shop */
     , (450450, 4, 1008808,  0, 0, 0, False) /* Create Shendolain Helm of the Lightbringer (1008808) for Shop */
     , (450450, 4, 1008809,  0, 0, 0, False) /* Create Herald's Helm of the Lightbringer (1008809) for Shop */
     , (450450, 4, 1026498,  0, 0, 0, False) /* Create Crown of Anointed Blood (1026498) for Shop */
     , (450450, 4, 450075,  0, 0, 0, False) /* Create Mask of the Hopeslayer (450075) for Shop */
     , (450450, 4, 450076,  0, 0, 0, False) /* Create Helm of Isin Dule (450076) for Shop */
     , (450450, 4, 1011986,  0, 0, 0, False) /* Create Energy Crown (1011986) for Shop */
     , (450450, 4, 42130741,  0, 0, 0, False) /* Create Party Hat (42130741) for Shop */
     , (450450, 4, 4200136,  0, 0, 0, False) /* Create Aphus Sun Guard (4200136) for Shop */
     , (450450, 4, 1042667,  0, 0, 0, False) /* Create Top Hat (1042667) for Shop */
     , (450450, 4, 1014932,  0, 0, 0, False) /* Create Crimped Hat (1014932) for Shop */
     , (450450, 4, 1009622,  0, 0, 0, False) /* Create Chef's Hat (1009622) for Shop */
     , (450450, 4, 450111,  0, 0, 0, False) /* Create Fletcher's Cap (450111) for Shop */
     , (450450, 4, 450117,  0, 0, 0, False) /* Create Stocking Cap (450117) for Shop */
     , (450450, 4, 450118,  0, 0, 0, False) /* Create Tall Stocking Cap (450118) for Shop */
     , (450450, 4, 450755,  0, 0, 0, False) /* Create Scribe Hat (450755) for Shop */
     , (450450, 4, 450757,  0, 0, 0, False) /* Create Miner's Hat (450757) for Shop */
     , (450450, 4, 450758,  0, 0, 0, False) /* Create Timberman's Hat (450758) for Shop */
     , (450450, 4, 450759,  0, 0, 0, False) /* Create Trapper's Hat (450759) for Shop */
     , (450450, 4, 10049811,  0, 0, 0, False) /* Create Ice Heaume of Frore (10049811) for Shop */
     , (450450, 4, 10049812,  0, 0, 0, False) /* Create Ice Heaume of Frore (10049812) for Shop */
     , (450450, 4, 10049813,  0, 0, 0, False) /* Create Ice Heaume of Frore (10049813) for Shop */
     , (450450, 4, 42149813,  0, 0, 0, False) /* Create Ice Heaume of Frore (42149813) for Shop */
     , (450450, 4, 450453,  0, 0, 0, False) /* Create Salvager's Helm (450453) for Shop */
     , (450450, 4, 450295,  0, 0, 0, False) /* Create Envoy's Heaume (450295) for Shop */
     , (450450, 4, 13241334,  0, 0, 0, False) /* Create The Helm of the Golden Flame (13241334) for Shop */
     , (450450, 4, 450512,  0, 0, 0, False) /* Create Helm of the White Totem (450512) for Shop */
     , (450450, 4, 450513,  0, 0, 0, False) /* Create Helm of the Black Totem (450513) for Shop */
     , (450450, 4, 450514,  0, 0, 0, False) /* Create Helm of the Abyssal Totem (450514) for Shop */
     , (450450, 4, 450509,  0, 0, 0, False) /* Create Niffis Shell Helm (450509) for Shop */
     , (450450, 4, 450510,  0, 0, 0, False) /* Create Helm of the Simulacra (450510) for Shop */
     , (450450, 4, 450511,  0, 0, 0, False) /* Create Imbued Helm of the Simulacra (450511) for Shop */
     , (450450, 4, 450515,  0, 0, 0, False) /* Create Horned Lugian Helm (450515) for Shop */
     , (450450, 4, 450516,  0, 0, 0, False) /* Create Nexus Commander's Helm (450516) for Shop */
     , (450450, 4, 450517,  0, 0, 0, False) /* Create Olthoi Helm (450517) for Shop */
     , (450450, 4, 450518,  0, 0, 0, False) /* Create Skeletal Helm (450518) for Shop */
	 , (450450, 4, 450756,  0, 0, 0, False) /* Create Helm of the Crag (450756) for Shop */
     , (450450, 4, 450763,  0, 0, 0, False) /* Create Surloshen's Helm (450763) for Shop */
     , (450450, 4, 450519,  0, 0, 0, False) /* Create Uber Balance Testing Helm (450519) for Shop */
     , (450450, 4, 450520,  0, 0, 0, False) /* Create Silver Invader Lord Helm (450520) for Shop */
     , (450450, 4, 450521,  0, 0, 0, False) /* Create Blooded Silver Invader Lord Helm (450521) for Shop */
     , (450450, 4, 450522,  0, 0, 0, False) /* Create Copper Invader Lord Helm (450522) for Shop */
     , (450450, 4, 450523,  0, 0, 0, False) /* Create Blooded Copper Invader Lord Helm (450523) for Shop */
     , (450450, 4, 450526,  0, 0, 0, False) /* Create Platinum Invader Lord Helm (450526) for Shop */
     , (450450, 4, 450527,  0, 0, 0, False) /* Create Blooded Platinum Invader Lord Helm (450527) for Shop */
     , (450450, 4, 450529,  0, 0, 0, False) /* Create Viamontian Lord's Helm (450529) for Shop */
     , (450450, 4, 450471,  0, 0, 0, False) /* Create Banderling Mask (450471) for Shop */
     , (450450, 4, 450472,  0, 0, 0, False) /* Create Banderling Mask (450472) for Shop */
     , (450450, 4, 450473,  0, 0, 0, False) /* Create Mosswart Mask (450473) for Shop */
     , (450450, 4, 450474,  0, 0, 0, False) /* Create Mosswart Mask (450474) for Shop */
     , (450450, 4, 450475,  0, 0, 0, False) /* Create Moarsman Mask (450475) for Shop */
     , (450450, 4, 450476,  0, 0, 0, False) /* Create Burun Guruk Mask (450476) for Shop */
     , (450450, 4, 450477,  0, 0, 0, False) /* Create Burun Ruuk Mask (450477) for Shop */
     , (450450, 4, 450478,  0, 0, 0, False) /* Create Chittick Mask (450478) for Shop */
     , (450450, 4, 450479,  0, 0, 0, False) /* Create Olthoi Helm (450479) for Shop */
     , (450450, 4, 450480,  0, 0, 0, False) /* Create Olthoi Helm (450480) for Shop */
     , (450450, 4, 450481,  0, 0, 0, False) /* Create Mutilator Helm (450481) for Shop */
     , (450450, 4, 450482,  0, 0, 0, False) /* Create Mukkir Mask (450482) for Shop */
     , (450450, 4, 450483,  0, 0, 0, False) /* Create Drudge Mask (450483) for Shop */
     , (450450, 4, 450484,  0, 0, 0, False) /* Create Tusker Mask (450484) for Shop */
     , (450450, 4, 450485,  0, 0, 0, False) /* Create Ursuin Mask (450485) for Shop */
     , (450450, 4, 450486,  0, 0, 0, False) /* Create Mite Mask (450486) for Shop */
     , (450450, 4, 450487,  0, 0, 0, False) /* Create Sclavus Mask (450487) for Shop */
     , (450450, 4, 450488,  0, 0, 0, False) /* Create Armored Sclavus Mask (450488) for Shop */
     , (450450, 4, 450489,  0, 0, 0, False) /* Create Armored Sclavus Mask (450489) for Shop */
     , (450450, 4, 450490,  0, 0, 0, False) /* Create Armored Sclavus Mask (450490) for Shop */
     , (450450, 4, 450508,  0, 0, 0, False) /* Create Skull Mask (450508) for Shop */
     , (450450, 4, 450491,  0, 0, 0, False) /* Create Zombie Mask (450491) for Shop */
     , (450450, 4, 450492,  0, 0, 0, False) /* Create Scarecrow Mask (450492) for Shop */
     , (450450, 4, 450537,  0, 0, 0, False) /* Create Accursed Scarecrow Mask (450537) for Shop */
     , (450450, 4, 450493,  0, 0, 0, False) /* Create Scary Minion Mask (450493) for Shop */
     , (450450, 4, 450494,  0, 0, 0, False) /* Create Hollow Minion Mask (450494) for Shop */
     , (450450, 4, 450495,  0, 0, 0, False) /* Create Doll Mask (450495) for Shop */
     , (450450, 4, 450496,  0, 0, 0, False) /* Create Maddened Fiun Mask (450496) for Shop */
     , (450450, 4, 450497,  0, 0, 0, False) /* Create Ruschk Mask (450497) for Shop */
     , (450450, 4, 450498,  0, 0, 0, False) /* Create Shadow Mask (450498) for Shop */
     , (450450, 4, 450499,  0, 0, 0, False) /* Create Cow Mask (450499) for Shop */
     , (450450, 4, 450500,  0, 0, 0, False) /* Create Penguin Mask (450500) for Shop */
     , (450450, 4, 450501,  0, 0, 0, False) /* Create Uber Penguin Mask (450501) for Shop */
     , (450450, 4, 450503,  0, 0, 0, False) /* Create Snowman Mask with Hat (450503) for Shop */
     , (450450, 4, 450504,  0, 0, 0, False) /* Create Snowman Mask with Fez (450504) for Shop */
     , (450450, 4, 450505,  0, 0, 0, False) /* Create Giant Snowman Mask (450505) for Shop */
     , (450450, 4, 450506,  0, 0, 0, False) /* Create Two Headed Snowman Mask (450506) for Shop */
     , (450450, 4, 450507,  0, 0, 0, False) /* Create Homunculus Mask (450507) for Shop */
     , (450450, 4, 450100,  0, 0, 0, False) /* Create Undead Captain Mask (450100) for Shop */
     , (450450, 4, 450129,  0, 0, 0, False) /* Create Undead Sailor Mask (450129) for Shop */
     , (450450, 4, 450130,  0, 0, 0, False) /* Create Undead Captain's Hat (450130) for Shop */
     , (450450, 4, 450120,  0, 0, 0, False) /* Create Chicken Hat (450120) for Shop */
     , (450450, 4, 450126,  0, 0, 0, False) /* Create Sawato Bandit's Mask (450126) for Shop */
     , (450450, 4, 450127,  0, 0, 0, False) /* Create Asheron Mask (450127) for Shop */
     , (450450, 4, 450181,  0, 0, 0, False) /* Create Pirate Hook (450181) for Shop */
     , (450450, 4, 450182,  0, 0, 0, False) /* Create Left Peg Leg (450182) for Shop */
     , (450450, 4, 450183,  0, 0, 0, False) /* Create Right Peg Leg (450183) for Shop */
     , (450450, 4, 450184,  0, 0, 0, False) /* Create Peg Legs (450184) for Shop */
     , (450450, 4, 450530,  0, 0, 0, False) /* Create Kareb Mask (450530) for Shop */
     , (450450, 4, 1012236,  0, 0, 0, False) /* Create Energy Crystal (1012236) for Shop */
     , (450450, 4, 450560,  0, 0, 0, False) /* Create Singularity Crossbow (450560) for Shop */
     , (450450, 4, 450561,  0, 0, 0, False) /* Create Singularity Dagger (450561) for Shop */
     , (450450, 4, 450562,  0, 0, 0, False) /* Create Singularity Katar (450562) for Shop */
     , (450450, 4, 450563,  0, 0, 0, False) /* Create Singularity Mace (450563) for Shop */
     , (450450, 4, 450564,  0, 0, 0, False) /* Create Singularity Spear (450564) for Shop */
     , (450450, 4, 450566,  0, 0, 0, False) /* Create Singularity Staff (450566) for Shop */
     , (450450, 4, 450567,  0, 0, 0, False) /* Create Singularity Sword (450567) for Shop */
     , (450450, 4, 450568,  0, 0, 0, False) /* Create Singularity Bow (450568) for Shop */
     , (450450, 4, 450569,  0, 0, 0, False) /* Create Ice Box (450569) for Shop */
     , (450450, 4, 450570,  0, 0, 0, False) /* Create Singularity Greatsword (450570) for Shop */
     , (450450, 4, 450573,  0, 0, 0, False) /* Create Bound Singularity Crossbow (450573) for Shop */
     , (450450, 4, 450574,  0, 0, 0, False) /* Create Bound Singularity Bow (450574) for Shop */
     , (450450, 4, 450575,  0, 0, 0, False) /* Create Bound Singularity Katar (450575) for Shop */
     , (450450, 4, 450576,  0, 0, 0, False) /* Create Bound Singularity Mace (450576) for Shop */
     , (450450, 4, 450577,  0, 0, 0, False) /* Create Bound Singularity Spear (450577) for Shop */
     , (450450, 4, 450578,  0, 0, 0, False) /* Create Bound Singularity Dagger (450578) for Shop */
     , (450450, 4, 450579,  0, 0, 0, False) /* Create Bound Singularity Sword (450579) for Shop */
     , (450450, 4, 450580,  0, 0, 0, False) /* Create Bound Singularity Greatsword (450580) for Shop */
     , (450450, 4, 450581,  0, 0, 0, False) /* Create Ultimate Singularity Bow (450581) for Shop */
     , (450450, 4, 450582,  0, 0, 0, False) /* Create Ultimate Singularity Crossbow (450582) for Shop */
     , (450450, 4, 450583,  0, 0, 0, False) /* Create Ultimate Singularity Dagger (450583) for Shop */
     , (450450, 4, 450584,  0, 0, 0, False) /* Create Ultimate Singularity Katar (450584) for Shop */
     , (450450, 4, 450585,  0, 0, 0, False) /* Create Ultimate Singularity Mace (450585) for Shop */
     , (450450, 4, 450586,  0, 0, 0, False) /* Create Ultimate Singularity Spear (450586) for Shop */
     , (450450, 4, 450587,  0, 0, 0, False) /* Create Ultimate Singularity Staff (450587) for Shop */
     , (450450, 4, 450588,  0, 0, 0, False) /* Create Ultimate Singularity Sword (450588) for Shop */
     , (450450, 4, 450589,  0, 0, 0, False) /* Create Ultimate Singularity Greatsword (450589) for Shop */
     , (450450, 4, 450590,  0, 0, 0, False) /* Create Purified Mouryou Katana (450590) for Shop */
     , (450450, 4, 450591,  0, 0, 0, False) /* Create Purified Mouryou Nanjou-tachi (450591) for Shop */
     , (450450, 4, 450592,  0, 0, 0, False) /* Create Purified Mouryou Nodachi (450592) for Shop */
     , (450450, 4, 450593,  0, 0, 0, False) /* Create Purified Mouryou Wakizashi (450593) for Shop */
     , (450450, 4, 450594,  0, 0, 0, False) /* Create Purified Mouryou Nekode (450594) for Shop */
     , (450450, 4, 450597,  0, 0, 0, False) /* Create Shroud of Night (450597) for Shop */
     , (450450, 4, 450598,  0, 0, 0, False) /* Create Tome of Caustics (450598) for Shop */
     , (450450, 4, 450599,  0, 0, 0, False) /* Create Tome of Flame (450599) for Shop */
     , (450450, 4, 450600,  0, 0, 0, False) /* Create Tome of Chill (450600) for Shop */
     , (450450, 4, 450602,  0, 0, 0, False) /* Create Dark Sorcerer's Phylactery (450602) for Shop */
     , (450450, 4, 450603,  0, 0, 0, False) /* Create Festival Lights (450603) for Shop */
     , (450450, 4, 450604,  0, 0, 0, False) /* Create Polestar (450604) for Shop */
     , (450450, 4, 450605,  0, 0, 0, False) /* Create Holiday Lights (450605) for Shop */
     , (450450, 4, 450606,  0, 0, 0, False) /* Create Ball of Gunk (450606) for Shop */
     , (450450, 4, 42724, -1, 0, 0, False) /* Create Armor Layering Tool (Top) (42724) for Shop */
     , (450450, 4, 42726, -1, 0, 0, False) /* Create Armor Layering Tool (Bottom) (42726) for Shop */
     , (450450, 4, 51445, -1, 0, 0, False) /* Create Weapon Tailoring Kit (51445) for Shop */
     , (450450, 4, 41956, -1, 0, 0, False) /* Create Armor Tailoring Kit (41956) for Shop */
     , (450450, 4, 42622, -1, 0, 0, False) /* Create Armor Main Reduction Tool (42622) for Shop */
     , (450450, 4, 44880, -1, 0, 0, False) /* Create Armor Middle Reduction Tool (44880) for Shop */
     , (450450, 4, 44879, -1, 0, 0, False) /* Create Armor Lower Reduction Tool (44879) for Shop */
     , (450450, 4, 450718,  0, 0, 0, False) /* Create Suzuhara's Girth of Flame Protection (450718) for Shop */
     , (450450, 4, 450715,  0, 0, 0, False) /* Create Impious Staff (450715) for Shop */
     , (450450, 4, 450716,  0, 0, 0, False) /* Create Stave of Palenqual (450716) for Shop */
     , (450450, 4, 450719,  0, 0, 0, False) /* Create Tusker Paws (450719) for Shop */
     , (450450, 4, 450720,  0, 0, 0, False) /* Create Opal Gauntlets (450720) for Shop */
     , (450450, 4, 450721,  0, 0, 0, False) /* Create Gromnie Hide Gauntlets (450721) for Shop */
     , (450450, 4, 450722,  0, 0, 0, False) /* Create Fiun Spellcasting Gloves (450722) for Shop */
     , (450450, 4, 450723,  0, 0, 0, False) /* Create  (450723) for Shop */
     , (450450, 4, 450724,  0, 0, 0, False) /* Create Fleet Strike Gauntlets (450724) for Shop */
     , (450450, 4, 450725,  0, 0, 0, False) /* Create Whispering Blade Gloves (450725) for Shop */
     , (450450, 4, 450726,  0, 0, 0, False) /* Create Machinist's Gloves (450726) for Shop */
     , (450450, 4, 450728,  0, 0, 0, False) /* Create Snake Skin Boots (450728) for Shop */
     , (450450, 4, 450729,  0, 0, 0, False) /* Create Ursuin Boots (450729) for Shop */
     , (450450, 4, 450730,  0, 0, 0, False) /* Create Bunny Slippers (450730) for Shop */
     , (450450, 4, 450731,  0, 0, 0, False) /* Create White Bunny Slippers (450731) for Shop */
     , (450450, 4, 450732,  0, 0, 0, False) /* Create Hulking Bunny Slippers (450732) for Shop */
     , (450450, 4, 450733,  0, 0, 0, False) /* Create Jaleh's Slippers (450733) for Shop */
     , (450450, 4, 450734,  0, 0, 0, False) /* Create Walking Boots (450734) for Shop */
     , (450450, 4, 450735,  0, 0, 0, False) /* Create Whispering Blade Boots (450735) for Shop */
     , (450450, 4, 450736,  0, 0, 0, False) /* Create Rossu Morta Boots (450736) for Shop */
     , (450450, 4, 450740,  0, 0, 0, False) /* Create Great Work Staff of the Lightbringer (450740) for Shop */
     , (450450, 4, 450741,  0, 0, 0, False) /* Create Nexus Staff of the Lightbringer (450741) for Shop */
     , (450450, 4, 450742,  0, 0, 0, False) /* Create Fenmalain Staff of the Lightbringer (450742) for Shop */
     , (450450, 4, 450743,  0, 0, 0, False) /* Create Herald's Staff of the Lightbringer (450743) for Shop */
     , (450450, 4, 450766,  0, 0, 0, False) /* Create Shendolain Staff of the Lightbringer (450766) for Shop */
     , (450450, 4, 450751,  0, 0, 0, False) /* Create Staff of the Painbringer (450751) for Shop */
     , (450450, 4, 450744,  0, 0, 0, False) /* Create Banderling Wand (450744) for Shop */
     , (450450, 4, 450745,  0, 0, 0, False) /* Create Drudge Wand (450745) for Shop */
     , (450450, 4, 450746,  0, 0, 0, False) /* Create Mosswart Wand (450746) for Shop */
     , (450450, 4, 450747,  0, 0, 0, False) /* Create Skull Wand (450747) for Shop */
     , (450450, 4, 450748,  0, 0, 0, False) /* Create Tusker Wand (450748) for Shop */
     , (450450, 4, 450749,  0, 0, 0, False) /* Create Tusker Paw Wand (450749) for Shop */
     , (450450, 4, 450750,  0, 0, 0, False) /* Create Shendolain Soul Crystal Orb (450750) for Shop */
     , (450450, 4, 450752,  0, 0, 0, False) /* Create Banished Orb (450752) for Shop */
     , (450450, 4, 450754,  0, 0, 0, False) /* Create Asteliary Orb (450754) for Shop */
     , (450450, 4, 450765,  0, 0, 0, False) /* Create Paradox-touched Olthoi Wand (450765) for Shop */
	 , (450450, 4, 450753,  0, 0, 0, False) /* Create Paradox-touched Olthoi Wand (450765) for Shop */
     , (450450, 4, 450770,  0, 0, 0, False) /* Create Herald's Staff of the Lightbringer (450743) for Shop */
     , (450450, 4, 450771,  0, 0, 0, False) /* Create Shendolain Staff of the Lightbringer (450766) for Shop */
     , (450450, 4, 450772,  0, 0, 0, False) /* Create Staff of the Painbringer (450751) for Shop */
     , (450450, 4, 450773,  0, 0, 0, False) /* Create Banderling Wand (450744) for Shop */
     , (450450, 4, 450774,  0, 0, 0, False) /* Create Drudge Wand (450745) for Shop */
     , (450450, 4, 450775,  0, 0, 0, False) /* Create Mosswart Wand (450746) for Shop */
     , (450450, 4, 450776,  0, 0, 0, False) /* Create Skull Wand (450747) for Shop */
     , (450450, 4, 450777,  0, 0, 0, False) /* Create Tusker Wand (450748) for Shop */
     , (450450, 4, 450778,  0, 0, 0, False) /* Create Tusker Paw Wand (450749) for Shop */
     , (450450, 4, 450779,  0, 0, 0, False) /* Create Shendolain Soul Crystal Orb (450750) for Shop */
     , (450450, 4, 450780,  0, 0, 0, False) /* Create Banished Orb (450752) for Shop */
     , (450450, 4, 450781,  0, 0, 0, False) /* Create Asteliary Orb (450754) for Shop */
     , (450450, 4, 450782,  0, 0, 0, False) /* Create Paradox-touched Olthoi Wand (450765) for Shop */;
	 
	 

	 
	 


	 
	 
