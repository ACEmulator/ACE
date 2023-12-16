DELETE FROM `weenie` WHERE `class_Id` = 490061;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490061, 'ace490061-Virindiassassin', 10, '2022-12-28 05:57:21') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490061,   1,         16) /* ItemType - Creature */
     , (490061,   2,         19) /* CreatureType - Virindi */
     , (490061,   6,         -1) /* ItemsCapacity */
     , (490061,   7,         -1) /* ContainersCapacity */
     , (490061,  16,          1) /* ItemUseable - No */
     , (490061,  25,        300) /* Level */
	 , (490061,  72,         8) /* FriendType - Virindi */
     , (490061,  68,          3) /* TargetingTactic - Random, Focused */
     , (490061,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (490061, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (490061, 146,    4000000) /* XpOverride */
     , (490061, 307,         20) /* DamageRating */
     , (490061, 332,        180) /* LuminanceAward */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490061,   1, True ) /* Stuck */
     , (490061,   6, False) /* AiUsesMana */
     , (490061,  11, False) /* IgnoreCollisions */
     , (490061,  12, True ) /* ReportCollisions */
     , (490061,  13, False) /* Ethereal */
     , (490061,  14, True ) /* GravityStatus */
     , (490061,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490061,   1,       5) /* HeartbeatInterval */
     , (490061,   2,       0) /* HeartbeatTimestamp */
     , (490061,   3,     0.6) /* HealthRate */
     , (490061,   4,     0.5) /* StaminaRate */
     , (490061,   5,       2) /* ManaRate */
     , (490061,  12,       0) /* Shade */
     , (490061,  13,     0.9) /* ArmorModVsSlash */
     , (490061,  14,       1) /* ArmorModVsPierce */
     , (490061,  15,       1) /* ArmorModVsBludgeon */
     , (490061,  16,       1) /* ArmorModVsCold */
     , (490061,  17,     0.9) /* ArmorModVsFire */
     , (490061,  18,     0.9) /* ArmorModVsAcid */
     , (490061,  19,       1) /* ArmorModVsElectric */
     , (490061,  31,      40) /* VisualAwarenessRange */
	 , (490061,  39,     1.4) /* DefaultScale */
	 , (490061,  55,      110) /* HomeRadius */
     , (490061,  34,       1) /* PowerupTime */
     , (490061,  36,       1) /* ChargeSpeed */
     , (490061,  64,     0.7) /* ResistSlash */
     , (490061,  65,     0.6) /* ResistPierce */
     , (490061,  66,     0.6) /* ResistBludgeon */
     , (490061,  67,     0.7) /* ResistFire */
     , (490061,  68,     0.4) /* ResistCold */
     , (490061,  69,     0.7) /* ResistAcid */
     , (490061,  70,     0.4) /* ResistElectric */
     , (490061,  80,       3) /* AiUseMagicDelay */
     , (490061, 104,      40) /* ObviousRadarRange */
     , (490061, 122,       2) /* AiAcquireHealth */
     , (490061, 125,       1) /* ResistHealthDrain */
     , (490061, 165,       1) /* ArmorModVsNether */
     , (490061, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490061,   1, 'Virindi Assassin') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490061,   1, 0x0200173C) /* Setup */
     , (490061,   2, 0x090000F8) /* MotionTable */
     , (490061,   3, 0x20000012) /* SoundTable */
     , (490061,   4, 0x3000000D) /* CombatTable */
     , (490061,   6, 0x040009B2) /* PaletteBase */
     , (490061,   8, 0x06001227) /* Icon */
     , (490061,  22, 0x34000029) /* PhysicsEffectTable */
     , (490061,  35,       2110) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490061,   1, 350, 0, 0) /* Strength */
     , (490061,   2, 350, 0, 0) /* Endurance */
     , (490061,   3, 320, 0, 0) /* Quickness */
     , (490061,   4, 380, 0, 0) /* Coordination */
     , (490061,   5, 480, 0, 0) /* Focus */
     , (490061,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490061,   1,  95000, 0, 0, 100000) /* MaxHealth */
     , (490061,   3,  30000, 0, 0, 50) /* MaxStamina */
     , (490061,   5,  48000, 0, 0, 30) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490061,  6, 0, 2, 0, 550, 0, 0) /* MeleeDefense        Trained */
     , (490061,  7, 0, 2, 0, 440, 0, 0) /* MissileDefense      Trained */
     , (490061, 15, 0, 2, 0, 370, 0, 0) /* MagicDefense        Trained */
     , (490061, 16, 0, 2, 0, 430, 0, 0) /* ManaConversion      Trained */
     , (490061, 31, 0, 2, 0, 430, 0, 0) /* CreatureEnchantment Trained */
     , (490061, 33, 0, 2, 0, 430, 0, 0) /* LifeMagic           Trained */
     , (490061, 34, 0, 2, 0, 430, 0, 0) /* WarMagic            Trained */
     , (490061, 41, 0, 2, 0, 450, 0, 0) /* TwoHandedCombat     Trained */
     , (490061, 43, 0, 2, 0, 430, 0, 0) /* VoidMagic           Trained */
     , (490061, 44, 0, 2, 0, 450, 0, 0) /* HeavyWeapons        Trained */
     , (490061, 45, 0, 2, 0, 450, 0, 0) /* LightWeapons        Trained */
     , (490061, 46, 0, 2, 0, 450, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490061,  0, 64,  0,    0,  650,  585,  650,  650,  650,  585,  585,  650,  650, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (490061,  1, 64,  0,    0,  650,  585,  650,  650,  650,  585,  585,  650,  650, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (490061,  2, 64,  0,    0,  650,  585,  650,  650,  650,  585,  585,  650,  650, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (490061,  3, 64,  0,    0,  650,  585,  650,  650,  650,  585,  585,  650,  650, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (490061,  4, 64,  0,    0,  650,  585,  650,  650,  650,  585,  585,  650,  650, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (490061,  5, 64, 200,  0.5,  650,  585,  650,  650,  650,  585,  585,  650,  650, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (490061,  6, 64,  0,    0,  650,  585,  650,  650,  650,  585,  585,  650,  650, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (490061,  7, 64,  0,    0,  650,  585,  650,  650,  650,  585,  585,  650,  650, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (490061,  8, 64, 200,  0.5,  650,  585,  650,  650,  650,  585,  585,  650,  650, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490061,  3940,   2.21)  /* Exsanguinating Wave */
     , (490061,  3941,  2.266)  /* Heavy Lightning Ring */
     , (490061,  3989,  2.414)  /* Dark Lightning */
     , (490061,  4312,  2.206)  /* Incantation of Imperil Other */
     , (490061,  4483,  2.259)  /* Incantation of Lightning Vulnerability Other */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES  (490061, 9,     0,  0, 0, 0.97, False) /* Create nothing for ContainTreasure */;
