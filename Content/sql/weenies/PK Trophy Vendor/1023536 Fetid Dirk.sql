DELETE FROM `weenie` WHERE `class_Id` = 1023536;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1023536, 'ace1023536-fetiddirk', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1023536,   1,          1) /* ItemType - MeleeWeapon */
     , (1023536,   3,          8) /* PaletteTemplate - Green */
     , (1023536,   5,          1) /* EncumbranceVal */
     , (1023536,   8,        360) /* Mass */
     , (1023536,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1023536,  16,          1) /* ItemUseable - No */
     , (1023536,  18,        256) /* UiEffects - Acid */
     , (1023536,  19,         20) /* Value */
     , (1023536,  44,          1) /* Damage */
     , (1023536,  45,         32) /* DamageType - Acid */
     , (1023536,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1023536,  47,          6) /* AttackType - Thrust, Slash */
     , (1023536,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1023536,  49,         35) /* WeaponTime */
     , (1023536,  51,          1) /* CombatUse - Melee */
     , (1023536,  53,        101) /* PlacementPosition - Resting */
     , (1023536,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1023536, 150,        103) /* HookPlacement - Hook */
     , (1023536, 151,          2) /* HookType - Wall */
     , (1023536, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1023536,  11, True ) /* IgnoreCollisions */
     , (1023536,  13, True ) /* Ethereal */
     , (1023536,  14, True ) /* GravityStatus */
     , (1023536,  19, True ) /* Attackable */
     , (1023536,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1023536,  21, 0.6200000047683716) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1023536,   1, 'Fetid Dirk') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1023536,   1,   33558184) /* Setup */
     , (1023536,   3,  536870932) /* SoundTable */
     , (1023536,   6,   67114156) /* PaletteBase */
     , (1023536,   8,  100674031) /* Icon */
     , (1023536,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:38:33.2320328-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
