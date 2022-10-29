DELETE FROM `weenie` WHERE `class_Id` = 1030351;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030351, 'ace1030351-dragonspinebow', 3, '2021-11-20 00:19:18') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030351,   1,        256) /* ItemType - MissileWeapon */
     , (1030351,   3,          4) /* PaletteTemplate - Brown */
     , (1030351,   5,          1) /* EncumbranceVal */
     , (1030351,   8,         90) /* Mass */
     , (1030351,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (1030351,  16,          1) /* ItemUseable - No */
     , (1030351,  18,       2048) /* UiEffects - Piercing */
     , (1030351,  19,         20) /* Value */
     , (1030351,  44,          1) /* Damage */
     , (1030351,  46,         16) /* DefaultCombatStyle - Bow */
     , (1030351,  49,         70) /* WeaponTime */
     , (1030351,  50,          1) /* AmmoType - Arrow */
     , (1030351,  51,          2) /* CombatUse - Missle */
     , (1030351,  52,          2) /* ParentLocation - LeftHand */
     , (1030351,  53,        101) /* PlacementPosition - Resting */
     , (1030351,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030351, 151,          2) /* HookType - Wall */
     , (1030351, 169,  118162702) /* TsysMutationData */
     , (1030351, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030351,  11, True ) /* IgnoreCollisions */
     , (1030351,  13, True ) /* Ethereal */
     , (1030351,  14, True ) /* GravityStatus */
     , (1030351,  19, True ) /* Attackable */
     , (1030351,  22, True ) /* Inscribable */
     , (1030351,  91, False) /* Retained */
     , (1030351, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030351,  12, 0.6600000262260437) /* Shade */
     , (1030351,  21,       0) /* WeaponLength */
     , (1030351,  39, 1.2999999523162842) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030351,   1, 'Dragonspine Bow') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030351,   1,   33559414) /* Setup */
     , (1030351,   3,  536870932) /* SoundTable */
     , (1030351,   6,   67111919) /* PaletteBase */
     , (1030351,   8,  100686812) /* Icon */
     , (1030351,  22,  872415275) /* PhysicsEffectTable */
     , (1030351,  36,  234881042) /* MutateFilter */
     , (1030351,  46,  939524146) /* TsysMutationFilter */
     , (1030351,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-07T16:26:20.6565713-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
