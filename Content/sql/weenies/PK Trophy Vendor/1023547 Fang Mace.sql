DELETE FROM `weenie` WHERE `class_Id` = 1023547;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1023547, 'ace1023547-fangmace', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1023547,   1,          1) /* ItemType - MeleeWeapon */
     , (1023547,   5,          1) /* EncumbranceVal */
     , (1023547,   8,        360) /* Mass */
     , (1023547,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1023547,  16,          1) /* ItemUseable - No */
     , (1023547,  19,         20) /* Value */
     , (1023547,  44,          1) /* Damage */
     , (1023547,  45,          2) /* DamageType - Pierce */
     , (1023547,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1023547,  47,          4) /* AttackType - Slash */
     , (1023547,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1023547,  49,         60) /* WeaponTime */
     , (1023547,  51,          1) /* CombatUse - Melee */
     , (1023547,  53,        101) /* PlacementPosition - Resting */
     , (1023547,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1023547, 150,        103) /* HookPlacement - Hook */
     , (1023547, 151,          2) /* HookType - Wall */
     , (1023547, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1023547,  11, True ) /* IgnoreCollisions */
     , (1023547,  13, True ) /* Ethereal */
     , (1023547,  14, True ) /* GravityStatus */
     , (1023547,  19, True ) /* Attackable */
     , (1023547,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1023547,  21, 0.6200000047683716) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1023547,   1, 'Fang Mace') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1023547,   1,   33556993) /* Setup */
     , (1023547,   3,  536870932) /* SoundTable */
     , (1023547,   8,  100671417) /* Icon */
     , (1023547,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:03:07.1500009-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
