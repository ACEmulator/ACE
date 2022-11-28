DELETE FROM `weenie` WHERE `class_Id` = 51746;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51746, 'ace51746-discorporaterynthidoftorment', 10, '2020-10-21 21:40:21') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51746,   1,         16) /* ItemType - Creature */
     , (51746,   2,         19) /* CreatureType - Virindi */
     , (51746,   3,         61) /* PaletteTemplate - White */
     , (51746,   6,         -1) /* ItemsCapacity */
     , (51746,   7,         -1) /* ContainersCapacity */
     , (51746,  16,          1) /* ItemUseable - No */
     , (51746,  25,        200) /* Level */
     , (51746,  65,          1) /* Placement - RightHandCombat */
     , (51746,  68,          3) /* TargetingTactic - Random, Focused */
     , (51746,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (51746, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (51746, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51746,   1, True ) /* Stuck */
     , (51746,   6, True ) /* AiUsesMana */
     , (51746,  11, False) /* IgnoreCollisions */
     , (51746,  12, True ) /* ReportCollisions */
     , (51746,  13, False) /* Ethereal */
     , (51746,  14, True ) /* GravityStatus */
     , (51746,  15, True ) /* LightsStatus */
     , (51746,  19, True ) /* Attackable */
     , (51746,  50, True ) /* NeverFailCasting */
     , (51746, 120, True ) /* TreasureCorpse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51746,   1,   5) /* HeartbeatInterval */
     , (51746,   2,   0) /* HeartbeatTimestamp */
     , (51746,   3, 0.6) /* HealthRate */
     , (51746,   4, 0.5) /* StaminaRate */
     , (51746,   5,   2) /* ManaRate */
     , (51746,  12,   0) /* Shade */
     , (51746,  13, 0.8) /* ArmorModVsSlash */
     , (51746,  14, 1.0) /* ArmorModVsPierce */
     , (51746,  15, 1.0) /* ArmorModVsBludgeon */
     , (51746,  16, 1.0) /* ArmorModVsCold */
     , (51746,  17, 0.8) /* ArmorModVsFire */
     , (51746,  18, 0.8) /* ArmorModVsAcid */
     , (51746,  19, 1.0) /* ArmorModVsElectric */
     , (51746,  31,  18) /* VisualAwarenessRange */
     , (51746,  34,   1) /* PowerupTime */
     , (51746,  36,   1) /* ChargeSpeed */
     , (51746,  55, 100) /* HomeRadius */
     , (51746,  64, 0.7) /* ResistSlash */
     , (51746,  65, 0.6) /* ResistPierce */
     , (51746,  66, 0.6) /* ResistBludgeon */
     , (51746,  67, 0.7) /* ResistFire */
     , (51746,  68, 0.4) /* ResistCold */
     , (51746,  69, 0.7) /* ResistAcid */
     , (51746,  70, 0.4) /* ResistElectric */
     , (51746,  80,   3) /* AiUseMagicDelay */
     , (51746, 104,  10) /* ObviousRadarRange */
     , (51746, 122,   2) /* AiAcquireHealth */
     , (51746, 125,   1) /* ResistHealthDrain */
     , (51746, 165, 1.0) /* ArmorModVsNether */
     , (51746, 166, 1.0) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51746,   1, 'Discorporate Rynthid of Torment') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51746,   1,   33561546) /* Setup */
     , (51746,   2,  150995487) /* MotionTable */
     , (51746,   3,  536870930) /* SoundTable */
     , (51746,   4,  805306381) /* CombatTable */
     , (51746,   6,   67111346) /* PaletteBase */
     , (51746,   7,  268437588) /* ClothingBase */
     , (51746,   8,  100667943) /* Icon */
     , (51746,  22,  872415443) /* PhysicsEffectTable */
     , (51746,  30,         87) /* PhysicsScript - BreatheLightning */
     , (51746,  35,       1000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (51746, 8040, 741408822, 155.337, 132.444, 97.539, 1, 0, 0, 0) /* PCAPRecordedLocation */
/* @teleloc 0x2C310036 [155.337006 132.444000 97.539001] 1.000000 0.000000 0.000000 0.000000 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (51746,   1, 400, 0, 0) /* Strength */
     , (51746,   2, 100, 0, 0) /* Endurance */
     , (51746,   3, 300, 0, 0) /* Quickness */
     , (51746,   4, 300, 0, 0) /* Coordination */
     , (51746,   5, 250, 0, 0) /* Focus */
     , (51746,   6, 250, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (51746,   1,   100, 0, 0, 100) /* MaxHealth */
     , (51746,   3,  2600, 0, 0, 2600) /* MaxStamina */
     , (51746,   5,   250, 0, 0, 250) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (51746,  6, 0, 2, 0, 550, 0, 0) /* MeleeDefense        Trained */
     , (51746,  7, 0, 2, 0, 450, 0, 0) /* MissileDefense      Trained */
     , (51746, 15, 0, 2, 0, 360, 0, 0) /* MagicDefense        Trained */
     , (51746, 16, 0, 2, 0, 405, 0, 0) /* ManaConversion      Trained */
     , (51746, 31, 0, 2, 0, 405, 0, 0) /* CreatureEnchantment Trained */
     , (51746, 33, 0, 2, 0, 405, 0, 0) /* LifeMagic           Trained */
     , (51746, 34, 0, 2, 0, 405, 0, 0) /* WarMagic            Trained */
     , (51746, 41, 0, 2, 0, 450, 0, 0) /* TwoHandedCombat     Trained */
     , (51746, 43, 0, 2, 0, 405, 0, 0) /* VoidMagic           Trained */
     , (51746, 44, 0, 2, 0, 450, 0, 0) /* HeavyWeapons        Trained */
     , (51746, 45, 0, 2, 0, 450, 0, 0) /* LightWeapons        Trained */
     , (51746, 46, 0, 2, 0, 450, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (51746,  0, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (51746,  1, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (51746,  2, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (51746,  3, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (51746,  4, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (51746,  5, 64, 250, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (51746,  6, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (51746,  7, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (51746,  8, 64, 250, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51746,  3989,     2.1)  /* Dark Lightning */
     , (51746,  4312,   2.056)  /* Incantation of Imperil Other */
     , (51746,  4483,   2.059)  /* Incantation of Lightning Vulnerability Other */
     , (51746,  4597,   2.063)  /* Incantation of Magic Yield Other */
     , (51746,  4633,   2.067)  /* Incantation of Vulnerability Other */;
