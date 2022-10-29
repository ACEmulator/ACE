DELETE FROM `weenie` WHERE `class_Id` = 1030323;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030323, 'ace1030323-tribladespear', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030323,   1,          1) /* ItemType - MeleeWeapon */
     , (1030323,   5,          1) /* EncumbranceVal */
     , (1030323,   8,         90) /* Mass */
     , (1030323,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030323,  19,         20) /* Value */
     , (1030323,  44,          1) /* Damage */
     , (1030323,  45,          2) /* DamageType - Pierce */
     , (1030323,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030323,  47,          2) /* AttackType - Thrust */
     , (1030323,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1030323,  49,         40) /* WeaponTime */
     , (1030323,  51,          1) /* CombatUse - Melee */
     , (1030323,  52,          1) /* ParentLocation - RightHand */
     , (1030323,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030323, 151,          2) /* HookType - Wall */
     , (1030323, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030323,  11, True ) /* IgnoreCollisions */
     , (1030323,  13, True ) /* Ethereal */
     , (1030323,  14, True ) /* GravityStatus */
     , (1030323,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030323,  21,       0) /* WeaponLength */
     , (1030323,  39, 1.100000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030323,   1, 'Tri-Blade Spear') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030323,   1,   33559387) /* Setup */
     , (1030323,   3,  536870932) /* SoundTable */
     , (1030323,   6,   67111919) /* PaletteBase */
     , (1030323,   7,  268437600) /* ClothingBase */
     , (1030323,   8,  100686757) /* Icon */
     , (1030323,  22,  872415275) /* PhysicsEffectTable */
     , (1030323,  36,  234881042) /* MutateFilter */
     , (1030323,  46,  939524146) /* TsysMutationFilter */
     , (1030323,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:44:18.1686786-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
