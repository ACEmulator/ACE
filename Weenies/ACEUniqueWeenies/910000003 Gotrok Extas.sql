DELETE FROM `weenie` WHERE `class_Id` = 910000003;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (910000003, 'lugianextasrenegade', 10, '2022-08-22 03:09:27') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (910000003,   1,         16) /* ItemType - Creature */
     , (910000003,   2,         70) /* CreatureType - GotrokLugian */
     , (910000003,   3,          14) /* PaletteTemplate - Silver */
     , (910000003,   6,         -1) /* ItemsCapacity */
     , (910000003,   7,         -1) /* ContainersCapacity */
     , (910000003,   8,       8000) /* Mass */
     , (910000003,  16,          1) /* ItemUseable - No */
     , (910000003,  25,         370) /* Level */
     , (910000003,  27,          0) /* ArmorType - None */
     , (910000003,  40,          2) /* CombatMode - Melee */
     , (910000003,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (910000003,  72,          6) /* FriendType - Tumerok */
     , (910000003,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (910000003, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (910000003, 133,          4) /* ShowableOnRadar - ShowAlways */
     , (910000003, 140,          1) /* AiOptions - CanOpenDoors */
     , (910000003, 146,      3000000) /* XpOverride */
     , (910000003,  81,          5) /* MaxGeneratedObjects */
     , (910000003,  82,          0) /* InitGeneratedObjects */
     , (910000003, 307,         40) /* DamageRating */
     , (910000003, 308,         30) /* DamageResistRating */
     , (910000003, 315,         20) /* CritResistRating */
     , (910000003, 316,         25) /* CritDamageResistRating */
     , (910000003, 386,          35) /* Overpower */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (910000003,   1, True ) /* Stuck */
     , (910000003,  11, False) /* IgnoreCollisions */
     , (910000003,  12, True ) /* ReportCollisions */
     , (910000003,  13, False) /* Ethereal */
     , (910000003,  14, True ) /* GravityStatus */
     , (910000003,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (910000003,   1,       5) /* HeartbeatInterval */
     , (910000003,   2,       0) /* HeartbeatTimestamp */
     , (910000003,   3,     0.9) /* HealthRate */
     , (910000003,   4,       4) /* StaminaRate */
     , (910000003,   5,       2) /* ManaRate */
     , (910000003,  12,     0.123) /* Shade */
     , (910000003,  39,       1.2) /* DefaultScale */
     , (910000003,  13,    0.85) /* ArmorModVsSlash */
     , (910000003,  14,    0.85) /* ArmorModVsPierce */
     , (910000003,  15,    0.85) /* ArmorModVsBludgeon */
     , (910000003,  16,    0.75) /* ArmorModVsCold */
     , (910000003,  17,    0.75) /* ArmorModVsFire */
     , (910000003,  18,    0.75) /* ArmorModVsAcid */
     , (910000003,  19,     0.7) /* ArmorModVsElectric */
     , (910000003,  31,      23) /* VisualAwarenessRange */
     , (910000003,  34,       2) /* PowerupTime */
     , (910000003,  36,       1) /* ChargeSpeed */
     , (910000003,  64,    0.5) /* ResistSlash */
     , (910000003,  65,    0.5) /* ResistPierce */
     , (910000003,  66,    0.5) /* ResistBludgeon */
     , (910000003,  67,    0.5) /* ResistFire */
     , (910000003,  68,    0.55) /* ResistCold */
     , (910000003,  69,     0.5) /* ResistAcid */
     , (910000003,  70,       .9) /* ResistElectric */
     , (910000003,  71,       1) /* ResistHealthBoost */
     , (910000003,  72,       1) /* ResistStaminaDrain */
     , (910000003,  73,       1) /* ResistStaminaBoost */
     , (910000003,  74,       1) /* ResistManaDrain */
     , (910000003,  75,       1) /* ResistManaBoost */
     , (910000003, 104,      10) /* ObviousRadarRange */
     , (910000003, 117,     0.5) /* FocusedProbability */
     , (910000003, 125,       1) /* ResistHealthDrain */
     , (910000003,  80,       2) /* AiUseMagicDelay */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (910000003,   1, 'Lithos Lugian') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (910000003,   1, 0x02000A0B) /* Setup */
     , (910000003,   2, 0x09000006) /* MotionTable */
     , (910000003,   3, 0x2000000A) /* SoundTable */
     , (910000003,   4, 0x30000003) /* CombatTable */
     , (910000003,   6, 0x040010C6) /* PaletteBase */
     , (910000003,   7, 0x100002BA) /* ClothingBase */
     , (910000003,   8, 0x06001037) /* Icon */
     , (910000003,  22, 0x3400001E) /* PhysicsEffectTable */
     -- , (910000003,  32,        438) /* WieldedTreasureType -
     --                               Wield Rock (23748) | Probability: 90%
     --                               Wield Rock (23746) | Probability: 10%
     --                               Wield Lugian Axe (23742) | Probability: 17.5%
     --                               Wield Lugian Morning Star (23768) | Probability: 17.5%
     --                               Wield Lugian Club (23752) | Probability: 17.5%
     --                               Wield Lugian Mace (23760) | Probability: 17.5%
     --                               Wield Lugian Axe (23740) | Probability: 5%
     --                               Wield Lugian Morning Star (23766) | Probability: 5% */
     , (910000003,  35,        2120) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (910000003,   1, 830, 0, 0) /* Strength */
     , (910000003,   2, 200, 0, 0) /* Endurance */
     , (910000003,   3,  85, 0, 0) /* Quickness */
     , (910000003,   4,  830, 0, 0) /* Coordination */
     , (910000003,   5,  55, 0, 0) /* Focus */
     , (910000003,   6,  85, 0, 0) /* Self */;


INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (910000003,   1,    20000, 0, 0, 150) /* MaxHealth */
     , (910000003,   3,   150, 0, 0, 350) /* MaxStamina */
     , (910000003,   5,     0, 0, 0, 85) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (910000003,  6, 0, 3, 0, 410, 0, 0) /* MeleeDefense        Specialized */
     , (910000003,  7, 0, 3, 0, 700, 0, 0) /* MissileDefense      Specialized */
     , (910000003, 15, 0, 3, 0, 453, 0, 0) /* MagicDefense        Specialized */
     , (910000003, 20, 0, 3, 0, 150, 0, 0) /* Deception           Specialized */
     , (910000003, 31, 0, 3, 0, 440, 0, 0) /* CreatureEnchantment Specialized */
     , (910000003, 50, 0, 3, 0, 540, 0, 0) /* Recklessness           Specialized */
     , (910000003, 51, 0, 3, 0, 540, 0, 0) /* Sneak attack           Specialized */
     , (910000003, 52, 0, 3, 0, 540, 0, 0) /* Dirty fighting           Specialized */
     , (910000003, 33, 0, 3, 0, 440, 0, 0) /* LifeMagic           Specialized */
     , (910000003, 34, 0, 3, 0, 900, 0, 0) /* WarMagic            Specialized */
     , (910000003, 44, 0, 3, 0, 455, 0, 0) /* HeavyWeapons        Specialized */
     , (910000003, 43, 0, 2, 0, 550, 0, 0) /* VoidMagic           Trained */
     , (910000003, 45, 0, 3, 0, 455, 0, 0) /* LightWeapons        Specialized */
     , (910000003, 46, 0, 3, 0, 455, 0, 0) /* FinesseWeapons      Specialized */
     , (910000003, 47, 0, 3, 0, 385, 0, 0) /* MissileWeapons      Specialized */
     , (910000003, 48, 0, 3, 0, 405, 0, 0) /* Shield              Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (910000003,  0,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (910000003,  1,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (910000003,  2,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (910000003,  3,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (910000003,  4,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (910000003,  5,  4, 400, 0.75,  500,  850,  850,  850,  850,  600,  850,  850,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (910000003,  6,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (910000003,  7,  4,  0,    0,  500,  850,  850,  850,  850,  600,  850,  850,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (910000003,  8,  4, 400, 0.75,  500,  850,  850,  850,  850,  600,  850,  850,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

-- INSERT INTO `weenie_properties_event_filter` (`object_Id`, `event`)
-- VALUES (910000003,  94) /* ATTACK_NOTIFICATION_EVENT */
--      , (910000003, 414) /* PLAYER_DEATH_EVENT */;

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003,  5 /* HeartBeat */,  0.025, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000052 /* Twitch2 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003,  5 /* HeartBeat */,    0.1, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003,  5 /* HeartBeat */,  0.125, NULL, 0x8000003C /* HandCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000053 /* Twitch3 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003,  5 /* HeartBeat */,  0.025, NULL, 0x8000003E /* SwordCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003,  5 /* HeartBeat */,  0.025, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000052 /* Twitch2 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003,  5 /* HeartBeat */,    0.1, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003,  5 /* HeartBeat */,  0.125, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000053 /* Twitch3 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003, 16 /* KillTaunt */,    0.5, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'Come back no more, frail one, slaying your kind has lost its challenge.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003, 18 /* Scream */,    0.2, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,   8 /* Say */, 0, 0, NULL, 'Fellow warriors, aid me!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
-- VALUES (910000003, 21 /* ResistSpell */,   0.75, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- SET @parent_id = LAST_INSERT_ID();

-- INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
-- VALUES (@parent_id,  0,  10 /* Tell */, 0, 1, NULL, 'I gather you are not a mighty wizard among your kind.  It would be best if you learned your role.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (910000003, 14 /* Taunt */,    0.25, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  4,  72 /* Generate */, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);



INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES
      (@parent_id,  1,   7 /* PhysScript */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 148 /* AetheriaLevelUp */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  2,  17 /* LocalBroadcast */, 0, 1, NULL, 'Lithos Lugian swells with excess fiery magical energy.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     -- , (@parent_id,  2,   5 /* Motion */, 2, 1, 0x430000FC /* WoahState */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  3,  19 /* CastSpellInstant */, 2, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4439 /* Fire Bolt 8*/, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)

     ,(@parent_id,  5,   7 /* PhysScript */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 161 /* AetheriaLevelUp */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  6,  19 /* CastSpellInstant */, 2, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4423 /* Fire Arc 8*/, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
;


INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (910000003, 4, 90004173, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Fire Dmg Cloud up to max of 1) - Regenerate upon Destruction - Location to (re)Generate: OnTop */;


-- INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
-- VALUES

-- -- (910000003,  3269,   2.09)  /* Under the Lash */
--      , (910000003,  3080,   2.15)  /* Bruised Flesh - Incat Bludge vuln 9 */;
     -- , (910000003,  4427,   2.09)  /* Incantation of Shock Arc*/;
     -- , (910000003,  4643,   2.09)  /* Drain Health other 8 */
     -- , (910000003,  5532,   2.09)  /* Bloodstone bolt 8 */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (910000003, 2, 20045448,  1, 0, 1, False) /*Rare Star of Linvak Tukal */
 ,(910000003, 2, 20000115,  1, 0, 1, False) /* Cast on strike Fire vuln 8 boots */
   ,(910000003, 2, 2000056,  1, 0, 1, False) /* Cast on strike Imp Gloves */
      ,(910000003, 2, 2000086,  1, 0, 1, False) /* Cast on strike Corruption Pauldrons */;


-- (910000003, 9,  6876,  0, 0, 0.02, False) /* Create Sturdy Iron Key (6876) for ContainTreasure */
--      , (910000003, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
--      , (910000003, 9,  7043,  0, 0, 0.03, False) /* Create Large Lugian Sinew (7043) for ContainTreasure */
--      , (910000003, 9,     0,  0, 0, 0.97, False) /* Create nothing for ContainTreasure */;
