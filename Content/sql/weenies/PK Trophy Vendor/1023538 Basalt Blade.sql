DELETE FROM `weenie` WHERE `class_Id` = 1023538;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1023538, 'ace1023538-basaltblade', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1023538,   1,          1) /* ItemType - MeleeWeapon */
     , (1023538,   5,          1) /* EncumbranceVal */
     , (1023538,   8,         90) /* Mass */
     , (1023538,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1023538,  16,          1) /* ItemUseable - No */
     , (1023538,  18,         32) /* UiEffects - Fire */
     , (1023538,  19,         20) /* Value */
     , (1023538,  44,          1) /* Damage */
     , (1023538,  45,         16) /* DamageType - Fire */
     , (1023538,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (1023538,  47,          1) /* AttackType - Punch */
     , (1023538,  48,         45) /* WeaponSkill - LightWeapons */
     , (1023538,  49,         20) /* WeaponTime */
     , (1023538,  51,          1) /* CombatUse - Melee */
     , (1023538,  53,        101) /* PlacementPosition - Resting */
     , (1023538,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1023538, 150,        103) /* HookPlacement - Hook */
     , (1023538, 151,          2) /* HookType - Wall */
     , (1023538, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1023538,  11, True ) /* IgnoreCollisions */
     , (1023538,  13, True ) /* Ethereal */
     , (1023538,  14, True ) /* GravityStatus */
     , (1023538,  19, True ) /* Attackable */
     , (1023538,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1023538,  21, 0.5199999809265137) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1023538,   1, 'Basalt Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1023538,   1,   33557335) /* Setup */
     , (1023538,   3,  536870932) /* SoundTable */
     , (1023538,   8,  100674097) /* Icon */
     , (1023538,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:17:19.7547634-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
