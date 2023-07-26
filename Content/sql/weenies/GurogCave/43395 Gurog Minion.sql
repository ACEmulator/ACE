DELETE FROM `weenie` WHERE `class_Id` = 43395;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (43395, 'ace43395-gurogminion', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (43395,   1,         16) /* ItemType - Creature */
     , (43395,   2,        100) /* CreatureType - Gurog */
     , (43395,   6,         -1) /* ItemsCapacity */
     , (43395,   7,         -1) /* ContainersCapacity */
     , (43395,  16,          1) /* ItemUseable - No */
     , (43395,  25,        200) /* Level */
     , (43395,  27,          0) /* ArmorType - None */
     , (43395,  68,          5) /* TargetingTactic - Random, LastDamager */
     , (43395,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (43395, 101,          2) /* AiAllowedCombatStyle - OneHanded */
     , (43395, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (43395, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (43395,   1, True ) /* Stuck */
     , (43395,   6, True ) /* AiUsesMana */
     , (43395,  11, False) /* IgnoreCollisions */
     , (43395,  12, True ) /* ReportCollisions */
     , (43395,  13, False) /* Ethereal */
     , (43395,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (43395,   1,       5) /* HeartbeatInterval */
     , (43395,   2,       0) /* HeartbeatTimestamp */
     , (43395,   3,     0.2) /* HealthRate */
     , (43395,   4,     0.5) /* StaminaRate */
     , (43395,   5,       2) /* ManaRate */
     , (43395,  12,       0) /* Shade */
     , (43395,  13,       1) /* ArmorModVsSlash */
     , (43395,  14,    0.55) /* ArmorModVsPierce */
     , (43395,  15,       1) /* ArmorModVsBludgeon */
     , (43395,  16,       1) /* ArmorModVsCold */
     , (43395,  17,    0.55) /* ArmorModVsFire */
     , (43395,  18,       1) /* ArmorModVsAcid */
     , (43395,  19,       1) /* ArmorModVsElectric */
     , (43395,  31,      16) /* VisualAwarenessRange */
     , (43395,  34,       1) /* PowerupTime */
     , (43395,  36,       1) /* ChargeSpeed */
     , (43395,  39,     1.3) /* DefaultScale */
     , (43395,  64,     0.3) /* ResistSlash */
     , (43395,  65,     0.8) /* ResistPierce */
     , (43395,  66,     0.3) /* ResistBludgeon */
     , (43395,  67,     0.8) /* ResistFire */
     , (43395,  68,     0.3) /* ResistCold */
     , (43395,  69,     0.3) /* ResistAcid */
     , (43395,  70,     0.4) /* ResistElectric */
     , (43395,  71,       1) /* ResistHealthBoost */
     , (43395,  72,       1) /* ResistStaminaDrain */
     , (43395,  73,       1) /* ResistStaminaBoost */
     , (43395,  74,       1) /* ResistManaDrain */
     , (43395,  75,       1) /* ResistManaBoost */
     , (43395,  80,       4) /* AiUseMagicDelay */
     , (43395, 104,      10) /* ObviousRadarRange */
     , (43395, 122,       2) /* AiAcquireHealth */
     , (43395, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (43395,   1, 'Gurog Minion') /* Name */
     , (43395,  45, 'KillTaskGurogMinion1110') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (43395,   1, 0x02001A2B) /* Setup */
     , (43395,   2, 0x090001A8) /* MotionTable */
     , (43395,   3, 0x200000D5) /* SoundTable */
     , (43395,   4, 0x30000000) /* CombatTable */
     , (43395,   8, 0x06002B2E) /* Icon */
     , (43395,  22, 0x340000CD) /* PhysicsEffectTable */
     , (43395,  35,       2000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (43395,   1, 530, 0, 0) /* Strength */
     , (43395,   2, 480, 0, 0) /* Endurance */
     , (43395,   3, 370, 0, 0) /* Quickness */
     , (43395,   4, 510, 0, 0) /* Coordination */
     , (43395,   5, 400, 0, 0) /* Focus */
     , (43395,   6, 400, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (43395,   1,  1510, 0, 0, 1750) /* MaxHealth */
     , (43395,   3,  3420, 0, 0, 3980) /* MaxStamina */
     , (43395,   5,  1000, 0, 0, 1400) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (43395,  6, 0, 3, 0, 380, 0, 0) /* MeleeDefense        Specialized */
     , (43395,  7, 0, 3, 0, 377, 0, 0) /* MissileDefense      Specialized */
     , (43395, 15, 0, 3, 0, 310, 0, 0) /* MagicDefense        Specialized */
     , (43395, 20, 0, 3, 0, 120, 0, 0) /* Deception           Specialized */
     , (43395, 33, 0, 3, 0, 260, 0, 0) /* LifeMagic           Specialized */
     , (43395, 34, 0, 3, 0, 260, 0, 0) /* WarMagic            Specialized */
     , (43395, 44, 0, 3, 0, 345, 0, 0) /* HeavyWeapons        Specialized */
     , (43395, 45, 0, 3, 0, 345, 0, 0) /* LightWeapons        Specialized */
     , (43395, 46, 0, 3, 0, 345, 0, 0) /* FinesseWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (43395,  0,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (43395,  1,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (43395,  2,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (43395,  3,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (43395,  4,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (43395,  5,  4, 150,  0.5,  500,  500,  275,  500,  500,  275,  500,  500,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (43395,  6,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (43395,  7,  4,  0,    0,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (43395,  8,  4, 150,  0.5,  500,  500,  275,  500,  500,  275,  500,  500,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (43395,  4446,   2.02)  /* Incantation of Frost Blast */
     , (43395,  4447,   2.02)  /* Incantation of Frost Bolt */
     , (43395,  4477,   2.02)  /* Incantation of Bludgeoning Vulnerability Other */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (43395, 9, 48908,  1, 0, 0.02, False) /* Create Shattered Legendary Key (48908) for ContainTreasure */
     , (43395, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (43395, 9, 44864,  0, 0, 0.02, False) /* Create Gurog Arm (44864) for ContainTreasure */
     , (43395, 9, 44868,  0, 0, 0.02, False) /* Create Gurog Torso with a Head (44868) for ContainTreasure */
     , (43395, 9, 44870,  0, 0, 0.02, False) /* Create Gurog Leg (44870) for ContainTreasure */
     , (43395, 9,     0,  0, 0, 0.94, False) /* Create nothing for ContainTreasure */
     , (43395, 9, 51370,  1, 0, 0.05, False) /* Create Frozen Fortress Testing Grounds Attunement Shard (Level 180+) (51370) for ContainTreasure */
     , (43395, 9, 51341,  1, 0, 0.05, False) /* Create Frozen Fortress Laboratory Attunement Shard (Level 180+) (51341) for ContainTreasure */
     , (43395, 9,     0,  0, 0, 0.9, False) /* Create nothing for ContainTreasure */;
