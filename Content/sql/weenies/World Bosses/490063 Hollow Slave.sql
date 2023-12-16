DELETE FROM `weenie` WHERE `class_Id` = 490063;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490063, 'ace490063-hollowslavewb', 10, '2022-12-28 05:57:21') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490063,   1,         16) /* ItemType - Creature */
     , (490063,   2,         48) /* CreatureType - HollowMinion */
     , (490063,   3,          2) /* PaletteTemplate - Blue */
     , (490063,   6,         -1) /* ItemsCapacity */
     , (490063,   7,         -1) /* ContainersCapacity */
     , (490063,  16,          1) /* ItemUseable - No */
     , (490063,  72,         65) /* FriendType - Virindi */
     , (490063,  25,        300) /* Level */
     , (490063,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (490063, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (490063, 146,    3000000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490063,   1, True ) /* Stuck */
     , (490063,  65, True ) /* IgnoreMagicResist */
     , (490063,  66, True ) /* IgnoreMagicArmor */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490063,   1,       5) /* HeartbeatInterval */
     , (490063,   2,       0) /* HeartbeatTimestamp */
     , (490063,   3,     3.5) /* HealthRate */
     , (490063,   4,     8.5) /* StaminaRate */
     , (490063,   5,       1) /* ManaRate */
     , (490063,  12,     0.5) /* Shade */
     , (490063,  13,     0.9) /* ArmorModVsSlash */
     , (490063,  14,     0.9) /* ArmorModVsPierce */
     , (490063,  15,       1) /* ArmorModVsBludgeon */
     , (490063,  16,     0.8) /* ArmorModVsCold */
     , (490063,  17,       1) /* ArmorModVsFire */
     , (490063,  18,       1) /* ArmorModVsAcid */
     , (490063,  19,       1) /* ArmorModVsElectric */
     , (490063,  31,      40) /* VisualAwarenessRange */
	 , (490063,  55,      110) /* HomeRadius */
     , (490063,  34,       1) /* PowerupTime */
     , (490063,  36,       1) /* ChargeSpeed */
     , (490063,  39,     1.1) /* DefaultScale */
     , (490063,  64,     0.5) /* ResistSlash */
     , (490063,  65,     0.5) /* ResistPierce */
     , (490063,  66,    0.33) /* ResistBludgeon */
     , (490063,  67,    0.25) /* ResistFire */
     , (490063,  68,    0.67) /* ResistCold */
     , (490063,  69,     0.5) /* ResistAcid */
     , (490063,  70,    0.25) /* ResistElectric */
     , (490063,  71,       1) /* ResistHealthBoost */
     , (490063,  72,       1) /* ResistStaminaDrain */
     , (490063,  73,       1) /* ResistStaminaBoost */
     , (490063,  74,       1) /* ResistManaDrain */
     , (490063,  75,       1) /* ResistManaBoost */
     , (490063, 104,      30) /* ObviousRadarRange */
     , (490063, 125,       1) /* ResistHealthDrain */
     , (490063, 165,       1) /* ArmorModVsNether */
     , (490063, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490063,   1, 'Hollow Servitor') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490063,   1, 0x02000938) /* Setup */
     , (490063,   2, 0x0900009D) /* MotionTable */
     , (490063,   3, 0x20000065) /* SoundTable */
     , (490063,   4, 0x3000002D) /* CombatTable */
     , (490063,   6, 0x04001007) /* PaletteBase */
     , (490063,   7, 0x10000489) /* ClothingBase */
     , (490063,   8, 0x06001EA4) /* Icon */
     , (490063,  22, 0x34000087) /* PhysicsEffectTable */
     , (490063,  35,       2111) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490063,   1, 600, 0, 0) /* Strength */
     , (490063,   2, 500, 0, 0) /* Endurance */
     , (490063,   3, 400, 0, 0) /* Quickness */
     , (490063,   4, 400, 0, 0) /* Coordination */
     , (490063,   5, 300, 0, 0) /* Focus */
     , (490063,   6, 300, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490063,   1,  9750, 0, 0, 10000) /* MaxHealth */
     , (490063,   3,  4500, 0, 0, 5000) /* MaxStamina */
     , (490063,   5,   200, 0, 0, 500) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490063,  6, 0, 2, 0, 500, 0, 0) /* MeleeDefense        Trained */
     , (490063,  7, 0, 2, 0, 440, 0, 0) /* MissileDefense      Trained */
     , (490063, 15, 0, 2, 0, 340, 0, 0) /* MagicDefense        Trained */
     , (490063, 41, 0, 2, 0, 420, 0, 0) /* TwoHandedCombat     Trained */
     , (490063, 44, 0, 2, 0, 420, 0, 0) /* HeavyWeapons        Trained */
     , (490063, 45, 0, 2, 0, 420, 0, 0) /* LightWeapons        Trained */
     , (490063, 46, 0, 2, 0, 420, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490063,  0,  4,  0,    0,  290,  261,  261,  290,  232,  290,  290,  290,  290, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (490063,  1,  4,  0,    0,  290,  261,  261,  290,  232,  290,  290,  290,  290, 2, 0.44, 0.23,    0, 0.44, 0.23,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (490063,  2,  4,  0,    0,  290,  261,  261,  290,  232,  290,  290,  290,  290, 3,    0, 0.23,  0.1,    0, 0.23,  0.2,    0, 0.17, 0.45,    0, 0.17, 0.45) /* Abdomen */
     , (490063,  3,  4,  0,    0,  290,  261,  261,  290,  232,  290,  290,  290,  290, 1, 0.23, 0.04,  0.2, 0.23, 0.04,  0.1, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (490063,  4,  4,  0,    0,  290,  261,  261,  290,  232,  290,  290,  290,  290, 2,    0,  0.3,  0.3,    0,  0.3,  0.4,    0,  0.3,  0.1,    0,  0.3,  0.1) /* LowerArm */
     , (490063,  5,  4, 40, 0.75,  290,  261,  261,  290,  232,  290,  290,  290,  290, 2,    0,  0.2,  0.3,    0,  0.2,  0.2,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (490063, 17,  4,  0,    0,  290,  261,  261,  290,  232,  290,  290,  290,  290, 3,    0,    0,  0.1,    0,    0,  0.1,    0, 0.13, 0.45,    0, 0.13, 0.45) /* Tail */;

