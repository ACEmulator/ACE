DELETE FROM `weenie` WHERE `class_Id` = 450794;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450794, 'ace450794-ravensabrapk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450794,   1,          1) /* ItemType - MeleeWeapon */
     , (450794,   3,         29) /* PaletteTemplate - DarkRedMetal */
     , (450794,   5,        350) /* EncumbranceVal */
     , (450794,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450794,  16,          1) /* ItemUseable - No */
     , (450794,  18,         64) /* UiEffects - Lightning */
     , (450794,  19,        20) /* Value */
     , (450794,  44,        0) /* Damage */
     , (450794,  45,         64) /* DamageType - Electric */
     , (450794,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450794,  47,        6) /* AttackType - Thrust, Slash, DoubleSlash, TripleSlash, DoubleThrust, TripleThrust */
     , (450794,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450794,  51,          1) /* CombatUse - Melee */
     , (450794,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450794, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450794,  11, True ) /* IgnoreCollisions */
     , (450794,  13, True ) /* Ethereal */
     , (450794,  14, True ) /* GravityStatus */
     , (450794,  19, True ) /* Attackable */
     , (450794,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450794,  12,       0) /* Shade */
     , (450794,  21,       1) /* WeaponLength */
     , (450794,  22,    0.25) /* DamageVariance */
     , (450794,  26,       0) /* MaximumVelocity */
     , (450794,  29,       1) /* WeaponDefense */
     , (450794,  62,       1) /* WeaponOffense */
     , (450794,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450794,   1, 'Raven Lightning Sabra') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450794,   1, 0x020013A4) /* Setup */
     , (450794,   3, 0x20000014) /* SoundTable */
     , (450794,   6, 0x04001A25) /* PaletteBase */
     , (450794,   7, 0x10000601) /* ClothingBase */
     , (450794,   8, 0x06005C5E) /* Icon */
     , (450794,  22, 0x3400002B) /* PhysicsEffectTable */;
