DELETE FROM `weenie` WHERE `class_Id` = 450556;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450556, 'ace450556-redrunesilveranclawtailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450556,   1,          1) /* ItemType - MeleeWeapon */
     , (450556,   5,        0) /* EncumbranceVal */
     , (450556,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450556,  16,          1) /* ItemUseable - No */
     , (450556,  19,      20) /* Value */
     , (450556,  44,         0) /* Damage */
     , (450556,  45,          1) /* DamageType - Slash */
     , (450556,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450556,  47,          1) /* AttackType - Punch */
     , (450556,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450556,  49,         20) /* WeaponTime */
     , (450556,  51,          1) /* CombatUse - Melee */
     , (450556,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450556, 151,          2) /* HookType - Wall */
     , (450556, 263,          1) /* ResistanceModifierType - Slash */
     , (450556, 353,          1) /* WeaponType - Unarmed */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450556,  19, True ) /* Attackable */
     , (450556,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450556,   5,   -0.05) /* ManaRate */
     , (450556,  22,    0.55) /* DamageVariance */
     , (450556,  29,     1.2) /* WeaponDefense */
     , (450556,  39,     0.8) /* DefaultScale */
     , (450556,  62,    1.15) /* WeaponOffense */
     , (450556, 157,     1.5) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450556,   1, 'Red Rune Silveran Claw') /* Name */
     , (450556,  15, 'A claw crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450556,   1, 0x02001588) /* Setup */
     , (450556,   3, 0x20000014) /* SoundTable */
     , (450556,   8, 0x0600641E) /* Icon */
     , (450556,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450556,  50, 0x06006413) /* IconOverlay */;

