DELETE FROM `weenie` WHERE `class_Id` = 33734;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33734, 'ace33734-soddenundeadruschk', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33734,   1,         16) /* ItemType - Creature */
     , (33734,   2,         14) /* CreatureType - Undead */
     , (33734,   6,         -1) /* ItemsCapacity */
     , (33734,   7,         -1) /* ContainersCapacity */
     , (33734,  16,          1) /* ItemUseable - No */
     , (33734,  25,        200) /* Level */
     , (33734,  27,          0) /* ArmorType - None */
     , (33734,  40,          2) /* CombatMode - Melee */
     , (33734,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (33734,  81,          1) /* MaxGeneratedObjects */
     , (33734,  82,          0) /* InitGeneratedObjects */
     , (33734,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (33734, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (33734, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33734, 140,          1) /* AiOptions - CanOpenDoors */
     , (33734, 146,     200000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33734,   1, True ) /* Stuck */
     , (33734,  11, False) /* IgnoreCollisions */
     , (33734,  12, True ) /* ReportCollisions */
     , (33734,  13, False) /* Ethereal */
     , (33734,  14, True ) /* GravityStatus */
     , (33734,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33734,   1,       5) /* HeartbeatInterval */
     , (33734,   2,       0) /* HeartbeatTimestamp */
     , (33734,   3,    0.15) /* HealthRate */
     , (33734,   4,       4) /* StaminaRate */
     , (33734,   5,     1.5) /* ManaRate */
     , (33734,  12,       0) /* Shade */
     , (33734,  13,     0.9) /* ArmorModVsSlash */
     , (33734,  14,     0.6) /* ArmorModVsPierce */
     , (33734,  15,     1.1) /* ArmorModVsBludgeon */
     , (33734,  16,     0.8) /* ArmorModVsCold */
     , (33734,  17,    0.55) /* ArmorModVsFire */
     , (33734,  18,       1) /* ArmorModVsAcid */
     , (33734,  19,     0.8) /* ArmorModVsElectric */
     , (33734,  31,      17) /* VisualAwarenessRange */
     , (33734,  34,       1) /* PowerupTime */
     , (33734,  36,       1) /* ChargeSpeed */
     , (33734,  39,       1) /* DefaultScale */
     , (33734,  43,       4) /* GeneratorRadius */
     , (33734,  64,     0.1) /* ResistSlash */
     , (33734,  65,     0.1) /* ResistPierce */
     , (33734,  66,     0.3) /* ResistBludgeon */
     , (33734,  67,     0.3) /* ResistFire */
     , (33734,  68,     0.1) /* ResistCold */
     , (33734,  69,     0.2) /* ResistAcid */
     , (33734,  70,     0.1) /* ResistElectric */
     , (33734,  71,       1) /* ResistHealthBoost */
     , (33734,  72,     0.5) /* ResistStaminaDrain */
     , (33734,  73,       1) /* ResistStaminaBoost */
     , (33734,  74,     0.5) /* ResistManaDrain */
     , (33734,  75,       1) /* ResistManaBoost */
     , (33734, 104,      10) /* ObviousRadarRange */
     , (33734, 125,     0.5) /* ResistHealthDrain */
     , (33734, 166,     0.2) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33734,   1, 'Sodden Undead Ruschk') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33734,   1, 0x020013D3) /* Setup */
     , (33734,   2, 0x09000007) /* MotionTable */
     , (33734,   3, 0x200000BD) /* SoundTable */
     , (33734,   4, 0x30000004) /* CombatTable */
     , (33734,   8, 0x060036FD) /* Icon */
     , (33734,  22, 0x34000084) /* PhysicsEffectTable */
     , (33734,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33734,   1, 330, 0, 0) /* Strength */
     , (33734,   2, 260, 0, 0) /* Endurance */
     , (33734,   3, 220, 0, 0) /* Quickness */
     , (33734,   4, 260, 0, 0) /* Coordination */
     , (33734,   5, 215, 0, 0) /* Focus */
     , (33734,   6, 215, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33734,   1,   850, 0, 0, 980) /* MaxHealth */
     , (33734,   3,  1000, 0, 0, 1260) /* MaxStamina */
     , (33734,   5,   200, 0, 0, 415) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33734,  6, 0, 3, 0, 375, 0, 0) /* MeleeDefense        Specialized */
     , (33734,  7, 0, 3, 0, 370, 0, 0) /* MissileDefense      Specialized */
     , (33734, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (33734, 15, 0, 3, 0, 400, 0, 0) /* MagicDefense        Specialized */
     , (33734, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (33734, 31, 0, 3, 0, 275, 0, 0) /* CreatureEnchantment Specialized */
     , (33734, 32, 0, 3, 0, 275, 0, 0) /* ItemEnchantment     Specialized */
     , (33734, 33, 0, 3, 0, 275, 0, 0) /* LifeMagic           Specialized */
     , (33734, 34, 0, 3, 0, 290, 0, 0) /* WarMagic            Specialized */
     , (33734, 45, 0, 3, 0, 385, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33734,  0,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (33734,  1,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (33734,  2,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (33734,  3,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (33734,  4,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (33734,  5,  4, 60,  0.5,  450,  405,  270,  495,  360,  248,  450,  360,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (33734,  6,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (33734,  7,  4,  0,    0,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (33734,  8,  4, 50,  0.4,  450,  405,  270,  495,  360,  248,  450,  360,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33734,  2074,   2.02)  /* Gossamer Flesh */
     , (33734,  2136,   2.02)  /* Icy Torment */
     , (33734,  2168,   2.02)  /* Gelidite's Gift */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (33734,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33734, 2, 48585,  1, 0, 0, False) /* Create Frozen Dagger (48585) for Wield */
     , (33734, 2, 48588,  1, 0, 0, False) /* Create Glacial Blade (48588) for Wield */
     , (33734, 2, 48584,  1, 0, 0, False) /* Create Icy Club (48584) for Wield */
     , (33734, 2, 48587,  1, 0, 0, False) /* Create Frigid Splinter (48587) for Wield */
     , (33734, 2, 48586,  1, 0, 0, False) /* Create Ice Shard (48586) for Wield */
     , (33734, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33734, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33734, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33734, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33734, -1, 33641, 0, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Sodden Ruschk Chieftain (33641) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
