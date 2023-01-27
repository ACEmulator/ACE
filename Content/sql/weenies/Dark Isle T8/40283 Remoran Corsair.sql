DELETE FROM `weenie` WHERE `class_Id` = 40283;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40283, 'ace40283-remorancorsair', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40283,   1,         16) /* ItemType - Creature */
     , (40283,   2,         84) /* CreatureType - Remoran */
     , (40283,   3,         13) /* PaletteTemplate - Purple */
     , (40283,   6,         -1) /* ItemsCapacity */
     , (40283,   7,         -1) /* ContainersCapacity */
     , (40283,  16,          1) /* ItemUseable - No */
     , (40283,  25,        185) /* Level */
     , (40283,  27,          0) /* ArmorType - None */
     , (40283,  40,          2) /* CombatMode - Melee */
     , (40283,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (40283,  72,         34) /* FriendType - Moarsman */
     , (40283,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (40283, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40283, 146,     250000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40283,   1, True ) /* Stuck */
     , (40283,   6, True ) /* AiUsesMana */
     , (40283,  12, True ) /* ReportCollisions */
     , (40283,  14, True ) /* GravityStatus */
     , (40283,  19, True ) /* Attackable */
     , (40283,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40283,   1,       5) /* HeartbeatInterval */
     , (40283,   2,       0) /* HeartbeatTimestamp */
     , (40283,   3,     0.6) /* HealthRate */
     , (40283,   4,       3) /* StaminaRate */
     , (40283,   5,       1) /* ManaRate */
     , (40283,  12,       0) /* Shade */
     , (40283,  13,    0.95) /* ArmorModVsSlash */
     , (40283,  14,    0.75) /* ArmorModVsPierce */
     , (40283,  15,    0.65) /* ArmorModVsBludgeon */
     , (40283,  16,    0.95) /* ArmorModVsCold */
     , (40283,  17,    0.75) /* ArmorModVsFire */
     , (40283,  18,    0.95) /* ArmorModVsAcid */
     , (40283,  19,    0.85) /* ArmorModVsElectric */
     , (40283,  31,      20) /* VisualAwarenessRange */
     , (40283,  34,       1) /* PowerupTime */
     , (40283,  36,       1) /* ChargeSpeed */
     , (40283,  39,     1.1) /* DefaultScale */
     , (40283,  64,     0.1) /* ResistSlash */
     , (40283,  65,     0.3) /* ResistPierce */
     , (40283,  66,     0.3) /* ResistBludgeon */
     , (40283,  67,     0.1) /* ResistFire */
     , (40283,  68,     0.1) /* ResistCold */
     , (40283,  69,     0.1) /* ResistAcid */
     , (40283,  70,     0.1) /* ResistElectric */
     , (40283,  71,       1) /* ResistHealthBoost */
     , (40283,  72,       1) /* ResistStaminaDrain */
     , (40283,  73,       1) /* ResistStaminaBoost */
     , (40283,  74,       1) /* ResistManaDrain */
     , (40283,  75,       1) /* ResistManaBoost */
     , (40283,  80,       2) /* AiUseMagicDelay */
     , (40283, 104,      10) /* ObviousRadarRange */
     , (40283, 125,       1) /* ResistHealthDrain */
     , (40283, 166,     0.3) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40283,   1, 'Remoran Corsair') /* Name */
     , (40283,  45, 'KillTaskMGHRemoran') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40283,   1, 0x02001494) /* Setup */
     , (40283,   2, 0x0900018E) /* MotionTable */
     , (40283,   3, 0x200000BF) /* SoundTable */
     , (40283,   4, 0x3000001C) /* CombatTable */
     , (40283,   6, 0x04001EB6) /* PaletteBase */
     , (40283,   7, 0x10000636) /* ClothingBase */
     , (40283,   8, 0x06001221) /* Icon */
     , (40283,  22, 0x340000B6) /* PhysicsEffectTable */
     , (40283,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40283,   1, 400, 0, 0) /* Strength */
     , (40283,   2, 320, 0, 0) /* Endurance */
     , (40283,   3, 400, 0, 0) /* Quickness */
     , (40283,   4, 340, 0, 0) /* Coordination */
     , (40283,   5, 280, 0, 0) /* Focus */
     , (40283,   6, 340, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40283,   1,   450, 0, 0, 610) /* MaxHealth */
     , (40283,   3,   300, 0, 0, 620) /* MaxStamina */
     , (40283,   5,   300, 0, 0, 640) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40283,  6, 0, 3, 0, 180, 0, 0) /* MeleeDefense        Specialized */
     , (40283,  7, 0, 3, 0, 230, 0, 0) /* MissileDefense      Specialized */
     , (40283, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (40283, 15, 0, 3, 0, 230, 0, 0) /* MagicDefense        Specialized */
     , (40283, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (40283, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (40283, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (40283, 33, 0, 3, 0, 175, 0, 0) /* LifeMagic           Specialized */
     , (40283, 34, 0, 3, 0, 175, 0, 0) /* WarMagic            Specialized */
     , (40283, 45, 0, 3, 0, 228, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40283,  0,  2, 130,  0.5,  625,  594,  469,  406,  594,  469,  594,  531,    0, 1,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Head */
     , (40283,  5,  4, 130,  0.4,  625,  594,  469,  406,  594,  469,  594,  531,    0, 2,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4) /* Hand */
     , (40283, 16,  1,  0,    0,  625,  594,  469,  406,  594,  469,  594,  531,    0, 3,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Torso */
     , (40283, 17,  1, 130, 0.75,  625,  594,  469,  406,  594,  469,  594,  531,    0, 3,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Tail */
     , (40283, 19,  4,  0,    0,  625,  594,  469,  406,  594,  469,  594,  531,    0, 2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Leg */
     , (40283, 21,  4,  0,    0,  625,  594,  469,  406,  594,  469,  594,  531,    0, 2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Wings */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40283,  2174,   2.15)  /* Archer's Gift */
     , (40283,  2084,   2.18)  /* Belly of Lead */
     , (40283,  2068,   2.15)  /* Brittle Bones */
     , (40283,  2318,   2.15)  /* Gravity Well */
     , (40283,  2088,   2.15)  /* Senescence */
     , (40283,  2164,   2.03)  /* Swordsman's Gift */
     , (40283,  2054,   2.15)  /* Synaptic Misfire */
     , (40283,  2146,   2.02)  /* Evisceration */
     , (40283,  2132,   2.15)  /* The Spike */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40283, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40283, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40283, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40283, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
