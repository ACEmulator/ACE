DELETE FROM `weenie` WHERE `class_Id` = 51734;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51734, 'ace51734-discorporaterynthidofconsumingtorment', 10, '2020-10-21 21:40:21') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51734,   1,         16) /* ItemType - Creature */
     , (51734,   2,         19) /* CreatureType - Virindi */
     , (51734,   3,         61) /* PaletteTemplate - White */
     , (51734,   6,         -1) /* ItemsCapacity */
     , (51734,   7,         -1) /* ContainersCapacity */
     , (51734,  16,          1) /* ItemUseable - No */
     , (51734,  25,        200) /* Level */
     , (51734,  65,          1) /* Placement - RightHandCombat */
     , (51734,  68,          3) /* TargetingTactic - Random, Focused */
     , (51734,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (51734, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (51734, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51734,   1, True ) /* Stuck */
     , (51734,   6, True ) /* AiUsesMana */
     , (51734,  11, False) /* IgnoreCollisions */
     , (51734,  12, True ) /* ReportCollisions */
     , (51734,  13, False) /* Ethereal */
     , (51734,  14, True ) /* GravityStatus */
     , (51734,  15, True ) /* LightsStatus */
     , (51734,  19, True ) /* Attackable */
     , (51734,  50, True ) /* NeverFailCasting */
     , (51734, 120, True ) /* TreasureCorpse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51734,   1,   5) /* HeartbeatInterval */
     , (51734,   2,   0) /* HeartbeatTimestamp */
     , (51734,   3, 0.6) /* HealthRate */
     , (51734,   4, 0.5) /* StaminaRate */
     , (51734,   5,   2) /* ManaRate */
     , (51734,  12,   0) /* Shade */
     , (51734,  13, 0.8) /* ArmorModVsSlash */
     , (51734,  14, 1.0) /* ArmorModVsPierce */
     , (51734,  15, 1.0) /* ArmorModVsBludgeon */
     , (51734,  16, 1.0) /* ArmorModVsCold */
     , (51734,  17, 0.8) /* ArmorModVsFire */
     , (51734,  18, 0.8) /* ArmorModVsAcid */
     , (51734,  19, 1.0) /* ArmorModVsElectric */
     , (51734,  31,  18) /* VisualAwarenessRange */
     , (51734,  34,   1) /* PowerupTime */
     , (51734,  36,   1) /* ChargeSpeed */
     , (51734,  55, 100) /* HomeRadius */
     , (51734,  64, 0.7) /* ResistSlash */
     , (51734,  65, 0.6) /* ResistPierce */
     , (51734,  66, 0.6) /* ResistBludgeon */
     , (51734,  67, 0.7) /* ResistFire */
     , (51734,  68, 0.4) /* ResistCold */
     , (51734,  69, 0.7) /* ResistAcid */
     , (51734,  70, 0.4) /* ResistElectric */
     , (51734,  80,   3) /* AiUseMagicDelay */
     , (51734, 104,  10) /* ObviousRadarRange */
     , (51734, 122,   2) /* AiAcquireHealth */
     , (51734, 125,   1) /* ResistHealthDrain */
     , (51734, 165, 1.0) /* ArmorModVsNether */
     , (51734, 166, 1.0) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51734,   1, 'Discorporate Rynthid of Consuming Torment') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51734,   1,   33561546) /* Setup */
     , (51734,   2,  150995487) /* MotionTable */
     , (51734,   3,  536870930) /* SoundTable */
     , (51734,   4,  805306381) /* CombatTable */
     , (51734,   6,   67111346) /* PaletteBase */
     , (51734,   7,  268437588) /* ClothingBase */
     , (51734,   8,  100667943) /* Icon */
     , (51734,  22,  872415443) /* PhysicsEffectTable */
     , (51734,  30,         87) /* PhysicsScript - BreatheLightning */
     , (51734,  35,       1000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (51734, 8040, 758120463, 32.8782, 151.324, 81.1484, -0.0152738, 0, 0, -0.999883) /* PCAPRecordedLocation */
/* @teleloc 0x2D30000F [32.878201 151.324005 81.148399] -0.015274 0.000000 0.000000 -0.999883 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (51734,   1, 400, 0, 0) /* Strength */
     , (51734,   2, 100, 0, 0) /* Endurance */
     , (51734,   3, 300, 0, 0) /* Quickness */
     , (51734,   4, 300, 0, 0) /* Coordination */
     , (51734,   5, 250, 0, 0) /* Focus */
     , (51734,   6, 250, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (51734,   1,   100, 0, 0, 100) /* MaxHealth */
     , (51734,   3,  2600, 0, 0, 2600) /* MaxStamina */
     , (51734,   5,   250, 0, 0, 250) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (51734,  6, 0, 2, 0, 550, 0, 0) /* MeleeDefense        Trained */
     , (51734,  7, 0, 2, 0, 450, 0, 0) /* MissileDefense      Trained */
     , (51734, 15, 0, 2, 0, 360, 0, 0) /* MagicDefense        Trained */
     , (51734, 16, 0, 2, 0, 405, 0, 0) /* ManaConversion      Trained */
     , (51734, 31, 0, 2, 0, 405, 0, 0) /* CreatureEnchantment Trained */
     , (51734, 33, 0, 2, 0, 405, 0, 0) /* LifeMagic           Trained */
     , (51734, 34, 0, 2, 0, 405, 0, 0) /* WarMagic            Trained */
     , (51734, 41, 0, 2, 0, 450, 0, 0) /* TwoHandedCombat     Trained */
     , (51734, 43, 0, 2, 0, 405, 0, 0) /* VoidMagic           Trained */
     , (51734, 44, 0, 2, 0, 450, 0, 0) /* HeavyWeapons        Trained */
     , (51734, 45, 0, 2, 0, 450, 0, 0) /* LightWeapons        Trained */
     , (51734, 46, 0, 2, 0, 450, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (51734,  0, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (51734,  1, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (51734,  2, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (51734,  3, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (51734,  4, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (51734,  5, 64, 270, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (51734,  6, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (51734,  7, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (51734,  8, 64, 270, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51734,  4483,   2.1)  /* Incantation of Lightning Vulnerability Other */;
