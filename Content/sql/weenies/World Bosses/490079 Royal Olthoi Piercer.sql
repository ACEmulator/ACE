DELETE FROM `weenie` WHERE `class_Id` = 490079;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490079, 'ace490079-Royal Olthoi Piercer', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490079,   1,         16) /* ItemType - Creature */
     , (490079,   2,          1) /* CreatureType - Olthoi */
     , (490079,   3,         14) /* PaletteTemplate - Red */
     , (490079,   6,         -1) /* ItemsCapacity */
     , (490079,   7,         -1) /* ContainersCapacity */
     , (490079,   8,        800) /* Mass */
     , (490079,  16,          1) /* ItemUseable - No */
     , (490079,  25,        220) /* Level */
     , (490079,  27,          0) /* ArmorType - None */
     , (490079,  40,          2) /* CombatMode - Melee */
     , (490079,  68,         13) /* TargetingTactic - Random, LastDamager, TopDamager */
     , (490079,  72,         35) /* FriendType - OlthoiLarvae */
     , (490079,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (490079, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (490079, 140,          1) /* AiOptions - CanOpenDoors */
     , (490079, 146,    1400000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490079,   1, True ) /* Stuck */
     , (490079,  11, False) /* IgnoreCollisions */
     , (490079,  12, True ) /* ReportCollisions */
     , (490079,  13, False) /* Ethereal */
     , (490079,  14, True ) /* GravityStatus */
     , (490079,  19, True ) /* Attackable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490079,   1,       5) /* HeartbeatInterval */
     , (490079,   2,       0) /* HeartbeatTimestamp */
     , (490079,   3,     0.7) /* HealthRate */
     , (490079,   4,       4) /* StaminaRate */
     , (490079,   5,       2) /* ManaRate */
     , (490079,  12,     0.5) /* Shade */
     , (490079,  13,       1) /* ArmorModVsSlash */
     , (490079,  14,    0.95) /* ArmorModVsPierce */
     , (490079,  15,     0.9) /* ArmorModVsBludgeon */
     , (490079,  16,    0.95) /* ArmorModVsCold */
     , (490079,  17,       1) /* ArmorModVsFire */
     , (490079,  18,       1) /* ArmorModVsAcid */
     , (490079,  19,       1) /* ArmorModVsElectric */
     , (490079,  31,      28) /* VisualAwarenessRange */
     , (490079,  34,       1) /* PowerupTime */
     , (490079,  36,       1) /* ChargeSpeed */
     , (490079,  39,     1.0) /* DefaultScale */
	 , (490079,  55,      110) /* HomeRadius */
     , (490079,  64,     0.4) /* ResistSlash */
     , (490079,  65,     0.5) /* ResistPierce */
     , (490079,  66,     0.5) /* ResistBludgeon */
     , (490079,  67,     0.2) /* ResistFire */
     , (490079,  68,     0.5) /* ResistCold */
     , (490079,  69,     0.2) /* ResistAcid */
     , (490079,  70,     0.2) /* ResistElectric */
     , (490079,  71,       1) /* ResistHealthBoost */
     , (490079,  72,       1) /* ResistStaminaDrain */
     , (490079,  73,       1) /* ResistStaminaBoost */
     , (490079,  74,       1) /* ResistManaDrain */
     , (490079,  75,       1) /* ResistManaBoost */
     , (490079,  77,       1) /* PhysicsScriptIntensity */
     , (490079, 104,      10) /* ObviousRadarRange */
     , (490079, 117,     0.6) /* FocusedProbability */
     , (490079, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490079,   1, 'Royal Olthoi Piercer') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490079,   1, 0x02000F95) /* Setup */
     , (490079,   2, 0x0900012B) /* MotionTable */
     , (490079,   3, 0x2000009E) /* SoundTable */
     , (490079,   4, 0x30000038) /* CombatTable */
     , (490079,   6, 0x040015C8) /* PaletteBase */
     , (490079,   7, 0x100004B3) /* ClothingBase */
     , (490079,   8, 0x06002C42) /* Icon */
     , (490079,  19, 0x00000056) /* ActivationAnimation */
     , (490079,  22, 0x340000A6) /* PhysicsEffectTable */
     , (490079,  30,         86) /* PhysicsScript - BreatheAcid */
     , (490079,  35,       1000) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490079,   1, 500, 0, 0) /* Strength */
     , (490079,   2, 500, 0, 0) /* Endurance */
     , (490079,   3, 350, 0, 0) /* Quickness */
     , (490079,   4, 350, 0, 0) /* Coordination */
     , (490079,   5, 300, 0, 0) /* Focus */
     , (490079,   6, 300, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490079,   1,  10000, 0, 0, 10000) /* MaxHealth */
     , (490079,   3,  3600, 0, 0, 4100) /* MaxStamina */
     , (490079,   5,  1000, 0, 0, 1300) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490079,  6, 0, 2, 0, 417, 0, 0) /* MeleeDefense        Trained */
     , (490079,  7, 0, 2, 0, 580, 0, 0) /* MissileDefense      Trained */
     , (490079, 15, 0, 2, 0, 370, 0, 0) /* MagicDefense        Trained */
     , (490079, 45, 0, 2, 0, 417, 0, 0) /* LightWeapons        Trained */
     , (490079, 52, 0, 2, 0, 417, 0, 0) /* DirtyFighting       Trained */
	 , (490079, 33, 0, 2, 0, 600, 0, 0) /* LifeMagic           Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490079,  0,  2, 50,  0.5,  450,  450,  428,  405,  428,  450,  450,  450,    0, 1,  0.1,    0,    0,  0.1,    0,    0,  0.1,    0,    0,  0.1,    0,    0) /* Head */
     , (490079, 10,  2, 50,  0.5,  450,  450,  428,  405,  428,  450,  450,  450,    0, 2,    0,  0.2,  0.1,    0,  0.2,  0.1,    0,  0.2,  0.1,    0,  0.2,  0.1) /* FrontLeg */
     , (490079, 13,  2, 50,  0.5,  450,  450,  428,  405,  428,  450,  450,  450,    0, 3,    0,  0.2, 0.45,    0,  0.2, 0.45,    0,  0.2, 0.45,    0,  0.2, 0.45) /* RearLeg */
     , (490079, 16,  2,  0,  0.5,  450,  450,  428,  405,  428,  450,  450,  450,    0, 1, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45, 0.45,  0.4, 0.45) /* Torso */
     , (490079, 17,  2, 50,  0.5,  450,  450,  428,  405,  428,  450,  450,  450,    0, 3,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0) /* Tail */
     , (490079, 19,  2, 50,  0.5,  450,  450,  428,  405,  428,  450,  450,  450,    0, 2, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0, 0.45,  0.2,    0) /* Leg */
     , (490079, 22, 32, 75,  0.5,    0,    0,    0,    0,    0,    0,    0,    0,    0, 0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0) /* Breath */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490079,  4473,   2.50)
, (490079,  3877,   2.50)  /* Corrosive Strike */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490079,  5 /* HeartBeat */,   0.15, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000051 /* Twitch1 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490079,  5 /* HeartBeat */,    0.3, NULL, 0x8000003D /* NonCombat */, 0x41000003 /* Ready */, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   5 /* Motion */, 0, 1, 0x10000052 /* Twitch2 */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
