DELETE FROM `weenie` WHERE `class_Id` = 1030335;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030335, 'ace1030335-hevelioshalfmoon', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030335,   1,          1) /* ItemType - MeleeWeapon */
     , (1030335,   5,          1) /* EncumbranceVal */
     , (1030335,   8,         90) /* Mass */
     , (1030335,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1030335,  19,         20) /* Value */
     , (1030335,  44,          1) /* Damage */
     , (1030335,  45,          2) /* DamageType - Pierce */
     , (1030335,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (1030335,  47,          1) /* AttackType - Punch */
     , (1030335,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1030335,  49,         20) /* WeaponTime */
     , (1030335,  51,          1) /* CombatUse - Melee */
     , (1030335,  52,          1) /* ParentLocation - RightHand */
     , (1030335,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030335, 151,          2) /* HookType - Wall */
     , (1030335, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030335,  11, True ) /* IgnoreCollisions */
     , (1030335,  13, True ) /* Ethereal */
     , (1030335,  14, True ) /* GravityStatus */
     , (1030335,  19, True ) /* Attackable */
     , (1030335,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030335,  21,       0) /* WeaponLength */
     , (1030335,  39, 0.699999988079071) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030335,   1, 'Hevelio''s Half-Moon') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030335,   1,   33559399) /* Setup */
     , (1030335,   3,  536870932) /* SoundTable */
     , (1030335,   6,   67111919) /* PaletteBase */
     , (1030335,   7,  268437600) /* ClothingBase */
     , (1030335,   8,  100686781) /* Icon */
     , (1030335,  22,  872415275) /* PhysicsEffectTable */
     , (1030335,  36,  234881042) /* MutateFilter */
     , (1030335,  46,  939524146) /* TsysMutationFilter */
     , (1030335,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T09:45:06.706762-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
