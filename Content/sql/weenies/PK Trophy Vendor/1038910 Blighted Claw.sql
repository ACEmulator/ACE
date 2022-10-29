DELETE FROM `weenie` WHERE `class_Id` = 1038910;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1038910, 'ace1038910-blightedclaw', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1038910,   1,          1) /* ItemType - MeleeWeapon */
     , (1038910,   5,          1) /* EncumbranceVal */
     , (1038910,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1038910,  16,          1) /* ItemUseable - No */
     , (1038910,  19,         20) /* Value */
     , (1038910,  44,          1) /* Damage */
     , (1038910,  45,          1) /* DamageType - Slash */
     , (1038910,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (1038910,  47,          1) /* AttackType - Punch */
     , (1038910,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1038910,  49,          1) /* WeaponTime */
     , (1038910,  51,          1) /* CombatUse - Melee */
     , (1038910,  52,          1) /* ParentLocation - RightHand */
     , (1038910,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1038910,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1038910, 151,          2) /* HookType - Wall */
     , (1038910, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1038910,  11, True ) /* IgnoreCollisions */
     , (1038910,  13, True ) /* Ethereal */
     , (1038910,  14, True ) /* GravityStatus */
     , (1038910,  19, True ) /* Attackable */
     , (1038910,  22, True ) /* Inscribable */
     , (1038910,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1038910,  21,       0) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1038910,   1, 'Blighted Claw') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1038910,   1,   33560670) /* Setup */
     , (1038910,   3,  536870932) /* SoundTable */
     , (1038910,   8,  100690278) /* Icon */
     , (1038910,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:47:08.0134867-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
