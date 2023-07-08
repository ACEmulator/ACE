DELETE FROM `weenie` WHERE `class_Id` = 40293;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40293, 'ace40293-degenerateshadow', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40293,   1,         16) /* ItemType - Creature */
     , (40293,   2,         22) /* CreatureType - Shadow */
     , (40293,   3,          2) /* PaletteTemplate - Blue */
     , (40293,   6,         -1) /* ItemsCapacity */
     , (40293,   7,         -1) /* ContainersCapacity */
     , (40293,   8,         90) /* Mass */
     , (40293,  16,          1) /* ItemUseable - No */
     , (40293,  25,        185) /* Level */
     , (40293,  27,          0) /* ArmorType - None */
     , (40293,  68,          3) /* TargetingTactic - Random, Focused */
     , (40293,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (40293, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (40293, 113,          1) /* Gender - Male */
     , (40293, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40293, 140,          1) /* AiOptions - CanOpenDoors */
     , (40293, 146,     200000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40293,   1, True ) /* Stuck */
     , (40293,   6, True ) /* AiUsesMana */
     , (40293,  11, False) /* IgnoreCollisions */
     , (40293,  12, True ) /* ReportCollisions */
     , (40293,  13, False) /* Ethereal */
     , (40293,  14, True ) /* GravityStatus */
     , (40293,  19, True ) /* Attackable */
     , (40293,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40293,   1,       5) /* HeartbeatInterval */
     , (40293,   2,       0) /* HeartbeatTimestamp */
     , (40293,   3,     0.7) /* HealthRate */
     , (40293,   4,     2.5) /* StaminaRate */
     , (40293,   5,       1) /* ManaRate */
     , (40293,  12,     0.5) /* Shade */
     , (40293,  13,     0.9) /* ArmorModVsSlash */
     , (40293,  14,       1) /* ArmorModVsPierce */
     , (40293,  15,       1) /* ArmorModVsBludgeon */
     , (40293,  16,     1.1) /* ArmorModVsCold */
     , (40293,  17,     0.9) /* ArmorModVsFire */
     , (40293,  18,       1) /* ArmorModVsAcid */
     , (40293,  19,       1) /* ArmorModVsElectric */
     , (40293,  31,      20) /* VisualAwarenessRange */
     , (40293,  34,     1.2) /* PowerupTime */
     , (40293,  36,       1) /* ChargeSpeed */
     , (40293,  39,       1) /* DefaultScale */
     , (40293,  64,     0.8) /* ResistSlash */
     , (40293,  65,     0.5) /* ResistPierce */
     , (40293,  66,     0.7) /* ResistBludgeon */
     , (40293,  67,     0.8) /* ResistFire */
     , (40293,  68,     0.1) /* ResistCold */
     , (40293,  69,     0.2) /* ResistAcid */
     , (40293,  70,     0.5) /* ResistElectric */
     , (40293,  71,       1) /* ResistHealthBoost */
     , (40293,  72,       1) /* ResistStaminaDrain */
     , (40293,  73,       1) /* ResistStaminaBoost */
     , (40293,  74,       1) /* ResistManaDrain */
     , (40293,  75,       1) /* ResistManaBoost */
     , (40293,  76,     0.5) /* Translucency */
     , (40293,  80,       3) /* AiUseMagicDelay */
     , (40293, 104,      10) /* ObviousRadarRange */
     , (40293, 122,       2) /* AiAcquireHealth */
     , (40293, 125,       1) /* ResistHealthDrain */
     , (40293, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40293,   1, 'Degenerate Shadow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40293,   1, 0x02001526) /* Setup */
     , (40293,   2, 0x09000186) /* MotionTable */
     , (40293,   3, 0x200000BE) /* SoundTable */
     , (40293,   4, 0x30000000) /* CombatTable */
     , (40293,   6, 0x040019CC) /* PaletteBase */
     , (40293,   7, 0x100005AB) /* ClothingBase */
     , (40293,   8, 0x06001BBE) /* Icon */
     , (40293,  22, 0x34000063) /* PhysicsEffectTable */
     , (40293,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40293,   1, 300, 0, 0) /* Strength */
     , (40293,   2, 400, 0, 0) /* Endurance */
     , (40293,   3, 300, 0, 0) /* Quickness */
     , (40293,   4, 300, 0, 0) /* Coordination */
     , (40293,   5, 540, 0, 0) /* Focus */
     , (40293,   6, 560, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40293,   1,   800, 0, 0, 1000) /* MaxHealth */
     , (40293,   3,   300, 0, 0, 700) /* MaxStamina */
     , (40293,   5,   280, 0, 0, 860) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40293,  6, 0, 3, 0, 310, 0, 0) /* MeleeDefense        Specialized */
     , (40293,  7, 0, 3, 0, 410, 0, 0) /* MissileDefense      Specialized */
     , (40293, 14, 0, 3, 0, 200, 0, 0) /* ArcaneLore          Specialized */
     , (40293, 15, 0, 3, 0, 243, 0, 0) /* MagicDefense        Specialized */
     , (40293, 31, 0, 3, 0, 225, 0, 0) /* CreatureEnchantment Specialized */
     , (40293, 33, 0, 3, 0, 225, 0, 0) /* LifeMagic           Specialized */
     , (40293, 34, 0, 3, 0, 225, 0, 0) /* WarMagic            Specialized */
     , (40293, 44, 0, 3, 0, 308, 0, 0) /* HeavyWeapons        Specialized */
     , (40293, 45, 0, 3, 0, 308, 0, 0) /* LightWeapons        Specialized */
     , (40293, 46, 0, 3, 0, 293, 0, 0) /* FinesseWeapons      Specialized */
     , (40293, 47, 0, 3, 0, 220, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40293,  0,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (40293,  1,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (40293,  2,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (40293,  3,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (40293,  4,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (40293,  5,  4, 120, 0.75,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (40293,  6,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (40293,  7,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (40293,  8,  4, 140, 0.75,   60,   54,   60,   60,   66,   54,   60,   60,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40293,  2140,  2.036)  /* Alset's Coil */
     , (40293,  2136,  2.036)  /* Icy Torment */
     , (40293,  4452,  2.009)  /* Incantation of Lightning Streak */
     , (40293,  2141,  2.036)  /* Lhen's Flare */
     , (40293,  2133,  2.036)  /* Outlander's Insolence */
     , (40293,  2137,  2.005)  /* Sudden Frost */
     , (40293,  2132,  2.005)  /* The Spike */
     , (40293,  2174,  2.005)  /* Archer's Gift */
     , (40293,  2172,  2.005)  /* Astyrrian's Gift */
     , (40293,  2168,   2.01)  /* Gelidite's Gift */
     , (40293,  2074,   2.01)  /* Gossamer Flesh */
     , (40293,  2318,   2.01)  /* Gravity Well */
     , (40293,  1161,  2.009)  /* Heal Self VI */
     , (40293,  2282,   2.02)  /* Futility */
     , (40293,  2164,   2.02)  /* Swordsman's Gift */
     , (40293,  1265,  2.009)  /* Drain Mana Other VI */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40293, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40293, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40293, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40293, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
