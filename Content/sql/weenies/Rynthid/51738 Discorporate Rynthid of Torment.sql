DELETE FROM `weenie` WHERE `class_Id` = 51738;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51738, 'ace51738-discorporaterynthidoftorment', 10, '2020-10-21 21:40:20') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51738,   1,         16) /* ItemType - Creature */
     , (51738,   2,         19) /* CreatureType - Virindi */
     , (51738,   3,         61) /* PaletteTemplate - White */
     , (51738,   6,         -1) /* ItemsCapacity */
     , (51738,   7,         -1) /* ContainersCapacity */
     , (51738,  16,          1) /* ItemUseable - No */
     , (51738,  25,        200) /* Level */
     , (51738,  65,          1) /* Placement - RightHandCombat */
     , (51738,  68,          3) /* TargetingTactic - Random, Focused */
     , (51738,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (51738, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (51738, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51738,   1, True ) /* Stuck */
     , (51738,   6, True ) /* AiUsesMana */
     , (51738,  11, False) /* IgnoreCollisions */
     , (51738,  12, True ) /* ReportCollisions */
     , (51738,  13, False) /* Ethereal */
     , (51738,  14, True ) /* GravityStatus */
     , (51738,  15, True ) /* LightsStatus */
     , (51738,  19, True ) /* Attackable */
     , (51738,  50, True ) /* NeverFailCasting */
     , (51738, 120, True ) /* TreasureCorpse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51738,   1,   5) /* HeartbeatInterval */
     , (51738,   2,   0) /* HeartbeatTimestamp */
     , (51738,   3, 0.6) /* HealthRate */
     , (51738,   4, 0.5) /* StaminaRate */
     , (51738,   5,   2) /* ManaRate */
     , (51738,  12,   0) /* Shade */
     , (51738,  13, 0.8) /* ArmorModVsSlash */
     , (51738,  14, 1.0) /* ArmorModVsPierce */
     , (51738,  15, 1.0) /* ArmorModVsBludgeon */
     , (51738,  16, 1.0) /* ArmorModVsCold */
     , (51738,  17, 0.8) /* ArmorModVsFire */
     , (51738,  18, 0.8) /* ArmorModVsAcid */
     , (51738,  19, 1.0) /* ArmorModVsElectric */
     , (51738,  31,  18) /* VisualAwarenessRange */
     , (51738,  34,   1) /* PowerupTime */
     , (51738,  36,   1) /* ChargeSpeed */
     , (51738,  55, 100) /* HomeRadius */
     , (51738,  64, 0.7) /* ResistSlash */
     , (51738,  65, 0.6) /* ResistPierce */
     , (51738,  66, 0.6) /* ResistBludgeon */
     , (51738,  67, 0.7) /* ResistFire */
     , (51738,  68, 0.4) /* ResistCold */
     , (51738,  69, 0.7) /* ResistAcid */
     , (51738,  70, 0.4) /* ResistElectric */
     , (51738,  80,   3) /* AiUseMagicDelay */
     , (51738, 104,  10) /* ObviousRadarRange */
     , (51738, 122,   2) /* AiAcquireHealth */
     , (51738, 125,   1) /* ResistHealthDrain */
     , (51738, 165, 1.0) /* ArmorModVsNether */
     , (51738, 166, 1.0) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51738,   1, 'Discorporate Rynthid of Torment') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51738,   1,   33561546) /* Setup */
     , (51738,   2,  150995487) /* MotionTable */
     , (51738,   3,  536870930) /* SoundTable */
     , (51738,   4,  805306381) /* CombatTable */
     , (51738,   6,   67111346) /* PaletteBase */
     , (51738,   7,  268437588) /* ClothingBase */
     , (51738,   8,  100667943) /* Icon */
     , (51738,  22,  872415443) /* PhysicsEffectTable */
     , (51738,  30,         87) /* PhysicsScript - BreatheLightning */
     , (51738,  35,       1000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (51738, 8040, 758186023, 114.098, 155.541, 127.646, -0.879723, 0, 0, -0.475486) /* PCAPRecordedLocation */
/* @teleloc 0x2D310027 [114.098000 155.541000 127.646004] -0.879723 0.000000 0.000000 -0.475486 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (51738,   1, 400, 0, 0) /* Strength */
     , (51738,   2, 100, 0, 0) /* Endurance */
     , (51738,   3, 300, 0, 0) /* Quickness */
     , (51738,   4, 300, 0, 0) /* Coordination */
     , (51738,   5, 250, 0, 0) /* Focus */
     , (51738,   6, 250, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (51738,   1,   100, 0, 0, 100) /* MaxHealth */
     , (51738,   3,  2600, 0, 0, 2600) /* MaxStamina */
     , (51738,   5,   250, 0, 0, 250) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (51738,  6, 0, 2, 0, 550, 0, 0) /* MeleeDefense        Trained */
     , (51738,  7, 0, 2, 0, 450, 0, 0) /* MissileDefense      Trained */
     , (51738, 15, 0, 2, 0, 360, 0, 0) /* MagicDefense        Trained */
     , (51738, 16, 0, 2, 0, 405, 0, 0) /* ManaConversion      Trained */
     , (51738, 31, 0, 2, 0, 405, 0, 0) /* CreatureEnchantment Trained */
     , (51738, 33, 0, 2, 0, 405, 0, 0) /* LifeMagic           Trained */
     , (51738, 34, 0, 2, 0, 405, 0, 0) /* WarMagic            Trained */
     , (51738, 41, 0, 2, 0, 450, 0, 0) /* TwoHandedCombat     Trained */
     , (51738, 43, 0, 2, 0, 405, 0, 0) /* VoidMagic           Trained */
     , (51738, 44, 0, 2, 0, 450, 0, 0) /* HeavyWeapons        Trained */
     , (51738, 45, 0, 2, 0, 450, 0, 0) /* LightWeapons        Trained */
     , (51738, 46, 0, 2, 0, 450, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (51738,  0, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (51738,  1, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (51738,  2, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (51738,  3, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (51738,  4, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (51738,  5, 64, 250, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (51738,  6, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (51738,  7, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (51738,  8, 64, 250, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51738,  3989,     2.1)  /* Dark Lightning */
     , (51738,  4312,   2.056)  /* Incantation of Imperil Other */
     , (51738,  4483,   2.059)  /* Incantation of Lightning Vulnerability Other */
     , (51738,  4597,   2.063)  /* Incantation of Magic Yield Other */
     , (51738,  4633,   2.067)  /* Incantation of Vulnerability Other */;
