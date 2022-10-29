DELETE FROM `weenie` WHERE `class_Id` = 1031500;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1031500, 'ace1031500-wornoldmace', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1031500,   1,          1) /* ItemType - MeleeWeapon */
     , (1031500,   5,          1) /* EncumbranceVal */
     , (1031500,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1031500,  16,          1) /* ItemUseable - No */
     , (1031500,  18,          1) /* UiEffects - Magical */
     , (1031500,  19,         20) /* Value */
     , (1031500,  44,          1) /* Damage */
     , (1031500,  45,          4) /* DamageType - Bludgeon */
     , (1031500,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1031500,  47,          4) /* AttackType - Slash */
     , (1031500,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1031500,  49,         40) /* WeaponTime */
     , (1031500,  51,          1) /* CombatUse - Melee */
     , (1031500,  53,        101) /* PlacementPosition - Resting */
     , (1031500,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1031500, 151,          2) /* HookType - Wall */
     , (1031500, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1031500,  11, True ) /* IgnoreCollisions */
     , (1031500,  13, True ) /* Ethereal */
     , (1031500,  14, True ) /* GravityStatus */
     , (1031500,  19, True ) /* Attackable */
     , (1031500,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1031500,  21,       0) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1031500,   1, 'Worn Old Mace') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1031500,   1,   33559579) /* Setup */
     , (1031500,   3,  536870932) /* SoundTable */
     , (1031500,   8,  100687920) /* Icon */
     , (1031500,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:39:31.101824-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
