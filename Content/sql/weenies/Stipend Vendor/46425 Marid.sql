DELETE FROM `weenie` WHERE `class_Id` = 46425;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (46425, 'ace46425-marid', 12, '2021-11-01 00:00:00') /* Vendor */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (46425,   1,         16) /* ItemType - Creature */
     , (46425,   2,         31) /* CreatureType - Human */
     , (46425,   6,         -1) /* ItemsCapacity */
     , (46425,   7,         -1) /* ContainersCapacity */
     , (46425,  16,         32) /* ItemUseable - Remote */
     , (46425,  25,        250) /* Level */
     , (46425,  27,          0) /* ArmorType - None */
     , (46425,  74,          0) /* MerchandiseItemTypes - None */
     , (46425,  75,          0) /* MerchandiseMinValue */
     , (46425,  76,     100000) /* MerchandiseMaxValue */
     , (46425,  93,    2098200) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity, ReportCollisionsAsEnvironment */
     , (46425, 113,          1) /* Gender - Male */
     , (46425, 126,        125) /* VendorHappyMean */
     , (46425, 127,        125) /* VendorHappyVariance */
     , (46425, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (46425, 134,         16) /* PlayerKillerStatus - RubberGlue */
     , (46425, 188,          2) /* HeritageGroup - Gharundim */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (46425,   1, True ) /* Stuck */
     , (46425,  19, False) /* Attackable */
     , (46425,  39, True ) /* DealMagicalItems */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (46425,   1,       5) /* HeartbeatInterval */
     , (46425,   2,       0) /* HeartbeatTimestamp */
     , (46425,   3,    0.16) /* HealthRate */
     , (46425,   4,       5) /* StaminaRate */
     , (46425,   5,       1) /* ManaRate */
     , (46425,  11,     300) /* ResetInterval */
     , (46425,  13,     0.9) /* ArmorModVsSlash */
     , (46425,  14,       1) /* ArmorModVsPierce */
     , (46425,  15,     1.1) /* ArmorModVsBludgeon */
     , (46425,  16,     0.4) /* ArmorModVsCold */
     , (46425,  17,     0.4) /* ArmorModVsFire */
     , (46425,  18,       1) /* ArmorModVsAcid */
     , (46425,  19,     0.6) /* ArmorModVsElectric */
     , (46425,  37,       1) /* BuyPrice */
     , (46425,  38,       1) /* SellPrice */
     , (46425,  54,       3) /* UseRadius */
     , (46425,  64,       1) /* ResistSlash */
     , (46425,  65,       1) /* ResistPierce */
     , (46425,  66,       1) /* ResistBludgeon */
     , (46425,  67,       1) /* ResistFire */
     , (46425,  68,       1) /* ResistCold */
     , (46425,  69,       1) /* ResistAcid */
     , (46425,  70,       1) /* ResistElectric */
     , (46425,  71,       1) /* ResistHealthBoost */
     , (46425,  72,       1) /* ResistStaminaDrain */
     , (46425,  73,       1) /* ResistStaminaBoost */
     , (46425,  74,       1) /* ResistManaDrain */
     , (46425,  75,       1) /* ResistManaBoost */
     , (46425, 104,      10) /* ObviousRadarRange */
     , (46425, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (46425,   1, 'Marid') /* Name */
     , (46425,   5, 'Stipend Vendor') /* Template */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (46425,   1, 0x02000001) /* Setup */
     , (46425,   2, 0x09000001) /* MotionTable */
     , (46425,   3, 0x20000001) /* SoundTable */
     , (46425,   6, 0x0400007E) /* PaletteBase */
     , (46425,   8, 0x06001036) /* Icon */
     , (46425,   9, 0x05001133) /* EyesTexture */
     , (46425,  10, 0x0500116A) /* NoseTexture */
     , (46425,  11, 0x050011B9) /* MouthTexture */
     , (46425,  15, 0x04002018) /* HairPalette */
     , (46425,  16, 0x040004AE) /* EyesPalette */
     , (46425,  17, 0x040002AE) /* SkinPalette */
     , (46425,  18, 0x01004802) /* HeadObject */
     , (46425,  57,      46423) /* AlternateCurrency - Stipend */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (46425,   1, 220, 0, 0) /* Strength */
     , (46425,   2, 270, 0, 0) /* Endurance */
     , (46425,   3, 200, 0, 0) /* Quickness */
     , (46425,   4, 200, 0, 0) /* Coordination */
     , (46425,   5, 290, 0, 0) /* Focus */
     , (46425,   6, 290, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (46425,   1,   196, 0, 0, 331) /* MaxHealth */
     , (46425,   3,   196, 0, 0, 466) /* MaxStamina */
     , (46425,   5,   196, 0, 0, 486) /* MaxMana */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (46425,  0,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (46425,  1,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (46425,  2,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (46425,  3,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (46425,  4,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (46425,  5,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (46425,  6,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (46425,  7,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (46425,  8,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (46425,  2 /* Vendor */,      1, NULL, NULL, NULL, NULL, 1 /* Open */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Welcome, do you have any stipends you''d like to spend today?', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (46425,  2 /* Vendor */,      1, NULL, NULL, NULL, NULL, 2 /* Close */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Thank you again for bartering with me.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (46425,  2 /* Vendor */,    0.5, NULL, NULL, NULL, NULL, 4 /* Buy */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'I find this to be a far easier way to get stipends than to actually work for them, thanks for the business.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (46425,  2 /* Vendor */,      1, NULL, NULL, NULL, NULL, 4 /* Buy */, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Always glad to help out a citizen of Dereth. I hope it serves you well.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (46425, 2, 25641,  0, 4, 0, False) /* Create Leather Cuirass (25641) for Wield */
     , (46425, 2, 25645,  0, 4, 0, False) /* Create Leather Leggings (25645) for Wield */
     , (46425, 2, 25651,  0, 4, 0, False) /* Create Leather Sleeves (25651) for Wield */
     , (46425, 2, 25661,  0, 4, 0, False) /* Create Leather Boots (25661) for Wield */
     , (46425, 2,   130,  0, 88, 0.4, False) /* Create Shirt (130) for Wield */
     , (46425, 4, 46441, -1, 0, 0, False) /* Create Boxed Augmentation Gem (46441) for Shop */
     , (46425, 4, 46421, -1, 0, 0, False) /* Create Attribute Reset Certificate (46421) for Shop */
     , (46425, 4, 46420, -1, 0, 0, False) /* Create Skill Reset Certificate (46420) for Shop */
     , (46425, 4, 46422, -1, 0, 0, False) /* Create Mastery Reset Certificate (46422) for Shop */
     , (46425, 4, 46418, -1, 0, 0, False) /* Create Item Spells Certificate (46418) for Shop */
     , (46425, 4, 46419, -1, 0, 0, False) /* Create Life Spells Certificate (46419) for Shop */
     , (46425, 4, 46417, -1, 0, 0, False) /* Create Creature Spells Certificate (46417) for Shop */
     , (46425, 4, 46416, -1, 0, 0, False) /* Create Combat Spells Certificate (46416) for Shop */
     , (46425, 4, 53406, -1, 0, 0, False) /* Create Ring Spells Certificate (53406) for Shop */
     , (46425, 4, 53407, -1, 0, 0, False) /* Create Wall Spells Certificate (53407) for Shop */
     , (46425, 4, 46415, -1, 0, 0, False) /* Create Experience Certificate (46415) for Shop */;
