INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`)
VALUES ('49114', 'moar', 10) /* Moar */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (49114, 1, 16) /* ITEM_TYPE_INT */
     , (49114, 7, -1) /* CONTAINERS_CAPACITY_INT */
     , (49114, 6, -1) /* ITEMS_CAPACITY_INT */
     , (49114, 133, 1) /* SHOWABLE_ON_RADAR_INT */
     , (49114, 16, 1) /* ITEM_USEABLE_INT */
     , (49114, 93, 1036) /* PHYSICS_STATE_INT */
     , (49114, 9007, 10) /* Creature_WeenieType */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (49114, 13, True) /* ETHEREAL_BOOL */
     , (49114, 12, True) /* REPORT_COLLISIONS_BOOL */
     , (49114, 14, True) /* GRAVITY_STATUS_BOOL */
     , (49114, 19, True) /* ATTACKABLE_BOOL */
     , (49114, 1, True) /* STUCK_BOOL */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (49114, 77, 1) /* PHYSICS_SCRIPT_INTENSITY_FLOAT */
     , (49114, 39, 2) /* DEFAULT_SCALE_FLOAT */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (49114, 1, 'Moar') /* NAME_STRING */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (49114, 8, 100671185) /* ICON_DID */
     , (49114, 1, 33561528) /* SETUP_DID */
     , (49114, 3, 536871018) /* SOUND_TABLE_DID */
     , (49114, 2, 150995346) /* MOTION_TABLE_DID */
     , (49114, 22, 872415415) /* PHYSICS_EFFECT_TABLE_DID */
     , (49114, 19, 86) /* ACTIVATION_ANIMATION_DID */
     , (49114, 6, 67116749) /* PALETTE_BASE_DID */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (49114, 1, 210, 0, 0) /* STRENGTH_ATTRIBUTE */
     , (49114, 2, 240, 0, 0) /* ENDURANCE_ATTRIBUTE */
     , (49114, 4, 160, 0, 0) /* COORDINATION_ATTRIBUTE */
     , (49114, 8, 250, 0, 0) /* QUICKNESS_ATTRIBUTE */
     , (49114, 16, 170, 0, 0) /* FOCUS_ATTRIBUTE */
     , (49114, 32, 170, 0, 0) /* SELF_ATTRIBUTE */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (49114,   1,    1370, 0, 0, 1370) /* MaxHealth */
     , (49114,   3,    1740, 0, 0, 1740) /* MaxStamina */
     , (49114,   5,     1070, 0, 0, 1070) /* MaxMana */;
	 
INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (49114,  0,  4,  0,    0,    3,    3,    3,    3,    2,    2,    3,    1,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (49114,  1,  4,  0,    0,    7,    6,    7,    8,    4,    4,    7,    3,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (49114,  2,  4,  0,    0,    7,    6,    7,    8,    4,    4,    7,    3,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (49114,  3,  4,  0,    0,    5,    5,    5,    6,    3,    3,    5,    2,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (49114,  4,  4,  0,    0,    7,    6,    7,    8,    4,    4,    7,    3,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (49114,  5,  4,  2, 0.75,    5,    5,    5,    6,    3,    3,    5,    2,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (49114,  6,  4,  0,    0,    5,    5,    5,    6,    3,    3,    5,    2,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (49114,  7,  4,  0,    0,    5,    5,    5,    6,    3,    3,    5,    2,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (49114,  8,  4,  3, 0.75,    5,    5,    5,    6,    3,    3,    5,    2,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

