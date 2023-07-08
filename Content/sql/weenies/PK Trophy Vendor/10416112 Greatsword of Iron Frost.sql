DELETE FROM `weenie` WHERE `class_Id` = 10416112;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10416112, 'ace10416112-greatswordofironfrost', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10416112,   1,          1) /* ItemType - MeleeWeapon */
     , (10416112,   5,       0) /* EncumbranceVal */
     , (10416112,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10416112,  16,          1) /* ItemUseable - No */
     , (10416112,  18,        128) /* UiEffects - Frost */
     , (10416112,  19,         20) /* Value */
     , (10416112,  45,          8) /* DamageType - Cold */
     , (10416112,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10416112,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (10416112,  51,          5) /* CombatUse - TwoHanded */
     , (10416112,  52,          1) /* ParentLocation - RightHand */
     , (10416112,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10416112, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10416112,  11, True ) /* IgnoreCollisions */
     , (10416112,  13, True ) /* Ethereal */
     , (10416112,  14, True ) /* GravityStatus */
     , (10416112,  15, True ) /* LightsStatus */
     , (10416112,  19, True ) /* Attackable */
     , (10416112,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10416112,   1, 'Greatsword of Iron Frost') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10416112,   1,   33560867) /* Setup */
     , (10416112,   3,  536870932) /* SoundTable */
     , (10416112,   8,  100690588) /* Icon */
     , (10416112,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T01:51:34.1005504-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
