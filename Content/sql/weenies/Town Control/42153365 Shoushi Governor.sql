DELETE FROM `weenie` WHERE `class_Id` = 42153365;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
<<<<<<< Updated upstream
VALUES (42153365, 'ace42153365-shoushigovernor', 10, '2022-01-20 02:40:28') /* Creature */;
=======
VALUES (42153365, 'ace42153365-shoushigovernorpk', 10, '2022-01-20 02:40:28') /* Creature */;
>>>>>>> Stashed changes

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42153365,   1,         16) /* ItemType - Creature */
     , (42153365,   2,          4) /* CreatureType - Mosswart */
     , (42153365,   3,          7) /* PaletteTemplate - DeepGreen */
     , (42153365,   6,         -1) /* ItemsCapacity */
     , (42153365,   7,         -1) /* ContainersCapacity */
     , (42153365,  16,          1) /* ItemUseable - No */
     , (42153365,  25,        420) /* Level */
     , (42153365,  67,         64) /* Tolerance - Retaliate */
     , (42153365,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (42153365,  93,    4195336) /* PhysicsState - ReportCollisions, Gravity, EdgeSlide */
     , (42153365, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (42153365, 386,         10) /* Overpower */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42153365,   1, True ) /* Stuck */
     , (42153365,   6, True ) /* AiUsesMana */
     , (42153365,  12, True ) /* ReportCollisions */
     , (42153365,  13, True ) /* Ethreal */
     , (42153365,  14, True ) /* GravityStatus */
     , (42153365,  19, True ) /* Attackable */
     , (42153365,  42, True ) /* AllowEdgeSlide */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (42153365,   1,     1.5) /* HeartbeatInterval */
     , (42153365,   2,       0) /* HeartbeatTimestamp */
     , (42153365,   3,     100) /* HealthRate */
     , (42153365,   4,     100) /* StaminaRate */
     , (42153365,   5,       3) /* ManaRate */
     , (42153365,  12,    0.16) /* Shade */
     , (42153365,  13,       2) /* ArmorModVsSlash */
     , (42153365,  14,       2) /* ArmorModVsPierce */
     , (42153365,  15,       2) /* ArmorModVsBludgeon */
     , (42153365,  16,     1.5) /* ArmorModVsCold */
     , (42153365,  17,       2) /* ArmorModVsFire */
     , (42153365,  18,     1.5) /* ArmorModVsAcid */
     , (42153365,  19,       2) /* ArmorModVsElectric */
     , (42153365,  31,       5) /* VisualAwarenessRange */
     , (42153365,  34,       1) /* PowerupTime */
     , (42153365,  36,       1) /* ChargeSpeed */
     , (42153365,  39,     2.1) /* DefaultScale */
     , (42153365,  55,      25) /* HomeRadius */
     , (42153365,  64,     0.5) /* ResistSlash */
     , (42153365,  65,     0.5) /* ResistPierce */
     , (42153365,  66,     0.5) /* ResistBludgeon */
     , (42153365,  67,     0.5) /* ResistFire */
     , (42153365,  68,       1) /* ResistCold */
     , (42153365,  69,       1) /* ResistAcid */
     , (42153365,  70,     0.5) /* ResistElectric */
     , (42153365,  80,       3) /* AiUseMagicDelay */
     , (42153365, 104,      10) /* ObviousRadarRange */
     , (42153365, 117,     0.5) /* FocusedProbability */
     , (42153365, 122,       2) /* AiAcquireHealth */
     , (42153365, 125,       1) /* ResistHealthDrain */
     , (42153365, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42153365,   1, 'Shoushi Governor') /* Name */
     , (42153365,   5, 'Elite Town Defender') /* Template */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42153365,   1, 0x02001A18) /* Setup */
     , (42153365,   2, 0x09000001) /* MotionTable */
     , (42153365,   3, 0x20000015) /* SoundTable */
     , (42153365,   4, 0x30000000) /* CombatTable */
     , (42153365,   6, 0x0400007E) /* PaletteBase */
     , (42153365,   7, 0x1000086B) /* ClothingBase */
     , (42153365,   8, 0x06001036) /* Icon */
     , (42153365,   9, 0x0500326F) /* EyesTexture */
     , (42153365,  10, 0x0500326A) /* NoseTexture */
     , (42153365,  11, 0x0500326B) /* MouthTexture */
     , (42153365,  17, 0x04001978) /* SkinPalette */
     , (42153365,  18, 0x0100491E) /* HeadObject */
     , (42153365,  22, 0x34000004) /* PhysicsEffectTable */
     , (42153365,  35,       2121) /* DeathTreasureType - Loot Tier: 8 */;

