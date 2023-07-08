DELETE FROM `weenie` WHERE `class_Id` = 33735;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33735, 'ace33735-shamblingundeadruschk', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33735,   1,         16) /* ItemType - Creature */
     , (33735,   2,         14) /* CreatureType - Undead */
     , (33735,   6,         -1) /* ItemsCapacity */
     , (33735,   7,         -1) /* ContainersCapacity */
     , (33735,  16,          1) /* ItemUseable - No */
     , (33735,  25,        185) /* Level */
     , (33735,  27,          0) /* ArmorType - None */
     , (33735,  40,          2) /* CombatMode - Melee */
     , (33735,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (33735,  81,          1) /* MaxGeneratedObjects */
     , (33735,  82,          0) /* InitGeneratedObjects */
     , (33735,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (33735, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (33735, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33735, 140,          1) /* AiOptions - CanOpenDoors */
     , (33735, 146,     200000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33735,   1, True ) /* Stuck */
     , (33735,  11, False) /* IgnoreCollisions */
     , (33735,  12, True ) /* ReportCollisions */
     , (33735,  13, False) /* Ethereal */
     , (33735,  14, True ) /* GravityStatus */
     , (33735,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33735,   1,       5) /* HeartbeatInterval */
     , (33735,   2,       0) /* HeartbeatTimestamp */
     , (33735,   3,    0.15) /* HealthRate */
     , (33735,   4,       4) /* StaminaRate */
     , (33735,   5,     1.5) /* ManaRate */
     , (33735,  12,       0) /* Shade */
     , (33735,  13,     0.9) /* ArmorModVsSlash */
     , (33735,  14,     0.6) /* ArmorModVsPierce */
     , (33735,  15,     1.1) /* ArmorModVsBludgeon */
     , (33735,  16,     0.8) /* ArmorModVsCold */
     , (33735,  17,    0.55) /* ArmorModVsFire */
     , (33735,  18,       1) /* ArmorModVsAcid */
     , (33735,  19,     0.8) /* ArmorModVsElectric */
     , (33735,  31,      17) /* VisualAwarenessRange */
     , (33735,  34,       1) /* PowerupTime */
     , (33735,  36,       1) /* ChargeSpeed */
     , (33735,  39,       1) /* DefaultScale */
     , (33735,  43,       4) /* GeneratorRadius */
     , (33735,  64,     0.1) /* ResistSlash */
     , (33735,  65,     0.1) /* ResistPierce */
     , (33735,  66,     0.3) /* ResistBludgeon */
     , (33735,  67,     0.3) /* ResistFire */
     , (33735,  68,     0.1) /* ResistCold */
     , (33735,  69,     0.2) /* ResistAcid */
     , (33735,  70,     0.1) /* ResistElectric */
     , (33735,  71,       1) /* ResistHealthBoost */
     , (33735,  72,     0.5) /* ResistStaminaDrain */
     , (33735,  73,       1) /* ResistStaminaBoost */
     , (33735,  74,     0.5) /* ResistManaDrain */
     , (33735,  75,       1) /* ResistManaBoost */
     , (33735, 104,      10) /* ObviousRadarRange */
     , (33735, 125,     0.5) /* ResistHealthDrain */
     , (33735, 166,     0.2) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33735,   1, 'Shambling Undead Ruschk') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33735,   1, 0x020015CD) /* Setup */
     , (33735,   2, 0x09000007) /* MotionTable */
     , (33735,   3, 0x200000BD) /* SoundTable */
     , (33735,   4, 0x30000004) /* CombatTable */
     , (33735,   8, 0x060036FD) /* Icon */
     , (33735,  22, 0x34000084) /* PhysicsEffectTable */
     , (33735,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33735,   1, 310, 0, 0) /* Strength */
     , (33735,   2, 240, 0, 0) /* Endurance */
     , (33735,   3, 200, 0, 0) /* Quickness */
     , (33735,   4, 240, 0, 0) /* Coordination */
     , (33735,   5, 210, 0, 0) /* Focus */
     , (33735,   6, 210, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33735,   1,   740, 0, 0, 860) /* MaxHealth */
     , (33735,   3,   800, 0, 0, 1040) /* MaxStamina */
     , (33735,   5,   200, 0, 0, 410) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33735,  6, 0, 3, 0, 375, 0, 0) /* MeleeDefense        Specialized */
     , (33735,  7, 0, 3, 0, 370, 0, 0) /* MissileDefense      Specialized */
     , (33735, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (33735, 15, 0, 3, 0, 400, 0, 0) /* MagicDefense        Specialized */
     , (33735, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (33735, 31, 0, 3, 0, 275, 0, 0) /* CreatureEnchantment Specialized */
     , (33735, 32, 0, 3, 0, 275, 0, 0) /* ItemEnchantment     Specialized */
     , (33735, 33, 0, 3, 0, 275, 0, 0) /* LifeMagic           Specialized */
     , (33735, 34, 0, 3, 0, 290, 0, 0) /* WarMagic            Specialized */
     , (33735, 45, 0, 3, 0, 385, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33735,  0,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (33735,  1,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (33735,  2,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (33735,  3,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (33735,  4,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (33735,  5,  4, 60,  0.5,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (33735,  6,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (33735,  7,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (33735,  8,  4, 50,  0.4,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33735,  2074,   2.02)  /* Gossamer Flesh */
     , (33735,  2136,   2.02)  /* Icy Torment */
     , (33735,  2168,   2.02)  /* Gelidite's Gift */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (33735,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33735, 2, 48633,  1, 0, 0, False) /* Create Glacial Blade (48633) for Wield */
     , (33735, 2, 48630,  1, 0, 0, False) /* Create  (48630) for Wield */
     , (33735, 2, 48631,  1, 0, 0, False) /* Create  (48631) for Wield */
     , (33735, 2, 48632,  1, 0, 0, False) /* Create Tursh's Spear (48632) for Wield */
     , (33735, 2, 48629,  1, 0, 0, False) /* Create Icy Club (48629) for Wield */
     , (33735, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33735, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33735, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33735, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33735, -1, 33639, 0, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Shambling Ruschk Chieftain (33639) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
