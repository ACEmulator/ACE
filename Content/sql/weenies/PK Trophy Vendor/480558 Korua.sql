DELETE FROM `weenie` WHERE `class_Id` = 480558;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480558, 'cestuskoruapk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480558,   1,          1) /* ItemType - MeleeWeapon */
     , (480558,   5,        0) /* EncumbranceVal */
     , (480558,   8,         90) /* Mass */
     , (480558,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480558,  16,          1) /* ItemUseable - No */
     , (480558,  19,      20) /* Value */
     , (480558,  44,         20) /* Damage */
     , (480558,  45,          4) /* DamageType - Bludgeon */
     , (480558,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (480558,  47,          1) /* AttackType - Punch */
     , (480558,  48,         45) /* WeaponSkill - LightWeapons */
     , (480558,  49,         20) /* WeaponTime */
     , (480558,  51,          1) /* CombatUse - Melee */
     , (480558,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480558, 150,        103) /* HookPlacement - Hook */
     , (480558, 151,          2) /* HookType - Wall */
     , (480558, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480558,  22, True ) /* Inscribable */
     , (480558,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480558,  21,    0.52) /* WeaponLength */
     , (480558,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480558,   1, 'Korua') /* Name */
     , (480558,  16, 'A beautifully carved cestus. Fuse this with a triple totem to create one of Palenqual''s living weapons.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480558,   1, 0x02001088) /* Setup */
     , (480558,   3, 0x20000014) /* SoundTable */
     , (480558,   6, 0x04001178) /* PaletteBase */
     , (480558,   7, 0x1000031C) /* ClothingBase */
     , (480558,   8, 0x0600330E) /* Icon */
     , (480558,  22, 0x3400002B) /* PhysicsEffectTable */;
