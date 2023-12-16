DELETE FROM `weenie` WHERE `class_Id` = 490062;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490062, 'ace490062-simulacrumAssistant', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490062,   1,         16) /* ItemType - Creature */
     , (490062,   2,         59) /* CreatureType - Simulacrum */
     , (490062,   6,         -1) /* ItemsCapacity */
     , (490062,   7,         -1) /* ContainersCapacity */
     , (490062,  16,          1) /* ItemUseable - No */
     , (490062,  25,        265) /* Level */
	 , (490062,  72,         92) /* FriendType - Virindi */
     , (490062,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (490062,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (490062, 113,          1) /* Gender - Male */
     , (490062, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (490062, 146,    2500000) /* XpOverride */
     , (490062, 188,          2) /* HeritageGroup - Gharundim */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490062,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490062,   1,       5) /* HeartbeatInterval */
     , (490062,   2,       0) /* HeartbeatTimestamp */
     , (490062,   3,       2) /* HealthRate */
     , (490062,   4,       5) /* StaminaRate */
     , (490062,   5,       1) /* ManaRate */
     , (490062,  13,     0.8) /* ArmorModVsSlash */
     , (490062,  14,       1) /* ArmorModVsPierce */
     , (490062,  15,     0.8) /* ArmorModVsBludgeon */
     , (490062,  16,       1) /* ArmorModVsCold */
     , (490062,  17,       1) /* ArmorModVsFire */
     , (490062,  18,       1) /* ArmorModVsAcid */
     , (490062,  19,       1) /* ArmorModVsElectric */
     , (490062,  31,      22) /* VisualAwarenessRange */
     , (490062,  41,      30) /* RegenerationInterval */
     , (490062,  43,       4) /* GeneratorRadius */
	 , (490062,  55,      110) /* HomeRadius */
     , (490062,  64,       1) /* ResistSlash */
     , (490062,  65,       1) /* ResistPierce */
     , (490062,  66,       1) /* ResistBludgeon */
     , (490062,  67,     0.8) /* ResistFire */
     , (490062,  68,     0.8) /* ResistCold */
     , (490062,  69,     0.8) /* ResistAcid */
     , (490062,  70,     0.8) /* ResistElectric */
     , (490062,  80,       3) /* AiUseMagicDelay */
     , (490062, 104,      10) /* ObviousRadarRange */
     , (490062, 122,       2) /* AiAcquireHealth */
     , (490062, 125,       1) /* ResistHealthDrain */
     , (490062, 165,       1) /* ArmorModVsNether */
     , (490062, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490062,   1, 'Simulacrum Assistant') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490062,   1, 0x02000001) /* Setup */
     , (490062,   2, 0x090000C5) /* MotionTable */
     , (490062,   3, 0x20000083) /* SoundTable */
     , (490062,   4, 0x30000000) /* CombatTable */
     , (490062,   6, 0x0400007E) /* PaletteBase */
     , (490062,   8, 0x06001036) /* Icon */
     , (490062,  22, 0x34000095) /* PhysicsEffectTable */
     , (490062,  35,       1000) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490062,   1, 340, 0, 0) /* Strength */
     , (490062,   2, 340, 0, 0) /* Endurance */
     , (490062,   3, 320, 0, 0) /* Quickness */
     , (490062,   4, 365, 0, 0) /* Coordination */
     , (490062,   5, 440, 0, 0) /* Focus */
     , (490062,   6, 440, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490062,   1, 19165, 0, 0, 19335) /* MaxHealth */
     , (490062,   3,  4660, 0, 0, 5000) /* MaxStamina */
     , (490062,   5,  4560, 0, 0, 5000) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490062,  6, 0, 2, 0, 500, 0, 0) /* MeleeDefense        Trained */
     , (490062,  7, 0, 2, 0, 420, 0, 0) /* MissileDefense      Trained */
     , (490062, 15, 0, 2, 0, 320, 0, 0) /* MagicDefense        Trained */
     , (490062, 16, 0, 2, 0, 440, 0, 0) /* ManaConversion      Trained */
     , (490062, 31, 0, 2, 0, 440, 0, 0) /* CreatureEnchantment Trained */
     , (490062, 33, 0, 2, 0, 440, 0, 0) /* LifeMagic           Trained */
     , (490062, 34, 0, 2, 0, 440, 0, 0) /* WarMagic            Trained */
     , (490062, 41, 0, 2, 0, 520, 0, 0) /* TwoHandedCombat     Trained */
     , (490062, 43, 0, 2, 0, 440, 0, 0) /* VoidMagic           Trained */
     , (490062, 44, 0, 2, 0, 520, 0, 0) /* HeavyWeapons        Trained */
     , (490062, 45, 0, 2, 0, 520, 0, 0) /* LightWeapons        Trained */
     , (490062, 46, 0, 2, 0, 520, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490062,  0,  4,  0,    0,  270,  216,  270,  216,  270,  270,  270,  270,  270, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (490062,  1,  4,  0,    0,  260,  208,  260,  208,  260,  260,  260,  260,  260, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (490062,  2,  4,  0,    0,  260,  208,  260,  208,  260,  260,  260,  260,  260, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (490062,  3,  4,  0,    0,  250,  200,  250,  200,  250,  250,  250,  250,  250, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (490062,  4,  4,  0,    0,  250,  200,  250,  200,  250,  250,  250,  250,  250, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (490062,  5,  4,  2, 0.75,  250,  200,  250,  200,  250,  250,  250,  250,  250, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (490062,  6,  4,  0,    0,  250,  200,  250,  200,  250,  250,  250,  250,  250, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (490062,  7,  4,  0,    0,  250,  200,  250,  200,  250,  250,  250,  250,  250, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (490062,  8,  4,  2, 0.75,  250,  200,  250,  200,  250,  250,  250,  250,  250, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490062,  4294,   2.06)  /* Incantation of Clumsiness Other */
     , (490062,  4302,  2.064)  /* Incantation of Feeblemind Other */
     , (490062,  4439,  2.114)  /* Incantation of Flame Bolt */
     , (490062,  4451,  2.128)  /* Incantation of Lightning Bolt */
     , (490062,  4457,  2.147)  /* Incantation of Whirling Blade */
     , (490062,  4597,  2.103)  /* Incantation of Magic Yield Other */
     , (490062,  2151,  2.058)  /* Blessing of the Blade Turner */
     , (490062,  2153,  2.061)  /* Blessing of the Mace Turner */
     , (490062,  2161,  2.065)  /* Blessing of the Arrow Turner */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (490062, 2, 49612,  1, 0, 0, False) /* Create Sickle (49612) for Wield */
     , (490062, 10,  5853,  1, 3, 0, False) /* Create Dho Vest and Robe (5853) for WieldTreasure */;
