DELETE FROM `weenie` WHERE `class_Id` = 450797;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450797, 'ace450797-shadowbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450797,   1,          1) /* ItemType - MeleeWeapon */
     , (450797,   5,        0) /* EncumbranceVal */
     , (450797,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450797,  16,          1) /* ItemUseable - No */
     , (450797,  18,          1) /* UiEffects - Magical */
     , (450797,  19,        20) /* Value */
     , (450797,  44,         0) /* Damage */
     , (450797,  45,          3) /* DamageType - Slash */
     , (450797,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450797,  47,        6) /* AttackType - DoubleSlash, DoubleThrust */
     , (450797,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450797,  51,          1) /* CombatUse - Melee */
     , (450797,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450797, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450797,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450797,  22,    0.45) /* DamageVariance */
     , (450797,  26,       0) /* MaximumVelocity */
	 , (450797,  39,     1.2) /* DefaultScale */
     , (450797,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450797,   1, 'Shadow Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450797,   1, 0x0200155E) /* Setup */
     , (450797,   3, 0x20000014) /* SoundTable */
     , (450797,   8, 0x06006408) /* Icon */
     , (450797,  22, 0x3400002B) /* PhysicsEffectTable */;
