DELETE FROM `weenie` WHERE `class_Id` = 490035;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490035, 'ace490035-shadowworshiper', 10, '2023-05-15 03:25:02') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490035,   1,         16) /* ItemType - Creature */
     , (490035,   2,         22) /* CreatureType - Shadow */
     , (490035,   3,         39) /* PaletteTemplate - Black */
     , (490035,   6,         -1) /* ItemsCapacity */
     , (490035,   7,         -1) /* ContainersCapacity */
     , (490035,   8,         90) /* Mass */
     , (490035,  16,          1) /* ItemUseable - No */
     , (490035,  25,        240) /* Level */
     , (490035,  27,          0) /* ArmorType - None */
	 , (490035,  72,         52) /* FriendType - Virindi */
     , (490035,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (490035,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (490035, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (490035, 113,          1) /* Gender - Male */
     , (490035, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (490035, 140,          1) /* AiOptions - CanOpenDoors */
     , (490035, 146,    1850000) /* XpOverride */
     , (490035, 188,          1) /* HeritageGroup - Aluvian */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490035,   1, True ) /* Stuck */
     , (490035,   6, True ) /* AiUsesMana */
     , (490035,  11, False) /* IgnoreCollisions */
     , (490035,  12, True ) /* ReportCollisions */
     , (490035,  13, False) /* Ethereal */
     , (490035,  14, True ) /* GravityStatus */
     , (490035,  19, True ) /* Attackable */
     , (490035,  42, True ) /* AllowEdgeSlide */
     , (490035,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490035,   1,       5) /* HeartbeatInterval */
     , (490035,   2,       0) /* HeartbeatTimestamp */
     , (490035,   3,     0.4) /* HealthRate */
     , (490035,   4,     2.5) /* StaminaRate */
     , (490035,   5,       1) /* ManaRate */
     , (490035,  12,     0.1) /* Shade */
     , (490035,  13,       1) /* ArmorModVsSlash */
     , (490035,  14,    0.61) /* ArmorModVsPierce */
     , (490035,  15,    0.74) /* ArmorModVsBludgeon */
     , (490035,  16,     0.3) /* ArmorModVsCold */
     , (490035,  17,       1) /* ArmorModVsFire */
     , (490035,  18,    0.38) /* ArmorModVsAcid */
     , (490035,  19,    0.61) /* ArmorModVsElectric */
     , (490035,  31,      50) /* VisualAwarenessRange */
	 , (490035,  55,      110) /* HomeRadius */
     , (490035,  34,     0.9) /* PowerupTime */
     , (490035,  36,       1) /* ChargeSpeed */
     , (490035,  39,    0.84) /* DefaultScale */
     , (490035,  64,       1) /* ResistSlash */
     , (490035,  65,     0.5) /* ResistPierce */
     , (490035,  66,    0.67) /* ResistBludgeon */
     , (490035,  67,       1) /* ResistFire */
     , (490035,  68,     0.1) /* ResistCold */
     , (490035,  69,     0.2) /* ResistAcid */
     , (490035,  70,     0.5) /* ResistElectric */
     , (490035,  71,       1) /* ResistHealthBoost */
     , (490035,  72,       1) /* ResistStaminaDrain */
     , (490035,  73,       1) /* ResistStaminaBoost */
     , (490035,  74,       1) /* ResistManaDrain */
     , (490035,  75,       1) /* ResistManaBoost */
     , (490035,  76,     0.5) /* Translucency */
     , (490035,  80,     3.2) /* AiUseMagicDelay */
     , (490035, 104,      404) /* ObviousRadarRange */
     , (490035, 122,       5) /* AiAcquireHealth */
     , (490035, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490035,   1, 'Shadow Worshiper') /* Name */
     , (490035,   3, 'Male') /* Sex */
     , (490035,   4, 'Aluvian') /* HeritageGroup */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490035,   1, 0x02000001) /* Setup */
     , (490035,   2, 0x09000001) /* MotionTable */
     , (490035,   3, 0x200000B2) /* SoundTable */
     , (490035,   4, 0x30000000) /* CombatTable */
     , (490035,   6, 0x0400007E) /* PaletteBase */
     , (490035,   7, 0x100000B0) /* ClothingBase */
     , (490035,   8, 0x06001BBD) /* Icon */
     , (490035,  22, 0x34000063) /* PhysicsEffectTable */
     , (490035,  35,       2101) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490035,   1, 350, 0, 0) /* Strength */
     , (490035,   2, 350, 0, 0) /* Endurance */
     , (490035,   3, 320, 0, 0) /* Quickness */
     , (490035,   4, 380, 0, 0) /* Coordination */
     , (490035,   5, 480, 0, 0) /* Focus */
     , (490035,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490035,   1,  5000, 0, 0, 5000) /* MaxHealth */
     , (490035,   3,  1910, 0, 0, 2260) /* MaxStamina */
     , (490035,   5,  1710, 0, 0, 2190) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490035,  6, 0, 3, 0, 532, 0, 0) /* MeleeDefense        Specialized */
     , (490035,  7, 0, 3, 0, 590, 0, 0) /* MissileDefense      Specialized */
     , (490035, 15, 0, 3, 0, 363, 0, 0) /* MagicDefense        Specialized */
     , (490035, 20, 0, 2, 0, 250, 0, 0) /* Deception           Trained */
     , (490035, 31, 0, 3, 0, 190, 0, 0) /* CreatureEnchantment Specialized */
     , (490035, 33, 0, 3, 0, 185, 0, 0) /* LifeMagic           Specialized */
     , (490035, 34, 0, 3, 0, 195, 0, 0) /* WarMagic            Specialized */
     , (490035, 43, 0, 3, 0, 195, 0, 0) /* VoidMagic           Specialized */
     , (490035, 44, 0, 3, 0, 477, 0, 0) /* HeavyWeapons        Specialized */
     , (490035, 45, 0, 3, 0, 477, 0, 0) /* LightWeapons        Specialized */
     , (490035, 46, 0, 3, 0, 487, 0, 0) /* FinesseWeapons      Specialized */
     , (490035, 47, 0, 3, 0, 310, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490035,  0,  4,  0,    0,  375,  375,  229,  278,  113,  375,  143,  229,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (490035,  1,  4,  0,    0,  375,  375,  229,  278,  113,  375,  143,  229,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (490035,  2,  4,  0,    0,  375,  375,  229,  278,  113,  375,  143,  229,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (490035,  3,  4,  0,    0,  375,  375,  229,  278,  113,  375,  143,  229,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (490035,  4,  4,  0,    0,  375,  375,  229,  278,  113,  375,  143,  229,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (490035,  5,  4, 260, 0.35,  375,  375,  229,  278,  113,  375,  143,  229,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (490035,  6,  4,  0,    0,  375,  375,  229,  278,  113,  375,  143,  229,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (490035,  7,  4,  0,    0,  375,  375,  229,  278,  113,  375,  143,  229,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (490035,  8,  4, 260, 0.35,  375,  375,  229,  278,  113,  375,  143,  229,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490035,  2228,   2.02)  /* Broadside of a Barn */
     , (490035,  2318,   2.02)  /* Gravity Well */
     , (490035,  4439,   2.02)  /* Incantation of Flame Bolt */
     , (490035,  4443,   2.02)  /* Incantation of Force Bolt */
     , (490035,  4451,   2.02)  /* Incantation of Lightning Bolt */
     , (490035,  4457,   2.02)  /* Incantation of Whirling Blade */;

