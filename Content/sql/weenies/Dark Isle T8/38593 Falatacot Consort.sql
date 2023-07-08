DELETE FROM `weenie` WHERE `class_Id` = 38593;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (38593, 'ace38593-falatacotconsort', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (38593,   1,         16) /* ItemType - Creature */
     , (38593,   2,         14) /* CreatureType - Undead */
     , (38593,   3,         68) /* PaletteTemplate - BlueSlime */
     , (38593,   6,         -1) /* ItemsCapacity */
     , (38593,   7,         -1) /* ContainersCapacity */
     , (38593,  16,          1) /* ItemUseable - No */
     , (38593,  25,        135) /* Level */
     , (38593,  27,          0) /* ArmorType - None */
     , (38593,  40,          1) /* CombatMode - NonCombat */
     , (38593,  68,          3) /* TargetingTactic - Random, Focused */
     , (38593,  81,          1) /* MaxGeneratedObjects */
     , (38593,  82,          0) /* InitGeneratedObjects */
     , (38593,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (38593, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (38593, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (38593, 140,          1) /* AiOptions - CanOpenDoors */
     , (38593, 146,     250000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (38593,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (38593,   1,       5) /* HeartbeatInterval */
     , (38593,   2,       0) /* HeartbeatTimestamp */
     , (38593,   3,     0.8) /* HealthRate */
     , (38593,   4,     0.5) /* StaminaRate */
     , (38593,   5,       2) /* ManaRate */
     , (38593,  12,     0.5) /* Shade */
     , (38593,  13,       1) /* ArmorModVsSlash */
     , (38593,  14,     1.3) /* ArmorModVsPierce */
     , (38593,  15,       1) /* ArmorModVsBludgeon */
     , (38593,  16,     1.3) /* ArmorModVsCold */
     , (38593,  17,       1) /* ArmorModVsFire */
     , (38593,  18,       1) /* ArmorModVsAcid */
     , (38593,  19,     1.2) /* ArmorModVsElectric */
     , (38593,  31,      16) /* VisualAwarenessRange */
     , (38593,  34,       1) /* PowerupTime */
     , (38593,  36,       1) /* ChargeSpeed */
     , (38593,  39,     1.3) /* DefaultScale */
     , (38593,  43,       5) /* GeneratorRadius */
     , (38593,  64,     0.5) /* ResistSlash */
     , (38593,  65,    0.45) /* ResistPierce */
     , (38593,  66,    0.65) /* ResistBludgeon */
     , (38593,  67,    0.65) /* ResistFire */
     , (38593,  68,    0.55) /* ResistCold */
     , (38593,  69,    0.55) /* ResistAcid */
     , (38593,  70,     0.5) /* ResistElectric */
     , (38593,  71,       1) /* ResistHealthBoost */
     , (38593,  72,       1) /* ResistStaminaDrain */
     , (38593,  73,       1) /* ResistStaminaBoost */
     , (38593,  74,       1) /* ResistManaDrain */
     , (38593,  75,       1) /* ResistManaBoost */
     , (38593,  80,       3) /* AiUseMagicDelay */
     , (38593, 104,      10) /* ObviousRadarRange */
     , (38593, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (38593,   1, 'Falatacot Consort') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (38593,   1, 0x02000FA4) /* Setup */
     , (38593,   2, 0x09000017) /* MotionTable */
     , (38593,   3, 0x20000016) /* SoundTable */
     , (38593,   4, 0x30000000) /* CombatTable */
     , (38593,   6, 0x040015F0) /* PaletteBase */
     , (38593,   7, 0x100004C0) /* ClothingBase */
     , (38593,   8, 0x06002CF5) /* Icon */
     , (38593,  22, 0x34000028) /* PhysicsEffectTable */
     , (38593,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (38593,   1, 205, 0, 0) /* Strength */
     , (38593,   2, 300, 0, 0) /* Endurance */
     , (38593,   3, 170, 0, 0) /* Quickness */
     , (38593,   4, 150, 0, 0) /* Coordination */
     , (38593,   5, 380, 0, 0) /* Focus */
     , (38593,   6, 360, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (38593,   1,  1650, 0, 0, 1800) /* MaxHealth */
     , (38593,   3,  2000, 0, 0, 2300) /* MaxStamina */
     , (38593,   5,  1000, 0, 0, 1360) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (38593,  6, 0, 3, 0, 375, 0, 0) /* MeleeDefense        Specialized */
     , (38593,  7, 0, 3, 0, 405, 0, 0) /* MissileDefense      Specialized */
     , (38593, 14, 0, 3, 0, 240, 0, 0) /* ArcaneLore          Specialized */
     , (38593, 15, 0, 3, 0, 320, 0, 0) /* MagicDefense        Specialized */
     , (38593, 20, 0, 3, 0,  90, 0, 0) /* Deception           Specialized */
     , (38593, 31, 0, 3, 0, 275, 0, 0) /* CreatureEnchantment Specialized */
     , (38593, 33, 0, 3, 0, 275, 0, 0) /* LifeMagic           Specialized */
     , (38593, 34, 0, 3, 0, 275, 0, 0) /* WarMagic            Specialized */
     , (38593, 44, 0, 3, 0, 375, 0, 0) /* HeavyWeapons        Specialized */
     , (38593, 45, 0, 3, 0, 375, 0, 0) /* LightWeapons        Specialized */
     , (38593, 46, 0, 3, 0, 375, 0, 0) /* FinesseWeapons      Specialized */
     , (38593, 47, 0, 3, 0, 175, 0, 0) /* MissileWeapons      Specialized */
     , (38593, 48, 0, 3, 0, 300, 0, 0) /* Shield              Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (38593,  0,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (38593,  1,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (38593,  2,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (38593,  3,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (38593,  4,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (38593,  5,  4,  5, 0.75,  625,  625,  813,  625,  813,  625,  625,  750,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (38593,  6,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (38593,  7,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (38593,  8,  4,  5, 0.75,  625,  625,  813,  625,  813,  625,  625,  750,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (38593,   176,   2.02)  /* Fester Other VI */
     , (38593,   526,   2.02)  /* Acid Vulnerability Other VI */
     , (38593,  1053,   2.02)  /* Bludgeoning Vulnerability Other VI */
     , (38593,  1065,   2.02)  /* Cold Vulnerability Other VI */
     , (38593,  1089,   2.02)  /* Lightning Vulnerability Other VI */
     , (38593,  1156,   2.02)  /* Piercing Vulnerability Other VI */
     , (38593,  1842,   2.02)  /* Spike Strafe */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (38593,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   8 /* Say */, 0, 0, NULL, 'Im vaik av tiu ikni liViliakti.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (38593, 2, 48100,  1, 0, 0, False) /* Create Khopesh (48100) for Wield */
     , (38593, 2, 48101,  1, 0, 0, False) /* Create Sickle (48101) for Wield */
     , (38593, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (38593, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (38593, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (38593, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (38593, -1, 38594, 0, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Falatacot Blood Prophetess (38594) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
