DELETE FROM `weenie` WHERE `class_Id` = 51728;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51728, 'ace51728-discorporaterynthidofrage', 10, '2020-10-21 21:40:21') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51728,   1,         16) /* ItemType - Creature */
     , (51728,   2,         19) /* CreatureType - Virindi */
     , (51728,   3,         61) /* PaletteTemplate - White */
     , (51728,   6,         -1) /* ItemsCapacity */
     , (51728,   7,         -1) /* ContainersCapacity */
     , (51728,  16,          1) /* ItemUseable - No */
     , (51728,  25,        200) /* Level */
     , (51728,  65,          1) /* Placement - RightHandCombat */
     , (51728,  68,          3) /* TargetingTactic - Random, Focused */
     , (51728,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (51728, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (51728, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51728,   1, True ) /* Stuck */
     , (51728,   6, True ) /* AiUsesMana */
     , (51728,  11, False) /* IgnoreCollisions */
     , (51728,  12, True ) /* ReportCollisions */
     , (51728,  13, False) /* Ethereal */
     , (51728,  14, True ) /* GravityStatus */
     , (51728,  15, True ) /* LightsStatus */
     , (51728,  19, True ) /* Attackable */
     , (51728,  50, True ) /* NeverFailCasting */
     , (51728, 120, True ) /* TreasureCorpse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51728,   1,   5) /* HeartbeatInterval */
     , (51728,   2,   0) /* HeartbeatTimestamp */
     , (51728,   3, 0.6) /* HealthRate */
     , (51728,   4, 0.5) /* StaminaRate */
     , (51728,   5,   2) /* ManaRate */
     , (51728,  12,   0) /* Shade */
     , (51728,  13, 0.8) /* ArmorModVsSlash */
     , (51728,  14, 1.0) /* ArmorModVsPierce */
     , (51728,  15, 1.0) /* ArmorModVsBludgeon */
     , (51728,  16, 1.0) /* ArmorModVsCold */
     , (51728,  17, 0.8) /* ArmorModVsFire */
     , (51728,  18, 0.8) /* ArmorModVsAcid */
     , (51728,  19, 1.0) /* ArmorModVsElectric */
     , (51728,  31,  18) /* VisualAwarenessRange */
     , (51728,  34,   1) /* PowerupTime */
     , (51728,  36,   1) /* ChargeSpeed */
     , (51728,  55, 100) /* HomeRadius */
     , (51728,  64, 0.7) /* ResistSlash */
     , (51728,  65, 0.6) /* ResistPierce */
     , (51728,  66, 0.6) /* ResistBludgeon */
     , (51728,  67, 0.7) /* ResistFire */
     , (51728,  68, 0.4) /* ResistCold */
     , (51728,  69, 0.7) /* ResistAcid */
     , (51728,  70, 0.4) /* ResistElectric */
     , (51728,  80,   3) /* AiUseMagicDelay */
     , (51728, 104,  10) /* ObviousRadarRange */
     , (51728, 122,   2) /* AiAcquireHealth */
     , (51728, 125,   1) /* ResistHealthDrain */
     , (51728, 165, 1.0) /* ArmorModVsNether */
     , (51728, 166, 1.0) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51728,   1, 'Discorporate Rynthid of Rage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51728,   1,   33561550) /* Setup */
     , (51728,   2,  150995487) /* MotionTable */
     , (51728,   3,  536870930) /* SoundTable */
     , (51728,   4,  805306381) /* CombatTable */
     , (51728,   6,   67111346) /* PaletteBase */
     , (51728,   7,  268437588) /* ClothingBase */
     , (51728,   8,  100667943) /* Icon */
     , (51728,  22,  872415443) /* PhysicsEffectTable */
     , (51728,  30,         84) /* PhysicsScript - BreatheFlame */
     , (51728,  35,       1000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (51728, 8040, 758185997, 31.3764, 108.018, 185.039, -0.128368, 0, 0, -0.991727) /* PCAPRecordedLocation */
/* @teleloc 0x2D31000D [31.376400 108.017998 185.039001] -0.128368 0.000000 0.000000 -0.991727 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (51728,   1, 400, 0, 0) /* Strength */
     , (51728,   2, 100, 0, 0) /* Endurance */
     , (51728,   3, 300, 0, 0) /* Quickness */
     , (51728,   4, 300, 0, 0) /* Coordination */
     , (51728,   5, 250, 0, 0) /* Focus */
     , (51728,   6, 250, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (51728,   1,   100, 0, 0, 100) /* MaxHealth */
     , (51728,   3,  2600, 0, 0, 2600) /* MaxStamina */
     , (51728,   5,   250, 0, 0, 250) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (51728,  6, 0, 2, 0, 550, 0, 0) /* MeleeDefense        Trained */
     , (51728,  7, 0, 2, 0, 450, 0, 0) /* MissileDefense      Trained */
     , (51728, 15, 0, 2, 0, 350, 0, 0) /* MagicDefense        Trained */
     , (51728, 16, 0, 2, 0, 380, 0, 0) /* ManaConversion      Trained */
     , (51728, 31, 0, 2, 0, 380, 0, 0) /* CreatureEnchantment Trained */
     , (51728, 33, 0, 2, 0, 380, 0, 0) /* LifeMagic           Trained */
     , (51728, 34, 0, 2, 0, 380, 0, 0) /* WarMagic            Trained */
     , (51728, 41, 0, 2, 0, 460, 0, 0) /* TwoHandedCombat     Trained */
     , (51728, 43, 0, 2, 0, 380, 0, 0) /* VoidMagic           Trained */
     , (51728, 44, 0, 2, 0, 460, 0, 0) /* HeavyWeapons        Trained */
     , (51728, 45, 0, 2, 0, 460, 0, 0) /* LightWeapons        Trained */
     , (51728, 46, 0, 2, 0, 460, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (51728,  0, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (51728,  1, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (51728,  2, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (51728,  3, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (51728,  4, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (51728,  5, 16, 250, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (51728,  6, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (51728,  7, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (51728,  8, 16, 250, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51728,  4312,     2.1)  /* Incantation of Imperil Other */
     , (51728,  4481,   2.056)  /* Incantation of Fire Vulnerability Other */
     , (51728,  4439,   2.059)  /* Incantation of Flame Bolt */
     , (51728,  4597,   2.063)  /* Incantation of Magic Yield Other */
     , (51728,  4633,   2.067)  /* Incantation of Vulnerability Other */;
