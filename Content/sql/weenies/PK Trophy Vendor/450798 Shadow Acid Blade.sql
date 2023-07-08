DELETE FROM `weenie` WHERE `class_Id` = 450798;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450798, 'ace450798-shadowbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450798,   1,          1) /* ItemType - MeleeWeapon */
     , (450798,   5,        0) /* EncumbranceVal */
     , (450798,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450798,  16,          1) /* ItemUseable - No */
     , (450798,  18,          1) /* UiEffects - Magical */
     , (450798,  19,        20) /* Value */
     , (450798,  44,         0) /* Damage */
     , (450798,  45,         32) /* DamageType - Acid */
     , (450798,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450798,  47,        6) /* AttackType - DoubleSlash, DoubleThrust */
     , (450798,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450798,  51,          1) /* CombatUse - Melee */
     , (450798,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450798, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450798,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450798,  22,    0.45) /* DamageVariance */
     , (450798,  26,       0) /* MaximumVelocity */
     , (450798,  39,     1.2) /* DefaultScale */
     , (450798,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450798,   1, 'Shadow Acid Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450798,   1, 0x0200155F) /* Setup */
     , (450798,   3, 0x20000014) /* SoundTable */
     , (450798,   8, 0x06006408) /* Icon */
     , (450798,  22, 0x3400002B) /* PhysicsEffectTable */;
