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
     , (450450,   3, 0.1599999964237213) /* HealthRate */
     , (450450,   4,       5) /* StaminaRate */
     , (450450,   5,       1) /* ManaRate */
     , (450450,  11,     300) /* ResetInterval */
     , (450450,  13, 0.8999999761581421) /* ArmorModVsSlash */
     , (450450,  14,       1) /* ArmorModVsPierce */
     , (450450,  15, 1.100000023841858) /* ArmorModVsBludgeon */
     , (450450,  16, 0.4000000059604645) /* ArmorModVsCold */
     , (450450,  17, 0.4000000059604645) /* ArmorModVsFire */
     , (450450,  18,       1) /* ArmorModVsAcid */
     , (450450,  19, 0.6000000238418579) /* ArmorModVsElectric */
     , (450450,  37,       1) /* BuyPrice */
     , (450450,  38,       1) /* SellPrice */
     , (450450,  39, 1.600000023841858) /* DefaultScale */
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
VALUES (450450,   1,   33560267) /* Setup */
     , (450450,   2,  150995214) /* MotionTable */
     , (450450,   3,  536870913) /* SoundTable */
     , (450450,   8,  100673074) /* Icon */
     , (450450,   9,   83890463) /* EyesTexture */
     , (450450,  10,   83890528) /* NoseTexture */
     , (450450,  11,   83890641) /* MouthTexture */
     , (450450,  12,   83886668) /* DefaultEyesTexture */
     , (450450,  13,   83886837) /* DefaultNoseTexture */
     , (450450,  14,   83886684) /* DefaultMouthTexture */
     , (450450,  15,   67116999) /* HairPalette */
     , (450450,  16,   67109565) /* EyesPalette */
     , (450450,  17,   67110047) /* SkinPalette */
     , (450450,  18,   16795643) /* HeadObject */
     , (450450,  22,  872415236) /* PhysicsEffectTable */
     , (450450,  57,    1000002) /* AlternateCurrency */;

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
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 318767239 /* Wave */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,   0.25, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 318767229 /* BowDeep */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,  0.375, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 318767238 /* Shrug */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (450450,  2 /* Vendor */,    0.5, NULL, NULL, NULL, NULL, 5 /* Heartbeat */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 318767235 /* Nod */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (450450, 4, 450531,  0, 0, 0, False) /* Create  (450531) for Shop */
     , (450450, 4, 450009,  0, 0, 0, False) /* Create  (450009) for Shop */
     , (450450, 4, 450010,  0, 0, 0, False) /* Create  (450010) for Shop */
     , (450450, 4, 450011,  0, 0, 0, False) /* Create  (450011) for Shop */
     , (450450, 4, 450012,  0, 0, 0, False) /* Create  (450012) for Shop */
     , (450450, 4, 450013,  0, 0, 0, False) /* Create  (450013) for Shop */
     , (450450, 4, 450014,  0, 0, 0, False) /* Create  (450014) for Shop */
     , (450450, 4, 450015,  0, 0, 0, False) /* Create  (450015) for Shop */
     , (450450, 4, 450016,  0, 0, 0, False) /* Create  (450016) for Shop */
     , (450450, 4, 450017,  0, 0, 0, False) /* Create  (450017) for Shop */
     , (450450, 4, 450018,  0, 0, 0, False) /* Create  (450018) for Shop */
     , (450450, 4, 450019,  0, 0, 0, False) /* Create  (450019) for Shop */
     , (450450, 4, 450021,  0, 0, 0, False) /* Create  (450021) for Shop */
     , (450450, 4, 450022,  0, 0, 0, False) /* Create  (450022) for Shop */
     , (450450, 4, 450023,  0, 0, 0, False) /* Create  (450023) for Shop */
     , (450450, 4, 450024,  0, 0, 0, False) /* Create  (450024) for Shop */
     , (450450, 4, 450025,  0, 0, 0, False) /* Create  (450025) for Shop */
     , (450450, 4, 450026,  0, 0, 0, False) /* Create  (450026) for Shop */
     , (450450, 4, 450027,  0, 0, 0, False) /* Create  (450027) for Shop */
     , (450450, 4, 450028,  0, 0, 0, False) /* Create  (450028) for Shop */
     , (450450, 4, 450029,  0, 0, 0, False) /* Create  (450029) for Shop */
     , (450450, 4, 450030,  0, 0, 0, False) /* Create  (450030) for Shop */
     , (450450, 4, 450040,  0, 0, 0, False) /* Create  (450040) for Shop */
     , (450450, 4, 450032,  0, 0, 0, False) /* Create  (450032) for Shop */
     , (450450, 4, 450033,  0, 0, 0, False) /* Create  (450033) for Shop */
     , (450450, 4, 450034,  0, 0, 0, False) /* Create  (450034) for Shop */
     , (450450, 4, 450035,  0, 0, 0, False) /* Create  (450035) for Shop */
     , (450450, 4, 450036,  0, 0, 0, False) /* Create  (450036) for Shop */
     , (450450, 4, 450037,  0, 0, 0, False) /* Create  (450037) for Shop */
     , (450450, 4, 450038,  0, 0, 0, False) /* Create  (450038) for Shop */
     , (450450, 4, 450039,  0, 0, 0, False) /* Create  (450039) for Shop */
     , (450450, 4, 450031,  0, 0, 0, False) /* Create  (450031) for Shop */
     , (450450, 4, 450041,  0, 0, 0, False) /* Create  (450041) for Shop */
     , (450450, 4, 450042,  0, 0, 0, False) /* Create  (450042) for Shop */
     , (450450, 4, 450043,  0, 0, 0, False) /* Create  (450043) for Shop */
     , (450450, 4, 450044,  0, 0, 0, False) /* Create  (450044) for Shop */
     , (450450, 4, 450045,  0, 0, 0, False) /* Create  (450045) for Shop */
     , (450450, 4, 450046,  0, 0, 0, False) /* Create  (450046) for Shop */
     , (450450, 4, 450047,  0, 0, 0, False) /* Create  (450047) for Shop */
     , (450450, 4, 450048,  0, 0, 0, False) /* Create  (450048) for Shop */
     , (450450, 4, 450049,  0, 0, 0, False) /* Create  (450049) for Shop */
     , (450450, 4, 450050,  0, 0, 0, False) /* Create  (450050) for Shop */
     , (450450, 4, 450051,  0, 0, 0, False) /* Create  (450051) for Shop */
     , (450450, 4, 450052,  0, 0, 0, False) /* Create  (450052) for Shop */
     , (450450, 4, 450053,  0, 0, 0, False) /* Create  (450053) for Shop */
     , (450450, 4, 1005851,  0, 0, 0, False) /* Create  (1005851) for Shop */
     , (450450, 4, 450055,  0, 0, 0, False) /* Create  (450055) for Shop */
     , (450450, 4, 450540,  0, 0, 0, False) /* Create  (450540) for Shop */
     , (450450, 4, 450541,  0, 0, 0, False) /* Create  (450541) for Shop */
     , (450450, 4, 450543,  0, 0, 0, False) /* Create  (450543) for Shop */
     , (450450, 4, 450544,  0, 0, 0, False) /* Create  (450544) for Shop */
     , (450450, 4, 450545,  0, 0, 0, False) /* Create  (450545) for Shop */
     , (450450, 4, 450546,  0, 0, 0, False) /* Create  (450546) for Shop */
     , (450450, 4, 450547,  0, 0, 0, False) /* Create  (450547) for Shop */
     , (450450, 4, 450548,  0, 0, 0, False) /* Create  (450548) for Shop */
     , (450450, 4, 450549,  0, 0, 0, False) /* Create  (450549) for Shop */
     , (450450, 4, 450058,  0, 0, 0, False) /* Create  (450058) for Shop */
     , (450450, 4, 450059,  0, 0, 0, False) /* Create  (450059) for Shop */
     , (450450, 4, 450060,  0, 0, 0, False) /* Create  (450060) for Shop */
     , (450450, 4, 450061,  0, 0, 0, False) /* Create  (450061) for Shop */
     , (450450, 4, 450062,  0, 0, 0, False) /* Create  (450062) for Shop */
     , (450450, 4, 450063,  0, 0, 0, False) /* Create  (450063) for Shop */
     , (450450, 4, 450064,  0, 0, 0, False) /* Create  (450064) for Shop */
     , (450450, 4, 450065,  0, 0, 0, False) /* Create  (450065) for Shop */
     , (450450, 4, 450066,  0, 0, 0, False) /* Create  (450066) for Shop */
     , (450450, 4, 450067,  0, 0, 0, False) /* Create  (450067) for Shop */
     , (450450, 4, 450068,  0, 0, 0, False) /* Create  (450068) for Shop */
     , (450450, 4, 450214,  0, 0, 0, False) /* Create  (450214) for Shop */
     , (450450, 4, 450215,  0, 0, 0, False) /* Create  (450215) for Shop */
     , (450450, 4, 450216,  0, 0, 0, False) /* Create  (450216) for Shop */
     , (450450, 4, 450217,  0, 0, 0, False) /* Create  (450217) for Shop */
     , (450450, 4, 450218,  0, 0, 0, False) /* Create  (450218) for Shop */
     , (450450, 4, 450222,  0, 0, 0, False) /* Create  (450222) for Shop */
     , (450450, 4, 450223,  0, 0, 0, False) /* Create  (450223) for Shop */
     , (450450, 4, 450224,  0, 0, 0, False) /* Create  (450224) for Shop */
     , (450450, 4, 450225,  0, 0, 0, False) /* Create  (450225) for Shop */
     , (450450, 4, 450226,  0, 0, 0, False) /* Create  (450226) for Shop */
     , (450450, 4, 450227,  0, 0, 0, False) /* Create  (450227) for Shop */
     , (450450, 4, 450228,  0, 0, 0, False) /* Create  (450228) for Shop */
     , (450450, 4, 450229,  0, 0, 0, False) /* Create  (450229) for Shop */
     , (450450, 4, 450230,  0, 0, 0, False) /* Create  (450230) for Shop */
     , (450450, 4, 450231,  0, 0, 0, False) /* Create  (450231) for Shop */
     , (450450, 4, 450232,  0, 0, 0, False) /* Create  (450232) for Shop */
     , (450450, 4, 450233,  0, 0, 0, False) /* Create  (450233) for Shop */
     , (450450, 4, 450234,  0, 0, 0, False) /* Create  (450234) for Shop */
     , (450450, 4, 450235,  0, 0, 0, False) /* Create  (450235) for Shop */
     , (450450, 4, 450237,  0, 0, 0, False) /* Create  (450237) for Shop */
     , (450450, 4, 450238,  0, 0, 0, False) /* Create  (450238) for Shop */
     , (450450, 4, 450239,  0, 0, 0, False) /* Create  (450239) for Shop */
     , (450450, 4, 450240,  0, 0, 0, False) /* Create  (450240) for Shop */
     , (450450, 4, 450241,  0, 0, 0, False) /* Create  (450241) for Shop */
     , (450450, 4, 450242,  0, 0, 0, False) /* Create  (450242) for Shop */
     , (450450, 4, 450243,  0, 0, 0, False) /* Create  (450243) for Shop */
     , (450450, 4, 450244,  0, 0, 0, False) /* Create  (450244) for Shop */
     , (450450, 4, 450245,  0, 0, 0, False) /* Create  (450245) for Shop */
     , (450450, 4, 450250,  0, 0, 0, False) /* Create  (450250) for Shop */
     , (450450, 4, 450247,  0, 0, 0, False) /* Create  (450247) for Shop */
     , (450450, 4, 450248,  0, 0, 0, False) /* Create  (450248) for Shop */
     , (450450, 4, 450249,  0, 0, 0, False) /* Create  (450249) for Shop */
     , (450450, 4, 450251,  0, 0, 0, False) /* Create  (450251) for Shop */
     , (450450, 4, 450252,  0, 0, 0, False) /* Create  (450252) for Shop */
     , (450450, 4, 450253,  0, 0, 0, False) /* Create  (450253) for Shop */
     , (450450, 4, 450254,  0, 0, 0, False) /* Create  (450254) for Shop */
     , (450450, 4, 450255,  0, 0, 0, False) /* Create  (450255) for Shop */
     , (450450, 4, 450550,  0, 0, 0, False) /* Create  (450550) for Shop */
     , (450450, 4, 1010731,  0, 0, 0, False) /* Create  (1010731) for Shop */
     , (450450, 4, 1009047,  0, 0, 0, False) /* Create  (1009047) for Shop */
     , (450450, 4, 1009064,  0, 0, 0, False) /* Create  (1009064) for Shop */
     , (450450, 4, 1022080,  0, 0, 0, False) /* Create  (1022080) for Shop */
     , (450450, 4, 1022256,  0, 0, 0, False) /* Create  (1022256) for Shop */
     , (450450, 4, 1023774,  0, 0, 0, False) /* Create  (1023774) for Shop */
     , (450450, 4, 1025703,  0, 0, 0, False) /* Create  (1025703) for Shop */
     , (450450, 4, 1025895,  0, 0, 0, False) /* Create  (1025895) for Shop */
     , (450450, 4, 1027839,  0, 0, 0, False) /* Create  (1027839) for Shop */
     , (450450, 4, 1027840,  0, 0, 0, False) /* Create  (1027840) for Shop */
     , (450450, 4, 1027841,  0, 0, 0, False) /* Create  (1027841) for Shop */
     , (450450, 4, 1030872,  0, 0, 0, False) /* Create  (1030872) for Shop */
     , (450450, 4, 1031330,  0, 0, 0, False) /* Create  (1031330) for Shop */
     , (450450, 4, 1031331,  0, 0, 0, False) /* Create  (1031331) for Shop */
     , (450450, 4, 1031332,  0, 0, 0, False) /* Create  (1031332) for Shop */
     , (450450, 4, 1031333,  0, 0, 0, False) /* Create  (1031333) for Shop */
     , (450450, 4, 1036229,  0, 0, 0, False) /* Create  (1036229) for Shop */
     , (450450, 4, 1036230,  0, 0, 0, False) /* Create  (1036230) for Shop */
     , (450450, 4, 1051989,  0, 0, 0, False) /* Create  (1051989) for Shop */
     , (450450, 4, 1052444,  0, 0, 0, False) /* Create  (1052444) for Shop */
     , (450450, 4, 1052514,  0, 0, 0, False) /* Create  (1052514) for Shop */
     , (450450, 4, 1052699,  0, 0, 0, False) /* Create  (1052699) for Shop */
     , (450450, 4, 4200170,  0, 0, 0, False) /* Create  (4200170) for Shop */
     , (450450, 4, 4200172,  0, 0, 0, False) /* Create  (4200172) for Shop */
     , (450450, 4, 4200173,  0, 0, 0, False) /* Create  (4200173) for Shop */
     , (450450, 4, 4200171,  0, 0, 0, False) /* Create  (4200171) for Shop */
     , (450450, 4, 450079,  0, 0, 0, False) /* Create  (450079) for Shop */
     , (450450, 4, 450080,  0, 0, 0, False) /* Create  (450080) for Shop */
     , (450450, 4, 450081,  0, 0, 0, False) /* Create  (450081) for Shop */
     , (450450, 4, 450082,  0, 0, 0, False) /* Create  (450082) for Shop */
     , (450450, 4, 450083,  0, 0, 0, False) /* Create  (450083) for Shop */
     , (450450, 4, 450084,  0, 0, 0, False) /* Create  (450084) for Shop */
     , (450450, 4, 450085,  0, 0, 0, False) /* Create  (450085) for Shop */
     , (450450, 4, 450086,  0, 0, 0, False) /* Create  (450086) for Shop */
     , (450450, 4, 450087,  0, 0, 0, False) /* Create  (450087) for Shop */
     , (450450, 4, 450088,  0, 0, 0, False) /* Create  (450088) for Shop */
     , (450450, 4, 450089,  0, 0, 0, False) /* Create  (450089) for Shop */
     , (450450, 4, 450090,  0, 0, 0, False) /* Create  (450090) for Shop */
     , (450450, 4, 1030372,  0, 0, 0, False) /* Create  (1030372) for Shop */
     , (450450, 4, 450212,  0, 0, 0, False) /* Create  (450212) for Shop */
     , (450450, 4, 450213,  0, 0, 0, False) /* Create  (450213) for Shop */
     , (450450, 4, 450091,  0, 0, 0, False) /* Create  (450091) for Shop */
     , (450450, 4, 450092,  0, 0, 0, False) /* Create  (450092) for Shop */
     , (450450, 4, 450093,  0, 0, 0, False) /* Create  (450093) for Shop */
     , (450450, 4, 450094,  0, 0, 0, False) /* Create  (450094) for Shop */
     , (450450, 4, 450096,  0, 0, 0, False) /* Create  (450096) for Shop */
     , (450450, 4, 450097,  0, 0, 0, False) /* Create  (450097) for Shop */
     , (450450, 4, 450105,  0, 0, 0, False) /* Create  (450105) for Shop */
     , (450450, 4, 450099,  0, 0, 0, False) /* Create  (450099) for Shop */
     , (450450, 4, 450377,  0, 0, 0, False) /* Create  (450377) for Shop */
     , (450450, 4, 450102,  0, 0, 0, False) /* Create  (450102) for Shop */
     , (450450, 4, 450101,  0, 0, 0, False) /* Create  (450101) for Shop */
     , (450450, 4, 450103,  0, 0, 0, False) /* Create  (450103) for Shop */
     , (450450, 4, 450104,  0, 0, 0, False) /* Create  (450104) for Shop */
     , (450450, 4, 450107,  0, 0, 0, False) /* Create  (450107) for Shop */
     , (450450, 4, 450108,  0, 0, 0, False) /* Create  (450108) for Shop */
     , (450450, 4, 450367,  0, 0, 0, False) /* Create  (450367) for Shop */
     , (450450, 4, 450369,  0, 0, 0, False) /* Create  (450369) for Shop */
     , (450450, 4, 450370,  0, 0, 0, False) /* Create  (450370) for Shop */
     , (450450, 4, 450371,  0, 0, 0, False) /* Create  (450371) for Shop */
     , (450450, 4, 450372,  0, 0, 0, False) /* Create  (450372) for Shop */
     , (450450, 4, 450373,  0, 0, 0, False) /* Create  (450373) for Shop */
     , (450450, 4, 450374,  0, 0, 0, False) /* Create  (450374) for Shop */
     , (450450, 4, 450376,  0, 0, 0, False) /* Create  (450376) for Shop */
     , (450450, 4, 450375,  0, 0, 0, False) /* Create  (450375) for Shop */
     , (450450, 4, 1035982,  0, 0, 0, False) /* Create  (1035982) for Shop */
     , (450450, 4, 1036227,  0, 0, 0, False) /* Create  (1036227) for Shop */
     , (450450, 4, 1036228,  0, 0, 0, False) /* Create  (1036228) for Shop */
     , (450450, 4, 1036254,  0, 0, 0, False) /* Create  (1036254) for Shop */
     , (450450, 4, 1038922,  0, 0, 0, False) /* Create  (1038922) for Shop */
     , (450450, 4, 1043130,  0, 0, 0, False) /* Create  (1043130) for Shop */
     , (450450, 4, 1043131,  0, 0, 0, False) /* Create  (1043131) for Shop */
     , (450450, 4, 1043141,  0, 0, 0, False) /* Create  (1043141) for Shop */
     , (450450, 4, 450185,  0, 0, 0, False) /* Create  (450185) for Shop */
     , (450450, 4, 1030303,  0, 0, 0, False) /* Create  (1030303) for Shop */
     , (450450, 4, 1030351,  0, 0, 0, False) /* Create  (1030351) for Shop */
     , (450450, 4, 1030304,  0, 0, 0, False) /* Create  (1030304) for Shop */
     , (450450, 4, 450211,  0, 0, 0, False) /* Create  (450211) for Shop */
     , (450450, 4, 4200160,  0, 0, 0, False) /* Create  (4200160) for Shop */
     , (450450, 4, 4200161,  0, 0, 0, False) /* Create  (4200161) for Shop */
     , (450450, 4, 4200162,  0, 0, 0, False) /* Create  (4200162) for Shop */
     , (450450, 4, 4200163,  0, 0, 0, False) /* Create  (4200163) for Shop */
     , (450450, 4, 4200165,  0, 0, 0, False) /* Create  (4200165) for Shop */
     , (450450, 4, 450210,  0, 0, 0, False) /* Create  (450210) for Shop */
     , (450450, 4, 450186,  0, 0, 0, False) /* Create  (450186) for Shop */
     , (450450, 4, 450187,  0, 0, 0, False) /* Create  (450187) for Shop */
     , (450450, 4, 450190,  0, 0, 0, False) /* Create  (450190) for Shop */
     , (450450, 4, 450191,  0, 0, 0, False) /* Create  (450191) for Shop */
     , (450450, 4, 450192,  0, 0, 0, False) /* Create  (450192) for Shop */
     , (450450, 4, 450188,  0, 0, 0, False) /* Create  (450188) for Shop */
     , (450450, 4, 450189,  0, 0, 0, False) /* Create  (450189) for Shop */
     , (450450, 4, 1030313,  0, 0, 0, False) /* Create  (1030313) for Shop */
     , (450450, 4, 1030339,  0, 0, 0, False) /* Create  (1030339) for Shop */
     , (450450, 4, 1030323,  0, 0, 0, False) /* Create  (1030323) for Shop */
     , (450450, 4, 4200122,  0, 0, 0, False) /* Create  (4200122) for Shop */
     , (450450, 4, 4200118,  0, 0, 0, False) /* Create  (4200118) for Shop */
     , (450450, 4, 450193,  0, 0, 0, False) /* Create  (450193) for Shop */
     , (450450, 4, 450194,  0, 0, 0, False) /* Create  (450194) for Shop */
     , (450450, 4, 450195,  0, 0, 0, False) /* Create  (450195) for Shop */
     , (450450, 4, 450196,  0, 0, 0, False) /* Create  (450196) for Shop */
     , (450450, 4, 450197,  0, 0, 0, False) /* Create  (450197) for Shop */
     , (450450, 4, 450198,  0, 0, 0, False) /* Create  (450198) for Shop */
     , (450450, 4, 450199,  0, 0, 0, False) /* Create  (450199) for Shop */
     , (450450, 4, 450200,  0, 0, 0, False) /* Create  (450200) for Shop */
     , (450450, 4, 1030332,  0, 0, 0, False) /* Create  (1030332) for Shop */
     , (450450, 4, 450201,  0, 0, 0, False) /* Create  (450201) for Shop */
     , (450450, 4, 450202,  0, 0, 0, False) /* Create  (450202) for Shop */
     , (450450, 4, 450203,  0, 0, 0, False) /* Create  (450203) for Shop */
     , (450450, 4, 450204,  0, 0, 0, False) /* Create  (450204) for Shop */
     , (450450, 4, 450205,  0, 0, 0, False) /* Create  (450205) for Shop */
     , (450450, 4, 450206,  0, 0, 0, False) /* Create  (450206) for Shop */
     , (450450, 4, 450207,  0, 0, 0, False) /* Create  (450207) for Shop */
     , (450450, 4, 450208,  0, 0, 0, False) /* Create  (450208) for Shop */
     , (450450, 4, 450209,  0, 0, 0, False) /* Create  (450209) for Shop */
     , (450450, 4, 1030342,  0, 0, 0, False) /* Create  (1030342) for Shop */
     , (450450, 4, 1030343,  0, 0, 0, False) /* Create  (1030343) for Shop */
     , (450450, 4, 450221,  0, 0, 0, False) /* Create  (450221) for Shop */
     , (450450, 4, 1042662,  0, 0, 0, False) /* Create  (1042662) for Shop */
     , (450450, 4, 1042663,  0, 0, 0, False) /* Create  (1042663) for Shop */
     , (450450, 4, 450219,  0, 0, 0, False) /* Create  (450219) for Shop */
     , (450450, 4, 4200113,  0, 0, 0, False) /* Create  (4200113) for Shop */
     , (450450, 4, 450220,  0, 0, 0, False) /* Create  (450220) for Shop */
     , (450450, 4, 1008473,  0, 0, 0, False) /* Create  (1008473) for Shop */
     , (450450, 4, 1026593,  0, 0, 0, False) /* Create  (1026593) for Shop */
     , (450450, 4, 1032499,  0, 0, 0, False) /* Create  (1032499) for Shop */
     , (450450, 4, 1024557,  0, 0, 0, False) /* Create  (1024557) for Shop */
     , (450450, 4, 1030866,  0, 0, 0, False) /* Create  (1030866) for Shop */
     , (450450, 4, 1035547,  0, 0, 0, False) /* Create  (1035547) for Shop */
     , (450450, 4, 1034024,  0, 0, 0, False) /* Create  (1034024) for Shop */
     , (450450, 4, 1028490,  0, 0, 0, False) /* Create  (1028490) for Shop */
     , (450450, 4, 1008788,  0, 0, 0, False) /* Create  (1008788) for Shop */
     , (450450, 4, 1023536,  0, 0, 0, False) /* Create  (1023536) for Shop */
     , (450450, 4, 1026031,  0, 0, 0, False) /* Create  (1026031) for Shop */
     , (450450, 4, 1025904,  0, 0, 0, False) /* Create  (1025904) for Shop */
     , (450450, 4, 1028218,  0, 0, 0, False) /* Create  (1028218) for Shop */
     , (450450, 4, 1043046,  0, 0, 0, False) /* Create  (1043046) for Shop */
     , (450450, 4, 1008799,  0, 0, 0, False) /* Create  (1008799) for Shop */
     , (450450, 4, 1011329,  0, 0, 0, False) /* Create  (1011329) for Shop */
     , (450450, 4, 1011431,  0, 0, 0, False) /* Create  (1011431) for Shop */
     , (450450, 4, 1020227,  0, 0, 0, False) /* Create  (1020227) for Shop */
     , (450450, 4, 1026599,  0, 0, 0, False) /* Create  (1026599) for Shop */
     , (450450, 4, 1028067,  0, 0, 0, False) /* Create  (1028067) for Shop */
     , (450450, 4, 1023522,  0, 0, 0, False) /* Create  (1023522) for Shop */
     , (450450, 4, 1035804,  0, 0, 0, False) /* Create  (1035804) for Shop */
     , (450450, 4, 1035949,  0, 0, 0, False) /* Create  (1035949) for Shop */
     , (450450, 4, 450595,  0, 0, 0, False) /* Create  (450595) for Shop */
     , (450450, 4, 450596,  0, 0, 0, False) /* Create  (450596) for Shop */
     , (450450, 4, 1040089,  0, 0, 0, False) /* Create  (1040089) for Shop */
     , (450450, 4, 1040517,  0, 0, 0, False) /* Create  (1040517) for Shop */
     , (450450, 4, 1040519,  0, 0, 0, False) /* Create  (1040519) for Shop */
     , (450450, 4, 1035297,  0, 0, 0, False) /* Create  (1035297) for Shop */
     , (450450, 4, 1031291,  0, 0, 0, False) /* Create  (1031291) for Shop */
     , (450450, 4, 10352971,  0, 0, 0, False) /* Create  (10352971) for Shop */
     , (450450, 4, 10352972,  0, 0, 0, False) /* Create  (10352972) for Shop */
     , (450450, 4, 10352973,  0, 0, 0, False) /* Create  (10352973) for Shop */
     , (450450, 4, 10352974,  0, 0, 0, False) /* Create  (10352974) for Shop */
     , (450450, 4, 10416111,  0, 0, 0, False) /* Create  (10416111) for Shop */
     , (450450, 4, 10416112,  0, 0, 0, False) /* Create  (10416112) for Shop */
     , (450450, 4, 10416113,  0, 0, 0, False) /* Create  (10416113) for Shop */
     , (450450, 4, 10416114,  0, 0, 0, False) /* Create  (10416114) for Shop */
     , (450450, 4, 10416115,  0, 0, 0, False) /* Create  (10416115) for Shop */
     , (450450, 4, 10416116,  0, 0, 0, False) /* Create  (10416116) for Shop */
     , (450450, 4, 1024028,  0, 0, 0, False) /* Create  (1024028) for Shop */
     , (450450, 4, 1023538,  0, 0, 0, False) /* Create  (1023538) for Shop */
     , (450450, 4, 1030862,  0, 0, 0, False) /* Create  (1030862) for Shop */
     , (450450, 4, 1038910,  0, 0, 0, False) /* Create  (1038910) for Shop */
     , (450450, 4, 4200137,  0, 0, 0, False) /* Create  (4200137) for Shop */
     , (450450, 4, 1025905,  0, 0, 0, False) /* Create  (1025905) for Shop */
     , (450450, 4, 1023547,  0, 0, 0, False) /* Create  (1023547) for Shop */
     , (450450, 4, 1024027,  0, 0, 0, False) /* Create  (1024027) for Shop */
     , (450450, 4, 1025906,  0, 0, 0, False) /* Create  (1025906) for Shop */
     , (450450, 4, 1029910,  0, 0, 0, False) /* Create  (1029910) for Shop */
     , (450450, 4, 1030860,  0, 0, 0, False) /* Create  (1030860) for Shop */
     , (450450, 4, 1031500,  0, 0, 0, False) /* Create  (1031500) for Shop */
     , (450450, 4, 1035407,  0, 0, 0, False) /* Create  (1035407) for Shop */
     , (450450, 4, 1038926,  0, 0, 0, False) /* Create  (1038926) for Shop */
     , (450450, 4, 1032773,  0, 0, 0, False) /* Create  (1032773) for Shop */
     , (450450, 4, 1030634,  0, 0, 0, False) /* Create  (1030634) for Shop */
     , (450450, 4, 1023539,  0, 0, 0, False) /* Create  (1023539) for Shop */
     , (450450, 4, 1035615,  0, 0, 0, False) /* Create  (1035615) for Shop */
     , (450450, 4, 4200094,  0, 0, 0, False) /* Create  (4200094) for Shop */
     , (450450, 4, 4200106,  0, 0, 0, False) /* Create  (4200106) for Shop */
     , (450450, 4, 4200108,  0, 0, 0, False) /* Create  (4200108) for Shop */
     , (450450, 4, 4200109,  0, 0, 0, False) /* Create  (4200109) for Shop */
     , (450450, 4, 4200110,  0, 0, 0, False) /* Create  (4200110) for Shop */
     , (450450, 4, 4200112,  0, 0, 0, False) /* Create  (4200112) for Shop */
     , (450450, 4, 4200115,  0, 0, 0, False) /* Create  (4200115) for Shop */
     , (450450, 4, 4200116,  0, 0, 0, False) /* Create  (4200116) for Shop */
     , (450450, 4, 4200119,  0, 0, 0, False) /* Create  (4200119) for Shop */
     , (450450, 4, 1042932,  0, 0, 0, False) /* Create  (1042932) for Shop */
     , (450450, 4, 4200166,  0, 0, 0, False) /* Create  (4200166) for Shop */
     , (450450, 4, 4200168,  0, 0, 0, False) /* Create  (4200168) for Shop */
     , (450450, 4, 4200169,  0, 0, 0, False) /* Create  (4200169) for Shop */
     , (450450, 4, 4200164,  0, 0, 0, False) /* Create  (4200164) for Shop */
     , (450450, 4, 1038923,  0, 0, 0, False) /* Create  (1038923) for Shop */
     , (450450, 4, 1033050,  0, 0, 0, False) /* Create  (1033050) for Shop */
     , (450450, 4, 1033058,  0, 0, 0, False) /* Create  (1033058) for Shop */
     , (450450, 4, 1033102,  0, 0, 0, False) /* Create  (1033102) for Shop */
     , (450450, 4, 1033121,  0, 0, 0, False) /* Create  (1033121) for Shop */
     , (450450, 4, 450551,  0, 0, 0, False) /* Create  (450551) for Shop */
     , (450450, 4, 450552,  0, 0, 0, False) /* Create  (450552) for Shop */
     , (450450, 4, 450553,  0, 0, 0, False) /* Create  (450553) for Shop */
     , (450450, 4, 450554,  0, 0, 0, False) /* Create  (450554) for Shop */
     , (450450, 4, 450555,  0, 0, 0, False) /* Create  (450555) for Shop */
     , (450450, 4, 450556,  0, 0, 0, False) /* Create  (450556) for Shop */
     , (450450, 4, 450557,  0, 0, 0, False) /* Create  (450557) for Shop */
     , (450450, 4, 450558,  0, 0, 0, False) /* Create  (450558) for Shop */
     , (450450, 4, 450301,  0, 0, 0, False) /* Create  (450301) for Shop */
     , (450450, 4, 450302,  0, 0, 0, False) /* Create  (450302) for Shop */
     , (450450, 4, 450303,  0, 0, 0, False) /* Create  (450303) for Shop */
     , (450450, 4, 450304,  0, 0, 0, False) /* Create  (450304) for Shop */
     , (450450, 4, 450305,  0, 0, 0, False) /* Create  (450305) for Shop */
     , (450450, 4, 450306,  0, 0, 0, False) /* Create  (450306) for Shop */
     , (450450, 4, 450307,  0, 0, 0, False) /* Create  (450307) for Shop */
     , (450450, 4, 450310,  0, 0, 0, False) /* Create  (450310) for Shop */
     , (450450, 4, 1037579,  0, 0, 0, False) /* Create  (1037579) for Shop */
     , (450450, 4, 1037585,  0, 0, 0, False) /* Create  (1037585) for Shop */
     , (450450, 4, 450311,  0, 0, 0, False) /* Create  (450311) for Shop */
     , (450450, 4, 450312,  0, 0, 0, False) /* Create  (450312) for Shop */
     , (450450, 4, 450313,  0, 0, 0, False) /* Create  (450313) for Shop */
     , (450450, 4, 450314,  0, 0, 0, False) /* Create  (450314) for Shop */
     , (450450, 4, 450315,  0, 0, 0, False) /* Create  (450315) for Shop */
     , (450450, 4, 450316,  0, 0, 0, False) /* Create  (450316) for Shop */
     , (450450, 4, 450317,  0, 0, 0, False) /* Create  (450317) for Shop */
     , (450450, 4, 450318,  0, 0, 0, False) /* Create  (450318) for Shop */
     , (450450, 4, 450319,  0, 0, 0, False) /* Create  (450319) for Shop */
     , (450450, 4, 1041794,  0, 0, 0, False) /* Create  (1041794) for Shop */
     , (450450, 4, 1041912,  0, 0, 0, False) /* Create  (1041912) for Shop */
     , (450450, 4, 450320,  0, 0, 0, False) /* Create  (450320) for Shop */
     , (450450, 4, 450321,  0, 0, 0, False) /* Create  (450321) for Shop */
     , (450450, 4, 450322,  0, 0, 0, False) /* Create  (450322) for Shop */
     , (450450, 4, 450323,  0, 0, 0, False) /* Create  (450323) for Shop */
     , (450450, 4, 450324,  0, 0, 0, False) /* Create  (450324) for Shop */
     , (450450, 4, 450325,  0, 0, 0, False) /* Create  (450325) for Shop */
     , (450450, 4, 450326,  0, 0, 0, False) /* Create  (450326) for Shop */
     , (450450, 4, 450327,  0, 0, 0, False) /* Create  (450327) for Shop */
     , (450450, 4, 450328,  0, 0, 0, False) /* Create  (450328) for Shop */
     , (450450, 4, 450329,  0, 0, 0, False) /* Create  (450329) for Shop */
     , (450450, 4, 450330,  0, 0, 0, False) /* Create  (450330) for Shop */
     , (450450, 4, 450331,  0, 0, 0, False) /* Create  (450331) for Shop */
     , (450450, 4, 450332,  0, 0, 0, False) /* Create  (450332) for Shop */
     , (450450, 4, 450333,  0, 0, 0, False) /* Create  (450333) for Shop */
     , (450450, 4, 450334,  0, 0, 0, False) /* Create  (450334) for Shop */
     , (450450, 4, 450335,  0, 0, 0, False) /* Create  (450335) for Shop */
     , (450450, 4, 450336,  0, 0, 0, False) /* Create  (450336) for Shop */
     , (450450, 4, 450337,  0, 0, 0, False) /* Create  (450337) for Shop */
     , (450450, 4, 450338,  0, 0, 0, False) /* Create  (450338) for Shop */
     , (450450, 4, 450339,  0, 0, 0, False) /* Create  (450339) for Shop */
     , (450450, 4, 450008,  0, 0, 0, False) /* Create  (450008) for Shop */
     , (450450, 4, 450000,  0, 0, 0, False) /* Create  (450000) for Shop */
     , (450450, 4, 450001,  0, 0, 0, False) /* Create  (450001) for Shop */
     , (450450, 4, 450002,  0, 0, 0, False) /* Create  (450002) for Shop */
     , (450450, 4, 450003,  0, 0, 0, False) /* Create  (450003) for Shop */
     , (450450, 4, 450004,  0, 0, 0, False) /* Create  (450004) for Shop */
     , (450450, 4, 450005,  0, 0, 0, False) /* Create  (450005) for Shop */
     , (450450, 4, 450006,  0, 0, 0, False) /* Create  (450006) for Shop */
     , (450450, 4, 450007,  0, 0, 0, False) /* Create  (450007) for Shop */
     , (450450, 4, 450008,  0, 0, 0, False) /* Create  (450008) for Shop */
     , (450450, 4, 450172,  0, 0, 0, False) /* Create  (450172) for Shop */
     , (450450, 4, 450173,  0, 0, 0, False) /* Create  (450173) for Shop */
     , (450450, 4, 450174,  0, 0, 0, False) /* Create  (450174) for Shop */
     , (450450, 4, 450175,  0, 0, 0, False) /* Create  (450175) for Shop */
     , (450450, 4, 450176,  0, 0, 0, False) /* Create  (450176) for Shop */
     , (450450, 4, 450177,  0, 0, 0, False) /* Create  (450177) for Shop */
     , (450450, 4, 450178,  0, 0, 0, False) /* Create  (450178) for Shop */
     , (450450, 4, 450179,  0, 0, 0, False) /* Create  (450179) for Shop */
     , (450450, 4, 450166,  0, 0, 0, False) /* Create  (450166) for Shop */
     , (450450, 4, 450167,  0, 0, 0, False) /* Create  (450167) for Shop */
     , (450450, 4, 450164,  0, 0, 0, False) /* Create  (450164) for Shop */
     , (450450, 4, 450165,  0, 0, 0, False) /* Create  (450165) for Shop */
     , (450450, 4, 450168,  0, 0, 0, False) /* Create  (450168) for Shop */
     , (450450, 4, 450169,  0, 0, 0, False) /* Create  (450169) for Shop */
     , (450450, 4, 450162,  0, 0, 0, False) /* Create  (450162) for Shop */
     , (450450, 4, 450163,  0, 0, 0, False) /* Create  (450163) for Shop */
     , (450450, 4, 4200096,  0, 0, 0, False) /* Create  (4200096) for Shop */
     , (450450, 4, 450077,  0, 0, 0, False) /* Create  (450077) for Shop */
     , (450450, 4, 450110,  0, 0, 0, False) /* Create  (450110) for Shop */
     , (450450, 4, 450112,  0, 0, 0, False) /* Create  (450112) for Shop */
     , (450450, 4, 450113,  0, 0, 0, False) /* Create  (450113) for Shop */
     , (450450, 4, 450114,  0, 0, 0, False) /* Create  (450114) for Shop */
     , (450450, 4, 450115,  0, 0, 0, False) /* Create  (450115) for Shop */
     , (450450, 4, 450116,  0, 0, 0, False) /* Create  (450116) for Shop */
     , (450450, 4, 450121,  0, 0, 0, False) /* Create  (450121) for Shop */
     , (450450, 4, 450122,  0, 0, 0, False) /* Create  (450122) for Shop */
     , (450450, 4, 450123,  0, 0, 0, False) /* Create  (450123) for Shop */
     , (450450, 4, 450124,  0, 0, 0, False) /* Create  (450124) for Shop */
     , (450450, 4, 450125,  0, 0, 0, False) /* Create  (450125) for Shop */
     , (450450, 4, 1049901,  0, 0, 0, False) /* Create  (1049901) for Shop */
     , (450450, 4, 1049905,  0, 0, 0, False) /* Create  (1049905) for Shop */
     , (450450, 4, 1049909,  0, 0, 0, False) /* Create  (1049909) for Shop */
     , (450450, 4, 1049913,  0, 0, 0, False) /* Create  (1049913) for Shop */
     , (450450, 4, 1049917,  0, 0, 0, False) /* Create  (1049917) for Shop */
     , (450450, 4, 1049921,  0, 0, 0, False) /* Create  (1049921) for Shop */
     , (450450, 4, 1049925,  0, 0, 0, False) /* Create  (1049925) for Shop */
     , (450450, 4, 1049929,  0, 0, 0, False) /* Create  (1049929) for Shop */
     , (450450, 4, 1049933,  0, 0, 0, False) /* Create  (1049933) for Shop */
     , (450450, 4, 1006600,  0, 0, 0, False) /* Create  (1006600) for Shop */
     , (450450, 4, 1006606,  0, 0, 0, False) /* Create  (1006606) for Shop */
     , (450450, 4, 1006594,  0, 0, 0, False) /* Create  (1006594) for Shop */
     , (450450, 4, 1006603,  0, 0, 0, False) /* Create  (1006603) for Shop */
     , (450450, 4, 1006615,  0, 0, 0, False) /* Create  (1006615) for Shop */
     , (450450, 4, 1006609,  0, 0, 0, False) /* Create  (1006609) for Shop */
     , (450450, 4, 1006801,  0, 0, 0, False) /* Create  (1006801) for Shop */
     , (450450, 4, 1006799,  0, 0, 0, False) /* Create  (1006799) for Shop */
     , (450450, 4, 1006797,  0, 0, 0, False) /* Create  (1006797) for Shop */
     , (450450, 4, 1006800,  0, 0, 0, False) /* Create  (1006800) for Shop */
     , (450450, 4, 1006802,  0, 0, 0, False) /* Create  (1006802) for Shop */
     , (450450, 4, 1006804,  0, 0, 0, False) /* Create  (1006804) for Shop */
     , (450450, 4, 450133,  0, 0, 0, False) /* Create  (450133) for Shop */
     , (450450, 4, 450134,  0, 0, 0, False) /* Create  (450134) for Shop */
     , (450450, 4, 450135,  0, 0, 0, False) /* Create  (450135) for Shop */
     , (450450, 4, 450136,  0, 0, 0, False) /* Create  (450136) for Shop */
     , (450450, 4, 450137,  0, 0, 0, False) /* Create  (450137) for Shop */
     , (450450, 4, 450138,  0, 0, 0, False) /* Create  (450138) for Shop */
     , (450450, 4, 450139,  0, 0, 0, False) /* Create  (450139) for Shop */
     , (450450, 4, 450140,  0, 0, 0, False) /* Create  (450140) for Shop */
     , (450450, 4, 450141,  0, 0, 0, False) /* Create  (450141) for Shop */
     , (450450, 4, 450142,  0, 0, 0, False) /* Create  (450142) for Shop */
     , (450450, 4, 450143,  0, 0, 0, False) /* Create  (450143) for Shop */
     , (450450, 4, 450144,  0, 0, 0, False) /* Create  (450144) for Shop */
     , (450450, 4, 450145,  0, 0, 0, False) /* Create  (450145) for Shop */
     , (450450, 4, 450146,  0, 0, 0, False) /* Create  (450146) for Shop */
     , (450450, 4, 450147,  0, 0, 0, False) /* Create  (450147) for Shop */
     , (450450, 4, 450148,  0, 0, 0, False) /* Create  (450148) for Shop */
     , (450450, 4, 450149,  0, 0, 0, False) /* Create  (450149) for Shop */
     , (450450, 4, 450150,  0, 0, 0, False) /* Create  (450150) for Shop */
     , (450450, 4, 450151,  0, 0, 0, False) /* Create  (450151) for Shop */
     , (450450, 4, 450152,  0, 0, 0, False) /* Create  (450152) for Shop */
     , (450450, 4, 450153,  0, 0, 0, False) /* Create  (450153) for Shop */
     , (450450, 4, 450154,  0, 0, 0, False) /* Create  (450154) for Shop */
     , (450450, 4, 450155,  0, 0, 0, False) /* Create  (450155) for Shop */
     , (450450, 4, 450156,  0, 0, 0, False) /* Create  (450156) for Shop */
     , (450450, 4, 450157,  0, 0, 0, False) /* Create  (450157) for Shop */
     , (450450, 4, 450158,  0, 0, 0, False) /* Create  (450158) for Shop */
     , (450450, 4, 450159,  0, 0, 0, False) /* Create  (450159) for Shop */
     , (450450, 4, 450160,  0, 0, 0, False) /* Create  (450160) for Shop */
     , (450450, 4, 450161,  0, 0, 0, False) /* Create  (450161) for Shop */
     , (450450, 4, 450263,  0, 0, 0, False) /* Create  (450263) for Shop */
     , (450450, 4, 450256,  0, 0, 0, False) /* Create  (450256) for Shop */
     , (450450, 4, 450257,  0, 0, 0, False) /* Create  (450257) for Shop */
     , (450450, 4, 450258,  0, 0, 0, False) /* Create  (450258) for Shop */
     , (450450, 4, 450259,  0, 0, 0, False) /* Create  (450259) for Shop */
     , (450450, 4, 450264,  0, 0, 0, False) /* Create  (450264) for Shop */
     , (450450, 4, 450260,  0, 0, 0, False) /* Create  (450260) for Shop */
     , (450450, 4, 450261,  0, 0, 0, False) /* Create  (450261) for Shop */
     , (450450, 4, 450265,  0, 0, 0, False) /* Create  (450265) for Shop */
     , (450450, 4, 450262,  0, 0, 0, False) /* Create  (450262) for Shop */
     , (450450, 4, 450266,  0, 0, 0, False) /* Create  (450266) for Shop */
     , (450450, 4, 450267,  0, 0, 0, False) /* Create  (450267) for Shop */
     , (450450, 4, 450268,  0, 0, 0, False) /* Create  (450268) for Shop */
     , (450450, 4, 450285,  0, 0, 0, False) /* Create  (450285) for Shop */
     , (450450, 4, 450286,  0, 0, 0, False) /* Create  (450286) for Shop */
     , (450450, 4, 450287,  0, 0, 0, False) /* Create  (450287) for Shop */
     , (450450, 4, 450288,  0, 0, 0, False) /* Create  (450288) for Shop */
     , (450450, 4, 450289,  0, 0, 0, False) /* Create  (450289) for Shop */
     , (450450, 4, 450290,  0, 0, 0, False) /* Create  (450290) for Shop */
     , (450450, 4, 450291,  0, 0, 0, False) /* Create  (450291) for Shop */
     , (450450, 4, 450292,  0, 0, 0, False) /* Create  (450292) for Shop */
     , (450450, 4, 450293,  0, 0, 0, False) /* Create  (450293) for Shop */
     , (450450, 4, 450294,  0, 0, 0, False) /* Create  (450294) for Shop */
     , (450450, 4, 450340,  0, 0, 0, False) /* Create  (450340) for Shop */
     , (450450, 4, 450341,  0, 0, 0, False) /* Create  (450341) for Shop */
     , (450450, 4, 450342,  0, 0, 0, False) /* Create  (450342) for Shop */
     , (450450, 4, 450343,  0, 0, 0, False) /* Create  (450343) for Shop */
     , (450450, 4, 450344,  0, 0, 0, False) /* Create  (450344) for Shop */
     , (450450, 4, 450345,  0, 0, 0, False) /* Create  (450345) for Shop */
     , (450450, 4, 450346,  0, 0, 0, False) /* Create  (450346) for Shop */
     , (450450, 4, 450347,  0, 0, 0, False) /* Create  (450347) for Shop */
     , (450450, 4, 450348,  0, 0, 0, False) /* Create  (450348) for Shop */
     , (450450, 4, 450349,  0, 0, 0, False) /* Create  (450349) for Shop */
     , (450450, 4, 450350,  0, 0, 0, False) /* Create  (450350) for Shop */
     , (450450, 4, 450351,  0, 0, 0, False) /* Create  (450351) for Shop */
     , (450450, 4, 450352,  0, 0, 0, False) /* Create  (450352) for Shop */
     , (450450, 4, 450353,  0, 0, 0, False) /* Create  (450353) for Shop */
     , (450450, 4, 450354,  0, 0, 0, False) /* Create  (450354) for Shop */
     , (450450, 4, 450355,  0, 0, 0, False) /* Create  (450355) for Shop */
     , (450450, 4, 450356,  0, 0, 0, False) /* Create  (450356) for Shop */
     , (450450, 4, 450357,  0, 0, 0, False) /* Create  (450357) for Shop */
     , (450450, 4, 450358,  0, 0, 0, False) /* Create  (450358) for Shop */
     , (450450, 4, 450359,  0, 0, 0, False) /* Create  (450359) for Shop */
     , (450450, 4, 450360,  0, 0, 0, False) /* Create  (450360) for Shop */
     , (450450, 4, 450361,  0, 0, 0, False) /* Create  (450361) for Shop */
     , (450450, 4, 450362,  0, 0, 0, False) /* Create  (450362) for Shop */
     , (450450, 4, 450363,  0, 0, 0, False) /* Create  (450363) for Shop */
     , (450450, 4, 450364,  0, 0, 0, False) /* Create  (450364) for Shop */
     , (450450, 4, 450365,  0, 0, 0, False) /* Create  (450365) for Shop */
     , (450450, 4, 450366,  0, 0, 0, False) /* Create  (450366) for Shop */
     , (450450, 4, 450269,  0, 0, 0, False) /* Create  (450269) for Shop */
     , (450450, 4, 450270,  0, 0, 0, False) /* Create  (450270) for Shop */
     , (450450, 4, 450271,  0, 0, 0, False) /* Create  (450271) for Shop */
     , (450450, 4, 450272,  0, 0, 0, False) /* Create  (450272) for Shop */
     , (450450, 4, 1046345,  0, 0, 0, False) /* Create  (1046345) for Shop */
     , (450450, 4, 1046551,  0, 0, 0, False) /* Create  (1046551) for Shop */
     , (450450, 4, 1046552,  0, 0, 0, False) /* Create  (1046552) for Shop */
     , (450450, 4, 1046553,  0, 0, 0, False) /* Create  (1046553) for Shop */
     , (450450, 4, 1046615,  0, 0, 0, False) /* Create  (1046615) for Shop */
     , (450450, 4, 2046345,  0, 0, 0, False) /* Create  (2046345) for Shop */
     , (450450, 4, 2046551,  0, 0, 0, False) /* Create  (2046551) for Shop */
     , (450450, 4, 2046552,  0, 0, 0, False) /* Create  (2046552) for Shop */
     , (450450, 4, 2046553,  0, 0, 0, False) /* Create  (2046553) for Shop */
     , (450450, 4, 2046615,  0, 0, 0, False) /* Create  (2046615) for Shop */
     , (450450, 4, 450278,  0, 0, 0, False) /* Create  (450278) for Shop */
     , (450450, 4, 450279,  0, 0, 0, False) /* Create  (450279) for Shop */
     , (450450, 4, 450280,  0, 0, 0, False) /* Create  (450280) for Shop */
     , (450450, 4, 450283,  0, 0, 0, False) /* Create  (450283) for Shop */
     , (450450, 4, 450284,  0, 0, 0, False) /* Create  (450284) for Shop */
     , (450450, 4, 450275,  0, 0, 0, False) /* Create  (450275) for Shop */
     , (450450, 4, 450276,  0, 0, 0, False) /* Create  (450276) for Shop */
     , (450450, 4, 450277,  0, 0, 0, False) /* Create  (450277) for Shop */
     , (450450, 4, 105280,  0, 0, 0, False) /* Create  (105280) for Shop */
     , (450450, 4, 450532,  0, 0, 0, False) /* Create  (450532) for Shop */
     , (450450, 4, 450533,  0, 0, 0, False) /* Create  (450533) for Shop */
     , (450450, 4, 450534,  0, 0, 0, False) /* Create  (450534) for Shop */
     , (450450, 4, 450535,  0, 0, 0, False) /* Create  (450535) for Shop */
     , (450450, 4, 450536,  0, 0, 0, False) /* Create  (450536) for Shop */
     , (450450, 4, 450538,  0, 0, 0, False) /* Create  (450538) for Shop */
     , (450450, 4, 450539,  0, 0, 0, False) /* Create  (450539) for Shop */
     , (450450, 4, 450132,  0, 0, 0, False) /* Create  (450132) for Shop */
     , (450450, 4, 450273,  0, 0, 0, False) /* Create  (450273) for Shop */
     , (450450, 4, 450274,  0, 0, 0, False) /* Create  (450274) for Shop */
     , (450450, 4, 450281,  0, 0, 0, False) /* Create  (450281) for Shop */
     , (450450, 4, 450282,  0, 0, 0, False) /* Create  (450282) for Shop */
     , (450450, 4, 1032148,  0, 0, 0, False) /* Create  (1032148) for Shop */
     , (450450, 4, 4200095,  0, 0, 0, False) /* Create  (4200095) for Shop */
     , (450450, 4, 4200101,  0, 0, 0, False) /* Create  (4200101) for Shop */
     , (450450, 4, 4200102,  0, 0, 0, False) /* Create  (4200102) for Shop */
     , (450450, 4, 4200103,  0, 0, 0, False) /* Create  (4200103) for Shop */
     , (450450, 4, 1029818,  0, 0, 0, False) /* Create  (1029818) for Shop */
     , (450450, 4, 1029819,  0, 0, 0, False) /* Create  (1029819) for Shop */
     , (450450, 4, 1029820,  0, 0, 0, False) /* Create  (1029820) for Shop */
     , (450450, 4, 1029821,  0, 0, 0, False) /* Create  (1029821) for Shop */
     , (450450, 4, 1029822,  0, 0, 0, False) /* Create  (1029822) for Shop */
     , (450450, 4, 1029823,  0, 0, 0, False) /* Create  (1029823) for Shop */
     , (450450, 4, 1029824,  0, 0, 0, False) /* Create  (1029824) for Shop */
     , (450450, 4, 1029825,  0, 0, 0, False) /* Create  (1029825) for Shop */
     , (450450, 4, 1051858,  0, 0, 0, False) /* Create  (1051858) for Shop */
     , (450450, 4, 1051854,  0, 0, 0, False) /* Create  (1051854) for Shop */
     , (450450, 4, 1051855,  0, 0, 0, False) /* Create  (1051855) for Shop */
     , (450450, 4, 1051857,  0, 0, 0, False) /* Create  (1051857) for Shop */
     , (450450, 4, 1051856,  0, 0, 0, False) /* Create  (1051856) for Shop */
     , (450450, 4, 1043197,  0, 0, 0, False) /* Create  (1043197) for Shop */
     , (450450, 4, 1043040,  0, 0, 0, False) /* Create  (1043040) for Shop */
     , (450450, 4, 1025335,  0, 0, 0, False) /* Create  (1025335) for Shop */
     , (450450, 4, 1024879,  0, 0, 0, False) /* Create  (1024879) for Shop */
     , (450450, 4, 1011998,  0, 0, 0, False) /* Create  (1011998) for Shop */
     , (450450, 4, 1008153,  0, 0, 0, False) /* Create  (1008153) for Shop */
     , (450450, 4, 450128,  0, 0, 0, False) /* Create  (450128) for Shop */
     , (450450, 4, 450131,  0, 0, 0, False) /* Create  (450131) for Shop */
     , (450450, 4, 1012207,  0, 0, 0, False) /* Create  (1012207) for Shop */
     , (450450, 4, 450451,  0, 0, 0, False) /* Create  (450451) for Shop */
     , (450450, 4, 450455,  0, 0, 0, False) /* Create  (450455) for Shop */
     , (450450, 4, 450456,  0, 0, 0, False) /* Create  (450456) for Shop */
     , (450450, 4, 450457,  0, 0, 0, False) /* Create  (450457) for Shop */
     , (450450, 4, 450460,  0, 0, 0, False) /* Create  (450460) for Shop */
     , (450450, 4, 450072,  0, 0, 0, False) /* Create  (450072) for Shop */
     , (450450, 4, 4200104,  0, 0, 0, False) /* Create  (4200104) for Shop */
     , (450450, 4, 4200454,  0, 0, 0, False) /* Create  (4200454) for Shop */
     , (450450, 4, 450459,  0, 0, 0, False) /* Create  (450459) for Shop */
     , (450450, 4, 4200452,  0, 0, 0, False) /* Create  (4200452) for Shop */
     , (450450, 4, 4200105,  0, 0, 0, False) /* Create  (4200105) for Shop */
     , (450450, 4, 450458,  0, 0, 0, False) /* Create  (450458) for Shop */
     , (450450, 4, 450070,  0, 0, 0, False) /* Create  (450070) for Shop */
     , (450450, 4, 450069,  0, 0, 0, False) /* Create  (450069) for Shop */
     , (450450, 4, 450074,  0, 0, 0, False) /* Create  (450074) for Shop */
     , (450450, 4, 450071,  0, 0, 0, False) /* Create  (450071) for Shop */
     , (450450, 4, 450119,  0, 0, 0, False) /* Create  (450119) for Shop */
     , (450450, 4, 450073,  0, 0, 0, False) /* Create  (450073) for Shop */
     , (450450, 4, 1021373,  0, 0, 0, False) /* Create  (1021373) for Shop */
     , (450450, 4, 1008805,  0, 0, 0, False) /* Create  (1008805) for Shop */
     , (450450, 4, 1008806,  0, 0, 0, False) /* Create  (1008806) for Shop */
     , (450450, 4, 1008807,  0, 0, 0, False) /* Create  (1008807) for Shop */
     , (450450, 4, 1008808,  0, 0, 0, False) /* Create  (1008808) for Shop */
     , (450450, 4, 1008809,  0, 0, 0, False) /* Create  (1008809) for Shop */
     , (450450, 4, 1026498,  0, 0, 0, False) /* Create  (1026498) for Shop */
     , (450450, 4, 450075,  0, 0, 0, False) /* Create  (450075) for Shop */
     , (450450, 4, 450076,  0, 0, 0, False) /* Create  (450076) for Shop */
     , (450450, 4, 1011986,  0, 0, 0, False) /* Create  (1011986) for Shop */
     , (450450, 4, 42130741,  0, 0, 0, False) /* Create  (42130741) for Shop */
     , (450450, 4, 4200136,  0, 0, 0, False) /* Create  (4200136) for Shop */
     , (450450, 4, 1042667,  0, 0, 0, False) /* Create  (1042667) for Shop */
     , (450450, 4, 1014932,  0, 0, 0, False) /* Create  (1014932) for Shop */
     , (450450, 4, 1009622,  0, 0, 0, False) /* Create  (1009622) for Shop */
     , (450450, 4, 450111,  0, 0, 0, False) /* Create  (450111) for Shop */
     , (450450, 4, 450117,  0, 0, 0, False) /* Create  (450117) for Shop */
     , (450450, 4, 450118,  0, 0, 0, False) /* Create  (450118) for Shop */
     , (450450, 4, 10049811,  0, 0, 0, False) /* Create  (10049811) for Shop */
     , (450450, 4, 10049812,  0, 0, 0, False) /* Create  (10049812) for Shop */
     , (450450, 4, 10049813,  0, 0, 0, False) /* Create  (10049813) for Shop */
     , (450450, 4, 42149813,  0, 0, 0, False) /* Create  (42149813) for Shop */
     , (450450, 4, 450453,  0, 0, 0, False) /* Create  (450453) for Shop */
     , (450450, 4, 450295,  0, 0, 0, False) /* Create  (450295) for Shop */
     , (450450, 4, 13241334,  0, 0, 0, False) /* Create  (13241334) for Shop */
     , (450450, 4, 450512,  0, 0, 0, False) /* Create  (450512) for Shop */
     , (450450, 4, 450513,  0, 0, 0, False) /* Create  (450513) for Shop */
     , (450450, 4, 450514,  0, 0, 0, False) /* Create  (450514) for Shop */
     , (450450, 4, 450509,  0, 0, 0, False) /* Create  (450509) for Shop */
     , (450450, 4, 450510,  0, 0, 0, False) /* Create  (450510) for Shop */
     , (450450, 4, 450511,  0, 0, 0, False) /* Create  (450511) for Shop */
     , (450450, 4, 450515,  0, 0, 0, False) /* Create  (450515) for Shop */
     , (450450, 4, 450516,  0, 0, 0, False) /* Create  (450516) for Shop */
     , (450450, 4, 450517,  0, 0, 0, False) /* Create  (450517) for Shop */
     , (450450, 4, 450518,  0, 0, 0, False) /* Create  (450518) for Shop */
     , (450450, 4, 450519,  0, 0, 0, False) /* Create  (450519) for Shop */
     , (450450, 4, 450520,  0, 0, 0, False) /* Create  (450520) for Shop */
     , (450450, 4, 450521,  0, 0, 0, False) /* Create  (450521) for Shop */
     , (450450, 4, 450522,  0, 0, 0, False) /* Create  (450522) for Shop */
     , (450450, 4, 450523,  0, 0, 0, False) /* Create  (450523) for Shop */
     , (450450, 4, 450526,  0, 0, 0, False) /* Create  (450526) for Shop */
     , (450450, 4, 450527,  0, 0, 0, False) /* Create  (450527) for Shop */
     , (450450, 4, 450529,  0, 0, 0, False) /* Create  (450529) for Shop */
     , (450450, 4, 450471,  0, 0, 0, False) /* Create  (450471) for Shop */
     , (450450, 4, 450472,  0, 0, 0, False) /* Create  (450472) for Shop */
     , (450450, 4, 450473,  0, 0, 0, False) /* Create  (450473) for Shop */
     , (450450, 4, 450474,  0, 0, 0, False) /* Create  (450474) for Shop */
     , (450450, 4, 450475,  0, 0, 0, False) /* Create  (450475) for Shop */
     , (450450, 4, 450476,  0, 0, 0, False) /* Create  (450476) for Shop */
     , (450450, 4, 450477,  0, 0, 0, False) /* Create  (450477) for Shop */
     , (450450, 4, 450478,  0, 0, 0, False) /* Create  (450478) for Shop */
     , (450450, 4, 450479,  0, 0, 0, False) /* Create  (450479) for Shop */
     , (450450, 4, 450480,  0, 0, 0, False) /* Create  (450480) for Shop */
     , (450450, 4, 450481,  0, 0, 0, False) /* Create  (450481) for Shop */
     , (450450, 4, 450482,  0, 0, 0, False) /* Create  (450482) for Shop */
     , (450450, 4, 450483,  0, 0, 0, False) /* Create  (450483) for Shop */
     , (450450, 4, 450484,  0, 0, 0, False) /* Create  (450484) for Shop */
     , (450450, 4, 450485,  0, 0, 0, False) /* Create  (450485) for Shop */
     , (450450, 4, 450486,  0, 0, 0, False) /* Create  (450486) for Shop */
     , (450450, 4, 450487,  0, 0, 0, False) /* Create  (450487) for Shop */
     , (450450, 4, 450488,  0, 0, 0, False) /* Create  (450488) for Shop */
     , (450450, 4, 450489,  0, 0, 0, False) /* Create  (450489) for Shop */
     , (450450, 4, 450490,  0, 0, 0, False) /* Create  (450490) for Shop */
     , (450450, 4, 450508,  0, 0, 0, False) /* Create  (450508) for Shop */
     , (450450, 4, 450491,  0, 0, 0, False) /* Create  (450491) for Shop */
     , (450450, 4, 450492,  0, 0, 0, False) /* Create  (450492) for Shop */
     , (450450, 4, 450537,  0, 0, 0, False) /* Create  (450537) for Shop */
     , (450450, 4, 450493,  0, 0, 0, False) /* Create  (450493) for Shop */
     , (450450, 4, 450494,  0, 0, 0, False) /* Create  (450494) for Shop */
     , (450450, 4, 450495,  0, 0, 0, False) /* Create  (450495) for Shop */
     , (450450, 4, 450496,  0, 0, 0, False) /* Create  (450496) for Shop */
     , (450450, 4, 450497,  0, 0, 0, False) /* Create  (450497) for Shop */
     , (450450, 4, 450498,  0, 0, 0, False) /* Create  (450498) for Shop */
     , (450450, 4, 450499,  0, 0, 0, False) /* Create  (450499) for Shop */
     , (450450, 4, 450500,  0, 0, 0, False) /* Create  (450500) for Shop */
     , (450450, 4, 450501,  0, 0, 0, False) /* Create  (450501) for Shop */
     , (450450, 4, 450503,  0, 0, 0, False) /* Create  (450503) for Shop */
     , (450450, 4, 450504,  0, 0, 0, False) /* Create  (450504) for Shop */
     , (450450, 4, 450505,  0, 0, 0, False) /* Create  (450505) for Shop */
     , (450450, 4, 450506,  0, 0, 0, False) /* Create  (450506) for Shop */
     , (450450, 4, 450507,  0, 0, 0, False) /* Create  (450507) for Shop */
     , (450450, 4, 450100,  0, 0, 0, False) /* Create  (450100) for Shop */
     , (450450, 4, 450129,  0, 0, 0, False) /* Create  (450129) for Shop */
     , (450450, 4, 450130,  0, 0, 0, False) /* Create  (450130) for Shop */
     , (450450, 4, 450120,  0, 0, 0, False) /* Create  (450120) for Shop */
     , (450450, 4, 450126,  0, 0, 0, False) /* Create  (450126) for Shop */
     , (450450, 4, 450127,  0, 0, 0, False) /* Create  (450127) for Shop */
     , (450450, 4, 450181,  0, 0, 0, False) /* Create  (450181) for Shop */
     , (450450, 4, 450182,  0, 0, 0, False) /* Create  (450182) for Shop */
     , (450450, 4, 450183,  0, 0, 0, False) /* Create  (450183) for Shop */
     , (450450, 4, 450184,  0, 0, 0, False) /* Create  (450184) for Shop */
     , (450450, 4, 450530,  0, 0, 0, False) /* Create  (450530) for Shop */
     , (450450, 4, 1012236,  0, 0, 0, False) /* Create  (1012236) for Shop */
     , (450450, 4, 450560,  0, 0, 0, False) /* Create  (450560) for Shop */
     , (450450, 4, 450561,  0, 0, 0, False) /* Create  (450561) for Shop */
     , (450450, 4, 450562,  0, 0, 0, False) /* Create  (450562) for Shop */
     , (450450, 4, 450563,  0, 0, 0, False) /* Create  (450563) for Shop */
     , (450450, 4, 450564,  0, 0, 0, False) /* Create  (450564) for Shop */
     , (450450, 4, 450566,  0, 0, 0, False) /* Create  (450566) for Shop */
     , (450450, 4, 450567,  0, 0, 0, False) /* Create  (450567) for Shop */
     , (450450, 4, 450568,  0, 0, 0, False) /* Create  (450568) for Shop */
     , (450450, 4, 450569,  0, 0, 0, False) /* Create  (450569) for Shop */
     , (450450, 4, 450570,  0, 0, 0, False) /* Create  (450570) for Shop */
     , (450450, 4, 450573,  0, 0, 0, False) /* Create  (450573) for Shop */
     , (450450, 4, 450574,  0, 0, 0, False) /* Create  (450574) for Shop */
     , (450450, 4, 450575,  0, 0, 0, False) /* Create  (450575) for Shop */
     , (450450, 4, 450576,  0, 0, 0, False) /* Create  (450576) for Shop */
     , (450450, 4, 450577,  0, 0, 0, False) /* Create  (450577) for Shop */
     , (450450, 4, 450578,  0, 0, 0, False) /* Create  (450578) for Shop */
     , (450450, 4, 450579,  0, 0, 0, False) /* Create  (450579) for Shop */
     , (450450, 4, 450580,  0, 0, 0, False) /* Create  (450580) for Shop */
     , (450450, 4, 450581,  0, 0, 0, False) /* Create  (450581) for Shop */
     , (450450, 4, 450582,  0, 0, 0, False) /* Create  (450582) for Shop */
     , (450450, 4, 450583,  0, 0, 0, False) /* Create  (450583) for Shop */
     , (450450, 4, 450584,  0, 0, 0, False) /* Create  (450584) for Shop */
     , (450450, 4, 450585,  0, 0, 0, False) /* Create  (450585) for Shop */
     , (450450, 4, 450586,  0, 0, 0, False) /* Create  (450586) for Shop */
     , (450450, 4, 450587,  0, 0, 0, False) /* Create  (450587) for Shop */
     , (450450, 4, 450588,  0, 0, 0, False) /* Create  (450588) for Shop */
     , (450450, 4, 450589,  0, 0, 0, False) /* Create  (450589) for Shop */
     , (450450, 4, 450590,  0, 0, 0, False) /* Create  (450590) for Shop */
     , (450450, 4, 450591,  0, 0, 0, False) /* Create  (450591) for Shop */
     , (450450, 4, 450592,  0, 0, 0, False) /* Create  (450592) for Shop */
     , (450450, 4, 450593,  0, 0, 0, False) /* Create  (450593) for Shop */
     , (450450, 4, 450594,  0, 0, 0, False) /* Create  (450594) for Shop */
     , (450450, 4, 450597,  0, 0, 0, False) /* Create  (450597) for Shop */
     , (450450, 4, 450598,  0, 0, 0, False) /* Create  (450598) for Shop */
     , (450450, 4, 450599,  0, 0, 0, False) /* Create  (450599) for Shop */
     , (450450, 4, 450600,  0, 0, 0, False) /* Create  (450600) for Shop */
     , (450450, 4, 450601,  0, 0, 0, False) /* Create  (450601) for Shop */
     , (450450, 4, 450602,  0, 0, 0, False) /* Create  (450602) for Shop */
     , (450450, 4, 450603,  0, 0, 0, False) /* Create  (450603) for Shop */
     , (450450, 4, 450604,  0, 0, 0, False) /* Create  (450604) for Shop */
     , (450450, 4, 450605,  0, 0, 0, False) /* Create  (450605) for Shop */
     , (450450, 4, 450606,  0, 0, 0, False) /* Create  (450606) for Shop */;

/* Lifestoned Changelog:
{
  "LastModified": "2022-11-14T22:48:42.2875328-05:00",
  "ModifiedBy": "Tindale",
  "Changelog": [
    {
      "created": "0001-01-01T00:00:00",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "testing",
  "IsDone": false
}
*/
