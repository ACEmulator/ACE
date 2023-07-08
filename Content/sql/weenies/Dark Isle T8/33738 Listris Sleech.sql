DELETE FROM `weenie` WHERE `class_Id` = 33738;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (33738, 'ace33738-listrissleech', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (33738,   1,         16) /* ItemType - Creature */
     , (33738,   2,         45) /* CreatureType - Niffis */
     , (33738,   3,         10) /* PaletteTemplate - LightBlue */
     , (33738,   6,         -1) /* ItemsCapacity */
     , (33738,   7,         -1) /* ContainersCapacity */
     , (33738,  16,          1) /* ItemUseable - No */
     , (33738,  25,        200) /* Level */
     , (33738,  27,          0) /* ArmorType - None */
     , (33738,  40,          2) /* CombatMode - Melee */
     , (33738,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (33738,  81,          1) /* MaxGeneratedObjects */
     , (33738,  82,          0) /* InitGeneratedObjects */
     , (33738,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (33738, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (33738, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (33738, 140,          1) /* AiOptions - CanOpenDoors */
     , (33738, 146,     315000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (33738,   1, True ) /* Stuck */
     , (33738,   6, True ) /* AiUsesMana */
     , (33738,  11, False) /* IgnoreCollisions */
     , (33738,  12, True ) /* ReportCollisions */
     , (33738,  13, False) /* Ethereal */
     , (33738,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (33738,   1,       5) /* HeartbeatInterval */
     , (33738,   2,       0) /* HeartbeatTimestamp */
     , (33738,   3,     0.6) /* HealthRate */
     , (33738,   4,       3) /* StaminaRate */
     , (33738,   5,       1) /* ManaRate */
     , (33738,  12,     0.5) /* Shade */
     , (33738,  13,       1) /* ArmorModVsSlash */
     , (33738,  14,    0.95) /* ArmorModVsPierce */
     , (33738,  15,    0.95) /* ArmorModVsBludgeon */
     , (33738,  16,       1) /* ArmorModVsCold */
     , (33738,  17,       1) /* ArmorModVsFire */
     , (33738,  18,       1) /* ArmorModVsAcid */
     , (33738,  19,       1) /* ArmorModVsElectric */
     , (33738,  31,      18) /* VisualAwarenessRange */
     , (33738,  34,       1) /* PowerupTime */
     , (33738,  36,       1) /* ChargeSpeed */
     , (33738,  39,     0.9) /* DefaultScale */
     , (33738,  43,       4) /* GeneratorRadius */
     , (33738,  64,     0.7) /* ResistSlash */
     , (33738,  65,     0.7) /* ResistPierce */
     , (33738,  66,     0.8) /* ResistBludgeon */
     , (33738,  67,     0.5) /* ResistFire */
     , (33738,  68,     0.5) /* ResistCold */
     , (33738,  69,     0.5) /* ResistAcid */
     , (33738,  70,     0.5) /* ResistElectric */
     , (33738,  71,       1) /* ResistHealthBoost */
     , (33738,  72,       1) /* ResistStaminaDrain */
     , (33738,  73,       1) /* ResistStaminaBoost */
     , (33738,  74,       1) /* ResistManaDrain */
     , (33738,  75,       1) /* ResistManaBoost */
     , (33738,  80,       2) /* AiUseMagicDelay */
     , (33738, 104,      10) /* ObviousRadarRange */
     , (33738, 125,       1) /* ResistHealthDrain */
     , (33738, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (33738,   1, 'Listris Sleech') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (33738,   1, 0x020014A0) /* Setup */
     , (33738,   2, 0x09000193) /* MotionTable */
     , (33738,   3, 0x20000062) /* SoundTable */
     , (33738,   4, 0x3000002A) /* CombatTable */
     , (33738,   6, 0x04001EDC) /* PaletteBase */
     , (33738,   7, 0x10000639) /* ClothingBase */
     , (33738,   8, 0x06001DF1) /* Icon */
     , (33738,  22, 0x340000B8) /* PhysicsEffectTable */
     , (33738,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (33738,   1, 370, 0, 0) /* Strength */
     , (33738,   2, 370, 0, 0) /* Endurance */
     , (33738,   3, 330, 0, 0) /* Quickness */
     , (33738,   4, 350, 0, 0) /* Coordination */
     , (33738,   5, 440, 0, 0) /* Focus */
     , (33738,   6, 490, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (33738,   1,   435, 0, 0, 620) /* MaxHealth */
     , (33738,   3,   500, 0, 0, 870) /* MaxStamina */
     , (33738,   5,  1000, 0, 0, 1490) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (33738,  6, 0, 3, 0, 380, 0, 0) /* MeleeDefense        Specialized */
     , (33738,  7, 0, 3, 0, 290, 0, 0) /* MissileDefense      Specialized */
     , (33738, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (33738, 15, 0, 3, 0, 275, 0, 0) /* MagicDefense        Specialized */
     , (33738, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (33738, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (33738, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (33738, 33, 0, 3, 0, 300, 0, 0) /* LifeMagic           Specialized */
     , (33738, 34, 0, 3, 0, 300, 0, 0) /* WarMagic            Specialized */
     , (33738, 45, 0, 3, 0, 210, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (33738,  0,  4, 200, 0.75,  650,  650,  618,  618,  650,  650,  650,  650,    0, 1, 0.44,  0.3,    0,  0.4,  0.1,    0, 0.44,  0.3,    0,  0.4,  0.1,    0) /* Head */
     , (33738, 16,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2,  0.5, 0.48,  0.1,  0.5,  0.6,  0.1,  0.5, 0.48,  0.1,  0.5,  0.6, 0.22) /* Torso */
     , (33738, 21,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0, 0.28) /* Wings */
     , (33738, 24,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2, 0.06, 0.22,  0.3,  0.1,  0.2,  0.3, 0.06, 0.22,  0.3,  0.1,  0.2, 0.22) /* UpperTentacle */
     , (33738, 25,  4, 200,  0.5,  650,  650,  618,  618,  650,  650,  650,  650,    0, 3,    0,    0,  0.3,    0,  0.1,  0.3,    0,    0,  0.3,    0,  0.1, 0.28) /* LowerTentacle */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (33738,  2074,   2.15)  /* Gossamer Flesh */
     , (33738,  2122,   2.15)  /* Disintegration */
     , (33738,  2162,   2.02)  /* Olthoi's Gift */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (33738,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  72 /* Generate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (33738, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (33738, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (33738, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (33738, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (33738, -1, 70331, 0, 1, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) /* Generate Ehlyis Sleech (70331) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
