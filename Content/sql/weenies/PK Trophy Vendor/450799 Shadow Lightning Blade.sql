DELETE FROM `weenie` WHERE `class_Id` = 450799;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450799, 'ace450799-shadowbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450799,   1,          1) /* ItemType - MeleeWeapon */
     , (450799,   5,        0) /* EncumbranceVal */
     , (450799,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450799,  16,          1) /* ItemUseable - No */
     , (450799,  18,          1) /* UiEffects - Magical */
     , (450799,  19,        20) /* Value */
     , (450799,  44,         0) /* Damage */
     , (450799,  45,         64) /* DamageType - Electric */
     , (450799,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450799,  47,        6) /* AttackType - DoubleSlash, DoubleThrust */
     , (450799,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450799,  51,          1) /* CombatUse - Melee */
     , (450799,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450799, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450799,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450799,  22,    0.45) /* DamageVariance */
     , (450799,  26,       0) /* MaximumVelocity */
     , (450799,  39,     1.2) /* DefaultScale */
     , (450799,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450799,   1, 'Shadow Lightning Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450799,   1, 0x02001560) /* Setup */
     , (450799,   3, 0x20000014) /* SoundTable */
     , (450799,   8, 0x06006408) /* Icon */
     , (450799,  22, 0x3400002B) /* PhysicsEffectTable */;
