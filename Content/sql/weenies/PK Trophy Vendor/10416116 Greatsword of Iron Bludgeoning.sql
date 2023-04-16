DELETE FROM `weenie` WHERE `class_Id` = 10416116;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10416116, 'ace10416116-greatswordofironbludgeoning', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10416116,   1,          1) /* ItemType - MeleeWeapon */
     , (10416116,   5,       0) /* EncumbranceVal */
     , (10416116,   9,   33554432) /* ValidLocations - TwoHanded */
     , (10416116,  16,          1) /* ItemUseable - No */
     , (10416116,  18,        512) /* UiEffects - Bludgeoning */
     , (10416116,  19,         20) /* Value */
     , (10416116,  45,          4) /* DamageType - Bludgeon */
     , (10416116,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (10416116,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (10416116,  51,          5) /* CombatUse - TwoHanded */
     , (10416116,  52,          1) /* ParentLocation - RightHand */
     , (10416116,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (10416116, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10416116,  11, True ) /* IgnoreCollisions */
     , (10416116,  13, True ) /* Ethereal */
     , (10416116,  14, True ) /* GravityStatus */
     , (10416116,  15, True ) /* LightsStatus */
     , (10416116,  19, True ) /* Attackable */
     , (10416116,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10416116,   1, 'Greatsword of Iron Bludgeoning') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10416116,   1,   33560867) /* Setup */
     , (10416116,   3,  536870932) /* SoundTable */
     , (10416116,   8,  100690588) /* Icon */
     , (10416116,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T01:49:24.6024714-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
