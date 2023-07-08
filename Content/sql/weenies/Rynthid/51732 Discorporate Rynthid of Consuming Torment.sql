DELETE FROM `weenie` WHERE `class_Id` = 51732;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51732, 'ace51732-discorporaterynthidofconsumingtorment', 10, '2020-10-21 21:40:18') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51732,   1,         16) /* ItemType - Creature */
     , (51732,   2,         19) /* CreatureType - Virindi */
     , (51732,   3,         61) /* PaletteTemplate - White */
     , (51732,   6,         -1) /* ItemsCapacity */
     , (51732,   7,         -1) /* ContainersCapacity */
     , (51732,  16,          1) /* ItemUseable - No */
     , (51732,  25,        200) /* Level */
     , (51732,  65,          1) /* Placement - RightHandCombat */
     , (51732,  68,          3) /* TargetingTactic - Random, Focused */
     , (51732,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (51732, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (51732, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51732,   1, True ) /* Stuck */
     , (51732,   6, True ) /* AiUsesMana */
     , (51732,  11, False) /* IgnoreCollisions */
     , (51732,  12, True ) /* ReportCollisions */
     , (51732,  13, False) /* Ethereal */
     , (51732,  14, True ) /* GravityStatus */
     , (51732,  15, True ) /* LightsStatus */
     , (51732,  19, True ) /* Attackable */
     , (51732,  50, True ) /* NeverFailCasting */
     , (51732, 120, True ) /* TreasureCorpse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51732,   1,   5) /* HeartbeatInterval */
     , (51732,   2,   0) /* HeartbeatTimestamp */
     , (51732,   3, 0.6) /* HealthRate */
     , (51732,   4, 0.5) /* StaminaRate */
     , (51732,   5,   2) /* ManaRate */
     , (51732,  12,   0) /* Shade */
     , (51732,  13, 0.8) /* ArmorModVsSlash */
     , (51732,  14, 1.0) /* ArmorModVsPierce */
     , (51732,  15, 1.0) /* ArmorModVsBludgeon */
     , (51732,  16, 1.0) /* ArmorModVsCold */
     , (51732,  17, 0.8) /* ArmorModVsFire */
     , (51732,  18, 0.8) /* ArmorModVsAcid */
     , (51732,  19, 1.0) /* ArmorModVsElectric */
     , (51732,  31,  18) /* VisualAwarenessRange */
     , (51732,  34,   1) /* PowerupTime */
     , (51732,  36,   1) /* ChargeSpeed */
     , (51732,  55, 100) /* HomeRadius */
     , (51732,  64, 0.7) /* ResistSlash */
     , (51732,  65, 0.6) /* ResistPierce */
     , (51732,  66, 0.6) /* ResistBludgeon */
     , (51732,  67, 0.7) /* ResistFire */
     , (51732,  68, 0.4) /* ResistCold */
     , (51732,  69, 0.7) /* ResistAcid */
     , (51732,  70, 0.4) /* ResistElectric */
     , (51732,  80,   3) /* AiUseMagicDelay */
     , (51732, 104,  10) /* ObviousRadarRange */
     , (51732, 122,   2) /* AiAcquireHealth */
     , (51732, 125,   1) /* ResistHealthDrain */
     , (51732, 165, 1.0) /* ArmorModVsNether */
     , (51732, 166, 1.0) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51732,   1, 'Discorporate Rynthid of Consuming Torment') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51732,   1,   33561546) /* Setup */
     , (51732,   2,  150995487) /* MotionTable */
     , (51732,   3,  536870930) /* SoundTable */
     , (51732,   4,  805306381) /* CombatTable */
     , (51732,   6,   67111346) /* PaletteBase */
     , (51732,   7,  268437588) /* ClothingBase */
     , (51732,   8,  100667943) /* Icon */
     , (51732,  22,  872415443) /* PhysicsEffectTable */
     , (51732,  30,         87) /* PhysicsScript - BreatheLightning */
     , (51732,  35,       1000) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (51732, 8040, 758186017, 112.731, 10.5756, 132.029, -0.707107, 0, 0, -0.707107) /* PCAPRecordedLocation */
/* @teleloc 0x2D310021 [112.731003 10.575600 132.029007] -0.707107 0.000000 0.000000 -0.707107 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (51732,   1, 400, 0, 0) /* Strength */
     , (51732,   2, 100, 0, 0) /* Endurance */
     , (51732,   3, 300, 0, 0) /* Quickness */
     , (51732,   4, 300, 0, 0) /* Coordination */
     , (51732,   5, 250, 0, 0) /* Focus */
     , (51732,   6, 250, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (51732,   1,   100, 0, 0, 100) /* MaxHealth */
     , (51732,   3,  2600, 0, 0, 2600) /* MaxStamina */
     , (51732,   5,   250, 0, 0, 250) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (51732,  6, 0, 2, 0, 550, 0, 0) /* MeleeDefense        Trained */
     , (51732,  7, 0, 2, 0, 450, 0, 0) /* MissileDefense      Trained */
     , (51732, 15, 0, 2, 0, 360, 0, 0) /* MagicDefense        Trained */
     , (51732, 16, 0, 2, 0, 405, 0, 0) /* ManaConversion      Trained */
     , (51732, 31, 0, 2, 0, 405, 0, 0) /* CreatureEnchantment Trained */
     , (51732, 33, 0, 2, 0, 405, 0, 0) /* LifeMagic           Trained */
     , (51732, 34, 0, 2, 0, 405, 0, 0) /* WarMagic            Trained */
     , (51732, 41, 0, 2, 0, 450, 0, 0) /* TwoHandedCombat     Trained */
     , (51732, 43, 0, 2, 0, 405, 0, 0) /* VoidMagic           Trained */
     , (51732, 44, 0, 2, 0, 450, 0, 0) /* HeavyWeapons        Trained */
     , (51732, 45, 0, 2, 0, 450, 0, 0) /* LightWeapons        Trained */
     , (51732, 46, 0, 2, 0, 450, 0, 0) /* FinesseWeapons      Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (51732,  0, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (51732,  1, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (51732,  2, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (51732,  3, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (51732,  4, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (51732,  5, 64, 270, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 2,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0,    0, 0.12,    0) /* Hand */
     , (51732,  6, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (51732,  7, 64,  0,    0, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (51732,  8, 64, 270, 0.5, 550, 520, 520, 520, 520, 520, 520, 520,  600, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51732,  4483,   2.1)  /* Incantation of Lightning Vulnerability Other */;
