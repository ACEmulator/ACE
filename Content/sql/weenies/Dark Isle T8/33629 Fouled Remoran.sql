DELETE FROM `weenie` WHERE `class_Id` = 33629;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33629, 'ace33629-fouledremoran', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33629,   1,         16) /* ItemType - Creature */
     , (33629,   2,         84) /* CreatureType - Remoran */
     , (33629,   3,          3) /* PaletteTemplate - BluePurple */
     , (33629,   6,         -1) /* ItemsCapacity */
     , (33629,   7,         -1) /* ContainersCapacity */
     , (33629,  16,          1) /* ItemUseable - No */
     , (33629,  25,        185) /* Level */
     , (33629,  27,          0) /* ArmorType - None */
     , (33629,  40,          2) /* CombatMode - Melee */
     , (33629,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (33629,  72,         34) /* FriendType - Moarsman */
     , (33629,  81,          2) /* MaxGeneratedObjects */
     , (33629,  82,          2) /* InitGeneratedObjects */
     , (33629,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (33629, 103,          3) /* GeneratorDestructionType - Kill */
     , (33629, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33629, 146,     365000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33629,   1, True ) /* Stuck */
     , (33629,   6, True ) /* AiUsesMana */
     , (33629,  12, True ) /* ReportCollisions */
     , (33629,  14, True ) /* GravityStatus */
     , (33629,  19, True ) /* Attackable */
     , (33629,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33629,   1,       5) /* HeartbeatInterval */
     , (33629,   2,       0) /* HeartbeatTimestamp */
     , (33629,   3,     0.6) /* HealthRate */
     , (33629,   4,       3) /* StaminaRate */
     , (33629,   5,       1) /* ManaRate */
     , (33629,  12,       0) /* Shade */
     , (33629,  13,    0.95) /* ArmorModVsSlash */
     , (33629,  14,    0.55) /* ArmorModVsPierce */
     , (33629,  15,    0.45) /* ArmorModVsBludgeon */
     , (33629,  16,    0.95) /* ArmorModVsCold */
     , (33629,  17,    0.75) /* ArmorModVsFire */
     , (33629,  18,    0.95) /* ArmorModVsAcid */
     , (33629,  19,    0.85) /* ArmorModVsElectric */
     , (33629,  31,      19) /* VisualAwarenessRange */
     , (33629,  34,       1) /* PowerupTime */
     , (33629,  36,       1) /* ChargeSpeed */
     , (33629,  39,     1.3) /* DefaultScale */
     , (33629,  41,      60) /* RegenerationInterval */
     , (33629,  43,       4) /* GeneratorRadius */
     , (33629,  64,    0.58) /* ResistSlash */
     , (33629,  65,    0.68) /* ResistPierce */
     , (33629,  66,    0.68) /* ResistBludgeon */
     , (33629,  67,    0.36) /* ResistFire */
     , (33629,  68,    0.58) /* ResistCold */
     , (33629,  69,    0.58) /* ResistAcid */
     , (33629,  70,    0.58) /* ResistElectric */
     , (33629,  71,       1) /* ResistHealthBoost */
     , (33629,  72,       1) /* ResistStaminaDrain */
     , (33629,  73,       1) /* ResistStaminaBoost */
     , (33629,  74,       1) /* ResistManaDrain */
     , (33629,  75,       1) /* ResistManaBoost */
     , (33629,  80,       2) /* AiUseMagicDelay */
     , (33629, 104,      10) /* ObviousRadarRange */
     , (33629, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33629,   1, 'Fouled Remoran') /* Name */
     , (33629,  45, 'KillTaskMGHRemoran') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33629,   1, 0x02001494) /* Setup */
     , (33629,   2, 0x0900018E) /* MotionTable */
     , (33629,   3, 0x200000BF) /* SoundTable */
     , (33629,   4, 0x3000001C) /* CombatTable */
     , (33629,   6, 0x04001EB6) /* PaletteBase */
     , (33629,   7, 0x10000636) /* ClothingBase */
     , (33629,   8, 0x06001221) /* Icon */
     , (33629,  22, 0x340000B6) /* PhysicsEffectTable */
     , (33629,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33629,   1, 400, 0, 0) /* Strength */
     , (33629,   2, 320, 0, 0) /* Endurance */
     , (33629,   3, 400, 0, 0) /* Quickness */
     , (33629,   4, 340, 0, 0) /* Coordination */
     , (33629,   5, 280, 0, 0) /* Focus */
     , (33629,   6, 340, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33629,   1,  9000, 0, 0, 9160) /* MaxHealth */
     , (33629,   3,  3000, 0, 0, 3320) /* MaxStamina */
     , (33629,   5,  3000, 0, 0, 3340) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33629,  6, 0, 3, 0, 180, 0, 0) /* MeleeDefense        Specialized */
     , (33629,  7, 0, 3, 0, 230, 0, 0) /* MissileDefense      Specialized */
     , (33629, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (33629, 15, 0, 3, 0, 230, 0, 0) /* MagicDefense        Specialized */
     , (33629, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (33629, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (33629, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (33629, 33, 0, 3, 0, 175, 0, 0) /* LifeMagic           Specialized */
     , (33629, 34, 0, 3, 0, 175, 0, 0) /* WarMagic            Specialized */
     , (33629, 45, 0, 3, 0, 228, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33629,  0,  2, 130,  0.5,  425,  404,  234,  191,  404,  319,  404,  361,    0, 1,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Head */
     , (33629,  5,  4, 130,  0.4,  425,  404,  234,  191,  404,  319,  404,  361,    0, 2,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4) /* Hand */
     , (33629, 16,  1,  0,    0,  425,  404,  234,  191,  404,  319,  404,  361,    0, 3,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Torso */
     , (33629, 17,  1, 130, 0.75,  425,  404,  234,  191,  404,  319,  404,  361,    0, 3,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Tail */
     , (33629, 19,  4,  0,    0,  425,  404,  234,  191,  404,  319,  404,  361,    0, 2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Leg */
     , (33629, 21,  4,  0,    0,  425,  404,  234,  191,  404,  319,  404,  361,    0, 2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Wings */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33629,  2174,   2.15)  /* Archer's Gift */
     , (33629,  2084,   2.18)  /* Belly of Lead */
     , (33629,  2068,   2.15)  /* Brittle Bones */
     , (33629,  2318,   2.15)  /* Gravity Well */
     , (33629,  2088,   2.15)  /* Senescence */
     , (33629,  2164,   2.03)  /* Swordsman's Gift */
     , (33629,  2054,   2.15)  /* Synaptic Misfire */
     , (33629,  2146,   2.02)  /* Evisceration */
     , (33629,  2132,   2.15)  /* The Spike */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33629, 9, 44469,  1, 0, 0, False) /* Create Lesser Corrupted Essence (44469) for ContainTreasure */
     , (33629, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33629, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33629, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33629, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33629, -1, 40283, 3600, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Remoran Corsair (40283) (x2 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
