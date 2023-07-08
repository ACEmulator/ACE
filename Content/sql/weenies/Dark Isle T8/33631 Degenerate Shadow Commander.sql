DELETE FROM `weenie` WHERE `class_Id` = 33631;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33631, 'ace33631-degenerateshadowcommander', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33631,   1,         16) /* ItemType - Creature */
     , (33631,   2,         22) /* CreatureType - Shadow */
     , (33631,   3,         39) /* PaletteTemplate - Black */
     , (33631,   6,         -1) /* ItemsCapacity */
     , (33631,   7,         -1) /* ContainersCapacity */
     , (33631,   8,         90) /* Mass */
     , (33631,  16,          1) /* ItemUseable - No */
     , (33631,  25,        185) /* Level */
     , (33631,  27,          0) /* ArmorType - None */
     , (33631,  68,          3) /* TargetingTactic - Random, Focused */
     , (33631,  81,          2) /* MaxGeneratedObjects */
     , (33631,  82,          2) /* InitGeneratedObjects */
     , (33631,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (33631, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (33631, 103,          3) /* GeneratorDestructionType - Kill */
     , (33631, 113,          2) /* Gender - Female */
     , (33631, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33631, 140,          1) /* AiOptions - CanOpenDoors */
     , (33631, 146,     345000) /* XpOverride */
     , (33631, 188,          1) /* HeritageGroup - Aluvian */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33631,   1, True ) /* Stuck */
     , (33631,   6, True ) /* AiUsesMana */
     , (33631,  11, False) /* IgnoreCollisions */
     , (33631,  12, True ) /* ReportCollisions */
     , (33631,  13, False) /* Ethereal */
     , (33631,  14, True ) /* GravityStatus */
     , (33631,  19, True ) /* Attackable */
     , (33631,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33631,   1,       5) /* HeartbeatInterval */
     , (33631,   2,       0) /* HeartbeatTimestamp */
     , (33631,   3,     0.7) /* HealthRate */
     , (33631,   4,     2.5) /* StaminaRate */
     , (33631,   5,       1) /* ManaRate */
     , (33631,  12,     0.5) /* Shade */
     , (33631,  13,     0.6) /* ArmorModVsSlash */
     , (33631,  14,     0.8) /* ArmorModVsPierce */
     , (33631,  15,    0.85) /* ArmorModVsBludgeon */
     , (33631,  16,     0.9) /* ArmorModVsCold */
     , (33631,  17,     0.5) /* ArmorModVsFire */
     , (33631,  18,     0.7) /* ArmorModVsAcid */
     , (33631,  19,    0.75) /* ArmorModVsElectric */
     , (33631,  31,      20) /* VisualAwarenessRange */
     , (33631,  34,     1.2) /* PowerupTime */
     , (33631,  36,       1) /* ChargeSpeed */
     , (33631,  39,     1.2) /* DefaultScale */
     , (33631,  41,       0) /* RegenerationInterval */
     , (33631,  43,       4) /* GeneratorRadius */
     , (33631,  64,     0.7) /* ResistSlash */
     , (33631,  65,     0.5) /* ResistPierce */
     , (33631,  66,     0.7) /* ResistBludgeon */
     , (33631,  67,     0.8) /* ResistFire */
     , (33631,  68,     0.1) /* ResistCold */
     , (33631,  69,     0.2) /* ResistAcid */
     , (33631,  70,     0.5) /* ResistElectric */
     , (33631,  71,       1) /* ResistHealthBoost */
     , (33631,  72,       1) /* ResistStaminaDrain */
     , (33631,  73,       1) /* ResistStaminaBoost */
     , (33631,  74,       1) /* ResistManaDrain */
     , (33631,  75,       1) /* ResistManaBoost */
     , (33631,  76,     0.5) /* Translucency */
     , (33631,  80,       3) /* AiUseMagicDelay */
     , (33631, 104,      10) /* ObviousRadarRange */
     , (33631, 122,       2) /* AiAcquireHealth */
     , (33631, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33631,   1, 'Degenerate Shadow Commander') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33631,   1, 0x0200071B) /* Setup */
     , (33631,   2, 0x09000093) /* MotionTable */
     , (33631,   3, 0x20000002) /* SoundTable */
     , (33631,   4, 0x30000000) /* CombatTable */
     , (33631,   6, 0x0400007E) /* PaletteBase */
     , (33631,   7, 0x1000019F) /* ClothingBase */
     , (33631,   8, 0x06001BBE) /* Icon */
     , (33631,  22, 0x34000063) /* PhysicsEffectTable */
     , (33631,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33631,   1, 300, 0, 0) /* Strength */
     , (33631,   2, 400, 0, 0) /* Endurance */
     , (33631,   3, 300, 0, 0) /* Quickness */
     , (33631,   4, 300, 0, 0) /* Coordination */
     , (33631,   5, 540, 0, 0) /* Focus */
     , (33631,   6, 560, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33631,   1,  9000, 0, 0, 9200) /* MaxHealth */
     , (33631,   3,   300, 0, 0, 700) /* MaxStamina */
     , (33631,   5,   280, 0, 0, 860) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33631,  6, 0, 3, 0, 310, 0, 0) /* MeleeDefense        Specialized */
     , (33631,  7, 0, 3, 0, 410, 0, 0) /* MissileDefense      Specialized */
     , (33631, 14, 0, 3, 0, 200, 0, 0) /* ArcaneLore          Specialized */
     , (33631, 15, 0, 3, 0, 243, 0, 0) /* MagicDefense        Specialized */
     , (33631, 31, 0, 3, 0, 225, 0, 0) /* CreatureEnchantment Specialized */
     , (33631, 33, 0, 3, 0, 225, 0, 0) /* LifeMagic           Specialized */
     , (33631, 34, 0, 3, 0, 225, 0, 0) /* WarMagic            Specialized */
     , (33631, 44, 0, 3, 0, 308, 0, 0) /* HeavyWeapons        Specialized */
     , (33631, 45, 0, 3, 0, 308, 0, 0) /* LightWeapons        Specialized */
     , (33631, 46, 0, 3, 0, 293, 0, 0) /* FinesseWeapons      Specialized */
     , (33631, 47, 0, 3, 0, 220, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33631,  0,  4,  0,    0,  500,  300,  400,  425,  450,  250,  350,  375,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (33631,  1,  4,  0,    0,  500,  300,  400,  425,  450,  250,  350,  375,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (33631,  2,  4,  0,    0,  500,  300,  400,  425,  450,  250,  350,  375,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (33631,  3,  4,  0,    0,  500,  300,  400,  425,  450,  250,  350,  375,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (33631,  4,  4,  0,    0,  500,  300,  400,  425,  450,  250,  350,  375,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (33631,  5,  4, 50, 0.75,  500,  300,  400,  425,  450,  250,  350,  375,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (33631,  6,  4,  0,    0,  500,  300,  400,  425,  450,  250,  350,  375,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (33631,  7,  4,  0,    0,  500,  300,  400,  425,  450,  250,  350,  375,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (33631,  8,  4, 60, 0.75,   60,   36,   48,   51,   54,   30,   42,   45,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33631,  2140,  2.036)  /* Alset's Coil */
     , (33631,  2136,  2.036)  /* Icy Torment */
     , (33631,  4452,  2.009)  /* Incantation of Lightning Streak */
     , (33631,  2141,  2.036)  /* Lhen's Flare */
     , (33631,  2133,  2.036)  /* Outlander's Insolence */
     , (33631,  2137,  2.005)  /* Sudden Frost */
     , (33631,  2132,  2.005)  /* The Spike */
     , (33631,  2174,  2.005)  /* Archer's Gift */
     , (33631,  2172,  2.005)  /* Astyrrian's Gift */
     , (33631,  2168,   2.01)  /* Gelidite's Gift */
     , (33631,  2074,   2.01)  /* Gossamer Flesh */
     , (33631,  2318,   2.01)  /* Gravity Well */
     , (33631,  1161,  2.009)  /* Heal Self VI */
     , (33631,  2282,   2.02)  /* Futility */
     , (33631,  2164,   2.02)  /* Swordsman's Gift */
     , (33631,  1265,  2.009)  /* Drain Mana Other VI */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33631, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33631, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33631, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33631, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33631, 9, 44469,  1, 0, 0, False) /* Create Lesser Corrupted Essence (44469) for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33631, -1, 40291, 1, 1, 1, 1, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Degenerate Shadow (40291) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */
     , (33631, -1, 40293, 1, 1, 1, 1, 2, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Degenerate Shadow (40293) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
