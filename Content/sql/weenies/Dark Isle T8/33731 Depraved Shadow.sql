DELETE FROM `weenie` WHERE `class_Id` = 33731;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33731, 'ace33731-depravedshadow', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33731,   1,         16) /* ItemType - Creature */
     , (33731,   2,         22) /* CreatureType - Shadow */
     , (33731,   3,         39) /* PaletteTemplate - Black */
     , (33731,   6,         -1) /* ItemsCapacity */
     , (33731,   7,         -1) /* ContainersCapacity */
     , (33731,   8,         90) /* Mass */
     , (33731,  16,          1) /* ItemUseable - No */
     , (33731,  25,        200) /* Level */
     , (33731,  27,          0) /* ArmorType - None */
     , (33731,  68,          3) /* TargetingTactic - Random, Focused */
     , (33731,  81,          1) /* MaxGeneratedObjects */
     , (33731,  82,          0) /* InitGeneratedObjects */
     , (33731,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (33731, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (33731, 113,          2) /* Gender - Female */
     , (33731, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33731, 140,          1) /* AiOptions - CanOpenDoors */
     , (33731, 146,     200000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33731,   1, True ) /* Stuck */
     , (33731,   6, True ) /* AiUsesMana */
     , (33731,  11, False) /* IgnoreCollisions */
     , (33731,  12, True ) /* ReportCollisions */
     , (33731,  13, False) /* Ethereal */
     , (33731,  14, True ) /* GravityStatus */
     , (33731,  19, True ) /* Attackable */
     , (33731,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33731,   1,       5) /* HeartbeatInterval */
     , (33731,   2,       0) /* HeartbeatTimestamp */
     , (33731,   3,     0.7) /* HealthRate */
     , (33731,   4,     2.5) /* StaminaRate */
     , (33731,   5,       1) /* ManaRate */
     , (33731,  12,       0) /* Shade */
     , (33731,  13,     0.9) /* ArmorModVsSlash */
     , (33731,  14,       1) /* ArmorModVsPierce */
     , (33731,  15,       1) /* ArmorModVsBludgeon */
     , (33731,  16,     1.1) /* ArmorModVsCold */
     , (33731,  17,     0.9) /* ArmorModVsFire */
     , (33731,  18,       1) /* ArmorModVsAcid */
     , (33731,  19,       1) /* ArmorModVsElectric */
     , (33731,  31,      20) /* VisualAwarenessRange */
     , (33731,  34,     1.2) /* PowerupTime */
     , (33731,  36,       1) /* ChargeSpeed */
     , (33731,  39,     1.1) /* DefaultScale */
     , (33731,  64,     0.7) /* ResistSlash */
     , (33731,  65,     0.5) /* ResistPierce */
     , (33731,  66,     0.7) /* ResistBludgeon */
     , (33731,  67,     0.8) /* ResistFire */
     , (33731,  68,     0.1) /* ResistCold */
     , (33731,  69,     0.2) /* ResistAcid */
     , (33731,  70,     0.5) /* ResistElectric */
     , (33731,  71,       1) /* ResistHealthBoost */
     , (33731,  72,       1) /* ResistStaminaDrain */
     , (33731,  73,       1) /* ResistStaminaBoost */
     , (33731,  74,       1) /* ResistManaDrain */
     , (33731,  75,       1) /* ResistManaBoost */
     , (33731,  76,     0.5) /* Translucency */
     , (33731,  80,       3) /* AiUseMagicDelay */
     , (33731, 104,      10) /* ObviousRadarRange */
     , (33731, 122,       2) /* AiAcquireHealth */
     , (33731, 125,       1) /* ResistHealthDrain */
     , (33731, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33731,   1, 'Depraved Shadow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33731,   1, 0x0200071B) /* Setup */
     , (33731,   2, 0x09000093) /* MotionTable */
     , (33731,   3, 0x20000002) /* SoundTable */
     , (33731,   4, 0x30000000) /* CombatTable */
     , (33731,   6, 0x0400007E) /* PaletteBase */
     , (33731,   7, 0x1000019F) /* ClothingBase */
     , (33731,   8, 0x06001BBE) /* Icon */
     , (33731,  22, 0x34000063) /* PhysicsEffectTable */
     , (33731,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33731,   1, 210, 0, 0) /* Strength */
     , (33731,   2, 230, 0, 0) /* Endurance */
     , (33731,   3, 280, 0, 0) /* Quickness */
     , (33731,   4, 260, 0, 0) /* Coordination */
     , (33731,   5, 240, 0, 0) /* Focus */
     , (33731,   6, 160, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33731,   1,   885, 0, 0, 1000) /* MaxHealth */
     , (33731,   3,  1000, 0, 0, 1230) /* MaxStamina */
     , (33731,   5,  1000, 0, 0, 1160) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33731,  6, 0, 3, 0, 310, 0, 0) /* MeleeDefense        Specialized */
     , (33731,  7, 0, 3, 0, 410, 0, 0) /* MissileDefense      Specialized */
     , (33731, 14, 0, 3, 0, 200, 0, 0) /* ArcaneLore          Specialized */
     , (33731, 15, 0, 3, 0, 243, 0, 0) /* MagicDefense        Specialized */
     , (33731, 31, 0, 3, 0, 225, 0, 0) /* CreatureEnchantment Specialized */
     , (33731, 33, 0, 3, 0, 225, 0, 0) /* LifeMagic           Specialized */
     , (33731, 34, 0, 3, 0, 225, 0, 0) /* WarMagic            Specialized */
     , (33731, 44, 0, 3, 0, 308, 0, 0) /* HeavyWeapons        Specialized */
     , (33731, 45, 0, 3, 0, 308, 0, 0) /* LightWeapons        Specialized */
     , (33731, 46, 0, 3, 0, 293, 0, 0) /* FinesseWeapons      Specialized */
     , (33731, 47, 0, 3, 0, 220, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33731,  0,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (33731,  1,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (33731,  2,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (33731,  3,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (33731,  4,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (33731,  5,  4, 50, 0.75,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (33731,  6,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (33731,  7,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (33731,  8,  4, 60, 0.75,   60,   54,   60,   60,   66,   54,   60,   60,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33731,  1161,  2.009)  /* Heal Self VI */
     , (33731,  1265,  2.009)  /* Drain Mana Other VI */
     , (33731,  2074,   2.01)  /* Gossamer Flesh */
     , (33731,  2132,  2.005)  /* The Spike */
     , (33731,  2133,  2.036)  /* Outlander's Insolence */
     , (33731,  2136,  2.036)  /* Icy Torment */
     , (33731,  2137,  2.005)  /* Sudden Frost */
     , (33731,  2140,  2.036)  /* Alset's Coil */
     , (33731,  2141,  2.036)  /* Lhen's Flare */
     , (33731,  2164,   2.02)  /* Swordsman's Gift */
     , (33731,  2168,   2.01)  /* Gelidite's Gift */
     , (33731,  2172,  2.005)  /* Astyrrian's Gift */
     , (33731,  2174,  2.005)  /* Archer's Gift */
     , (33731,  2282,   2.02)  /* Futility */
     , (33731,  2318,   2.01)  /* Gravity Well */
     , (33731,  4452,  2.009)  /* Incantation of Lightning Streak */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (33731,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33731, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33731, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33731, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33731, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33731, -1, 33633, 0, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Depraved Shadow Commander (33633) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
