INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`)
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
VALUES (49351, 69, 1) /* IS_SELLABLE_BOOL */;