DELETE FROM `weenie` WHERE `class_Id` = 480520;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480520, 'dirkrustedpk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480520,   1,          1) /* ItemType - MeleeWeapon */
     , (480520,   3,         25) /* PaletteTemplate - DarkCopperMetal */
     , (480520,   5,        0) /* EncumbranceVal */
     , (480520,   8,        200) /* Mass */
     , (480520,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480520,  16,          1) /* ItemUseable - No */
     , (480520,  19,          20) /* Value */
     , (480520,  44,          0) /* Damage */
     , (480520,  45,          3) /* DamageType - Slash, Pierce */
     , (480520,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480520,  47,          6) /* AttackType - Thrust, Slash */
     , (480520,  48,          4) /* WeaponSkill - Dagger */
     , (480520,  49,         40) /* WeaponTime */
     , (480520,  51,          1) /* CombatUse - Melee */
     , (480520,  92,        100) /* Structure */
     , (480520,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480520, 131,         62) /* MaterialType - Pyreal */
     , (480520, 150,        103) /* HookPlacement - Hook */
     , (480520, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480520,  22, True ) /* Inscribable */
     , (480520,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480520,  21,     0.4) /* WeaponLength */
     , (480520,  22,       1) /* DamageVariance */
     , (480520,  29,       1) /* WeaponDefense */
     , (480520,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480520,   1, 'Rusted Dirk') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480520,   1, 0x02000E49) /* Setup */
     , (480520,   3, 0x20000014) /* SoundTable */
     , (480520,   6, 0x04000BEF) /* PaletteBase */
     , (480520,   7, 0x10000415) /* ClothingBase */
     , (480520,   8, 0x06002AAF) /* Icon */
     , (480520,  22, 0x3400002B) /* PhysicsEffectTable */;
