DELETE FROM `weenie` WHERE `class_Id` = 450792;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450792, 'ace450792-ravensabrapk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450792,   1,          1) /* ItemType - MeleeWeapon */
     , (450792,   3,         29) /* PaletteTemplate - DarkRedMetal */
     , (450792,   5,        0) /* EncumbranceVal */
     , (450792,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450792,  16,          1) /* ItemUseable - No */
     , (450792,  18,        128) /* UiEffects - Frost */
     , (450792,  19,        20) /* Value */
     , (450792,  44,        0) /* Damage */
     , (450792,  45,          8) /* DamageType - Cold */
     , (450792,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450792,  47,        6) /* AttackType - Thrust, Slash, DoubleSlash, TripleSlash, DoubleThrust, TripleThrust */
     , (450792,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450792,  51,          1) /* CombatUse - Melee */
     , (450792,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450792, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450792,  11, True ) /* IgnoreCollisions */
     , (450792,  13, True ) /* Ethereal */
     , (450792,  14, True ) /* GravityStatus */
     , (450792,  19, True ) /* Attackable */
     , (450792,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450792,  12,       0) /* Shade */
     , (450792,  21,       1) /* WeaponLength */
     , (450792,  22,    0.25) /* DamageVariance */
     , (450792,  26,       0) /* MaximumVelocity */
     , (450792,  29,       1) /* WeaponDefense */
     , (450792,  62,       1) /* WeaponOffense */
     , (450792,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450792,   1, 'Raven Frost Sabra') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450792,   1, 0x020013A2) /* Setup */
     , (450792,   3, 0x20000014) /* SoundTable */
     , (450792,   6, 0x04001A25) /* PaletteBase */
     , (450792,   7, 0x10000601) /* ClothingBase */
     , (450792,   8, 0x06005C5E) /* Icon */
     , (450792,  22, 0x3400002B) /* PhysicsEffectTable */;
