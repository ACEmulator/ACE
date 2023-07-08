DELETE FROM `weenie` WHERE `class_Id` = 10416115;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10416115, 'ace10416115-greatswordofironslashingandpiercing', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10416115,   1,          1) /* ItemType - MeleeWeapon */
     , (10416115,   5,       0) /* EncumbranceVal */
     , (10416115,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10416115,  16,          1) /* ItemUseable - No */
     , (10416115,  18,       1024) /* UiEffects - Slashing */
     , (10416115,  19,         20) /* Value */
     , (10416115,  45,          3) /* DamageType - Slash, Pierce */
     , (10416115,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10416115,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (10416115,  51,          5) /* CombatUse - TwoHanded */
     , (10416115,  52,          1) /* ParentLocation - RightHand */
     , (10416115,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10416115, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10416115,  11, True ) /* IgnoreCollisions */
     , (10416115,  13, True ) /* Ethereal */
     , (10416115,  14, True ) /* GravityStatus */
     , (10416115,  15, True ) /* LightsStatus */
     , (10416115,  19, True ) /* Attackable */
     , (10416115,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10416115,   1, 'Greatsword of Iron Slashing and Piercing') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10416115,   1,   33560867) /* Setup */
     , (10416115,   3,  536870932) /* SoundTable */
     , (10416115,   8,  100690588) /* Icon */
     , (10416115,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T01:50:49.7362198-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
