DELETE FROM `weenie` WHERE `class_Id` = 4200137;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200137, '4200137-katarhamudspyreal', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200137,   1,          1) /* ItemType - MeleeWeapon */
     , (4200137,   3,          8) /* PaletteTemplate - Green */
     , (4200137,   5,        120) /* EncumbranceVal */
     , (4200137,   8,         80) /* Mass */
     , (4200137,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (4200137,  16,          1) /* ItemUseable - No */
     , (4200137,  18,          1) /* UiEffects - Magical */
     , (4200137,  19,         20) /* Value */
     , (4200137,  44,          1) /* Damage */
     , (4200137,  45,         64) /* DamageType - Slash, Pierce */
     , (4200137,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (4200137,  47,          1) /* AttackType - Punch */
     , (4200137,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (4200137,  49,         15) /* WeaponTime */
     , (4200137,  51,          1) /* CombatUse - Melee */
     , (4200137,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200137, 150,        103) /* HookPlacement - Hook */
     , (4200137, 151,          2) /* HookType - Wall */
     , (4200137, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200137,  22, True ) /* Inscribable */
     , (4200137,  23, True ) /* DestroyOnSell */
     , (4200137,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200137,   5,    -0.1) /* ManaRate */
     , (4200137,  21,       0) /* WeaponLength */
     , (4200137,  22,     0.6) /* DamageVariance */
     , (4200137,  26,       0) /* MaximumVelocity */
     , (4200137,  29,       1) /* WeaponDefense */
     , (4200137,  39,    1.25) /* DefaultScale */
     , (4200137,  62,       1) /* WeaponOffense */
     , (4200137,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200137,   1, 'Hamud''s Pyreal Katar') /* Name */
     , (4200137,  15, 'An exquisitely crafted katar with a damascened blade.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200137,   1, 0x02000735) /* Setup */
     , (4200137,   3, 0x20000014) /* SoundTable */
     , (4200137,   6, 0x04000BEF) /* PaletteBase */
     , (4200137,   7, 0x1000020A) /* ClothingBase */
     , (4200137,   8, 0x06001600) /* Icon */
     , (4200137,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200137,  36, 0x0E000014) /* MutateFilter */;
