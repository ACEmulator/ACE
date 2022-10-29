DELETE FROM `weenie` WHERE `class_Id` = 1038926;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1038926, 'ace1038926-tthuunmace', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1038926,   1,          1) /* ItemType - MeleeWeapon */
     , (1038926,   5,        350) /* EncumbranceVal */
     , (1038926,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1038926,  16,          1) /* ItemUseable - No */
     , (1038926,  19,         20) /* Value */
     , (1038926,  44,          1) /* Damage */
     , (1038926,  45,          4) /* DamageType - Bludgeon */
     , (1038926,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1038926,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1038926,  51,          1) /* CombatUse - Melee */
     , (1038926,  52,          1) /* ParentLocation - RightHand */
     , (1038926,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1038926,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1038926, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1038926,  11, True ) /* IgnoreCollisions */
     , (1038926,  13, True ) /* Ethereal */
     , (1038926,  14, True ) /* GravityStatus */
     , (1038926,  19, True ) /* Attackable */
     , (1038926,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1038926,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1038926,   1, 'T''thuun Mace') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1038926,   1,   33560673) /* Setup */
     , (1038926,   3,  536870932) /* SoundTable */
     , (1038926,   8,  100690281) /* Icon */
     , (1038926,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:48:33.8819678-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
