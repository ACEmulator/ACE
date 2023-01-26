DELETE FROM `weenie` WHERE `class_Id` = 34973;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (34973, 'ace34973-falatacotconsort', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (34973,   1,         16) /* ItemType - Creature */
     , (34973,   2,         14) /* CreatureType - Undead */
     , (34973,   3,         68) /* PaletteTemplate - BlueSlime */
     , (34973,   6,         -1) /* ItemsCapacity */
     , (34973,   7,         -1) /* ContainersCapacity */
     , (34973,  16,          1) /* ItemUseable - No */
     , (34973,  25,        115) /* Level */
     , (34973,  27,          0) /* ArmorType - None */
     , (34973,  40,          1) /* CombatMode - NonCombat */
     , (34973,  68,          3) /* TargetingTactic - Random, Focused */
     , (34973,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (34973, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (34973, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (34973, 140,          1) /* AiOptions - CanOpenDoors */
     , (34973, 146,     125000) /* XpOverride */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (34973,   1,       5) /* HeartbeatInterval */
     , (34973,   2,       0) /* HeartbeatTimestamp */
     , (34973,   3,     0.8) /* HealthRate */
     , (34973,   4,     0.5) /* StaminaRate */
     , (34973,   5,       2) /* ManaRate */
     , (34973,  12,     0.5) /* Shade */
     , (34973,  13,       1) /* ArmorModVsSlash */
     , (34973,  14,     1.3) /* ArmorModVsPierce */
     , (34973,  15,       1) /* ArmorModVsBludgeon */
     , (34973,  16,     1.3) /* ArmorModVsCold */
     , (34973,  17,       1) /* ArmorModVsFire */
     , (34973,  18,       1) /* ArmorModVsAcid */
     , (34973,  19,     1.2) /* ArmorModVsElectric */
     , (34973,  31,      16) /* VisualAwarenessRange */
     , (34973,  34,       1) /* PowerupTime */
     , (34973,  36,       1) /* ChargeSpeed */
     , (34973,  39,     1.3) /* DefaultScale */
     , (34973,  64,     0.5) /* ResistSlash */
     , (34973,  65,    0.45) /* ResistPierce */
     , (34973,  66,    0.65) /* ResistBludgeon */
     , (34973,  67,    0.65) /* ResistFire */
     , (34973,  68,    0.55) /* ResistCold */
     , (34973,  69,    0.55) /* ResistAcid */
     , (34973,  70,     0.5) /* ResistElectric */
     , (34973,  71,       1) /* ResistHealthBoost */
     , (34973,  72,       1) /* ResistStaminaDrain */
     , (34973,  73,       1) /* ResistStaminaBoost */
     , (34973,  74,       1) /* ResistManaDrain */
     , (34973,  75,       1) /* ResistManaBoost */
     , (34973,  80,       3) /* AiUseMagicDelay */
     , (34973, 104,      10) /* ObviousRadarRange */
     , (34973, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (34973,   1, 'Falatacot Consort') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (34973,   1, 0x02000FA5) /* Setup */
     , (34973,   2, 0x09000017) /* MotionTable */
     , (34973,   3, 0x20000016) /* SoundTable */
     , (34973,   4, 0x30000000) /* CombatTable */
     , (34973,   6, 0x040015F0) /* PaletteBase */
     , (34973,   7, 0x100004C0) /* ClothingBase */
     , (34973,   8, 0x06002CF5) /* Icon */
     , (34973,  22, 0x34000028) /* PhysicsEffectTable */
     , (34973,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (34973,   1, 105, 0, 0) /* Strength */
     , (34973,   2, 210, 0, 0) /* Endurance */
     , (34973,   3,  80, 0, 0) /* Quickness */
     , (34973,   4,  60, 0, 0) /* Coordination */
     , (34973,   5, 250, 0, 0) /* Focus */
     , (34973,   6, 240, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (34973,   1,   829, 0, 0, 934) /* MaxHealth */
     , (34973,   3,   800, 0, 0, 1010) /* MaxStamina */
     , (34973,   5,   300, 0, 0, 540) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (34973,  6, 0, 3, 0, 375, 0, 0) /* MeleeDefense        Specialized */
     , (34973,  7, 0, 3, 0, 405, 0, 0) /* MissileDefense      Specialized */
     , (34973, 14, 0, 3, 0, 240, 0, 0) /* ArcaneLore          Specialized */
     , (34973, 15, 0, 3, 0, 320, 0, 0) /* MagicDefense        Specialized */
     , (34973, 20, 0, 3, 0,  90, 0, 0) /* Deception           Specialized */
     , (34973, 31, 0, 3, 0, 275, 0, 0) /* CreatureEnchantment Specialized */
     , (34973, 33, 0, 3, 0, 275, 0, 0) /* LifeMagic           Specialized */
     , (34973, 34, 0, 3, 0, 275, 0, 0) /* WarMagic            Specialized */
     , (34973, 44, 0, 3, 0, 375, 0, 0) /* HeavyWeapons        Specialized */
     , (34973, 45, 0, 3, 0, 375, 0, 0) /* LightWeapons        Specialized */
     , (34973, 46, 0, 3, 0, 375, 0, 0) /* FinesseWeapons      Specialized */
     , (34973, 47, 0, 3, 0, 175, 0, 0) /* MissileWeapons      Specialized */
     , (34973, 48, 0, 3, 0, 300, 0, 0) /* Shield              Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (34973,  0,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (34973,  1,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (34973,  2,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (34973,  3,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (34973,  4,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (34973,  5,  4,  5, 0.75,  625,  625,  813,  625,  813,  625,  625,  750,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (34973,  6,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (34973,  7,  4,  0,    0,  625,  625,  813,  625,  813,  625,  625,  750,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (34973,  8,  4,  5, 0.75,  625,  625,  813,  625,  813,  625,  625,  750,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (34973,   176,   2.02)  /* Fester Other VI */
     , (34973,   526,   2.02)  /* Acid Vulnerability Other VI */
     , (34973,  1053,   2.02)  /* Bludgeoning Vulnerability Other VI */
     , (34973,  1065,   2.02)  /* Cold Vulnerability Other VI */
     , (34973,  1089,   2.02)  /* Lightning Vulnerability Other VI */
     , (34973,  1156,   2.02)  /* Piercing Vulnerability Other VI */
     , (34973,  1842,   2.02)  /* Spike Strafe */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (34973,  3 /* Death */,   0.05, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   8 /* Say */, 0, 0, NULL, 'Im vaik av tiu ikni liViliakti.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (34973, 2, 48100,  1, 0, 0, False) /* Create Khopesh (48100) for Wield */
     , (34973, 2, 48101,  1, 0, 0, False) /* Create Sickle (48101) for Wield */
     , (34973, 2, 48102,  1, 0, 0, False) /* Create Khopesh (48102) for Wield */
     , (34973, 2, 48103,  1, 0, 0, False) /* Create Sickle (48103) for Wield */
     , (34973, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (34973, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (34973, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (34973, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
