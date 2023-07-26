DELETE FROM `weenie` WHERE `class_Id` = 43516;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (43516, 'ace43516-gurogsoldier', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (43516,   1,         16) /* ItemType - Creature */
     , (43516,   2,        100) /* CreatureType - Gurog */
     , (43516,   6,         -1) /* ItemsCapacity */
     , (43516,   7,         -1) /* ContainersCapacity */
     , (43516,  16,          1) /* ItemUseable - No */
     , (43516,  25,        220) /* Level */
     , (43516,  27,          0) /* ArmorType - None */
     , (43516,  68,          5) /* TargetingTactic - Random, LastDamager */
     , (43516,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (43516, 101,          2) /* AiAllowedCombatStyle - OneHanded */
     , (43516, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (43516, 146,    1400000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (43516,   1, True ) /* Stuck */
     , (43516,   6, False) /* AiUsesMana */
     , (43516,  11, False) /* IgnoreCollisions */
     , (43516,  12, True ) /* ReportCollisions */
     , (43516,  13, False) /* Ethereal */
     , (43516,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (43516,   1,       5) /* HeartbeatInterval */
     , (43516,   2,       0) /* HeartbeatTimestamp */
     , (43516,   3,     0.8) /* HealthRate */
     , (43516,   4,     0.8) /* StaminaRate */
     , (43516,   5,       2) /* ManaRate */
     , (43516,  12,       0) /* Shade */
     , (43516,  13,       1) /* ArmorModVsSlash */
     , (43516,  14,    0.55) /* ArmorModVsPierce */
     , (43516,  15,       1) /* ArmorModVsBludgeon */
     , (43516,  16,       1) /* ArmorModVsCold */
     , (43516,  17,    0.55) /* ArmorModVsFire */
     , (43516,  18,       1) /* ArmorModVsAcid */
     , (43516,  19,       1) /* ArmorModVsElectric */
     , (43516,  31,      18) /* VisualAwarenessRange */
     , (43516,  34,       1) /* PowerupTime */
     , (43516,  36,       1) /* ChargeSpeed */
     , (43516,  39,     1.3) /* DefaultScale */
     , (43516,  64,     0.3) /* ResistSlash */
     , (43516,  65,     0.8) /* ResistPierce */
     , (43516,  66,     0.3) /* ResistBludgeon */
     , (43516,  67,     0.8) /* ResistFire */
     , (43516,  68,     0.3) /* ResistCold */
     , (43516,  69,     0.3) /* ResistAcid */
     , (43516,  70,     0.4) /* ResistElectric */
     , (43516,  71,       1) /* ResistHealthBoost */
     , (43516,  72,       1) /* ResistStaminaDrain */
     , (43516,  73,       1) /* ResistStaminaBoost */
     , (43516,  74,       1) /* ResistManaDrain */
     , (43516,  75,       1) /* ResistManaBoost */
     , (43516,  80,       1) /* AiUseMagicDelay */
     , (43516, 104,      10) /* ObviousRadarRange */
     , (43516, 122,       2) /* AiAcquireHealth */
     , (43516, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (43516,   1, 'Gurog Soldier') /* Name */
     , (43516,  45, 'KillTaskGurogSoldier1110') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (43516,   1, 0x02001A2C) /* Setup */
     , (43516,   2, 0x090001A8) /* MotionTable */
     , (43516,   3, 0x200000D5) /* SoundTable */
     , (43516,   4, 0x30000000) /* CombatTable */
     , (43516,   8, 0x06002B2E) /* Icon */
     , (43516,  22, 0x340000CD) /* PhysicsEffectTable */
     , (43516,  35,       2000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (43516,   1, 550, 0, 0) /* Strength */
     , (43516,   2, 490, 0, 0) /* Endurance */
     , (43516,   3, 380, 0, 0) /* Quickness */
     , (43516,   4, 520, 0, 0) /* Coordination */
     , (43516,   5, 410, 0, 0) /* Focus */
     , (43516,   6, 410, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (43516,   1,  1655, 0, 0, 1900) /* MaxHealth */
     , (43516,   3,  3500, 0, 0, 3990) /* MaxStamina */
     , (43516,   5,  1000, 0, 0, 1410) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (43516,  6, 0, 3, 0, 470, 0, 0) /* MeleeDefense        Specialized */
     , (43516,  7, 0, 3, 0, 420, 0, 0) /* MissileDefense      Specialized */
     , (43516, 15, 0, 3, 0, 320, 0, 0) /* MagicDefense        Specialized */
     , (43516, 20, 0, 3, 0, 420, 0, 0) /* Deception           Specialized */
     , (43516, 33, 0, 3, 0, 265, 0, 0) /* LifeMagic           Specialized */
     , (43516, 34, 0, 3, 0, 265, 0, 0) /* WarMagic            Specialized */
     , (43516, 41, 0, 3, 0, 400, 0, 0) /* TwoHandedCombat     Specialized */
     , (43516, 45, 0, 3, 0, 420, 0, 0) /* LightWeapons        Specialized */
     , (43516, 46, 0, 3, 0, 420, 0, 0) /* FinesseWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (43516,  0,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (43516,  1,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (43516,  2,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (43516,  3,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (43516,  4,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (43516,  5,  4, 200,  0.5,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (43516,  6,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (43516,  7,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (43516,  8,  4, 200,  0.5,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (43516,  4312,   2.02)  /* Incantation of Imperil Other */
     , (43516,  4446,   2.02)  /* Incantation of Frost Blast */
     , (43516,  4447,   2.25)  /* Incantation of Frost Bolt */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (43516, 9, 43519,  0, 0, 1, False) /* Create Entryway Key (43519) for ContainTreasure */
     , (43516, 10, 43397,  0, 0, 1, False) /* Create Frost Greataxe (43397) for WieldTreasure */;
