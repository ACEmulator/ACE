DELETE FROM `weenie` WHERE `class_Id` = 1042662;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1042662, 'ace1042662-chitincracker', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1042662,   1,          1) /* ItemType - MeleeWeapon */
     , (1042662,   5,          1) /* EncumbranceVal */
     , (1042662,   8,         90) /* Mass */
     , (1042662,   9,   33554432) /* ValidLocations - TwoHanded */
     , (1042662,  16,          1) /* ItemUseable - No */
     , (1042662,  19,         20) /* Value */
     , (1042662,  44,          1) /* Damage */
     , (1042662,  45,          4) /* DamageType - Bludgeon */
     , (1042662,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (1042662,  47,          4) /* AttackType - Slash */
     , (1042662,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (1042662,  49,         50) /* WeaponTime */
     , (1042662,  51,          1) /* CombatUse - Melee */
     , (1042662,  52,          1) /* ParentLocation - RightHand */
     , (1042662,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1042662, 151,          2) /* HookType - Wall */
     , (1042662, 292,          2) /* Cleaving */
     , (1042662, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1042662,  11, True ) /* IgnoreCollisions */
     , (1042662,  13, True ) /* Ethereal */
     , (1042662,  14, True ) /* GravityStatus */
     , (1042662,  19, True ) /* Attackable */
     , (1042662,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1042662,  21,       1) /* WeaponLength */
     , (1042662,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1042662,   1, 'Chitin Cracker') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1042662,   1,   33561144) /* Setup */
     , (1042662,   3,  536870932) /* SoundTable */
     , (1042662,   6,   67111919) /* PaletteBase */
     , (1042662,   7,  268437600) /* ClothingBase */
     , (1042662,   8,  100691764) /* Icon */
     , (1042662,  22,  872415275) /* PhysicsEffectTable */
     , (1042662,  36,  234881042) /* MutateFilter */
     , (1042662,  46,  939524146) /* TsysMutationFilter */
     , (1042662,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T20:48:54.5630821-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
