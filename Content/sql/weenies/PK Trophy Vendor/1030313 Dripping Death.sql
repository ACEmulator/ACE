DELETE FROM `weenie` WHERE `class_Id` = 1030313;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030313, 'ace1030313-drippingdeath', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030313,   1,          1) /* ItemType - MeleeWeapon */
     , (1030313,   5,          1) /* EncumbranceVal */
     , (1030313,   8,         90) /* Mass */
     , (1030313,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030313,  18,        256) /* UiEffects - Acid */
     , (1030313,  19,         20) /* Value */
     , (1030313,  44,          1) /* Damage */
     , (1030313,  45,         32) /* DamageType - Acid */
     , (1030313,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030313,  47,          4) /* AttackType - Slash */
     , (1030313,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1030313,  49,         50) /* WeaponTime */
     , (1030313,  51,          1) /* CombatUse - Melee */
     , (1030313,  52,          1) /* ParentLocation - RightHand */
     , (1030313,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030313, 151,          2) /* HookType - Wall */
     , (1030313, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030313,  11, True ) /* IgnoreCollisions */
     , (1030313,  13, True ) /* Ethereal */
     , (1030313,  14, True ) /* GravityStatus */
     , (1030313,  19, True ) /* Attackable */
     , (1030313,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030313,  21,       0) /* WeaponLength */
     , (1030313,  39, 1.100000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030313,   1, 'Dripping Death') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030313,   1,   33559377) /* Setup */
     , (1030313,   3,  536870932) /* SoundTable */
     , (1030313,   6,   67111919) /* PaletteBase */
     , (1030313,   7,  268437600) /* ClothingBase */
     , (1030313,   8,  100686737) /* Icon */
     , (1030313,  22,  872415275) /* PhysicsEffectTable */
     , (1030313,  36,  234881042) /* MutateFilter */
     , (1030313,  46,  939524146) /* TsysMutationFilter */
     , (1030313,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:45:52.8736473-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
