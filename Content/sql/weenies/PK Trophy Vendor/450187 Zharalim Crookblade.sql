DELETE FROM `weenie` WHERE `class_Id` = 450187;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450187, 'daggerrarezharalimcrookbladetailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450187,   1,          1) /* ItemType - MeleeWeapon */
     , (450187,   5,        0) /* EncumbranceVal */
     , (450187,   8,         90) /* Mass */
     , (450187,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450187,  16,          1) /* ItemUseable - No */
     , (450187,  19,      20) /* Value */
     , (450187,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450187,  45,          3) /* DamageType - Slash, Pierce */
     , (450187,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450187,  47,          6) /* AttackType - Thrust, Slash */
     , (450187,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450187,  49,         50) /* WeaponTime */
     , (450187,  51,          1) /* CombatUse - Melee */
     , (450187,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450187, 151,          2) /* HookType - Wall */
     , (450187, 353,          6) /* WeaponType - Dagger */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450187,  11, True ) /* IgnoreCollisions */
     , (450187,  13, True ) /* Ethereal */
     , (450187,  14, True ) /* GravityStatus */
     , (450187,  19, True ) /* Attackable */
     , (450187,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450187,   5,  -0.033) /* ManaRate */
     , (450187,  21,       0) /* WeaponLength */
     , (450187,  22,   0.192) /* DamageVariance */
     , (450187,  26,       0) /* MaximumVelocity */
     , (450187,  29,    1.18) /* WeaponDefense */
     , (450187,  39,     1.1) /* DefaultScale */
     , (450187,  62,    1.18) /* WeaponOffense */
     , (450187,  63,       1) /* DamageMod */
     , (450187, 147,    0.33) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450187,   1, 'Zharalim Crookblade') /* Name */
     , (450187,  16, 'This is a sacred blade of the Zharalim, carried only by Masters of the order. Its razor sharp blade is designed to create a wider, more deadly wound than an ordinary straight blade. It is said that Rafik ibn Jaraz, a legendary Master of the Zharalim, slew six Viamontian royal guards in six steps, wielding this blade and using his "Wind and Smoke" technique.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450187,   1, 0x0200134F) /* Setup */
     , (450187,   3, 0x20000014) /* SoundTable */
     , (450187,   6, 0x04000BEF) /* PaletteBase */
     , (450187,   7, 0x10000860) /* ClothingBase */
     , (450187,   8, 0x06005B8D) /* Icon */
     , (450187,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450187,  36, 0x0E000012) /* MutateFilter */
     , (450187,  46, 0x38000032) /* TsysMutationFilter */
     , (450187,  52, 0x06005B0C) /* IconUnderlay */;
