DELETE FROM `weenie` WHERE `class_Id` = 480573;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480573, 'swordukirapk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480573,   1,          1) /* ItemType - MeleeWeapon */
     , (480573,   5,        0) /* EncumbranceVal */
     , (480573,   8,        180) /* Mass */
     , (480573,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480573,  16,          1) /* ItemUseable - No */
     , (480573,  19,      20) /* Value */
     , (480573,  33,          1) /* Bonded - Bonded */
     , (480573,  44,         0) /* Damage */
     , (480573,  45,          3) /* DamageType - Slash, Pierce */
     , (480573,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480573,  47,          6) /* AttackType - Thrust, Slash */
     , (480573,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (480573,  49,         60) /* WeaponTime */
     , (480573,  51,          1) /* CombatUse - Melee */
     , (480573,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480573, 150,        103) /* HookPlacement - Hook */
     , (480573, 151,          2) /* HookType - Wall */
     , (480573, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480573,  22, True ) /* Inscribable */
     , (480573,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480573,  21,    0.95) /* WeaponLength */
     , (480573,  22,     0.5) /* DamageVariance */
     , (480573,  39,     1.1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480573,   1, 'Ukira') /* Name */
     , (480573,  16, 'A beautifully carved sword. Fuse this with a triple totem to create one of Palenqual''s living weapons.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480573,   1, 0x0200108A) /* Setup */
     , (480573,   3, 0x20000014) /* SoundTable */
     , (480573,   6, 0x04001178) /* PaletteBase */
     , (480573,   7, 0x1000031C) /* ClothingBase */
     , (480573,   8, 0x060032FD) /* Icon */
     , (480573,  22, 0x3400002B) /* PhysicsEffectTable */;
