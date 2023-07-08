DELETE FROM `weenie` WHERE `class_Id` = 33633;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33633, 'ace33633-depravedshadowcommander', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33633,   1,         16) /* ItemType - Creature */
     , (33633,   2,         22) /* CreatureType - Shadow */
     , (33633,   3,         39) /* PaletteTemplate - Black */
     , (33633,   6,         -1) /* ItemsCapacity */
     , (33633,   7,         -1) /* ContainersCapacity */
     , (33633,   8,         90) /* Mass */
     , (33633,  16,          1) /* ItemUseable - No */
     , (33633,  25,        200) /* Level */
     , (33633,  27,          0) /* ArmorType - None */
     , (33633,  68,          3) /* TargetingTactic - Random, Focused */
     , (33633,  81,          2) /* MaxGeneratedObjects */
     , (33633,  82,          2) /* InitGeneratedObjects */
     , (33633,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (33633, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (33633, 103,          3) /* GeneratorDestructionType - Kill */
     , (33633, 113,          1) /* Gender - Male */
     , (33633, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33633, 140,          1) /* AiOptions - CanOpenDoors */
     , (33633, 146,     320000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33633,   1, True ) /* Stuck */
     , (33633,   6, True ) /* AiUsesMana */
     , (33633,  11, False) /* IgnoreCollisions */
     , (33633,  12, True ) /* ReportCollisions */
     , (33633,  13, False) /* Ethereal */
     , (33633,  14, True ) /* GravityStatus */
     , (33633,  19, True ) /* Attackable */
     , (33633,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33633,   1,       5) /* HeartbeatInterval */
     , (33633,   2,       0) /* HeartbeatTimestamp */
     , (33633,   3,     0.7) /* HealthRate */
     , (33633,   4,     2.5) /* StaminaRate */
     , (33633,   5,       1) /* ManaRate */
     , (33633,  12,       0) /* Shade */
     , (33633,  13,       1) /* ArmorModVsSlash */
     , (33633,  14,     0.8) /* ArmorModVsPierce */
     , (33633,  15,    0.85) /* ArmorModVsBludgeon */
     , (33633,  16,     1.1) /* ArmorModVsCold */
     , (33633,  17,     0.6) /* ArmorModVsFire */
     , (33633,  18,     0.7) /* ArmorModVsAcid */
     , (33633,  19,    0.75) /* ArmorModVsElectric */
     , (33633,  31,      20) /* VisualAwarenessRange */
     , (33633,  34,     1.2) /* PowerupTime */
     , (33633,  36,       1) /* ChargeSpeed */
     , (33633,  39,     1.3) /* DefaultScale */
     , (33633,  41,      60) /* RegenerationInterval */
     , (33633,  43,       4) /* GeneratorRadius */
     , (33633,  64,       1) /* ResistSlash */
     , (33633,  65,     0.5) /* ResistPierce */
     , (33633,  66,     0.7) /* ResistBludgeon */
     , (33633,  67,       1) /* ResistFire */
     , (33633,  68,     0.1) /* ResistCold */
     , (33633,  69,     0.2) /* ResistAcid */
     , (33633,  70,     0.5) /* ResistElectric */
     , (33633,  71,       1) /* ResistHealthBoost */
     , (33633,  72,       1) /* ResistStaminaDrain */
     , (33633,  73,       1) /* ResistStaminaBoost */
     , (33633,  74,       1) /* ResistManaDrain */
     , (33633,  75,       1) /* ResistManaBoost */
     , (33633,  76,     0.5) /* Translucency */
     , (33633,  80,       3) /* AiUseMagicDelay */
     , (33633, 104,      10) /* ObviousRadarRange */
     , (33633, 122,       2) /* AiAcquireHealth */
     , (33633, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33633,   1, 'Depraved Shadow Commander') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33633,   1, 0x0200071B) /* Setup */
     , (33633,   2, 0x09000093) /* MotionTable */
     , (33633,   3, 0x20000002) /* SoundTable */
     , (33633,   4, 0x30000000) /* CombatTable */
     , (33633,   6, 0x0400007E) /* PaletteBase */
     , (33633,   7, 0x1000019F) /* ClothingBase */
     , (33633,   8, 0x06001BBE) /* Icon */
     , (33633,  22, 0x34000063) /* PhysicsEffectTable */
     , (33633,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33633,   1, 310, 0, 0) /* Strength */
     , (33633,   2, 420, 0, 0) /* Endurance */
     , (33633,   3, 310, 0, 0) /* Quickness */
     , (33633,   4, 310, 0, 0) /* Coordination */
     , (33633,   5, 550, 0, 0) /* Focus */
     , (33633,   6, 570, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33633,   1, 12000, 0, 0, 12205) /* MaxHealth */
     , (33633,   3,  1000, 0, 0, 1210) /* MaxStamina */
     , (33633,   5,  1000, 0, 0, 1140) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33633,  6, 0, 3, 0, 310, 0, 0) /* MeleeDefense        Specialized */
     , (33633,  7, 0, 3, 0, 410, 0, 0) /* MissileDefense      Specialized */
     , (33633, 14, 0, 3, 0, 200, 0, 0) /* ArcaneLore          Specialized */
     , (33633, 15, 0, 3, 0, 243, 0, 0) /* MagicDefense        Specialized */
     , (33633, 31, 0, 3, 0, 225, 0, 0) /* CreatureEnchantment Specialized */
     , (33633, 33, 0, 3, 0, 225, 0, 0) /* LifeMagic           Specialized */
     , (33633, 34, 0, 3, 0, 225, 0, 0) /* WarMagic            Specialized */
     , (33633, 44, 0, 3, 0, 308, 0, 0) /* HeavyWeapons        Specialized */
     , (33633, 45, 0, 3, 0, 308, 0, 0) /* LightWeapons        Specialized */
     , (33633, 46, 0, 3, 0, 293, 0, 0) /* FinesseWeapons      Specialized */
     , (33633, 47, 0, 3, 0, 220, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33633,  0,  4,  0,    0,  500,  500,  400,  425,  550,  300,  350,  375,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (33633,  1,  4,  0,    0,  500,  500,  400,  425,  550,  300,  350,  375,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (33633,  2,  4,  0,    0,  500,  500,  400,  425,  550,  300,  350,  375,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (33633,  3,  4,  0,    0,  500,  500,  400,  425,  550,  300,  350,  375,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (33633,  4,  4,  0,    0,  500,  500,  400,  425,  550,  300,  350,  375,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (33633,  5,  4, 50, 0.75,  500,  500,  400,  425,  550,  300,  350,  375,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (33633,  6,  4,  0,    0,  500,  500,  400,  425,  550,  300,  350,  375,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (33633,  7,  4,  0,    0,  500,  500,  400,  425,  550,  300,  350,  375,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (33633,  8,  4, 60, 0.75,  500,  500,  400,  425,  550,  300,  350,  375,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33633,  1161,  2.009)  /* Heal Self VI */
     , (33633,  1265,  2.009)  /* Drain Mana Other VI */
     , (33633,  2074,   2.01)  /* Gossamer Flesh */
     , (33633,  2132,  2.005)  /* The Spike */
     , (33633,  2133,  2.036)  /* Outlander's Insolence */
     , (33633,  2136,  2.036)  /* Icy Torment */
     , (33633,  2137,  2.005)  /* Sudden Frost */
     , (33633,  2140,  2.036)  /* Alset's Coil */
     , (33633,  2141,  2.036)  /* Lhen's Flare */
     , (33633,  2164,   2.02)  /* Swordsman's Gift */
     , (33633,  2168,   2.01)  /* Gelidite's Gift */
     , (33633,  2172,  2.005)  /* Astyrrian's Gift */
     , (33633,  2174,  2.005)  /* Archer's Gift */
     , (33633,  2282,   2.02)  /* Futility */
     , (33633,  2318,   2.01)  /* Gravity Well */
     , (33633,  4452,  2.009)  /* Incantation of Lightning Streak */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33633, 9, 44470,  1, 0, 0, False) /* Create Corrupted Essence (44470) for ContainTreasure */
     , (33633, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33633, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33633, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33633, -1, 40295, 3600, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Depraved Shadow (40295) (x2 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
