DELETE FROM `weenie` WHERE `class_Id` = 33730;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33730, 'ace33730-degenerateshadow', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33730,   1,         16) /* ItemType - Creature */
     , (33730,   2,         22) /* CreatureType - Shadow */
     , (33730,   3,         39) /* PaletteTemplate - Black */
     , (33730,   6,         -1) /* ItemsCapacity */
     , (33730,   7,         -1) /* ContainersCapacity */
     , (33730,   8,         90) /* Mass */
     , (33730,  16,          1) /* ItemUseable - No */
     , (33730,  25,        185) /* Level */
     , (33730,  27,          0) /* ArmorType - None */
     , (33730,  68,          3) /* TargetingTactic - Random, Focused */
     , (33730,  81,          1) /* MaxGeneratedObjects */
     , (33730,  82,          0) /* InitGeneratedObjects */
     , (33730,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (33730, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (33730, 113,          2) /* Gender - Female */
     , (33730, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33730, 140,          1) /* AiOptions - CanOpenDoors */
     , (33730, 146,     200000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33730,   1, True ) /* Stuck */
     , (33730,   6, True ) /* AiUsesMana */
     , (33730,  11, False) /* IgnoreCollisions */
     , (33730,  12, True ) /* ReportCollisions */
     , (33730,  13, False) /* Ethereal */
     , (33730,  14, True ) /* GravityStatus */
     , (33730,  19, True ) /* Attackable */
     , (33730,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33730,   1,       5) /* HeartbeatInterval */
     , (33730,   2,       0) /* HeartbeatTimestamp */
     , (33730,   3,     0.7) /* HealthRate */
     , (33730,   4,     2.5) /* StaminaRate */
     , (33730,   5,       1) /* ManaRate */
     , (33730,  12,       0) /* Shade */
     , (33730,  13,     0.9) /* ArmorModVsSlash */
     , (33730,  14,       1) /* ArmorModVsPierce */
     , (33730,  15,       1) /* ArmorModVsBludgeon */
     , (33730,  16,     1.1) /* ArmorModVsCold */
     , (33730,  17,     0.9) /* ArmorModVsFire */
     , (33730,  18,       1) /* ArmorModVsAcid */
     , (33730,  19,       1) /* ArmorModVsElectric */
     , (33730,  31,      20) /* VisualAwarenessRange */
     , (33730,  34,     1.2) /* PowerupTime */
     , (33730,  36,       1) /* ChargeSpeed */
     , (33730,  39,       1) /* DefaultScale */
     , (33730,  43,       4) /* GeneratorRadius */
     , (33730,  64,     0.8) /* ResistSlash */
     , (33730,  65,     0.5) /* ResistPierce */
     , (33730,  66,     0.7) /* ResistBludgeon */
     , (33730,  67,     0.8) /* ResistFire */
     , (33730,  68,     0.1) /* ResistCold */
     , (33730,  69,     0.2) /* ResistAcid */
     , (33730,  70,     0.5) /* ResistElectric */
     , (33730,  71,       1) /* ResistHealthBoost */
     , (33730,  72,       1) /* ResistStaminaDrain */
     , (33730,  73,       1) /* ResistStaminaBoost */
     , (33730,  74,       1) /* ResistManaDrain */
     , (33730,  75,       1) /* ResistManaBoost */
     , (33730,  76,     0.5) /* Translucency */
     , (33730,  80,       3) /* AiUseMagicDelay */
     , (33730, 104,      10) /* ObviousRadarRange */
     , (33730, 122,       2) /* AiAcquireHealth */
     , (33730, 125,       1) /* ResistHealthDrain */
     , (33730, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33730,   1, 'Degenerate Shadow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33730,   1, 0x0200071B) /* Setup */
     , (33730,   2, 0x09000093) /* MotionTable */
     , (33730,   3, 0x20000002) /* SoundTable */
     , (33730,   4, 0x30000000) /* CombatTable */
     , (33730,   6, 0x0400007E) /* PaletteBase */
     , (33730,   7, 0x1000019F) /* ClothingBase */
     , (33730,   8, 0x06001BBE) /* Icon */
     , (33730,  22, 0x34000063) /* PhysicsEffectTable */
     , (33730,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33730,   1, 190, 0, 0) /* Strength */
     , (33730,   2, 210, 0, 0) /* Endurance */
     , (33730,   3, 260, 0, 0) /* Quickness */
     , (33730,   4, 240, 0, 0) /* Coordination */
     , (33730,   5, 220, 0, 0) /* Focus */
     , (33730,   6, 140, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33730,   1,   700, 0, 0, 805) /* MaxHealth */
     , (33730,   3,  1000, 0, 0, 1210) /* MaxStamina */
     , (33730,   5,  1000, 0, 0, 1140) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33730,  6, 0, 3, 0, 310, 0, 0) /* MeleeDefense        Specialized */
     , (33730,  7, 0, 3, 0, 410, 0, 0) /* MissileDefense      Specialized */
     , (33730, 14, 0, 3, 0, 200, 0, 0) /* ArcaneLore          Specialized */
     , (33730, 15, 0, 3, 0, 243, 0, 0) /* MagicDefense        Specialized */
     , (33730, 31, 0, 3, 0, 225, 0, 0) /* CreatureEnchantment Specialized */
     , (33730, 33, 0, 3, 0, 225, 0, 0) /* LifeMagic           Specialized */
     , (33730, 34, 0, 3, 0, 225, 0, 0) /* WarMagic            Specialized */
     , (33730, 44, 0, 3, 0, 308, 0, 0) /* HeavyWeapons        Specialized */
     , (33730, 45, 0, 3, 0, 308, 0, 0) /* LightWeapons        Specialized */
     , (33730, 46, 0, 3, 0, 293, 0, 0) /* FinesseWeapons      Specialized */
     , (33730, 47, 0, 3, 0, 220, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33730,  0,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (33730,  1,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (33730,  2,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (33730,  3,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (33730,  4,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (33730,  5,  4, 50, 0.75,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (33730,  6,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (33730,  7,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (33730,  8,  4, 60, 0.75,   60,   54,   60,   60,   66,   54,   60,   60,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33730,  1161,  2.009)  /* Heal Self VI */
     , (33730,  1265,  2.009)  /* Drain Mana Other VI */
     , (33730,  2074,   2.01)  /* Gossamer Flesh */
     , (33730,  2132,  2.005)  /* The Spike */
     , (33730,  2133,  2.036)  /* Outlander's Insolence */
     , (33730,  2136,  2.036)  /* Icy Torment */
     , (33730,  2137,  2.005)  /* Sudden Frost */
     , (33730,  2140,  2.036)  /* Alset's Coil */
     , (33730,  2141,  2.036)  /* Lhen's Flare */
     , (33730,  2164,   2.02)  /* Swordsman's Gift */
     , (33730,  2168,   2.01)  /* Gelidite's Gift */
     , (33730,  2172,  2.005)  /* Astyrrian's Gift */
     , (33730,  2174,  2.005)  /* Archer's Gift */
     , (33730,  2282,   2.02)  /* Futility */
     , (33730,  2318,   2.01)  /* Gravity Well */
     , (33730,  4452,  2.009)  /* Incantation of Lightning Streak */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (33730,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33730, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33730, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33730, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33730, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33730, -1, 33631, 0, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Degenerate Shadow Commander (33631) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
