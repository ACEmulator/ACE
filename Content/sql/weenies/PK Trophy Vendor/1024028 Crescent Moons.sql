DELETE FROM `weenie` WHERE `class_Id` = 1024028;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1024028, 'ace1024028-crescentmoons', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1024028,   1,          1) /* ItemType - MeleeWeapon */
     , (1024028,   5,          1) /* EncumbranceVal */
     , (1024028,   8,         90) /* Mass */
     , (1024028,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1024028,  16,          1) /* ItemUseable - No */
     , (1024028,  18,          1) /* UiEffects - Magical */
     , (1024028,  19,         20) /* Value */
     , (1024028,  44,          1) /* Damage */
     , (1024028,  45,          8) /* DamageType - Cold */
     , (1024028,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (1024028,  47,          1) /* AttackType - Punch */
     , (1024028,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (1024028,  49,         20) /* WeaponTime */
     , (1024028,  51,          1) /* CombatUse - Melee */
     , (1024028,  53,        101) /* PlacementPosition - Resting */
     , (1024028,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1024028, 150,        103) /* HookPlacement - Hook */
     , (1024028, 151,          2) /* HookType - Wall */
     , (1024028, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1024028,  11, True ) /* IgnoreCollisions */
     , (1024028,  13, True ) /* Ethereal */
     , (1024028,  14, True ) /* GravityStatus */
     , (1024028,  15, True ) /* LightsStatus */
     , (1024028,  19, True ) /* Attackable */
     , (1024028,  22, True ) /* Inscribable */
     , (1024028,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1024028,  21, 0.5199999809265137) /* WeaponLength */
     , (1024028,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1024028,   1, 'Crescent Moons') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1024028,   1,   33558267) /* Setup */
     , (1024028,   3,  536870932) /* SoundTable */
     , (1024028,   8,  100674150) /* Icon */
     , (1024028,  22,  872415275) /* PhysicsEffectTable */
     , (1024028,  36,  234881044) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T19:44:47.4277178-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
