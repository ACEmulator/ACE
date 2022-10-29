DELETE FROM `weenie` WHERE `class_Id` = 1026599;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1026599, 'ace1026599-esorcelledfalchion', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1026599,   1,          1) /* ItemType - MeleeWeapon */
     , (1026599,   3,          8) /* PaletteTemplate - Green */
     , (1026599,   5,          1) /* EncumbranceVal */
     , (1026599,   8,        180) /* Mass */
     , (1026599,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1026599,  19,         20) /* Value */
     , (1026599,  44,          1) /* Damage */
     , (1026599,  45,          3) /* DamageType - Slash, Pierce */
     , (1026599,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1026599,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (1026599,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1026599,  49,         40) /* WeaponTime */
     , (1026599,  51,          1) /* CombatUse - Melee */
     , (1026599,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1026599, 150,        103) /* HookPlacement - Hook */
     , (1026599, 151,          2) /* HookType - Wall */
     , (1026599, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1026599,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1026599,  21, 0.9599999785423279) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1026599,   1, 'Esorcelled Falchion') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1026599,   1,   33558592) /* Setup */
     , (1026599,   3,  536870932) /* SoundTable */
     , (1026599,   6,   67114956) /* PaletteBase */
     , (1026599,   7,  268436792) /* ClothingBase */
     , (1026599,   8,  100675773) /* Icon */
     , (1026599,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:53:06.2450687-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Custom",
  "IsDone": true
}
*/
