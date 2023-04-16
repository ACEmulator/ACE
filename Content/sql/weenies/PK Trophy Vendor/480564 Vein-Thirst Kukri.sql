DELETE FROM `weenie` WHERE `class_Id` = 480564;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480564, 'kukriliazk4pk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480564,   1,          1) /* ItemType - MeleeWeapon */
     , (480564,   3,         14) /* PaletteTemplate - Red */
     , (480564,   5,        0) /* EncumbranceVal */
     , (480564,   8,         90) /* Mass */
     , (480564,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480564,  16,          1) /* ItemUseable - No */
     , (480564,  18,          1) /* UiEffects - Magical */
     , (480564,  19,       20) /* Value */
     , (480564,  44,         0) /* Damage */
     , (480564,  45,          3) /* DamageType - Slash, Pierce */
     , (480564,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480564,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (480564,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480564,  49,         10) /* WeaponTime */
     , (480564,  51,          1) /* CombatUse - Melee */
     , (480564,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480564, 150,        103) /* HookPlacement - Hook */
     , (480564, 151,          2) /* HookType - Wall */
     , (480564, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480564,  22, True ) /* Inscribable */
     , (480564,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480564,   5,   -0.05) /* ManaRate */
     , (480564,  21,     0.4) /* WeaponLength */
     , (480564,  22,     0.5) /* DamageVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480564,   1, 'Vein-Thirst Kukri') /* Name */
     , (480564,  15, 'This Falatacot weapon appears to be an ornamental or sacrificial one. Curved and sharp, the blade seems alive in some way.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480564,   1, 0x0200106A) /* Setup */
     , (480564,   3, 0x20000014) /* SoundTable */
     , (480564,   6, 0x040017CC) /* PaletteBase */
     , (480564,   7, 0x10000538) /* ClothingBase */
     , (480564,   8, 0x06003151) /* Icon */
     , (480564,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480564,  36, 0x0E000014) /* MutateFilter */;

