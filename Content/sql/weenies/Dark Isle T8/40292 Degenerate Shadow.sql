DELETE FROM `weenie` WHERE `class_Id` = 40292;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40292, 'ace40292-degenerateshadow', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40292,   1,         16) /* ItemType - Creature */
     , (40292,   2,         22) /* CreatureType - Shadow */
     , (40292,   3,         39) /* PaletteTemplate - Black */
     , (40292,   6,         -1) /* ItemsCapacity */
     , (40292,   7,         -1) /* ContainersCapacity */
     , (40292,   8,         90) /* Mass */
     , (40292,  16,          1) /* ItemUseable - No */
     , (40292,  25,        185) /* Level */
     , (40292,  27,          0) /* ArmorType - None */
     , (40292,  68,          3) /* TargetingTactic - Random, Focused */
     , (40292,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (40292, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (40292, 113,          2) /* Gender - Female */
     , (40292, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40292, 140,          1) /* AiOptions - CanOpenDoors */
     , (40292, 146,     200000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40292,   1, True ) /* Stuck */
     , (40292,   6, True ) /* AiUsesMana */
     , (40292,  11, False) /* IgnoreCollisions */
     , (40292,  12, True ) /* ReportCollisions */
     , (40292,  13, False) /* Ethereal */
     , (40292,  14, True ) /* GravityStatus */
     , (40292,  19, True ) /* Attackable */
     , (40292,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40292,   1,       5) /* HeartbeatInterval */
     , (40292,   2,       0) /* HeartbeatTimestamp */
     , (40292,   3,     0.7) /* HealthRate */
     , (40292,   4,     2.5) /* StaminaRate */
     , (40292,   5,       1) /* ManaRate */
     , (40292,  12,       0) /* Shade */
     , (40292,  13,     0.9) /* ArmorModVsSlash */
     , (40292,  14,       1) /* ArmorModVsPierce */
     , (40292,  15,       1) /* ArmorModVsBludgeon */
     , (40292,  16,     1.1) /* ArmorModVsCold */
     , (40292,  17,     0.9) /* ArmorModVsFire */
     , (40292,  18,       1) /* ArmorModVsAcid */
     , (40292,  19,       1) /* ArmorModVsElectric */
     , (40292,  31,      20) /* VisualAwarenessRange */
     , (40292,  34,     1.2) /* PowerupTime */
     , (40292,  36,       1) /* ChargeSpeed */
     , (40292,  39,       1) /* DefaultScale */
     , (40292,  64,     0.8) /* ResistSlash */
     , (40292,  65,     0.5) /* ResistPierce */
     , (40292,  66,     0.7) /* ResistBludgeon */
     , (40292,  67,     0.8) /* ResistFire */
     , (40292,  68,     0.1) /* ResistCold */
     , (40292,  69,     0.2) /* ResistAcid */
     , (40292,  70,     0.5) /* ResistElectric */
     , (40292,  71,       1) /* ResistHealthBoost */
     , (40292,  72,       1) /* ResistStaminaDrain */
     , (40292,  73,       1) /* ResistStaminaBoost */
     , (40292,  74,       1) /* ResistManaDrain */
     , (40292,  75,       1) /* ResistManaBoost */
     , (40292,  76,     0.5) /* Translucency */
     , (40292,  80,       3) /* AiUseMagicDelay */
     , (40292, 104,      10) /* ObviousRadarRange */
     , (40292, 122,       2) /* AiAcquireHealth */
     , (40292, 125,       1) /* ResistHealthDrain */
     , (40292, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40292,   1, 'Degenerate Shadow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40292,   1, 0x0200071B) /* Setup */
     , (40292,   2, 0x09000093) /* MotionTable */
     , (40292,   3, 0x20000002) /* SoundTable */
     , (40292,   4, 0x30000000) /* CombatTable */
     , (40292,   6, 0x0400007E) /* PaletteBase */
     , (40292,   7, 0x1000019F) /* ClothingBase */
     , (40292,   8, 0x06001BBE) /* Icon */
     , (40292,  22, 0x34000063) /* PhysicsEffectTable */
     , (40292,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40292,   1, 190, 0, 0) /* Strength */
     , (40292,   2, 210, 0, 0) /* Endurance */
     , (40292,   3, 260, 0, 0) /* Quickness */
     , (40292,   4, 240, 0, 0) /* Coordination */
     , (40292,   5, 220, 0, 0) /* Focus */
     , (40292,   6, 140, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40292,   1,   700, 0, 0, 805) /* MaxHealth */
     , (40292,   3,  1000, 0, 0, 1210) /* MaxStamina */
     , (40292,   5,  1000, 0, 0, 1140) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40292,  6, 0, 3, 0, 310, 0, 0) /* MeleeDefense        Specialized */
     , (40292,  7, 0, 3, 0, 410, 0, 0) /* MissileDefense      Specialized */
     , (40292, 14, 0, 3, 0, 200, 0, 0) /* ArcaneLore          Specialized */
     , (40292, 15, 0, 3, 0, 243, 0, 0) /* MagicDefense        Specialized */
     , (40292, 31, 0, 3, 0, 225, 0, 0) /* CreatureEnchantment Specialized */
     , (40292, 33, 0, 3, 0, 225, 0, 0) /* LifeMagic           Specialized */
     , (40292, 34, 0, 3, 0, 225, 0, 0) /* WarMagic            Specialized */
     , (40292, 44, 0, 3, 0, 308, 0, 0) /* HeavyWeapons        Specialized */
     , (40292, 45, 0, 3, 0, 308, 0, 0) /* LightWeapons        Specialized */
     , (40292, 46, 0, 3, 0, 293, 0, 0) /* FinesseWeapons      Specialized */
     , (40292, 47, 0, 3, 0, 220, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40292,  0,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (40292,  1,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (40292,  2,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (40292,  3,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (40292,  4,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (40292,  5,  4, 50, 0.75,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (40292,  6,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (40292,  7,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (40292,  8,  4, 60, 0.75,   60,   54,   60,   60,   66,   54,   60,   60,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40292,  1161,  2.009)  /* Heal Self VI */
     , (40292,  1265,  2.009)  /* Drain Mana Other VI */
     , (40292,  2074,   2.01)  /* Gossamer Flesh */
     , (40292,  2132,  2.005)  /* The Spike */
     , (40292,  2133,  2.036)  /* Outlander's Insolence */
     , (40292,  2136,  2.036)  /* Icy Torment */
     , (40292,  2137,  2.005)  /* Sudden Frost */
     , (40292,  2140,  2.036)  /* Alset's Coil */
     , (40292,  2141,  2.036)  /* Lhen's Flare */
     , (40292,  2164,   2.02)  /* Swordsman's Gift */
     , (40292,  2168,   2.01)  /* Gelidite's Gift */
     , (40292,  2172,  2.005)  /* Astyrrian's Gift */
     , (40292,  2174,  2.005)  /* Archer's Gift */
     , (40292,  2282,   2.02)  /* Futility */
     , (40292,  2318,   2.01)  /* Gravity Well */
     , (40292,  4452,  2.009)  /* Incantation of Lightning Streak */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40292, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40292, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40292, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40292, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
