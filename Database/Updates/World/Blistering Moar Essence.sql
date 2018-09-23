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
VALUES (49344, 69, 1) /* IS_SELLABLE_BOOL */;