DELETE FROM `weenie` WHERE `class_Id` = 1008473;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008473, 'ace1008473-finespineaxe', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008473,   1,          1) /* ItemType - MeleeWeapon */
     , (1008473,   5,          1) /* EncumbranceVal */
     , (1008473,   8,        270) /* Mass */
     , (1008473,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1008473,  16,          1) /* ItemUseable - No */
     , (1008473,  18,        128) /* UiEffects - Frost */
     , (1008473,  19,         20) /* Value */
     , (1008473,  44,          1) /* Damage */
     , (1008473,  45,          8) /* DamageType - Cold */
     , (1008473,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1008473,  47,          4) /* AttackType - Slash */
     , (1008473,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1008473,  49,         40) /* WeaponTime */
     , (1008473,  51,          1) /* CombatUse - Melee */
     , (1008473,  53,        101) /* PlacementPosition - Resting */
     , (1008473,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1008473, 150,        103) /* HookPlacement - Hook */
     , (1008473, 151,          2) /* HookType - Wall */
     , (1008473, 353,          3) /* WeaponType - Axe */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008473,  11, True ) /* IgnoreCollisions */
     , (1008473,  13, True ) /* Ethereal */
     , (1008473,  14, True ) /* GravityStatus */
     , (1008473,  19, True ) /* Attackable */
     , (1008473,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008473,  21,    0.75) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008473,   1, 'Fine Spine Axe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008473,   1,   33558225) /* Setup */
     , (1008473,   3,  536870932) /* SoundTable */
     , (1008473,   6,   67114170) /* PaletteBase */
     , (1008473,   8,  100674101) /* Icon */
     , (1008473,  19,         88) /* ActivationAnimation */
     , (1008473,  22,  872415275) /* PhysicsEffectTable */
     , (1008473,  30,         87) /* PhysicsScript - BreatheLightning */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:05:10.8648201-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
