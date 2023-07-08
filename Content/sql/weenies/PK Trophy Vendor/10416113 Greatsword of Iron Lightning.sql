DELETE FROM `weenie` WHERE `class_Id` = 10416113;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10416113, 'ace10416113-greatswordofironlightning', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10416113,   1,          1) /* ItemType - MeleeWeapon */
     , (10416113,   5,       0) /* EncumbranceVal */
     , (10416113,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10416113,  16,          1) /* ItemUseable - No */
     , (10416113,  18,         64) /* UiEffects - Lightning */
     , (10416113,  19,         20) /* Value */
     , (10416113,  45,         64) /* DamageType - Electric */
     , (10416113,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10416113,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (10416113,  51,          5) /* CombatUse - TwoHanded */
     , (10416113,  52,          1) /* ParentLocation - RightHand */
     , (10416113,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10416113, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10416113,  11, True ) /* IgnoreCollisions */
     , (10416113,  13, True ) /* Ethereal */
     , (10416113,  14, True ) /* GravityStatus */
     , (10416113,  15, True ) /* LightsStatus */
     , (10416113,  19, True ) /* Attackable */
     , (10416113,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10416113,   1, 'Greatsword of Iron Lightning') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10416113,   1,   33560867) /* Setup */
     , (10416113,   3,  536870932) /* SoundTable */
     , (10416113,   8,  100690588) /* Icon */
     , (10416113,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T01:51:17.4359009-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
