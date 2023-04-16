DELETE FROM `weenie` WHERE `class_Id` = 450801;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450801, 'ace450801-shadowbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450801,   1,          1) /* ItemType - MeleeWeapon */
     , (450801,   5,        350) /* EncumbranceVal */
     , (450801,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450801,  16,          1) /* ItemUseable - No */
     , (450801,  18,          1) /* UiEffects - Magical */
     , (450801,  19,        20) /* Value */
     , (450801,  44,         0) /* Damage */
     , (450801,  45,          8) /* DamageType - Cold */
     , (450801,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450801,  47,        6) /* AttackType - DoubleSlash, DoubleThrust */
     , (450801,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450801,  51,          1) /* CombatUse - Melee */
     , (450801,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450801, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450801,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450801,  22,    0.45) /* DamageVariance */
     , (450801,  26,       0) /* MaximumVelocity */
     , (450801,  39,     1.2) /* DefaultScale */
     , (450801,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450801,   1, 'Shadow Frost Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450801,   1, 0x02001562) /* Setup */
     , (450801,   3, 0x20000014) /* SoundTable */
     , (450801,   8, 0x06006408) /* Icon */
     , (450801,  22, 0x3400002B) /* PhysicsEffectTable */;
