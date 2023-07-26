DELETE FROM `weenie` WHERE `class_Id` = 43396;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (43396, 'ace43396-gurogsoldier', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (43396,   1,         16) /* ItemType - Creature */
     , (43396,   2,        100) /* CreatureType - Gurog */
     , (43396,   6,         -1) /* ItemsCapacity */
     , (43396,   7,         -1) /* ContainersCapacity */
     , (43396,  16,          1) /* ItemUseable - No */
     , (43396,  25,        220) /* Level */
     , (43396,  27,          0) /* ArmorType - None */
     , (43396,  68,          5) /* TargetingTactic - Random, LastDamager */
     , (43396,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (43396, 101,          2) /* AiAllowedCombatStyle - OneHanded */
     , (43396, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (43396, 146,    1400000) /* XpOverride */
     , (43396, 332,         70) /* LuminanceAward */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (43396,   1, True ) /* Stuck */
     , (43396,   6, False) /* AiUsesMana */
     , (43396,  11, False) /* IgnoreCollisions */
     , (43396,  12, True ) /* ReportCollisions */
     , (43396,  13, False) /* Ethereal */
     , (43396,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (43396,   1,       5) /* HeartbeatInterval */
     , (43396,   2,       0) /* HeartbeatTimestamp */
     , (43396,   3,     0.8) /* HealthRate */
     , (43396,   4,     0.8) /* StaminaRate */
     , (43396,   5,       2) /* ManaRate */
     , (43396,  12,       0) /* Shade */
     , (43396,  13,       1) /* ArmorModVsSlash */
     , (43396,  14,    0.55) /* ArmorModVsPierce */
     , (43396,  15,       1) /* ArmorModVsBludgeon */
     , (43396,  16,       1) /* ArmorModVsCold */
     , (43396,  17,    0.55) /* ArmorModVsFire */
     , (43396,  18,       1) /* ArmorModVsAcid */
     , (43396,  19,       1) /* ArmorModVsElectric */
     , (43396,  31,      16) /* VisualAwarenessRange */
     , (43396,  34,       1) /* PowerupTime */
     , (43396,  36,       1) /* ChargeSpeed */
     , (43396,  39,     1.3) /* DefaultScale */
     , (43396,  64,     0.3) /* ResistSlash */
     , (43396,  65,     0.8) /* ResistPierce */
     , (43396,  66,     0.3) /* ResistBludgeon */
     , (43396,  67,     0.8) /* ResistFire */
     , (43396,  68,     0.3) /* ResistCold */
     , (43396,  69,     0.3) /* ResistAcid */
     , (43396,  70,     0.4) /* ResistElectric */
     , (43396,  71,       1) /* ResistHealthBoost */
     , (43396,  72,       1) /* ResistStaminaDrain */
     , (43396,  73,       1) /* ResistStaminaBoost */
     , (43396,  74,       1) /* ResistManaDrain */
     , (43396,  75,       1) /* ResistManaBoost */
     , (43396,  80,       1) /* AiUseMagicDelay */
     , (43396, 104,      10) /* ObviousRadarRange */
     , (43396, 122,       2) /* AiAcquireHealth */
     , (43396, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (43396,   1, 'Gurog Soldier') /* Name */
     , (43396,  45, 'KillTaskGurogSoldier1110') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (43396,   1, 0x02001A2C) /* Setup */
     , (43396,   2, 0x090001A8) /* MotionTable */
     , (43396,   3, 0x200000D5) /* SoundTable */
     , (43396,   4, 0x30000000) /* CombatTable */
     , (43396,   8, 0x06002B2E) /* Icon */
     , (43396,  22, 0x340000CD) /* PhysicsEffectTable */
     , (43396,  35,       2000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (43396,   1, 560, 0, 0) /* Strength */
     , (43396,   2, 450, 0, 0) /* Endurance */
     , (43396,   3, 450, 0, 0) /* Quickness */
     , (43396,   4, 460, 0, 0) /* Coordination */
     , (43396,   5, 450, 0, 0) /* Focus */
     , (43396,   6, 450, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (43396,   1,  1655, 0, 0, 2000) /* MaxHealth */
     , (43396,   3,  3500, 0, 0, 3950) /* MaxStamina */
     , (43396,   5,    10, 0, 0, 450) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (43396,  6, 0, 3, 0, 470, 0, 0) /* MeleeDefense        Specialized */
     , (43396,  7, 0, 3, 0, 420, 0, 0) /* MissileDefense      Specialized */
     , (43396, 15, 0, 3, 0, 320, 0, 0) /* MagicDefense        Specialized */
     , (43396, 20, 0, 3, 0, 420, 0, 0) /* Deception           Specialized */
     , (43396, 33, 0, 3, 0, 265, 0, 0) /* LifeMagic           Specialized */
     , (43396, 34, 0, 3, 0, 265, 0, 0) /* WarMagic            Specialized */
     , (43396, 41, 0, 3, 0, 400, 0, 0) /* TwoHandedCombat     Specialized */
     , (43396, 45, 0, 3, 0, 420, 0, 0) /* LightWeapons        Specialized */
     , (43396, 46, 0, 3, 0, 420, 0, 0) /* FinesseWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (43396,  0,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (43396,  1,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (43396,  2,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (43396,  3,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (43396,  4,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (43396,  5,  4, 200,  0.5,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (43396,  6,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (43396,  7,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (43396,  8,  4, 200,  0.5,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (43396,  4312,   2.02)  /* Incantation of Imperil Other */
     , (43396,  4446,   2.02)  /* Incantation of Frost Blast */
     , (43396,  4447,   2.25)  /* Incantation of Frost Bolt */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (43396, 9, 48908,  1, 0, 0.02, False) /* Create Shattered Legendary Key (48908) for ContainTreasure */
     , (43396, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (43396, 9, 44864,  0, 0, 0.02, False) /* Create Gurog Arm (44864) for ContainTreasure */
     , (43396, 9, 44868,  0, 0, 0.02, False) /* Create Gurog Torso with a Head (44868) for ContainTreasure */
     , (43396, 9, 44870,  0, 0, 0.02, False) /* Create Gurog Leg (44870) for ContainTreasure */
     , (43396, 9,     0,  0, 0, 0.94, False) /* Create nothing for ContainTreasure */
     , (43396, 9, 51370,  1, 0, 0.05, False) /* Create Frozen Fortress Testing Grounds Attunement Shard (Level 180+) (51370) for ContainTreasure */
     , (43396, 9, 51341,  1, 0, 0.05, False) /* Create Frozen Fortress Laboratory Attunement Shard (Level 180+) (51341) for ContainTreasure */
     , (43396, 9,     0,  0, 0, 0.9, False) /* Create nothing for ContainTreasure */
     , (43396, 10, 43397,  0, 0, 1, False) /* Create Frost Greataxe (43397) for WieldTreasure */;
