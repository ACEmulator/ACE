DELETE FROM `weenie` WHERE `class_Id` = 10416111;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10416111, 'ace10416111-greatswordofironflame', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10416111,   1,          1) /* ItemType - MeleeWeapon */
     , (10416111,   5,       0) /* EncumbranceVal */
     , (10416111,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10416111,  16,          1) /* ItemUseable - No */
     , (10416111,  18,         32) /* UiEffects - Fire */
     , (10416111,  19,         20) /* Value */
     , (10416111,  45,         16) /* DamageType - Fire */
     , (10416111,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10416111,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (10416111,  51,          5) /* CombatUse - TwoHanded */
     , (10416111,  52,          1) /* ParentLocation - RightHand */
     , (10416111,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10416111, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10416111,  11, True ) /* IgnoreCollisions */
     , (10416111,  13, True ) /* Ethereal */
     , (10416111,  14, True ) /* GravityStatus */
     , (10416111,  15, True ) /* LightsStatus */
     , (10416111,  19, True ) /* Attackable */
     , (10416111,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10416111,   1, 'Greatsword of Iron Flame') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10416111,   1,   33560867) /* Setup */
     , (10416111,   3,  536870932) /* SoundTable */
     , (10416111,   8,  100690588) /* Icon */
     , (10416111,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T01:52:53.3182204-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
