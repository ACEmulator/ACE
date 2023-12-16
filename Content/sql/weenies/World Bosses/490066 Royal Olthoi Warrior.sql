DELETE FROM `weenie` WHERE `class_Id` = 490066;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490066, 'ace490066-Royal Olthoi Warrior', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490066,   1,         16) /* ItemType - Creature */
     , (490066,   2,         1) /* CreatureType - ParadoxOlthoi */
     , (490066,   3,         39) /* PaletteTemplate - Black */
     , (490066,   6,         -1) /* ItemsCapacity */
     , (490066,   7,         -1) /* ContainersCapacity */
     , (490066,  16,          1) /* ItemUseable - No */
     , (490066,  25,        265) /* Level */
     , (490066,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (490066,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (490066, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (490066, 146,    2500000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490066,   1, True ) /* Stuck */
     , (490066,  65, True ) /* IgnoreMagicResist */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490066,   1,       5) /* HeartbeatInterval */
     , (490066,   2,       0) /* HeartbeatTimestamp */
     , (490066,   3,    0.65) /* HealthRate */
     , (490066,   4,       4) /* StaminaRate */
     , (490066,   5,       2) /* ManaRate */
     , (490066,  13,    2.25) /* ArmorModVsSlash */
     , (490066,  14,    1.75) /* ArmorModVsPierce */
     , (490066,  15,    1.12) /* ArmorModVsBludgeon */
     , (490066,  16,     3.5) /* ArmorModVsCold */
     , (490066,  17,     3.5) /* ArmorModVsFire */
     , (490066,  18,       4) /* ArmorModVsAcid */
     , (490066,  19,     3.5) /* ArmorModVsElectric */
     , (490066,  31,      24) /* VisualAwarenessRange */
     , (490066,  34,       1) /* PowerupTime */
     , (490066,  36,       1) /* ChargeSpeed */
     , (490066,  39,     1.3) /* DefaultScale */
	 , (490066,  55,      110) /* HomeRadius */
     , (490066,  64,       1) /* ResistSlash */
     , (490066,  65,     1.2) /* ResistPierce */
     , (490066,  66,     1.3) /* ResistBludgeon */
     , (490066,  67,    0.75) /* ResistFire */
     , (490066,  68,     0.5) /* ResistCold */
     , (490066,  69,     0.5) /* ResistAcid */
     , (490066,  70,    0.75) /* ResistElectric */
     , (490066,  77,       1) /* PhysicsScriptIntensity */
     , (490066, 104,      10) /* ObviousRadarRange */
     , (490066, 125,       1) /* ResistHealthDrain */
     , (490066, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490066,   1, 'Royal Olthoi Warrior') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490066,   1, 0x0200170A) /* Setup */
     , (490066,   2, 0x09000135) /* MotionTable */
     , (490066,   3, 0x200000A1) /* SoundTable */
     , (490066,   4, 0x30000039) /* CombatTable */
     , (490066,   6, 0x04001606) /* PaletteBase */
     , (490066,   7, 0x100004C7) /* ClothingBase */
     , (490066,   8, 0x06002D3E) /* Icon */
     , (490066,  19, 0x00000056) /* ActivationAnimation */
     , (490066,  22, 0x340000A8) /* PhysicsEffectTable */
     , (490066,  30,         86) /* PhysicsScript - BreatheAcid */
     , (490066,  35,         2111) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490066,   1, 400, 0, 0) /* Strength */
     , (490066,   2, 500, 0, 0) /* Endurance */
     , (490066,   3, 500, 0, 0) /* Quickness */
     , (490066,   4, 350, 0, 0) /* Coordination */
     , (490066,   5, 490, 0, 0) /* Focus */
     , (490066,   6, 490, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490066,   1, 15000, 0, 0, 15250) /* MaxHealth */
     , (490066,   3,  5000, 0, 0, 5500) /* MaxStamina */
     , (490066,   5,  5000, 0, 0, 5490) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490066,  6, 0, 2, 0, 460, 0, 0) /* MeleeDefense        Trained */
     , (490066,  7, 0, 2, 0, 420, 0, 0) /* MissileDefense      Trained */
     , (490066, 15, 0, 2, 0, 400, 0, 0) /* MagicDefense        Trained */
     , (490066, 41, 0, 2, 0, 519, 0, 0) /* TwoHandedCombat     Trained */
     , (490066, 44, 0, 2, 0, 519, 0, 0) /* HeavyWeapons        Trained */
     , (490066, 45, 0, 2, 0, 519, 0, 0) /* LightWeapons        Trained */
     , (490066, 46, 0, 2, 0, 519, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490066,  0,  2, 450, 0.75,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 1,  0.1,    0,    0,  0.1,    0,    0,  0.1,    0,    0,  0.1,    0,    0) /* Head */
     , (490066, 16,  4,  0,    0,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 2, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45) /* Torso */
     , (490066, 18,  1, 350,  0.5,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 2,    0,  0.2,  0.1,    0,  0.2,  0.1,    0,  0.2,  0.1,    0,  0.2,  0.1) /* Arm */
     , (490066, 19,  1,  0,    0,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 3,    0,  0.2, 0.45,    0,  0.2, 0.45,    0,  0.2, 0.45,    0,  0.2, 0.45) /* Leg */
     , (490066, 20,  1, 500, 0.75,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 2, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0) /* Claw */
     , (490066, 22, 32, 475,  0.5,  700, 1575, 1225,  784, 2450, 2450, 2800, 2450,    0, 0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0) /* Breath */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (490066, 9,     0,  1, 0, 0.05, False) /* Create nothing for ContainTreasure */
     , (490066, 9, 36376,  1, 0, 0.8, False) /* Create Small Olthoi Venom Sac (36376) for ContainTreasure */
     , (490066, 9,     0,  1, 0, 0.2, False) /* Create nothing for ContainTreasure */
     , (490066, 9, 36376,  1, 0, 0.8, False) /* Create Small Olthoi Venom Sac (36376) for ContainTreasure */
     , (490066, 9,     0,  1, 0, 0.2, False) /* Create nothing for ContainTreasure */;
