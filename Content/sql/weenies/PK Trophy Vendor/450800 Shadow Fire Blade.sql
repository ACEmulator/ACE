DELETE FROM `weenie` WHERE `class_Id` = 450800;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450800, 'ace450800-shadowbladepk', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450800,   1,          1) /* ItemType - MeleeWeapon */
     , (450800,   5,        0) /* EncumbranceVal */
     , (450800,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450800,  16,          1) /* ItemUseable - No */
     , (450800,  18,          1) /* UiEffects - Magical */
     , (450800,  19,        20) /* Value */
     , (450800,  44,         0) /* Damage */
     , (450800,  45,         16) /* DamageType - Fire */
     , (450800,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450800,  47,        6) /* AttackType - DoubleSlash, DoubleThrust */
     , (450800,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450800,  51,          1) /* CombatUse - Melee */
     , (450800,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450800, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450800,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450800,  22,    0.45) /* DamageVariance */
     , (450800,  26,       0) /* MaximumVelocity */
     , (450800,  39,     1.2) /* DefaultScale */
     , (450800,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450800,   1, 'Shadow Fire Blade') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450800,   1, 0x02001561) /* Setup */
     , (450800,   3, 0x20000014) /* SoundTable */
     , (450800,   8, 0x06006408) /* Icon */
     , (450800,  22, 0x3400002B) /* PhysicsEffectTable */;
