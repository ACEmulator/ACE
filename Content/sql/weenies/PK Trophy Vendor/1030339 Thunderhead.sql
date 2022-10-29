DELETE FROM `weenie` WHERE `class_Id` = 1030339;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030339, 'ace1030339-thunderhead', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030339,   1,          1) /* ItemType - MeleeWeapon */
     , (1030339,   5,          1) /* EncumbranceVal */
     , (1030339,   8,         90) /* Mass */
     , (1030339,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030339,  16,          1) /* ItemUseable - No */
     , (1030339,  18,         64) /* UiEffects - Lightning */
     , (1030339,  19,         20) /* Value */
     , (1030339,  44,          1) /* Damage */
     , (1030339,  45,         64) /* DamageType - Electric */
     , (1030339,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030339,  47,          4) /* AttackType - Slash */
     , (1030339,  48,         45) /* WeaponSkill - LightWeapons */
     , (1030339,  49,         55) /* WeaponTime */
     , (1030339,  51,          1) /* CombatUse - Melee */
     , (1030339,  52,          1) /* ParentLocation - RightHand */
     , (1030339,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030339, 151,          2) /* HookType - Wall */
     , (1030339, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030339,  11, True ) /* IgnoreCollisions */
     , (1030339,  13, True ) /* Ethereal */
     , (1030339,  14, True ) /* GravityStatus */
     , (1030339,  19, True ) /* Attackable */
     , (1030339,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030339,  21,       0) /* WeaponLength */
     , (1030339,  39, 1.100000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030339,   1, 'Thunderhead') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030339,   1,   33559403) /* Setup */
     , (1030339,   3,  536870932) /* SoundTable */
     , (1030339,   6,   67111919) /* PaletteBase */
     , (1030339,   7,  268437600) /* ClothingBase */
     , (1030339,   8,  100686789) /* Icon */
     , (1030339,  22,  872415275) /* PhysicsEffectTable */
     , (1030339,  36,  234881042) /* MutateFilter */
     , (1030339,  46,  939524146) /* TsysMutationFilter */
     , (1030339,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:17:57.5514197-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
