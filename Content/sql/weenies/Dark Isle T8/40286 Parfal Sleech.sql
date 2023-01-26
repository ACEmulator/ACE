DELETE FROM `weenie` WHERE `class_Id` = 40286;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40286, 'ace40286-parfalsleech', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40286,   1,         16) /* ItemType - Creature */
     , (40286,   2,         45) /* CreatureType - Niffis */
     , (40286,   3,         82) /* PaletteTemplate - PinkPurple */
     , (40286,   6,         -1) /* ItemsCapacity */
     , (40286,   7,         -1) /* ContainersCapacity */
     , (40286,  16,          1) /* ItemUseable - No */
     , (40286,  25,        185) /* Level */
     , (40286,  27,          0) /* ArmorType - None */
     , (40286,  40,          2) /* CombatMode - Melee */
     , (40286,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (40286,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (40286, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (40286, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40286, 140,          1) /* AiOptions - CanOpenDoors */
     , (40286, 146,     290000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40286,   1, True ) /* Stuck */
     , (40286,   6, True ) /* AiUsesMana */
     , (40286,  11, False) /* IgnoreCollisions */
     , (40286,  12, True ) /* ReportCollisions */
     , (40286,  13, False) /* Ethereal */
     , (40286,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40286,   1,       5) /* HeartbeatInterval */
     , (40286,   2,       0) /* HeartbeatTimestamp */
     , (40286,   3,     0.6) /* HealthRate */
     , (40286,   4,       3) /* StaminaRate */
     , (40286,   5,       1) /* ManaRate */
     , (40286,  12,     0.5) /* Shade */
     , (40286,  13,       1) /* ArmorModVsSlash */
     , (40286,  14,    0.95) /* ArmorModVsPierce */
     , (40286,  15,    0.95) /* ArmorModVsBludgeon */
     , (40286,  16,       1) /* ArmorModVsCold */
     , (40286,  17,       1) /* ArmorModVsFire */
     , (40286,  18,       1) /* ArmorModVsAcid */
     , (40286,  19,       1) /* ArmorModVsElectric */
     , (40286,  31,      18) /* VisualAwarenessRange */
     , (40286,  34,       1) /* PowerupTime */
     , (40286,  36,       1) /* ChargeSpeed */
     , (40286,  39,     0.8) /* DefaultScale */
     , (40286,  64,     0.6) /* ResistSlash */
     , (40286,  65,     0.6) /* ResistPierce */
     , (40286,  66,     0.7) /* ResistBludgeon */
     , (40286,  67,     0.5) /* ResistFire */
     , (40286,  68,     0.5) /* ResistCold */
     , (40286,  69,     0.5) /* ResistAcid */
     , (40286,  70,     0.5) /* ResistElectric */
     , (40286,  71,       1) /* ResistHealthBoost */
     , (40286,  72,       1) /* ResistStaminaDrain */
     , (40286,  73,       1) /* ResistStaminaBoost */
     , (40286,  74,       1) /* ResistManaDrain */
     , (40286,  75,       1) /* ResistManaBoost */
     , (40286,  80,       2) /* AiUseMagicDelay */
     , (40286, 104,      10) /* ObviousRadarRange */
     , (40286, 125,       1) /* ResistHealthDrain */
     , (40286, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40286,   1, 'Parfal Sleech') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40286,   1, 0x020014A0) /* Setup */
     , (40286,   2, 0x09000193) /* MotionTable */
     , (40286,   3, 0x20000062) /* SoundTable */
     , (40286,   4, 0x3000002A) /* CombatTable */
     , (40286,   6, 0x04001EDC) /* PaletteBase */
     , (40286,   7, 0x10000639) /* ClothingBase */
     , (40286,   8, 0x06001DF1) /* Icon */
     , (40286,  22, 0x340000B8) /* PhysicsEffectTable */
     , (40286,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40286,   1, 360, 0, 0) /* Strength */
     , (40286,   2, 360, 0, 0) /* Endurance */
     , (40286,   3, 320, 0, 0) /* Quickness */
     , (40286,   4, 340, 0, 0) /* Coordination */
     , (40286,   5, 430, 0, 0) /* Focus */
     , (40286,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40286,   1,   435, 0, 0, 615) /* MaxHealth */
     , (40286,   3,   500, 0, 0, 860) /* MaxStamina */
     , (40286,   5,  1000, 0, 0, 1480) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40286,  6, 0, 3, 0, 380, 0, 0) /* MeleeDefense        Specialized */
     , (40286,  7, 0, 3, 0, 290, 0, 0) /* MissileDefense      Specialized */
     , (40286, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (40286, 15, 0, 3, 0, 275, 0, 0) /* MagicDefense        Specialized */
     , (40286, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (40286, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (40286, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (40286, 33, 0, 3, 0, 300, 0, 0) /* LifeMagic           Specialized */
     , (40286, 34, 0, 3, 0, 300, 0, 0) /* WarMagic            Specialized */
     , (40286, 45, 0, 3, 0, 210, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40286,  0,  4, 200, 0.75,  650,  650,  618,  618,  650,  650,  650,  650,    0, 1, 0.44,  0.3,    0,  0.4,  0.1,    0, 0.44,  0.3,    0,  0.4,  0.1,    0) /* Head */
     , (40286, 16,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2,  0.5, 0.48,  0.1,  0.5,  0.6,  0.1,  0.5, 0.48,  0.1,  0.5,  0.6, 0.22) /* Torso */
     , (40286, 21,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0, 0.28) /* Wings */
     , (40286, 24,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2, 0.06, 0.22,  0.3,  0.1,  0.2,  0.3, 0.06, 0.22,  0.3,  0.1,  0.2, 0.22) /* UpperTentacle */
     , (40286, 25,  4, 200,  0.5,  650,  650,  618,  618,  650,  650,  650,  650,    0, 3,    0,    0,  0.3,    0,  0.1,  0.3,    0,    0,  0.3,    0,  0.1, 0.28) /* LowerTentacle */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40286,  2074,   2.15)  /* Gossamer Flesh */
     , (40286,  2122,   2.15)  /* Disintegration */
     , (40286,  2162,   2.02)  /* Olthoi's Gift */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40286, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40286, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40286, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40286, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
