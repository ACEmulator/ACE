DELETE FROM `weenie` WHERE `class_Id` = 450795;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450795, 'ace450795-ravensabrapk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450795,   1,          1) /* ItemType - MeleeWeapon */
     , (450795,   3,         29) /* PaletteTemplate - DarkRedMetal */
     , (450795,   5,        350) /* EncumbranceVal */
     , (450795,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450795,  16,          1) /* ItemUseable - No */
     , (450795,  18,        256) /* UiEffects - Acid */
     , (450795,  19,        20) /* Value */
     , (450795,  44,        0) /* Damage */
     , (450795,  45,         32) /* DamageType - Acid */
     , (450795,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450795,  47,        6) /* AttackType - Thrust, Slash, DoubleSlash, TripleSlash, DoubleThrust, TripleThrust */
     , (450795,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450795,  51,          1) /* CombatUse - Melee */
     , (450795,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450795, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450795,  11, True ) /* IgnoreCollisions */
     , (450795,  13, True ) /* Ethereal */
     , (450795,  14, True ) /* GravityStatus */
     , (450795,  19, True ) /* Attackable */
     , (450795,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450795,  12,       0) /* Shade */
     , (450795,  21,       1) /* WeaponLength */
     , (450795,  22,    0.25) /* DamageVariance */
     , (450795,  26,       0) /* MaximumVelocity */
     , (450795,  29,       1) /* WeaponDefense */
     , (450795,  62,       1) /* WeaponOffense */
     , (450795,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450795,   1, 'Raven Acid Sabra') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450795,   1, 0x020013A3) /* Setup */
     , (450795,   3, 0x20000014) /* SoundTable */
     , (450795,   6, 0x04001A25) /* PaletteBase */
     , (450795,   7, 0x10000601) /* ClothingBase */
     , (450795,   8, 0x06005C5E) /* Icon */
     , (450795,  22, 0x3400002B) /* PhysicsEffectTable */;
