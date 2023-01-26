DELETE FROM `weenie` WHERE `class_Id` = 38594;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (38594, 'ace38594-falatacotbloodprophetess', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (38594,   1,         16) /* ItemType - Creature */
     , (38594,   2,         14) /* CreatureType - Undead */
     , (38594,   3,         69) /* PaletteTemplate - YellowSlime */
     , (38594,   6,         -1) /* ItemsCapacity */
     , (38594,   7,         -1) /* ContainersCapacity */
     , (38594,  16,          1) /* ItemUseable - No */
     , (38594,  25,        200) /* Level */
     , (38594,  27,          0) /* ArmorType - None */
     , (38594,  40,          1) /* CombatMode - NonCombat */
     , (38594,  68,          3) /* TargetingTactic - Random, Focused */
     , (38594,  81,          2) /* MaxGeneratedObjects */
     , (38594,  82,          2) /* InitGeneratedObjects */
     , (38594,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (38594, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (38594, 103,          3) /* GeneratorDestructionType - Kill */
     , (38594, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (38594, 140,          1) /* AiOptions - CanOpenDoors */
     , (38594, 146,    1100000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (38594,   1, True ) /* Stuck */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (38594,   1,       5) /* HeartbeatInterval */
     , (38594,   2,       0) /* HeartbeatTimestamp */
     , (38594,   3,     0.8) /* HealthRate */
     , (38594,   4,     0.5) /* StaminaRate */
     , (38594,   5,       2) /* ManaRate */
     , (38594,  13,       1) /* ArmorModVsSlash */
     , (38594,  14,     1.3) /* ArmorModVsPierce */
     , (38594,  15,       1) /* ArmorModVsBludgeon */
     , (38594,  16,     1.3) /* ArmorModVsCold */
     , (38594,  17,       1) /* ArmorModVsFire */
     , (38594,  18,       1) /* ArmorModVsAcid */
     , (38594,  19,     1.2) /* ArmorModVsElectric */
     , (38594,  31,      17) /* VisualAwarenessRange */
     , (38594,  34,       1) /* PowerupTime */
     , (38594,  36,       1) /* ChargeSpeed */
     , (38594,  39,     1.3) /* DefaultScale */
     , (38594,  43,       4) /* GeneratorRadius */
     , (38594,  64,     0.5) /* ResistSlash */
     , (38594,  65,    0.45) /* ResistPierce */
     , (38594,  66,    0.65) /* ResistBludgeon */
     , (38594,  67,    0.65) /* ResistFire */
     , (38594,  68,    0.55) /* ResistCold */
     , (38594,  69,    0.55) /* ResistAcid */
     , (38594,  70,     0.5) /* ResistElectric */
     , (38594,  71,       1) /* ResistHealthBoost */
     , (38594,  72,       1) /* ResistStaminaDrain */
     , (38594,  73,       1) /* ResistStaminaBoost */
     , (38594,  74,       1) /* ResistManaDrain */
     , (38594,  75,       1) /* ResistManaBoost */
     , (38594,  80,       3) /* AiUseMagicDelay */
     , (38594, 104,      10) /* ObviousRadarRange */
     , (38594, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (38594,   1, 'Falatacot Blood Prophetess') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (38594,   1, 0x02000FA5) /* Setup */
     , (38594,   2, 0x09000017) /* MotionTable */
     , (38594,   3, 0x20000016) /* SoundTable */
     , (38594,   4, 0x30000000) /* CombatTable */
     , (38594,   6, 0x040015F0) /* PaletteBase */
     , (38594,   7, 0x100004C0) /* ClothingBase */
     , (38594,   8, 0x06002CF5) /* Icon */
     , (38594,  22, 0x34000028) /* PhysicsEffectTable */
     , (38594,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (38594,   1, 400, 0, 0) /* Strength */
     , (38594,   2, 420, 0, 0) /* Endurance */
     , (38594,   3, 360, 0, 0) /* Quickness */
     , (38594,   4, 360, 0, 0) /* Coordination */
     , (38594,   5, 490, 0, 0) /* Focus */
     , (38594,   6, 490, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (38594,   1,  2800, 0, 0, 3010) /* MaxHealth */
     , (38594,   3,  1500, 0, 0, 1920) /* MaxStamina */
     , (38594,   5,  3080, 0, 0, 3570) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (38594,  6, 0, 3, 0, 375, 0, 0) /* MeleeDefense        Specialized */
     , (38594,  7, 0, 3, 0, 405, 0, 0) /* MissileDefense      Specialized */
     , (38594, 14, 0, 3, 0, 240, 0, 0) /* ArcaneLore          Specialized */
     , (38594, 15, 0, 3, 0, 320, 0, 0) /* MagicDefense        Specialized */
     , (38594, 20, 0, 3, 0,  90, 0, 0) /* Deception           Specialized */
     , (38594, 31, 0, 3, 0, 275, 0, 0) /* CreatureEnchantment Specialized */
     , (38594, 33, 0, 3, 0, 275, 0, 0) /* LifeMagic           Specialized */
     , (38594, 34, 0, 3, 0, 275, 0, 0) /* WarMagic            Specialized */
     , (38594, 44, 0, 3, 0, 375, 0, 0) /* HeavyWeapons        Specialized */
     , (38594, 45, 0, 3, 0, 375, 0, 0) /* LightWeapons        Specialized */
     , (38594, 46, 0, 3, 0, 375, 0, 0) /* FinesseWeapons      Specialized */
     , (38594, 47, 0, 3, 0, 175, 0, 0) /* MissileWeapons      Specialized */
     , (38594, 48, 0, 3, 0, 300, 0, 0) /* Shield              Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (38594,  0,  4,  0,    0,  425,  425,  553,  425,  553,  425,  425,  510,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (38594,  1,  4,  0,    0,  425,  425,  553,  425,  553,  425,  425,  510,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (38594,  2,  4,  0,    0,  425,  425,  553,  425,  553,  425,  425,  510,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (38594,  3,  4,  0,    0,  425,  425,  553,  425,  553,  425,  425,  510,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (38594,  4,  4,  0,    0,  425,  425,  553,  425,  553,  425,  425,  510,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (38594,  5,  4,  5, 0.75,  425,  425,  553,  425,  553,  425,  425,  510,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (38594,  6,  4,  0,    0,  425,  425,  553,  425,  553,  425,  425,  510,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (38594,  7,  4,  0,    0,  425,  425,  553,  425,  553,  425,  425,  510,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (38594,  8,  4,  5, 0.75,  425,  425,  553,  425,  553,  425,  425,  510,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (38594,  2144,   2.02)  /* Crushing Shame */
     , (38594,  2170,   2.02)  /* Inferno's Gift */
     , (38594,  3882,   2.02)  /* Incendiary Ring */
     , (38594,  4441,   2.02)  /* Incantation of Flame Volley */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (38594, 2, 48101,  1, 0, 0, False) /* Create Sickle (48101) for Wield */
     , (38594, 2, 48103,  1, 0, 0, False) /* Create Sickle (48103) for Wield */
     , (38594, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (38594, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (38594, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (38594, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (38594, 9, 38614,  1, 0, 1, False) /* Create Falatacot Battle Report (38614) for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (38594, -1, 34973, 4, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Falatacot Consort (34973) (x2 up to max of 2) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
