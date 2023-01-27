DELETE FROM `weenie` WHERE `class_Id` = 33636;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33636, 'ace33636-glissnalsleech', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33636,   1,         16) /* ItemType - Creature */
     , (33636,   2,         45) /* CreatureType - Niffis */
     , (33636,   3,         64) /* PaletteTemplate - OrangeBrown */
     , (33636,   6,         -1) /* ItemsCapacity */
     , (33636,   7,         -1) /* ContainersCapacity */
     , (33636,  16,          1) /* ItemUseable - No */
     , (33636,  25,        185) /* Level */
     , (33636,  27,          0) /* ArmorType - None */
     , (33636,  40,          2) /* CombatMode - Melee */
     , (33636,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (33636,  81,          2) /* MaxGeneratedObjects */
     , (33636,  82,          2) /* InitGeneratedObjects */
     , (33636,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (33636, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (33636, 103,          3) /* GeneratorDestructionType - Kill */
     , (33636, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33636, 140,          1) /* AiOptions - CanOpenDoors */
     , (33636, 146,     388000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33636,   1, True ) /* Stuck */
     , (33636,   6, True ) /* AiUsesMana */
     , (33636,  11, False) /* IgnoreCollisions */
     , (33636,  12, True ) /* ReportCollisions */
     , (33636,  13, False) /* Ethereal */
     , (33636,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33636,   1,       5) /* HeartbeatInterval */
     , (33636,   2,       0) /* HeartbeatTimestamp */
     , (33636,   3,     0.6) /* HealthRate */
     , (33636,   4,       3) /* StaminaRate */
     , (33636,   5,       1) /* ManaRate */
     , (33636,  12,     0.5) /* Shade */
     , (33636,  13,       1) /* ArmorModVsSlash */
     , (33636,  14,    0.55) /* ArmorModVsPierce */
     , (33636,  15,    0.45) /* ArmorModVsBludgeon */
     , (33636,  16,    0.95) /* ArmorModVsCold */
     , (33636,  17,    0.85) /* ArmorModVsFire */
     , (33636,  18,    0.95) /* ArmorModVsAcid */
     , (33636,  19,    0.85) /* ArmorModVsElectric */
     , (33636,  31,      24) /* VisualAwarenessRange */
     , (33636,  34,       1) /* PowerupTime */
     , (33636,  36,       1) /* ChargeSpeed */
     , (33636,  39,     1.1) /* DefaultScale */
     , (33636,  41,      60) /* RegenerationInterval */
     , (33636,  43,       4) /* GeneratorRadius */
     , (33636,  64,     0.7) /* ResistSlash */
     , (33636,  65,     0.7) /* ResistPierce */
     , (33636,  66,     0.8) /* ResistBludgeon */
     , (33636,  67,     0.5) /* ResistFire */
     , (33636,  68,     0.5) /* ResistCold */
     , (33636,  69,     0.5) /* ResistAcid */
     , (33636,  70,     0.5) /* ResistElectric */
     , (33636,  71,       1) /* ResistHealthBoost */
     , (33636,  72,       1) /* ResistStaminaDrain */
     , (33636,  73,       1) /* ResistStaminaBoost */
     , (33636,  74,       1) /* ResistManaDrain */
     , (33636,  75,       1) /* ResistManaBoost */
     , (33636,  80,       2) /* AiUseMagicDelay */
     , (33636, 104,      10) /* ObviousRadarRange */
     , (33636, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33636,   1, 'Glissnal Sleech') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33636,   1, 0x020014A0) /* Setup */
     , (33636,   2, 0x09000193) /* MotionTable */
     , (33636,   3, 0x20000062) /* SoundTable */
     , (33636,   4, 0x3000002A) /* CombatTable */
     , (33636,   6, 0x04001EDC) /* PaletteBase */
     , (33636,   7, 0x10000639) /* ClothingBase */
     , (33636,   8, 0x06001DF1) /* Icon */
     , (33636,  22, 0x340000B8) /* PhysicsEffectTable */
     , (33636,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33636,   1, 360, 0, 0) /* Strength */
     , (33636,   2, 360, 0, 0) /* Endurance */
     , (33636,   3, 320, 0, 0) /* Quickness */
     , (33636,   4, 340, 0, 0) /* Coordination */
     , (33636,   5, 430, 0, 0) /* Focus */
     , (33636,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33636,   1,  9000, 0, 0, 9180) /* MaxHealth */
     , (33636,   3,  3000, 0, 0, 3360) /* MaxStamina */
     , (33636,   5,  1000, 0, 0, 1480) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33636,  6, 0, 3, 0, 400, 0, 0) /* MeleeDefense        Specialized */
     , (33636,  7, 0, 3, 0, 300, 0, 0) /* MissileDefense      Specialized */
     , (33636, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (33636, 15, 0, 3, 0, 300, 0, 0) /* MagicDefense        Specialized */
     , (33636, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (33636, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (33636, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (33636, 33, 0, 3, 0, 350, 0, 0) /* LifeMagic           Specialized */
     , (33636, 34, 0, 3, 0, 320, 0, 0) /* WarMagic            Specialized */
     , (33636, 45, 0, 3, 0, 210, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33636,  0,  4, 220, 0.75,  650,  650,  358,  293,  618,  553,  618,  553,    0, 1, 0.44,  0.3,    0,  0.4,  0.1,    0, 0.44,  0.3,    0,  0.4,  0.1,    0) /* Head */
     , (33636, 16,  4,  0,    0,  650,  650,  358,  293,  618,  553,  618,  553,    0, 2,  0.5, 0.48,  0.1,  0.5,  0.6,  0.1,  0.5, 0.48,  0.1,  0.5,  0.6, 0.22) /* Torso */
     , (33636, 21,  4,  0,    0,  650,  650,  358,  293,  618,  553,  618,  553,    0, 2,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0, 0.28) /* Wings */
     , (33636, 24,  4,  0,    0,  650,  650,  358,  293,  618,  553,  618,  553,    0, 2, 0.06, 0.22,  0.3,  0.1,  0.2,  0.3, 0.06, 0.22,  0.3,  0.1,  0.2, 0.22) /* UpperTentacle */
     , (33636, 25,  4, 220,  0.5,  650,  650,  358,  293,  618,  553,  618,  553,    0, 3,    0,    0,  0.3,    0,  0.1,  0.3,    0,    0,  0.3,    0,  0.1, 0.28) /* LowerTentacle */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33636,  2074,   2.15)  /* Gossamer Flesh */
     , (33636,  2122,   2.15)  /* Disintegration */
     , (33636,  2162,   2.02)  /* Olthoi's Gift */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33636, 9, 44469,  1, 0, 0, False) /* Create Lesser Corrupted Essence (44469) for ContainTreasure */
     , (33636, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33636, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33636, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33636, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33636, -1, 40286, 3600, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Parfal Sleech (40286) (x2 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
