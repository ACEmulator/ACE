DELETE FROM `weenie` WHERE `class_Id` = 10416114;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10416114, 'ace10416114-greatswordofironacid', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10416114,   1,          1) /* ItemType - MeleeWeapon */
     , (10416114,   5,       0) /* EncumbranceVal */
     , (10416114,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10416114,  16,          1) /* ItemUseable - No */
     , (10416114,  18,        256) /* UiEffects - Acid */
     , (10416114,  19,         20) /* Value */
     , (10416114,  45,         32) /* DamageType - Acid */
     , (10416114,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10416114,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (10416114,  51,          5) /* CombatUse - TwoHanded */
     , (10416114,  52,          1) /* ParentLocation - RightHand */
     , (10416114,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10416114, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10416114,  11, True ) /* IgnoreCollisions */
     , (10416114,  13, True ) /* Ethereal */
     , (10416114,  14, True ) /* GravityStatus */
     , (10416114,  15, True ) /* LightsStatus */
     , (10416114,  19, True ) /* Attackable */
     , (10416114,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10416114,   1, 'Greatsword of Iron Acid') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10416114,   1,   33560867) /* Setup */
     , (10416114,   3,  536870932) /* SoundTable */
     , (10416114,   8,  100690588) /* Icon */
     , (10416114,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T01:50:35.4443058-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
