DELETE FROM `weenie` WHERE `class_Id` = 480519;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480519, 'axebattlerustedpk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480519,   1,          1) /* ItemType - MeleeWeapon */
     , (480519,   3,         25) /* PaletteTemplate - DarkCopperMetal */
     , (480519,   5,        0) /* EncumbranceVal */
     , (480519,   8,        320) /* Mass */
     , (480519,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480519,  16,          1) /* ItemUseable - No */
     , (480519,  19,          20) /* Value */
     , (480519,  44,          0) /* Damage */
     , (480519,  45,          1) /* DamageType - Slash */
     , (480519,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480519,  47,          4) /* AttackType - Slash */
     , (480519,  48,          1) /* WeaponSkill - Axe */
     , (480519,  49,         60) /* WeaponTime */
     , (480519,  51,          1) /* CombatUse - Melee */
     , (480519,  92,        100) /* Structure */
     , (480519,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480519, 131,         59) /* MaterialType - Copper */
     , (480519, 150,        103) /* HookPlacement - Hook */
     , (480519, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480519,  22, True ) /* Inscribable */
     , (480519,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480519,  21,    1) /* WeaponLength */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480519,   1, 'Rusted Battle Axe') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480519,   1, 0x02000125) /* Setup */
     , (480519,   3, 0x20000014) /* SoundTable */
     , (480519,   6, 0x04000BEF) /* PaletteBase */
     , (480519,   7, 0x10000143) /* ClothingBase */
     , (480519,   8, 0x06002AAE) /* Icon */
     , (480519,  22, 0x3400002B) /* PhysicsEffectTable */;
