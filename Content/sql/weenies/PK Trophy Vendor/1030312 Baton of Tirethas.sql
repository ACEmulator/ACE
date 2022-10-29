DELETE FROM `weenie` WHERE `class_Id` = 1030312;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030312, 'ace1030312-batonoftirethas', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030312,   1,          1) /* ItemType - MeleeWeapon */
     , (1030312,   5,          1) /* EncumbranceVal */
     , (1030312,   8,         90) /* Mass */
     , (1030312,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030312,  16,          1) /* ItemUseable - No */
     , (1030312,  19,         20) /* Value */
     , (1030312,  44,          1) /* Damage */
     , (1030312,  45,          4) /* DamageType - Bludgeon */
     , (1030312,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030312,  47,          4) /* AttackType - Slash */
     , (1030312,  48,         45) /* WeaponSkill - LightWeapons */
     , (1030312,  49,         50) /* WeaponTime */
     , (1030312,  51,          1) /* CombatUse - Melee */
     , (1030312,  52,          1) /* ParentLocation - RightHand */
     , (1030312,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030312, 151,          2) /* HookType - Wall */
     , (1030312, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030312,  11, True ) /* IgnoreCollisions */
     , (1030312,  13, True ) /* Ethereal */
     , (1030312,  14, True ) /* GravityStatus */
     , (1030312,  19, True ) /* Attackable */
     , (1030312,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030312,  21,       0) /* WeaponLength */
     , (1030312,  39, 1.100000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030312,   1, 'Baton of Tirethas') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030312,   1,   33559376) /* Setup */
     , (1030312,   3,  536870932) /* SoundTable */
     , (1030312,   6,   67111919) /* PaletteBase */
     , (1030312,   7,  268437600) /* ClothingBase */
     , (1030312,   8,  100686735) /* Icon */
     , (1030312,  22,  872415275) /* PhysicsEffectTable */
     , (1030312,  36,  234881042) /* MutateFilter */
     , (1030312,  46,  939524146) /* TsysMutationFilter */
     , (1030312,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:19:20.2864962-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
