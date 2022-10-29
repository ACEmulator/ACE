DELETE FROM `weenie` WHERE `class_Id` = 1011431;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1011431, 'ace1011431-audetaungastaiahaofthemountains', 6, '2021-11-20 00:19:18') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1011431,   1,          1) /* ItemType - MeleeWeapon */
     , (1011431,   5,          1) /* EncumbranceVal */
     , (1011431,   8,        140) /* Mass */
     , (1011431,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (1011431,  16,          1) /* ItemUseable - No */
     , (1011431,  18,          1) /* UiEffects - Magical */
     , (1011431,  19,         20) /* Value */
     , (1011431,  44,          1) /* Damage */
     , (1011431,  45,          2) /* DamageType - Pierce */
     , (1011431,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (1011431,  47,          2) /* AttackType - Thrust */
     , (1011431,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (1011431,  49,         30) /* WeaponTime */
     , (1011431,  51,          1) /* CombatUse - Melee */
     , (1011431,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1011431, 150,        103) /* HookPlacement - Hook */
     , (1011431, 151,          2) /* HookType - Wall */
     , (1011431, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1011431,  22, True ) /* Inscribable */
     , (1011431,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1011431,  21,     1.5) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1011431,   1, 'Audetaunga''s Taiaha of the Mountains') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1011431,   1,   33557236) /* Setup */
     , (1011431,   3,  536870932) /* SoundTable */
     , (1011431,   6,   67113336) /* PaletteBase */
     , (1011431,   7,  268436248) /* ClothingBase */
     , (1011431,   8,  100672087) /* Icon */
     , (1011431,  22,  872415275) /* PhysicsEffectTable */
     , (1011431,  36,  234881044) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-06T10:01:09.8901525-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
