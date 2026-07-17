DELETE FROM `weenie` WHERE `class_Id` = 910000004;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (910000004, 'EliteGigasLugian', 10, '2022-08-22 03:09:27') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (910000004,   1,         16) /* ItemType - Creature */
     , (910000004,   2,         70) /* CreatureType - GotrokLugian */
     , (910000004,   3,          19) /* PaletteTemplate - Silver */
     , (910000004,   6,         -1) /* ItemsCapacity */
     , (910000004,   7,         -1) /* ContainersCapacity */
     , (910000004,   8,       8000) /* Mass */
     , (910000004,  16,          1) /* ItemUseable - No */
     , (910000004,  25,         380) /* Level */
     , (910000004,  27,          0) /* ArmorType - None */
     , (910000004,  40,          2) /* CombatMode - Melee */
     , (910000004,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (910000004,  72,          6) /* FriendType - Tumerok */
     , (910000004,  81,          5) /* MaxGeneratedObjects */
     , (910000004,  82,          0) /* InitGeneratedObjects */
     , (910000004,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (910000004, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (910000004, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (910000004, 140,          1) /* AiOptions - CanOpenDoors */
     , (910000004, 146,      30000) /* XpOverride */
     , (910000004, 307,         60) /* DamageRating */
     , (910000004, 308,         30) /* DamageResistRating */
     , (910000004, 315,         20) /* CritResistRating */
     , (910000004, 316,         25) /* CritDamageResistRating */
     , (910000004, 386,          35) /* Overpower */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (910000004,   1, True ) /* Stuck */
     , (910000004,  11, False) /* IgnoreCollisions */
     , (910000004,  12, True ) /* ReportCollisions */
     , (910000004,  13, False) /* Ethereal */
     , (910000004,  14, True ) /* GravityStatus */
     , (910000004,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (910000004,   1,       5) /* HeartbeatInterval */
     , (910000004,   2,       0) /* HeartbeatTimestamp */
     , (910000004,   3,     0.9) /* HealthRate */
      , (910000004,  39,       1.5) /* DefaultScale */
     , (910000004,   4,       4) /* StaminaRate */
     , (910000004,   5,       2) /* ManaRate */
     , (910000004,  12,     0.123) /* Shade */
     , (910000004,  13,    0.85) /* ArmorModVsSlash */
     , (910000004,  14,    0.85) /* ArmorModVsPierce */
     , (910000004,  15,    0.85) /* ArmorModVsBludgeon */
     , (910000004,  16,    0.75) /* ArmorModVsCold */
     , (910000004,  17,    0.75) /* ArmorModVsFire */
     , (910000004,  18,    0.75) /* ArmorModVsAcid */
     , (910000004,  19,     0.7) /* ArmorModVsElectric */
     , (910000004,  31,      23) /* VisualAwarenessRange */
     , (910000004,  34,       3) /* PowerupTime */
     , (910000004,  36,       1) /* ChargeSpeed */
     , (910000004,  64,    0.6) /* ResistSlash */
     , (910000004,  65,    0.6) /* ResistPierce */
     , (910000004,  66,    0.6) /* ResistBludgeon */
     , (910000004,  67,    0.65) /* ResistFire */
     , (910000004,  68,    0.7) /* ResistCold */
     , (910000004,  69,     0.6) /* ResistAcid */
     , (910000004,  70,       .85) /* ResistElectric */
     , (910000004,  71,       1) /* ResistHealthBoost */
     , (910000004,  72,       1) /* ResistStaminaDrain */
     , (910000004,  73,       1) /* ResistStaminaBoost */
     , (910000004,  74,       1) /* ResistManaDrain */
     , (910000004,  75,       1) /* ResistManaBoost */
     , (910000004, 104,      10) /* ObviousRadarRange */

     , (910000004, 117,     0.5) /* FocusedProbability */
     , (910000004, 125,       1) /* ResistHealthDrain */
     , (910000004,  80,       2) /* AiUseMagicDelay */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (910000004,   1, 'Gigas Lugian') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (910000004,   1, 0x02000A0B) /* Setup */
     , (910000004,   2, 0x09000006) /* MotionTable */
     , (910000004,   3, 0x2000000A) /* SoundTable */
     , (910000004,   4, 0x30000003) /* CombatTable */
     , (910000004,   6, 0x040010C6) /* PaletteBase */
     , (910000004,   7, 0x100002BA) /* ClothingBase */
     , (910000004,   8, 0x06001037) /* Icon */
     , (910000004,  22, 0x3400001E) /* PhysicsEffectTable */
     -- , (910000004,  32,        438) /* WieldedTreasureType -
     --                               Wield Rock (23748) | Probability: 90%
     --                               Wield Rock (23746) | Probability: 10%
     --                               Wield Lugian Axe (23742) | Probability: 17.5%
     --                               Wield Lugian Morning Star (23768) | Probability: 17.5%
     --                               Wield Lugian Club (23752) | Probability: 17.5%
     --                               Wield Lugian Mace (23760) | Probability: 17.5%
     --                               Wield Lugian Axe (23740) | Probability: 5%
     --                               Wield Lugian Morning Star (23766) | Probability: 5% */
     , (910000004,  35,        2120) /* DeathTreasureType - Loot Tier: 3 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (910000004,   1, 870, 0, 0) /* Strength */
     , (910000004,   2, 200, 0, 0) /* Endurance */
     , (910000004,   3,  100, 0, 0) /* Quickness */
     , (910000004,   4,  870, 0, 0) /* Coordination */
     , (910000004,   5,  55, 0, 0) /* Focus */
     , (910000004,   6,  85, 0, 0) /* Self */;


INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (910000004,   1,    22500, 0, 0, 150) /* MaxHealth */
     , (910000004,   3,   1500, 0, 0, 350) /* MaxStamina */
     , (910000004,   5,     0, 0, 0, 85) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (910000004,  6, 0, 3, 0, 400, 0, 0) /* MeleeDefense        Specialized */
     , (910000004,  7, 0, 3, 0, 700, 0, 0) /* MissileDefense      Specialized */
     , (910000004, 15, 0, 3, 0, 453, 0, 0) /* MagicDefense        Specialized */
     , (910000004, 20, 0, 3, 0, 150, 0, 0) /* Deception           Specialized */
     , (910000004, 31, 0, 3, 0, 440, 0, 0) /* CreatureEnchantment Specialized */
     , (910000004, 33, 0, 3, 0, 540, 0, 0) /* LifeMagic           Specialized */
     , (910000004, 34, 0, 3, 0, 485, 0, 0) /* WarMagic            Specialized */
     , (910000004, 50, 0, 3, 0, 540, 0, 0) /* Recklessness           Specialized */
     , (910000004, 51, 0, 3, 0, 540, 0, 0) /* Sneak attack           Specialized */
     , (910000004, 52, 0, 3, 0, 540, 0, 0) /* Dirty fighting           Specialized */
     , (910000004, 44, 0, 3, 0, 455, 0, 0) /* HeavyWeapons        Specialized */
     , (910000004, 45, 0, 3, 0, 455, 0, 0) /* LightWeapons        Specialized */
     , (910000004, 46, 0, 3, 0, 455, 0, 0) /* FinesseWeapons      Specialized */
     , (910000004, 47, 0, 3, 0, 385, 0, 0) /* MissileWeapons      Specialized */
     , (910000004, 48, 0, 3, 0, 405, 0, 0) /* Shield              Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (910000004,  0,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (910000004,  1,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (910000004,  2,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (910000004,  3,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (910000004,  4,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (910000004,  5,  4, 400, 0.75,  500,  850,  850,  850,  850,  600,  850,  850,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (910000004,  6,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (910000004,  7,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (910000004,  8,  4, 400, 0.75,  500,  850,  850,  850,  850,  600,  850,  850,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

-- INSERT INTO `weenie_properties_event_filter` (`object_Id`, `event`)
-- VALUES (910000004,  94) /* ATTACK_NOTIFICATION_EVENT */
--      , (910000004, 414) /* PLAYER_DEATH_EVENT */;

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004,  5 /* HeartBeat */,  0.025, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000052 /* Twitch2 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004,  5 /* HeartBeat */,    0.1, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004,  5 /* HeartBeat */,  0.125, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000053 /* Twitch3 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004,  5 /* HeartBeat */,  0.025, NULL, 0x8000003E /* SwordCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004,  5 /* HeartBeat */,  0.025, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000052 /* Twitch2 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004,  5 /* HeartBeat */,    0.1, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004,  5 /* HeartBeat */,  0.125, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000053 /* Twitch3 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004, 16 /* KillTaunt */,    0.5, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Come back no more, frail one, slaying your kind has lost its challenge.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004, 18 /* Scream */,    0.2, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   8 /* Say */, 0, 0, NULL, 'Fellow warriors, aid me!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000004, 21 /* ResistSpell */,   0.75, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (910000004, 14 /* Taunt */,    0.25, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();




INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  4,  72 /* Generate */, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);


INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES
      (@parent_id,  1,   7 /* PhysScript */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 161 /* AetheriaLevelUp */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     -- , (@parent_id,  1,  17 /* LocalBroadcast */, 0, 1, NULL, 'The Thorn Reaver begins casting a deadly thorn ring! RUN!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     -- , (@parent_id,  2,   5 /* Motion */, 2, 1, 0x430000FC /* WoahState */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  2,  17 /* LocalBroadcast */, 0, 1, NULL, 'Gigas Lugian swells with excess electrical magical energy.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  3,  19 /* CastSpellInstant */, 2, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6199 /* Lighnting Arc 8*/, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     ,(@parent_id,  5,   7 /* PhysScript */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 148 /* Dispel */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  6,  19 /* CastSpellInstant */, 2, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6198 /* Lightning Bolt 8*/, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
;

-- INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
-- VALUES (910000004,  3269,   2.09)  /* Under the Lash */
--      , (910000004,  3084,   2.09)  /* Bruised Flesh - Incat Pierce vuln 9 */
--      , (910000004,  3121	,   2.09)  /* Soul Spike*/;
--      -- , (910000004,  4643,   2.09)  /* Drain Health other 8 */
--      -- , (910000004,  5532,   2.09)  /* Bloodstone bolt 8 */;


INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (910000004, 4, 90004175, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Cold Cloud (52466) (x1 up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (910000004, 2, 20029924,  1, 0, 1, False) /*Lugian War Maul */
     ,(910000004, 2, 20044975,  1, 0, 1, False) /* Cast on strike Lightning Vuln 8 BP*/
     ,(910000004, 2, 2000056,  1, 0, 1, False) /* Cast on strike Imp Gloves */
     ,(910000004, 2, 2000039,  1, 0, 1, False) /* Cast on strike Incant Corrosion bp */;

-- (910000004, 9,  6876,  0, 0, 0.02, False) /* Create Sturdy Iron Key (6876) for ContainTreasure */
--      , (910000004, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
--      , (910000004, 9,  7043,  0, 0, 0.03, False) /* Create Large Lugian Sinew (7043) for ContainTreasure */
--      , (910000004, 9,     0,  0, 0, 0.97, False) /* Create nothing for ContainTreasure */;
