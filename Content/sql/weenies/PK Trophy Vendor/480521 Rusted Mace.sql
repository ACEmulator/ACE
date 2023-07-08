DELETE FROM `weenie` WHERE `class_Id` = 480521;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480521, 'macerustedpk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480521,   1,          1) /* ItemType - MeleeWeapon */
     , (480521,   3,         14) /* PaletteTemplate - Red */
     , (480521,   5,        0) /* EncumbranceVal */
     , (480521,   8,        450) /* Mass */
     , (480521,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480521,  16,          1) /* ItemUseable - No */
     , (480521,  19,          20) /* Value */
     , (480521,  44,          0) /* Damage */
     , (480521,  45,          4) /* DamageType - Bludgeon */
     , (480521,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480521,  47,          4) /* AttackType - Slash */
     , (480521,  48,          5) /* WeaponSkill - Mace */
     , (480521,  49,         40) /* WeaponTime */
     , (480521,  51,          1) /* CombatUse - Melee */
     , (480521,  92,        100) /* Structure */
     , (480521,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480521, 105,          3) /* ItemWorkmanship */
     , (480521, 131,         61) /* MaterialType - Iron */
     , (480521, 150,        103) /* HookPlacement - Hook */
     , (480521, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480521,  22, True ) /* Inscribable */
     , (480521,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480521,  21,    0.62) /* WeaponLength */
     , (480521,  22,       1) /* DamageVariance */
     , (480521,  29,       1) /* WeaponDefense */
     , (480521,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480521,   1, 'Rusted Mace') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480521,   1, 0x0200013A) /* Setup */
     , (480521,   3, 0x20000014) /* SoundTable */
     , (480521,   6, 0x04000BEF) /* PaletteBase */
     , (480521,   7, 0x10000150) /* ClothingBase */
     , (480521,   8, 0x06002AB1) /* Icon */
     , (480521,  22, 0x3400002B) /* PhysicsEffectTable */;
