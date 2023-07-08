DELETE FROM `weenie` WHERE `class_Id` = 480523;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480523, 'tachirustedpk', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480523,   1,          1) /* ItemType - MeleeWeapon */
     , (480523,   3,         14) /* PaletteTemplate - Red */
     , (480523,   5,        0) /* EncumbranceVal */
     , (480523,   8,        180) /* Mass */
     , (480523,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480523,  16,          1) /* ItemUseable - No */
     , (480523,  19,          20) /* Value */
     , (480523,  44,          0) /* Damage */
     , (480523,  45,          3) /* DamageType - Slash, Pierce */
     , (480523,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480523,  47,          6) /* AttackType - Thrust, Slash */
     , (480523,  48,         11) /* WeaponSkill - Sword */
     , (480523,  49,         35) /* WeaponTime */
     , (480523,  51,          1) /* CombatUse - Melee */
     , (480523,  92,        100) /* Structure */
     , (480523,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480523, 105,          3) /* ItemWorkmanship */
     , (480523, 131,         64) /* MaterialType - Steel */
     , (480523, 150,        103) /* HookPlacement - Hook */
     , (480523, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480523,  22, True ) /* Inscribable */
     , (480523,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480523,  21,     1.1) /* WeaponLength */
     , (480523,  22,       1) /* DamageVariance */
     , (480523,  29,       1) /* WeaponDefense */
     , (480523,  62,       1) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480523,   1, 'Rusted Tachi') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480523,   1, 0x02000136) /* Setup */
     , (480523,   3, 0x20000014) /* SoundTable */
     , (480523,   6, 0x04000BEF) /* PaletteBase */
     , (480523,   7, 0x1000014C) /* ClothingBase */
     , (480523,   8, 0x06002AB4) /* Icon */
     , (480523,  22, 0x3400002B) /* PhysicsEffectTable */;
