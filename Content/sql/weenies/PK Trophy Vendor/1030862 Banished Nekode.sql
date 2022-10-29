DELETE FROM `weenie` WHERE `class_Id` = 1030862;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030862, 'ace1030862-banishednekode', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030862,   1,          1) /* ItemType - MeleeWeapon */
     , (1030862,   5,          1) /* EncumbranceVal */
     , (1030862,   8,         90) /* Mass */
     , (1030862,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030862,  19,         20) /* Value */
     , (1030862,  44,          1) /* Damage */
     , (1030862,  45,          8) /* DamageType - Cold */
     , (1030862,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (1030862,  47,          1) /* AttackType - Punch */
     , (1030862,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1030862,  49,         20) /* WeaponTime */
     , (1030862,  51,          1) /* CombatUse - Melee */
     , (1030862,  53,        101) /* PlacementPosition - Resting */
     , (1030862,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030862, 150,        103) /* HookPlacement - Hook */
     , (1030862, 151,          2) /* HookType - Wall */
     , (1030862, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030862,  11, True ) /* IgnoreCollisions */
     , (1030862,  13, True ) /* Ethereal */
     , (1030862,  14, True ) /* GravityStatus */
     , (1030862,  19, True ) /* Attackable */
     , (1030862,  22, True ) /* Inscribable */
     , (1030862,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030862,  21, 0.5199999809265137) /* WeaponLength */
     , (1030862,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030862,   1, 'Banished Nekode') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030862,   1,   33559254) /* Setup */
     , (1030862,   3,  536870932) /* SoundTable */
     , (1030862,   8,  100677484) /* Icon */
     , (1030862,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:46:37.7320326-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
