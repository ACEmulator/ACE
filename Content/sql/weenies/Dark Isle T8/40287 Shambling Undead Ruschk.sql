DELETE FROM `weenie` WHERE `class_Id` = 40287;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40287, 'ace40287-shamblingundeadruschk', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40287,   1,         16) /* ItemType - Creature */
     , (40287,   2,         14) /* CreatureType - Undead */
     , (40287,   6,         -1) /* ItemsCapacity */
     , (40287,   7,         -1) /* ContainersCapacity */
     , (40287,  16,          1) /* ItemUseable - No */
     , (40287,  25,        185) /* Level */
     , (40287,  27,          0) /* ArmorType - None */
     , (40287,  40,          2) /* CombatMode - Melee */
     , (40287,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (40287,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (40287, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (40287, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40287, 140,          1) /* AiOptions - CanOpenDoors */
     , (40287, 146,     200000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40287,   1, True ) /* Stuck */
     , (40287,  11, False) /* IgnoreCollisions */
     , (40287,  12, True ) /* ReportCollisions */
     , (40287,  13, False) /* Ethereal */
     , (40287,  14, True ) /* GravityStatus */
     , (40287,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40287,   1,       5) /* HeartbeatInterval */
     , (40287,   2,       0) /* HeartbeatTimestamp */
     , (40287,   3,    0.15) /* HealthRate */
     , (40287,   4,       4) /* StaminaRate */
     , (40287,   5,     1.5) /* ManaRate */
     , (40287,  12,       0) /* Shade */
     , (40287,  13,     0.9) /* ArmorModVsSlash */
     , (40287,  14,     0.6) /* ArmorModVsPierce */
     , (40287,  15,     1.1) /* ArmorModVsBludgeon */
     , (40287,  16,     0.8) /* ArmorModVsCold */
     , (40287,  17,    0.55) /* ArmorModVsFire */
     , (40287,  18,       1) /* ArmorModVsAcid */
     , (40287,  19,     0.8) /* ArmorModVsElectric */
     , (40287,  31,      17) /* VisualAwarenessRange */
     , (40287,  34,       1) /* PowerupTime */
     , (40287,  36,       1) /* ChargeSpeed */
     , (40287,  39,       1) /* DefaultScale */
     , (40287,  64,     0.1) /* ResistSlash */
     , (40287,  65,     0.1) /* ResistPierce */
     , (40287,  66,     0.3) /* ResistBludgeon */
     , (40287,  67,     0.3) /* ResistFire */
     , (40287,  68,     0.1) /* ResistCold */
     , (40287,  69,     0.2) /* ResistAcid */
     , (40287,  70,     0.1) /* ResistElectric */
     , (40287,  71,       1) /* ResistHealthBoost */
     , (40287,  72,     0.5) /* ResistStaminaDrain */
     , (40287,  73,       1) /* ResistStaminaBoost */
     , (40287,  74,     0.5) /* ResistManaDrain */
     , (40287,  75,       1) /* ResistManaBoost */
     , (40287, 104,      10) /* ObviousRadarRange */
     , (40287, 125,     0.5) /* ResistHealthDrain */
     , (40287, 166,     0.2) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40287,   1, 'Shambling Undead Ruschk') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40287,   1, 0x020015CD) /* Setup */
     , (40287,   2, 0x09000007) /* MotionTable */
     , (40287,   3, 0x200000BD) /* SoundTable */
     , (40287,   4, 0x30000004) /* CombatTable */
     , (40287,   8, 0x060036FD) /* Icon */
     , (40287,  22, 0x34000084) /* PhysicsEffectTable */
     , (40287,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40287,   1, 310, 0, 0) /* Strength */
     , (40287,   2, 240, 0, 0) /* Endurance */
     , (40287,   3, 200, 0, 0) /* Quickness */
     , (40287,   4, 240, 0, 0) /* Coordination */
     , (40287,   5, 210, 0, 0) /* Focus */
     , (40287,   6, 210, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40287,   1,   740, 0, 0, 860) /* MaxHealth */
     , (40287,   3,   800, 0, 0, 1040) /* MaxStamina */
     , (40287,   5,   200, 0, 0, 410) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40287,  6, 0, 3, 0, 375, 0, 0) /* MeleeDefense        Specialized */
     , (40287,  7, 0, 3, 0, 370, 0, 0) /* MissileDefense      Specialized */
     , (40287, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (40287, 15, 0, 3, 0, 400, 0, 0) /* MagicDefense        Specialized */
     , (40287, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (40287, 31, 0, 3, 0, 275, 0, 0) /* CreatureEnchantment Specialized */
     , (40287, 32, 0, 3, 0, 275, 0, 0) /* ItemEnchantment     Specialized */
     , (40287, 33, 0, 3, 0, 275, 0, 0) /* LifeMagic           Specialized */
     , (40287, 34, 0, 3, 0, 290, 0, 0) /* WarMagic            Specialized */
     , (40287, 45, 0, 3, 0, 385, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40287,  0,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (40287,  1,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (40287,  2,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (40287,  3,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (40287,  4,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (40287,  5,  4, 60,  0.5,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (40287,  6,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (40287,  7,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (40287,  8,  4, 50,  0.4,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40287,  2074,   2.02)  /* Gossamer Flesh */
     , (40287,  2136,   2.02)  /* Icy Torment */
     , (40287,  2168,   2.02)  /* Gelidite's Gift */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40287, 2, 48633,  1, 0, 0, False) /* Create Glacial Blade (48633) for Wield */
     , (40287, 2, 48630,  1, 0, 0, False) /* Create  (48630) for Wield */
     , (40287, 2, 48631,  1, 0, 0, False) /* Create  (48631) for Wield */
     , (40287, 2, 48632,  1, 0, 0, False) /* Create Tursh's Spear (48632) for Wield */
     , (40287, 2, 48629,  1, 0, 0, False) /* Create Icy Club (48629) for Wield */
     , (40287, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40287, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40287, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40287, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
