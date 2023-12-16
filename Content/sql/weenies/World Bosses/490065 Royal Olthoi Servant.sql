DELETE FROM `weenie` WHERE `class_Id` = 490065;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490065, 'ace490065-Royal Olthoi Servant', 10, '2023-11-17 05:21:40') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490065,   1,         16) /* ItemType - Creature */
     , (490065,   2,          1) /* CreatureType - Olthoi */
     , (490065,   3,         29) /* PaletteTemplate - DarkRedMetal */
     , (490065,   6,         -1) /* ItemsCapacity */
     , (490065,   7,         -1) /* ContainersCapacity */
     , (490065,  16,          1) /* ItemUseable - No */
     , (490065,  25,        240) /* Level */
     , (490065,  27,          0) /* ArmorType - None */
     , (490065,  40,          2) /* CombatMode - Melee */
     , (490065,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (490065,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (490065, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (490065, 140,          1) /* AiOptions - CanOpenDoors */
     , (490065, 146,    2500000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490065,   1, True ) /* Stuck */
     , (490065,  11, False) /* IgnoreCollisions */
     , (490065,  12, True ) /* ReportCollisions */
     , (490065,  13, False) /* Ethereal */
     , (490065,  14, True ) /* GravityStatus */
     , (490065,  19, True ) /* Attackable */
     , (490065,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490065,   1,       5) /* HeartbeatInterval */
     , (490065,   2,       0) /* HeartbeatTimestamp */
     , (490065,   3,     100) /* HealthRate */
     , (490065,   4,       4) /* StaminaRate */
     , (490065,   5,       2) /* ManaRate */
     , (490065,  13,       1) /* ArmorModVsSlash */
     , (490065,  14,     0.7) /* ArmorModVsPierce */
     , (490065,  15,     0.6) /* ArmorModVsBludgeon */
     , (490065,  16,     0.6) /* ArmorModVsCold */
     , (490065,  17,       1) /* ArmorModVsFire */
     , (490065,  18,       1) /* ArmorModVsAcid */
     , (490065,  19,       1) /* ArmorModVsElectric */
     , (490065,  31,      50) /* VisualAwarenessRange */
     , (490065,  34,       1) /* PowerupTime */
     , (490065,  36,       1) /* ChargeSpeed */
     , (490065,  39,     0.7) /* DefaultScale */
	 , (490065,  55,      110) /* HomeRadius */
     , (490065,  64,     0.4) /* ResistSlash */
     , (490065,  65,    0.65) /* ResistPierce */
     , (490065,  66,     0.7) /* ResistBludgeon */
     , (490065,  67,     0.4) /* ResistFire */
     , (490065,  68,     0.7) /* ResistCold */
     , (490065,  69,     0.4) /* ResistAcid */
     , (490065,  70,     0.4) /* ResistElectric */
     , (490065,  77,       1) /* PhysicsScriptIntensity */
     , (490065, 104,      40) /* ObviousRadarRange */
     , (490065, 125,       1) /* ResistHealthDrain */
     , (490065, 151,       0.5) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490065,   1, 'Royal Olthoi Servant') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490065,   1, 0x02000FB3) /* Setup */
     , (490065,   2, 0x09000135) /* MotionTable */
     , (490065,   3, 0x200000A1) /* SoundTable */
     , (490065,   4, 0x30000039) /* CombatTable */
     , (490065,   6, 0x04001606) /* PaletteBase */
     , (490065,   7, 0x100004C7) /* ClothingBase */
     , (490065,   8, 0x06002D3E) /* Icon */
     , (490065,  19, 0x00000057) /* ActivationAnimation */
     , (490065,  22, 0x340000A8) /* PhysicsEffectTable */
     , (490065,  30,         87) /* PhysicsScript - BreatheLightning */
     , (490065,  35,       1000) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490065,   1, 360, 0, 0) /* Strength */
     , (490065,   2, 380, 0, 0) /* Endurance */
     , (490065,   3, 380, 0, 0) /* Quickness */
     , (490065,   4, 310, 0, 0) /* Coordination */
     , (490065,   5, 480, 0, 0) /* Focus */
     , (490065,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490065,   1,  6210, 0, 0, 6400) /* MaxHealth */
     , (490065,   3,  5000, 0, 0, 5380) /* MaxStamina */
     , (490065,   5,  1350, 0, 0, 1830) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490065,  6, 0, 2, 0, 480, 0, 0) /* MeleeDefense        Trained */
     , (490065,  7, 0, 2, 0, 460, 0, 0) /* MissileDefense      Trained */
     , (490065, 15, 0, 2, 0, 380, 0, 0) /* MagicDefense        Trained */
     , (490065, 41, 0, 2, 0, 570, 0, 0) /* TwoHandedCombat     Trained */
     , (490065, 44, 0, 2, 0, 570, 0, 0) /* HeavyWeapons        Trained */
     , (490065, 45, 0, 2, 0, 570, 0, 0) /* LightWeapons        Trained */
     , (490065, 46, 0, 2, 0, 570, 0, 0) /* FinesseWeapons      Trained */
     , (490065, 51, 0, 2, 0, 570, 0, 0) /* SneakAttack         Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490065,  0,  2, 450, 0.75,  700,  700,  490,  420,  420,  700,  700,  700,    0, 1,  0.1,    0,    0,  0.1,    0,    0,  0.1,    0,    0,  0.1,    0,    0) /* Head */
     , (490065, 16,  4,  0,    0,  700,  700,  490,  420,  420,  700,  700,  700,    0, 2, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45) /* Torso */
     , (490065, 18,  1, 350,  0.5,  700,  700,  490,  420,  420,  700,  700,  700,    0, 2,    0,  0.2,  0.1,    0,  0.2,  0.1,    0,  0.2,  0.1,    0,  0.2,  0.1) /* Arm */
     , (490065, 19,  1,  0,    0,  700,  700,  490,  420,  420,  700,  700,  700,    0, 3,    0,  0.2, 0.45,    0,  0.2, 0.45,    0,  0.2, 0.45,    0,  0.2, 0.45) /* Leg */
     , (490065, 20,  1, 500, 0.75,  700,  700,  490,  420,  420,  700,  700,  700,    0, 2, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0) /* Claw */
     , (490065, 22, 64, 520,  0.5,  700,  700,  490,  420,  420,  700,  700,  700,    0, 0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0) /* Breath */;