INSERT INTO `weenie_properties_i_i_d` (`object_Id`, `type`, `value`)
<<<<<<< Updated upstream
VALUES (42153365,  16, 0x7DE51014) /* ActivationTarget */;
=======
VALUES (42153365,  16, 0x7DE51015) /* ActivationTarget */;
>>>>>>> Stashed changes

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (42153365,   1, 400, 0, 0) /* Strength */
     , (42153365,   2, 400, 0, 0) /* Endurance */
     , (42153365,   3, 250, 0, 0) /* Quickness */
     , (42153365,   4, 400, 0, 0) /* Coordination */
     , (42153365,   5, 500, 0, 0) /* Focus */
     , (42153365,   6, 500, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (42153365,   1, 149800, 0, 0, 5) /* MaxHealth */
     , (42153365,   3, 99600, 0, 0, 0) /* MaxStamina */
     , (42153365,   5,  4500, 0, 0, 0) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (42153365,  6, 0, 2, 0, 500, 0, 0) /* MeleeDefense        Trained */
     , (42153365,  7, 0, 2, 0, 450, 0, 0) /* MissileDefense      Trained */
     , (42153365, 15, 0, 2, 0, 275, 0, 0) /* MagicDefense        Trained */
     , (42153365, 16, 0, 2, 0, 280, 0, 0) /* ManaConversion      Trained */
     , (42153365, 31, 0, 2, 0, 280, 0, 0) /* CreatureEnchantment Trained */
     , (42153365, 33, 0, 2, 0, 280, 0, 0) /* LifeMagic           Trained */
     , (42153365, 34, 0, 2, 0, 400, 0, 0) /* WarMagic            Trained */
     , (42153365, 41, 0, 2, 0, 500, 0, 0) /* TwoHandedCombat     Trained */
     , (42153365, 43, 0, 2, 0, 280, 0, 0) /* VoidMagic           Trained */
     , (42153365, 44, 0, 2, 0, 500, 0, 0) /* HeavyWeapons        Trained */
     , (42153365, 45, 0, 2, 0, 500, 0, 0) /* LightWeapons        Trained */
     , (42153365, 46, 0, 2, 0, 500, 0, 0) /* FinesseWeapons      Trained */
     , (42153365, 47, 0, 2, 0, 260, 0, 0) /* MissileWeapons      Trained */
     , (42153365, 51, 0, 2, 0, 500, 0, 0) /* SneakAttack         Trained */
     , (42153365, 52, 0, 2, 0, 500, 0, 0) /* DirtyFighting       Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (42153365,  0,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (42153365,  1,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (42153365,  2,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (42153365,  3,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (42153365,  4,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (42153365,  5,  4, 600, 0.75,  400,  200,  200,  200,  200,  200,  200,  200,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (42153365,  6,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (42153365,  7,  4,  0,    0,  400,  200,  200,  200,  200,  200,  200,  200,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (42153365,  8,  4, 600, 0.75,  400,  200,  200,  200,  200,  200,  200,  200,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (42153365,  4312,   2.10)  /* Incantation of Imperil Other */
     , (42153365,  4483,  2.20)  /* Incantation of Lightning Vulnerability Other */
     , (42153365,  6198,  2.168)  /* Incantation of Lightning Bolt */
     , (42153365,  6199,  2.163)  /* Incantation of Lightning Arc */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42153365,  3 /* Death */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
<<<<<<< Updated upstream
VALUES (@parent_id,  0,  15 /* Activate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,  77 /* DeleteSelf */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
=======
VALUES (@parent_id,  0,  23 /* StartEvent */, 0.1, 1, NULL, 'Towncontrol3', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
, (@parent_id,  1,  15 /* Activate */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  2,  77 /* DeleteSelf */, 0.2, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
>>>>>>> Stashed changes

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42153365, 14 /* Taunt */,   0.08, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   7 /* PhysScript */, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 144 /* CampingMastery */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,  17 /* LocalBroadcast */, 0, 1, NULL, 'Shoushi Governor begins casting Violet Rain! RUN!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  2,   5 /* Motion */, 2, 1, 0x430000F2 /* AkimboState */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  3,  19 /* CastSpellInstant */, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4097 /* Violet Rain */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42153365, 14 /* Taunt */,   0.20, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   7 /* PhysScript */, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 144 /* CampingMastery */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,  17 /* LocalBroadcast */, 0, 1, NULL, 'Shoushi Governor begins casting a deadly lightning ring! RUN!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  2,   5 /* Motion */, 2, 1, 0x430000F2 /* AkimboState */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  3,  19 /* CastSpellInstant */, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6168 /* Deadly Ring of Lightning */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42153365, 14 /* Taunt */,   0.20, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,   7 /* PhysScript */, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 145 /* CampingIneptitude */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  1,  17 /* LocalBroadcast */, 0, 1, NULL, 'Shoushi Governor begins casting a deadly lightning volley! RUN!', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  2,   5 /* Motion */, 2, 1, 0x430000F2 /* AkimboState */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  3,  19 /* CastSpellInstant */, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6169 /* Deadly Lightning Volley */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (42153365, 19 /* Homesick */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  14 /* CastSpell */, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 2343 /* Rushed Recovery */, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (42153365, 2,  8150,  1, 0, 0, False) /* Create Mosswart Mask (8150) for Wield */
     , (42153365, 2, 25527,  1, 0, 0, False) /* Create Gauloth Leggings (25527) for Wield */
     , (42153365, 2, 42142662,  1, 0, 0, False) /* Create Blood Bathed Mace (42142662) for Wield */;

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2022-01-17T21:54:14.6293546Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
