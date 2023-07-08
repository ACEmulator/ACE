DELETE FROM `weenie` WHERE `class_Id` = 450796;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450796, 'ace450796-ravensabrapk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450796,   1,          1) /* ItemType - MeleeWeapon */
     , (450796,   3,         29) /* PaletteTemplate - DarkRedMetal */
     , (450796,   5,        0) /* EncumbranceVal */
     , (450796,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450796,  16,          1) /* ItemUseable - No */
     , (450796,  18,       1024) /* UiEffects - Slashing */
     , (450796,  19,        20) /* Value */
     , (450796,  44,        0) /* Damage */
     , (450796,  45,          3) /* DamageType - Slash, Pierce */
     , (450796,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450796,  47,        6) /* AttackType - Thrust, Slash, DoubleSlash, TripleSlash, DoubleThrust, TripleThrust */
     , (450796,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450796,  51,          1) /* CombatUse - Melee */
     , (450796,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450796, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450796,  11, True ) /* IgnoreCollisions */
     , (450796,  13, True ) /* Ethereal */
     , (450796,  14, True ) /* GravityStatus */
     , (450796,  19, True ) /* Attackable */
     , (450796,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450796,  12,       0) /* Shade */
     , (450796,  21,       1) /* WeaponLength */
     , (450796,  22,    0.25) /* DamageVariance */
     , (450796,  26,       0) /* MaximumVelocity */
     , (450796,  29,       1) /* WeaponDefense */
     , (450796,  62,       1) /* WeaponOffense */
     , (450796,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450796,   1, 'Raven Sabra') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450796,   1, 0x02001319) /* Setup */
     , (450796,   3, 0x20000014) /* SoundTable */
     , (450796,   6, 0x04001A25) /* PaletteBase */
     , (450796,   7, 0x10000601) /* ClothingBase */
     , (450796,   8, 0x06005C5E) /* Icon */
     , (450796,  22, 0x3400002B) /* PhysicsEffectTable */;
