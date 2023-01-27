DELETE FROM `weenie` WHERE `class_Id` = 51736;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51736, 'ace51736-discorporaterynthidoftorment', 10, '2020-10-21 21:40:18') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51736,   1,         16) /* ItemType - Creature */
     , (51736,   2,         19) /* CreatureType - Virindi */
     , (51736,   3,         61) /* PaletteTemplate - White */
     , (51736,   6,         -1) /* ItemsCapacity */
     , (51736,   7,         -1) /* ContainersCapacity */
     , (51736,  16,          1) /* ItemUseable - No */
     , (51736,  25,        200) /* Level */
     , (51736,  65,          1) /* Placement - RightHandCombat */
     , (51736,  68,          3) /* TargetingTactic - Random, Focused */
     , (51736,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (51736, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (51736, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51736,   1, True ) /* Stuck */
     , (51736,   6, True ) /* AiUsesMana */
     , (51736,  11, False) /* IgnoreCollisions */
     , (51736,  12, True ) /* ReportCollisions */
     , (51736,  13, False) /* Ethereal */
     , (51736,  14, True ) /* GravityStatus */
     , (51736,  15, True ) /* LightsStatus */
     , (51736,  19, True ) /* Attackable */
     , (51736,  50, True ) /* NeverFailCasting */
     , (51736, 120, True ) /* TreasureCorpse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51736,   1,   5) /* HeartbeatInterval */
     , (51736,   2,   0) /* HeartbeatTimestamp */
     , (51736,   3, 0.6) /* HealthRate */
     , (51736,   4, 0.5) /* StaminaRate */
     , (51736,   5,   2) /* ManaRate */
     , (51736,  12,   0) /* Shade */
     , (51736,  13, 0.8) /* ArmorModVsSlash */
     , (51736,  14, 1.0) /* ArmorModVsPierce */
     , (51736,  15, 1.0) /* ArmorModVsBludgeon */
     , (51736,  16, 1.0) /* ArmorModVsCold */
     , (51736,  17, 0.8) /* ArmorModVsFire */
     , (51736,  18, 0.8) /* ArmorModVsAcid */
     , (51736,  19, 1.0) /* ArmorModVsElectric */
     , (51736,  31,  18) /* VisualAwarenessRange */
     , (51736,  34,   1) /* PowerupTime */
     , (51736,  36,   1) /* ChargeSpeed */
     , (51736,  55, 100) /* HomeRadius */
     , (51736,  64, 0.7) /* ResistSlash */
     , (51736,  65, 0.6) /* ResistPierce */
     , (51736,  66, 0.6) /* ResistBludgeon */
     , (51736,  67, 0.7) /* ResistFire */
     , (51736,  68, 0.4) /* ResistCold */
     , (51736,  69, 0.7) /* ResistAcid */
     , (51736,  70, 0.4) /* ResistElectric */
     , (51736,  80,   3) /* AiUseMagicDelay */
     , (51736, 104,  10) /* ObviousRadarRange */
     , (51736, 122,   2) /* AiAcquireHealth */
     , (51736, 125,   1) /* ResistHealthDrain */
     , (51736, 165, 1.0) /* ArmorModVsNether */
     , (51736, 166, 1.0) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51736,   1, 'Discorporate Rynthid of Torment') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51736,   1,   33561546) /* Setup */
     , (51736,   2,  150995487) /* MotionTable */
     , (51736,   3,  536870930) /* SoundTable */
     , (51736,   4,  805306381) /* CombatTable */
     , (51736,   6,   67111346) /* PaletteBase */
     , (51736,   7,  268437588) /* ClothingBase */
     , (51736,   8,  100667943) /* Icon */
     , (51736,  22,  872415443) /* PhysicsEffectTable */
     , (51736,  30,         87) /* PhysicsScript - BreatheLightning */
     , (51736,  35,       1000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (51736, 8040, 758186046, 181.678, 132.515, 132.029, 0.817007, 0, 0, -0.576629) /* PCAPRecordedLocation */
/* @teleloc 0x2D31003E [181.677994 132.514999 132.029007] 0.817007 0.000000 0.000000 -0.576629 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (51736,   1, 400, 0, 0) /* Strength */
     , (51736,   2, 100, 0, 0) /* Endurance */
     , (51736,   3, 300, 0, 0) /* Quickness */
     , (51736,   4, 300, 0, 0) /* Coordination */
     , (51736,   5, 250, 0, 0) /* Focus */
     , (51736,   6, 250, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (51736,   1,   100, 0, 0, 100) /* MaxHealth */
     , (51736,   3,  2600, 0, 0, 2600) /* MaxStamina */
     , (51736,   5,   250, 0, 0, 250) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (51736,  6, 0, 2, 0, 550, 0, 0) /* MeleeDefense        Trained */
     , (51736,  7, 0, 2, 0, 450, 0, 0) /* MissileDefense      Trained */
     , (51736, 15, 0, 2, 0, 360, 0, 0) /* MagicDefense        Trained */
     , (51736, 16, 0, 2, 0, 405, 0, 0) /* ManaConversion      Trained */
     , (51736, 31, 0, 2, 0, 405, 0, 0) /* CreatureEnchantment Trained */
     , (51736, 33, 0, 2, 0, 405, 0, 0) /* LifeMagic           Trained */
     , (51736, 34, 0, 2, 0, 405, 0, 0) /* WarMagic            Trained */
     , (51736, 41, 0, 2, 0, 450, 0, 0) /* TwoHandedCombat     Trained */
     , (51736, 43, 0, 2, 0, 405, 0, 0) /* VoidMagic           Trained */
     , (51736, 44, 0, 2, 0, 450, 0, 0) /* HeavyWeapons        Trained */
     , (51736, 45, 0, 2, 0, 450, 0, 0) /* LightWeapons        Trained */
     , (51736, 46, 0, 2, 0, 450, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (51736,  0, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (51736,  1, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (51736,  2, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (51736,  3, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (51736,  4, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (51736,  5, 64, 250, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (51736,  6, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (51736,  7, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (51736,  8, 64, 250, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51736,  3989,   2.08)  /* Dark Lightning */
     , (51736,  4312,   2.05)  /* Incantation of Imperil Other */
     , (51736,  4483,   2.05)  /* Incantation of Lightning Vulnerability Other */
     , (51736,  4597,   2.05)  /* Incantation of Magic Yield Other */
     , (51736,  4633,   2.05)  /* Incantation of Vulnerability Other */;
