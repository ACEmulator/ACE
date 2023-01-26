DELETE FROM `weenie` WHERE `class_Id` = 33739;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33739, 'ace33739-parfalsleech', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33739,   1,         16) /* ItemType - Creature */
     , (33739,   2,         45) /* CreatureType - Niffis */
     , (33739,   3,         82) /* PaletteTemplate - PinkPurple */
     , (33739,   6,         -1) /* ItemsCapacity */
     , (33739,   7,         -1) /* ContainersCapacity */
     , (33739,  16,          1) /* ItemUseable - No */
     , (33739,  25,        185) /* Level */
     , (33739,  27,          0) /* ArmorType - None */
     , (33739,  40,          2) /* CombatMode - Melee */
     , (33739,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (33739,  81,          1) /* MaxGeneratedObjects */
     , (33739,  82,          0) /* InitGeneratedObjects */
     , (33739,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (33739, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (33739, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33739, 140,          1) /* AiOptions - CanOpenDoors */
     , (33739, 146,     290000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33739,   1, True ) /* Stuck */
     , (33739,   6, True ) /* AiUsesMana */
     , (33739,  11, False) /* IgnoreCollisions */
     , (33739,  12, True ) /* ReportCollisions */
     , (33739,  13, False) /* Ethereal */
     , (33739,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33739,   1,       5) /* HeartbeatInterval */
     , (33739,   2,       0) /* HeartbeatTimestamp */
     , (33739,   3,     0.6) /* HealthRate */
     , (33739,   4,       3) /* StaminaRate */
     , (33739,   5,       1) /* ManaRate */
     , (33739,  12,     0.5) /* Shade */
     , (33739,  13,       1) /* ArmorModVsSlash */
     , (33739,  14,    0.95) /* ArmorModVsPierce */
     , (33739,  15,    0.95) /* ArmorModVsBludgeon */
     , (33739,  16,       1) /* ArmorModVsCold */
     , (33739,  17,       1) /* ArmorModVsFire */
     , (33739,  18,       1) /* ArmorModVsAcid */
     , (33739,  19,       1) /* ArmorModVsElectric */
     , (33739,  31,      18) /* VisualAwarenessRange */
     , (33739,  34,       1) /* PowerupTime */
     , (33739,  36,       1) /* ChargeSpeed */
     , (33739,  39,     0.8) /* DefaultScale */
     , (33739,  43,       4) /* GeneratorRadius */
     , (33739,  64,     0.6) /* ResistSlash */
     , (33739,  65,     0.6) /* ResistPierce */
     , (33739,  66,     0.7) /* ResistBludgeon */
     , (33739,  67,     0.5) /* ResistFire */
     , (33739,  68,     0.5) /* ResistCold */
     , (33739,  69,     0.5) /* ResistAcid */
     , (33739,  70,     0.5) /* ResistElectric */
     , (33739,  71,       1) /* ResistHealthBoost */
     , (33739,  72,       1) /* ResistStaminaDrain */
     , (33739,  73,       1) /* ResistStaminaBoost */
     , (33739,  74,       1) /* ResistManaDrain */
     , (33739,  75,       1) /* ResistManaBoost */
     , (33739,  80,       2) /* AiUseMagicDelay */
     , (33739, 104,      10) /* ObviousRadarRange */
     , (33739, 125,       1) /* ResistHealthDrain */
     , (33739, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33739,   1, 'Parfal Sleech') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33739,   1, 0x020014A0) /* Setup */
     , (33739,   2, 0x09000193) /* MotionTable */
     , (33739,   3, 0x20000062) /* SoundTable */
     , (33739,   4, 0x3000002A) /* CombatTable */
     , (33739,   6, 0x04001EDC) /* PaletteBase */
     , (33739,   7, 0x10000639) /* ClothingBase */
     , (33739,   8, 0x06001DF1) /* Icon */
     , (33739,  22, 0x340000B8) /* PhysicsEffectTable */
     , (33739,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33739,   1, 360, 0, 0) /* Strength */
     , (33739,   2, 360, 0, 0) /* Endurance */
     , (33739,   3, 320, 0, 0) /* Quickness */
     , (33739,   4, 340, 0, 0) /* Coordination */
     , (33739,   5, 430, 0, 0) /* Focus */
     , (33739,   6, 480, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33739,   1,   435, 0, 0, 615) /* MaxHealth */
     , (33739,   3,   500, 0, 0, 860) /* MaxStamina */
     , (33739,   5,  1000, 0, 0, 1480) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33739,  6, 0, 3, 0, 380, 0, 0) /* MeleeDefense        Specialized */
     , (33739,  7, 0, 3, 0, 290, 0, 0) /* MissileDefense      Specialized */
     , (33739, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (33739, 15, 0, 3, 0, 275, 0, 0) /* MagicDefense        Specialized */
     , (33739, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (33739, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (33739, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (33739, 33, 0, 3, 0, 300, 0, 0) /* LifeMagic           Specialized */
     , (33739, 34, 0, 3, 0, 300, 0, 0) /* WarMagic            Specialized */
     , (33739, 45, 0, 3, 0, 210, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33739,  0,  4, 200, 0.75,  650,  650,  618,  618,  650,  650,  650,  650,    0, 1, 0.44,  0.3,    0,  0.4,  0.1,    0, 0.44,  0.3,    0,  0.4,  0.1,    0) /* Head */
     , (33739, 16,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2,  0.5, 0.48,  0.1,  0.5,  0.6,  0.1,  0.5, 0.48,  0.1,  0.5,  0.6, 0.22) /* Torso */
     , (33739, 21,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0, 0.28) /* Wings */
     , (33739, 24,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2, 0.06, 0.22,  0.3,  0.1,  0.2,  0.3, 0.06, 0.22,  0.3,  0.1,  0.2, 0.22) /* UpperTentacle */
     , (33739, 25,  4, 200,  0.5,  650,  650,  618,  618,  650,  650,  650,  650,    0, 3,    0,    0,  0.3,    0,  0.1,  0.3,    0,    0,  0.3,    0,  0.1, 0.28) /* LowerTentacle */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33739,  2074,   2.15)  /* Gossamer Flesh */
     , (33739,  2122,   2.15)  /* Disintegration */
     , (33739,  2162,   2.02)  /* Olthoi's Gift */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (33739,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33739, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33739, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33739, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33739, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33739, -1, 33636, 0, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Glissnal Sleech (33636) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
