DELETE FROM `weenie` WHERE `class_Id` = 450793;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450793, 'ace450793-ravensabrapk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450793,   1,          1) /* ItemType - MeleeWeapon */
     , (450793,   3,         29) /* PaletteTemplate - DarkRedMetal */
     , (450793,   5,        350) /* EncumbranceVal */
     , (450793,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450793,  16,          1) /* ItemUseable - No */
     , (450793,  18,         32) /* UiEffects - Fire */
     , (450793,  19,        20) /* Value */
     , (450793,  44,        0) /* Damage */
     , (450793,  45,         16) /* DamageType - Fire */
     , (450793,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450793,  47,        486) /* AttackType - Thrust, Slash, DoubleSlash, TripleSlash, DoubleThrust, TripleThrust */
     , (450793,  48,         6) /* WeaponSkill - FinesseWeapons */
     , (450793,  51,          1) /* CombatUse - Melee */
     , (450793,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450793, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450793,  11, True ) /* IgnoreCollisions */
     , (450793,  13, True ) /* Ethereal */
     , (450793,  14, True ) /* GravityStatus */
     , (450793,  19, True ) /* Attackable */
     , (450793,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450793,  12,       0) /* Shade */
     , (450793,  21,       1) /* WeaponLength */
     , (450793,  22,    0.25) /* DamageVariance */
     , (450793,  26,       0) /* MaximumVelocity */
     , (450793,  29,       1) /* WeaponDefense */
     , (450793,  62,       1) /* WeaponOffense */
     , (450793,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450793,   1, 'Raven Fire Sabra') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450793,   1, 0x020013A1) /* Setup */
     , (450793,   3, 0x20000014) /* SoundTable */
     , (450793,   6, 0x04001A25) /* PaletteBase */
     , (450793,   7, 0x10000601) /* ClothingBase */
     , (450793,   8, 0x06005C5E) /* Icon */
     , (450793,  22, 0x3400002B) /* PhysicsEffectTable */;
