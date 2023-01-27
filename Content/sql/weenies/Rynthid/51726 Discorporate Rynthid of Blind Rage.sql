DELETE FROM `weenie` WHERE `class_Id` = 51726;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51726, 'ace51726-discorporaterynthidofblindrage', 10, '2020-10-21 21:40:18') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51726,   1,         16) /* ItemType - Creature */
     , (51726,   2,         19) /* CreatureType - Virindi */
     , (51726,   3,         61) /* PaletteTemplate - White */
     , (51726,   6,         -1) /* ItemsCapacity */
     , (51726,   7,         -1) /* ContainersCapacity */
     , (51726,  16,          1) /* ItemUseable - No */
     , (51726,  25,        200) /* Level */
     , (51726,  65,          1) /* Placement - RightHandCombat */
     , (51726,  68,          3) /* TargetingTactic - Random, Focused */
     , (51726,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (51726, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (51726, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51726,   1, True ) /* Stuck */
     , (51726,   6, True ) /* AiUsesMana */
     , (51726,  11, False) /* IgnoreCollisions */
     , (51726,  12, True ) /* ReportCollisions */
     , (51726,  13, False) /* Ethereal */
     , (51726,  14, True ) /* GravityStatus */
     , (51726,  15, True ) /* LightsStatus */
     , (51726,  19, True ) /* Attackable */
     , (51726,  50, True ) /* NeverFailCasting */
     , (51726, 120, True ) /* TreasureCorpse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51726,   1,   5) /* HeartbeatInterval */
     , (51726,   2,   0) /* HeartbeatTimestamp */
     , (51726,   3, 0.6) /* HealthRate */
     , (51726,   4, 0.5) /* StaminaRate */
     , (51726,   5,   2) /* ManaRate */
     , (51726,  12,   0) /* Shade */
     , (51726,  13, 0.8) /* ArmorModVsSlash */
     , (51726,  14, 1.0) /* ArmorModVsPierce */
     , (51726,  15, 1.0) /* ArmorModVsBludgeon */
     , (51726,  16, 1.0) /* ArmorModVsCold */
     , (51726,  17, 0.8) /* ArmorModVsFire */
     , (51726,  18, 0.8) /* ArmorModVsAcid */
     , (51726,  19, 1.0) /* ArmorModVsElectric */
     , (51726,  31,  18) /* VisualAwarenessRange */
     , (51726,  34,   1) /* PowerupTime */
     , (51726,  36,   1) /* ChargeSpeed */
     , (51726,  55, 100) /* HomeRadius */
     , (51726,  64, 0.7) /* ResistSlash */
     , (51726,  65, 0.6) /* ResistPierce */
     , (51726,  66, 0.6) /* ResistBludgeon */
     , (51726,  67, 0.7) /* ResistFire */
     , (51726,  68, 0.4) /* ResistCold */
     , (51726,  69, 0.7) /* ResistAcid */
     , (51726,  70, 0.4) /* ResistElectric */
     , (51726,  80,   3) /* AiUseMagicDelay */
     , (51726, 104,  10) /* ObviousRadarRange */
     , (51726, 122,   2) /* AiAcquireHealth */
     , (51726, 125,   1) /* ResistHealthDrain */
     , (51726, 165, 1.0) /* ArmorModVsNether */
     , (51726, 166, 1.0) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51726,   1, 'Discorporate Rynthid of Blind Rage') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51726,   1,   33561550) /* Setup */
     , (51726,   2,  150995487) /* MotionTable */
     , (51726,   3,  536870930) /* SoundTable */
     , (51726,   4,  805306381) /* CombatTable */
     , (51726,   6,   67111346) /* PaletteBase */
     , (51726,   7,  268437588) /* ClothingBase */
     , (51726,   8,  100667943) /* Icon */
     , (51726,  22,  872415443) /* PhysicsEffectTable */
     , (51726,  30,         84) /* PhysicsScript - BreatheFlame */
     , (51726,  35,       1000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (51726, 8040, 758185991, 12.102, 144.223, 98.0089, 0.624594, 0, 0, -0.78095) /* PCAPRecordedLocation */
/* @teleloc 0x2D310007 [12.102000 144.223007 98.008904] 0.624594 0.000000 0.000000 -0.780950 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (51726,   1, 400, 0, 0) /* Strength */
     , (51726,   2, 100, 0, 0) /* Endurance */
     , (51726,   3, 300, 0, 0) /* Quickness */
     , (51726,   4, 300, 0, 0) /* Coordination */
     , (51726,   5, 250, 0, 0) /* Focus */
     , (51726,   6, 250, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (51726,   1,   100, 0, 0, 100) /* MaxHealth */
     , (51726,   3,  2600, 0, 0, 2600) /* MaxStamina */
     , (51726,   5,   250, 0, 0, 250) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (51726,  6, 0, 2, 0, 550, 0, 0) /* MeleeDefense        Trained */
     , (51726,  7, 0, 2, 0, 450, 0, 0) /* MissileDefense      Trained */
     , (51726, 15, 0, 2, 0, 350, 0, 0) /* MagicDefense        Trained */
     , (51726, 16, 0, 2, 0, 380, 0, 0) /* ManaConversion      Trained */
     , (51726, 31, 0, 2, 0, 380, 0, 0) /* CreatureEnchantment Trained */
     , (51726, 33, 0, 2, 0, 380, 0, 0) /* LifeMagic           Trained */
     , (51726, 34, 0, 2, 0, 380, 0, 0) /* WarMagic            Trained */
     , (51726, 41, 0, 2, 0, 460, 0, 0) /* TwoHandedCombat     Trained */
     , (51726, 43, 0, 2, 0, 380, 0, 0) /* VoidMagic           Trained */
     , (51726, 44, 0, 2, 0, 460, 0, 0) /* HeavyWeapons        Trained */
     , (51726, 45, 0, 2, 0, 460, 0, 0) /* LightWeapons        Trained */
     , (51726, 46, 0, 2, 0, 460, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (51726,  0, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (51726,  1, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (51726,  2, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (51726,  3, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (51726,  4, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (51726,  5, 16, 270, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (51726,  6, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (51726,  7, 16,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (51726,  8, 16, 270, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;
	 
INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51726,  4481,   2.1)  /* Incantation of Fire Vulnerability Other */;
