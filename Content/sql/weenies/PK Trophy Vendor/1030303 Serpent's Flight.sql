DELETE FROM `weenie` WHERE `class_Id` = 1030303;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030303, 'ace1030303-serpentsflight', 3, '2021-11-20 00:19:18') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030303,   1,        256) /* ItemType - MissileWeapon */
     , (1030303,   3,          4) /* PaletteTemplate - Brown */
     , (1030303,   5,          1) /* EncumbranceVal */
     , (1030303,   8,         90) /* Mass */
     , (1030303,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (1030303,  16,          1) /* ItemUseable - No */
     , (1030303,  19,         20) /* Value */
     , (1030303,  44,          0) /* Damage */
     , (1030303,  46,         16) /* DefaultCombatStyle - Bow */
     , (1030303,  49,         40) /* WeaponTime */
     , (1030303,  50,          1) /* AmmoType - Arrow */
     , (1030303,  51,          2) /* CombatUse - Missle */
     , (1030303,  52,          2) /* ParentLocation - LeftHand */
     , (1030303,  53,        101) /* PlacementPosition - Resting */
     , (1030303,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030303, 151,          2) /* HookType - Wall */
     , (1030303, 169,  118162702) /* TsysMutationData */
     , (1030303, 353,          8) /* WeaponType - Bow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030303,  11, True ) /* IgnoreCollisions */
     , (1030303,  13, True ) /* Ethereal */
     , (1030303,  14, True ) /* GravityStatus */
     , (1030303,  19, True ) /* Attackable */
     , (1030303,  22, True ) /* Inscribable */
     , (1030303, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030303,  12, 0.6600000262260437) /* Shade */
     , (1030303,  21,       0) /* WeaponLength */
     , (1030303,  29, 1.1799999475479126) /* WeaponDefense */
     , (1030303,  39, 1.2999999523162842) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030303,   1, 'Serpent''s Flight') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030303,   1,   33559367) /* Setup */
     , (1030303,   3,  536870932) /* SoundTable */
     , (1030303,   6,   67111919) /* PaletteBase */
     , (1030303,   8,  100686717) /* Icon */
     , (1030303,  22,  872415275) /* PhysicsEffectTable */
     , (1030303,  36,  234881042) /* MutateFilter */
     , (1030303,  46,  939524146) /* TsysMutationFilter */
     , (1030303,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-07T16:26:04.2674898-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
