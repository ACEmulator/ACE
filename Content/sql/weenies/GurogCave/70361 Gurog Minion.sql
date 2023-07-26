DELETE FROM `weenie` WHERE `class_Id` = 70361;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (70361, 'ace70361-gurogminion', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (70361,   1,         16) /* ItemType - Creature */
     , (70361,   2,        100) /* CreatureType - Gurog */
     , (70361,   6,         -1) /* ItemsCapacity */
     , (70361,   7,         -1) /* ContainersCapacity */
     , (70361,  16,          1) /* ItemUseable - No */
     , (70361,  25,        220) /* Level */
     , (70361,  27,          0) /* ArmorType - None */
     , (70361,  68,          5) /* TargetingTactic - Random, LastDamager */
     , (70361,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (70361, 101,          2) /* AiAllowedCombatStyle - OneHanded */
     , (70361, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (70361, 146,    1400000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (70361,   1, True ) /* Stuck */
     , (70361,   6, True ) /* AiUsesMana */
     , (70361,  11, False) /* IgnoreCollisions */
     , (70361,  12, True ) /* ReportCollisions */
     , (70361,  13, False) /* Ethereal */
     , (70361,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (70361,   1,       5) /* HeartbeatInterval */
     , (70361,   2,       0) /* HeartbeatTimestamp */
     , (70361,   3,     0.8) /* HealthRate */
     , (70361,   4,     0.8) /* StaminaRate */
     , (70361,   5,       2) /* ManaRate */
     , (70361,  12,       0) /* Shade */
     , (70361,  13,       1) /* ArmorModVsSlash */
     , (70361,  14,       1) /* ArmorModVsPierce */
     , (70361,  15,       1) /* ArmorModVsBludgeon */
     , (70361,  16,       1) /* ArmorModVsCold */
     , (70361,  17,    0.85) /* ArmorModVsFire */
     , (70361,  18,       1) /* ArmorModVsAcid */
     , (70361,  19,       1) /* ArmorModVsElectric */
     , (70361,  31,      16) /* VisualAwarenessRange */
     , (70361,  34,     1.3) /* PowerupTime */
     , (70361,  36,       1) /* ChargeSpeed */
     , (70361,  39,     1.3) /* DefaultScale */
     , (70361,  64,       1) /* ResistSlash */
     , (70361,  65,       1) /* ResistPierce */
     , (70361,  66,       1) /* ResistBludgeon */
     , (70361,  67,       1) /* ResistFire */
     , (70361,  68,       1) /* ResistCold */
     , (70361,  69,       1) /* ResistAcid */
     , (70361,  70,       1) /* ResistElectric */
     , (70361,  71,       1) /* ResistHealthBoost */
     , (70361,  72,       1) /* ResistStaminaDrain */
     , (70361,  73,       1) /* ResistStaminaBoost */
     , (70361,  74,       1) /* ResistManaDrain */
     , (70361,  75,       1) /* ResistManaBoost */
     , (70361,  80,       4) /* AiUseMagicDelay */
     , (70361, 104,      10) /* ObviousRadarRange */
     , (70361, 122,       2) /* AiAcquireHealth */
     , (70361, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (70361,   1, 'Gurog Minion') /* Name */
     , (70361,  45, 'KillTaskGurogMinion1110') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (70361,   1, 0x02001A2B) /* Setup */
     , (70361,   2, 0x090001A8) /* MotionTable */
     , (70361,   3, 0x200000D5) /* SoundTable */
     , (70361,   4, 0x30000000) /* CombatTable */
     , (70361,   8, 0x06002B2E) /* Icon */
     , (70361,  22, 0x340000CD) /* PhysicsEffectTable */
     , (70361,  35,       2000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (70361,   1, 550, 0, 0) /* Strength */
     , (70361,   2, 490, 0, 0) /* Endurance */
     , (70361,   3, 380, 0, 0) /* Quickness */
     , (70361,   4, 520, 0, 0) /* Coordination */
     , (70361,   5, 410, 0, 0) /* Focus */
     , (70361,   6, 410, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (70361,   1,  1655, 0, 0, 1900) /* MaxHealth */
     , (70361,   3,  3500, 0, 0, 3990) /* MaxStamina */
     , (70361,   5,  1000, 0, 0, 1410) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (70361,  6, 0, 3, 0, 360, 0, 0) /* MeleeDefense        Specialized */
     , (70361,  7, 0, 3, 0, 367, 0, 0) /* MissileDefense      Specialized */
     , (70361, 15, 0, 3, 0, 345, 0, 0) /* MagicDefense        Specialized */
     , (70361, 20, 0, 3, 0, 420, 0, 0) /* Deception           Specialized */
     , (70361, 33, 0, 3, 0, 460, 0, 0) /* LifeMagic           Specialized */
     , (70361, 34, 0, 3, 0, 460, 0, 0) /* WarMagic            Specialized */
     , (70361, 41, 0, 3, 0, 475, 0, 0) /* TwoHandedCombat     Specialized */
     , (70361, 44, 0, 3, 0, 445, 0, 0) /* HeavyWeapons        Specialized */
     , (70361, 45, 0, 3, 0, 475, 0, 0) /* LightWeapons        Specialized */
     , (70361, 46, 0, 3, 0, 475, 0, 0) /* FinesseWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (70361,  0,  4,  0,    0,  500,  500,  500,  500,  500,  425,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (70361,  1,  4,  0,    0,  500,  500,  500,  500,  500,  425,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (70361,  2,  4,  0,    0,  500,  500,  500,  500,  500,  425,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (70361,  3,  4,  0,    0,  500,  500,  500,  500,  500,  425,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (70361,  4,  4,  0,    0,  500,  500,  500,  500,  500,  425,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (70361,  5,  4, 200,  0.5,  500,  500,  500,  500,  500,  425,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (70361,  6,  4,  0,    0,  500,  500,  500,  500,  500,  425,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (70361,  7,  4,  0,    0,  500,  500,  500,  500,  500,  425,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (70361,  8,  4, 250,  0.5,  500,  500,  500,  500,  500,  425,  500,  500,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (70361,  2074,   2.01)  /* Gossamer Flesh */
     , (70361,  2135,   2.01)  /* Winter's Embrace */
     , (70361,  2136,   2.01)  /* Icy Torment */
     , (70361,  2166,   2.01)  /* Tusker's Gift */
     , (70361,  2168,   2.01)  /* Gelidite's Gift */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (70361, 9, 43520,  0, 0, 1, False) /* Create Torn Note (43520) for ContainTreasure */;
