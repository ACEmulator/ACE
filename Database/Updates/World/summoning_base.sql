INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`)
VALUES ('49344', 'blisteringmoaressence', 70) /* Pet Device */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (49344, 65, 101) /* PLACEMENT_INT */
     , (49344, 1, 128) /* ITEM_TYPE_INT */
     , (49344, 5, 50) /* ENCUMB_VAL_INT */
     , (49344, 280, 213) /* SHARED_COOLDOWN_INT */
     , (49344, 18, 256) /* UI_EFFECTS_INT */
     , (49344, 91, 50) /* MAX_STRUCTURE_INT */
     , (49344, 92, 50) /* STRUCTURE_INT */
     , (49344, 94, 16) /* TARGET_TYPE_INT */
     , (49344, 16, 8) /* ITEM_USEABLE_INT */
     , (49344, 19, 10000) /* VALUE_INT */
     , (49344, 93, 1044) /* PHYSICS_STATE_INT */
     , (49344, 9007, 70) /* PetDevice_WeenieType */;
	 
INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (49344, 13, True) /* ETHEREAL_BOOL */
     , (49344, 11, True) /* IGNORE_COLLISIONS_BOOL */
     , (49344, 14, True) /* GRAVITY_STATUS_BOOL */
     , (49344, 19, True) /* ATTACKABLE_BOOL */
     , (49344, 22, True) /* INSCRIBABLE_BOOL */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (49344, 167, 45) /* COOLDOWN_DURATION_FLOAT */
     , (49344, 39, 0.4) /* DEFAULT_SCALE_FLOAT */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (49344, 1, 'Blistering Moar Essence') /* NAME_STRING */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (49344, 8, 100693034) /* ICON_DID */
     , (49344, 50, 100693032) /* ICON_OVERLAY_DID */
     , (49344, 52, 100693024) /* ICON_UNDERLAY_DID */
     , (49344, 1, 33554817) /* SETUP_DID */
     , (49344, 3, 536870932) /* SOUND_TABLE_DID */
     , (49344, 22, 872415275) /* PHYSICS_EFFECT_TABLE_DID */
     , (49344, 6, 67111919) /* PALETTE_BASE_DID */;

/* Extended Appraisal Data */

REPLACE INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (49344, 14, 'Use this essence to summon or dismiss your Blistering Moar.') /* USE_STRING */;

REPLACE INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (49344, 368, 54) /* USE_REQUIRES_SKILL_SPEC_INT */
     , (49344, 33, 0) /* BONDED_INT */
     , (49344, 369, 185) /* USE_REQUIRES_LEVEL_INT */
     , (49344, 114, 0) /* ATTUNED_INT */
     , (49344, 370, 16) /* GEAR_DAMAGE_INT */
     , (49344, 19, 10000) /* VALUE_INT */
     , (49344, 372, 13) /* GEAR_CRIT_INT */
     , (49344, 5, 50) /* ENCUMB_VAL_INT */
     , (49344, 373, 11) /* GEAR_CRIT_RESIST_INT */
     , (49344, 374, 13) /* GEAR_CRIT_DAMAGE_INT */
     , (49344, 280, 213) /* SHARED_COOLDOWN_INT */
     , (49344, 105, 6) /* ITEM_WORKMANSHIP_INT */
     , (49344, 91, 50) /* MAX_STRUCTURE_INT */
     , (49344, 366, 54) /* USE_REQUIRES_SKILL_INT */
     , (49344, 367, 570) /* USE_REQUIRES_SKILL_LEVEL_INT */;

REPLACE INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (49344, 167, 45) /* COOLDOWN_DURATION_FLOAT */;

REPLACE INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (49344, 69, 1) /* IS_SELLABLE_BOOL */;INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`)
VALUES ('49351', 'electrifiedmoaressence', 10) /* Pet */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (49351, 65, 101) /* PLACEMENT_INT */
     , (49351, 1, 128) /* ITEM_TYPE_INT */
     , (49351, 5, 50) /* ENCUMB_VAL_INT */
     , (49351, 280, 213) /* SHARED_COOLDOWN_INT */
     , (49351, 18, 64) /* UI_EFFECTS_INT */
     , (49351, 91, 50) /* MAX_STRUCTURE_INT */
     , (49351, 92, 50) /* STRUCTURE_INT */
     , (49351, 94, 16) /* TARGET_TYPE_INT */
     , (49351, 16, 8) /* ITEM_USEABLE_INT */
     , (49351, 19, 10000) /* VALUE_INT */
     , (49351, 93, 1044) /* PHYSICS_STATE_INT */
     , (49351, 9007, 70) /* PetDevice_WeenieType */;
	 
INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (49351, 13, True) /* ETHEREAL_BOOL */
     , (49351, 11, True) /* IGNORE_COLLISIONS_BOOL */
     , (49351, 14, True) /* GRAVITY_STATUS_BOOL */
     , (49351, 19, True) /* ATTACKABLE_BOOL */
     , (49351, 22, True) /* INSCRIBABLE_BOOL */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (49351, 167, 45) /* COOLDOWN_DURATION_FLOAT */
     , (49351, 39, 0.4) /* DEFAULT_SCALE_FLOAT */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (49351, 1, 'Electrified Moar Essence') /* NAME_STRING */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (49351, 8, 100693034) /* ICON_DID */
     , (49351, 50, 100693032) /* ICON_OVERLAY_DID */
     , (49351, 52, 100693024) /* ICON_UNDERLAY_DID */
     , (49351, 1, 33554817) /* SETUP_DID */
     , (49351, 3, 536870932) /* SOUND_TABLE_DID */
     , (49351, 22, 872415275) /* PHYSICS_EFFECT_TABLE_DID */
     , (49351, 6, 67111919) /* PALETTE_BASE_DID */;

/* Extended Appraisal Data */

REPLACE INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (49351, 14, 'Use this essence to summon or dismiss your Electrified Moar.') /* USE_STRING */;

REPLACE INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (49351, 368, 54) /* USE_REQUIRES_SKILL_SPEC_INT */
     , (49351, 33, 0) /* BONDED_INT */
     , (49351, 369, 185) /* USE_REQUIRES_LEVEL_INT */
     , (49351, 114, 0) /* ATTUNED_INT */
     , (49351, 19, 10000) /* VALUE_INT */
     , (49351, 5, 50) /* ENCUMB_VAL_INT */
     , (49351, 373, 14) /* GEAR_CRIT_RESIST_INT */
     , (49351, 374, 9) /* GEAR_CRIT_DAMAGE_INT */
     , (49351, 375, 17) /* GEAR_CRIT_DAMAGE_RESIST_INT */
     , (49351, 280, 213) /* SHARED_COOLDOWN_INT */
     , (49351, 105, 7) /* ITEM_WORKMANSHIP_INT */
     , (49351, 91, 50) /* MAX_STRUCTURE_INT */
     , (49351, 366, 54) /* USE_REQUIRES_SKILL_INT */
     , (49351, 367, 570) /* USE_REQUIRES_SKILL_LEVEL_INT */;

REPLACE INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (49351, 167, 45) /* COOLDOWN_DURATION_FLOAT */;

REPLACE INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (49351, 69, 1) /* IS_SELLABLE_BOOL */;INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`)
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

