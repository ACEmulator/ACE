DELETE FROM `weenie` WHERE `class_Id` = 480522;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480522, 'nekoderustedpk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480522,   1,          1) /* ItemType - MeleeWeapon */
     , (480522,   3,         14) /* PaletteTemplate - Red */
     , (480522,   5,        0) /* EncumbranceVal */
     , (480522,   8,         90) /* Mass */
     , (480522,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480522,  16,          1) /* ItemUseable - No */
     , (480522,  19,          20) /* Value */
     , (480522,  44,          0) /* Damage */
     , (480522,  45,          1) /* DamageType - Slash */
     , (480522,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (480522,  47,          1) /* AttackType - Punch */
     , (480522,  48,         13) /* WeaponSkill - UnarmedCombat */
     , (480522,  49,         20) /* WeaponTime */
     , (480522,  51,          1) /* CombatUse - Melee */
     , (480522,  92,        100) /* Structure */
     , (480522,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480522, 105,          3) /* ItemWorkmanship */
     , (480522, 131,         58) /* MaterialType - Bronze */
     , (480522, 150,        103) /* HookPlacement - Hook */
     , (480522, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480522,  22, True ) /* Inscribable */
     , (480522,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480522,  21,    0.52) /* WeaponLength */
     , (480522,  22,       1) /* DamageVariance */
     , (480522,  29,    1.05) /* WeaponDefense */
     , (480522,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480522,   1, 'Rusted Nekode') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480522,   1, 0x0200061C) /* Setup */
     , (480522,   3, 0x20000014) /* SoundTable */
     , (480522,   6, 0x04000BEF) /* PaletteBase */
     , (480522,   7, 0x10000174) /* ClothingBase */
     , (480522,   8, 0x06002AB0) /* Icon */
     , (480522,  22, 0x3400002B) /* PhysicsEffectTable */;
