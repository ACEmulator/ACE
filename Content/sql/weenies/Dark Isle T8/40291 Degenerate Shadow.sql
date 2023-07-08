DELETE FROM `weenie` WHERE `class_Id` = 40291;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40291, 'ace40291-degenerateshadow', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40291,   1,         16) /* ItemType - Creature */
     , (40291,   2,         22) /* CreatureType - Shadow */
     , (40291,   3,         39) /* PaletteTemplate - Black */
     , (40291,   6,         -1) /* ItemsCapacity */
     , (40291,   7,         -1) /* ContainersCapacity */
     , (40291,   8,         90) /* Mass */
     , (40291,  16,          1) /* ItemUseable - No */
     , (40291,  25,        185) /* Level */
     , (40291,  27,          0) /* ArmorType - None */
     , (40291,  68,          3) /* TargetingTactic - Random, Focused */
     , (40291,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (40291, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (40291, 113,          2) /* Gender - Female */
     , (40291, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40291, 140,          1) /* AiOptions - CanOpenDoors */
     , (40291, 146,     200000) /* XpOverride */
     , (40291, 188,          1) /* HeritageGroup - Aluvian */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40291,   1, True ) /* Stuck */
     , (40291,   6, True ) /* AiUsesMana */
     , (40291,  11, False) /* IgnoreCollisions */
     , (40291,  12, True ) /* ReportCollisions */
     , (40291,  13, False) /* Ethereal */
     , (40291,  14, True ) /* GravityStatus */
     , (40291,  19, True ) /* Attackable */
     , (40291,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40291,   1,       5) /* HeartbeatInterval */
     , (40291,   2,       0) /* HeartbeatTimestamp */
     , (40291,   3,     0.7) /* HealthRate */
     , (40291,   4,     2.5) /* StaminaRate */
     , (40291,   5,       1) /* ManaRate */
     , (40291,  12,     0.5) /* Shade */
     , (40291,  13,     0.9) /* ArmorModVsSlash */
     , (40291,  14,       1) /* ArmorModVsPierce */
     , (40291,  15,       1) /* ArmorModVsBludgeon */
     , (40291,  16,     1.1) /* ArmorModVsCold */
     , (40291,  17,     0.9) /* ArmorModVsFire */
     , (40291,  18,       1) /* ArmorModVsAcid */
     , (40291,  19,       1) /* ArmorModVsElectric */
     , (40291,  31,      20) /* VisualAwarenessRange */
     , (40291,  34,     1.2) /* PowerupTime */
     , (40291,  36,       1) /* ChargeSpeed */
     , (40291,  39,       1) /* DefaultScale */
     , (40291,  64,     0.8) /* ResistSlash */
     , (40291,  65,     0.5) /* ResistPierce */
     , (40291,  66,     0.7) /* ResistBludgeon */
     , (40291,  67,     0.8) /* ResistFire */
     , (40291,  68,     0.1) /* ResistCold */
     , (40291,  69,     0.2) /* ResistAcid */
     , (40291,  70,     0.5) /* ResistElectric */
     , (40291,  71,       1) /* ResistHealthBoost */
     , (40291,  72,       1) /* ResistStaminaDrain */
     , (40291,  73,       1) /* ResistStaminaBoost */
     , (40291,  74,       1) /* ResistManaDrain */
     , (40291,  75,       1) /* ResistManaBoost */
     , (40291,  80,       3) /* AiUseMagicDelay */
     , (40291, 104,      10) /* ObviousRadarRange */
     , (40291, 122,       2) /* AiAcquireHealth */
     , (40291, 125,       1) /* ResistHealthDrain */
     , (40291, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40291,   1, 'Degenerate Shadow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40291,   1, 0x0200004E) /* Setup */
     , (40291,   2, 0x09000001) /* MotionTable */
     , (40291,   3, 0x20000002) /* SoundTable */
     , (40291,   4, 0x30000000) /* CombatTable */
     , (40291,   6, 0x0400007E) /* PaletteBase */
     , (40291,   7, 0x1000019F) /* ClothingBase */
     , (40291,   8, 0x06001BBE) /* Icon */
     , (40291,  22, 0x34000063) /* PhysicsEffectTable */
     , (40291,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40291,   1, 300, 0, 0) /* Strength */
     , (40291,   2, 400, 0, 0) /* Endurance */
     , (40291,   3, 300, 0, 0) /* Quickness */
     , (40291,   4, 300, 0, 0) /* Coordination */
     , (40291,   5, 540, 0, 0) /* Focus */
     , (40291,   6, 560, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40291,   1,   605, 0, 0, 805) /* MaxHealth */
     , (40291,   3,   300, 0, 0, 700) /* MaxStamina */
     , (40291,   5,   280, 0, 0, 860) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40291,  6, 0, 3, 0, 310, 0, 0) /* MeleeDefense        Specialized */
     , (40291,  7, 0, 3, 0, 410, 0, 0) /* MissileDefense      Specialized */
     , (40291, 14, 0, 3, 0, 200, 0, 0) /* ArcaneLore          Specialized */
     , (40291, 15, 0, 3, 0, 243, 0, 0) /* MagicDefense        Specialized */
     , (40291, 31, 0, 3, 0, 225, 0, 0) /* CreatureEnchantment Specialized */
     , (40291, 33, 0, 3, 0, 225, 0, 0) /* LifeMagic           Specialized */
     , (40291, 34, 0, 3, 0, 225, 0, 0) /* WarMagic            Specialized */
     , (40291, 44, 0, 3, 0, 308, 0, 0) /* HeavyWeapons        Specialized */
     , (40291, 45, 0, 3, 0, 308, 0, 0) /* LightWeapons        Specialized */
     , (40291, 46, 0, 3, 0, 293, 0, 0) /* FinesseWeapons      Specialized */
     , (40291, 47, 0, 3, 0, 220, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40291,  0,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (40291,  1,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (40291,  2,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (40291,  3,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (40291,  4,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (40291,  5,  4, 50, 0.75,  500,  450,  500,  500,  550,  450,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (40291,  6,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (40291,  7,  4,  0,    0,  500,  450,  500,  500,  550,  450,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (40291,  8,  4, 60, 0.75,   60,   54,   60,   60,   66,   54,   60,   60,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40291,  2140,  2.036)  /* Alset's Coil */
     , (40291,  2136,  2.036)  /* Icy Torment */
     , (40291,  2141,  2.036)  /* Lhen's Flare */
     , (40291,  2133,  2.036)  /* Outlander's Insolence */
     , (40291,  2137,  2.005)  /* Sudden Frost */
     , (40291,  2132,  2.005)  /* The Spike */
     , (40291,  2174,  2.005)  /* Archer's Gift */
     , (40291,  2172,  2.005)  /* Astyrrian's Gift */
     , (40291,  2168,   2.01)  /* Gelidite's Gift */
     , (40291,  2074,   2.01)  /* Gossamer Flesh */
     , (40291,  2318,   2.01)  /* Gravity Well */
     , (40291,  4452,  2.009)  /* Incantation of Lightning Streak */
     , (40291,  2282,   2.02)  /* Futility */
     , (40291,  2164,   2.02)  /* Swordsman's Gift */
     , (40291,  1265,  2.009)  /* Drain Mana Other VI */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40291, 2, 32852,  1, 0, 0, False) /* Create Blade of the Realm (32852) for Wield */
     , (40291, 2, 32637,  1, 0, 0, False) /* Create Shield of Elysa's Royal Guard (32637) for Wield */
     , (40291, 2,  2587,  0, 14, 0, False) /* Create Shirt (2587) for Wield */
     , (40291, 2,  2601,  0, 14, 0, False) /* Create Loose Pants (2601) for Wield */
     , (40291, 2, 21150,  0, 21, 0, False) /* Create Covenant Sollerets (21150) for Wield */
     , (40291, 2, 21151,  0, 21, 0, False) /* Create Covenant Bracers (21151) for Wield */
     , (40291, 2, 21152,  0, 21, 0, False) /* Create Covenant Breastplate (21152) for Wield */
     , (40291, 2, 21153,  0, 21, 0, False) /* Create Covenant Gauntlets (21153) for Wield */
     , (40291, 2, 21154,  0, 21, 0, False) /* Create Covenant Girth (21154) for Wield */
     , (40291, 2, 21155,  0, 21, 0, False) /* Create Covenant Greaves (21155) for Wield */
     , (40291, 2, 34027,  0, 21, 0, False) /* Create Shadow Mask (34027) for Wield */
     , (40291, 2, 21157,  0, 21, 0, False) /* Create Covenant Pauldrons (21157) for Wield */
     , (40291, 2, 21159,  0, 21, 0, False) /* Create Covenant Tassets (21159) for Wield */
     , (40291, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40291, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40291, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40291, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
