DELETE FROM `weenie` WHERE `class_Id` = 480524;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480524, 'tridentrustedpk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480524,   1,          1) /* ItemType - MeleeWeapon */
     , (480524,   3,         14) /* PaletteTemplate - Red */
     , (480524,   5,        0) /* EncumbranceVal */
     , (480524,   8,        150) /* Mass */
     , (480524,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480524,  16,          1) /* ItemUseable - No */
     , (480524,  19,          20) /* Value */
     , (480524,  44,          0) /* Damage */
     , (480524,  45,          2) /* DamageType - Pierce */
     , (480524,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480524,  47,          2) /* AttackType - Thrust */
     , (480524,  48,          9) /* WeaponSkill - Spear */
     , (480524,  49,         55) /* WeaponTime */
     , (480524,  51,          1) /* CombatUse - Melee */
     , (480524,  92,        100) /* Structure */
     , (480524,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480524, 105,          3) /* ItemWorkmanship */
     , (480524, 131,         63) /* MaterialType - Silver */
     , (480524, 150,        103) /* HookPlacement - Hook */
     , (480524, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480524,  22, True ) /* Inscribable */
     , (480524,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480524,  21,     1.3) /* WeaponLength */
     , (480524,  22,       1) /* DamageVariance */
     , (480524,  29,       1) /* WeaponDefense */
     , (480524,  39,     1.2) /* DefaultScale */
     , (480524,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480524,   1, 'Rusted Trident') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480524,   1, 0x020008A1) /* Setup */
     , (480524,   3, 0x20000014) /* SoundTable */
     , (480524,   6, 0x04000BEF) /* PaletteBase */
     , (480524,   7, 0x1000022D) /* ClothingBase */
     , (480524,   8, 0x06002AB5) /* Icon */
     , (480524,  22, 0x3400002B) /* PhysicsEffectTable */;
