DELETE FROM `weenie` WHERE `class_Id` = 70082;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (70082, 'ace70082-darkremoran', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (70082,   1,         16) /* ItemType - Creature */
     , (70082,   2,         84) /* CreatureType - Remoran */
     , (70082,   3,         39) /* PaletteTemplate - Black */
     , (70082,   6,         -1) /* ItemsCapacity */
     , (70082,   7,         -1) /* ContainersCapacity */
     , (70082,  16,          1) /* ItemUseable - No */
     , (70082,  25,        200) /* Level */
     , (70082,  27,          0) /* ArmorType - None */
     , (70082,  40,          2) /* CombatMode - Melee */
     , (70082,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (70082,  72,         34) /* FriendType - Moarsman */
     , (70082,  81,          2) /* MaxGeneratedObjects */
     , (70082,  82,          2) /* InitGeneratedObjects */
     , (70082,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (70082, 103,          3) /* GeneratorDestructionType - Kill */
     , (70082, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (70082, 146,     395000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (70082,   1, True ) /* Stuck */
     , (70082,   6, True ) /* AiUsesMana */
     , (70082,  12, True ) /* ReportCollisions */
     , (70082,  14, True ) /* GravityStatus */
     , (70082,  19, True ) /* Attackable */
     , (70082,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (70082,   1,       5) /* HeartbeatInterval */
     , (70082,   2,       0) /* HeartbeatTimestamp */
     , (70082,   3,     0.6) /* HealthRate */
     , (70082,   4,       3) /* StaminaRate */
     , (70082,   5,       1) /* ManaRate */
     , (70082,  12,       0) /* Shade */
     , (70082,  13,    0.95) /* ArmorModVsSlash */
     , (70082,  14,    0.55) /* ArmorModVsPierce */
     , (70082,  15,    0.45) /* ArmorModVsBludgeon */
     , (70082,  16,    0.95) /* ArmorModVsCold */
     , (70082,  17,    0.75) /* ArmorModVsFire */
     , (70082,  18,    0.95) /* ArmorModVsAcid */
     , (70082,  19,    0.85) /* ArmorModVsElectric */
     , (70082,  31,      24) /* VisualAwarenessRange */
     , (70082,  34,       1) /* PowerupTime */
     , (70082,  36,       1) /* ChargeSpeed */
     , (70082,  39,     1.5) /* DefaultScale */
     , (70082,  41,      60) /* RegenerationInterval */
     , (70082,  43,       4) /* GeneratorRadius */
     , (70082,  64,    0.58) /* ResistSlash */
     , (70082,  65,    0.68) /* ResistPierce */
     , (70082,  66,    0.68) /* ResistBludgeon */
     , (70082,  67,    0.36) /* ResistFire */
     , (70082,  68,    0.58) /* ResistCold */
     , (70082,  69,    0.58) /* ResistAcid */
     , (70082,  70,    0.58) /* ResistElectric */
     , (70082,  71,       1) /* ResistHealthBoost */
     , (70082,  72,       1) /* ResistStaminaDrain */
     , (70082,  73,       1) /* ResistStaminaBoost */
     , (70082,  74,       1) /* ResistManaDrain */
     , (70082,  75,       1) /* ResistManaBoost */
     , (70082,  80,       2) /* AiUseMagicDelay */
     , (70082, 104,      10) /* ObviousRadarRange */
     , (70082, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (70082,   1, 'Dark Remoran') /* Name */
     , (70082,  45, 'KillTaskMGHRemoran') /* KillQuest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (70082,   1, 0x02001494) /* Setup */
     , (70082,   2, 0x0900018E) /* MotionTable */
     , (70082,   3, 0x200000BF) /* SoundTable */
     , (70082,   4, 0x3000001C) /* CombatTable */
     , (70082,   6, 0x04001EB6) /* PaletteBase */
     , (70082,   7, 0x10000636) /* ClothingBase */
     , (70082,   8, 0x06001221) /* Icon */
     , (70082,  22, 0x340000B6) /* PhysicsEffectTable */
     , (70082,  35,  2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (70082,   1, 400, 0, 0) /* Strength */
     , (70082,   2, 320, 0, 0) /* Endurance */
     , (70082,   3, 400, 0, 0) /* Quickness */
     , (70082,   4, 340, 0, 0) /* Coordination */
     , (70082,   5, 280, 0, 0) /* Focus */
     , (70082,   6, 340, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (70082,   1,  9000, 0, 0, 9160) /* MaxHealth */
     , (70082,   3,  3000, 0, 0, 3320) /* MaxStamina */
     , (70082,   5,  3000, 0, 0, 3340) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (70082,  6, 0, 3, 0, 180, 0, 0) /* MeleeDefense        Specialized */
     , (70082,  7, 0, 3, 0, 230, 0, 0) /* MissileDefense      Specialized */
     , (70082, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (70082, 15, 0, 3, 0, 230, 0, 0) /* MagicDefense        Specialized */
     , (70082, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (70082, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (70082, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (70082, 33, 0, 3, 0, 175, 0, 0) /* LifeMagic           Specialized */
     , (70082, 34, 0, 3, 0, 175, 0, 0) /* WarMagic            Specialized */
     , (70082, 45, 0, 3, 0, 228, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (70082,  0,  2, 130,  0.5,  425,  404,  234,  191,  404,  319,  404,  361,    0, 1,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Head */
     , (70082,  5,  4, 130,  0.4,  425,  404,  234,  191,  404,  319,  404,  361,    0, 2,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4,  0.4) /* Hand */
     , (70082, 16,  1,  0,    0,  425,  404,  234,  191,  404,  319,  404,  361,    0, 3,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Torso */
     , (70082, 17,  1, 130, 0.75,  425,  404,  234,  191,  404,  319,  404,  361,    0, 3,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Tail */
     , (70082, 19,  4,  0,    0,  425,  404,  234,  191,  404,  319,  404,  361,    0, 2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Leg */
     , (70082, 21,  4,  0,    0,  425,  404,  234,  191,  404,  319,  404,  361,    0, 2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2,  0.2) /* Wings */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (70082,  2174,   2.15)  /* Archer's Gift */
     , (70082,  2084,   2.18)  /* Belly of Lead */
     , (70082,  2068,   2.15)  /* Brittle Bones */
     , (70082,  2318,   2.15)  /* Gravity Well */
     , (70082,  2088,   2.15)  /* Senescence */
     , (70082,  2164,   2.03)  /* Swordsman's Gift */
     , (70082,  2054,   2.15)  /* Synaptic Misfire */
     , (70082,  2146,   2.02)  /* Evisceration */
     , (70082,  2132,   2.15)  /* The Spike */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (70082, 9, 44470,  1, 0, 0, False) /* Create Corrupted Essence (44470) for ContainTreasure */
     , (70082, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (70082, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (70082, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (70082, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (70082, -1, 40284, 3600, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Horrid Remoran (40284) (x2 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
