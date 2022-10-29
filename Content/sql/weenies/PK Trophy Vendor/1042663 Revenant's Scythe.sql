DELETE FROM `weenie` WHERE `class_Id` = 1042663;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1042663, 'ace1042663-revenantsscythe', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1042663,   1,          1) /* ItemType - MeleeWeapon */
     , (1042663,   5,          1) /* EncumbranceVal */
     , (1042663,   8,         90) /* Mass */
     , (1042663,   9,   33554432) /* ValidLocations - TwoHanded */
     , (1042663,  16,          1) /* ItemUseable - No */
     , (1042663,  19,         20) /* Value */
     , (1042663,  44,          1) /* Damage */
     , (1042663,  45,          1) /* DamageType - Slash */
     , (1042663,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (1042663,  47,          4) /* AttackType - Slash */
     , (1042663,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (1042663,  49,         50) /* WeaponTime */
     , (1042663,  51,          5) /* CombatUse - TwoHanded */
     , (1042663,  52,          1) /* ParentLocation - RightHand */
     , (1042663,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1042663, 151,          2) /* HookType - Wall */
     , (1042663, 292,          2) /* Cleaving */
     , (1042663, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1042663,  11, True ) /* IgnoreCollisions */
     , (1042663,  13, True ) /* Ethereal */
     , (1042663,  14, True ) /* GravityStatus */
     , (1042663,  19, True ) /* Attackable */
     , (1042663,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1042663,  21,       1) /* WeaponLength */
     , (1042663,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1042663,   1, 'Revenant''s Scythe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1042663,   1,   33561149) /* Setup */
     , (1042663,   3,  536870932) /* SoundTable */
     , (1042663,   6,   67111919) /* PaletteBase */
     , (1042663,   7,  268437600) /* ClothingBase */
     , (1042663,   8,  100691763) /* Icon */
     , (1042663,  22,  872415275) /* PhysicsEffectTable */
     , (1042663,  36,  234881042) /* MutateFilter */
     , (1042663,  46,  939524146) /* TsysMutationFilter */
     , (1042663,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T20:49:30.1929565-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
