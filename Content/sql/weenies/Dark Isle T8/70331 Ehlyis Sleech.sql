DELETE FROM `weenie` WHERE `class_Id` = 70331;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (70331, 'ace70331-ehlyissleech', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (70331,   1,         16) /* ItemType - Creature */
     , (70331,   2,         45) /* CreatureType - Niffis */
     , (70331,   3,         55) /* PaletteTemplate - BrownBlueDark */
     , (70331,   6,         -1) /* ItemsCapacity */
     , (70331,   7,         -1) /* ContainersCapacity */
     , (70331,  16,          1) /* ItemUseable - No */
     , (70331,  25,        200) /* Level */
     , (70331,  27,          0) /* ArmorType - None */
     , (70331,  40,          2) /* CombatMode - Melee */
     , (70331,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (70331,  81,          2) /* MaxGeneratedObjects */
     , (70331,  82,          2) /* InitGeneratedObjects */
     , (70331,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (70331, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (70331, 103,          3) /* GeneratorDestructionType - Kill */
     , (70331, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (70331, 140,          1) /* AiOptions - CanOpenDoors */
     , (70331, 146,     415000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (70331,   1, True ) /* Stuck */
     , (70331,   6, True ) /* AiUsesMana */
     , (70331,  11, False) /* IgnoreCollisions */
     , (70331,  12, True ) /* ReportCollisions */
     , (70331,  13, False) /* Ethereal */
     , (70331,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (70331,   1,       5) /* HeartbeatInterval */
     , (70331,   2,       0) /* HeartbeatTimestamp */
     , (70331,   3,     0.6) /* HealthRate */
     , (70331,   4,       3) /* StaminaRate */
     , (70331,   5,       1) /* ManaRate */
     , (70331,  12,     0.5) /* Shade */
     , (70331,  13,       1) /* ArmorModVsSlash */
     , (70331,  14,    0.55) /* ArmorModVsPierce */
     , (70331,  15,    0.45) /* ArmorModVsBludgeon */
     , (70331,  16,    0.95) /* ArmorModVsCold */
     , (70331,  17,    0.85) /* ArmorModVsFire */
     , (70331,  18,    0.95) /* ArmorModVsAcid */
     , (70331,  19,    0.85) /* ArmorModVsElectric */
     , (70331,  31,      20) /* VisualAwarenessRange */
     , (70331,  34,       1) /* PowerupTime */
     , (70331,  36,       1) /* ChargeSpeed */
     , (70331,  39,     1.1) /* DefaultScale */
     , (70331,  41,      60) /* RegenerationInterval */
     , (70331,  43,       4) /* GeneratorRadius */
     , (70331,  64,     0.6) /* ResistSlash */
     , (70331,  65,     0.6) /* ResistPierce */
     , (70331,  66,     0.7) /* ResistBludgeon */
     , (70331,  67,     0.5) /* ResistFire */
     , (70331,  68,     0.5) /* ResistCold */
     , (70331,  69,     0.5) /* ResistAcid */
     , (70331,  70,     0.5) /* ResistElectric */
     , (70331,  71,       1) /* ResistHealthBoost */
     , (70331,  72,       1) /* ResistStaminaDrain */
     , (70331,  73,       1) /* ResistStaminaBoost */
     , (70331,  74,       1) /* ResistManaDrain */
     , (70331,  75,       1) /* ResistManaBoost */
     , (70331,  80,       2) /* AiUseMagicDelay */
     , (70331, 104,      10) /* ObviousRadarRange */
     , (70331, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (70331,   1, 'Ehlyis Sleech') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (70331,   1, 0x020014A0) /* Setup */
     , (70331,   2, 0x09000193) /* MotionTable */
     , (70331,   3, 0x20000062) /* SoundTable */
     , (70331,   4, 0x3000002A) /* CombatTable */
     , (70331,   6, 0x04001EDC) /* PaletteBase */
     , (70331,   8, 0x06001DF1) /* Icon */
     , (70331,  22, 0x340000B8) /* PhysicsEffectTable */
     , (70331,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (70331,   1, 370, 0, 0) /* Strength */
     , (70331,   2, 370, 0, 0) /* Endurance */
     , (70331,   3, 330, 0, 0) /* Quickness */
     , (70331,   4, 350, 0, 0) /* Coordination */
     , (70331,   5, 440, 0, 0) /* Focus */
     , (70331,   6, 490, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (70331,   1, 12000, 0, 0, 12185) /* MaxHealth */
     , (70331,   3,  3400, 0, 0, 3770) /* MaxStamina */
     , (70331,   5,  1000, 0, 0, 1490) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (70331,  6, 0, 3, 0, 400, 0, 0) /* MeleeDefense        Specialized */
     , (70331,  7, 0, 3, 0, 300, 0, 0) /* MissileDefense      Specialized */
     , (70331, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (70331, 15, 0, 3, 0, 300, 0, 0) /* MagicDefense        Specialized */
     , (70331, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (70331, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (70331, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (70331, 33, 0, 3, 0, 350, 0, 0) /* LifeMagic           Specialized */
     , (70331, 34, 0, 3, 0, 320, 0, 0) /* WarMagic            Specialized */
     , (70331, 45, 0, 3, 0, 210, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (70331,  0,  4, 220, 0.75,  650,  650,  358,  293,  618,  553,  618,  553,    0, 1, 0.44,  0.3,    0,  0.4,  0.1,    0, 0.44,  0.3,    0,  0.4,  0.1,    0) /* Head */
     , (70331, 16,  4,  0,    0,  650,  650,  358,  293,  618,  553,  618,  553,    0, 2,  0.5, 0.48,  0.1,  0.5,  0.6,  0.1,  0.5, 0.48,  0.1,  0.5,  0.6, 0.22) /* Torso */
     , (70331, 21,  4,  0,    0,  650,  650,  358,  293,  618,  553,  618,  553,    0, 2,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0, 0.28) /* Wings */
     , (70331, 24,  4,  0,    0,  650,  650,  358,  293,  618,  553,  618,  553,    0, 2, 0.06, 0.22,  0.3,  0.1,  0.2,  0.3, 0.06, 0.22,  0.3,  0.1,  0.2, 0.22) /* UpperTentacle */
     , (70331, 25,  4, 220,  0.5,  650,  650,  358,  293,  618,  553,  618,  553,    0, 3,    0,    0,  0.3,    0,  0.1,  0.3,    0,    0,  0.3,    0,  0.1, 0.28) /* LowerTentacle */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (70331,  2074,   2.15)  /* Gossamer Flesh */
     , (70331,  2122,   2.15)  /* Disintegration */
     , (70331,  2162,   2.02)  /* Olthoi's Gift */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (70331, 9, 44470,  1, 0, 0, False) /* Create Corrupted Essence (44470) for ContainTreasure */
     , (70331, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (70331, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (70331, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (70331, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (70331, -1, 40285, 3600, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Listris Sleech (40285) (x2 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
