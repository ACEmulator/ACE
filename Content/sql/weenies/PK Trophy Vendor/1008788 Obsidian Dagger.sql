DELETE FROM `weenie` WHERE `class_Id` = 1008788;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008788, 'ace1008788-obsidiandagger', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008788,   1,          1) /* ItemType - MeleeWeapon */
     , (1008788,   3,         39) /* PaletteTemplate - Black */
     , (1008788,   5,          1) /* EncumbranceVal */
     , (1008788,   8,         90) /* Mass */
     , (1008788,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1008788,  16,          1) /* ItemUseable - No */
     , (1008788,  19,         20) /* Value */
     , (1008788,  44,          1) /* Damage */
     , (1008788,  45,          3) /* DamageType - Slash, Pierce */
     , (1008788,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1008788,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (1008788,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1008788,  49,         20) /* WeaponTime */
     , (1008788,  51,          1) /* CombatUse - Melee */
     , (1008788,  52,          1) /* ParentLocation - RightHand */
     , (1008788,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1008788,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1008788, 150,        103) /* HookPlacement - Hook */
     , (1008788, 151,          2) /* HookType - Wall */
     , (1008788, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008788,  11, True ) /* IgnoreCollisions */
     , (1008788,  13, True ) /* Ethereal */
     , (1008788,  14, True ) /* GravityStatus */
     , (1008788,  19, True ) /* Attackable */
     , (1008788,  22, True ) /* Inscribable */
     , (1008788,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008788,  21, 0.4000000059604645) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008788,   1, 'Obsidian Dagger') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008788,   1,   33554887) /* Setup */
     , (1008788,   3,  536870932) /* SoundTable */
     , (1008788,   6,   67111919) /* PaletteBase */
     , (1008788,   7,  268436097) /* ClothingBase */
     , (1008788,   8,  100671248) /* Icon */
     , (1008788,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:09:10.2900924-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
