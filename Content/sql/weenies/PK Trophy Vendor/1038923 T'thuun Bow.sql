DELETE FROM `weenie` WHERE `class_Id` = 1038923;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1038923, 'ace1038923-tthuunbow', 3, '2021-11-20 00:19:18') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1038923,   1,        256) /* ItemType - MissileWeapon */
     , (1038923,   5,          1) /* EncumbranceVal */
     , (1038923,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (1038923,  16,          1) /* ItemUseable - No */
     , (1038923,  19,         20) /* Value */
     , (1038923,  44,          1) /* Damage */
     , (1038923,  46,         16) /* DefaultCombatStyle - Bow */
     , (1038923,  50,          1) /* AmmoType - Arrow */
     , (1038923,  51,          2) /* CombatUse - Missle */
     , (1038923,  52,          2) /* ParentLocation - LeftHand */
     , (1038923,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1038923, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1038923,  11, True ) /* IgnoreCollisions */
     , (1038923,  13, True ) /* Ethereal */
     , (1038923,  14, True ) /* GravityStatus */
     , (1038923,  19, True ) /* Attackable */
     , (1038923,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1038923,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1038923,   1, 'T''thuun Bow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1038923,   1,   33560669) /* Setup */
     , (1038923,   3,  536870932) /* SoundTable */
     , (1038923,   8,  100690277) /* Icon */
     , (1038923,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-07T16:25:19.5596252-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
