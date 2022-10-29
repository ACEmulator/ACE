DELETE FROM `weenie` WHERE `class_Id` = 1030343;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030343, 'ace1030343-smite', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030343,   1,          1) /* ItemType - MeleeWeapon */
     , (1030343,   5,          1) /* EncumbranceVal */
     , (1030343,   8,         90) /* Mass */
     , (1030343,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030343,  16,          1) /* ItemUseable - No */
     , (1030343,  19,         20) /* Value */
     , (1030343,  44,          1) /* Damage */
     , (1030343,  45,          1) /* DamageType - Slash */
     , (1030343,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1030343,  47,          4) /* AttackType - Slash */
     , (1030343,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1030343,  49,         50) /* WeaponTime */
     , (1030343,  51,          1) /* CombatUse - Melee */
     , (1030343,  52,          1) /* ParentLocation - RightHand */
     , (1030343,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030343, 151,          2) /* HookType - Wall */
     , (1030343, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030343,  11, True ) /* IgnoreCollisions */
     , (1030343,  13, True ) /* Ethereal */
     , (1030343,  14, True ) /* GravityStatus */
     , (1030343,  19, True ) /* Attackable */
     , (1030343,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030343,  21,       0) /* WeaponLength */
     , (1030343,  39, 1.100000023841858) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030343,   1, 'Smite') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030343,   1,   33559322) /* Setup */
     , (1030343,   3,  536870932) /* SoundTable */
     , (1030343,   6,   67111919) /* PaletteBase */
     , (1030343,   7,  268437600) /* ClothingBase */
     , (1030343,   8,  100686797) /* Icon */
     , (1030343,  22,  872415275) /* PhysicsEffectTable */
     , (1030343,  36,  234881042) /* MutateFilter */
     , (1030343,  46,  939524146) /* TsysMutationFilter */
     , (1030343,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:49:13.342029-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
