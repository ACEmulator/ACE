DELETE FROM `weenie` WHERE `class_Id` = 480566;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480566, 'ace480566-bloodlettingdaggerpk', 6, '2021-11-08 06:01:47') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480566,   1,          1) /* ItemType - MeleeWeapon */
     , (480566,   3,         14) /* PaletteTemplate - Red */
     , (480566,   5,        0) /* EncumbranceVal */
     , (480566,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480566,  16,          1) /* ItemUseable - No */
     , (480566,  19,        20) /* Value */
     , (480566,  44,         0) /* Damage */
     , (480566,  45,          3) /* DamageType - Slash, Pierce */
     , (480566,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480566,  47,        166) /* AttackType - Thrust, Slash, DoubleSlash, DoubleThrust */
     , (480566,  48,         45) /* WeaponSkill - LightWeapons */
     , (480566,  49,         20) /* WeaponTime */
     , (480566,  51,          1) /* CombatUse - Melee */
     , (480566,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480566, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480566,  11, True ) /* IgnoreCollisions */
     , (480566,  13, True ) /* Ethereal */
     , (480566,  14, True ) /* GravityStatus */
     , (480566,  19, True ) /* Attackable */
     , (480566,  22, True ) /* Inscribable */
     , (480566,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480566,  12,     0.5) /* Shade */
     , (480566,  21,     0.4) /* WeaponLength */
     , (480566,  22,     0.6) /* DamageVariance */
     , (480566,  76,     0.5) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480566,   1, 'Bloodletting Dagger') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480566,   1, 0x02001839) /* Setup */
     , (480566,   3, 0x20000014) /* SoundTable */
     , (480566,   6, 0x040017CC) /* PaletteBase */
     , (480566,   7, 0x10000538) /* ClothingBase */
     , (480566,   8, 0x06003151) /* Icon */
     , (480566,  22, 0x3400002B) /* PhysicsEffectTable */;



