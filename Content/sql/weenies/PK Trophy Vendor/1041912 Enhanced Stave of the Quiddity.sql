DELETE FROM `weenie` WHERE `class_Id` = 1041912;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1041912, 'ace1041912-enhancedstaveofthequiddity', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1041912,   1,          1) /* ItemType - MeleeWeapon */
     , (1041912,   5,          1) /* EncumbranceVal */
     , (1041912,   8,         90) /* Mass */
     , (1041912,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1041912,  16,          1) /* ItemUseable - No */
     , (1041912,  18,          1) /* UiEffects - Magical */
     , (1041912,  19,         20) /* Value */
     , (1041912,  44,          1) /* Damage */
     , (1041912,  45,          4) /* DamageType - Bludgeon */
     , (1041912,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1041912,  47,          6) /* AttackType - Thrust, Slash */
     , (1041912,  48,         45) /* WeaponSkill - LightWeapons */
     , (1041912,  49,         30) /* WeaponTime */
     , (1041912,  51,          1) /* CombatUse - Melee */
     , (1041912,  53,        101) /* PlacementPosition - Resting */
     , (1041912,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1041912, 150,        103) /* HookPlacement - Hook */
     , (1041912, 151,          2) /* HookType - Wall */
     , (1041912, 353,          7) /* WeaponType - Staff */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1041912,  11, True ) /* IgnoreCollisions */
     , (1041912,  13, True ) /* Ethereal */
     , (1041912,  14, True ) /* GravityStatus */
     , (1041912,  15, True ) /* LightsStatus */
     , (1041912,  19, True ) /* Attackable */
     , (1041912,  22, True ) /* Inscribable */
     , (1041912,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1041912,  21, 1.3300000429153442) /* WeaponLength */
     , (1041912,  39, 0.6700000166893005) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1041912,   1, 'Enhanced Stave of the Quiddity') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1041912,   1,   33557107) /* Setup */
     , (1041912,   3,  536870932) /* SoundTable */
     , (1041912,   8,  100671699) /* Icon */
     , (1041912,  22,  872415275) /* PhysicsEffectTable */
     , (1041912,  36,  234881044) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T18:14:39.4118852-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
