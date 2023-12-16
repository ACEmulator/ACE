DELETE FROM `weenie` WHERE `class_Id` = 490036;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490036, 'ace490036-shadowapprentice', 10, '2023-05-15 03:25:02') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490036,   1,         16) /* ItemType - Creature */
     , (490036,   2,         22) /* CreatureType - Shadow */
     , (490036,   3,         39) /* PaletteTemplate - Black */
     , (490036,   6,         -1) /* ItemsCapacity */
     , (490036,   7,         -1) /* ContainersCapacity */
     , (490036,   8,         90) /* Mass */
     , (490036,  16,          1) /* ItemUseable - No */
     , (490036,  25,        240) /* Level */
     , (490036,  68,          3) /* TargetingTactic - Random, Focused */
	 , (490036,  72,         52) /* FriendType - Virindi */
     , (490036,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (490036, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (490036, 113,          2) /* Gender - Female */
     , (490036, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (490036, 146,    1850000) /* XpOverride */
     , (490036, 188,          1) /* HeritageGroup - Aluvian */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490036,   1, True ) /* Stuck */
     , (490036,   6, True ) /* AiUsesMana */
     , (490036,  11, False) /* IgnoreCollisions */
     , (490036,  12, True ) /* ReportCollisions */
     , (490036,  13, False) /* Ethereal */
     , (490036,  14, True ) /* GravityStatus */
     , (490036,  19, True ) /* Attackable */
     , (490036,  42, True ) /* AllowEdgeSlide */
     , (490036,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490036,   1,       5) /* HeartbeatInterval */
     , (490036,   2,       0) /* HeartbeatTimestamp */
     , (490036,   3,     0.7) /* HealthRate */
     , (490036,   4,     2.5) /* StaminaRate */
     , (490036,   5,       1) /* ManaRate */
     , (490036,  12,     0.5) /* Shade */
     , (490036,  13,       1) /* ArmorModVsSlash */
     , (490036,  14,     1.4) /* ArmorModVsPierce */
     , (490036,  15,    1.35) /* ArmorModVsBludgeon */
     , (490036,  16,     1.4) /* ArmorModVsCold */
     , (490036,  17,    0.82) /* ArmorModVsFire */
     , (490036,  18,     1.7) /* ArmorModVsAcid */
     , (490036,  19,    1.35) /* ArmorModVsElectric */
     , (490036,  31,      50) /* VisualAwarenessRange */
     , (490036,  34,     1.1) /* PowerupTime */
     , (490036,  36,       1) /* ChargeSpeed */
     , (490036,  39,       1) /* DefaultScale */
	 , (490036,  55,      110) /* HomeRadius */
     , (490036,  64,     0.7) /* ResistSlash */
     , (490036,  65,     0.5) /* ResistPierce */
     , (490036,  66,    0.35) /* ResistBludgeon */
     , (490036,  67,    0.65) /* ResistFire */
     , (490036,  68,     0.1) /* ResistCold */
     , (490036,  69,     0.2) /* ResistAcid */
     , (490036,  70,     0.5) /* ResistElectric */
     , (490036,  71,       1) /* ResistHealthBoost */
     , (490036,  72,       1) /* ResistStaminaDrain */
     , (490036,  73,       1) /* ResistStaminaBoost */
     , (490036,  74,       1) /* ResistManaDrain */
     , (490036,  75,       1) /* ResistManaBoost */
     , (490036,  76,     0.5) /* Translucency */
     , (490036,  80,       3) /* AiUseMagicDelay */
     , (490036, 104,      10) /* ObviousRadarRange */
     , (490036, 122,       5) /* AiAcquireHealth */
     , (490036, 125,       1) /* ResistHealthDrain */
     , (490036, 166,    0.85) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490036,   1, 'Shadow Apprentice') /* Name */
     , (490036,   3, 'Female') /* Sex */
     , (490036,   4, 'Aluvian') /* HeritageGroup */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490036,   1, 0x0200071B) /* Setup */
     , (490036,   2, 0x09000093) /* MotionTable */
     , (490036,   3, 0x20000002) /* SoundTable */
     , (490036,   4, 0x30000028) /* CombatTable */
     , (490036,   6, 0x0400007E) /* PaletteBase */
     , (490036,   7, 0x1000019F) /* ClothingBase */
     , (490036,   8, 0x06001BBE) /* Icon */
     , (490036,  22, 0x34000063) /* PhysicsEffectTable */
     , (490036,  35,       2101) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490036,   1, 350, 0, 0) /* Strength */
     , (490036,   2, 350, 0, 0) /* Endurance */
     , (490036,   3, 320, 0, 0) /* Quickness */
     , (490036,   4, 380, 0, 0) /* Coordination */
     , (490036,   5, 480, 0, 0) /* Focus */
     , (490036,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490036,   1,  9000, 0, 0, 10000) /* MaxHealth */
     , (490036,   3,  1250, 0, 0, 1600) /* MaxStamina */
     , (490036,   5,  1400, 0, 0, 1880) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490036,  6, 0, 3, 0, 532, 0, 0) /* MeleeDefense        Specialized */
     , (490036,  7, 0, 3, 0, 590, 0, 0) /* MissileDefense      Specialized */
     , (490036, 14, 0, 2, 0, 290, 0, 0) /* ArcaneLore          Trained */
     , (490036, 15, 0, 3, 0, 363, 0, 0) /* MagicDefense        Specialized */
     , (490036, 20, 0, 2, 0, 250, 0, 0) /* Deception           Trained */
     , (490036, 31, 0, 3, 0, 190, 0, 0) /* CreatureEnchantment Specialized */
     , (490036, 33, 0, 3, 0, 185, 0, 0) /* LifeMagic           Specialized */
     , (490036, 34, 0, 3, 0, 195, 0, 0) /* WarMagic            Specialized */
     , (490036, 43, 0, 3, 0, 195, 0, 0) /* VoidMagic           Specialized */
     , (490036, 44, 0, 3, 0, 477, 0, 0) /* HeavyWeapons        Specialized */
     , (490036, 45, 0, 3, 0, 477, 0, 0) /* LightWeapons        Specialized */
     , (490036, 46, 0, 3, 0, 487, 0, 0) /* FinesseWeapons      Specialized */
     , (490036, 47, 0, 3, 0, 310, 0, 0) /* MissileWeapons      Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490036,  0,  4,  0,    0,  375,  375,  525,  506,  525,  308,  638,  506,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (490036,  1,  4,  0,    0,  375,  375,  525,  506,  525,  308,  638,  506,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (490036,  2,  4,  0,    0,  375,  375,  525,  506,  525,  308,  638,  506,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (490036,  3,  4,  0,    0,  375,  375,  525,  506,  525,  308,  638,  506,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (490036,  4,  4,  0,    0,  375,  375,  525,  506,  525,  308,  638,  506,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (490036,  5,  4, 260, 0.35,  375,  375,  525,  506,  525,  308,  638,  506,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (490036,  6,  4,  0,    0,  375,  375,  525,  506,  525,  308,  638,  506,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (490036,  7,  4,  0,    0,  375,  375,  525,  506,  525,  308,  638,  506,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (490036,  8,  4, 260, 0.35,  375,  375,  525,  506,  525,  308,  638,  506,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490036,  2264,   2.02)  /* Wrath of Harlune */
     , (490036,  2282,   2.02)  /* Futility */
     , (490036,  2328,   2.01)  /* Vitality Siphon */
     , (490036,  4302,   2.02)  /* Incantation of Feeblemind Other */
     , (490036,  4322,   2.02)  /* Incantation of Slowness Other */
     , (490036,  4436,   2.02)  /* Incantation of Blade Volley */
     , (490036,  4439,   2.02)  /* Incantation of Flame Bolt */
     , (490036,  4443,   2.02)  /* Incantation of Force Bolt */
     , (490036,  4447,   2.02)  /* Incantation of Frost Bolt */
     , (490036,  4451,   2.02)  /* Incantation of Lightning Bolt */
     , (490036,  4457,   2.02)  /* Incantation of Whirling Blade */
     , (490036,  4633,   2.02)  /* Incantation of Vulnerability Other */
     , (490036,  5344,   2.04)  /* Destructive Curse VI */
     , (490036,  5355,   2.06)  /* Nether Bolt VII */
     , (490036,  5367,   2.07)  /* Nether Arc VII */
     , (490036,  5377,   2.05)  /* Festering Curse VII */
     , (490036,  5385,   2.07)  /* Weakening Curse VII */
     , (490036,  5392,   2.09)  /* Corrosion VI */
     , (490036,  5401,   2.07)  /* Corruption VII */;

