DELETE FROM `weenie` WHERE `class_Id` = 450791;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450791, 'katarhamudspyrealpk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450791,   1,          1) /* ItemType - MeleeWeapon */
     , (450791,   3,          8) /* PaletteTemplate - Green */
     , (450791,   5,        0) /* EncumbranceVal */
     , (450791,   8,         80) /* Mass */
     , (450791,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450791,  16,          1) /* ItemUseable - No */
     , (450791,  18,          1) /* UiEffects - Magical */
     , (450791,  19,       20) /* Value */
     , (450791,  44,         0) /* Damage */
     , (450791,  45,          3) /* DamageType - Slash, Pierce */
     , (450791,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450791,  47,          1) /* AttackType - Punch */
     , (450791,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450791,  49,         15) /* WeaponTime */
     , (450791,  51,          1) /* CombatUse - Melee */
     , (450791,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450791, 150,        103) /* HookPlacement - Hook */
     , (450791, 151,          2) /* HookType - Wall */
     , (450791, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450791,  22, True ) /* Inscribable */
     , (450791,  23, True ) /* DestroyOnSell */
     , (450791,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450791,   5,    -0.1) /* ManaRate */
     , (450791,  21,       0) /* WeaponLength */
     , (450791,  22,     0.6) /* DamageVariance */
     , (450791,  26,       0) /* MaximumVelocity */
     , (450791,  29,       1) /* WeaponDefense */
     , (450791,  39,    1.25) /* DefaultScale */
     , (450791,  62,       1) /* WeaponOffense */
     , (450791,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450791,   1, 'Hamud''s Pyreal Katar') /* Name */
     , (450791,  15, 'An exquisitely crafted katar with a damascened blade.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450791,   1, 0x02000735) /* Setup */
     , (450791,   3, 0x20000014) /* SoundTable */
     , (450791,   6, 0x04000BEF) /* PaletteBase */
     , (450791,   7, 0x1000020A) /* ClothingBase */
     , (450791,   8, 0x06001600) /* Icon */
     , (450791,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450791,  36, 0x0E000014) /* MutateFilter */;


